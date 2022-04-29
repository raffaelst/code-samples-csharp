using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Dividendos.InvestidorB3;
using Dividendos.InvestidorB3.Config;
using Dividendos.InvestidorB3.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddImportInvestidorB3(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var importInvestidorB3Config = new ImportInvestidorB3Config();

            configuration.GetSection("ImportInvestidorB3Config").Bind(importInvestidorB3Config);

            var handler = new HttpClientHandler();
            handler.UseCookies = false;

//#if DEBUG
//            handler.ClientCertificates.Add(new X509Certificate2("HomologInvestidorB3.p12", importInvestidorB3Config.PasswordCertificate));
//#endif
//#if !DEBUG
            handler.ClientCertificates.Add(new X509Certificate2("CertProdB3Investidor.p12", importInvestidorB3Config.PasswordCertificate));
//#endif

            HttpClient httpClient = new HttpClient(handler);
            httpClient.Timeout = TimeSpan.FromDays(1);
            

            serviceCollection.AddTransient<IImportInvestidorB3Helper, ImportInvestidorB3Helper>(service => new ImportInvestidorB3Helper(importInvestidorB3Config.URLBasePortalInvestidorB3,
                importInvestidorB3Config.UrlEndPointLogin,
                importInvestidorB3Config.ClientId,
                importInvestidorB3Config.ClientSecret,
                importInvestidorB3Config.Scope,
                importInvestidorB3Config.URLAuthB3,
                httpClient));

            return serviceCollection;
        }
    }
}
