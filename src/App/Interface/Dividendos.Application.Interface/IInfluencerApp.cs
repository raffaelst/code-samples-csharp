using Dividendos.API.Model.Request.Affiliation;
using Dividendos.API.Model.Request.Device;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using Dividendos.API.Model.Response.Influencer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Application.Interface
{
    public interface IInfluencerApp
    {
        ResultResponseObject<IEnumerable<InfluencerVM>> GetUsersBehindInfluencer(string influencerAffiliatorGuid);
    }
}
