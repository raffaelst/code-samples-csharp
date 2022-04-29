using System;
using System.Collections.Generic;
using System.Text;
using Dividendos.BitcoinToYou;
using Dividendos.BitcoinToYou.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIntegrationBitcoinToYou(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient(typeof(IBitcoinToYouHelper), typeof(BitcoinToYouHelper));
            return serviceCollection;
        }
    }
}
