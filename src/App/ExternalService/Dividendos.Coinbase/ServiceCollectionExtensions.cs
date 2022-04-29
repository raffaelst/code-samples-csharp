using System;
using System.Collections.Generic;
using System.Text;
using Dividendos.Coinbase;
using Dividendos.Coinbase.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIntegrationCoinbase(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient(typeof(ICoinbaseHelper), typeof(CoinbaseHelper));
            return serviceCollection;
        }
    }
}
