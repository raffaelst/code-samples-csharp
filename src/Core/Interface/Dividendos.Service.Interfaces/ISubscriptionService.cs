using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Service.Interface
{
    public interface ISubscriptionService : IBaseService
    {
        ResultServiceObject<Subscription> GetByUser(string idUser);

        ResultServiceObject<Subscription> Insert(Subscription subscription);

        ResultServiceObject<Subscription> Update(Subscription subscription);
        ResultServiceObject<IEnumerable<Subscription>> GetByPartner(long partnerId);
    }
}
