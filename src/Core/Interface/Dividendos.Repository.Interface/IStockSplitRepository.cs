using Dividendos.Entity.Entities;
using Dividendos.Repository.Interface.GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Repository.Interface
{
    public interface IStockSplitRepository : IRepository<StockSplit>
    {
        IEnumerable<StockSplit> Get(bool onlyMyStocks, string userID, DateTime starDate, DateTime endDate);
        IEnumerable<StockSplit> GetByIdStock(long idStock, DateTime eventDate);
        IEnumerable<StockSplit> GetLatestByIdStock(long idStock);
        bool HasStockSplit(string idUser, DateTime limitDate);
        IEnumerable<StockSplit> GetByGuidAndDate(Guid stockGuid, DateTime starDate, DateTime endDate);
    }
}
