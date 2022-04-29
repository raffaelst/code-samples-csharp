using Dividendos.API.Model.Request.Settings;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using Dividendos.API.Model.Response.Insight;
using Dividendos.Entity.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Application.Interface
{
    public interface IInsightsApp
    {
        ResultResponseObject<InsightsWrapper> GetByFilter(API.Model.Request.Insight.InsightsVM insightsVM);
    }
}
