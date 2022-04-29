using AutoMapper;
using Dividendos.API.Model.Request.Operation;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using Dividendos.Application.Base;
using Dividendos.Application.Interface;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Enum;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using K.Logger;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Dividendos.Application
{
    public class OperationApp : BaseApp, IOperationApp
    {
        private readonly IGlobalAuthenticationService _globalAuthenticationService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly IOperationService _operationService;
        private readonly IOperationItemService _operationItemService;
        private readonly IOperationHistService _operationHistService;
        private readonly IOperationItemHistService _operationItemHistService;
        private readonly ILogger _logger;
        private readonly IPortfolioService _portfolioService;
        private readonly ICompanyService _companyService;
        private readonly ISystemSettingsService _systemSettingsService;
        private readonly ISubscriptionService _subscriptionService;
        private readonly ISubPortfolioService _subPortfolioService;
        private readonly ISubPortfolioOperationService _subPortfolioOperationService;
        private readonly IPortfolioPerformanceService _portfolioPerformanceService;
        private readonly IStockService _stockService;
        private readonly IPerformanceStockService _performanceStockService;
        private readonly ICryptoCurrencyService _cryptoCurrencyService;
        private readonly ITraderService _traderService;
        private readonly IHolidayService _holidayService;
        private readonly ICryptoPortfolioService _cryptoPortfolioService;
        private readonly ICryptoSubPortfolioService _cryptoSubPortfolioService;

        public OperationApp(IMapper mapper,
            IUnitOfWork uow,
            IGlobalAuthenticationService globalAuthenticationService,
            IOperationService operationService,
            IOperationItemService operationItemService,
            IOperationHistService operationHistService,
            IOperationItemHistService operationItemHistService,
            IPortfolioService portfolioService,
            ICompanyService companyService,
            ISystemSettingsService systemSettingsService,
            ILogger logger,
            ISubscriptionService subscriptionService,
            ISubPortfolioService subPortfolioService,
            ISubPortfolioOperationService subPortfolioOperationService,
            IPortfolioPerformanceService portfolioPerformanceService,
            IStockService stockService,
            IPerformanceStockService performanceStockService,
            ICryptoCurrencyService cryptoCurrencyService,
            ITraderService traderService,
            IHolidayService holidayService,
            ICryptoPortfolioService cryptoPortfolioService,
            ICryptoSubPortfolioService cryptoSubPortfolioService
            )
        {
            _mapper = mapper;
            _uow = uow;
            _operationService = operationService;
            _operationItemService = operationItemService;
            _operationHistService = operationHistService;
            _operationItemHistService = operationItemHistService;
            _portfolioService = portfolioService;
            _companyService = companyService;
            _globalAuthenticationService = globalAuthenticationService;
            _logger = logger;
            _systemSettingsService = systemSettingsService;
            _subscriptionService = subscriptionService;
            _subPortfolioService = subPortfolioService;
            _subPortfolioOperationService = subPortfolioOperationService;
            _portfolioPerformanceService = portfolioPerformanceService;
            _stockService = stockService;
            _performanceStockService = performanceStockService;
            _cryptoCurrencyService = cryptoCurrencyService;
            _traderService = traderService;
            _holidayService = holidayService;
            _cryptoPortfolioService = cryptoPortfolioService;
            _cryptoSubPortfolioService = cryptoSubPortfolioService;
        }

        public ResultResponseObject<OperationEditAvgPriceVM> Update(Guid guidOperation, OperationEditAvgPriceVM operationEditVM)
        {
            ResultResponseObject<OperationEditAvgPriceVM> resultResponse = new ResultResponseObject<OperationEditAvgPriceVM>();
            resultResponse.Success = false;

            using (_uow.Create())
            {
                resultResponse = _operationService.UpdateOperation(guidOperation, operationEditVM, _portfolioService, _operationService, _operationItemService, _operationItemHistService, _operationHistService, _systemSettingsService, _portfolioPerformanceService, _stockService, _performanceStockService, _holidayService, _logger);
            }

            return resultResponse;
        }

        private OperationSummaryWrapperVM TransformOperationSummary(ResultServiceObject<IEnumerable<OperationSummaryView>> result, bool includeLogos)
        {
            bool zeroPrice = CheckTimeZeroPerc();

            ResultServiceObject<Subscription> subscription = _subscriptionService.GetByUser(_globalAuthenticationService.IdUser);
            bool hasSubscription = false;

            if (subscription.Value != null && subscription.Value.Active && (subscription.Value.SubscriptionTypeID.Equals((int)SubscriptionTypeEnum.Gold) ||
                                                                            subscription.Value.SubscriptionTypeID.Equals((int)SubscriptionTypeEnum.Annuity)))
            {
                hasSubscription = true;
            }

            OperationSummaryWrapperVM operationSummaryWrapperVM = new OperationSummaryWrapperVM();
            operationSummaryWrapperVM.OperationsSummary = new List<OperationSummaryVM>();
            operationSummaryWrapperVM.CalculationDate = result.Value.First().CalculationDate.ToString("dd/MM");
            operationSummaryWrapperVM.LastUpdated = DateTime.Now.ToString("HH:mm:ss");

            decimal totalMarketPortfolio = result.Value.Sum(op => op.NumberOfShares * op.MarketPrice);

            foreach (OperationSummaryView operationSummaryView in result.Value)
            {
                if (operationSummaryView.IdCountry.Equals((int)CountryEnum.EUA) && !hasSubscription)
                {
                    continue;
                }

                decimal perc = operationSummaryView.PerformancePerc * 100;
                decimal percTwr = operationSummaryView.LastChangePerc * 100;
                decimal totalMarket = operationSummaryView.NumberOfShares * operationSummaryView.MarketPrice;

                if (zeroPrice)
                {
                    percTwr = 0;
                }

                OperationSummaryVM operationSummaryVM = new OperationSummaryVM();
                operationSummaryVM.GuidOperation = operationSummaryView.GuidOperation;
                operationSummaryVM.AveragePrice = operationSummaryView.AveragePrice.ToString("n2", new CultureInfo("pt-br"));
                operationSummaryVM.AveragePriceN = operationSummaryView.AveragePrice;
                operationSummaryVM.IdOperation = operationSummaryView.IdOperation;
                operationSummaryVM.MarketPrice = operationSummaryView.MarketPrice.ToString("n2", new CultureInfo("pt-br"));
                operationSummaryVM.MarketPriceN = operationSummaryView.MarketPrice;
                operationSummaryVM.PerformancePerc = GetSignal(perc) + perc.ToString("n2", new CultureInfo("pt-br"));
                operationSummaryVM.PerformancePercN = operationSummaryView.PerformancePerc;
                operationSummaryVM.PerformancePercTWR = GetSignal(percTwr) + percTwr.ToString("n2", new CultureInfo("pt-br"));
                operationSummaryVM.PerformancePercTWRN = percTwr;
                operationSummaryVM.Symbol = operationSummaryView.Symbol;
                operationSummaryVM.NumberOfShares = operationSummaryView.NumberOfShares;
                operationSummaryVM.TotalMarket = totalMarket.ToString("n2", new CultureInfo("pt-br"));
                operationSummaryVM.TotalMarketN = totalMarket;
                operationSummaryVM.TotalDividends = operationSummaryView.TotalDividends.ToString("n2", new CultureInfo("pt-br"));
                operationSummaryVM.TotalDividendsN = operationSummaryView.TotalDividends;
                operationSummaryVM.IdStock = operationSummaryView.IdStock;

                operationSummaryVM.StockDividend12M = operationSummaryView.Dividend12Months.ToString("n2", new CultureInfo("pt-br"));
                operationSummaryVM.StockDividend12MN = operationSummaryView.Dividend12Months;
                operationSummaryVM.StockDividend24M = operationSummaryView.Dividend24Months.ToString("n2", new CultureInfo("pt-br"));
                operationSummaryVM.StockDividend24MN = operationSummaryView.Dividend24Months;
                operationSummaryVM.StockDividend36M = operationSummaryView.Dividend36Months.ToString("n2", new CultureInfo("pt-br"));
                operationSummaryVM.StockDividend36MN = operationSummaryView.Dividend36Months;

                operationSummaryVM.StockDividendYield12M = operationSummaryView.Dividend12MonthsYield.ToString("n2", new CultureInfo("pt-br"));
                operationSummaryVM.StockDividendYield12MN = operationSummaryView.Dividend12MonthsYield;
                operationSummaryVM.StockDividendYield24M = operationSummaryView.Dividend24MonthsYield.ToString("n2", new CultureInfo("pt-br"));
                operationSummaryVM.StockDividendYield24MN = operationSummaryView.Dividend24MonthsYield;
                operationSummaryVM.StockDividendYield36M = operationSummaryView.Dividend36MonthsYield.ToString("n2", new CultureInfo("pt-br"));
                operationSummaryVM.StockDividendYield36MN = operationSummaryView.Dividend36MonthsYield;

                operationSummaryVM.PricePerVap = operationSummaryView.PricePerVpa.ToString("n2", new CultureInfo("pt-br"));
                operationSummaryVM.PricePerVapN = operationSummaryView.PricePerVpa;

                if (includeLogos)
                {
                    operationSummaryVM.Logo = operationSummaryView.Logo;
                }

                if (totalMarketPortfolio > 0)
                {
                    decimal allocation = (totalMarket / totalMarketPortfolio) * 100;

                    operationSummaryVM.Allocation = allocation.ToString("n2", new CultureInfo("pt-br"));
                    operationSummaryVM.AllocationN = allocation;
                }

                if (operationSummaryView.IdCountry == 1)
                {
                    operationSummaryVM.NumberOfSharesString = FormatNumber(Convert.ToInt64(operationSummaryView.NumberOfShares));
                }
                else
                {
                    operationSummaryVM.NumberOfSharesString = operationSummaryView.NumberOfShares.ToString("0.#############################", new CultureInfo("pt-br"));
                }

                operationSummaryWrapperVM.OperationsSummary.Add(operationSummaryVM);
            }

            return operationSummaryWrapperVM;
        }

        private static bool CheckTimeZeroPerc()
        {
            bool zeroPrice = false;
            DateTime timeUtc = DateTime.UtcNow;
            var brasilia = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            DateTime dtBrasilia = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, brasilia);

            TimeSpan start = new TimeSpan(0, 0, 0); //10 o'clock
            TimeSpan end = new TimeSpan(9, 58, 0); //12 o'clock
            TimeSpan now = dtBrasilia.TimeOfDay;

            if ((now > start) && (now < end) || (dtBrasilia.DayOfWeek == DayOfWeek.Saturday || dtBrasilia.DayOfWeek == DayOfWeek.Sunday))
            {
                zeroPrice = true;
            }

            return zeroPrice;
        }

        public ResultResponseObject<OperationSummaryWrapperVM> GetOperationSummary()
        {
            ResultResponseObject<OperationSummaryWrapperVM> resultServiceObject = new ResultResponseObject<OperationSummaryWrapperVM>();
            OperationSummaryWrapperVM operationSummaryWrapperVM = null;

            using (_uow.Create())
            {
                ResultServiceObject<IEnumerable<OperationSummaryView>> result = new ResultServiceObject<IEnumerable<OperationSummaryView>>();

                result = _operationService.GetOperationSummary(_globalAuthenticationService.IdUser);

                if (result.Success && result.Value != null && result.Value.Count() > 0)
                {
                    operationSummaryWrapperVM = TransformOperationSummary(result, true);
                }
            }

            resultServiceObject.Value = operationSummaryWrapperVM;
            resultServiceObject.Success = true;

            return resultServiceObject;
        }

        public ResultResponseObject<OperationSummaryWrapperVM> GetOperationSummaryV3(string portfolioOrSubPortfolio)
        {
            return this.GetOperationSummaryBase(portfolioOrSubPortfolio, false, true);
        }

        public ResultResponseObject<OperationSummaryWrapperVM> GetOperationSummaryV4(string portfolioOrSubPortfolio)
        {
            return this.GetOperationSummaryBase(portfolioOrSubPortfolio, true, true);
        }

        public ResultResponseObject<OperationSummaryWrapperVM> GetOperationSummaryV5(string portfolioOrSubPortfolio)
        {
            return this.GetOperationSummaryBase(portfolioOrSubPortfolio, true, false);
        }

        public ResultResponseObject<OperationSummaryWrapperVM> GetOperationSummaryBase(string portfolioOrSubPortfolioOrTrader, bool includeCryptos, bool includeLogos)
        {
            ResultResponseObject<OperationSummaryWrapperVM> resultServiceObject = new ResultResponseObject<OperationSummaryWrapperVM>();
            OperationSummaryWrapperVM operationSummaryWrapperVM = null;

            using (_uow.Create())
            {
                Guid guidPortfolioOrSubPortfolio;

                if (Guid.TryParse(portfolioOrSubPortfolioOrTrader, out guidPortfolioOrSubPortfolio))
                {
                    ResultServiceObject<IEnumerable<OperationSummaryView>> result = new ResultServiceObject<IEnumerable<OperationSummaryView>>();

                    result = _operationService.GetOperationSummaryByPortfolioOrSubPortfolio(_globalAuthenticationService.IdUser, portfolioOrSubPortfolioOrTrader);

                    ResultServiceObject<Portfolio> resultPortfolio = _portfolioService.GetByGuid(Guid.Parse(portfolioOrSubPortfolioOrTrader));
                    ResultServiceObject<SubPortfolio> resultSubPortfolio = _subPortfolioService.GetByGuid(Guid.Parse(portfolioOrSubPortfolioOrTrader));
                    int idCountry = 0;
                    bool isManual = false;
                    Guid guidPortfolio = Guid.Empty;
                    Portfolio portfolio = null;

                    if (resultSubPortfolio.Success && resultPortfolio.Success)
                    {
                        portfolio = resultPortfolio.Value;
                        SubPortfolio subportfolio = resultSubPortfolio.Value;

                        if (portfolio != null)
                        {
                            idCountry = portfolio.IdCountry;
                            isManual = portfolio.ManualPortfolio;
                            guidPortfolio = portfolio.GuidPortfolio;
                        }
                        else if (subportfolio != null)
                        {
                            resultPortfolio = _portfolioService.GetById(subportfolio.IdPortfolio);

                            if (resultPortfolio.Value != null)
                            {
                                idCountry = resultPortfolio.Value.IdCountry;
                                isManual = resultPortfolio.Value.ManualPortfolio;
                                guidPortfolio = resultPortfolio.Value.GuidPortfolio;
                                portfolio = resultPortfolio.Value;
                            }
                        }
                    }

                    if (result.Success && result.Value != null && result.Value.Count() > 0)
                    {
                        ResultServiceObject<Trader> resultTrader = _traderService.GetById(portfolio.IdTrader);

                        operationSummaryWrapperVM = TransformOperationSummary(result, includeLogos);
                        operationSummaryWrapperVM.RedirectScreen = isManual || resultTrader.Value.TraderTypeID == (int)TraderTypeEnum.Avenue || resultTrader.Value.TraderTypeID == (int)TraderTypeEnum.Toro;
                        operationSummaryWrapperVM.IdCountry = idCountry;
                        operationSummaryWrapperVM.GuidPortfolio = guidPortfolio;
                        resultServiceObject.Value = operationSummaryWrapperVM;
                    }
                    else if (includeCryptos)
                    {
                        ResultServiceObject<IEnumerable<CryptoStatementView>> cryptoStatements = _cryptoCurrencyService.GetCryptosWithLogoByTrader(guidPortfolioOrSubPortfolio, _globalAuthenticationService.IdUser);

                        if (cryptoStatements.Value != null && cryptoStatements.Value.Count() > 0)
                        {
                            OperationSummaryWrapperVM operationSummaryCryptosWrapperVM = new OperationSummaryWrapperVM();
                            operationSummaryCryptosWrapperVM.IdCountry = (int)CountryEnum.Brazil;
                            operationSummaryCryptosWrapperVM.OperationsSummary = new List<OperationSummaryVM>();
                            operationSummaryCryptosWrapperVM.RedirectScreen = false;

                            decimal totalMarketPortfolio = cryptoStatements.Value.Sum(op => op.NumberOfShares * op.MarketPrice);

                            foreach (CryptoStatementView cryptoView in cryptoStatements.Value)
                            {
                                decimal netValue = cryptoView.NumberOfShares * cryptoView.MarketPrice;

                                OperationSummaryVM operationSummaryVM = new OperationSummaryVM();

                                if (cryptoView.AveragePrice != null && cryptoView.AveragePrice > 0)
                                {
                                    operationSummaryVM.AveragePrice = cryptoView.AveragePrice.Value.ToString("n2", new CultureInfo("pt-br"));
                                    operationSummaryVM.AveragePriceN = cryptoView.AveragePrice.Value;

                                    var variation = ((cryptoView.MarketPrice - cryptoView.AveragePrice.Value) / cryptoView.AveragePrice.Value) * 100;
                                    operationSummaryVM.PerformancePerc = GetSignal(variation) + variation.ToString("n2", new CultureInfo("pt-br"));
                                    operationSummaryVM.PerformancePercN = variation;
                                }
                                else
                                {
                                    operationSummaryVM.AveragePrice = "N/A";
                                    operationSummaryVM.AveragePriceN = 0;
                                    operationSummaryVM.PerformancePerc = "N/A";
                                    operationSummaryVM.PerformancePercN = 0;
                                }

                                operationSummaryVM.MarketPrice = cryptoView.MarketPrice.ToString("n2", new CultureInfo("pt-br"));
                                operationSummaryVM.MarketPriceN = cryptoView.MarketPrice;
                                operationSummaryVM.Symbol = cryptoView.Symbol.ToUpper();
                                operationSummaryVM.NumberOfShares = cryptoView.NumberOfShares;
                                operationSummaryVM.TotalMarket = netValue.ToString("n2", new CultureInfo("pt-br"));
                                operationSummaryVM.TotalMarketN = netValue;
                                operationSummaryVM.NumberOfSharesString = cryptoView.NumberOfShares.ToString("0.#############################", new CultureInfo("pt-br"));

                                if (includeLogos)
                                {
                                    operationSummaryVM.Logo = cryptoView.Logo;
                                }

                                operationSummaryVM.GuidOperation = cryptoView.CryptoCurrencyGuid;

                                if (totalMarketPortfolio > 0)
                                {
                                    decimal allocation = (netValue / totalMarketPortfolio) * 100;

                                    operationSummaryVM.Allocation = allocation.ToString("n2", new CultureInfo("pt-br"));
                                    operationSummaryVM.AllocationN = allocation;
                                }

                                operationSummaryCryptosWrapperVM.OperationsSummary.Add(operationSummaryVM);
                            }

                            resultServiceObject.Value = operationSummaryCryptosWrapperVM;
                        }
                        else
                        {
                            ResultServiceObject<IEnumerable<CryptoCurrencyStatementView>> resultServiceStatement = _cryptoPortfolioService.GetCryptoSummaryByPortfolioOrSubPortfolio(_globalAuthenticationService.IdUser, guidPortfolioOrSubPortfolio.ToString());

                            if (resultServiceStatement.Value != null && resultServiceStatement.Value.Count() > 0)
                            {
                                OperationSummaryWrapperVM operationSummaryCryptosWrapperVM = new OperationSummaryWrapperVM();
                                operationSummaryCryptosWrapperVM.IdCountry = (int)CountryEnum.EUA;
                                operationSummaryCryptosWrapperVM.OperationsSummary = new List<OperationSummaryVM>();
                                operationSummaryCryptosWrapperVM.RedirectScreen = true;
                                operationSummaryCryptosWrapperVM.IsCryptoManual = true;
                                operationSummaryCryptosWrapperVM.GuidPortfolio = guidPortfolioOrSubPortfolio;

                                decimal totalMarketPortfolio = resultServiceStatement.Value.Sum(op => op.Quantity * op.MarketPrice);

                                foreach (CryptoCurrencyStatementView cryptoCurrencyStatementView in resultServiceStatement.Value)
                                {
                                    decimal netValue = cryptoCurrencyStatementView.Quantity * cryptoCurrencyStatementView.MarketPrice;

                                    OperationSummaryVM operationSummaryVM = new OperationSummaryVM();
                                    operationSummaryVM.GuidCryptoCurrency = cryptoCurrencyStatementView.GuidCryptoCurrency;

                                    if (cryptoCurrencyStatementView.AveragePrice > 0)
                                    {
                                        operationSummaryVM.AveragePrice = cryptoCurrencyStatementView.AveragePrice.ToString("n2", new CultureInfo("pt-br"));
                                        operationSummaryVM.AveragePriceN = cryptoCurrencyStatementView.AveragePrice;

                                        var variation = ((cryptoCurrencyStatementView.MarketPrice - cryptoCurrencyStatementView.AveragePrice) / cryptoCurrencyStatementView.AveragePrice) * 100;
                                        operationSummaryVM.PerformancePerc = GetSignal(variation) + variation.ToString("n2", new CultureInfo("pt-br"));
                                        operationSummaryVM.PerformancePercN = variation;
                                    }
                                    else
                                    {
                                        operationSummaryVM.AveragePrice = "N/A";
                                        operationSummaryVM.AveragePriceN = 0;
                                        operationSummaryVM.PerformancePerc = "N/A";
                                        operationSummaryVM.PerformancePercN = 0;
                                    }


                                    operationSummaryVM.MarketPrice = cryptoCurrencyStatementView.MarketPrice.ToString("n2", new CultureInfo("pt-br"));
                                    operationSummaryVM.MarketPriceN = cryptoCurrencyStatementView.MarketPrice;
                                    operationSummaryVM.Symbol = cryptoCurrencyStatementView.Name.ToUpper();
                                    operationSummaryVM.NumberOfShares = cryptoCurrencyStatementView.Quantity;
                                    operationSummaryVM.TotalMarket = netValue.ToString("n2", new CultureInfo("pt-br"));
                                    operationSummaryVM.TotalMarketN = netValue;
                                    operationSummaryVM.NumberOfSharesString = cryptoCurrencyStatementView.Quantity.ToString("0.#############################", new CultureInfo("pt-br"));

                                    if (includeLogos)
                                    {
                                        operationSummaryVM.Logo = cryptoCurrencyStatementView.Logo;
                                    }

                                    operationSummaryVM.GuidOperation = cryptoCurrencyStatementView.GuidCryptoCurrency;

                                    if (totalMarketPortfolio > 0)
                                    {
                                        decimal allocation = (netValue / totalMarketPortfolio) * 100;

                                        operationSummaryVM.Allocation = allocation.ToString("n2", new CultureInfo("pt-br"));
                                        operationSummaryVM.AllocationN = allocation;
                                    }

                                    operationSummaryCryptosWrapperVM.OperationsSummary.Add(operationSummaryVM);
                                }

                                resultServiceObject.Value = operationSummaryCryptosWrapperVM;
                            }
                        }
                    }

                    resultServiceObject.Success = true;
                }
                else
                {
                    _logger.SendInformationAsync(string.Concat("Guid zoado: ", portfolioOrSubPortfolioOrTrader));
                    resultServiceObject.Value = operationSummaryWrapperVM;
                    resultServiceObject.Success = false;
                }
            }

            return resultServiceObject;
        }

        public string GetSignal(decimal value)
        {
            string signal = string.Empty;

            if (value > 0)
            {
                signal = "+";
            }

            return signal;
        }


        public ResultResponseBase BuyStock(Guid guidPortfolio, OperationAddVM operationAddVM)
        {
            ResultResponseBase resultResponseBase = new ResultResponseBase();
            resultResponseBase.Success = false;

            using (_uow.Create())
            {
                BuyStock(guidPortfolio, operationAddVM, resultResponseBase);
            }

            return resultResponseBase;
        }

        private void BuyStock(Guid guidPortfolio, OperationAddVM operationAddVM, ResultResponseBase resultResponseBase, bool priceAdjust = false)
        {
            _operationService.BuyStock(guidPortfolio, operationAddVM, resultResponseBase, _portfolioService, _operationService, _operationItemService,
                                       _operationHistService, _operationItemHistService, _systemSettingsService, _portfolioPerformanceService, _stockService,
                                       _performanceStockService, _holidayService, priceAdjust);
        }

        public ResultResponseBase SellStock(Guid guidPortfolio, OperationAddVM operationAddVM)
        {
            ResultResponseBase resultResponseBase = new ResultResponseBase();
            resultResponseBase.Success = false;

            using (_uow.Create())
            {
                SellStock(guidPortfolio, operationAddVM, resultResponseBase);
            }

            return resultResponseBase;
        }

        private void SellStock(Guid guidPortfolio, OperationAddVM operationAddVM, ResultResponseBase resultResponseBase, bool priceAdjust = false)
        {
            _operationService.SellStock(guidPortfolio, operationAddVM, resultResponseBase, _portfolioService, _operationService, _operationItemService,
                                       _operationHistService, _operationItemHistService, _systemSettingsService, _portfolioPerformanceService, _stockService,
                                       _performanceStockService, _holidayService, priceAdjust);
        }

        public ResultResponseBase EditBuyOperation(Guid guidPortfolio, OperationEditVM operationEditVM)
        {
            ResultResponseBase resultResponseBase = new ResultResponseBase();
            resultResponseBase.Success = false;

            using (_uow.Create())
            {
                resultResponseBase = _operationService.EditBuyOperation(guidPortfolio, operationEditVM, _portfolioService, _operationItemService, _systemSettingsService, _portfolioPerformanceService, _stockService, _performanceStockService, _holidayService, _operationService);
            }

            return resultResponseBase;
        }

        public ResultResponseBase EditSellOperation(Guid guidPortfolio, OperationEditVM operationEditVM)
        {
            ResultResponseBase resultResponseBase = new ResultResponseBase();
            resultResponseBase.Success = false;

            using (_uow.Create())
            {
                _operationService.EditSellOperation(guidPortfolio, operationEditVM, resultResponseBase, _portfolioService, _operationItemService, _systemSettingsService, _portfolioPerformanceService, _stockService, _performanceStockService, _holidayService, _operationService);
            }

            return resultResponseBase;
        }

        public ResultResponseBase EditSellOperation(Guid guidPortfolio, OperationEditVM operationEditVM, ResultResponseBase resultResponseBase)
        {
            _operationService.EditSellOperation(guidPortfolio, operationEditVM, resultResponseBase, _portfolioService, _operationItemService, _systemSettingsService, _portfolioPerformanceService, _stockService, _performanceStockService, _holidayService, _operationService);

            return resultResponseBase;
        }

        //private void EditSellOperation(Guid guidPortfolio, OperationEditVM operationEditVM, ResultResponseBase resultResponseBase)
        //{
        //    ResultServiceObject<Portfolio> resultPortfolio = _portfolioService.GetByGuid(guidPortfolio);

        //    if (resultPortfolio.Success && resultPortfolio.Value != null && operationEditVM != null)
        //    {
        //        decimal numberOfSharesInput = 0;
        //        decimal.TryParse(operationEditVM.NumberOfShares.Replace(".", string.Empty), NumberStyles.Currency, new CultureInfo("pt-br"), out numberOfSharesInput);

        //        if (numberOfSharesInput > 0 && !string.IsNullOrWhiteSpace(operationEditVM.Price) && !string.IsNullOrWhiteSpace(operationEditVM.OperationDate))
        //        {

        //            Portfolio portfolio = resultPortfolio.Value;
        //            ResultServiceObject<Operation> resultOperationSell = _operationService.GetByPortfolioAndIdStock(portfolio.IdPortfolio, operationEditVM.IdStock, 2);
        //            ResultServiceObject<Operation> resultOperationBuy = _operationService.GetByPortfolioAndIdStock(portfolio.IdPortfolio, operationEditVM.IdStock, 1);
        //            List<OperationItem> operationItems = new List<OperationItem>();

        //            if (resultOperationSell.Success && resultOperationBuy.Success)
        //            {
        //                Operation operationSell = resultOperationSell.Value;
        //                Operation operationBuy = resultOperationBuy.Value;

        //                if (operationBuy != null)
        //                {
        //                    //List<OperationItem> operationSellItems = new List<OperationItem>();

        //                    if (operationSell != null)
        //                    {
        //                        ResultServiceObject<IEnumerable<OperationItem>> resultOperationSellItems = _operationItemService.GetByIdOperation(operationSell.IdOperation, 2);

        //                        if (resultOperationSellItems.Success)
        //                        {
        //                            operationItems.AddRange(resultOperationSellItems.Value.ToList());
        //                        }
        //                    }
        //                    else
        //                    {
        //                        operationSell = new Operation();
        //                    }

        //                    if (operationBuy != null)
        //                    {
        //                        ResultServiceObject<IEnumerable<OperationItem>> resultOperationItems = _operationItemService.GetByIdOperation(operationBuy.IdOperation, 1);

        //                        if (resultOperationItems.Success)
        //                        {
        //                            operationItems.AddRange(resultOperationItems.Value.ToList());
        //                        }
        //                    }

        //                    decimal numberOfSellShares = numberOfSharesInput;
        //                    decimal price = 0;
        //                    decimal.TryParse(operationEditVM.Price.Replace(".", string.Empty), NumberStyles.Currency, new CultureInfo("pt-br"), out price);
        //                    decimal numberOfSharesBuy = 0;
        //                    decimal totalPrice = numberOfSellShares * price;

        //                    OperationItem operationItemDb = null;

        //                    if (operationItems != null && operationItems.Count > 0)
        //                    {
        //                        operationItemDb = operationItems.FirstOrDefault(op => op.IdOperationItem == operationEditVM.IdOperationItem);

        //                        if (operationItemDb != null)
        //                        {
        //                            DateTime operationDate;

        //                            if (!DateTime.TryParseExact(operationEditVM.OperationDate, "dd/MM/yyyy", new CultureInfo("pt-br"), DateTimeStyles.None, out operationDate))
        //                            {
        //                                resultResponseBase.ErrorMessages.Add("A data da operação é inválida");
        //                            }

        //                            operationItemDb.NumberOfShares = numberOfSellShares;
        //                            operationItemDb.AveragePrice = price;
        //                            operationItemDb.EventDate = operationDate.Add(DateTime.Now.TimeOfDay);
        //                            operationItemDb.HomeBroker = string.IsNullOrWhiteSpace(operationEditVM.Broker) ? "ND" : operationEditVM.Broker;
        //                        }

        //                        numberOfSellShares = operationItems.Where(op => op.IdOperationType == 2).Sum(opItemTmp => opItemTmp.NumberOfShares);
        //                        numberOfSharesBuy = operationItems.Where(op => op.IdOperationType == 1).Sum(opItemTmp => opItemTmp.NumberOfShares);
        //                    }

        //                    decimal numberOfSharesCalc = 0;
        //                    decimal avgPriceCalc = 0;
        //                    bool valid = _operationService.CalculateAveragePrice(ref operationItems, out numberOfSharesCalc, out avgPriceCalc);

        //                    if (numberOfSellShares <= numberOfSharesBuy && valid)
        //                    {
        //                        if (resultResponseBase.ErrorMessages.Count.Equals(0))
        //                        {
        //                            if (operationItemDb != null)
        //                            {
        //                                decimal acquisitionPrice = 0;

        //                                if (!string.IsNullOrWhiteSpace(operationEditVM.AcquisitionPrice) && decimal.TryParse(operationEditVM.AcquisitionPrice.Replace(".", string.Empty), NumberStyles.Currency, new CultureInfo("pt-br"), out acquisitionPrice))
        //                                {
        //                                    operationItemDb.AcquisitionPrice = acquisitionPrice;
        //                                }
        //                                else if (operationBuy != null)
        //                                {
        //                                    operationItemDb.AcquisitionPrice = operationBuy.AveragePrice;
        //                                }

        //                                _operationItemService.Update(operationItemDb);
        //                            }

        //                            operationSell.AveragePrice = numberOfSellShares == 0 ? 0 : totalPrice / numberOfSellShares;
        //                            operationSell.NumberOfShares = numberOfSellShares;
        //                            operationSell.HomeBroker = string.IsNullOrWhiteSpace(operationSell.HomeBroker) ? "ND" : operationSell.HomeBroker;
        //                            operationSell.IdPortfolio = portfolio.IdPortfolio;
        //                            operationSell.IdStock = operationEditVM.IdStock;
        //                            operationSell.IdOperationType = 2;

        //                            if (operationSell.NumberOfShares <= 0)
        //                            {
        //                                operationSell.Active = false;
        //                                operationSell.NumberOfShares = 0;
        //                                operationSell.AveragePrice = 0;
        //                            }
        //                            else
        //                            {
        //                                operationSell.Active = true;
        //                            }

        //                            if (operationSell.IdOperation == 0)
        //                            {
        //                                operationSell = _operationService.Insert(operationSell).Value;
        //                            }
        //                            else
        //                            {
        //                                _operationService.Update(operationSell);
        //                            }

        //                            if (operationBuy != null)
        //                            {
        //                                operationBuy.NumberOfShares = numberOfSharesCalc;
        //                                operationBuy.AveragePrice = avgPriceCalc;

        //                                if (operationBuy.NumberOfShares <= 0)
        //                                {
        //                                    operationBuy.Active = false;
        //                                    operationBuy.NumberOfShares = 0;
        //                                    operationBuy.AveragePrice = 0;
        //                                }
        //                                else
        //                                {
        //                                    operationBuy.Active = true;
        //                                }

        //                                //if (!operationBuy.Active)
        //                                //{
        //                                //    _operationService.MoveOperation(operationBuy.IdPortfolio, operationBuy.IdStock, _operationHistService, _operationItemService, _operationItemHistService);
        //                                //}

        //                                _operationService.Update(operationBuy);
        //                            }

        //                            _portfolioService.CalculatePerformance(portfolio.IdPortfolio, _systemSettingsService, _portfolioPerformanceService, _stockService, _operationService, _performanceStockService, _holidayService);

        //                            resultResponseBase.Success = true;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        resultResponseBase.ErrorMessages.Add("Quantidade vendida informada é maior do que a quantidade que você possui para este ativo");
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        public ResultResponseBase InactiveOperationItem(Guid guidPortfolio, long idOperationItem)
        {
            ResultResponseBase resultResponseBase = new ResultResponseBase();
            resultResponseBase.Success = false;

            using (_uow.Create())
            {
                InactiveOperationItem(guidPortfolio, idOperationItem, resultResponseBase);
            }

            return resultResponseBase;
        }

        private void InactiveOperationItem(Guid guidPortfolio, long idOperationItem, ResultResponseBase resultResponseBase)
        {
            if (idOperationItem != 0)
            {
                ResultServiceObject<OperationItem> resultOperationItem = _operationItemService.GetByIdOperationItem(idOperationItem);

                if (resultOperationItem.Success && resultOperationItem.Value != null)
                {
                    OperationItem operationItemDb = resultOperationItem.Value;
                    operationItemDb.IdOperationItem = idOperationItem;

                    ResultServiceObject<Portfolio> resultPortfolio = _portfolioService.GetByGuid(guidPortfolio);

                    if (resultPortfolio.Success && resultPortfolio.Value != null)
                    {
                        Portfolio portfolio = resultPortfolio.Value;
                        ResultServiceObject<Operation> resultOperation = _operationService.GetByPortfolioAndIdStock(portfolio.IdPortfolio, operationItemDb.IdStock, 2);
                        ResultServiceObject<Operation> resultOperationBuy = _operationService.GetByPortfolioAndIdStock(portfolio.IdPortfolio, operationItemDb.IdStock, 1);

                        if (resultOperation.Success && resultOperationBuy.Success)
                        {
                            Operation operationSell = resultOperation.Value;
                            Operation operationBuy = resultOperationBuy.Value;

                            if (operationBuy != null)
                            {
                                decimal numberOfSharesSell = 0;

                                if (operationSell != null)
                                {
                                    numberOfSharesSell = operationSell.NumberOfShares;
                                }

                                ResultServiceObject<IEnumerable<OperationItem>> resultOperationBuyItems = _operationItemService.GetByIdOperation(operationBuy.IdOperation, 1);
                                decimal numberOfSharesBuy = 0;

                                if (resultOperationBuyItems.Success && resultOperationBuyItems.Value != null && resultOperationBuyItems.Value.Count() > 0)
                                {
                                    numberOfSharesBuy = resultOperationBuyItems.Value.Sum(opItemTmp => opItemTmp.NumberOfShares);
                                }

                                if (operationItemDb.IdOperationType == 1)
                                {
                                    numberOfSharesBuy -= operationItemDb.NumberOfShares;
                                }
                                else if (operationItemDb.IdOperationType == 2)
                                {
                                    numberOfSharesSell -= operationItemDb.NumberOfShares;
                                }

                                if (numberOfSharesSell <= numberOfSharesBuy)
                                {
                                    List<OperationItem> operationItems = new List<OperationItem>();

                                    _operationItemService.Delete(operationItemDb);

                                    numberOfSharesSell = 0;
                                    decimal totalPriceSell = 0;
                                    numberOfSharesBuy = 0;
                                    decimal totalPriceBuy = 0;

                                    if (operationSell != null)
                                    {
                                        ResultServiceObject<IEnumerable<OperationItem>> resultOperationSellItems = _operationItemService.GetByIdOperation(operationSell.IdOperation, 2);

                                        if (resultOperationSellItems.Success)
                                        {
                                            operationItems.AddRange(resultOperationSellItems.Value.ToList());
                                        }

                                        if (operationItems != null && operationItems.Count > 0)
                                        {
                                            numberOfSharesSell = operationItems.Where(op => op.IdOperationType == 2).Sum(opItemTmp => opItemTmp.NumberOfShares);
                                            totalPriceSell = operationItems.Where(op => op.IdOperationType == 2).Sum(opItemTmp => (opItemTmp.AveragePrice * opItemTmp.NumberOfShares));
                                        }
                                        else
                                        {
                                            operationSell.NumberOfShares = 0;
                                        }

                                        operationSell.AveragePrice = numberOfSharesSell == 0 ? 0 : totalPriceSell / numberOfSharesSell;
                                        operationSell.NumberOfShares = numberOfSharesSell;

                                        if (operationSell.NumberOfShares <= 0)
                                        {
                                            operationSell.Active = false;
                                            operationSell.NumberOfShares = 0;
                                            operationSell.AveragePrice = 0;
                                        }
                                        else
                                        {
                                            operationSell.Active = true;
                                        }

                                        _operationService.Update(operationSell);
                                    }


                                    if (operationBuy != null)
                                    {
                                        resultOperationBuyItems = _operationItemService.GetByIdOperation(operationBuy.IdOperation, 1);

                                        if (resultOperationBuyItems.Success)
                                        {
                                            operationItems.AddRange(resultOperationBuyItems.Value.ToList());
                                        }

                                        if (operationItems != null && operationItems.Count > 0)
                                        {
                                            numberOfSharesBuy = operationItems.Where(op => op.IdOperationType == 1).Sum(opItemTmp => opItemTmp.NumberOfShares);
                                            totalPriceBuy = operationItems.Where(op => op.IdOperationType == 1).Sum(opItemTmp => (opItemTmp.AveragePrice * opItemTmp.NumberOfShares));
                                        }
                                        else
                                        {
                                            operationBuy.NumberOfShares = 0;
                                        }

                                        decimal numberOfSharesCalc = 0;
                                        decimal avgPriceCalc = 0;
                                        bool valid = _operationService.CalculateAveragePrice(ref operationItems, out numberOfSharesCalc, out avgPriceCalc);

                                        if (valid)
                                        {
                                            operationBuy.AveragePrice = avgPriceCalc;
                                            operationBuy.NumberOfShares = numberOfSharesCalc;

                                            if (operationBuy.NumberOfShares <= 0)
                                            {
                                                operationBuy.Active = false;
                                                operationBuy.NumberOfShares = 0;
                                                operationBuy.AveragePrice = 0;
                                            }
                                            else
                                            {
                                                operationBuy.Active = true;
                                            }

                                            //if (!operationBuy.Active)
                                            //{
                                            //    _operationService.MoveOperation(operationBuy.IdPortfolio, operationBuy.IdStock, _operationHistService, _operationItemService, _operationItemHistService);
                                            //}

                                            _operationService.Update(operationBuy);
                                        }
                                        else
                                        {
                                            throw new Exception("Quantidade vendida informada é maior do que a quantidade que você possui para este ativo");
                                        }
                                    }

                                    _portfolioService.CalculatePerformance(portfolio.IdPortfolio, _systemSettingsService, _portfolioPerformanceService, _stockService, _operationService, _performanceStockService, _holidayService);

                                    resultResponseBase.Success = true;
                                }
                                else
                                {
                                    resultResponseBase.ErrorMessages.Add("Quantidade vendida informada é maior do que a quantidade que você possui para este ativo");
                                }
                            }
                        }
                    }
                }
            }
        }

        public ResultResponseObject<OperationItemSummaryWrapperVM> GetOperationItemSummary(Guid guidPortfolio, long idStock, int idOperationType)
        {
            ResultResponseObject<OperationItemSummaryWrapperVM> resultServiceObject = new ResultResponseObject<OperationItemSummaryWrapperVM>();
            OperationItemSummaryWrapperVM operationItemSummaryWrapperVM = null;

            using (_uow.Create())
            {
                if (guidPortfolio != Guid.Empty && idStock != 0 && idOperationType != 0)
                {
                    ResultServiceObject<Portfolio> resultPortfolio = _portfolioService.GetByGuid(guidPortfolio);
                    ResultServiceObject<CompanyView> resultCompany = _companyService.GetCompanyLogoDetails(idStock);

                    if (resultPortfolio.Success && resultPortfolio.Value != null && resultCompany.Success && resultCompany.Value != null)
                    {
                        Portfolio portfolio = resultPortfolio.Value;
                        ResultServiceObject<Operation> resultOperation = _operationService.GetByPortfolioAndIdStock(portfolio.IdPortfolio, idStock, idOperationType);

                        if (resultOperation.Success && resultOperation.Value != null)
                        {
                            ResultServiceObject<IEnumerable<OperationItem>> resultOperationItems = _operationItemService.GetByIdOperation(resultOperation.Value.IdOperation, idOperationType);

                            if (resultOperationItems.Success && resultOperationItems.Value != null && resultOperationItems.Value.Count() > 0)
                            {
                                List<OperationItem> operationItems = resultOperationItems.Value.Where(opItemTmp => opItemTmp.EventDate.HasValue).OrderByDescending(opItemTmp => opItemTmp.EventDate.Value).ToList();
                                operationItemSummaryWrapperVM = new OperationItemSummaryWrapperVM();
                                operationItemSummaryWrapperVM.OperationsItemSummary = new List<OperationItemSummaryVM>();

                                operationItemSummaryWrapperVM.Company = resultCompany.Value.Company;
                                operationItemSummaryWrapperVM.Symbol = resultCompany.Value.Symbol;
                                operationItemSummaryWrapperVM.Logo = resultCompany.Value.Logo;
                                operationItemSummaryWrapperVM.Segment = resultCompany.Value.Segment;
                                operationItemSummaryWrapperVM.GuidPortfolio = portfolio.GuidPortfolio;
                                operationItemSummaryWrapperVM.IdOperation = resultOperation.Value.IdOperation;
                                operationItemSummaryWrapperVM.IdStock = resultOperation.Value.IdStock;
                                operationItemSummaryWrapperVM.IdCountry = portfolio.IdCountry;

                                foreach (OperationItem operationSummaryView in operationItems)
                                {
                                    OperationItemSummaryVM operationItemSummaryVM = new OperationItemSummaryVM();
                                    operationItemSummaryVM.AveragePrice = operationSummaryView.AveragePrice.ToString("n2", new CultureInfo("pt-br"));
                                    operationItemSummaryVM.IdStock = resultOperation.Value.IdStock;
                                    operationItemSummaryVM.IdOperationItem = operationSummaryView.IdOperationItem;
                                    operationItemSummaryVM.IdOperation = operationSummaryView.IdOperation;
                                    operationItemSummaryVM.IdOperationType = operationSummaryView.IdOperationType;
                                    operationItemSummaryVM.NumberOfShares = operationSummaryView.NumberOfShares.ToString("0.#############################", new CultureInfo("pt-br"));
                                    operationItemSummaryVM.NumberOfSharesZeros = operationSummaryView.NumberOfShares.ToString("n5", new CultureInfo("pt-br"));
                                    operationItemSummaryVM.GuidOperationItem = operationSummaryView.GuidOperationItem;
                                    operationItemSummaryVM.HomeBroker = operationSummaryView.HomeBroker;

                                    if (operationSummaryView.EventDate.HasValue)
                                    {
                                        operationItemSummaryVM.EventDate = operationSummaryView.EventDate.Value.ToString("dd/MM/yyyy");
                                    }

                                    operationItemSummaryWrapperVM.OperationsItemSummary.Add(operationItemSummaryVM);
                                }
                            }
                        }
                    }
                }
            }

            resultServiceObject.Value = operationItemSummaryWrapperVM;
            resultServiceObject.Success = true;

            return resultServiceObject;
        }

        public ResultResponseObject<OperationSellViewWrapperVM> GetOperationSellView(Guid guidPortfolioSub, string startDate, string endDate)
        {
            ResultResponseObject<OperationSellViewWrapperVM> resultServiceObject = new ResultResponseObject<OperationSellViewWrapperVM>();
            resultServiceObject.Success = true;
            OperationSellViewWrapperVM operationSellViewWrapperVM = new OperationSellViewWrapperVM();


            DateTime? startDateParam = null;
            DateTime? endDateParam = null;

            if (!string.IsNullOrWhiteSpace(startDate) && !string.IsNullOrWhiteSpace(endDate))
            {
                DateTime operationStartDate;

                if (DateTime.TryParseExact(startDate, "dd/MM/yyyy", new CultureInfo("pt-br"), DateTimeStyles.None, out operationStartDate))
                {
                    startDateParam = operationStartDate;
                }
                else
                {
                    resultServiceObject.ErrorMessages.Add("Data de início inválida");
                }

                DateTime operationEndDate;

                if (DateTime.TryParseExact(endDate, "dd/MM/yyyy", new CultureInfo("pt-br"), DateTimeStyles.None, out operationEndDate))
                {
                    endDateParam = operationEndDate;
                }
                else
                {
                    resultServiceObject.ErrorMessages.Add("Data fim inválida");
                }
            }

            if (resultServiceObject.ErrorMessages == null || resultServiceObject.ErrorMessages.Count() == 0)
            {
                using (_uow.Create())
                {
                    ResultServiceObject<Portfolio> resultPortfolio = _portfolioService.GetByGuid(guidPortfolioSub);
                    ResultServiceObject<SubPortfolio> resultSubPortfolio = _subPortfolioService.GetByGuid(guidPortfolioSub);

                    if (resultSubPortfolio.Success && resultPortfolio.Success)
                    {
                        ResultServiceObject<IEnumerable<OperationSellDetailsView>> result = new ResultServiceObject<IEnumerable<OperationSellDetailsView>>();
                        Portfolio portfolio = resultPortfolio.Value;
                        SubPortfolio subportfolio = resultSubPortfolio.Value;

                        if (portfolio != null)
                        {
                            operationSellViewWrapperVM.GuidPortfolioSubPortfolio = portfolio.GuidPortfolio;
                            operationSellViewWrapperVM.IdPortfolio = portfolio.IdPortfolio;
                            result = _operationService.GetSellDetailsByIdPortfolio(portfolio.IdPortfolio, startDateParam, endDateParam);
                            operationSellViewWrapperVM.IdCountry = portfolio.IdCountry;
                        }
                        else if (subportfolio != null)
                        {
                            operationSellViewWrapperVM.GuidPortfolioSubPortfolio = subportfolio.GuidSubPortfolio;
                            operationSellViewWrapperVM.IdPortfolio = subportfolio.IdPortfolio;
                            result = _operationService.GetSellDetailsByIdSubportfolio(subportfolio.IdPortfolio, subportfolio.IdSubPortfolio, startDateParam, endDateParam);

                            resultPortfolio = _portfolioService.GetById(subportfolio.IdPortfolio);

                            if (resultPortfolio.Value != null)
                            {
                                operationSellViewWrapperVM.IdCountry = resultPortfolio.Value.IdCountry;
                            }
                        }

                        if (result.Success && result.Value != null && result.Value.Count() > 0)
                        {
                            operationSellViewWrapperVM.OperationsSellCompany = new List<OperationSellCompanyVM>();
                            decimal totalSoldGeneral = 0;
                            decimal totalLossProfitGeneral = 0;
                            decimal totalLossGeneral = 0;
                            decimal totalProfitGeneral = 0;
                            string[] stocksThousand = null;
                            ResultServiceObject<Entity.Entities.SystemSettings> resultStockThousand = _systemSettingsService.GetByKey(Constants.SYSTEM_SETTINGS_DIVIDE_STOCK_BY_THOUSAND);

                            if (resultStockThousand.Success && resultStockThousand.Value != null)
                            {
                                stocksThousand = resultStockThousand.Value.SettingValue.Split(';');
                            }

                            List<OperationSellDetailsView> operationSellDetailsViews = result.Value.OrderBy(op => op.Symbol).ThenByDescending(op => op.EventDate).ToList();

                            foreach (OperationSellDetailsView operationSellDetailsView in operationSellDetailsViews)
                            {
                                #region Item

                                OperationSellDetailsVM operationSellDetailsVM = new OperationSellDetailsVM();
                                operationSellDetailsVM.AveragePrice = operationSellDetailsView.AveragePrice.ToString("n2", new CultureInfo("pt-br"));
                                if (operationSellDetailsView.AcquisitionPrice.HasValue)
                                {
                                    operationSellDetailsVM.AcquisitionPrice = operationSellDetailsView.AcquisitionPrice.Value.ToString("n2", new CultureInfo("pt-br"));
                                }
                                else
                                {
                                    operationSellDetailsVM.AcquisitionPrice = "--";
                                }

                                operationSellDetailsVM.IdOperationItem = operationSellDetailsView.IdOperationItem;
                                operationSellDetailsVM.NumberOfShares = operationSellDetailsView.NumberOfShares.ToString("0.#############################", new CultureInfo("pt-br"));
                                operationSellDetailsVM.EventDate = operationSellDetailsView.EventDate.HasValue ? operationSellDetailsView.EventDate.Value.ToString("dd/MM/yyyy") : "Data Indef";
                                operationSellDetailsVM.LossProfit = "--";
                                operationSellDetailsVM.GuidOperationItem = operationSellDetailsView.GuidOperationItem.ToString();
                                operationSellDetailsVM.Broker = operationSellDetailsView.HomeBroker;


                                decimal totalLossProfitItem = 0;

                                if (operationSellDetailsView.AcquisitionPrice.HasValue)
                                {
                                    totalLossProfitItem = (operationSellDetailsView.AveragePrice - operationSellDetailsView.AcquisitionPrice.Value) * operationSellDetailsView.NumberOfShares;

                                    if (stocksThousand != null && stocksThousand.Length > 0 && stocksThousand.Contains(operationSellDetailsView.Symbol))
                                    {
                                        totalLossProfitItem = totalLossProfitItem / 1000;
                                    }

                                    operationSellDetailsVM.LossProfit = GetSignal(totalLossProfitItem) + totalLossProfitItem.ToString("n2", new CultureInfo("pt-br"));
                                }

                                decimal totalItem = operationSellDetailsView.AveragePrice * operationSellDetailsView.NumberOfShares;

                                if (stocksThousand != null && stocksThousand.Length > 0 && stocksThousand.Contains(operationSellDetailsView.Symbol))
                                {
                                    totalItem = totalItem / 1000;
                                }

                                operationSellDetailsVM.Total = totalItem.ToString("n2", new CultureInfo("pt-br"));

                                if (totalItem != 0)
                                {
                                    decimal perc = totalLossProfitItem / totalItem * 100;
                                    operationSellDetailsVM.Perc = GetSignal(perc) + perc.ToString("n2", new CultureInfo("pt-br"));
                                }

                                #endregion

                                #region Company

                                OperationSellCompanyVM operationSellCompanyVM = operationSellViewWrapperVM.OperationsSellCompany.FirstOrDefault(op => op.Symbol == operationSellDetailsView.Symbol);

                                if (operationSellCompanyVM == null)
                                {
                                    decimal lossCp = 0;
                                    decimal profitCp = 0;

                                    decimal totalSoldCp = operationSellDetailsViews.Where(opTmp => opTmp.Symbol == operationSellDetailsView.Symbol)
                                        .Sum(opTmp =>
                                        {
                                            decimal total = opTmp.AveragePrice * opTmp.NumberOfShares;

                                            if (stocksThousand != null && stocksThousand.Length > 0 && stocksThousand.Contains(opTmp.Symbol))
                                            {
                                                total = total / 1000;
                                            }

                                            return total;
                                        });

                                    totalSoldGeneral += totalSoldCp;

                                    decimal totalProfitCp = operationSellDetailsViews.Where(opTmp => opTmp.Symbol == operationSellDetailsView.Symbol)
                                        .Sum(opTmp =>
                                        {
                                            decimal totalLossProfit = 0;

                                            if (opTmp.AcquisitionPrice.HasValue)
                                            {
                                                totalLossProfit = (opTmp.AveragePrice - opTmp.AcquisitionPrice.Value) * opTmp.NumberOfShares;
                                            }

                                            if (stocksThousand != null && stocksThousand.Length > 0 && stocksThousand.Contains(opTmp.Symbol))
                                            {
                                                totalLossProfit = totalLossProfit / 1000;
                                            }

                                            if (totalLossProfit > 0)
                                            {
                                                profitCp += totalLossProfit;
                                            }
                                            else
                                            {
                                                lossCp += totalLossProfit;
                                            }

                                            return totalLossProfit;
                                        });

                                    totalLossGeneral += lossCp;
                                    totalProfitGeneral += profitCp;
                                    totalLossProfitGeneral += totalProfitCp;


                                    operationSellCompanyVM = new OperationSellCompanyVM();
                                    operationSellCompanyVM.Company = operationSellDetailsView.Company;
                                    operationSellCompanyVM.IdCompany = operationSellDetailsView.IdCompany;
                                    operationSellCompanyVM.IdStock = operationSellDetailsView.IdStock;
                                    operationSellCompanyVM.Logo = operationSellDetailsView.Logo;
                                    operationSellCompanyVM.Segment = operationSellDetailsView.Segment;
                                    operationSellCompanyVM.Symbol = operationSellDetailsView.Symbol;
                                    operationSellCompanyVM.TotalSold = totalSoldCp.ToString("n2", new CultureInfo("pt-br"));
                                    operationSellCompanyVM.Profit = GetSignal(profitCp) + profitCp.ToString("n2", new CultureInfo("pt-br"));
                                    operationSellCompanyVM.Loss = GetSignal(lossCp) + lossCp.ToString("n2", new CultureInfo("pt-br"));
                                    operationSellCompanyVM.LossProfit = GetSignal(totalProfitCp) + totalProfitCp.ToString("n2", new CultureInfo("pt-br"));


                                    if (totalSoldCp != 0)
                                    {
                                        decimal perc = (totalProfitCp / totalSoldCp * 100);
                                        operationSellCompanyVM.Perc = GetSignal(perc) + perc.ToString("n2", new CultureInfo("pt-br"));
                                    }

                                    operationSellCompanyVM.Operations = new List<OperationSellDetailsVM>();

                                    operationSellViewWrapperVM.OperationsSellCompany.Add(operationSellCompanyVM);

                                    totalSoldCp = 0;
                                    totalProfitCp = 0;
                                }

                                #endregion

                                operationSellCompanyVM.Operations.Add(operationSellDetailsVM);
                            }

                            if (totalSoldGeneral != 0)
                            {
                                decimal perc = (totalLossProfitGeneral / totalSoldGeneral * 100);
                                operationSellViewWrapperVM.Perc = GetSignal(perc) + perc.ToString("n2", new CultureInfo("pt-br"));
                            }

                            operationSellViewWrapperVM.TotalLoss = GetSignal(totalLossGeneral) + totalLossGeneral.ToString("n2", new CultureInfo("pt-br"));
                            operationSellViewWrapperVM.TotalProfit = GetSignal(totalProfitGeneral) + totalProfitGeneral.ToString("n2", new CultureInfo("pt-br"));
                            operationSellViewWrapperVM.TotalLossProfit = GetSignal(totalLossProfitGeneral) + totalLossProfitGeneral.ToString("n2", new CultureInfo("pt-br"));
                            operationSellViewWrapperVM.TotalSold = totalSoldGeneral.ToString("n2", new CultureInfo("pt-br"));
                        }
                    }
                }
            }

            if (resultServiceObject.ErrorMessages != null && resultServiceObject.ErrorMessages.Count() > 0)
            {
                resultServiceObject.Success = false;
            }

            resultServiceObject.Value = operationSellViewWrapperVM;
            resultServiceObject.Success = true;

            return resultServiceObject;
        }

        public ResultResponseObject<OperationBuyViewWrapperVM> GetOperationBuyView(Guid guidPortfolioSub, string startDate, string endDate)
        {
            ResultResponseObject<OperationBuyViewWrapperVM> resultServiceObject = new ResultResponseObject<OperationBuyViewWrapperVM>();
            OperationBuyViewWrapperVM operationBuyViewWrapperVM = new OperationBuyViewWrapperVM();

            DateTime? startDateParam = null;
            DateTime? endDateParam = null;

            if (!string.IsNullOrWhiteSpace(startDate) && !string.IsNullOrWhiteSpace(endDate))
            {
                DateTime operationStartDate;

                if (DateTime.TryParseExact(startDate, "dd/MM/yyyy", new CultureInfo("pt-br"), DateTimeStyles.None, out operationStartDate))
                {
                    startDateParam = operationStartDate;
                }
                else
                {
                    resultServiceObject.ErrorMessages.Add("Data de início inválida");
                }

                DateTime operationEndDate;

                if (DateTime.TryParseExact(endDate, "dd/MM/yyyy", new CultureInfo("pt-br"), DateTimeStyles.None, out operationEndDate))
                {
                    endDateParam = operationEndDate;
                }
                else
                {
                    resultServiceObject.ErrorMessages.Add("Data fim inválida");
                }
            }

            using (_uow.Create())
            {
                ResultServiceObject<Portfolio> resultPortfolio = _portfolioService.GetByGuid(guidPortfolioSub);
                ResultServiceObject<SubPortfolio> resultSubPortfolio = _subPortfolioService.GetByGuid(guidPortfolioSub);

                if (resultSubPortfolio.Success && resultPortfolio.Success)
                {
                    ResultServiceObject<IEnumerable<OperationBuyDetailsView>> result = new ResultServiceObject<IEnumerable<OperationBuyDetailsView>>();
                    ResultServiceObject<IEnumerable<PortfolioStatementView>> resultPortfolioStView = new ResultServiceObject<IEnumerable<PortfolioStatementView>>();
                    Portfolio portfolio = resultPortfolio.Value;
                    SubPortfolio subportfolio = resultSubPortfolio.Value;

                    if (portfolio != null)
                    {
                        operationBuyViewWrapperVM.GuidPortfolioSubPortfolio = portfolio.GuidPortfolio;
                        operationBuyViewWrapperVM.IdPortfolio = portfolio.IdPortfolio;
                        result = _operationService.GetBuyDetailsByIdPortfolio(portfolio.IdPortfolio, startDateParam, endDateParam);
                        operationBuyViewWrapperVM.IdCountry = portfolio.IdCountry;
                        resultPortfolioStView = _portfolioService.GetByPortfolio(portfolio.GuidPortfolio, null);
                    }
                    else if (subportfolio != null)
                    {
                        operationBuyViewWrapperVM.GuidPortfolioSubPortfolio = subportfolio.GuidSubPortfolio;
                        operationBuyViewWrapperVM.IdPortfolio = subportfolio.IdPortfolio;
                        result = _operationService.GetBuyDetailsByIdSubportfolio(subportfolio.IdPortfolio, subportfolio.IdSubPortfolio, startDateParam, endDateParam);

                        resultPortfolio = _portfolioService.GetById(subportfolio.IdPortfolio);
                        resultPortfolioStView = _portfolioService.GetBySubportfolio(subportfolio.GuidSubPortfolio, null);

                        if (resultPortfolio.Value != null)
                        {
                            operationBuyViewWrapperVM.IdCountry = resultPortfolio.Value.IdCountry;
                        }
                    }

                    if (result.Success && result.Value != null && result.Value.Count() > 0 && resultPortfolioStView.Success)
                    {
                        operationBuyViewWrapperVM.OperationsBuyCompany = new List<OperationBuyCompanyVM>();
                        decimal totalBuyGeneral = 0;
                        string[] stocksThousand = null;
                        ResultServiceObject<Entity.Entities.SystemSettings> resultStockThousand = _systemSettingsService.GetByKey(Constants.SYSTEM_SETTINGS_DIVIDE_STOCK_BY_THOUSAND);

                        if (resultStockThousand.Success && resultStockThousand.Value != null)
                        {
                            stocksThousand = resultStockThousand.Value.SettingValue.Split(';');
                        }

                        List<OperationBuyDetailsView> operationBuyDetailsViews = result.Value.OrderBy(op => op.Symbol).ThenByDescending(op => op.EventDate).ToList();

                        foreach (OperationBuyDetailsView operationBuyDetailsView in operationBuyDetailsViews)
                        {
                            #region Item

                            OperationBuyDetailsVM operationBuyDetailsVM = new OperationBuyDetailsVM();
                            operationBuyDetailsVM.AveragePrice = operationBuyDetailsView.AveragePrice.ToString("n2", new CultureInfo("pt-br"));
                            operationBuyDetailsVM.IdOperationItem = operationBuyDetailsView.IdOperationItem;
                            operationBuyDetailsVM.NumberOfShares = operationBuyDetailsView.NumberOfShares.ToString("0.#############################", new CultureInfo("pt-br"));
                            operationBuyDetailsVM.EventDate = operationBuyDetailsView.EventDate.HasValue ? operationBuyDetailsView.EventDate.Value.ToString("dd/MM/yyyy") : "Data Indef";
                            operationBuyDetailsVM.GuidOperationItem = operationBuyDetailsView.GuidOperationItem.ToString();
                            operationBuyDetailsVM.Broker = operationBuyDetailsView.HomeBroker;

                            decimal totalItem = operationBuyDetailsView.AveragePrice * operationBuyDetailsView.NumberOfShares;

                            if (stocksThousand != null && stocksThousand.Length > 0 && stocksThousand.Contains(operationBuyDetailsView.Symbol))
                            {
                                totalItem = totalItem / 1000;
                            }

                            operationBuyDetailsVM.Total = totalItem.ToString("n2", new CultureInfo("pt-br"));

                            #endregion

                            #region Company

                            OperationBuyCompanyVM operationBuyCompanyVM = operationBuyViewWrapperVM.OperationsBuyCompany.FirstOrDefault(op => op.Symbol == operationBuyDetailsView.Symbol);

                            if (operationBuyCompanyVM == null)
                            {
                                decimal totalBuyCp = operationBuyDetailsViews.Where(opTmp => opTmp.Symbol == operationBuyDetailsView.Symbol)
                                    .Sum(opTmp =>
                                    {
                                        decimal total = opTmp.AveragePrice * opTmp.NumberOfShares;

                                        if (stocksThousand != null && stocksThousand.Length > 0 && stocksThousand.Contains(opTmp.Symbol))
                                        {
                                            total = total / 1000;
                                        }

                                        return total;
                                    });

                                totalBuyGeneral += totalBuyCp;


                                operationBuyCompanyVM = new OperationBuyCompanyVM();
                                operationBuyCompanyVM.Company = operationBuyDetailsView.Company;
                                operationBuyCompanyVM.IdCompany = operationBuyDetailsView.IdCompany;
                                operationBuyCompanyVM.IdStock = operationBuyDetailsView.IdStock;
                                operationBuyCompanyVM.Logo = operationBuyDetailsView.Logo;
                                operationBuyCompanyVM.Segment = operationBuyDetailsView.Segment;
                                operationBuyCompanyVM.Symbol = operationBuyDetailsView.Symbol;
                                operationBuyCompanyVM.TotalBuy = totalBuyCp.ToString("n2", new CultureInfo("pt-br"));

                                if (resultPortfolioStView.Value != null && resultPortfolioStView.Value.Count() > 0)
                                {
                                    PortfolioStatementView portfolioStView = resultPortfolioStView.Value.FirstOrDefault(portStView => portStView.IdStock == operationBuyDetailsView.IdStock);

                                    if (portfolioStView != null)
                                    {
                                        decimal perc = portfolioStView.PerformancePerc * 100;
                                        operationBuyCompanyVM.NumberOfShares = portfolioStView.NumberOfShares.ToString("0.#############################", new CultureInfo("pt-br"));
                                        operationBuyCompanyVM.NetValue = GetSignal(portfolioStView.NetValue) + portfolioStView.NetValue.ToString("n2", new CultureInfo("pt-br"));
                                        operationBuyCompanyVM.PerformancePerc = GetSignal(perc) + perc.ToString("n2", new CultureInfo("pt-br"));
                                        operationBuyCompanyVM.TotalMarket = portfolioStView.TotalMarket.ToString("n2", new CultureInfo("pt-br"));
                                    }
                                }

                                operationBuyCompanyVM.Operations = new List<OperationBuyDetailsVM>();

                                operationBuyViewWrapperVM.OperationsBuyCompany.Add(operationBuyCompanyVM);

                                totalBuyCp = 0;
                            }

                            #endregion

                            operationBuyCompanyVM.Operations.Add(operationBuyDetailsVM);
                        }

                        operationBuyViewWrapperVM.TotalBuy = totalBuyGeneral.ToString("n2", new CultureInfo("pt-br"));
                    }
                }
            }

            if (resultServiceObject.ErrorMessages != null && resultServiceObject.ErrorMessages.Count() > 0)
            {
                resultServiceObject.Success = false;
            }

            resultServiceObject.Value = operationBuyViewWrapperVM;
            resultServiceObject.Success = true;

            return resultServiceObject;
        }

        public ResultResponseBase EditSellOperationIem(Guid guidOperationItem, OperationItemEditVM operationItemEditVM)
        {
            ResultResponseBase resultResponseBase = new ResultResponseBase();
            resultResponseBase.Success = true;

            decimal numberOfSharesInput = 0;
            decimal.TryParse(operationItemEditVM.NumberOfShares.Replace(".", string.Empty), NumberStyles.Currency, new CultureInfo("pt-br"), out numberOfSharesInput);
            decimal price = 0;
            decimal.TryParse(operationItemEditVM.Price.Replace(".", string.Empty), NumberStyles.Currency, new CultureInfo("pt-br"), out price);
            decimal acquisitionPrice = 0;
            decimal.TryParse(operationItemEditVM.AcquisitionPrice.Replace(".", string.Empty), NumberStyles.Currency, new CultureInfo("pt-br"), out acquisitionPrice);
            DateTime operationDate = DateTime.MinValue;

            if (numberOfSharesInput <= 0)
            {
                resultResponseBase.ErrorMessages.Add("Quantidade deve ser maior que zero");
            }
            else if (price <= 0)
            {
                resultResponseBase.ErrorMessages.Add("Preço de Venda deve ser informado");
            }
            else if (acquisitionPrice <= 0)
            {
                resultResponseBase.ErrorMessages.Add("Preço de Compra deve ser informado");
            }
            else if (string.IsNullOrWhiteSpace(operationItemEditVM.OperationDate))
            {
                resultResponseBase.ErrorMessages.Add("A data da operação é inválida");
            }
            else
            {
                if (!DateTime.TryParseExact(operationItemEditVM.OperationDate, "dd/MM/yyyy", new CultureInfo("pt-br"), DateTimeStyles.None, out operationDate))
                {
                    if (!DateTime.TryParseExact(operationItemEditVM.OperationDate, "dd/MM/yy", new CultureInfo("pt-br"), DateTimeStyles.None, out operationDate))
                    {
                        resultResponseBase.ErrorMessages.Add("A data da operação é inválida");
                    }
                }
            }


            if (resultResponseBase.ErrorMessages == null || resultResponseBase.ErrorMessages.Count() == 0)
            {
                using (_uow.Create())
                {
                    ResultServiceObject<OperationItem> resultOperationItem = _operationItemService.GetByGuidOperationItem(guidOperationItem);
                    ResultServiceObject<OperationItemHist> resultOperationItemHist = _operationItemHistService.GetByGuidOperationItem(guidOperationItem);

                    if (resultOperationItem.Success && resultOperationItemHist.Success)
                    {
                        OperationItem operationItem = resultOperationItem.Value;
                        OperationItemHist operationItemHist = resultOperationItemHist.Value;

                        if (operationItem != null)
                        {
                            ResultServiceObject<Portfolio> resultPortfolio = _portfolioService.GetByIdOperationItem(operationItem.IdOperationItem);

                            if (resultPortfolio.Value != null)
                            {
                                if (resultPortfolio.Value.ManualPortfolio)
                                {
                                    OperationEditVM operationEditVM = new OperationEditVM();
                                    operationEditVM.AcquisitionPrice = operationItemEditVM.AcquisitionPrice;
                                    operationEditVM.Broker = operationItemEditVM.Broker;
                                    operationEditVM.IdOperationItem = operationItem.IdOperationItem;
                                    operationEditVM.IdStock = operationItem.IdStock;
                                    operationEditVM.NumberOfShares = operationItemEditVM.NumberOfShares;
                                    operationEditVM.OperationDate = operationItemEditVM.OperationDate;
                                    operationEditVM.Price = operationItemEditVM.Price;

                                    EditSellOperation(resultPortfolio.Value.GuidPortfolio, operationEditVM, resultResponseBase);
                                }
                                else
                                {
                                    operationItem.Active = false;
                                    _operationItemService.Inactive(operationItem.IdOperationItem);

                                    operationItem.IdOperationItem = 0;
                                    operationItem.GuidOperationItem = Guid.Empty;
                                    operationItem.NumberOfShares = numberOfSharesInput;
                                    operationItem.AveragePrice = price;
                                    operationItem.EventDate = operationDate.Add(DateTime.Now.TimeOfDay);
                                    operationItem.HomeBroker = string.IsNullOrWhiteSpace(operationItemEditVM.Broker) ? "ND" : operationItemEditVM.Broker;
                                    operationItem.AcquisitionPrice = acquisitionPrice;

                                    _operationItemService.Insert(operationItem);

                                }
                            }
                        }
                        else if (operationItemHist != null)
                        {
                            operationItemHist.Active = false;
                            _operationItemHistService.Update(operationItemHist);

                            operationItemHist.IdOperationItemHist = 0;
                            operationItemHist.GuidOperationItemHist = Guid.Empty;
                            operationItemHist.NumberOfShares = numberOfSharesInput;
                            operationItemHist.AveragePrice = price;
                            operationItemHist.EventDate = operationDate.Add(DateTime.Now.TimeOfDay);
                            operationItemHist.HomeBroker = string.IsNullOrWhiteSpace(operationItemEditVM.Broker) ? "ND" : operationItemEditVM.Broker;
                            operationItemHist.AcquisitionPrice = acquisitionPrice;

                            _operationItemHistService.Insert(operationItemHist);
                        }
                    }
                }
            }
            else
            {
                resultResponseBase.Success = false;
            }


            return resultResponseBase;
        }

        public ResultResponseBase InactiveOperationItem(Guid guidOperationItem)
        {
            ResultResponseBase resultResponseBase = new ResultResponseBase();
            resultResponseBase.Success = true;

            using (_uow.Create())
            {
                ResultServiceObject<OperationItem> resultOperationItem = _operationItemService.GetByGuidOperationItem(guidOperationItem);
                ResultServiceObject<OperationItemHist> resultOperationItemHist = _operationItemHistService.GetByGuidOperationItem(guidOperationItem);

                if (resultOperationItem.Success && resultOperationItemHist.Success)
                {
                    OperationItem operationItem = resultOperationItem.Value;
                    OperationItemHist operationItemHist = resultOperationItemHist.Value;

                    if (operationItem != null)
                    {
                        ResultServiceObject<Portfolio> resultPortfolio = _portfolioService.GetByIdOperationItem(operationItem.IdOperationItem);

                        if (resultPortfolio.Value != null)
                        {
                            if (resultPortfolio.Value.ManualPortfolio)
                            {
                                InactiveOperationItem(resultPortfolio.Value.GuidPortfolio, operationItem.IdOperationItem, resultResponseBase);
                            }
                            else
                            {
                                ResultServiceObject<Operation> resultOperation = _operationService.GetById(operationItem.IdOperation);

                                if (resultOperation.Success && resultOperation.Value != null)
                                {
                                    Operation operation = resultOperation.Value;

                                    operation.NumberOfShares -= operationItem.NumberOfShares;

                                    if (operation.NumberOfShares <= 0)
                                    {
                                        operation.NumberOfShares = 0;
                                        operation.AveragePrice = 0;
                                        operation.Active = false;
                                    }

                                    _operationService.Update(operation);

                                    operationItem.Active = false;
                                    _operationItemService.Inactive(operationItem.IdOperationItem);
                                }
                            }
                        }
                    }
                    else if (operationItemHist != null)
                    {
                        ResultServiceObject<OperationHist> resultOperationHist = _operationHistService.GetById(operationItemHist.IdOperationHist);

                        if (resultOperationHist.Success && resultOperationHist.Value != null)
                        {
                            OperationHist operationHist = resultOperationHist.Value;

                            operationHist.NumberOfShares -= operationItemHist.NumberOfShares;

                            if (operationHist.NumberOfShares <= 0)
                            {
                                operationHist.NumberOfShares = 0;
                                operationHist.AveragePrice = 0;
                                operationHist.Active = false;
                            }

                            _operationHistService.Update(operationHist);

                            operationItemHist.Active = false;
                            _operationItemHistService.Update(operationItemHist);
                        }
                    }
                }
            }

            if (resultResponseBase.ErrorMessages != null && resultResponseBase.ErrorMessages.Count() > 0)
            {
                resultResponseBase.Success = false;
            }

            return resultResponseBase;
        }

        public ResultResponseBase AddSellOperationIem(Guid guidPortfolio, OperationItemAddVM operationItemAddVM)
        {
            ResultResponseBase resultResponseBase = new ResultResponseBase();
            resultResponseBase.Success = true;

            decimal numberOfSharesInput = 0;
            decimal.TryParse(operationItemAddVM.NumberOfShares.Replace(".", string.Empty), NumberStyles.Currency, new CultureInfo("pt-br"), out numberOfSharesInput);
            decimal price = 0;
            decimal.TryParse(operationItemAddVM.Price.Replace(".", string.Empty), NumberStyles.Currency, new CultureInfo("pt-br"), out price);
            decimal acquisitionPrice = 0;
            decimal.TryParse(operationItemAddVM.AcquisitionPrice.Replace(".", string.Empty), NumberStyles.Currency, new CultureInfo("pt-br"), out acquisitionPrice);
            DateTime operationDate = DateTime.MinValue;

            if (numberOfSharesInput <= 0)
            {
                resultResponseBase.ErrorMessages.Add("Quantidade deve ser maior que zero");
            }
            else if (price <= 0)
            {
                resultResponseBase.ErrorMessages.Add("Preço de Venda deve ser informado");
            }
            else if (acquisitionPrice <= 0)
            {
                resultResponseBase.ErrorMessages.Add("Preço de Compra deve ser informado");
            }
            else if (string.IsNullOrWhiteSpace(operationItemAddVM.OperationDate))
            {
                resultResponseBase.ErrorMessages.Add("A data da operação é inválida");
            }
            else
            {
                if (!DateTime.TryParseExact(operationItemAddVM.OperationDate, "dd/MM/yyyy", new CultureInfo("pt-br"), DateTimeStyles.None, out operationDate))
                {
                    if (!DateTime.TryParseExact(operationItemAddVM.OperationDate, "dd/MM/yy", new CultureInfo("pt-br"), DateTimeStyles.None, out operationDate))
                    {
                        resultResponseBase.ErrorMessages.Add("A data da operação é inválida");
                    }
                }
            }

            SubPortfolio subportfolio = null;

            if (resultResponseBase.ErrorMessages == null || resultResponseBase.ErrorMessages.Count() == 0)
            {
                using (_uow.Create())
                {
                    ResultServiceObject<Portfolio> resultPortfolio = _portfolioService.GetByGuid(guidPortfolio);
                    ResultServiceObject<SubPortfolio> resultSubPortfolio = _subPortfolioService.GetByGuid(guidPortfolio);

                    if (resultSubPortfolio.Success && resultPortfolio.Success)
                    {
                        Portfolio portfolio = resultPortfolio.Value;
                        subportfolio = resultSubPortfolio.Value;

                        if (subportfolio != null)
                        {
                            resultPortfolio = _portfolioService.GetById(subportfolio.IdPortfolio);
                        }
                    }

                    if (resultPortfolio.Success && resultPortfolio.Value != null && operationItemAddVM != null)
                    {
                        if (resultPortfolio.Value.ManualPortfolio)
                        {
                            ResultServiceObject<Operation> resultOperationBuy = _operationService.GetByPortfolioAndIdStock(resultPortfolio.Value.IdPortfolio, operationItemAddVM.IdStock, 1);

                            if (resultOperationBuy.Success)
                            {
                                if (resultOperationBuy.Value != null)
                                {
                                    if (resultOperationBuy.Value.Active)
                                    {
                                        OperationAddVM operationAddVM = new OperationAddVM();
                                        operationAddVM.AcquisitionPrice = operationItemAddVM.AcquisitionPrice;
                                        operationAddVM.Broker = operationItemAddVM.Broker;
                                        operationAddVM.IdStock = operationItemAddVM.IdStock;
                                        operationAddVM.NumberOfShares = operationItemAddVM.NumberOfShares;
                                        operationAddVM.OperationDate = operationItemAddVM.OperationDate;
                                        operationAddVM.Price = operationItemAddVM.Price;

                                        SellStock(guidPortfolio, operationAddVM, resultResponseBase);
                                    }
                                    else
                                    {
                                        OperationHist operationHist = null;
                                        ResultServiceObject<OperationHist> resultOperationHistSell = _operationHistService.GetByPortfolioAndIdStock(resultPortfolio.Value.IdPortfolio, operationItemAddVM.IdStock, 2);

                                        if (resultOperationHistSell.Success && resultOperationHistSell.Value != null)
                                        {
                                            operationHist = resultOperationHistSell.Value;

                                            decimal numberSharesSell = operationHist.NumberOfShares + numberOfSharesInput;
                                            decimal sumPrice = operationHist.AveragePrice + (numberOfSharesInput * price);
                                            operationHist.AveragePrice = sumPrice / numberSharesSell;
                                            operationHist.NumberOfShares = numberSharesSell;
                                            operationHist.Active = true;
                                            _operationHistService.Update(operationHist);
                                        }
                                        else
                                        {
                                            operationHist = new OperationHist();
                                            operationHist.IdOperationType = 2;
                                            operationHist.IdPortfolio = resultPortfolio.Value.IdPortfolio;
                                            operationHist.IdStock = operationItemAddVM.IdStock;
                                            operationHist.NumberOfShares = numberOfSharesInput;
                                            operationHist.AveragePrice = price / operationHist.NumberOfShares;

                                            operationHist = _operationHistService.Insert(operationHist).Value;
                                        }

                                        OperationItemHist operationItemHist = new OperationItemHist();
                                        operationItemHist.IdOperationItemHist = 0;
                                        operationItemHist.GuidOperationItemHist = Guid.Empty;
                                        operationItemHist.NumberOfShares = numberOfSharesInput;
                                        operationItemHist.AveragePrice = price;
                                        operationItemHist.EventDate = operationDate.Add(DateTime.Now.TimeOfDay);
                                        operationItemHist.HomeBroker = string.IsNullOrWhiteSpace(operationItemAddVM.Broker) ? "ND" : operationItemAddVM.Broker;
                                        operationItemHist.AcquisitionPrice = acquisitionPrice;
                                        operationItemHist.IdStock = operationItemAddVM.IdStock;
                                        operationItemHist.IdOperationHist = operationHist.IdOperationHist;
                                        operationItemHist.IdOperationType = 2;
                                        _operationItemHistService.Insert(operationItemHist);
                                    }

                                    if (subportfolio != null)
                                    {
                                        ResultServiceObject<SubPortfolioOperation> resultSubPortfolioOperation = _subPortfolioOperationService.GetBySubPortfolioAndIdOperation(subportfolio.IdSubPortfolio, resultOperationBuy.Value.IdOperation);

                                        if (resultSubPortfolioOperation.Success)
                                        {
                                            if (resultSubPortfolioOperation.Value == null)
                                            {
                                                SubPortfolioOperation subPortfolioOperation = new SubPortfolioOperation();
                                                subPortfolioOperation.IdOperation = resultOperationBuy.Value.IdOperation;
                                                subPortfolioOperation.IdSubPortfolio = subportfolio.IdSubPortfolio;
                                                _subPortfolioOperationService.Insert(subPortfolioOperation);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    resultResponseBase.ErrorMessages.Add("Este ativo não faz parte da sua carteira. Venda cancelada.");
                                }
                            }
                        }
                        else
                        {
                            OperationHist operationHist = null;
                            ResultServiceObject<OperationHist> resultOperationHistSell = _operationHistService.GetByPortfolioAndIdStock(resultPortfolio.Value.IdPortfolio, operationItemAddVM.IdStock, 2);

                            if (resultOperationHistSell.Success)
                            {
                                if (resultOperationHistSell.Value != null)
                                {
                                    operationHist = resultOperationHistSell.Value;

                                    decimal numberSharesSell = operationHist.NumberOfShares + numberOfSharesInput;
                                    decimal sumPrice = operationHist.AveragePrice + (numberOfSharesInput * price);
                                    operationHist.Active = true;
                                    operationHist.NumberOfShares = numberSharesSell;
                                    operationHist.AveragePrice = sumPrice / numberSharesSell;
                                    _operationHistService.Update(operationHist);
                                }
                                else
                                {
                                    operationHist = new OperationHist();
                                    operationHist.IdOperationType = 2;
                                    operationHist.IdPortfolio = resultPortfolio.Value.IdPortfolio;
                                    operationHist.IdStock = operationItemAddVM.IdStock;
                                    operationHist.NumberOfShares = numberOfSharesInput;
                                    operationHist.AveragePrice = price / operationHist.NumberOfShares;

                                    operationHist = _operationHistService.Insert(operationHist).Value;
                                }

                                OperationItemHist operationItemHist = new OperationItemHist();
                                operationItemHist.IdOperationItemHist = 0;
                                operationItemHist.GuidOperationItemHist = Guid.Empty;
                                operationItemHist.NumberOfShares = numberOfSharesInput;
                                operationItemHist.AveragePrice = price;
                                operationItemHist.EventDate = operationDate.Add(DateTime.Now.TimeOfDay);
                                operationItemHist.HomeBroker = string.IsNullOrWhiteSpace(operationItemAddVM.Broker) ? "ND" : operationItemAddVM.Broker;
                                operationItemHist.AcquisitionPrice = acquisitionPrice;
                                operationItemHist.IdStock = operationItemAddVM.IdStock;
                                operationItemHist.IdOperationHist = operationHist.IdOperationHist;
                                operationItemHist.IdOperationType = 2;
                                _operationItemHistService.Insert(operationItemHist);
                            }
                        }

                        if (subportfolio != null)
                        {
                            ResultServiceObject<Operation> resultOperationSell = _operationService.GetByPortfolioAndIdStock(resultPortfolio.Value.IdPortfolio, operationItemAddVM.IdStock, 2);

                            if (resultOperationSell.Success)
                            {
                                Operation operationSell = resultOperationSell.Value;

                                if (operationSell == null)
                                {
                                    operationSell = new Operation();
                                    operationSell.IdStock = operationItemAddVM.IdStock;
                                    operationSell.Active = false;
                                    operationSell.AveragePrice = 0;
                                    operationSell.NumberOfShares = 0;
                                    operationSell.IdOperationType = 2;
                                    operationSell.IdPortfolio = resultPortfolio.Value.IdPortfolio;
                                    operationSell = _operationService.Insert(operationSell).Value;
                                }

                                if (operationSell != null)
                                {
                                    ResultServiceObject<SubPortfolioOperation> resultSubPortfolioOperation = _subPortfolioOperationService.GetBySubPortfolioAndIdOperation(subportfolio.IdSubPortfolio, operationSell.IdOperation);

                                    if (resultSubPortfolioOperation.Success)
                                    {
                                        if (resultSubPortfolioOperation.Value == null)
                                        {
                                            SubPortfolioOperation subPortfolioOperation = new SubPortfolioOperation();
                                            subPortfolioOperation.IdOperation = operationSell.IdOperation;
                                            subPortfolioOperation.IdSubPortfolio = subportfolio.IdSubPortfolio;
                                            _subPortfolioOperationService.Insert(subPortfolioOperation);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }

            if (resultResponseBase.ErrorMessages != null && resultResponseBase.ErrorMessages.Count() > 0)
            {
                resultResponseBase.Success = false;
            }

            return resultResponseBase;
        }

        internal static string FormatNumber(long num)
        {
            num = MaxThreeSignificantDigits(num);

            //if (num >= 100000000)
            //    return (num / 1000000D).ToString("0.#M");
            //if (num >= 1000000)
            //    return (num / 1000000D).ToString("0.##M");
            //if (num >= 100000)
            //    return (num / 1000D).ToString("0k");
            //if (num >= 10000)
            //    return (num / 1000D).ToString("0.#k");
            if (num >= 1000)
                return (num / 1000D).ToString("0.###k");
            return num.ToString("#,0");
        }


        internal static long MaxThreeSignificantDigits(long x)
        {
            int i = (int)Math.Log10(x);
            i = Math.Max(0, i - 3);
            i = (int)Math.Pow(10, i);
            return x / i * i;
        }

        public ResultResponseObject<OperationEditAvgPriceVM> Adjust(Guid guidOperation, OperationEditAvgPriceVM operationEditVM)
        {
            ResultResponseBase resultResponseBase = new ResultResponseBase();
            resultResponseBase.Success = true;
            ResultResponseObject<OperationEditAvgPriceVM> resultResponse = new ResultResponseObject<OperationEditAvgPriceVM>();
            resultResponse.Success = false;

            using (_uow.Create())
            {
                _operationService.Adjust(guidOperation, operationEditVM, _portfolioService, _operationService, _operationItemService,
                    _operationHistService, _operationItemHistService, _systemSettingsService, _portfolioPerformanceService, _stockService, _performanceStockService, _holidayService);
            }

            return resultResponse;
        }
    }
}