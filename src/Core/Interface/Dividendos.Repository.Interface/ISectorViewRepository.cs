using Dividendos.Entity.Views;
using Dividendos.Repository.Interface.GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Repository.Interface
{
    public interface ISectorViewRepository : IRepository<SectorView>
    {
        IEnumerable<SectorView> GetSumIdPortfolioPerformance(long idPortfolioPerformance);
        IEnumerable<SectorView> GetSumSubPortfolioPerformance(long idPortfolioPerformance, long idSubportfolio);
    }
}
