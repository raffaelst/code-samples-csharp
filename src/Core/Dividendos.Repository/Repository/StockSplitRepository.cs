using Dapper;
using Dividendos.Entity.Entities;
using Dividendos.Repository.GenericRepository;
using Dividendos.Repository.Interface;
using Dividendos.Repository.Interface.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dividendos.Repository.Repository
{
    public class StockSplitRepository : Repository<StockSplit>, IStockSplitRepository
    {
        private IUnitOfWork _unitOfWork;

        public StockSplitRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public IEnumerable<StockSplit> Get(bool onlyMyStocks, string userID, DateTime starDate, DateTime endDate)
        {
            StringBuilder sql = new StringBuilder(@"SELECT StockSplit.StockSplitID, StockSplit.StockID,StockSplit.DateSplit,StockSplit.ProportionFrom,StockSplit.ProportionTo,StockSplit.Unfolded,StockSplit.IdCountry  FROM StockSplit WHERE StockSplit.DateSplit BETWEEN @StartDate AND @EndDate");

            if (onlyMyStocks)
            {
                sql.Append(@" AND StockSplit.StockID IN (SELECT Stock.IdStock FROM Stock
                            INNER JOIN Operation ON Operation.IdStock = Stock.IdStock AND Operation.Active = 1 AND Operation.IdOperationType = 1 AND Operation.NumberOfShares > 0
                            INNER JOIN Portfolio ON Portfolio.IdPortfolio = Operation.IdPortfolio AND Portfolio.Active = 1
                            INNER JOIN Trader ON Trader.IdTrader = Portfolio.IdTrader AND Trader.Active = 1
                            WHERE Trader.IdUser = @IdUser) ");
            }

            var advertiser = _unitOfWork.Connection.Query<StockSplit>(sql.ToString(), new { IdUser = userID, StartDate = starDate, EndDate = endDate }, _unitOfWork.Transaction);

            return advertiser;
        }

        public IEnumerable<StockSplit> GetByIdStock(long idStock, DateTime eventDate)
        {
            StringBuilder sql = new StringBuilder(@"SELECT top 1 StockSplit.StockSplitID, StockSplit.StockID, StockSplit.DateSplit, StockSplit.ProportionFrom, StockSplit.ProportionTo, StockSplit.Unfolded, StockSplit.IdCountry ");
            sql.Append("FROM StockSplit where StockSplit.StockID = @IdStock and StockSplit.DateSplit > @EventDate order by DateSplit desc ");


            var advertiser = _unitOfWork.Connection.Query<StockSplit>(sql.ToString(), new { IdStock = idStock, EventDate = eventDate }, _unitOfWork.Transaction);

            return advertiser;
        }

        public IEnumerable<StockSplit> GetLatestByIdStock(long idStock)
        {
            StringBuilder sql = new StringBuilder(@"SELECT top 1 StockSplit.StockSplitID, StockSplit.StockID, StockSplit.DateSplit, StockSplit.ProportionFrom, StockSplit.ProportionTo, StockSplit.Unfolded, StockSplit.IdCountry ");
            sql.Append("FROM StockSplit where StockSplit.StockID = @IdStock order by DateSplit desc ");


            var advertiser = _unitOfWork.Connection.Query<StockSplit>(sql.ToString(), new { IdStock = idStock }, _unitOfWork.Transaction);

            return advertiser;
        }

        public bool HasStockSplit(string idUser, DateTime limitDate)
        {
            string sql = @"select count(operationitem.IdOperationItem) hasSPLIT from operationitem 
                            inner join StockSplit on StockSplit.StockID = OperationItem.IdStock 
                            inner join Operation on Operation.IdOperation = OperationItem.IdOperation 
                            inner join Portfolio on Portfolio.IdPortfolio = Operation.IdPortfolio 
                            inner join Trader on trader.IdTrader = Portfolio.IdTrader 
                            where Trader.IdUser = @IdUser and Operation.IdOperationType = 1 and Operation.Active = 1 and Portfolio.Active = 1 and OperationItem.active = 1 and Trader.Active = 1 
                            and OperationItem.LastSplitDate < StockSplit.DateSplit and StockSplit.DateSplit > @LimitDate and StockSplit.DateSplit <= Convert(DATE, getdate())  ";

            var hasRecentSubscription = _unitOfWork.Connection.Query<bool>(sql, new { IdUser = idUser, LimitDate = limitDate }, _unitOfWork.Transaction).FirstOrDefault();

            return hasRecentSubscription;
        }

        public IEnumerable<StockSplit> GetByGuidAndDate(Guid stockGuid, DateTime starDate, DateTime endDate)
        {
            StringBuilder sql = new StringBuilder(@"select StockSplit.StockSplitID, StockSplit.StockID, StockSplit.DateSplit, StockSplit.ProportionFrom, StockSplit.ProportionTo, StockSplit.Unfolded, Stock.IdCountry from StockSplit
                                                    inner join Stock on Stock.IdStock = StockSplit.StockID
                                                    where Stock.GuidStock = @GuidStock and DateSplit between @StartDate and @EndDate");


            var advertiser = _unitOfWork.Connection.Query<StockSplit>(sql.ToString(), new { GuidStock = stockGuid, StartDate = starDate, EndDate = endDate }, _unitOfWork.Transaction);

            return advertiser;
        }
    }
}
