using System;
using System.Collections.Generic;
using System.Text;
using Dividendos.IexAPIsHelper;
using Dividendos.IexAPIsHelper.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIexAPIs(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient(typeof(IIexAPIsHelper), typeof(IexAPIsHelper));
            return serviceCollection;
        }
    }
}
