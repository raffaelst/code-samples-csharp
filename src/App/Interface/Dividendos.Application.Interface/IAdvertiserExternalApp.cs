using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using System.Collections;
using System.Collections.Generic;

namespace Dividendos.Application.Interface
{
    public interface IAdvertiserExternalApp
    {
        ResultResponseObject<IEnumerable<AdvertiserExternalVM>> Get();
        ResultResponseObject<AdvertiserExternalDetailVM> GetDetails(string advertiserId);
        ResultResponseObject<IEnumerable<AdvertiserExternalVM>> GetV2();
        ResultResponseObject<AdvertiserExternalDetailVM> GetDetailsV2(string advertiserId);
    }
}
