using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Dividendos.Hurst;
using Dividendos.Hurst.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHurstIntegration(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            HttpClient httpClient = new HttpClient();

            serviceCollection.AddTransient<IHurstHelper, HurstHelper>(service => new HurstHelper(httpClient));

            return serviceCollection;
        }
    }
}
