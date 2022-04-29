using System;
using System.Collections.Generic;
using System.Text;
using Dividendos.Biscoint;
using Dividendos.Biscoint.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIntegrationBiscoint(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient(typeof(IBiscointHelper), typeof(BiscointHelper));
            return serviceCollection;
        }
    }
}
