using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
namespace Dividendos.Service.Interface
{
    public interface IPerformanceStockService : IBaseService
    {
        ResultServiceObject<IEnumerable<PerformanceStock>> GetByIdPortfolioPerformance(long idPortfolioPerformance);
        ResultServiceObject<IEnumerable<StockAllocation>> GetSumIdPortfolioPerformance(long idPortfolioPerformance);
        ResultServiceObject<IEnumerable<StockAllocation>> GetSumSubPortfolioPerformance(long idPortfolioPerformance, long idSubportfolio);
        ResultServiceObject<IEnumerable<StockAllocation>> GetSumPerformanceStock(long idPortfolioPerformance);
        ResultServiceObject<IEnumerable<StockAllocation>> GetSumSubPerformanceStock(long idPortfolioPerformance, long idSubportfolio);
        ResultServiceObject<PerformanceStock> Update(PerformanceStock portfolioStock);
        ResultServiceObject<PerformanceStock> Insert(PerformanceStock portfolioStock);
        ResultServiceObject<bool> Delete(PerformanceStock portfolioStock);
        ResultServiceObject<IEnumerable<StockAllocation>> GetSumSegmentStock(long idPortfolioPerformance, long idSegment);
        ResultServiceObject<IEnumerable<StockAllocation>> GetSumSubsectorStock(long idPortfolioPerformance, long idSubsector);
        ResultServiceObject<IEnumerable<StockAllocation>> GetSumSectorStock(long idPortfolioPerformance, long idSector);
        decimal GetTotalByIdStockType(long idPortfolioPerformance, long? idStock = null, int? idStockType = null);
        ResultServiceObject<PerformanceStock> GetByIdPortfolioPerformanceStock(long idPortfolioPerformance, long idStock);
        decimal GetLatestTotalByIdStock(long idPortfolio, long idStock, int year);
        decimal GetTotalAmount(long idPortfolio, DateTime limitDate, long idStock);
    }
}
