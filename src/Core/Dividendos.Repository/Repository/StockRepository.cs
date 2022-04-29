using Dapper;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Enum;
using Dividendos.Repository.GenericRepository;
using Dividendos.Repository.Interface;
using Dividendos.Repository.Interface.UoW;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Dividendos.Repository
{
    public class StockRepository : Repository<Stock>, IStockRepository
    {
        private IUnitOfWork _unitOfWork;

        public StockRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public IEnumerable<Stock> GetByUser(string idUser)
        {
            string sql = @"SELECT Stock.IdStock, Stock.GuidStock, Stock.IdCompany, Stock.IdStockType, Stock.MarketPrice, Stock.Symbol, Stock.TradeTime, Stock.UpdatedDate, Stock.ShowOnPortolio, Logo.LogoImage Logo, Stock.LastDailyVariationNotification FROM Stock
                            INNER JOIN Operation ON Operation.IdStock = Stock.IdStock
                            INNER JOIN Portfolio ON Portfolio.IdPortfolio = Operation.IdPortfolio
                            INNER JOIN Trader ON Trader.IdTrader = Portfolio.IdTrader
                            INNER JOIN Company ON Company.IdCompany = Stock.IdCompany
                            LEFT JOIN Logo ON Logo.IdLogo = Company.IdLogo
                            WHERE Trader.IdUser = @IdUser";

            var stocks = _unitOfWork.Connection.Query<Stock>(sql, new { IdUser = idUser }, _unitOfWork.Transaction);

            return stocks;
        }

        public IEnumerable<Stock> GetLikeSymbol(string symbol, int idCountry)
        {
            string sql = @"select top 7 IdStock, Symbol, GuidStock from stock where Symbol like '' + @Symbol + '%' and IdCountry = @IdCountry order by symbol";

            var stocks = _unitOfWork.Connection.Query<Stock>(sql, new { Symbol = symbol, IdCountry = idCountry }, _unitOfWork.Transaction);

            return stocks;
        }

        public IEnumerable<Stock> GetAllByCountry(int idCountry)
        {
            string sql = @"SELECT Stock.IdStock, Stock.IdCompany, Stock.Symbol, Stock.IdStockType, Stock.GuidStock, Stock.MarketPrice,
            Stock.UpdatedDate, Stock.TradeTime, Stock.ShowOnPortolio, Stock.IdCountry, Stock.LastChangePerc, Stock.OldSymbols, Stock.LastDividendUpdateSync, Stock.LastDailyVariationNotification
            FROM Stock WHERE Stock.IdCountry = @IdCountry";

            var stocks = _unitOfWork.Connection.Query<Stock>(sql, new { IdCountry = idCountry }, _unitOfWork.Transaction);

            return stocks;
        }

        public IEnumerable<Stock> GetAllShowOnPortfolio(int idCountry)
        {
            string sql = @"SELECT Stock.IdStock, Stock.IdCompany, Stock.Symbol, Stock.IdStockType, Stock.GuidStock, Stock.MarketPrice,
            Stock.UpdatedDate, Stock.TradeTime, Stock.ShowOnPortolio, Stock.IdCountry, Stock.LastChangePerc, Stock.OldSymbols, Stock.LastDividendUpdateSync, Stock.LastDailyVariationNotification
            FROM Stock WHERE Stock.IdCountry = @IdCountry AND Stock.ShowOnPortolio = 1";

            var stocks = _unitOfWork.Connection.Query<Stock>(sql, new { IdCountry = idCountry }, _unitOfWork.Transaction);

            return stocks;
        }

        public IEnumerable<Stock> GetByNameOrSymbol(string name)
        {
            string sql = @"SELECT TOP 7 IdStock, Symbol, GuidStock, Stock.IdCountry, Logo.LogoImage Logo, Stock.LastDailyVariationNotification FROM Stock 
            INNER JOIN Company ON Company.IdCompany = Stock.IdCompany
            LEFT JOIN Logo ON Logo.IdLogo = Company.IdLogo
            WHERE Stock.Symbol LIKE '%' + @Name + '%' OR Company.Name LIKE '%' + @Name + '%' OR Company.FullName LIKE '%' + @Name + '%'";

            var stocks = _unitOfWork.Connection.Query<Stock>(sql, new { Name = name }, _unitOfWork.Transaction);

            return stocks;
        }


        public IEnumerable<string> GetAllUsersWithStock(long idStock)
        {
            string sql = @"SELECT DISTINCT Trader.IdUser
							FROM Operation
							INNER JOIN Portfolio ON Portfolio.IdPortfolio = Operation.IdPortfolio AND Portfolio.Active = 1 AND Operation.Active = 1 AND Operation.IdOperationType = 1 AND Operation.NumberOfShares > 0
							INNER JOIN Trader ON Trader.IdTrader = Portfolio.IdTrader
							INNER JOIN Stock ON Stock.IdStock = Operation.IdStock AND Stock.ShowOnPortolio = 1
							WHERE Operation.IdStock = @IdStock";

            var portfolios = _unitOfWork.Connection.Query<string>(sql, new { IdStock = idStock }, _unitOfWork.Transaction);


            return portfolios;
        }

        public IEnumerable<Stock> GetByCompanyID(long idCompany)
        {
            string sql = @"SELECT Stock.IdStock, Stock.IdCompany, Stock.Symbol, Stock.IdStockType, Stock.GuidStock, Stock.MarketPrice,
            Stock.UpdatedDate, Stock.TradeTime, Stock.ShowOnPortolio, Stock.IdCountry, Stock.LastChangePerc, Stock.OldSymbols, Stock.LastDividendUpdateSync, Stock.LastDailyVariationNotification
            FROM Stock
            INNER JOIN Company ON Company.IdCompany = Stock.IdCompany
            WHERE Company.IdCompany = @IdCompany";

            var stocks = _unitOfWork.Connection.Query<Stock>(sql, new { IdCompany = idCompany }, _unitOfWork.Transaction);


            return stocks;
        }

        public Stock GetByLastDividendUpdateSyncOrderingAsc(int idCountry)
        {
            string sql = @"SELECT TOP 1 Stock.GuidStock, Stock.IdCompany, 
                            Stock.IdCountry, Stock.IdStock, Stock.IdStockType, Stock.LastChangePerc, 
                            Stock.MarketPrice, Stock.OldSymbols, Stock.ShowOnPortolio, Stock.Symbol, 
                            Stock.TradeTime, Stock.UpdatedDate, Stock.LastDailyVariationNotification FROM Stock WHERE Stock.IdCountry = @IdCountry ORDER BY Stock.LastDividendUpdateSync ASC";

            var stock = _unitOfWork.Connection.Query<Stock>(sql, new { IdCountry = idCountry }, _unitOfWork.Transaction).FirstOrDefault();


            return stock;
        }

        public Stock UpdateLastDividendUpdateSync(Stock stock, DateTime date)
        {

            string sql = "UPDATE Stock SET LastDividendUpdateSync = @LastDividendUpdateSync WHERE Stock.IdStock = @IdStock";

            _unitOfWork.Connection.Execute(sql, new { LastDividendUpdateSync = date, IdStock = stock.IdStock }, _unitOfWork.Transaction);

            return stock;
        }

        public void UpdateLastDailyVariationNotification(long idStock, DateTime date)
        {

            string sql = "UPDATE Stock SET LastDailyVariationNotification = @LastDailyVariationNotification WHERE Stock.IdStock = @IdStock";

            _unitOfWork.Connection.Execute(sql, new { LastDailyVariationNotification = date, IdStock = idStock }, _unitOfWork.Transaction);
        }

        public Stock GetByLastDividendUpdateSyncOrderingAsc(int idCountry, StockTypeEnum stockType)
        {
            string sql = @"SELECT TOP 1 Stock.GuidStock, Stock.IdCompany, 
                            Stock.IdCountry, Stock.IdStock, Stock.IdStockType, Stock.LastChangePerc, 
                            Stock.MarketPrice, Stock.OldSymbols, Stock.ShowOnPortolio, Stock.Symbol, 
                            Stock.TradeTime, Stock.UpdatedDate, Stock.LastDailyVariationNotification FROM Stock WHERE Stock.IdCountry = @IdCountry AND Stock.IdStockType = @IdStockType ORDER BY Stock.LastDividendUpdateSync ASC";

            var stock = _unitOfWork.Connection.Query<Stock>(sql, new { IdCountry = idCountry, IdStockType = (int)stockType }, _unitOfWork.Transaction).FirstOrDefault();


            return stock;
        }

        public IEnumerable<Stock> GetBySymbolOrLikeOldSymbol(string symbol, int idCountry)
        {
            string sql = @"select top 7 Stock.IdStock, Stock.IdCompany, Stock.Symbol, Stock.IdStockType, Stock.GuidStock, Stock.MarketPrice,
            Stock.UpdatedDate, Stock.TradeTime, Stock.ShowOnPortolio, Stock.IdCountry, Stock.LastChangePerc, Stock.OldSymbols, Stock.LastDividendUpdateSync, Stock.LastDailyVariationNotification from stock where (Symbol = @Symbol or OldSymbols like '' + @Symbol + '%') and IdCountry = @IdCountry order by symbol";

            var stocks = _unitOfWork.Connection.Query<Stock>(sql, new { Symbol = symbol, IdCountry = idCountry }, _unitOfWork.Transaction);

            return stocks;
        }

        public IEnumerable<Stock> GetAllByStockType(int idCountry, StockTypeEnum stockType)
        {
            string sql = @"SELECT Stock.GuidStock, Stock.IdCompany, 
                            Stock.IdCountry, Stock.IdStock, Stock.IdStockType, Stock.LastChangePerc, 
                            Stock.MarketPrice, Stock.OldSymbols, Stock.ShowOnPortolio, Stock.Symbol, 
                            Stock.TradeTime, Stock.UpdatedDate, Stock.LastDailyVariationNotification FROM Stock WHERE Stock.IdCountry = @IdCountry AND Stock.IdStockType = @IdStockType AND stock.TradeTime > @LastTradeTime ORDER BY Stock.LastDividendUpdateSync ASC";

            var stock = _unitOfWork.Connection.Query<Stock>(sql, new { IdCountry = idCountry, IdStockType = (int)stockType, LastTradeTime = DateTime.Now.AddDays(-10) }, _unitOfWork.Transaction);


            return stock;
        }

        public IEnumerable<Stock> GetAllByCountryOrderByLastDividendUpdateSync(int idCountry)
        {
            string sql = @"SELECT Stock.GuidStock, Stock.IdCompany, 
                            Stock.IdCountry, Stock.IdStock, Stock.IdStockType, Stock.LastChangePerc, 
                            Stock.MarketPrice, Stock.OldSymbols, Stock.ShowOnPortolio, Stock.Symbol, 
                            Stock.TradeTime, Stock.UpdatedDate, Stock.LastDailyVariationNotification FROM Stock WHERE Stock.IdCountry = @IdCountry AND stock.TradeTime > @LastTradeTime ORDER BY Stock.LastDividendUpdateSync ASC";

            var stock = _unitOfWork.Connection.Query<Stock>(sql, new { IdCountry = idCountry, LastTradeTime = DateTime.Now.AddDays(-10) }, _unitOfWork.Transaction);


            return stock;
        }
    }
}
