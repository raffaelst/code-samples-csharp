using Dividendos.Entity.Entities;
using Dividendos.Entity.Enum;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Service.Interface
{
    public interface IOperationItemService : IBaseService
    {
        ResultServiceObject<IEnumerable<OperationItem>> GetActiveByPortfolio(long idPortfolio, bool? includeSubscription = null, long? idStock = null);
        ResultServiceObject<OperationItem> Update(OperationItem operation);
        ResultServiceObject<OperationItem> Insert(OperationItem operation, DateTime? lastUpdatedDate = null);
        ResultServiceObject<bool> Delete(OperationItem operation);
        ResultServiceObject<IEnumerable<OperationItem>> GetByIdOperation(long idOperation, int idOperationType);
        ResultServiceObject<OperationItem> GetByIdOperationItem(long idOperationItem);
        ResultServiceObject<IEnumerable<OperationItemView>> GetOperationItemsByIdPortfolio(long idPortfolio);
        void Inactive(long idOperationItem);
        ResultServiceObject<OperationItem> GetByGuidOperationItem(Guid guidOperationItem);
        ResultServiceObject<IEnumerable<OperationItemView>> GetAllItemViewByPortfolio(long idPortfolio, int idOperationType, bool? active = null, bool? includeSubscription = null);        
        ResultServiceObject<IEnumerable<OperationItem>> GetActiveByPortfolioAndDate(long idPortfolio, long idStock, DateTime dateTime);
        void InactivePriceAdjust(long idOperation);
        ResultServiceObject<bool> DeleteAllByPortfolio(long idPortfolio);
        DateTime? GetLastEventDate(string idUser, string identifier, string password, TraderTypeEnum traderTypeEnum, long? idTrader = null);
        ResultServiceObject<IEnumerable<OperationItem>> GetAllActiveByPortfolio(long idPortfolio);
        ResultServiceObject<IEnumerable<StockDivRangeView>> GetStockDivRangeView(long idPortfolio, int? idStockType = null);
        void UpdateSplitDate(long idOperationItem, DateTime splitDate);
        DateTime? GetLastEventDate(long idPortfolio);
        void DeleteFromDate(long idPortfolio, DateTime eventDate);
        void Update(long idOperationItem, decimal numberOfShares, decimal averagePrice, DateTime lastSplitDate);
    }
}
