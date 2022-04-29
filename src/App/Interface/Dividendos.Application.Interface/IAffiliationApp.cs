using Dividendos.API.Model.Request.Affiliation;
using Dividendos.API.Model.Request.Device;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Application.Interface
{
    public interface IAffiliationApp
    {
        ResultResponseObject<IEnumerable<AffiliateProductDetailVM>> GetByType(AffiliationType affiliationType);
    }
}
