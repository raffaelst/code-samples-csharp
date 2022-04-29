using Dividendos.Job;
using Dividendos.Application.Interface;
using Dividendos.CrossCutting.Config.Model;
using Dividendos.CrossCutting.IoC;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using AutoMapper;
using Dividendos.CrossCutting.Mapper;
using Dividendos.Repository.Context;
using Microsoft.EntityFrameworkCore;
using Dividendos.CrossCutting.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Hangfire.Storage;
using Dividendos.Job.Middlewares;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using System.Threading;
using System.Globalization;
using K.UnitOfWorkBase;
using K.Logger;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Hangfire.Dashboard;
using Hangfire.Mongo;
using MongoDB.Driver;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Amazon.Runtime;
using Amazon.S3;
using Dividendos.AWS;

namespace Dividendos.Job
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc();

            //Singleton Configuration
            services.AddSingleton(_ => Configuration);
            string connectionString = Configuration.GetConnectionString("DefaultConnection");

            services.AddKLogger(Configuration);

            services.AddKMailSender(Configuration);

            services.AddImportB3(Configuration);

            services.AddImportInvestidorB3(Configuration);

            //IoC            
            IoCConfig.Config(services, connectionString);

            // Ativando a utilização do ASP.NET Identity, a fim de
            // permitir a recuperação de seus objetos via injeção de
            // dependências            
            // Configurando o uso da classe de contexto para
            // acesso às tabelas do ASP.NET Identity Core
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Ativando a utilização do ASP.NET Identity, a fim de
            // permitir a recuperação de seus objetos via injeção de
            // dependências
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            var awsOption = Configuration.GetAWSOptions();
            awsOption.Credentials = new BasicAWSCredentials(Configuration["AWS:AccessKey"], Configuration["AWS:SecretKey"]);
            services.AddDefaultAWSOptions(awsOption);
            services.AddAWSService<IAmazonS3>();
            services.AddRDStationIntegration(Configuration);
            SetupResolvers.Setup();


            this.GetParameters(services);

			services.AddOptions();
            services.AddAutoMapper(typeof(MappingProfile));

#if !DEBUG
            services.AddDataProtection().UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration()
            {
                EncryptionAlgorithm = EncryptionAlgorithm.AES_256_GCM,
                ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
            });
#endif

            var mongoUrlBuilder = new MongoUrlBuilder(Configuration.GetConnectionString("JobsConnection"));
            var mongoClient = new MongoClient(mongoUrlBuilder.ToMongoUrl());

            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseMongoStorage(mongoClient, mongoUrlBuilder.DatabaseName, new MongoStorageOptions
                {
                    MigrationOptions = new MongoMigrationOptions
                    {
                        MigrationStrategy = new MigrateMongoMigrationStrategy(),
                        BackupStrategy = new CollectionMongoBackupStrategy()
                    },
                    Prefix = "hangfire.mongo",
                    CheckConnection = true
                }));

            // Add the processing server as IHostedService
            services.AddHangfireServer();

            var defaultApp = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("dividendos-7350e-firebase-adminsdk-bj43y-09e5e2c3e2.json"),
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IRecurringJobManager recurringJobManager,
            IOptions<JobConfig> jobConfig,
            ILogger logger)
        {
            GlobalJobFilters.Filters.Add(new ErrorReportingJobFilter(logger));

            app.UseStaticFiles();

            app.UseRouting();

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new HangFireAuthorizationFilter() },
                AppPath = jobConfig.Value.URLSite,
                DisplayStorageConnectionString = false,
                IsReadOnlyFunc = (DashboardContext context) => true
            });

            #region Configurações Job

            // Setar a quantidade de tentativas globalmente para 0
            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete });

            // Contexto separado para cada JOB
            GlobalConfiguration.Configuration.UseActivator(new HangfireJobActivator(app.ApplicationServices));

            var options = new BackgroundJobServerOptions
            {
                Queues = new[] { "syncstockprice", 
                    "autosyncportfolios", 
                    "canculateperformance", 
                    "importindicators", 
                    "sendpushdividendsnight", 
                    "sendpushdividendsmorning", 
                    "sendpushalertblockedcei", 
                    "callinactiveusers", 
                    "sendmaildailystatistics",
                    "specialistportfolios",
                    "dividendcalendargetall",
                    "dividendcalendar",
                    "dividendcalendarstock",
                    "dividendcalendarfii",
                    "dividendcalendarbdr",
                    "divcalendareuaiex",
                    "dividendcalendareua",
                    "dividendcalendareua0",
                    "dividendcalendareua1",
                    "dividendcalendareua2",
                    "dividendcalendareua3",
                    "dividendcalendareua4",
                    "dividendcalendareua5",
                    "dividendcalendareua6",
                    "dividendcalendareua7",
                    "dividendcalendareua8",
                    "dividendcalendareua9",
                    "dividendcalendareua10",
                    "dividendcalendareua11",
                    "dividendcalendareua12",
                    "dividendcalendareua13",
                    "stocksusa",
					"criptocurrencies",
                    "calltoaction",
                    "marketmovers",
                    "queuemanut",
                    "queuesynccei",
                    "followstock",
                    "notification",
                    "createdividends",
                    "datacomnotification",
                    "awesomevariations",
                    "stocksplits",
                    "sendpushdailystatistics",
                    "importrelevantfacts",
                    "relevantfact",
                    "importinvestingindicators",
                    "importompanyindicators",
                    "importompanybrst",
                    "importompanyfiist",
                    "importompanyetfst",
                    "importompanystockst",
                    "importompanyreitsst",
                    "importstocksplitbr",
                    "importstocksplitus",
                    "awesomevariationsalldaylong",
                }
            };

            app.UseHangfireServer(options);

            #endregion

            Scheduler scheduler = new Scheduler(recurringJobManager, logger, jobConfig);
        }

        private void GetParameters(IServiceCollection services)
        {
            IConfigurationSection parameters = Configuration.GetSection("Parameters");
            services.Configure<JobConfig>(parameters.GetSection("Job"));
            services.Configure<AWSConfig>(Configuration.GetSection("AWS"));
        }
    }
}