using Dividendos.Entity.Entities;
using Dividendos.Entity.Views;
using Dividendos.Repository.Interface.GenericRepository;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Dividendos.Repository.Interface
{
    public interface IStockAllocationRepository : IRepository<StockAllocation>
    {
        IEnumerable<StockAllocation> GetSumIdPortfolioPerformance(long idPortfolioPerformance);
        IEnumerable<StockAllocation> GetSumSubPortfolioPerformance(long idPortfolioPerformance, long idSubportfolio);
        IEnumerable<StockAllocation> GetSumPerformanceStock(long idPortfolioPerformance);
        IEnumerable<StockAllocation> GetSumSubPerformanceStock(long idPortfolioPerformance, long idSubportfolio);
        IEnumerable<StockAllocation> GetSumSegmentStock(long idPortfolioPerformance, long idSegment);
        IEnumerable<StockAllocation> GetSumSubsectorStock(long idPortfolioPerformance, long idSubsector);
        IEnumerable<StockAllocation> GetSumSectorStock(long idPortfolioPerformance, long idSector);
    }
}
