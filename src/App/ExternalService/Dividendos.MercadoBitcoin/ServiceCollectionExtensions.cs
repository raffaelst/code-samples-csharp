using System;
using System.Collections.Generic;
using System.Text;
using Dividendos.MercadoBitcoin;
using Dividendos.MercadoBitcoin.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIntegrationMercadoBitCoin(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient(typeof(IAccountInfo), typeof(AccountInfoClient));
            serviceCollection.AddTransient(typeof(ICurrenciesQuotations), typeof(CurrenciesQuotations));
            return serviceCollection;
        }
    }
}
