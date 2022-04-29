using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Service.Interface
{
    public interface ISectorService : IBaseService
    {
        ResultServiceObject<IEnumerable<Sector>> GetAll();
        ResultServiceObject<Sector> GetByIdStock(long idStock);
        ResultServiceObject<IEnumerable<SectorView>> GetSumIdPortfolioPerformance(long idPortfolioPerformance);

        ResultServiceObject<IEnumerable<SectorView>> GetSumSubPortfolioPerformance(long idPortfolioPerformance, long idSubportfolio);

        ResultServiceObject<Sector> Update(Sector sector);
        ResultServiceObject<Sector> Insert(Sector sector);
    }
}
