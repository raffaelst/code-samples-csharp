using Dividendos.API.Model.Request.FreeRecommendations;
using Dividendos.API.Model.Request.InvestmentAdvisor;
using Dividendos.API.Model.Request.Settings;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using Dividendos.API.Model.Response.v1.InvestmentAdvisor;
using Dividendos.Entity.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Application.Interface
{
    public interface IInvestmentAdvisorApp
    {
        ResultResponseObject<IEnumerable<InvestmentAdvisorVideoResponse>> GetVideosAvailable(bool onlyLastVideo);
        ResultResponseObject<IEnumerable<FreeRecommendationResponse>> GetFreeRecommendation();
    }
}
