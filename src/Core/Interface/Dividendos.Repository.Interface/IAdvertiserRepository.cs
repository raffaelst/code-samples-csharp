using Dividendos.Entity.Entities;
using Dividendos.Repository.Interface.GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Repository.Interface
{
    public interface IAdvertiserRepository : IRepository<Advertiser>
    {
        IEnumerable<Advertiser> GetGeneralAndByUser(string userId, DateTime dateTimeShowOffSet);

        IEnumerable<Advertiser> GetOnlyGeneral();
    }
}
