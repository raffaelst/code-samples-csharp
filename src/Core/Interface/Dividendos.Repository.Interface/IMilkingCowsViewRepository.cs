using Dividendos.Entity.Entities;
using Dividendos.Entity.Enum;
using Dividendos.Repository.Interface.GenericRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Repository.Interface
{
    public interface IMilkingCowsViewRepository : IRepository<MilkingCowsView>
    {

        IEnumerable<MilkingCowsView> GetListMilkingCows(CountryEnum countryEnum);
    }
}
