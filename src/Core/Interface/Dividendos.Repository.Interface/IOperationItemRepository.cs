using Dividendos.Entity.Entities;
using Dividendos.Entity.Enum;
using Dividendos.Entity.Views;
using Dividendos.Repository.Interface.GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;


namespace Dividendos.Repository.Interface
{
    public interface IOperationItemRepository : IRepository<OperationItem>
    {
        IEnumerable<OperationItem> GetActiveByPortfolio(long idPortfolio, bool? includeSubscription = null, long? idStock = null);
        IEnumerable<OperationItem> GetAllActiveByPortfolio(long idPortfolio);
        IEnumerable<OperationItemView> GetAllItemViewByPortfolio(long idPortfolio, int idOperationType, bool? active = null, bool? includeSubscription = null);
        IEnumerable<OperationItem> GetActiveByPortfolioAndDate(long idPortfolio, long stock, DateTime dateTimeOffSet);
        void InactivePriceAdjust(long idOperation);

        void DeleteAllByPortfolio(long idPortfolio);
        DateTime? GetLastEventDate(string idUser, string identifier, string password, TraderTypeEnum traderTypeEnum, long? idTrader = null);
        IEnumerable<StockDivRangeView> GetStockDivRangeView(long idPortfolio, int? idStockType = null);
        void UpdateSplitDate(long idOperationItem, DateTime splitDate);
        DateTime? GetLastEventDate(long idPortfolio);
        void DeleteFromDate(long idPortfolio, DateTime eventDate);
        void Inactive(long idOperationItem);
        void Update(long idOperationItem, decimal numberOfShares, decimal averagePrice, DateTime lastSplitDate);
    }
}
