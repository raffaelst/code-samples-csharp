using System;
using System.Collections.Generic;
using System.Text;
using Dividendos.Binance;
using Dividendos.Binance.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIntegrationBinance(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient(typeof(IBinanceHelper), typeof(BinanceHelper));
            return serviceCollection;
        }
    }
}
