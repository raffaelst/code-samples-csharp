using Dividendos.Entity.Entities;
using Dividendos.Repository.GenericRepository;
using Dividendos.Repository.Interface;
using Dividendos.Repository.Interface.UoW;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Linq;
using Dividendos.Entity.Views;
using System.Dynamic;

namespace Dividendos.Repository.Repository
{
    public class CryptoPortfolioViewRepository : Repository<CryptoPortfolioView>, ICryptoPortfolioViewRepository
    {
        private IUnitOfWork _unitOfWork;

        public CryptoPortfolioViewRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<CryptoPortfolioView> GetByUser(string idUser)
        {
            string sql = @"with tbPortPerf as
                            (
                            select perf.IdCryptoPortfolio, perf.TotalMarket, perf.NetValue, perf.Total, perf.PerformancePercTWR, perf.PerformancePerc, perf.CalculationDate,
                            isnull(perf.NetValueTWR, 0) LatestNetValue, (0) PreviousNetValue, (0) PreviousTotalMarket 
                            from CryptoPortfolioPerformance perf
                            inner join CryptoPortfolio on CryptoPortfolio.IdCryptoPortfolio = perf.IdCryptoPortfolio
                            inner join Trader on Trader.IdTrader = CryptoPortfolio.IdTrader
                            inner join
                            (select CryptoPortfolioPerformance.IdCryptoPortfolio, max (CryptoPortfolioPerformance.CalculationDate) CalculationDate
                            from CryptoPortfolioPerformance
                            group by CryptoPortfolioPerformance.IdCryptoPortfolio) perfmax
                            on perfmax.IdCryptoPortfolio = perf.IdCryptoPortfolio and perfmax.CalculationDate = perf.CalculationDate
                            where Trader.IdUser = @IdUser and CryptoPortfolio.Active = 1 and Trader.Active = 1 


                            ),
                            tbSubPortPerf as
                            (select CryptoSubPortfolio.GuidCryptoSubPortfolio, CryptoSubPortfolio.IdCryptoPortfolio, CryptoSubPortfolio.IdCryptoSubPortfolio, CryptoSubPortfolio.Name, sum(CryptoCurrencyPerformance.TotalMarket) TotalMarket, sum(CryptoCurrencyPerformance.Total) Total, sum(CryptoCurrencyPerformance.NetValue) NetValue, CryptoPortfolioPerformance.CalculationDate,
                            isnull(sum(CryptoCurrencyPerformance.NetValueTWR), 0) LatestNetValue, (0) PreviousNetValue,							
								(
								SELECT TOP 1  TotalMarket PreviousTotalMarket From
								(select top 2 sum(CryptoCurrencyPerformance.TotalMarket) TotalMarket, CryptoPortfolioPerformance.CalculationDate from CryptoSubPortfolio sub
								inner join CryptoSubPortfolioTransaction on CryptoSubPortfolioTransaction.IdCryptoSubPortfolio = sub.IdCryptoSubPortfolio
								inner join CryptoTransaction on CryptoTransaction.IdCryptoTransaction = CryptoSubPortfolioTransaction.IdCryptoTransaction  and CryptoTransaction.IdCryptoPortfolio = CryptoSubPortfolio.IdCryptoPortfolio
								inner join CryptoPortfolioPerformance on CryptoPortfolioPerformance.IdCryptoPortfolio = sub.IdCryptoPortfolio
								inner join CryptoCurrencyPerformance on CryptoCurrencyPerformance.IdCryptoPortfolioPerformance = CryptoPortfolioPerformance.IdCryptoPortfolioPerformance
								and CryptoCurrencyPerformance.IdCryptoCurrency = CryptoTransaction.IdCryptoCurrency
								inner join CryptoPortfolio on CryptoPortfolio.IdCryptoPortfolio = sub.IdCryptoPortfolio
								inner join Trader on Trader.IdTrader = CryptoPortfolio.IdTrader
								where Trader.IdUser = @IdUser 
								and sub.IdCryptoSubPortfolio = CryptoSubPortfolio.IdCryptoSubPortfolio
								and sub.Active = 1 and CryptoPortfolio.Active =1 and Trader.Active = 1 
								group by CryptoPortfolioPerformance.CalculationDate
								ORDER BY CryptoPortfolioPerformance.CalculationDate DESC ) x order by x.CalculationDate asc 
								) PreviousTotalMarket							
                            from CryptoSubPortfolio
                            inner join CryptoSubPortfolioTransaction on CryptoSubPortfolioTransaction.IdCryptoSubPortfolio = CryptoSubPortfolio.IdCryptoSubPortfolio
                            inner join CryptoTransaction on CryptoTransaction.IdCryptoTransaction = CryptoSubPortfolioTransaction.IdCryptoTransaction  and CryptoTransaction.IdCryptoPortfolio = CryptoSubPortfolio.IdCryptoPortfolio
                            inner join CryptoPortfolioPerformance on CryptoPortfolioPerformance.IdCryptoPortfolio = CryptoSubPortfolio.IdCryptoPortfolio
                            inner join CryptoCurrencyPerformance on CryptoCurrencyPerformance.IdCryptoPortfolioPerformance = CryptoPortfolioPerformance.IdCryptoPortfolioPerformance
                            and CryptoCurrencyPerformance.IdCryptoCurrency = CryptoTransaction.IdCryptoCurrency
                            inner join CryptoPortfolio on CryptoPortfolio.IdCryptoPortfolio = CryptoSubPortfolio.IdCryptoPortfolio
                            inner join Trader on Trader.IdTrader = CryptoPortfolio.IdTrader
                            inner join
                            (
                            select CryptoSubPortfolio.IdCryptoSubPortfolio,  max(CryptoCurrencyPerformance.CalculationDate) CalculationDate from CryptoSubPortfolio
                            inner join CryptoSubPortfolioTransaction on CryptoSubPortfolioTransaction.IdCryptoSubPortfolio = CryptoSubPortfolio.IdCryptoSubPortfolio
                            inner join CryptoTransaction on CryptoTransaction.IdCryptoTransaction = CryptoSubPortfolioTransaction.IdCryptoTransaction  and CryptoTransaction.IdCryptoPortfolio = CryptoSubPortfolio.IdCryptoPortfolio
                            inner join CryptoPortfolioPerformance on CryptoPortfolioPerformance.IdCryptoPortfolio = CryptoSubPortfolio.IdCryptoPortfolio
                            inner join CryptoCurrencyPerformance on CryptoCurrencyPerformance.IdCryptoPortfolioPerformance = CryptoPortfolioPerformance.IdCryptoPortfolioPerformance
                            and CryptoCurrencyPerformance.IdCryptoCurrency = CryptoTransaction.IdCryptoCurrency
                            inner join CryptoPortfolio on CryptoPortfolio.IdCryptoPortfolio = CryptoSubPortfolio.IdCryptoPortfolio
                            inner join Trader on Trader.IdTrader = CryptoPortfolio.IdTrader
                            where Trader.IdUser = @IdUser and CryptoSubPortfolio.Active = 1 and CryptoPortfolio.Active = 1 and Trader.Active = 1 
                            group by CryptoSubPortfolio.IdCryptoSubPortfolio 
                            ) submax
                            on submax.IdCryptoSubPortfolio = CryptoSubPortfolio.IdCryptoSubPortfolio and submax.CalculationDate = CryptoPortfolioPerformance.CalculationDate
                            where Trader.IdUser = @IdUser  and CryptoSubPortfolio.Active = 1  and Trader.Active = 1 
                            group by CryptoSubPortfolio.GuidCryptoSubPortfolio,CryptoSubPortfolio.IdCryptoPortfolio,CryptoSubPortfolio.IdCryptoSubPortfolio, CryptoSubPortfolio.Name, CryptoPortfolioPerformance.CalculationDate
                            )
                            select CryptoPortfolio.IdCryptoPortfolio, Trader.LastSync, CryptoPortfolio.GuidCryptoPortfolio GuidCryptoPortfolioSubPortfolio, CryptoPortfolio.Name, tbPortPerf.TotalMarket,tbPortPerf.PreviousTotalMarket, tbPortPerf.Total, tbPortPerf.NetValue, tbPortPerf.LatestNetValue, tbPortPerf.PreviousNetValue, tbPortPerf.PerformancePerc, tbPortPerf.PerformancePercTWR, tbPortPerf.CalculationDate, 1 ordering, 1 IsPortfolio, NULL ParentPortfolio, CryptoPortfolio.IdFiatCurrency
                            from tbPortPerf
                            inner join CryptoPortfolio on CryptoPortfolio.IdCryptoPortfolio = tbPortPerf.IdCryptoPortfolio
                            inner join Trader on Trader.IdTrader = CryptoPortfolio.IdTrader                            
                            where Trader.IdUser = @IdUser 
                            union all 
                            select CryptoPortfolio.IdCryptoPortfolio, Trader.LastSync, tbSubPortPerf.GuidCryptoSubPortfolio GuidCryptoPortfolioSubPortfolio, tbSubPortPerf.Name, tbSubPortPerf.TotalMarket,tbSubPortPerf.PreviousTotalMarket, tbSubPortPerf.Total, tbSubPortPerf.NetValue, tbSubPortPerf.LatestNetValue, tbSubPortPerf.PreviousNetValue, case when tbSubPortPerf.Total = 0 then 0 else (tbSubPortPerf.NetValue / tbSubPortPerf.Total) end as PerformancePerc, case when tbSubPortPerf.PreviousTotalMarket = 0 then 0 else (tbSubPortPerf.LatestNetValue / tbSubPortPerf.PreviousTotalMarket) end as PerformancePercTWR, tbSubPortPerf.CalculationDate, 2 ordering, 0 IsPortfolio, CryptoPortfolio.GuidCryptoPortfolio ParentPortfolio, CryptoPortfolio.IdFiatCurrency
                            from tbSubPortPerf
                            inner join CryptoPortfolio on CryptoPortfolio.IdCryptoPortfolio = tbSubPortPerf.IdCryptoPortfolio
                            inner join Trader on Trader.IdTrader = CryptoPortfolio.IdTrader                            
                            where Trader.IdUser = @IdUser and Trader.Active = 1 
                            order by ordering asc  ";

            IEnumerable<CryptoPortfolioView> portfolios = _unitOfWork.Connection.Query<CryptoPortfolioView>(sql, new { IdUser = idUser }, _unitOfWork.Transaction);

            return portfolios;
        }

