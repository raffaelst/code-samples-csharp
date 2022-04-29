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
    public class CryptoCurrencyStatementViewRepository : Repository<CryptoCurrencyStatementView>, ICryptoCurrencyStatementViewRepository
    {
        private IUnitOfWork _unitOfWork;

        public CryptoCurrencyStatementViewRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<CryptoCurrencyStatementView> GetByCryptoPortfolio(Guid guidCryptoPortfolio)
        {
            string sql = @"select distinct CryptoCurrency.CryptoCurrencyGuid GuidCryptoCurrency, CryptoTransaction.GuidCryptoTransaction, CryptoCurrency.Name, CryptoCurrencyPerformance.TotalMarket, CryptoCurrencyPerformance.Total, CryptoCurrencyPerformance.PerformancePerc, CryptoCurrencyPerformance.Quantity,
                            CryptoCurrencyPerformance.AveragePrice,  case when IdFiatCurrency = 1 then CryptoCurrency.MarketPrice when IdFiatCurrency = 2 then CryptoCurrency.MarketPriceUSD else CryptoCurrency.MarketPriceEuro end as MarketPrice, CryptoCurrencyPerformance.NetValue,Logo.URL Logo, CryptoCurrencyPerformance.CalculationDate, CryptoPortfolio.IdFiatCurrency                            
                            from CryptoCurrencyPerformance
                            inner join CryptoCurrency on CryptoCurrency.CryptoCurrencyID = CryptoCurrencyPerformance.IdCryptoCurrency                            
                            inner join Logo on logo.IdLogo = CryptoCurrency.LogoID                            
                            inner join CryptoPortfolioPerformance on CryptoPortfolioPerformance.IdCryptoPortfolioPerformance = CryptoCurrencyPerformance.IdCryptoPortfolioPerformance
                            INNER JOIN CryptoPortfolio on CryptoPortfolio.IdCryptoPortfolio = CryptoPortfolioPerformance.IdCryptoPortfolio 
                            inner join CryptoTransaction on CryptoTransaction.IdCryptoCurrency = CryptoCurrency.CryptoCurrencyID and CryptoTransaction.IdCryptoPortfolio = CryptoPortfolio.IdCryptoPortfolio and CryptoTransaction.TransactionType = 1 and CryptoTransaction.Active = 1 
                            where CryptoPortfolio.GuidCryptoPortfolio = @GuidCryptoPortfolio and 
                            CryptoCurrencyPerformance.CalculationDate = (select top 1 max(pmax.CalculationDate) from CryptoPortfolioPerformance pmax where pmax.IdCryptoPortfolio = CryptoPortfolio.IdCryptoPortfolio ) 
                            order by CryptoCurrency.Name, CryptoCurrencyPerformance.CalculationDate desc
                            ";

            IEnumerable<CryptoCurrencyStatementView> portfolioStatementView = _unitOfWork.Connection.Query<CryptoCurrencyStatementView>(sql, new { GuidCryptoPortfolio = guidCryptoPortfolio }, _unitOfWork.Transaction);

            return portfolioStatementView;
        }

        public IEnumerable<CryptoCurrencyStatementView> GetByCryptoSubportfolio(Guid guidCryptoSubPortfolio)
        {
            string sql = @"select distinct CryptoCurrencyGuid GuidCryptoCurrency, CryptoTransaction.GuidCryptoTransaction, CryptoCurrency.Name, CryptoCurrencyPerformance.TotalMarket, CryptoCurrencyPerformance.Total, CryptoCurrencyPerformance.PerformancePerc, CryptoCurrencyPerformance.Quantity,
                            CryptoCurrencyPerformance.AveragePrice,  case when IdFiatCurrency = 1 then CryptoCurrency.MarketPrice when IdFiatCurrency = 2 then CryptoCurrency.MarketPriceUSD else CryptoCurrency.MarketPriceEuro end as MarketPrice, CryptoCurrencyPerformance.NetValue,Logo.URL Logo, CryptoCurrencyPerformance.CalculationDate, CryptoPortfolio.IdFiatCurrency                            
                            from CryptoSubPortfolioTransaction
                            inner join CryptoTransaction on CryptoTransaction.IdCryptoTransaction = CryptoSubPortfolioTransaction.IdCryptoTransaction and CryptoTransaction.TransactionType = 1  and CryptoTransaction.Active = 1 
                            inner join CryptoCurrencyPerformance on CryptoCurrencyPerformance.IdCryptoCurrency = CryptoTransaction.IdCryptoCurrency
                            inner join CryptoCurrency on CryptoCurrency.CryptoCurrencyID = CryptoCurrencyPerformance.IdCryptoCurrency                            
                            inner join Logo on logo.IdLogo = CryptoCurrency.LogoID                            
                            inner join CryptoPortfolioPerformance on CryptoPortfolioPerformance.IdCryptoPortfolioPerformance = CryptoCurrencyPerformance.IdCryptoPortfolioPerformance
                            INNER JOIN CryptoPortfolio on CryptoPortfolio.IdCryptoPortfolio = CryptoPortfolioPerformance.IdCryptoPortfolio 
                            inner join CryptoSubPortfolio on CryptoSubPortfolio.IdCryptoPortfolio = CryptoPortfolioPerformance.IdCryptoPortfolio and CryptoSubPortfolioTransaction.IdCryptoSubPortfolio = CryptoSubPortfolio.IdCryptoSubPortfolio 
                            where CryptoCurrencyPerformance.CalculationDate =  (select top 1 max(pmax.CalculationDate) from CryptoPortfolioPerformance pmax where pmax.IdCryptoPortfolio = CryptoSubPortfolio.IdCryptoPortfolio ) 
                            and CryptoSubPortfolio.GuidCryptoSubPortfolio = @GuidCryptoSubPortfolio
                            order by CryptoCurrency.Name, CryptoCurrencyPerformance.CalculationDate desc
                            ";

            IEnumerable<CryptoCurrencyStatementView> portfolioStatementView = _unitOfWork.Connection.Query<CryptoCurrencyStatementView>(sql, new { GuidCryptoSubPortfolio = guidCryptoSubPortfolio }, _unitOfWork.Transaction);

            return portfolioStatementView;
        }

        public CryptoCurrencyStatementView GetByGuidCurrency(Guid guidCryptoPortfolio, Guid guidCryptoCurrency)
        {
            string sql = @"with tbcryptocurrency as
                            (
                            select distinct CryptoCurrency.CryptoCurrencyGuid GuidCryptoCurrency, CryptoCurrency.CryptoCurrencyID IdCryptoCurrency, CryptoCurrency.Name, Logo.URL Logo,
							case when IdFiatCurrency = 1 then CryptoCurrency.MarketPrice when IdFiatCurrency = 2 then CryptoCurrency.MarketPriceUSD else CryptoCurrency.MarketPriceEuro end as MarketPrice
                                                        from CryptoCurrency	
														inner join CryptoPortfolio on CryptoPortfolio.GuidCryptoPortfolio = @GuidCryptoPortfolio
                                                        inner join Logo on logo.IdLogo = CryptoCurrency.LogoID
							                            where CryptoCurrency.CryptoCurrencyGuid = @GuidCryptoCurrency
                            ), tbPerformance as
                            (
                            select distinct CryptoCurrencyPerformance.IdCryptoCurrency, CryptoTransaction.GuidCryptoTransaction, CryptoCurrencyPerformance.TotalMarket, CryptoCurrencyPerformance.Total, CryptoCurrencyPerformance.PerformancePerc, CryptoCurrencyPerformance.Quantity,
                                                        CryptoCurrencyPerformance.AveragePrice, CryptoCurrencyPerformance.NetValue, CryptoCurrencyPerformance.CalculationDate 														
                                                        from CryptoCurrencyPerformance
                                                        inner join CryptoPortfolioPerformance on CryptoPortfolioPerformance.IdCryptoPortfolioPerformance = CryptoCurrencyPerformance.IdCryptoPortfolioPerformance
                                                        INNER JOIN CryptoPortfolio on CryptoPortfolio.IdCryptoPortfolio = CryptoPortfolioPerformance.IdCryptoPortfolio 
                                                        inner join CryptoTransaction on CryptoTransaction.IdCryptoCurrency = CryptoCurrencyPerformance.IdCryptoCurrency and CryptoTransaction.IdCryptoPortfolio = CryptoPortfolio.IdCryptoPortfolio and CryptoTransaction.TransactionType = 1 and CryptoTransaction.Active = 1
														inner join CryptoCurrency on CryptoCurrency.CryptoCurrencyID = CryptoCurrencyPerformance.IdCryptoCurrency
                                                        where CryptoPortfolio.GuidCryptoPortfolio = @GuidCryptoPortfolio and CryptoCurrency.CryptoCurrencyGuid = @GuidCryptoCurrency and  
                                                        CryptoCurrencyPerformance.CalculationDate = (select top 1 max(pmax.CalculationDate) from CryptoPortfolioPerformance pmax where pmax.IdCryptoPortfolio = CryptoPortfolio.IdCryptoPortfolio )                             
                            )
                            select tbcryptocurrency.GuidCryptoCurrency, isnull(tbPerformance.GuidCryptoTransaction,cast(0x0 as uniqueidentifier)) GuidCryptoTransaction, tbcryptocurrency.Name, isnull(tbPerformance.TotalMarket,0) TotalMarket, isnull(tbPerformance.Total,0) Total, isnull(tbPerformance.PerformancePerc,0) PerformancePerc, isnull(tbPerformance.Quantity,0) Quantity,
                            isnull(tbPerformance.AveragePrice,0) AveragePrice, tbcryptocurrency.MarketPrice, isnull(tbPerformance.NetValue,0) NetValue, tbcryptocurrency.Logo, isnull(tbPerformance.CalculationDate,cast(-53690 as datetime)) CalculationDate
                            from tbcryptocurrency
                            left join tbPerformance on tbcryptocurrency.IdCryptoCurrency = tbPerformance.IdCryptoCurrency ";

            IEnumerable<CryptoCurrencyStatementView> portfolioStatementView = _unitOfWork.Connection.Query<CryptoCurrencyStatementView>(sql, new { GuidCryptoPortfolio = guidCryptoPortfolio, GuidCryptoCurrency = guidCryptoCurrency }, _unitOfWork.Transaction);

            return portfolioStatementView.FirstOrDefault();
        }

        public IEnumerable<CryptoCurrencyStatementView> GetCryptoSummaryByPortfolioOrSubPortfolio(string idUser, string guidCryptoPortfolioSub)
        {
            string sql = @"	SELECT  * 
                            FROM 
                            (
                            select distinct CryptoTransaction.GuidCryptoTransaction, CryptoTransaction.IdCryptoTransaction, Logo.LogoImage Logo, CryptoCurrency.CryptoCurrencyGuid GuidCryptoCurrency, CryptoCurrency.Name, CryptoTransaction.AveragePrice, CryptoTransaction.Quantity, case when IdFiatCurrency = 1 then CryptoCurrency.MarketPrice when IdFiatCurrency = 2 then CryptoCurrency.MarketPriceUSD else CryptoCurrency.MarketPriceEuro end as MarketPrice, CryptoCurrencyPerformance.PerformancePerc, CryptoCurrencyPerformance.PerformancePercTWR, CryptoCurrencyPerformance.CalculationDate, CryptoPortfolio.IdFiatCurrency                         
                            from CryptoCurrencyPerformance
                            inner join CryptoCurrency on CryptoCurrency.CryptoCurrencyID = CryptoCurrencyPerformance.IdCryptoCurrency		                    
		                    inner join Logo on logo.IdLogo = CryptoCurrency.LogoID
                            inner join CryptoPortfolioPerformance on CryptoPortfolioPerformance.IdCryptoPortfolioPerformance = CryptoCurrencyPerformance.IdCryptoPortfolioPerformance
                            inner join CryptoPortfolio on CryptoPortfolio.IdCryptoPortfolio = CryptoPortfolioPerformance.IdCryptoPortfolio
                            inner join CryptoTransaction on CryptoTransaction.IdCryptoCurrency = CryptoCurrency.CryptoCurrencyID and CryptoTransaction.IdCryptoPortfolio = CryptoPortfolio.IdCryptoPortfolio
                            inner join Trader on Trader.IdTrader = CryptoPortfolio.IdTrader
                            where CryptoPortfolio.Active = 1 and Trader.IdUser = @IdUser and 
							CryptoCurrencyPerformance.CalculationDate = (select top 1 max(pmax.CalculationDate) from CryptoPortfolioPerformance pmax where pmax.IdCryptoPortfolio = CryptoPortfolio.IdCryptoPortfolio)
                            and CryptoTransaction.TransactionType = 1 and CryptoTransaction.Active = 1 and CryptoPortfolio.GuidCryptoPortfolio = @GuidCryptoPortfolioSub
							union all
							select distinct CryptoTransaction.GuidCryptoTransaction, CryptoTransaction.IdCryptoTransaction, Logo.LogoImage Logo, CryptoCurrency.CryptoCurrencyGuid GuidCryptoCurrency, CryptoCurrency.Name, CryptoTransaction.AveragePrice, CryptoTransaction.Quantity, case when IdFiatCurrency = 1 then CryptoCurrency.MarketPrice when IdFiatCurrency = 2 then CryptoCurrency.MarketPriceUSD else CryptoCurrency.MarketPriceEuro end as MarketPrice, CryptoCurrencyPerformance.PerformancePerc, CryptoCurrencyPerformance.PerformancePercTWR, CryptoCurrencyPerformance.CalculationDate, CryptoPortfolio.IdFiatCurrency                           
                            from CryptoCurrencyPerformance
                            inner join CryptoCurrency on CryptoCurrency.CryptoCurrencyID = CryptoCurrencyPerformance.IdCryptoCurrency
		                    inner join Logo on logo.IdLogo = CryptoCurrency.LogoID
                            inner join CryptoPortfolioPerformance on CryptoPortfolioPerformance.IdCryptoPortfolioPerformance = CryptoCurrencyPerformance.IdCryptoPortfolioPerformance
                            inner join CryptoSubPortfolio on CryptoSubPortfolio.IdCryptoPortfolio = CryptoPortfolioPerformance.IdCryptoPortfolio
							inner join CryptoPortfolio on CryptoPortfolio.IdCryptoPortfolio = CryptoPortfolioPerformance.IdCryptoPortfolio
                            inner join CryptoTransaction on CryptoTransaction.IdCryptoCurrency = CryptoCurrency.CryptoCurrencyID and CryptoTransaction.IdCryptoPortfolio = CryptoSubPortfolio.IdCryptoPortfolio
                            inner join Trader on Trader.IdTrader = CryptoPortfolio.IdTrader
                            where CryptoSubPortfolio.Active = 1 and Trader.IdUser = @IdUser and CryptoCurrencyPerformance.CalculationDate = (select top 1 max(pmax.CalculationDate) from CryptoPortfolioPerformance pmax where pmax.IdCryptoPortfolio = CryptoPortfolio.IdCryptoPortfolio)
                            and CryptoTransaction.TransactionType = 1 and CryptoTransaction.Active = 1 
                            and CryptoSubPortfolio.GuidCryptoSubPortfolio = @GuidCryptoPortfolioSub
							and CryptoTransaction.IdCryptoCurrency in (
			                    select CryptoCurrency.CryptoCurrencyID from CryptoSubPortfolioTransaction
			                    inner join CryptoSubPortfolio on CryptoSubPortfolio.IdCryptoSubPortfolio = CryptoSubPortfolioTransaction.IdCryptoSubPortfolio
			                    inner join CryptoTransaction on CryptoTransaction.IdCryptoTransaction = CryptoSubPortfolioTransaction.IdCryptoTransaction
		                        inner join CryptoCurrency on CryptoCurrency.CryptoCurrencyID = CryptoTransaction.IdCryptoCurrency
			                    where CryptoSubPortfolio.GuidCryptoSubPortfolio = @GuidCryptoPortfolioSub
		                    )
                            ) resulttable
                            order by Name asc";

            var portfolios = _unitOfWork.Connection.Query<CryptoCurrencyStatementView>(sql, new { IdUser = idUser, GuidCryptoPortfolioSub = guidCryptoPortfolioSub }, _unitOfWork.Transaction);

            return portfolios;
        }

    }
}
