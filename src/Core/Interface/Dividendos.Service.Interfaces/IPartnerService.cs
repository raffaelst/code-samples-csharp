using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Service.Interface
{
    public interface IPartnerService : IBaseService
    {
        ResultServiceObject<Partner> GetAvailable(string idUser);

        ResultServiceObject<PartnerRedeem> Add(string idUser, long partnerID);

        void UpdateUserConfirmation(long partnerRedeemID);

        ResultServiceObject<PartnerRedeem> CheckReedemAvailable(string idUser);

        ResultServiceObject<Partner> GetPartnerByGuid(Guid partnerGuid);

        ResultServiceObject<PartnerRedeem> GetPartnerRedeemByGuid(Guid partnerRedeemGuid);

        ResultServiceObject<Partner> GetDetail(Guid partnerGuid);

        ResultServiceObject<IEnumerable<Partner>> GetAllAvailable(string idUser);
        ResultServiceObject<bool> GetSubscriptionByPartnerIsAvailable(string idUser);
    }
}
