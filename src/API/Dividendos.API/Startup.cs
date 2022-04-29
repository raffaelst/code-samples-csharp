using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using Dividendos.CrossCutting.Mapper;
using Dividendos.API.Middlewares;
using Dividendos.CrossCutting.Config.Model;
using Dividendos.CrossCutting.Identity.Models;
using Dividendos.CrossCutting.IoC;
using Dividendos.Repository.Context;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using K.ApiKeyPolicy;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Amazon.S3;
using Amazon.Runtime;
using Dividendos.AWS;

namespace Dividendos.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Ativando o uso de cache via Redis
            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = Configuration.GetConnectionString("RedisConnection");
            });

            services.AddControllers();

            //Singleton Configuration
            services.AddSingleton(_ => Configuration);
            string connectionString = Configuration.GetConnectionString("DefaultConnection");

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                        //.AllowCredentials();
                    });
            });

            var awsOption = Configuration.GetAWSOptions();
            awsOption.Credentials = new BasicAWSCredentials(Configuration["AWS:AccessKey"], Configuration["AWS:SecretKey"]);
            services.AddDefaultAWSOptions(awsOption);
            services.AddAWSService<IAmazonS3>();

            SetupResolvers.Setup();

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

            var tokenConfigurations = new TokenConfigurations();
            new ConfigureFromConfigurationOptions<TokenConfigurations>(
                Configuration.GetSection("TokenConfigurations"))
                    .Configure(tokenConfigurations);
            services.AddSingleton(tokenConfigurations);

            var symetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfigurations.Key));

            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = "Bearer";
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(bearerOptions =>
            {
                var paramsValidation = bearerOptions.TokenValidationParameters;
                paramsValidation.IssuerSigningKey = symetricKey;
                paramsValidation.ValidAudience = tokenConfigurations.Audience;
                paramsValidation.ValidIssuer = tokenConfigurations.Issuer;

                // Valida a assinatura de um token recebido
                paramsValidation.ValidateIssuerSigningKey = true;

                // Verifica se um token recebido ainda é válido
                paramsValidation.ValidateLifetime = true;

                // Tempo de tolerância para a expiração de um token (utilizado
                // caso haja problemas de sincronismo de horário entre diferentes
                // computadores envolvidos no processo de comunicação)
                paramsValidation.ClockSkew = TimeSpan.Zero;
            });

            services.AddAutoMapper(typeof(MappingProfile));

            services.AddKLogger(Configuration);

            services.AddKMailSender(Configuration);

            services.AddImportB3(Configuration);
            services.AddImportInvestidorB3(Configuration);
            services.AddRDStationIntegration(Configuration);
            services.AddKSocialLogin(Configuration);
            services.AddHurstIntegration(Configuration);

            // Ativa o uso do token como forma de autorizar o acesso
            // a recursos deste projeto
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });

            services.AddTransient<IAuthorizationHandler, ApiKeyRequirementHandler>();
            services.AddAuthorization(authConfig =>
            {
                authConfig.AddPolicy("XApiKey",
                    policyBuilder => policyBuilder
                        .AddRequirements(new ApiKeyRequirement()));
            });

            //Add framework services.
            services.AddControllers()
             .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            }
            );

            services.AddApiVersioning(p =>
            {
                p.DefaultApiVersion = new ApiVersion(1, 0);
                p.ReportApiVersions = true;
                p.AssumeDefaultVersionWhenUnspecified = true;
            });

            services.AddVersionedApiExplorer(p =>
            {
                p.GroupNameFormat = "'v'VVV";
                p.SubstituteApiVersionInUrl = true;
            });

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(options =>
            {

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In =  ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                options.AddSecurityDefinition("X-API-KEY", new OpenApiSecurityScheme
                {
                    Description = "API Key Authentication",
                    Name = "X-API-KEY",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Dividendos API",
                    Description = "Integration API of dividendos.me project",
                    TermsOfService = new Uri("http://dividendos.me"),
                    Contact = new OpenApiContact
                    {
                        Name = "dividendos.me",
                        Email = "support@dividendos.me",
                        Url = new Uri("http://dividendos.me")
                    }
                });


                options.SwaggerDoc("v2", new OpenApiInfo
                {
                    Version = "v2",
                    Title = "Dividendos API",
                    Description = "Integration API of dividendos.me project",
                    TermsOfService = new Uri("http://dividendos.me"),
                    Contact = new OpenApiContact
                    {
                        Name = "dividendos.me",
                        Email = "support@dividendos.me",
                        Url = new Uri("http://dividendos.me")
                    }
                });

                options.SwaggerDoc("v3", new OpenApiInfo
                {
                    Version = "v3",
                    Title = "Dividendos API",
                    Description = "Integration API of dividendos.me project",
                    TermsOfService = new Uri("http://dividendos.me"),
                    Contact = new OpenApiContact
                    {
                        Name = "dividendos.me",
                        Email = "support@dividendos.me",
                        Url = new Uri("http://dividendos.me")
                    }
                });

                options.SwaggerDoc("v4", new OpenApiInfo
                {
                    Version = "v4",
                    Title = "Dividendos API",
                    Description = "Integration API of dividendos.me project",
                    TermsOfService = new Uri("http://dividendos.me"),
                    Contact = new OpenApiContact
                    {
                        Name = "dividendos.me",
                        Email = "support@dividendos.me",
                        Url = new Uri("http://dividendos.me")
                    }
                });

                options.SwaggerDoc("v5", new OpenApiInfo
                {
                    Version = "v5",
                    Title = "Dividendos API",
                    Description = "Integration API of dividendos.me project",
                    TermsOfService = new Uri("http://dividendos.me"),
                    Contact = new OpenApiContact
                    {
                        Name = "dividendos.me",
                        Email = "support@dividendos.me",
                        Url = new Uri("http://dividendos.me")
                    }
                });

                options.SwaggerDoc("v6", new OpenApiInfo
                {
                    Version = "v6",
                    Title = "Dividendos API",
                    Description = "Integration API of dividendos.me project",
                    TermsOfService = new Uri("http://dividendos.me"),
                    Contact = new OpenApiContact
                    {
                        Name = "dividendos.me",
                        Email = "support@dividendos.me",
                        Url = new Uri("http://dividendos.me")
                    }
                });

                options.SwaggerDoc("v7", new OpenApiInfo
                {
                    Version = "v7",
                    Title = "Dividendos API",
                    Description = "Integration API of dividendos.me project",
                    TermsOfService = new Uri("http://dividendos.me"),
                    Contact = new OpenApiContact
                    {
                        Name = "dividendos.me",
                        Email = "support@dividendos.me",
                        Url = new Uri("http://dividendos.me")
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);

            });

            services.AddSwaggerGenNewtonsoftSupport();

            //IoC            
            IoCConfig.Config(services, connectionString);

            //Parameters Config
            this.GetParameters(services);

            services.AddAuthentication(
                    options =>
                    {
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    }
                );

            services.AddOptions();


            int accessFailureCount = 0;
            int.TryParse(Configuration.GetValue<string>("Parameters:AccessFailureCount"), out accessFailureCount);

            int defaultLockoutTimeSpan = 0;
            int.TryParse(Configuration.GetValue<string>("Parameters:DefaultLockoutTimeSpan"), out defaultLockoutTimeSpan);

            services.Configure<IdentityOptions>(options =>
            {
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(defaultLockoutTimeSpan);
                options.Lockout.MaxFailedAccessAttempts = accessFailureCount;
                options.Lockout.AllowedForNewUsers = true;
            });

#if !DEBUG
            services.AddDataProtection().UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration()
            {
                EncryptionAlgorithm = EncryptionAlgorithm.AES_256_GCM,
                ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
            });
#endif
            var defaultApp = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("dividendos-7350e-firebase-adminsdk-bj43y-09e5e2c3e2.json"),
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {
            app.UseMiddleware<ExceptionMiddleware>();

            #region [ CORS ]

            app.UseCors("AllowAll");

            #endregion

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();


            bool enableSwagger = false;
            bool.TryParse(Configuration.GetValue<string>("Parameters:EnableSwagger"), out enableSwagger);

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            if (enableSwagger)
            {
                app.UseSwaggerUI(c =>
                {
                    c.RoutePrefix = string.Empty;

                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        c.SwaggerEndpoint(
                        $"/swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());
                    }

                    c.DocExpansion(DocExpansion.List);
                });
            }
            app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void GetParameters(IServiceCollection services)
        {
            IConfigurationSection parameters = Configuration.GetSection("Parameters");
            services.Configure<GlobalSystemConfig>(parameters.GetSection("GlobalSystemConfig"));
            services.Configure<AWSConfig>(Configuration.GetSection("AWS"));
            services.Configure<RequestLocalizationOptions>(options =>
            {
                string languageSystem = "pt-BR";

                options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture(languageSystem);
                options.SupportedCultures = new List<CultureInfo> { new CultureInfo(languageSystem) };
                options.RequestCultureProviders.Clear();
            });
        }
    }
}
