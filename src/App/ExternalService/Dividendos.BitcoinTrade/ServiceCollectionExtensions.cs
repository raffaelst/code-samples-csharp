using System;
using System.Collections.Generic;
using System.Text;
using Dividendos.BitcoinTrade;
using Dividendos.BitcoinTrade.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIntegrationBitcoinTrade(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient(typeof(IBitcoinTradeHelper), typeof(BitcoinTradeHelper));
            return serviceCollection;
        }
    }
}
