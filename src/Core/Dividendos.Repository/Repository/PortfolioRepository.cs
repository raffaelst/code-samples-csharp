using Dapper;
using Dividendos.Entity.Entities;
using Dividendos.Repository.GenericRepository;
using Dividendos.Repository.Interface;
using Dividendos.Repository.Interface.UoW;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Linq;

namespace Dividendos.Repository.Repository
{
    public class PortfolioRepository : Repository<Portfolio>, IPortfolioRepository
    {
        private IUnitOfWork _unitOfWork;

        public PortfolioRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Portfolio> GetByUserAndStatus(string idUser, bool status, bool? manual, bool onlySelectedToShowOnPatrimony)
        {
            StringBuilder sql = new StringBuilder(@"select Portfolio.IdPortfolio,Portfolio.Name,Portfolio.CreatedDate,Portfolio.Active,Portfolio.GuidPortfolio,Portfolio.IdTrader, Portfolio.ManualPortfolio, Portfolio.IdCountry, Portfolio.CalculatePerformanceDate, Portfolio.RestoredDividends 
                            from Portfolio  
                            inner join trader on trader.idtrader = portfolio.idtrader
                            where trader.iduser = @Iduser AND Portfolio.Active = @Active ");

            dynamic queryParams = new ExpandoObject();
            queryParams.Iduser = idUser;
            queryParams.Active = status;

            if (manual.HasValue)
            {
                queryParams.ManualPortfolio = manual.Value;
                sql.Append("and Portfolio.ManualPortfolio = @ManualPortfolio ");
            }

            if (onlySelectedToShowOnPatrimony)
            {
                sql.Append("and trader.ShowOnPatrimony = 1");
            }

            var portfolios = _unitOfWork.Connection.Query<Portfolio>(sql.ToString(), (object)queryParams, _unitOfWork.Transaction);

            return portfolios;
        }

        public IEnumerable<Portfolio> GetLastPortfoliosWithoutCalculation(DateTime currentDate)
        {
            string sql = @"SELECT TOP 100 Portfolio.IdPortfolio,Portfolio.Name,Portfolio.CreatedDate,Portfolio.Active,Portfolio.GuidPortfolio,Portfolio.IdTrader , Portfolio.CalculatePerformanceDate, AspNetUsers.LastAccess
							FROM Portfolio 
							INNER JOIN Trader ON Trader.IdTrader = Portfolio.IdTrader
							INNER JOIN AspNetUsers ON AspNetUsers.Id = Trader.IdUser
							WHERE Portfolio.Active = 1 AND Trader.Active = 1
                            AND (Portfolio.CalculatePerformanceDate IS NULL OR CONVERT(date, Portfolio.CalculatePerformanceDate) < @CurrentDate)
							ORDER BY Portfolio.CalculatePerformanceDate ASC";

            var portfolios = _unitOfWork.Connection.Query<Portfolio>(sql, new { CurrentDate = currentDate }, _unitOfWork.Transaction);

            return portfolios;
        }

        public IEnumerable<Portfolio> GetByGuidOperationItem(long idOperationItem)
        {
            string sql = @"select Portfolio.* from Portfolio
                            inner join Operation on Operation.IdPortfolio = Portfolio.IdPortfolio
                            inner join OperationItem on OperationItem.IdOperation = Operation.IdOperation
                            where OperationItem.IdOperationItem = @IdOperationItem";

            var portfolios = _unitOfWork.Connection.Query<Portfolio>(sql, new { IdOperationItem = idOperationItem }, _unitOfWork.Transaction);

            return portfolios;
        }

        public IEnumerable<Portfolio> GetByIdOperationItemHist(long idOperationItemHist)
        {
            string sql = @"select Portfolio.* from Portfolio
                            inner join OperationHist on OperationHist.IdPortfolio = Portfolio.IdPortfolio
                            inner join OperationItemHist on OperationItemHist.IdOperationHist = OperationHist.IdOperationHist
                            where OperationItemHist.IdOperationItemHist = @IdOperationItemHist";

            var portfolios = _unitOfWork.Connection.Query<Portfolio>(sql, new { IdOperationItemHist = idOperationItemHist }, _unitOfWork.Transaction);

            return portfolios;
        }

        public bool HasSubscription(Guid guidPortfolio)
        {
            string sql = @"SELECT CASE WHEN COUNT(Operation.IdOperation)  >= 1 THEN 1 ELSE 0 END
                            FROM Operation 
                            INNER JOIN Stock ON Stock.IdStock = Operation.IdStock 
                            INNER JOIN Portfolio on Portfolio.IdPortfolio = Operation.IdPortfolio            
                            WHERE Portfolio.GuidPortfolio = @GuidPortfolio AND Stock.ShowOnPortolio = 0  and Operation.IdOperationType = 1 and Operation.Active = 1 ";

            var portfolios = _unitOfWork.Connection.QueryFirstOrDefault<bool>(sql, new { GuidPortfolio = guidPortfolio }, _unitOfWork.Transaction);

            return portfolios;
        }

        public void UpdateName(long idPortfolio, string name)
        {
            string sql = $"UPDATE Portfolio SET Name = @Name WHERE IdPortfolio = @IdPortfolio";

            _unitOfWork.Connection.Execute(sql, new { IdPortfolio = idPortfolio, Name = name }, _unitOfWork.Transaction);
        }

        public void UpdateCalculatePerformanceDate(long idPortfolio, DateTime calculatePerformanceDate)
        {
            string sql = $"UPDATE Portfolio SET CalculatePerformanceDate = @CalculatePerformanceDate WHERE IdPortfolio = @IdPortfolio";

            _unitOfWork.Connection.Execute(sql, new { IdPortfolio = idPortfolio, CalculatePerformanceDate = calculatePerformanceDate }, _unitOfWork.Transaction);
        }

        public decimal? GetTotalPortfolio(long idPortfolio, DateTime limitDate, long? idStock = null, int? idStockType = null)
        {
            StringBuilder sql = new StringBuilder(@"with tbOperationItem as (
                            select distinct OperationItem.* from Operation  WITH (NOLOCK) 
                            inner join Stock  WITH (NOLOCK)  on Stock.IdStock = Operation.IdStock
                            inner join OperationItem  WITH (NOLOCK)  on Operation.IdOperation = OperationItem.IdOperation
                            where Operation.IdPortfolio = @IdPortfolio and OperationItem.Active = 1 and OperationItem.EventDate <= @LimitDate ");


            dynamic queryParams = new ExpandoObject();
            queryParams.IdPortfolio = idPortfolio;
            queryParams.LimitDate = limitDate;

            if (idStock.HasValue)
            {
                sql.Append("and OperationItem.IdStock = @IdStock ");
                queryParams.IdStock = idStock.Value;
            }

            if (idStockType.HasValue)
            {
                sql.Append("and Stock.IdStockType = @IdStockType ");
                queryParams.IdStockType = idStockType.Value;
            }

            sql.Append(@"), tbOpBuy as
                            (
	                            select sum (tbOperationItem.AveragePrice * tbOperationItem.NumberOfShares) total from tbOperationItem where tbOperationItem.idOperationType = 1
                            ), tbOpSell as
                            (
	                            select sum (tbOperationItem.AveragePrice * tbOperationItem.NumberOfShares) total from tbOperationItem where tbOperationItem.idOperationType = 2 
                           )
                          select isnull(tbOpBuy.total,0) - isnull(tbOpSell.total,0) from tbOpBuy, tbOpSell");



            var totalPortfolio = _unitOfWork.Connection.ExecuteScalar(sql.ToString(), (object)queryParams, _unitOfWork.Transaction);

            decimal? total = (decimal?)totalPortfolio;

            return total;
        }

        public bool HasZeroPrice(string idUser, string identifier, string password, DateTime deployDate)
        {
            string sql = @"select case when count(*) > 0 then 1 else 0 end as haszeroprice from Operation
                            inner join Stock on Stock.IdStock = Operation.IdStock
                            inner join Portfolio on Portfolio.IdPortfolio = Operation.IdPortfolio
                            inner join Trader on Trader.IdTrader = Portfolio.IdTrader
                            where Trader.TraderTypeID = 1 and Stock.ShowOnPortolio = 1 and Operation.Active = 1 and Operation.IdOperationType = 1 and Trader.IdUser = @IdUser and Trader.Identifier = @Identifier and Trader.Password = @Password and Operation.AveragePrice = 0 and Trader.LastSync < @DeployDate ";

            var isAdjusted = _unitOfWork.Connection.Query<bool>(sql, new { IdUser = idUser, Identifier = identifier, Password = password, DeployDate = deployDate }, _unitOfWork.Transaction).FirstOrDefault();

            return isAdjusted;
        }

        public void Disable(long idPortfolio)
        {
            string sql = $"UPDATE Portfolio SET Active = 0 WHERE IdPortfolio = @IdPortfolio";

            _unitOfWork.Connection.Execute(sql, new { IdPortfolio = idPortfolio }, _unitOfWork.Transaction);
        }
    }
}
