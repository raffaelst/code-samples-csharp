using System;
using AutoMapper;
using Dividendos.Application;
using Dividendos.Application.Interface;
using Dividendos.CrossCutting.Identity.Models;
using Dividendos.CrossCutting.IoC;
using Dividendos.Entity.Enum;
using Dividendos.Repository.Context;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;
using Dividendos.CrossCutting.Mapper;
using Dividendos.API.Model.Response.BrokerIntegration;
using Dividendos.API.Model.Response.Common;
using Amazon.Runtime;
using Amazon.S3;
using Dividendos.AWS;
using OpenQA.Selenium.Chrome;

namespace ClientCluster
{
    public class Program
    {
        public static IConfigurationRoot Configuration { get; set; }

        private static ServiceProvider Configure()
        {
            var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            string connectionString = Configuration.GetConnectionString("DefaultConnection");

            IServiceCollection services = new ServiceCollection();

            services.AddSingleton(_ => Configuration);
            services.AddKLogger(Configuration);
            services.AddKMailSender(Configuration);
            services.AddImportB3(Configuration);
            services.AddImportInvestidorB3(Configuration);
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddRDStationIntegration(Configuration);

            var awsOption = Configuration.GetAWSOptions();
            awsOption.Credentials = new BasicAWSCredentials(Configuration["AWS:AccessKey"], Configuration["AWS:SecretKey"]);
            services.AddDefaultAWSOptions(awsOption);
            services.AddAWSService<IAmazonS3>();

            services.Configure<AWSConfig>(Configuration.GetSection("AWS"));

            SetupResolvers.Setup();

            services.AddLogging();

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

            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = Configuration.GetConnectionString("RedisConnection");
            });

            var serviceProvider = services.BuildServiceProvider();

