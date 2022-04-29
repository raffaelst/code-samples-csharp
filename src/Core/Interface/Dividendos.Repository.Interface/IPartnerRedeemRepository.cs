using Dividendos.Entity.Entities;
using Dividendos.Repository.Interface.GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Repository.Interface
{
    public interface IPartnerRedeemRepository : IRepository<PartnerRedeem>
    {
        PartnerRedeem ExistForASpecificUser(string userID);

        void UpdateUserConfirmation(long partnerRedeemID);
    }
}
