using Dividendos.Entity.Entities;
using Dividendos.Entity.Views;
using Dividendos.Repository.Interface.GenericRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Repository.Interface
{
    public interface IInfluencerViewRepository : IRepository<InfluencerView>
    {
        IEnumerable<InfluencerView> GetUsersBehindInfluencer(string influencerAffiliatorGuid);
    }
}
