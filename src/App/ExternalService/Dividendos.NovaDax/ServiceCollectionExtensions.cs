using System;
using System.Collections.Generic;
using System.Text;
using Dividendos.NovaDax;
using Dividendos.NovaDax.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIntegrationNovaDax(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient(typeof(INovaDaxHelper), typeof(NovaDaxHelper));
            return serviceCollection;
        }
    }
}
