using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Dividendos.RDStation;
using Dividendos.RDStation.Config;
using Dividendos.RDStation.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRDStationIntegration(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var rdStationConfig = new RDStationConfig();

            configuration.GetSection("RDStationConfig").Bind(rdStationConfig);

            HttpClient httpClient = new HttpClient();

            serviceCollection.AddTransient<IRDStationHelper, RDStationB3Helper>(service => new RDStationB3Helper(rdStationConfig.URLBase, rdStationConfig.ApiKey, httpClient));

            return serviceCollection;
        }
    }
}
