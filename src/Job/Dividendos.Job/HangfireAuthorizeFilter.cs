using Hangfire.Dashboard;

namespace Dividendos.Job
{
    public class HangFireAuthorizationFilter : IDashboardAuthorizationFilter
    {

        public bool Authorize(DashboardContext context)
        {
            return true;
        }
    }
}
