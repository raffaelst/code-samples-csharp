using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using System.Collections;
using System.Collections.Generic;

namespace Dividendos.Application.Interface
{
    public interface IAdvertiserApp
    {
        ResultResponseObject<AdvertiserVM> Get();
        ResultResponseObject<AdvertiserDetailsVM> GetDetails(string advertiserId);

        ResultResponseObject<IEnumerable<AdvertiserVM>> GetV2();
    }
}
