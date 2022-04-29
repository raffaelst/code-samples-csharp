using System;
using System.Collections.Generic;
using System.Text;
using Dividendos.CoinMarketCap;
using Dividendos.CoinMarketCap.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIntegrationCoinMarketCap(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient(typeof(ICoinMarketCapHelper), typeof(CoinMarketCapHelper));
            return serviceCollection;
        }
    }
}
