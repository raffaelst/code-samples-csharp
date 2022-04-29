using System;
using System.Collections.Generic;
using System.Text;
using Dividendos.CoinNext;
using Dividendos.CoinNext.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIntegrationCoinNext(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient(typeof(ICoinNextHelper), typeof(CoinNextHelper));
            return serviceCollection;
        }
    }
}
