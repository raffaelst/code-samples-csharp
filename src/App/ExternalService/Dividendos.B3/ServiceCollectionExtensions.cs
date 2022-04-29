using System;
using System.Collections.Generic;
using System.Text;
using Dividendos.B3;
using Dividendos.B3.Config;
using Dividendos.B3.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddImportB3(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var importB3Config = new ImportB3Config();

            configuration.GetSection("ImportB3Config").Bind(importB3Config);

            serviceCollection.AddTransient<IImportB3Helper, ImportB3Helper>(service => new ImportB3Helper(importB3Config.WorkingDirectoryCei, importB3Config.WorkingDirectoryAnacondaActivate));

            return serviceCollection;
        }
    }
}
