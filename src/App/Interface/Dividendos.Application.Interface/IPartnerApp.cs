using Dividendos.API.Model.Request.Settings;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using Dividendos.API.Model.Response.v1;
using Dividendos.Entity.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Application.Interface
{
    public interface IPartnerApp
    {
        ResultResponseObject<PartnerOptionVM> GetSubscriptionByPartnerIsAvailable();
        ResultResponseObject<PartnerVM> GetAvailableV1();
        ResultResponseObject<IEnumerable<PartnerVM>> GetAvailableV2();
        ResultResponseObject<PartnerVM> GetDetail(Guid partnerID);
    }
}
