using Dividendos.Entity.Enum;
using Dividendos.Entity.Views;
using Dividendos.Repository.Interface.GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Repository.Interface
{
    public interface ISegmentViewRepository : IRepository<SegmentView>
    {
        IEnumerable<SegmentView> GetSumIdPortfolioPerformance(long idPortfolioPerformance);
        IEnumerable<SegmentView> GetSumSubPortfolioPerformance(long idPortfolioPerformance, long idSubportfolio);
        IEnumerable<SegmentView> GetSumIdPortfolioPerformance(long idPortfolioPerformance, StockTypeEnum stockTypeEnum);
        IEnumerable<SegmentView> GetSumSubPortfolioPerformance(long idPortfolioPerformance, long idSubportfolio, StockTypeEnum stockTypeEnum);
    }
}
