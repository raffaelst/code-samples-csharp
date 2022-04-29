using System;
using System.Collections.Generic;
using System.Text;
using Dividendos.BitPreco;
using Dividendos.BitPreco.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIntegrationBitPreco(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient(typeof(IBitPrecoHelper), typeof(BitPrecoHelper));
            return serviceCollection;
        }
    }
}