        public IEnumerable<CryptoSubportfolioItemView> GetByCryptoPortfolio(Guid guidCryptoPortfolio)
        {
            string sql = @"select cryptotransaction.GuidCryptoTransaction, CryptoCurrency.Name CryptoName, Logo.URL Logo from cryptotransaction
                            inner join cryptocurrency on cryptocurrency.CryptoCurrencyID = CryptoTransaction.IdCryptoCurrency
                            inner join Logo on Logo.IdLogo = CryptoCurrency.LogoID
                            inner join CryptoPortfolio on CryptoPortfolio.IdCryptoPortfolio = CryptoTransaction.IdCryptoPortfolio
                            where CryptoPortfolio.GuidCryptoPortfolio = @GuidCryptoPortfolio and CryptoTransaction.TransactionType = 1 and CryptoTransaction.Active = 1
                            order by CryptoCurrency.Name";

            var cryptoItems = _unitOfWork.Connection.Query<CryptoSubportfolioItemView>(sql, new { GuidCryptoPortfolio = guidCryptoPortfolio }, _unitOfWork.Transaction);

            return cryptoItems;
        }

        public IEnumerable<CryptoSubportfolioItemView> GetBySubCryptoPortfolio(long idCryptoPortfolio, long idCryptoSubPortfolio)
        {
            string sql = @"select cryptotransaction.GuidCryptoTransaction, CryptoCurrency.Name CryptoName, Logo.URL Logo, case when CryptoSubPortfolioTransaction.IdCryptoSubPortfolioTransaction is null then 0 else 1 end Selected  from cryptotransaction
	                        inner join cryptocurrency on cryptocurrency.CryptoCurrencyID = CryptoTransaction.IdCryptoCurrency
	                        inner join Logo on Logo.IdLogo = CryptoCurrency.LogoID
	                        inner join CryptoPortfolio on CryptoPortfolio.IdCryptoPortfolio = CryptoTransaction.IdCryptoPortfolio
	                        left join CryptoSubPortfolioTransaction on CryptoSubPortfolioTransaction.IdCryptoTransaction = CryptoTransaction.IdCryptoTransaction and CryptoSubPortfolioTransaction.IdCryptoPortfolio = CryptoPortfolio.IdCryptoPortfolio and CryptoSubPortfolioTransaction.IdCryptoSubPortfolio = @IdCryptoSubPortfolio 
	                        where CryptoPortfolio.IdCryptoPortfolio = @IdCryptoPortfolio and CryptoTransaction.TransactionType = 1 and CryptoTransaction.Active = 1
	                        order by CryptoCurrency.Name";

            var cryptoItems = _unitOfWork.Connection.Query<CryptoSubportfolioItemView>(sql, new { IdCryptoPortfolio = idCryptoPortfolio, IdCryptoSubPortfolio = idCryptoSubPortfolio }, _unitOfWork.Transaction);

            return cryptoItems;
        }


    }
}
