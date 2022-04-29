using Dapper;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Enum;
using Dividendos.Entity.Views;
using Dividendos.Repository.GenericRepository;
using Dividendos.Repository.Interface;
using Dividendos.Repository.Interface.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dividendos.Repository.Repository
{
    public class DividendCalendarViewRepository : Repository<DividendCalendarView>, IDividendCalendarViewRepository
    {
        private IUnitOfWork _unitOfWork;

        public DividendCalendarViewRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public DividendCalendarView ExistByDateAndStock(long idStock, DateTime dataCom, DateTime? paymentDate, decimal value)
        {
            StringBuilder sql = new StringBuilder(@"SELECT DividendCalendarID
                      FROM DividendCalendar
					  WHERE IdStock = @IdStock AND Value = @Value AND DataCom = @DataCom AND ");

            if (paymentDate.HasValue)
            {
                sql.Append("PaymentDate = @PaymentDate ");
            }
            else
            {
                sql.Append("PaymentDate IS NULL ");
            }

            DividendCalendarView  dividend = _unitOfWork.Connection.Query<DividendCalendarView>(sql.ToString(), new { IdStock = idStock, Value = value, DataCom = dataCom, PaymentDate = paymentDate }, _unitOfWork.Transaction).FirstOrDefault();

            return dividend;
        }

        public IEnumerable<DividendCalendarView> GetByDataComLimit(DateTime startDate)
        {
            string sql = @"SELECT DISTINCT [DividendCalendarID]
                          ,DividendCalendar.[IdStock]
	                      ,Stock.Symbol AS StockName
                          ,DividendCalendar.[IdDividendType]
	                      ,DividendType.Name AS DividendType
                          ,DataCom
                          ,[PaymentDate]
                          ,[Value],
						  DividendType.Name,
                          PaymentDatepartiallyUndefined,
                          PaymentUndefined
                      FROM [DividendCalendar]
                      INNER JOIN DividendType ON DividendType.IdDividendType = DividendCalendar.IdDividendType
                      INNER JOIN Stock ON Stock.IdStock = DividendCalendar.IdStock
					  WHERE DataCom >= @DataCom";

            IEnumerable<DividendCalendarView> dividends = _unitOfWork.Connection.Query<DividendCalendarView>(sql, new { DataCom = startDate }, _unitOfWork.Transaction);

            return dividends;
        }
        public IEnumerable<DividendCalendarView> GetByPaymentDateAndStock(DateTime paymentStartDate, DateTime paymentEndDate, long idStock)
        {
            string sql = @"SELECT [DividendCalendarID]
                          ,DividendCalendar.[IdStock]
	                      ,Stock.Symbol AS StockName
                          ,DividendCalendar.[IdDividendType]
	                      ,DividendType.Name AS DividendType
                          ,DataCom
                          ,[PaymentDate]
                          ,[Value],
						  DividendType.Name,
                          PaymentDatepartiallyUndefined,
                          PaymentUndefined
                      FROM [DividendCalendar]
                      INNER JOIN DividendType ON DividendType.IdDividendType = DividendCalendar.IdDividendType
                      INNER JOIN Stock ON Stock.IdStock = DividendCalendar.IdStock
					  WHERE DividendCalendar.IdStock = @IdStock AND (DividendCalendar.PaymentDate BETWEEN @PaymentDateStart AND @PaymentDateEnd OR DividendCalendar.DataCom BETWEEN @PaymentDateStart AND @PaymentDateEnd)";

            IEnumerable<DividendCalendarView> dividends = _unitOfWork.Connection.Query<DividendCalendarView>(sql, new { PaymentDateStart = paymentStartDate, PaymentDateEnd = paymentEndDate, IdStock = idStock  }, _unitOfWork.Transaction);

            return dividends;
        }

        public IEnumerable<DividendCalendarView> GetByPaymentDateByYear(DateTime startDate, DateTime endDate, CountryEnum countryEnum, string idUser, bool onlyMyStocks, List<StockTypeEnum> stockTypes)
        {
            StringBuilder filterStockType = new StringBuilder();
            if (stockTypes != null && stockTypes.Count > 0)
            {
                filterStockType.Append("Stock.IdStockType IN (");
                bool firstExecution = true;

                foreach (var itemStockType in stockTypes)
                {
                    if (firstExecution)
                    {
                        firstExecution = false;
                        filterStockType.Append((int)itemStockType);
                    }
                    else
                    {
                        filterStockType.Append(string.Concat(",", (int)itemStockType));
                    }
                }

                filterStockType.Append(") AND ");
            }

            StringBuilder sql = new StringBuilder(@"SELECT DISTINCT [DividendCalendarID]
                          ,DividendCalendar.[IdStock]
	                      ,Stock.Symbol AS StockName
                          ,DividendCalendar.[IdDividendType]
	                      ,DividendType.Name AS DividendType
                          ,Stock.MarketPrice AS MarketStockValue
                          ,DataCom
                          ,[PaymentDate]
                          ,[Value],
						  DividendType.Name,
                          PaymentDatepartiallyUndefined,
                          PaymentUndefined
                      FROM [DividendCalendar]
                      INNER JOIN DividendType ON DividendType.IdDividendType = DividendCalendar.IdDividendType
                      INNER JOIN Stock ON Stock.IdStock = DividendCalendar.IdStock AND Stock.IdCountry = @IdCountry ");

            if (onlyMyStocks)
            {
                sql.Append(string.Format(@"WHERE {0} Stock.IdStock IN (SELECT Stock.IdStock FROM Stock
                            INNER JOIN Operation ON Operation.IdStock = Stock.IdStock AND Operation.Active = 1 AND Operation.IdOperationType = 1 AND Operation.NumberOfShares > 0
                            INNER JOIN Portfolio ON Portfolio.IdPortfolio = Operation.IdPortfolio AND Portfolio.Active = 1
                            INNER JOIN Trader ON Trader.IdTrader = Portfolio.IdTrader AND Trader.Active = 1
                            WHERE Trader.IdUser = @IdUser) AND DividendCalendar.PaymentDate BETWEEN @StartDate AND @FinalDate ORDER BY DividendCalendar.PaymentDate ASC", filterStockType));
            }
            else
            {
                sql.Append(string.Format("WHERE {0} PaymentDate BETWEEN @StartDate AND @FinalDate ORDER BY PaymentDate ASC", filterStockType));
            }

            IEnumerable<DividendCalendarView> dividends = _unitOfWork.Connection.Query<DividendCalendarView>(sql.ToString(), new { StartDate = startDate, FinalDate = endDate, IdCountry = (int)countryEnum, IdUser = idUser }, _unitOfWork.Transaction);

            return dividends;
        }


        public IEnumerable<DividendCalendarView> GetByDataComByYear(DateTime startDate, DateTime endDate, CountryEnum countryEnum, string idUser, bool onlyMyStocks, List<StockTypeEnum> stockTypes)
        {
            StringBuilder filterStockType = new StringBuilder();
            if (stockTypes != null && stockTypes.Count > 0)
            {
                filterStockType.Append("Stock.IdStockType IN (");
                bool firstExecution = true;

                foreach (var itemStockType in stockTypes)
                {
                    if (firstExecution)
                    {
                        firstExecution = false;
                        filterStockType.Append((int)itemStockType);
                    }
                    else
                    {
                        filterStockType.Append(string.Concat(",", (int)itemStockType));
                    }
                }

                filterStockType.Append(") AND ");
            }

            StringBuilder sql = new StringBuilder(@"SELECT DISTINCT [DividendCalendarID]
                          ,DividendCalendar.[IdStock]
	                      ,Stock.Symbol AS StockName
                          ,Stock.MarketPrice AS MarketStockValue
                          ,DividendCalendar.[IdDividendType]
	                      ,DividendType.Name AS DividendType
                          ,DataCom
                          ,[PaymentDate]
                          ,[Value],
						  DividendType.Name,
                          PaymentDatepartiallyUndefined,
                          PaymentUndefined
                      FROM [DividendCalendar]
                      INNER JOIN DividendType ON DividendType.IdDividendType = DividendCalendar.IdDividendType
                      INNER JOIN Stock ON Stock.IdStock = DividendCalendar.IdStock AND Stock.IdCountry = @IdCountry ");

            if (onlyMyStocks)
            {
                sql.Append(string.Format(@"WHERE {0} Stock.IdStock IN (SELECT Stock.IdStock FROM Stock
                            INNER JOIN Operation ON Operation.IdStock = Stock.IdStock AND Operation.Active = 1 AND Operation.IdOperationType = 1 AND Operation.NumberOfShares > 0
                            INNER JOIN Portfolio ON Portfolio.IdPortfolio = Operation.IdPortfolio AND Portfolio.Active = 1
                            INNER JOIN Trader ON Trader.IdTrader = Portfolio.IdTrader AND Trader.Active = 1
                            WHERE Trader.IdUser = @IdUser) AND DividendCalendar.DataCom BETWEEN @StartDate AND @FinalDate ORDER BY DividendCalendar.DataCom ASC", filterStockType));
            }
            else
            {
                sql.Append(string.Format("WHERE {0} DataCom BETWEEN @StartDate AND @FinalDate ORDER BY DataCom ASC", filterStockType));
            }

            IEnumerable<DividendCalendarView> dividends = _unitOfWork.Connection.Query<DividendCalendarView>(sql.ToString(), new { StartDate = startDate, FinalDate = endDate, IdCountry = (int)countryEnum, IdUser = idUser }, _unitOfWork.Transaction);

            return dividends;
        }

        public IEnumerable<DividendCalendarView> GetByPaymentDateBySymbol(DateTime startDate, DateTime endDate, string symbol)
        {
            string sql = @"SELECT DISTINCT [DividendCalendarID]
                          ,DividendCalendar.[IdStock]
	                      ,Stock.Symbol AS StockName
                          ,Stock.MarketPrice AS MarketStockValue
                          ,DividendCalendar.[IdDividendType]
	                      ,DividendType.Name AS DividendType
                          ,DataCom
                          ,[PaymentDate]
                          ,[Value],
						  DividendType.Name,
                          PaymentDatepartiallyUndefined,
                          PaymentUndefined
                      FROM [DividendCalendar]
                      INNER JOIN DividendType ON DividendType.IdDividendType = DividendCalendar.IdDividendType
                      INNER JOIN Stock ON Stock.IdStock = DividendCalendar.IdStock
                      AND Stock.Symbol = @Symbol
					  WHERE PaymentDate BETWEEN @StartDate AND @FinalDate ORDER BY PaymentDate DESC";

            IEnumerable<DividendCalendarView> dividends = _unitOfWork.Connection.Query<DividendCalendarView>(sql, new { StartDate = startDate, FinalDate = endDate, Symbol = symbol }, _unitOfWork.Transaction);

            return dividends;
        }


        public IEnumerable<DividendCalendarView> GetByDataComBySymbol(DateTime startDate, DateTime endDate, string symbol)
        {
            string sql = @"SELECT DISTINCT [DividendCalendarID]
                          ,DividendCalendar.[IdStock]
	                      ,Stock.Symbol AS StockName
                          ,Stock.MarketPrice AS MarketStockValue
                          ,DividendCalendar.[IdDividendType]
	                      ,DividendType.Name AS DividendType
                          ,DataCom
                          ,[PaymentDate]
                          ,[Value],
						  DividendType.Name,
                          PaymentDatepartiallyUndefined,
                          PaymentUndefined
                      FROM [DividendCalendar]
                      INNER JOIN DividendType ON DividendType.IdDividendType = DividendCalendar.IdDividendType
                      INNER JOIN Stock ON Stock.IdStock = DividendCalendar.IdStock
					  WHERE DataCom BETWEEN @StartDate AND @FinalDate
                      AND Stock.Symbol = @Symbol
                      ORDER BY DataCom DESC";

            IEnumerable<DividendCalendarView> dividends = _unitOfWork.Connection.Query<DividendCalendarView>(sql, new { StartDate = startDate, FinalDate = endDate, Symbol = symbol }, _unitOfWork.Transaction);

            return dividends;
        }

        public IEnumerable<DividendCalendarView> GetAllByDataEx(DateTime dataCom)
        {
            string sql = @"SELECT DISTINCT DividendCalendar.[IdStock]
	                      ,Stock.Symbol AS StockName
                          ,DividendCalendar.[IdDividendType]
	                      ,DividendType.Name AS DividendType
                          ,DataCom
                          ,[PaymentDate]
                          ,[Value],
						  DividendType.Name,
                          PaymentDatepartiallyUndefined,
                          PaymentUndefined,
                          Stock.IdCountry
                      FROM [DividendCalendar]
                      INNER JOIN DividendType ON DividendType.IdDividendType = DividendCalendar.IdDividendType
                      INNER JOIN Stock ON Stock.IdStock = DividendCalendar.IdStock
					  WHERE DATEADD(DD,1,DataCom) = @DataCom AND PaymentDate IS NOT NULL";

            IEnumerable<DividendCalendarView> dividends = _unitOfWork.Connection.Query<DividendCalendarView>(sql, new { DataCom = dataCom }, _unitOfWork.Transaction);

            return dividends;
        }

        public IEnumerable<DividendCalendarView> GetByDataCom(DateTime dataCom)
        {
            string sql = @"SELECT DISTINCT DividendCalendar.[IdStock]
	                      ,Stock.Symbol AS StockName
                          ,DividendCalendar.[IdDividendType]
	                      ,DividendType.Name AS DividendType
                          ,DataCom
                          ,[PaymentDate]
                          ,[Value],
						  DividendType.Name,
                          PaymentDatepartiallyUndefined,
                          PaymentUndefined
                      FROM [DividendCalendar]
                      INNER JOIN DividendType ON DividendType.IdDividendType = DividendCalendar.IdDividendType
                      INNER JOIN Stock ON Stock.IdStock = DividendCalendar.IdStock
					  WHERE DataCom = @DataCom";

            IEnumerable<DividendCalendarView> dividends = _unitOfWork.Connection.Query<DividendCalendarView>(sql, new { DataCom = dataCom }, _unitOfWork.Transaction);

            return dividends;
        }

        public IEnumerable<DividendCalendarView> GetNextDataComByUser(DateTime dataComStartDate, DateTime dataComEndDate, string idUser)
        {
            string sql = @"SELECT DISTINCT DividendCalendar.[IdStock]
	                      ,Stock.Symbol AS StockName
                          ,Stock.MarketPrice AS MarketStockValue
                          ,DividendCalendar.[IdDividendType]
	                      ,DividendType.Name AS DividendType
                          ,DataCom
                          ,[PaymentDate]
                          ,[Value],
						  DividendType.Name,
                          PaymentDatepartiallyUndefined,
                          PaymentUndefined,
                          Logo.LogoImage Logo
                      FROM [DividendCalendar]
                      INNER JOIN DividendType ON DividendType.IdDividendType = DividendCalendar.IdDividendType
                      INNER JOIN Stock ON Stock.IdStock = DividendCalendar.IdStock
                      INNER JOIN Company ON Company.IdCompany = Stock.IdCompany
                      LEFT JOIN Logo ON Logo.IdLogo = Company.IdLogo
					  WHERE Stock.IdStock IN (SELECT Stock.IdStock FROM Stock
                            INNER JOIN Operation ON Operation.IdStock = Stock.IdStock AND Operation.Active = 1 AND Operation.IdOperationType = 1 AND Operation.NumberOfShares > 0
                            INNER JOIN Portfolio ON Portfolio.IdPortfolio = Operation.IdPortfolio AND Portfolio.Active = 1
                            INNER JOIN Trader ON Trader.IdTrader = Portfolio.IdTrader AND Trader.Active = 1
                            WHERE Trader.IdUser = @IdUser)
                     AND DataCom BETWEEN @DataComStartDate AND @DataComEndDate ORDER BY DividendCalendar.DataCom";

            IEnumerable<DividendCalendarView> dividends = _unitOfWork.Connection.Query<DividendCalendarView>(sql, new { DataComStartDate = dataComStartDate, DataComEndDate = dataComEndDate, IdUser = idUser }, _unitOfWork.Transaction);

            return dividends;
        }

    }
}
