using AutoMapper;
using Dividendos.API.Model.Request;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using Dividendos.API.Model.Response.FinancialProducts;
using Dividendos.Application.Base;
using Dividendos.Application.Interface;
using Dividendos.Application.Interface.Model;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Enum;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Dividendos.Application
{
    public class PatrimonyApp : BaseApp, IPatrimonyApp
    {
        private readonly IFinancialProductService _financialProductService;
        private readonly IUnitOfWork _uow;
        private readonly IGlobalAuthenticationService _globalAuthenticationService;
        private readonly ICryptoCurrencyService _cryptoCurrencyService;
        private readonly IIndicatorSeriesService _indicatorSeriesService;
        private readonly ISubscriptionService _subscriptionService;
        private readonly IPortfolioService _portfolioService;
        private readonly IPortfolioPerformanceService _portfolioPerformanceService;
        private readonly ICryptoPortfolioPerformanceService _cryptoPortfolioPerformanceService;
        private readonly ITraderService _traderService;
        private readonly IMapper _mapper;
        private readonly ICryptoPortfolioService _cryptoPortfolioService;
        private readonly IFiatCurrencyService _fiatCurrencyService;

        public PatrimonyApp(IUnitOfWork uow,
            IFinancialProductService financialProductService,
            IGlobalAuthenticationService globalAuthenticationService,
            ICryptoCurrencyService cryptoCurrencyService,
            IIndicatorSeriesService indicatorSeriesService,
            ISubscriptionService subscriptionService,
            IPortfolioService portfolioService,
            IPortfolioPerformanceService portfolioPerformanceService,
            IMapper mapper,
            ITraderService traderService,
            ICryptoPortfolioService cryptoPortfolioService,
            ICryptoPortfolioPerformanceService cryptoPortfolioPerformanceService,
            IFiatCurrencyService fiatCurrencyService)
        {
            _financialProductService = financialProductService;
            _uow = uow;
            _globalAuthenticationService = globalAuthenticationService;
            _cryptoCurrencyService = cryptoCurrencyService;
            _indicatorSeriesService = indicatorSeriesService;
            _subscriptionService = subscriptionService;
            _portfolioService = portfolioService;
            _portfolioPerformanceService = portfolioPerformanceService;
            _mapper = mapper;
            _traderService = traderService;
            _cryptoPortfolioService = cryptoPortfolioService;
            _cryptoPortfolioPerformanceService = cryptoPortfolioPerformanceService;
            _fiatCurrencyService = fiatCurrencyService;
        }

        public ResultResponseObject<PatrimonyVM> GetPatrimony()
        {
            PatrimonyVM patrimonyVM = new PatrimonyVM();
            CryptoCurrency btcCurrency = new CryptoCurrency();
            decimal patrimonyInDolar = 0, patrimonyInReal = 0, dollarExchangeRate = 0, patrimonyInBTC = 0, btcQuotation = 0;

            bool hasSubscription = false;

            using (_uow.Create())
            {
                ResultServiceObject<Subscription> subscription = _subscriptionService.GetByUser(_globalAuthenticationService.IdUser);

                if (subscription.Value != null && subscription.Value.Active && (subscription.Value.SubscriptionTypeID.Equals((int)SubscriptionTypeEnum.Gold) ||
                                                                                subscription.Value.SubscriptionTypeID.Equals((int)SubscriptionTypeEnum.Annuity)))
                {
                    hasSubscription = true;
                }

                var resultService = _financialProductService.GetAllProductsByUser(_globalAuthenticationService.IdUser);
                ResultServiceObject<IEnumerable<Entity.Entities.CryptoCurrency>> cryptoCurrencies = _cryptoCurrencyService.GetAll();
                btcCurrency = cryptoCurrencies.Value.FirstOrDefault(currency => currency.Name.Equals("btc"));
                btcQuotation = btcCurrency.MarketPrice;


                foreach (var item in resultService.Value)
                {
                    if (item.ProductCategoryID.Equals((int)ProductCategoryEnum.CryptoCurrencies))
                    {
                        var valueWithMarketPrice = cryptoCurrencies.Value.Where(currency => currency.Name.Equals(item.ExternalName)).Sum(totalSum => totalSum.MarketPrice) * item.CurrentValue;

                        //Calculate crypto to BTC
                        valueWithMarketPrice = valueWithMarketPrice / btcQuotation;

                        patrimonyInBTC = patrimonyInBTC + valueWithMarketPrice;
                    }
                    else
                    {
                        patrimonyInReal = patrimonyInReal + item.CurrentValue;
                    }
                }
            }

            ResultServiceObject<IEnumerable<Portfolio>> portfolios;
            ResultServiceObject<IEnumerable<CryptoPortfolio>> cryptoPortfolios;

            using (_uow.Create())
            {
                portfolios = _portfolioService.GetByUser(_globalAuthenticationService.IdUser, true, true);
                cryptoPortfolios = _cryptoPortfolioService.GetByUser(_globalAuthenticationService.IdUser, true, true);
                ResultServiceObject<IEnumerable<IndicatorSeriesView>> dolarIndicator = _indicatorSeriesService.GetLatest((int)IndicatorTypeEnum.USDBRL);
                ResultServiceObject<IEnumerable<IndicatorSeriesView>> euroIndicator = _indicatorSeriesService.GetLatest((int)IndicatorTypeEnum.EURBRL);

                var dolar = dolarIndicator.Value.FirstOrDefault();

                if (dolar != null)
                {
                    dollarExchangeRate = dolarIndicator.Value.FirstOrDefault().Points;
                }

                foreach (Portfolio itemPortfolio in portfolios.Value)
                {
                    ResultServiceObject<PortfolioPerformance> resultServiceObject = _portfolioPerformanceService.GetByCalculationDate(itemPortfolio.IdPortfolio);

                    if (resultServiceObject != null && resultServiceObject.Value != null)
                    {
                        if (itemPortfolio.IdCountry.Equals((int)CountryEnum.EUA))
                        {
                            if (hasSubscription)
                            {
                                patrimonyInDolar = patrimonyInDolar + resultServiceObject.Value.TotalMarket;
                            }
                        }
                        else
                        {
                            patrimonyInReal = patrimonyInReal + resultServiceObject.Value.TotalMarket;
                        }
                    }
                }

                foreach (CryptoPortfolio itemPortfolio in cryptoPortfolios.Value)
                {
                    ResultServiceObject<CryptoPortfolioPerformance> resultServiceObject = _cryptoPortfolioPerformanceService.GetByCalculationDate(itemPortfolio.IdCryptoPortfolio, DateTime.Now);

                    if (resultServiceObject != null && resultServiceObject.Value != null)
                    {
                        if (itemPortfolio.IdFiatCurrency.Equals((int)FiatCurrencyEnum.BRL))
                        {
                            var value = resultServiceObject.Value.TotalMarket / btcCurrency.MarketPrice;
                            patrimonyInBTC = patrimonyInBTC + value;
                        }
                        else if (itemPortfolio.IdFiatCurrency.Equals((int)FiatCurrencyEnum.EURO))
                        {
                            var value = resultServiceObject.Value.TotalMarket / btcCurrency.MarketPriceUSD;
                            patrimonyInBTC = patrimonyInBTC + value;
                        }
                        else if (itemPortfolio.IdFiatCurrency.Equals((int)FiatCurrencyEnum.USD))
                        {
                            var value = resultServiceObject.Value.TotalMarket / btcCurrency.MarketPriceEuro;
                            patrimonyInBTC = patrimonyInBTC + value;
                        }
                    }
                }
            }

            patrimonyVM.PatrimonyInDolarN = patrimonyInDolar;
            patrimonyVM.PatrimonyInBTCN = patrimonyInBTC;
            patrimonyVM.PatrimonyInRealN = patrimonyInReal;

            patrimonyVM.PatrimonyInDolar = patrimonyInDolar.ToString("n2", new CultureInfo("pt-br"));
            patrimonyVM.PatrimonyInBTC = patrimonyInBTC.ToString("n8", new CultureInfo("pt-br"));
            patrimonyVM.PatrimonyInReal = patrimonyInReal.ToString("n2", new CultureInfo("pt-br"));

            patrimonyVM.PatrimonyTotalInDolar = (patrimonyInDolar + (patrimonyInReal / dollarExchangeRate) + (patrimonyInBTC * btcCurrency.MarketPriceUSD)).ToString("n2", new CultureInfo("pt-br"));
            patrimonyVM.PatrimonyTotalInReal = ((patrimonyInDolar * dollarExchangeRate) + patrimonyInReal + (patrimonyInBTC * btcCurrency.MarketPrice)).ToString("n2", new CultureInfo("pt-br"));

            //percents
            decimal totalGeneral = (patrimonyInDolar * dollarExchangeRate) + patrimonyInReal + (patrimonyInBTC * btcQuotation);

            if (totalGeneral != 0)
            {
                patrimonyVM.TotalPercentDolar = ((patrimonyInDolar * dollarExchangeRate) / totalGeneral) * 100;
                patrimonyVM.TotalPercentReal = (patrimonyInReal / totalGeneral) * 100;
                patrimonyVM.TotalPercentBTC = ((patrimonyInBTC * btcCurrency.MarketPrice) / totalGeneral) * 100;
            }

            ResultResponseObject<PatrimonyVM> resultResponseObject = new ResultResponseObject<PatrimonyVM>();
            resultResponseObject.Value = patrimonyVM;
            resultResponseObject.Success = true;

            return resultResponseObject;
        }


        public ResultResponseObject<List<PatrimonyComposeResponse>> GetPatrimonyCompose()
        {
            List<PatrimonyComposeResponse> patrimonyComposeResponses = new List<PatrimonyComposeResponse>();

            using (_uow.Create())
            {
                var resultService = _traderService.GetItemsComposePatrimony(_globalAuthenticationService.IdUser);

                foreach (var item in resultService.Value)
                {
                    patrimonyComposeResponses.Add(new PatrimonyComposeResponse() { ShowOnPatrimony = item.ShowOnPatrimony, TraderGuid = item.GuidTrader, TraderName = item.Name });
                }
            }

            ResultResponseObject<List<PatrimonyComposeResponse>> result = new ResultResponseObject<List<PatrimonyComposeResponse>>();
            result.Value = patrimonyComposeResponses;
            result.Success = true;

            return result;
        }

        public void ChangePatrimonyCompose(IEnumerable<PatrimonyComposeRequest> patrimonyComposeRequests)
        {
            using (_uow.Create())
            {
                foreach (var item in patrimonyComposeRequests)
                {
                    _traderService.ChangeComposePatrimony(item.TraderGuid, item.ShowOnPatrimony);
                }
            }
        }
    }
}