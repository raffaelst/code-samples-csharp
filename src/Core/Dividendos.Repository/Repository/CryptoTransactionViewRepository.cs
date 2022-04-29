using Dapper;
using Dividendos.Entity.Views;
using Dividendos.Repository.GenericRepository;
using Dividendos.Repository.Interface;
using Dividendos.Repository.Interface.UoW;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Repository
{
    public class CryptoTransactionViewRepository : Repository<CryptoTransactionDetailsView>, ICryptoTransactionViewRepository
    {
        private IUnitOfWork _unitOfWork;

        public CryptoTransactionViewRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

		public IEnumerable<CryptoTransactionDetailsView> GetDetailsByIdCryptoPortfolio(long idCryptoPortfolio, int transactionType, DateTime? startDate, DateTime? endDate)
		{
			dynamic queryParams = new ExpandoObject();
			queryParams.IdCryptoPortfolio = idCryptoPortfolio;
			queryParams.TransactionType = transactionType;

			StringBuilder sbQuery = new StringBuilder();

			sbQuery.Append(@"select distinct CryptoTransactionItem.GuidCryptoTransactionItem, CryptoTransactionItem.IdCryptoTransactionItem, CryptoTransactionItem.EventDate, CryptoTransactionItem.Quantity, CryptoTransactionItem.AveragePrice, case when (CryptoTransactionItem.AcquisitionPrice = 0 or CryptoTransactionItem.AcquisitionPrice is null) then CryptoCurrencyPerformance.AveragePrice else  CryptoTransactionItem.AcquisitionPrice end as AcquisitionPrice, CryptoPortfolio.IdCryptoPortfolio, CryptoCurrency.CryptoCurrencyGuid GuidCryptoCurrency, CryptoCurrency.Name,Logo.URL Logo, CryptoTransactionItem.Exchange   
		                    from CryptoTransactionItem 
		                    inner join CryptoTransaction on CryptoTransaction.IdCryptoTransaction = CryptoTransactionItem.IdCryptoTransaction
		                    inner join CryptoCurrency on CryptoCurrency.CryptoCurrencyID = CryptoTransactionItem.IdCryptoCurrency		                    
		                    inner join Logo on logo.IdLogo = CryptoCurrency.LogoID		                    
		                    inner join CryptoPortfolio on CryptoPortfolio.IdCryptoPortfolio = CryptoTransaction.IdCryptoPortfolio
							left join CryptoPortfolioPerformance on CryptoPortfolioPerformance.IdCryptoPortfolio = CryptoPortfolio.IdCryptoPortfolio and CryptoPortfolioPerformance.CalculationDate = CONVERT(date,CryptoTransactionItem.EventDate) 
							left join CryptoCurrencyPerformance on CryptoCurrencyPerformance.IdCryptoPortfolioPerformance = CryptoPortfolioPerformance.IdCryptoPortfolioPerformance and CryptoCurrencyPerformance.IdCryptoCurrency = CryptoTransactionItem.IdCryptoCurrency
		                    where CryptoTransactionItem.TransactionType = @TransactionType and CryptoTransactionItem.Active = 1 
                            and CryptoTransaction.IdCryptoPortfolio = @IdCryptoPortfolio ");

			if (startDate.HasValue && endDate.HasValue)
			{
				sbQuery.AppendFormat("and CONVERT(date,CryptoTransactionItem.EventDate) between @StartDate and @EndDate ");
				queryParams.StartDate = startDate.Value;
				queryParams.EndDate = endDate.Value;
			}

			sbQuery.Append("order by CryptoCurrency.Name, CryptoTransactionItem.EventDate desc");
			string query = sbQuery.ToString();

			var portfolios = _unitOfWork.Connection.Query<CryptoTransactionDetailsView>(query, (object)queryParams, _unitOfWork.Transaction);

			return portfolios;
		}

		public IEnumerable<CryptoTransactionDetailsView> GetDetailsByIdCryptoSubportfolio(long idCryptoPortfolio, long idCryptoSubPortfolio, int transactionType, DateTime? startDate, DateTime? endDate)
		{
			dynamic queryParams = new ExpandoObject();
			queryParams.IdCryptoPortfolio = idCryptoPortfolio;
			queryParams.IdCryptoSubPortfolio = idCryptoSubPortfolio;
			queryParams.TransactionType = transactionType;

			StringBuilder sbQuery = new StringBuilder();

			sbQuery.Append(@"select distinct CryptoTransactionItem.GuidCryptoTransactionItem, CryptoTransactionItem.IdCryptoTransactionItem, CryptoTransactionItem.EventDate, CryptoTransactionItem.Quantity, CryptoTransactionItem.AveragePrice, case when (CryptoTransactionItem.AcquisitionPrice = 0 or CryptoTransactionItem.AcquisitionPrice is null) then CryptoCurrencyPerformance.AveragePrice else  CryptoTransactionItem.AcquisitionPrice end as AcquisitionPrice, CryptoPortfolio.IdCryptoPortfolio, CryptoCurrency.CryptoCurrencyGuid GuidCryptoCurrency, CryptoCurrency.Name,Logo.URL Logo, CryptoTransactionItem.Exchange   
		                    from CryptoTransactionItem 
		                    inner join CryptoTransaction on CryptoTransaction.IdCryptoTransaction = CryptoTransactionItem.IdCryptoTransaction
		                    inner join CryptoCurrency on CryptoCurrency.CryptoCurrencyID = CryptoTransactionItem.IdCryptoCurrency		                    
		                    inner join Logo on logo.IdLogo = CryptoCurrency.LogoID		                    
		                    inner join CryptoPortfolio on CryptoPortfolio.IdCryptoPortfolio = CryptoTransaction.IdCryptoPortfolio
							left join CryptoPortfolioPerformance on CryptoPortfolioPerformance.IdCryptoPortfolio = CryptoPortfolio.IdCryptoPortfolio and CryptoPortfolioPerformance.CalculationDate = CONVERT(date,CryptoTransactionItem.EventDate) 
							left join CryptoCurrencyPerformance on CryptoCurrencyPerformance.IdCryptoPortfolioPerformance = CryptoPortfolioPerformance.IdCryptoPortfolioPerformance and CryptoCurrencyPerformance.IdCryptoCurrency = CryptoTransactionItem.IdCryptoCurrency
		                    where CryptoTransactionItem.TransactionType = @TransactionType and CryptoTransactionItem.Active = 1 
		                    and CryptoTransaction.IdCryptoCurrency in (
			                    select CryptoCurrency.CryptoCurrencyID from CryptoSubPortfolioTransaction
			                    inner join CryptoSubPortfolio on CryptoSubPortfolio.IdCryptoSubPortfolio = CryptoSubPortfolioTransaction.IdCryptoSubPortfolio
			                    inner join CryptoTransaction on CryptoTransaction.IdCryptoTransaction = CryptoSubPortfolioTransaction.IdCryptoTransaction
		                        inner join CryptoCurrency on CryptoCurrency.CryptoCurrencyID = CryptoTransaction.IdCryptoCurrency
			                    where CryptoSubPortfolio.IdCryptoSubPortfolio = @IdCryptoSubPortfolio
		                    ) 
                            and CryptoTransaction.IdCryptoPortfolio = @IdCryptoPortfolio ");

			if (startDate.HasValue && endDate.HasValue)
			{
				sbQuery.AppendFormat("and CONVERT(date,CryptoTransactionItem.EventDate) between @StartDate and @EndDate ");
				queryParams.StartDate = startDate.Value;
				queryParams.EndDate = endDate.Value;
			}

			sbQuery.Append("order by CryptoCurrency.Name, CryptoTransactionItem.EventDate desc");
			string query = sbQuery.ToString();

			var portfolios = _unitOfWork.Connection.Query<CryptoTransactionDetailsView>(query, (object)queryParams, _unitOfWork.Transaction);

			return portfolios;
		}

	}
}
