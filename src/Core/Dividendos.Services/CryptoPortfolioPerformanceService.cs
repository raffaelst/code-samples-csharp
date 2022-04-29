using Dividendos.Entity.Entities;
using Dividendos.Entity.Enum;
using Dividendos.Entity.Model;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Service
{
    public class CryptoPortfolioPerformanceService : BaseService, ICryptoPortfolioPerformanceService
    {
        public CryptoPortfolioPerformanceService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public ResultServiceObject<CryptoPortfolioPerformance> Insert(CryptoPortfolioPerformance cryptoPortfolioPerformance)
        {
            ResultServiceObject<CryptoPortfolioPerformance> resultService = new ResultServiceObject<CryptoPortfolioPerformance>();
            cryptoPortfolioPerformance.GuidCryptoPortfolioPerformance = Guid.NewGuid();
            cryptoPortfolioPerformance.LastUpdatedDate = DateTime.Now;
            cryptoPortfolioPerformance.IdCryptoPortfolioPerformance = _uow.CryptoPortfolioPerformanceRepository.Insert(cryptoPortfolioPerformance);
            resultService.Value = cryptoPortfolioPerformance;

            return resultService;
        }

        public ResultServiceObject<CryptoPortfolioPerformance> Update(CryptoPortfolioPerformance cryptoPortfolioPerformance)
        {
            ResultServiceObject<CryptoPortfolioPerformance> resultService = new ResultServiceObject<CryptoPortfolioPerformance>();
            cryptoPortfolioPerformance.LastUpdatedDate = DateTime.Now;
            resultService.Value = _uow.CryptoPortfolioPerformanceRepository.Update(cryptoPortfolioPerformance);

            return resultService;
        }

        public ResultServiceObject<CryptoPortfolioPerformance> GetByCalculationDate(long idCryptoPortfolio, DateTime calculationDate)
        {
            ResultServiceObject<CryptoPortfolioPerformance> resultService = new ResultServiceObject<CryptoPortfolioPerformance>();

            IEnumerable<CryptoPortfolioPerformance> portfolioPerformance = _uow.CryptoPortfolioPerformanceRepository.Select(p => p.IdCryptoPortfolio == idCryptoPortfolio && p.CalculationDate == calculationDate.Date);

            resultService.Value = portfolioPerformance.FirstOrDefault();

            return resultService;
        }

        public ResultServiceObject<CryptoPortfolioPerformance> GetPreviousDate(long idCryptoPortfolio, DateTime calculationDate)
        {
            ResultServiceObject<CryptoPortfolioPerformance> resultService = new ResultServiceObject<CryptoPortfolioPerformance>();

            CryptoPortfolioPerformance portfolioPerformance = _uow.CryptoPortfolioPerformanceRepository.GetPreviousDate(idCryptoPortfolio, calculationDate);

            resultService.Value = portfolioPerformance;

            return resultService;
        }

        public void CalculatePerformance(long idCryptoPortfolio,
                                ICryptoPortfolioService _cryptoPortfolioService,
                                ICryptoCurrencyService _cryptoCurrencyService,
                                ICryptoTransactionService _cryptoTransactionService,
                                ICryptoCurrencyPerformanceService _cryptoCurrencyPerformanceService,
                                decimal totalLossProfit = 0)
        {
            ResultServiceObject<CryptoPortfolio> resultCryptoPortfolio = _cryptoPortfolioService.GetById(idCryptoPortfolio);

            if (resultCryptoPortfolio.Success && resultCryptoPortfolio.Value != null)
            {
                FiatCurrencyEnum fiatCurrencyEnum = (FiatCurrencyEnum)resultCryptoPortfolio.Value.IdFiatCurrency;
                DateTime calculationDate = DateTime.Now.Date;
                ResultServiceObject<IEnumerable<CryptoTransaction>> resultCryptoTransaction = _cryptoTransactionService.GetByPortfolioTransactionType(idCryptoPortfolio, 1);
                ResultServiceObject<CryptoPortfolioPerformance> resultCryptoPortfolioPerformance = GetByCalculationDate(idCryptoPortfolio, calculationDate);

                if (resultCryptoTransaction.Success && resultCryptoPortfolioPerformance.Success)
                {
                    decimal total = 0;
                    decimal totalMarket = 0;
                    decimal netValue = 0;
                    IEnumerable<CryptoTransaction> transactions = resultCryptoTransaction.Value;

                    if (transactions != null && transactions.Count() > 0)
                    {
                        List<CryptoCurrencyPerformance> cryptoCurrencyPerformances = new List<CryptoCurrencyPerformance>();

                        foreach (CryptoTransaction transaction in transactions)
                        {
                            CryptoCurrency cryptoCurrency = _cryptoCurrencyService.GetById(transaction.IdCryptoCurrency).Value;

                            if (cryptoCurrency != null)
                            {
                                decimal marketPrice = 0;

                                switch (fiatCurrencyEnum)
                                {   
                                    case FiatCurrencyEnum.BRL:
                                        marketPrice = cryptoCurrency.MarketPrice;
                                        break;
                                    case FiatCurrencyEnum.USD:
                                        marketPrice = cryptoCurrency.MarketPriceUSD;
                                        break;
                                    case FiatCurrencyEnum.EURO:
                                        marketPrice = cryptoCurrency.MarketPriceEuro;
                                        break;
                                    default:
                                        break;
                                }

                                CryptoCurrencyPerformance cryptoCurrencyPerformance = new CryptoCurrencyPerformance();
                                cryptoCurrencyPerformance.AveragePrice = transaction.AveragePrice;
                                cryptoCurrencyPerformance.IdCryptoCurrency = cryptoCurrency.CryptoCurrencyID;
                                cryptoCurrencyPerformance.IdCryptoPortfolio = idCryptoPortfolio;
                                cryptoCurrencyPerformance.Quantity = transaction.Quantity;
                                cryptoCurrencyPerformance.Total = cryptoCurrencyPerformance.AveragePrice * cryptoCurrencyPerformance.Quantity;
                                cryptoCurrencyPerformance.TotalMarket = marketPrice * cryptoCurrencyPerformance.Quantity;
                                cryptoCurrencyPerformance.CalculationDate = calculationDate;
                                cryptoCurrencyPerformance.NetValue = cryptoCurrencyPerformance.TotalMarket - cryptoCurrencyPerformance.Total;

                                if (cryptoCurrencyPerformance.AveragePrice == 0)
                                {
                                    cryptoCurrencyPerformance.NetValue = 0;
                                }

                                if (cryptoCurrencyPerformance.Total != 0)
                                {
                                    cryptoCurrencyPerformance.PerformancePerc = cryptoCurrencyPerformance.NetValue / cryptoCurrencyPerformance.Total;
                                }

                                cryptoCurrencyPerformances.Add(cryptoCurrencyPerformance);

                                total += cryptoCurrencyPerformance.Total;
                                totalMarket += cryptoCurrencyPerformance.TotalMarket;
                                netValue += cryptoCurrencyPerformance.NetValue;
                            }
                        }

                        if (cryptoCurrencyPerformances != null && cryptoCurrencyPerformances.Count > 0)
                        {
                            ResultServiceObject<CryptoPortfolioPerformance> resultPortfolioPerformancePrev = GetPreviousDate(idCryptoPortfolio, calculationDate);

                            CryptoPortfolioPerformance cryptoPortfolioPerformance = null;
                            long? idPortfolioPerformancePrev = null;

                            if (resultCryptoPortfolioPerformance.Success && resultPortfolioPerformancePrev.Success)
                            {
                                cryptoPortfolioPerformance = resultCryptoPortfolioPerformance.Value;

                                if (cryptoPortfolioPerformance == null)
                                {
                                    cryptoPortfolioPerformance = new CryptoPortfolioPerformance();
                                }
                                else
                                {
                                    cryptoPortfolioPerformance = resultCryptoPortfolioPerformance.Value;
                                }

                                cryptoPortfolioPerformance.IdCryptoPortfolio = idCryptoPortfolio;
                                cryptoPortfolioPerformance.CalculationDate = calculationDate;
                                cryptoPortfolioPerformance.Total = total;
                                cryptoPortfolioPerformance.TotalMarket = totalMarket;
                                cryptoPortfolioPerformance.NetValue = netValue;

                                if (cryptoPortfolioPerformance.NetValue != 0)
                                {
                                    cryptoPortfolioPerformance.PerformancePerc = cryptoPortfolioPerformance.NetValue / cryptoPortfolioPerformance.Total;
                                }

                                if (resultPortfolioPerformancePrev.Value == null)
                                {
                                    cryptoPortfolioPerformance.PerformancePercTWR = 0;
                                    cryptoPortfolioPerformance.NetValueTWR = 0;
                                }
                                else
                                {
                                    cryptoPortfolioPerformance.NetValueTWR = cryptoPortfolioPerformance.NetValue - (resultPortfolioPerformancePrev.Value.NetValue - totalLossProfit);

                                    idPortfolioPerformancePrev = resultPortfolioPerformancePrev.Value.IdCryptoPortfolioPerformance;

                                    if (resultPortfolioPerformancePrev.Value.TotalMarket != 0)
                                    {
                                        cryptoPortfolioPerformance.PerformancePercTWR = (cryptoPortfolioPerformance.NetValueTWR / resultPortfolioPerformancePrev.Value.TotalMarket);
                                    }
                                }

                                if (cryptoPortfolioPerformance.IdCryptoPortfolioPerformance == 0)
                                {
                                    ResultServiceObject<CryptoPortfolioPerformance> resultPortPerfInsert = Insert(cryptoPortfolioPerformance);
                                    cryptoPortfolioPerformance = resultPortPerfInsert.Value;
                                }
                                else
                                {
                                    ResultServiceObject<CryptoPortfolioPerformance> resultPortPerfUpd = Update(cryptoPortfolioPerformance);
                                    cryptoPortfolioPerformance = resultPortPerfUpd.Value;
                                }
                            }

                            ResultServiceObject<IEnumerable<CryptoCurrencyPerformance>> resultCryptoCurrencyPerformance = _cryptoCurrencyPerformanceService.GetByIdPortfolioPerformance(cryptoPortfolioPerformance.IdCryptoPortfolioPerformance);
                            IEnumerable<CryptoCurrencyPerformance> cryptoCurrenciesDb = resultCryptoCurrencyPerformance.Value;

                            List<CryptoCurrencyPerformance> cryptoCurrenciesPerformancePreviousDb = new List<CryptoCurrencyPerformance>();

                            if (idPortfolioPerformancePrev.HasValue)
                            {
                                ResultServiceObject<IEnumerable<CryptoCurrencyPerformance>> resultCryptoCurrencyPerformancePrevious = _cryptoCurrencyPerformanceService.GetByIdPortfolioPerformance(idPortfolioPerformancePrev.Value);

                                if (resultCryptoCurrencyPerformancePrevious != null && resultCryptoCurrencyPerformancePrevious.Value.Count() > 0)
                                {
                                    cryptoCurrenciesPerformancePreviousDb = resultCryptoCurrencyPerformancePrevious.Value.ToList();
                                }
                            }

                            foreach (CryptoCurrencyPerformance cryptoCurrencyPerformance in cryptoCurrencyPerformances)
                            {
                                cryptoCurrencyPerformance.IdCryptoPortfolioPerformance = cryptoPortfolioPerformance.IdCryptoPortfolioPerformance;

                                CryptoCurrencyPerformance cryptoCurrencyPerformanceDb = null;
                                CryptoCurrencyPerformance cryptoCurrencyPerformancePreviousDb = null;

                                if (cryptoCurrenciesDb != null && cryptoCurrenciesDb.Count() > 0)
                                {
                                    cryptoCurrencyPerformanceDb = cryptoCurrenciesDb.FirstOrDefault(performanceStockTmp => performanceStockTmp.IdCryptoCurrency == cryptoCurrencyPerformance.IdCryptoCurrency);
                                }

                                if (cryptoCurrenciesPerformancePreviousDb != null && cryptoCurrenciesPerformancePreviousDb.Count() > 0)
                                {
                                    cryptoCurrencyPerformancePreviousDb = cryptoCurrenciesPerformancePreviousDb.FirstOrDefault(performanceStockTmp => performanceStockTmp.IdCryptoCurrency == cryptoCurrencyPerformance.IdCryptoCurrency);
                                }

                                if (cryptoCurrencyPerformancePreviousDb == null)
                                {
                                    cryptoCurrencyPerformance.NetValueTWR = 0;
                                    cryptoCurrencyPerformance.PerformancePercTWR = 0;
                                }
                                else
                                {
                                    cryptoCurrencyPerformance.NetValueTWR = cryptoCurrencyPerformance.NetValue - cryptoCurrencyPerformancePreviousDb.NetValue;

                                    if (cryptoCurrencyPerformancePreviousDb.TotalMarket != 0)
                                    {
                                        cryptoCurrencyPerformance.PerformancePercTWR = (cryptoCurrencyPerformance.NetValueTWR / cryptoCurrencyPerformancePreviousDb.TotalMarket);
                                    }
                                }

                                if (cryptoCurrencyPerformanceDb == null)
                                {
                                    ResultServiceObject<CryptoCurrencyPerformance> resultPerfStockInsert = _cryptoCurrencyPerformanceService.Insert(cryptoCurrencyPerformance);
                                }
                                else
                                {
                                    cryptoCurrencyPerformance.IdCryptoCurrencyPerformance = cryptoCurrencyPerformanceDb.IdCryptoCurrencyPerformance;
                                    ResultServiceObject<CryptoCurrencyPerformance> resultPerfStockUpdate = _cryptoCurrencyPerformanceService.Update(cryptoCurrencyPerformance);
                                }
                            }

                            if (cryptoCurrenciesDb != null && cryptoCurrenciesDb.Count() > 0)
                            {
                                foreach (CryptoCurrencyPerformance performanceStockDb in cryptoCurrenciesDb)
                                {
                                    CryptoCurrencyPerformance performanceStock = cryptoCurrencyPerformances.FirstOrDefault(performanceStockTmp => performanceStockTmp.IdCryptoCurrency == performanceStockDb.IdCryptoCurrency);

                                    if (performanceStock == null)
                                    {
                                        ResultServiceObject<CryptoCurrencyPerformance> resultPerfStockDelete = _cryptoCurrencyPerformanceService.Update(performanceStockDb);
                                    }
                                }
                            }
                        }
                    }
                    else if (resultCryptoPortfolioPerformance.Value != null)
                    {
                        CryptoPortfolioPerformance portfolioPerformance = resultCryptoPortfolioPerformance.Value;
                        portfolioPerformance.IdCryptoPortfolio = idCryptoPortfolio;
                        portfolioPerformance.CalculationDate = calculationDate;
                        portfolioPerformance.Total = 0;
                        portfolioPerformance.TotalMarket = 0;
                        portfolioPerformance.NetValue = 0;
                        portfolioPerformance.PerformancePerc = 0;
                        portfolioPerformance.PerformancePercTWR = 0;
                        portfolioPerformance.NetValueTWR = 0;

                        Update(portfolioPerformance);
                    }
                }

                _cryptoPortfolioService.UpdateCalculatePerformanceDate(resultCryptoPortfolio.Value.IdCryptoPortfolio, DateTime.Now);
            }
        }

    }
}
