using Dividendos.Entity.Entities;
using Dividendos.Entity.Enum;
using Dividendos.Repository.Interface.GenericRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Repository.Interface
{
    public interface IMarketMoverViewRepository : IRepository<MarketMoverView>
    {
        IEnumerable<MarketMoverView> GetByType(MaketMoversTypeEnum marketMoverTypeEnum);

        IEnumerable<MarketMoverView> BiggestHighsOrLowsAllBR(bool highs);

        IEnumerable<MarketMoverView> GetChineseCompanies();

        IEnumerable<MarketMoverView> BiggestHighsOrLowsBDR(bool highs);
    }
}
