using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Service.Interface
{
    public interface IAdvertiserService : IBaseService
    {
        ResultServiceObject<Advertiser> Get();
        ResultServiceObject<AdvertiserDetails> GetDetails(string advertiserID);

        ResultServiceObject<IEnumerable<Advertiser>> GetGeneralAndByUser(string userId);
    }
}