            var defaultApp = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("dividendos-7350e-firebase-adminsdk-bj43y-09e5e2c3e2.json"),
            });
            return serviceProvider;
        }

        public static async System.Threading.Tasks.Task Main(string[] args)
        {
            Console.InputEncoding = System.Text.Encoding.Default;
            Console.OutputEncoding = System.Text.Encoding.Default;
            ServiceProvider serviceProvider = Configure();
            ScrapyClusterEnum scrapyClusterEnum = ScrapyClusterEnum.RunParallelCei;
            var scrapySchedulerApp = serviceProvider.GetService<IScrapySchedulerApp>();
            var portfolioApp = serviceProvider.GetService<IPortfolioApp>();
            var stockApp = serviceProvider.GetService<IStockApp>();
            var stockSplitApp = serviceProvider.GetService<IStockSplitApp>();


            if (args != null && args.Length > 0)
            {
                int param;

                if (int.TryParse(args[0], out param))
                {
                    scrapyClusterEnum = (ScrapyClusterEnum)param;
                }
            }

            var dividendCalendarApp = serviceProvider.GetService<IDividendCalendarApp>();
            var brokerIntegrationApp = serviceProvider.GetService<IBrokerIntegrationApp>();
            var userApp = serviceProvider.GetService<IUserApp>();
            #region Settings

            string agentName = "NotSet";
            double defaulTimeout = 3600;
            string resetTime = "1";
            int amountItems = 10;
            int daysDeleteTasks = 4;

            if (Configuration.GetSection("ScrapyClusterConfig") != null)
            {
                if (Configuration.GetSection("ScrapyClusterConfig").GetSection("AgentName").Value != null)
                {
                    agentName = Configuration.GetSection("ScrapyClusterConfig").GetSection("AgentName").Value;
                }

                if (Configuration.GetSection("ScrapyClusterConfig").GetSection("TimeoutSeconds").Value != null)
                {
                    defaulTimeout = Convert.ToDouble(Configuration.GetSection("ScrapyClusterConfig").GetSection("TimeoutSeconds").Value);
                }

                if (Configuration.GetSection("ScrapyClusterConfig").GetSection("AmountItems").Value != null)
                {
                    amountItems = Convert.ToInt32(Configuration.GetSection("ScrapyClusterConfig").GetSection("AmountItems").Value);
                }

                if (Configuration.GetSection("ScrapyClusterConfig").GetSection("ResetTime").Value != null)
                {
                    resetTime = Configuration.GetSection("ScrapyClusterConfig").GetSection("ResetTime").Value;
                }

                if (Configuration.GetSection("ScrapyClusterConfig").GetSection("DeleteDays").Value != null)
                {
                    daysDeleteTasks = Convert.ToInt32(Configuration.GetSection("ScrapyClusterConfig").GetSection("DeleteDays").Value);
                }
            }

            #endregion

            #region Tests
            
            try
            {
                //dividendCalendarApp.GetAllAndUpdateFromSIFIIs();
                //userApp.SendEmailWithDailyStatistics();
                //dividendCalendarApp.GetAllAndUpdateFromSIFIIs();
                //dividendCalendarApp.GetAllAndUpdateFromSIStocks();
                //dividendCalendarApp.GetAllAndUpdateFromSIBDRs();
                //portfolioApp.ImportAllLogos();
                //portfolioApp.ImportStocksAndTesouroDiretoCei("97255190006", "Lando78ze@", "47965ca9-3ebb-4833-ac78-662e4b06eebe");
                //portfolioApp.ImportStocksAndTesouroDiretoCei("31171035896", "senhacei@052021", "83c2051c-6e9d-4e24-810e-50f9ed4f34e4");
                //portfolioApp.ImportStocksAndTesouroDiretoCei("3dasdas11710355896", "senha5cei@052021", "83c2051c-6e9d-4e24-810e-50f9ed4f34e4");
                //portfolioApp.ImportStocksAndTesouroDiretoCei("10607979763", "Ol91533255%", "c2ea53c7-fdb4-4afa-9e9c-8c54b1f9476c");

                //portfolioApp.RunDelayedCeiImport();

                //scrapySchedulerApp.RunCeiDirect("03127235127", "Queir@zb46", "e9be9667-8a07-4d9b-87cc-8f289dc62ac5");
                //scrapySchedulerApp.RunCeiDirect("70248010930", "PUNHEIS*980", "732a3508-4521-40e6-a8a1-773fe1e47b9b");
                //scrapySchedulerApp.RunCeiDirect("01870147103", "Pauloh@36", "520bae5b-635b-44b5-aceb-8140e88f8c2c");
                //scrapySchedulerApp.RunCeiDirect("31171035896", "senhacei@112021", "83c2051c-6e9d-4e24-810e-50f9ed4f34e4");
                //scrapySchedulerApp.RunCeiDirect("31171035896", "senhacei@112021", "d88abb8d-2a2f-4cea-b3aa-dabaa748ffeb");



                //scrapySchedulerApp.RunNewCeiDirect("31171035896", "83c2051c-6e9d-4e24-810e-50f9ed4f34e4");

                //scrapySchedulerApp.RunNewCeiDirect("24142378805", "e23ff3eb-377e-45b0-ab68-a5f436614150");

                //scrapySchedulerApp.EnqueueNewB3Traders();

                //var purchaseApp = serviceProvider.GetService<IPurchaseApp>();

                //purchaseApp.SendPushPartner();

                //purchaseApp.GetEmailByIdentifier(@"C:\Users\trademap\Documents\Toro", "Toro-20-07-2021.txt");

                //purchaseApp.SubscribePartnerFileV2(@"C:\Users\trademap\Documents\Toro", "Toro-20-07-2021.txt", true);

                //purchaseApp.SubscribePartnerFileNotDefined(@"C:\Users\trademap\Documents\Toro", "Toro-20-07-2021.txt", false);

                //purchaseApp.SubscribePartnerActivated(@"C:\Users\trademap\Documents\Toro", "Ativados-19-07-2021.txt");

                //purchaseApp.CheckPilantra(@"C:\Users\trademap\Documents\Toro", "Toro-20-07-2021.txt", true);

                //purchaseApp.SendPushPilantra(@"C:\Users\trademap\Documents\Toro", "Toro-20-07-2021.txt");

                //var dividendApp = serviceProvider.GetService<IDividendApp>();

                //dividendApp.RestorePastDividends(5, 1);

                //dividendApp.GetDividendYieldList(Guid.Parse("BFAED463-AB82-4E99-A89A-EA3444EB0C58"), null, null);

                //var stockApp = serviceProvider.GetService<IStockApp>();

                //await stockApp.SyncStockPriceUsingTradeMapAsync(1);
                //await stockApp.ImportUsStocks(2);
                //await stockApp.ImportStatusInvestCompanies(2);

                //portfolioApp.ImportFromPasfolio("andreluistc@gmail.com", "3d5e6fdc-868b-4228-89fc-253acaa346ea", "a788b3d5-68e3-4089-8fce-d960d92d73b8", true);

                //var brokerIntegrationApp = serviceProvider.GetService<IBrokerIntegrationApp>();

                //await brokerIntegrationApp.ImportFromXptAsync("9a3538c1-1863-46e4-be96-22948460dbbf", "2316783", "060708", "914276");

                //await brokerIntegrationApp.ImportFromXptAsync("83c2051c-6e9d-4e24-810e-50f9ed4f34e4", "3003357", "061126", "362165");

                //brokerIntegrationApp.ImportFromNuInvest("83cdef6b-6dcd-44c9-b12c-9c8442d9607e", "37791933888", "12Nibru@s29", "183532");


                //brokerIntegrationApp.ImportFromNuInvest("ed5bbebf-0b83-4ae9-81d6-2143c386d0e7", "503.365.358-05 ", "061019Jj@", "183532");

                //brokerIntegrationApp.ImportFromNuInvest("83c2051c-6e9d-4e24-810e-50f9ed4f34e4", "08070102608", "MEtron213412@", "183532");

                //brokerIntegrationApp.ImportFromNuInvest("83c2051c-6e9d-4e24-810e-50f9ed4f34e4", "raffaelst@gmail.com", "Dividendos@1", "677137");

                //brokerIntegrationApp.ImportFromNuInvest("4d97bdec-7398-4304-a3e5-08d4c70bd53a", "28070074817", "//%Liberdade2028", "635402");

                //brokerIntegrationApp.ImportFromRico("83c2051c-6e9d-4e24-810e-50f9ed4f34e4", "7723857", "936724", "776626");

                //brokerIntegrationApp.ImportFromRico("d822b1c1-b6bf-4ca0-ac40-7cc13b8e0d16", "cbneto02 ", "15052021", "570662");


                //brokerIntegrationApp.ImportFromRico("d02e28af-0ed8-42c0-9e8a-f73e2d17865f", "6742690 ", "12022311", "205943");

                //brokerIntegrationApp.ImportFromClear("dcee47a9-570c-4645-9a23-77fc8fb42730", "447.066.208-99", "20/09/1997", "030896");


                //brokerIntegrationApp.ImportFromClear("2b95014f-83df-4d08-a089-351008c7949c", "416.022.468-74", "18/11/1994", "195818");

                //brokerIntegrationApp.ImportFromClear("33c73421-251b-4fa8-b469-9670305e60c2", "138.174.228-90", "18/06/1974", "533486");

                //brokerIntegrationApp.ImportFromClear("18ae3ce6-4126-4d04-aef8-46151ef6931e", "041.672.671-28", "17/04/1993", "105010");

                //brokerIntegrationApp.ImportFromClear("cfdd8038-ec2e-4abb-9fe3-ee3754d982ce", "083.159.401-20", "15/01/2003", "150103");

                //brokerIntegrationApp.ImportFromClear("83c2051c-6e9d-4e24-810e-50f9ed4f34e4", "311.710.358-96", "11/11/1982", "061126");

                //brokerIntegrationApp.ImportFromNuInvest("b6c91b48-21a5-4727-8307-cd93ce040109", "05279005088", "Buildcraft245324!", "806498");

                //ResultResponseObject<AvenueAddResponse> avenueAddResponse = await brokerIntegrationApp.AuthenticateOnAvenue(new Dividendos.API.Model.Request.BrokerIntegration.AvenueAddRequest { Email = "andreluistc@gmail.com", Password = "M!Rd8cqV@XRzurp" });

                //await portfolioApp.ImportFromAvenue("andreluistc@gmail.com", "M!Rd8cqV@XRzurp", "800370", "83c2051c-6e9d-4e24-810e-50f9ed4f34e4", "", avenueAddResponse.Value.SessionId);

                // brokerIntegrationApp.AuthenticateOnToro(new Dividendos.API.Model.Request.BrokerIntegration.ToroAddRequest { IdUser = "83c2051c-6e9d-4e24-810e-50f9ed4f34e4", Email = "raffaelst@gmail.com", Password = "dividendos@123" });
                //brokerIntegrationApp.ImportFromToro("raffaelst@gmail.com", "dividendos@123", "490442", "83c2051c-6e9d-4e24-810e-50f9ed4f34e4");

                //brokerIntegrationApp.AuthenticateOnToro(new Dividendos.API.Model.Request.BrokerIntegration.ToroAddRequest { Email = "andreluistc@gmail.com", Password = "MEtron213412" });
                //brokerIntegrationApp.ImportFromToro("andreluistc@gmail.com", "MEtron213412", "72062", "83c2051c-6e9d-4e24-810e-50f9ed4f34e4");

                //brokerIntegrationApp.AuthenticateOnToro(new Dividendos.API.Model.Request.BrokerIntegration.ToroAddRequest { Email = "oruan413@gmail.com", Password = "ruh275517" });
                //brokerIntegrationApp.ImportFromToro("oruan413@gmail.com", "ruh275517", "14510", "6769fec1-515e-4676-926f-50d81524d36d");

                //stockSplitApp.GetStockSplits("8e743b09-b466-4e01-b3d1-bad3e325b0c4");
                //stockSplitApp.ImportStockSplit(2);


                //var holidayApp = serviceProvider.GetService<IHolidayApp>();

                //await holidayApp.ImportHolidays();


                //var companyApp = serviceProvider.GetService<ICompanyIndicatorsApp>();
                //companyApp.GetCompanyIndicators(Guid.Parse("80A6B3C4-D045-4C4D-8689-D6351547029A"));
                //companyApp.ImportCompanyIndicators();

                //brokerIntegrationApp.TesteInvestidorB3();

                //brokerIntegrationApp.SyncB3AssetsTrading();
                //brokerIntegrationApp.TestNewCei();



                //var cryptoCurrencyApp = serviceProvider.GetService<ICryptoCurrencyApp>();
                //cryptoCurrencyApp.SyncCryptoCurrencyPrice();

                //portfolioApp.ImportFromPasfolio("andreluistc@gmail.com", "3d5e6fdc-868b-4228-89fc-253acaa346ea", "83c2051c-6e9d-4e24-810e-50f9ed4f34e4", true);


                //var indicatorApp = serviceProvider.GetService<IIndicatorApp>();
                //indicatorApp.ImportInvestingIndicators();

                //var relevantFactApp = serviceProvider.GetService<IRelevantFactApp>();
                //relevantFactApp.ImportRelevantFacts();


                //dividendCalendarApp.GetAndUpdateFromSIStocks();


                var dividendApp = serviceProvider.GetService<IDividendApp>();
                //dividendApp.RestorePastDividends(27679, 1, DateTime.Now);

                dividendApp.RestorePastDividends(Guid.Parse("B713E5B6-D8A5-44E4-8F0F-5141267F3BD7"), null);


                //await scrapySchedulerApp.RunParallelNewCei(defaulTimeout * 1000, agentName, 20);

                //portfolioApp.Disable(Guid.Parse("743B0B05-D524-4577-908C-CCB17C0BDB29"//));

                //string uid = Guid.NewGuid().ToString("N");
            }
            catch (Exception ex)
            {

                throw;
            }


            #endregion

            switch (scrapyClusterEnum)
            {
                case ScrapyClusterEnum.RunParallelCei:
                    await scrapySchedulerApp.RunParallelCei(defaulTimeout, agentName, amountItems);
                    break;
                case ScrapyClusterEnum.ResetTasks:
                    scrapySchedulerApp.RenewBlockedTasks(resetTime);
                    break;
                case ScrapyClusterEnum.DeleteTasks:
                    scrapySchedulerApp.DeleteOldCompletedTasks(daysDeleteTasks);
                    break;
                case ScrapyClusterEnum.RunDelayedCei:
                    portfolioApp.RunDelayedCeiImport();
                    break;
                case ScrapyClusterEnum.RunScrapyAgent:
                    portfolioApp.RunScrapyAgent(agentName, amountItems);
                    break;
                case ScrapyClusterEnum.ImportCompanies:
                    #region Import Companies
                    try
                    {
                        stockApp.ImportUsStocks(1);
                    }
                    catch
                    {
                    }

                    try
                    {
                        stockApp.ImportUsStocks(2);
                    }
                    catch
                    {
                    }

                    try
                    {
                        stockApp.ImportUsStocks(3);
                    }
                    catch
                    {
                    }

                    try
                    {
                        stockApp.ImportStatusInvestCompanies(1);
                    }
                    catch
                    {
                    }

                    try
                    {
                        stockApp.ImportStatusInvestCompanies(2);
                    }
                    catch
                    {
                    }

                    try
                    {
                        stockApp.ImportStatusInvestCompanies(6);
                    }
                    catch
                    {
                    }

                    try
                    {
                        stockApp.ImportStatusInvestCompanies(12);
                    }
                    catch
                    {
                    }

                    try
                    {
                        stockApp.ImportStatusInvestCompanies(13);
                    }
                    catch
                    {
                    }

                    #endregion
                    break;
                case ScrapyClusterEnum.ImportSplit:
                    try
                    {
                        stockSplitApp.ImportStockSplit(1);
                        stockSplitApp.ImportStockSplit(2);
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    break;
                case ScrapyClusterEnum.RunParallelNewCei:
                    await scrapySchedulerApp.RunParallelNewCei(defaulTimeout * 2, agentName, amountItems);
                    break;
                case ScrapyClusterEnum.ResetTasksNewCei:
                    scrapySchedulerApp.RenewBlockedTasksNewCei(resetTime);
                    break;
                case ScrapyClusterEnum.DeleteTasksNewCei:
                    scrapySchedulerApp.DeleteOldCompletedTasksNewCei(daysDeleteTasks);
                    break;
                case ScrapyClusterEnum.EnqueueNewB3Traders:
                    scrapySchedulerApp.EnqueueNewB3Traders();
                    break;
                default:
                    break;
            }
        }
    }
}
