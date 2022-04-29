using Dapper.Contrib.Extensions;
using Dividendos.Entity.Entities;
using Dividendos.Repository.GenericRepository;
using Dividendos.Repository.Interface;
using Dividendos.Repository.Interface.GenericRepository;
using Dividendos.Repository.Interface.UoW;
using Dapper;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Dividendos.Repository.Repository
{
    public class CryptoPortfolioPerformanceRepository : Repository<CryptoPortfolioPerformance>, ICryptoPortfolioPerformanceRepository
    {
        private IUnitOfWork _unitOfWork;

        public CryptoPortfolioPerformanceRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public CryptoPortfolioPerformance GetPreviousDate(long idCryptoPortfolio, DateTime calculationDate)
        {
            CryptoPortfolioPerformance cryptoPortfolioPerformance = null;
            string sql = @"SELECT IdCryptoPortfolioPerformance,IdCryptoPortfolio,Total,TotalMarket,PerformancePerc,CalculationDate,LastUpdatedDate,NetValue,PerformancePercTWR,NetValueTWR,GuidCryptoPortfolioPerformance
                           FROM CryptoPortfolioPerformance
                           WHERE IdCryptoPortfolio = @IdCryptoPortfolio and CalculationDate < @CalculationDate order by CalculationDate desc ";

            IEnumerable<CryptoPortfolioPerformance> portfoliosPerformance = _unitOfWork.Connection.Query<CryptoPortfolioPerformance>(sql, new { IdCryptoPortfolio = idCryptoPortfolio, CalculationDate = calculationDate }, _unitOfWork.Transaction);

            cryptoPortfolioPerformance = portfoliosPerformance.FirstOrDefault();

            return cryptoPortfolioPerformance;
        }
    }
}
