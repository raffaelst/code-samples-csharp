using System;
using System.Collections.Generic;
using System.Text;
using Dividendos.Passfolio;
using Dividendos.Passfolio.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPassfolio(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient(typeof(IPassfolioHelper), typeof(PassfolioHelper));
            return serviceCollection;
        }
    }
}
