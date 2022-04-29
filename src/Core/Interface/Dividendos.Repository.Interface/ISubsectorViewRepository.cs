using Dividendos.Entity.Views;
using Dividendos.Repository.Interface.GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Repository.Interface
{
    public interface ISubsectorViewRepository : IRepository<SubsectorView>
    {
        IEnumerable<SubsectorView> GetSumIdPortfolioPerformance(long idPortfolioPerformance);
        IEnumerable<SubsectorView> GetSumSubPortfolioPerformance(long idPortfolioPerformance, long idSubportfolio);
    }
}
