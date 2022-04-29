using Hangfire.Common;
using Hangfire.States;
using K.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dividendos.Job.Middlewares
{
    public class ErrorReportingJobFilter : JobFilterAttribute, IElectStateFilter
    {
        private readonly ILogger _logger;
        public ErrorReportingJobFilter(ILogger logger)
        {
            _logger = logger;
        }

        public void OnStateElection(ElectStateContext context)
        {
            // the way Hangfire works is retrying a job X times (10 by default), so this wont be called directly with a 
            // failed state sometimes.
            // To solve this we should look into TraversedStates for a failed state

            var failed = context.CandidateState as FailedState ??
                         context.TraversedStates.FirstOrDefault(x => x is FailedState) as FailedState;

            if (failed == null)
                return;

            _logger.SendErrorAsync(failed.Exception);
        }
    }
}
