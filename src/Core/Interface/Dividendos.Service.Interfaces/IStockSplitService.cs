using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Service.Interface
{
    public interface IStockSplitService : IBaseService
    {
        ResultServiceObject<IEnumerable<StockSplit>> Get(bool onlyMyStocks, string userID, DateTime starDate, DateTime endDate);
        ResultServiceObject<StockSplit> GetBy(long idStock, DateTime splitDate, int idCountry);
        ResultServiceObject<StockSplit> Add(StockSplit stockSplit);
        ResultServiceObject<IEnumerable<StockSplit>> GetAllByDate(long idStock, DateTime splitDate, int idCountry);
        List<OperationItem> ApplyStockSplit(ref List<OperationItem> operationItemStock, int idCountry);
        ResultServiceObject<StockSplit> GetByIdStock(long idStock, DateTime eventDate);
        ResultServiceObject<StockSplit> GetLatestByIdStock(long idStock);
        bool HasStockSplit(string idUser, DateTime limitDate);
        ResultServiceObject<IEnumerable<StockSplit>> GetByGuidAndDate(Guid stockGuid, DateTime starDate, DateTime endDate);
    }
}
