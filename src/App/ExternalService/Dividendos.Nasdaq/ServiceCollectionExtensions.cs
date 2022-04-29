using System;
using System.Collections.Generic;
using System.Text;
using Dividendos.Nasdaq;
using Dividendos.Nasdaq.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNasdaq(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient(typeof(INasdaqHelper), typeof(NasdaqHelper));
            return serviceCollection;
        }
    }
}
