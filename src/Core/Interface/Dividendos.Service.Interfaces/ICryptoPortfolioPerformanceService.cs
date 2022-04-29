using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Service.Interface
{
    public interface ICryptoPortfolioPerformanceService : IBaseService
    {
        ResultServiceObject<CryptoPortfolioPerformance> GetByCalculationDate(long idCryptoPortfolio, DateTime calculationDate);
        ResultServiceObject<CryptoPortfolioPerformance> GetPreviousDate(long idCryptoPortfolio, DateTime calculationDate);
        ResultServiceObject<CryptoPortfolioPerformance> Insert(CryptoPortfolioPerformance cryptoPortfolioPerformance);
        void CalculatePerformance(long idCryptoPortfolio,
                                ICryptoPortfolioService _cryptoPortfolioService,
                                ICryptoCurrencyService _cryptoCurrencyService,
                                ICryptoTransactionService _cryptoTransactionService,
                                ICryptoCurrencyPerformanceService _cryptoCurrencyPerformanceService,
                                decimal totalLossProfit = 0);
    }
}
