using Dividendos.Entity.Entities;
using Dividendos.Repository.Interface.GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Repository.Interface
{
    public interface IAdvertiserExternalDetailRepository : IRepository<AdvertiserExternalDetail>
    {
        IEnumerable<AdvertiserExternalDetail> GetByAdvertiser(string advertiserGuid);
    }
}
