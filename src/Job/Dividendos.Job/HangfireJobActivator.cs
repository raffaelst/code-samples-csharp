using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dividendos.Job
{
    public class HangfireJobActivator : JobActivator
    {
        private readonly IServiceProvider _serviceProvider;

        public HangfireJobActivator(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            _serviceProvider = serviceProvider;
        }

        public override object ActivateJob(Type jobType)
        {
            // Ensure jobs run in their own "request" scope. Please.
            var scopeFactory = _serviceProvider.GetService<IServiceScopeFactory>();
            var scope = scopeFactory.CreateScope();
            var job = scope.ServiceProvider.GetService(jobType);

            if (job == null)
                throw new Exception($"Could not activate a job of type '{jobType.FullName}'.");

            return job;
        }
    }
}
