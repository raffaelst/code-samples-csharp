using Dividendos.Entity.Entities;
using Dividendos.Entity.Enum;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Service.Interface
{
    public interface IMarketMoverService : IBaseService
    {
        ResultServiceObject<IEnumerable<MarketMoverView>> GetByType(MaketMoversTypeEnum marketMoverTypeEnum);

        ResultServiceObject<IEnumerable<MarketMover>> InsertAll(IEnumerable<MarketMover> marketMovers, MaketMoversTypeEnum marketMoverTypeEnum);

        ResultServiceObject<MarketMover> Insert(MarketMover marketMover);

        void DeleteByType(MaketMoversTypeEnum marketMoverTypeEnum);

        ResultServiceObject<IEnumerable<MilkingCowsView>> GetMilkingCows(CountryEnum countryEnum);

    }
}
