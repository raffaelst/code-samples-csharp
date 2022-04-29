using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Dividendos.Service.Interface
{
    public interface ISubsectorService : IBaseService
    {
        ResultServiceObject<IEnumerable<Subsector>> GetAll();
        ResultServiceObject<IEnumerable<SubsectorView>> GetSumIdPortfolioPerformance(long idPortfolioPerformance);
        ResultServiceObject<IEnumerable<SubsectorView>> GetSumSubPortfolioPerformance(long idPortfolioPerformance, long idSubportfolio);
        ResultServiceObject<Subsector> Update(Subsector subSector);
        ResultServiceObject<Subsector> Insert(Subsector subSector);
    }
}
