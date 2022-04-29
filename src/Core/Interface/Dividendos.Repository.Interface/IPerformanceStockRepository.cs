using Dividendos.Entity.Entities;
using Dividendos.Repository.Interface.GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Repository.Interface
{
    public interface IPerformanceStockRepository : IRepository<PerformanceStock>
    {
        decimal GetTotalByIdStockType(long idPortfolioPerformance, long? idStock = null, int? idStockType = null);
        decimal GetLatestTotalByIdStock(long idPortfolio, long idStock, int year);
        decimal GetTotalAmount(long idPortfolio, DateTime limitDate, long idStock);
    }
}
