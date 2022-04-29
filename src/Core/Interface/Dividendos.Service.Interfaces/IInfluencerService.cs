using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Service.Interface
{
    public interface IInfluencerService : IBaseService
    {
        ResultServiceObject<IEnumerable<InfluencerView>> GetUsersBehindInfluencer(string influencerAffiliatorGuid);
    }
}
