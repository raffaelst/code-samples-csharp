using AutoMapper;
using Dividendos.API.Model.Response.Common;
using Dividendos.Application.Interface;
using Dividendos.CrossCutting.Identity.Models;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.MercadoBitcoin.Interface;
using Dividendos.MercadoBitcoin.Interface.Model;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using K.Logger;
using K.Logger.Model.In;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Globalization;
using Dividendos.API.Model.Response.v2;
using Dividendos.Entity.Views;
using Dividendos.API.Model.Request.Stock;
using Dividendos.Passfolio.Interface;
using Dividendos.Entity.Enum;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Crypto;
using Dividendos.Binance;
using Dividendos.CoinMarketCap;
using Dividendos.CoinMarketCap.Interface.Model;
using Dividendos.API.Model.Request.Crypto;
using Newtonsoft.Json;

namespace Dividendos.Application
{
    public class CryptoCurrencyApp : ICryptoCurrencyApp
    {
        private readonly ICurrenciesQuotations _currenciesQuotations;
        private readonly ICryptoCurrencyService  _cryptoCurrencyService;
        private readonly IUnitOfWork _uow;
        private readonly IPassfolioHelper _passfolioHelper;
        private readonly IMapper _mapper;
        private readonly IIndicatorSeriesService _indicatorSeriesService;
        private readonly ISystemSettingsService _systemSettingsService;
        private readonly IFinancialProductService _financialProductService;
        private readonly IGlobalAuthenticationService _globalAuthenticationService;
        private readonly IBinanceHelper _binanceHelper;
        private readonly ICoinMarketCapHelper _coinMarketCapHelper;
        private readonly ILogoService _logoService;
        private readonly ICacheService _cacheService;

        public CryptoCurrencyApp(
            IUnitOfWork uow,
             ICurrenciesQuotations currenciesQuotations,
             ICryptoCurrencyService cryptoCurrencyService,
             IPassfolioHelper passfolioHelper,
             IMapper mapper,
             IIndicatorSeriesService indicatorSeriesService,
             ISystemSettingsService systemSettingsService,
             IFinancialProductService financialProductService,
             IGlobalAuthenticationService globalAuthenticationService,
             IBinanceHelper binanceHelper,
             ICoinMarketCapHelper coinMarketCapHelper,
             ILogoService logoService,
             ICacheService cacheService)
        {
            _uow = uow;
            _currenciesQuotations = currenciesQuotations;
            _cryptoCurrencyService = cryptoCurrencyService;
            _passfolioHelper = passfolioHelper;
            _mapper = mapper;
            _indicatorSeriesService = indicatorSeriesService;
            _systemSettingsService = systemSettingsService;
            _financialProductService = financialProductService;
            _globalAuthenticationService = globalAuthenticationService;
            _binanceHelper = binanceHelper;
            _coinMarketCapHelper = coinMarketCapHelper;
            _logoService = logoService;
            _cacheService = cacheService;
        }

        public void SyncCryptoCurrencyPrice()
        {
            using (_uow.Create())
            {
                ResultServiceObject<IEnumerable<Entity.Entities.CryptoCurrency>> cryptoCurrencies = _cryptoCurrencyService.GetAll();

                StringBuilder stringBuilder = new StringBuilder();

                bool firstExecutionLoop = true;

                foreach (var itemCrypto in cryptoCurrencies.Value)
                {
                    if (firstExecutionLoop)
                    {
                        firstExecutionLoop = false;
                        stringBuilder.Append(itemCrypto.Name);
                    }
                    else
                    {
                        stringBuilder.Append(string.Concat(",", itemCrypto.Name));
                    }
                }

                var keysCoinMarketCap = _systemSettingsService.GetByKey("KeyCoinMarketCap").Value.SettingValue.Split(";");

                var keyCoinMarketCap = DateTime.Now.Hour % 2 == 0 ? keysCoinMarketCap[0] : keysCoinMarketCap[1];

                List<TickerCrypto> tickers = _coinMarketCapHelper.GetQuotationOfCryptos(stringBuilder.ToString(), keyCoinMarketCap);

                if(tickers.Count == 0)
                {
                    tickers = _coinMarketCapHelper.GetQuotationOfCryptos(stringBuilder.ToString(), "94130164-4089-412c-8015-fe331dcd1c0b");
                }

                ResultServiceObject<IEnumerable<IndicatorSeriesView>> dolarIndicators = _indicatorSeriesService.GetLatest((int)IndicatorTypeEnum.USDBRL);
                ResultServiceObject<IEnumerable<IndicatorSeriesView>> euroIndicators = _indicatorSeriesService.GetLatest((int)IndicatorTypeEnum.EURBRL);

                decimal? dollarExchangeRate = null;
                decimal? euroExchangeRate = null;

                if (dolarIndicators.Value != null && dolarIndicators.Value.Count() > 0)
                {
                    dollarExchangeRate = dolarIndicators.Value.FirstOrDefault().Points;
                }

                if (euroIndicators.Value != null && euroIndicators.Value.Count() > 0)
                {
                    euroExchangeRate = euroIndicators.Value.FirstOrDefault().Points;
                }

                foreach (var item in tickers)
                {
                    Entity.Entities.CryptoCurrency cryptoCurrency = cryptoCurrencies.Value.FirstOrDefault(itemSelect => itemSelect.Name.ToLower().Equals(item.Symbol.ToLower()));

                    decimal marketPrice = 0;
                    decimal marketPriceUsd = item.Price;
                    decimal marketPriceEuro = 0;

                    if (dollarExchangeRate.HasValue)
                    {
                        marketPrice = item.Price * dollarExchangeRate.Value;
                    }

                    if (euroExchangeRate.HasValue && euroExchangeRate.Value > 0)
                    {
                        marketPriceEuro = marketPrice / euroExchangeRate.Value;
                    }

                    if (cryptoCurrency != null)
                    {
                        try
                        {
                            cryptoCurrency.MarketPrice = decimal.Round(marketPrice, 8);
                            //cryptoCurrency.Volume24h = item.Volume24h;
                            cryptoCurrency.PercentChange1h = decimal.Round(item.PercentChange1h, 8);
                            cryptoCurrency.PercentChange24h = decimal.Round(item.PercentChange24h, 8);
                            cryptoCurrency.PercentChange7d = decimal.Round(item.PercentChange7d, 8);
                            cryptoCurrency.PercentChange30d = decimal.Round(item.PercentChange30d, 8);
                            cryptoCurrency.PercentChange60d = decimal.Round(item.PercentChange60d, 8);
                            cryptoCurrency.PercentChange90d = decimal.Round(item.PercentChange90d, 8);
                            cryptoCurrency.Variation = decimal.Round(item.PercentChange24h, 2);
                            cryptoCurrency.UpdatedDate = DateTime.Now;
                            cryptoCurrency.MarketPriceUSD = marketPriceUsd;
                            cryptoCurrency.MarketPriceEuro = marketPriceEuro;
                            _cryptoCurrencyService.Update(cryptoCurrency);
                        }
                        catch
                        {
                            
                        }
                    }
                    else
                    {
                        //cryptoCurrency = new Entity.Entities.CryptoCurrency();
                        //cryptoCurrency.LogoID = 8487;
                        //cryptoCurrency.Name = item.Symbol.ToLower();
                        //_cryptoCurrencyService.Insert(cryptoCurrency);

                        //_financialProductService.AddProduct(new Product() { Description = item.Name, ExternalName = item.Symbol.ToLower(), ProductGuid = Guid.NewGuid(), ProductCategoryID = 2 });
                    }
                }
            }
        }

 
        public ResultResponseObject<CryptoPortfolioStatementWrapperVM> GetCryptosByTrader(Guid? traderGuid)
        {
            ResultResponseObject<CryptoPortfolioStatementWrapperVM> result = new ResultResponseObject<CryptoPortfolioStatementWrapperVM>();

            using (_uow.Create())
            {
                CryptoPortfolioStatementWrapperVM portfolioStatementWrapperVM = new CryptoPortfolioStatementWrapperVM();
                portfolioStatementWrapperVM.GuidPortfolioSubPortfolio = Guid.NewGuid();
                portfolioStatementWrapperVM.ManualPortfolio = true;
                portfolioStatementWrapperVM.LastUpdated = DateTime.Now.ToString("HH:mm:ss");
                List<CryptoStatementVM> resultModel = new List<CryptoStatementVM>();

                ResultServiceObject<IEnumerable<CryptoStatementView>> cryptoStatements = _cryptoCurrencyService.GetCryptosWithLogoByTrader(traderGuid, _globalAuthenticationService.IdUser);

                ResultServiceObject<IEnumerable<IndicatorSeriesView>> dolarIndicator = _indicatorSeriesService.GetLatest((int)IndicatorTypeEnum.USDBRL);
                var dollarExchangeRate = dolarIndicator.Value.FirstOrDefault().Points;

                CryptoStatementView cryptoStatementView = _cryptoCurrencyService.GetCryptosByNameOrSymbol("btc").Value.FirstOrDefault();

                decimal netValueTotalBRL = 0, netValueTotalUSD = 0, netValueTotalBTC =0;


                foreach (CryptoStatementView cryptoView in cryptoStatements.Value)
                {
                    decimal netValue = cryptoView.NumberOfShares * cryptoView.MarketPrice;
                    decimal netValueUSD = netValue / dollarExchangeRate;
                    decimal netValueBTC = netValue / cryptoStatementView.MarketPrice;

                    CryptoStatementVM portfolioViewModel = new CryptoStatementVM();
                    portfolioViewModel.GuidOperation = cryptoView.CryptoCurrencyGuid;
                    portfolioViewModel.IdStock = cryptoView.IdProductUser;
                    portfolioViewModel.IdCountry =(int)CountryType.Brazil;
                    portfolioViewModel.Company = cryptoView.Company;
                    portfolioViewModel.Segment = cryptoView.Segment;
                    portfolioViewModel.Symbol = cryptoView.Symbol.ToUpper();
                    portfolioViewModel.Logo = cryptoView.Logo;

                    portfolioViewModel.PerformancePerc = GetSignal(cryptoView.PerformancePerc) + cryptoView.PerformancePerc.ToString("n2", new CultureInfo("pt-br"));
                    portfolioViewModel.PerformancePercN = cryptoView.PerformancePerc;

                    if (cryptoView.AveragePrice != null && cryptoView.AveragePrice > 0)
                    {
                        var variation = ((cryptoView.MarketPrice - cryptoView.AveragePrice.Value) / cryptoView.AveragePrice.Value) * 100;
                        portfolioViewModel.YourPerformancePerc = GetSignal(variation) + variation.ToString("n2", new CultureInfo("pt-br"));

                        var variationReal = ((cryptoView.MarketPrice / 100) * variation);

                        var variationDolar = (((cryptoView.MarketPrice / dollarExchangeRate) / 100) * variation);

                        portfolioViewModel.VariationInDolarN = variationDolar;
                        portfolioViewModel.VariationInDolar = variationDolar.ToString("n2", new CultureInfo("pt-br"));

                        portfolioViewModel.VariationInRealN = variationReal;
                        portfolioViewModel.VariationInReal = variationReal.ToString("n2", new CultureInfo("pt-br"));

                        portfolioViewModel.AveragePriceN = cryptoView.AveragePrice.Value;
                        portfolioViewModel.AveragePrice = cryptoView.AveragePrice.Value.ToString("n2", new CultureInfo("pt-br"));
                    }
                    else
                    {
                        portfolioViewModel.YourPerformancePerc = "N/A";
                        portfolioViewModel.VariationInDolar = "N/A";
                        portfolioViewModel.VariationInReal = "N/A";
                        portfolioViewModel.AveragePrice = "N/A";
                    }

                    portfolioViewModel.MarketPrice = cryptoView.MarketPrice.ToString("n2", new CultureInfo("pt-br"));
                    portfolioViewModel.MarketPriceN = cryptoView.MarketPrice;
                    portfolioViewModel.MarketPriceUSD = (cryptoView.MarketPrice / dollarExchangeRate).ToString("n2", new CultureInfo("pt-br"));
                    portfolioViewModel.MarketPriceUSDN = (cryptoView.MarketPrice / dollarExchangeRate);

                    portfolioViewModel.MarketPriceBTC = (cryptoView.MarketPrice / cryptoStatementView.MarketPrice).ToString("n8", new CultureInfo("pt-br"));
                    portfolioViewModel.MarketPriceBTCN = (cryptoView.MarketPrice / cryptoStatementView.MarketPrice);

                    portfolioViewModel.NumberOfShares = cryptoView.NumberOfShares.ToString("0.#############################", new CultureInfo("pt-br"));
                    portfolioViewModel.PerformancePercN = cryptoView.PerformancePerc;
                    portfolioViewModel.NumberOfSharesN = cryptoView.NumberOfShares;

                    portfolioViewModel.Total = netValue.ToString("n2", new CultureInfo("pt-br"));
                    portfolioViewModel.TotalN = netValue;
                    portfolioViewModel.TotalUSD = netValueUSD.ToString("n2", new CultureInfo("pt-br"));
                    portfolioViewModel.TotalUSDN = netValueUSD;
                    portfolioViewModel.TotalBTC = netValueBTC.ToString("n8", new CultureInfo("pt-br"));
                    portfolioViewModel.TotalBTCN = netValueBTC;
                    portfolioViewModel.Broker = cryptoView.FinancialInstitutionName;

                    resultModel.Add(portfolioViewModel);


                    netValueTotalBRL = netValueTotalBRL + netValue;
                    netValueTotalUSD = netValueTotalUSD + netValueUSD;
                    netValueTotalBTC = netValueTotalBTC + netValueBTC;

                }

                portfolioStatementWrapperVM.TotalInReal = netValueTotalBRL.ToString("n2", new CultureInfo("pt-br"));
                portfolioStatementWrapperVM.TotalInDolar = netValueTotalUSD.ToString("n2", new CultureInfo("pt-br"));
                portfolioStatementWrapperVM.TotalInBTC = netValueTotalBTC.ToString("n8", new CultureInfo("pt-br"));
                portfolioStatementWrapperVM.StocksStatement = resultModel;

                result.Value = portfolioStatementWrapperVM;
                result.Success = true;
            }

            return result;
        }

        public ResultResponseObject<API.Model.Response.v2.Crypto.CryptoPortfolioStatementWrapperVM> GetCryptosByTraderV2(Guid? traderGuid)
        {
            ResultResponseObject<API.Model.Response.v2.Crypto.CryptoPortfolioStatementWrapperVM> result = new ResultResponseObject<API.Model.Response.v2.Crypto.CryptoPortfolioStatementWrapperVM>();

            using (_uow.Create())
            {
                API.Model.Response.v2.Crypto.CryptoPortfolioStatementWrapperVM portfolioStatementWrapperVM = new API.Model.Response.v2.Crypto.CryptoPortfolioStatementWrapperVM();
                portfolioStatementWrapperVM.GuidPortfolioSubPortfolio = Guid.NewGuid();
                portfolioStatementWrapperVM.ManualPortfolio = true;
                portfolioStatementWrapperVM.LastUpdated = DateTime.Now.ToString("HH:mm:ss");
                List<API.Model.Response.v2.Crypto.CryptoStatementVM> resultModel = new List<API.Model.Response.v2.Crypto.CryptoStatementVM>();

                ResultServiceObject<IEnumerable<CryptoStatementView>> cryptoStatements = _cryptoCurrencyService.GetCryptosWithLogoByTrader(traderGuid, _globalAuthenticationService.IdUser);

                ResultServiceObject<IEnumerable<IndicatorSeriesView>> dolarIndicator = _indicatorSeriesService.GetLatest((int)IndicatorTypeEnum.USDBRL);
                var dollarExchangeRate = dolarIndicator.Value.FirstOrDefault().Points;

                CryptoStatementView cryptoStatementView = _cryptoCurrencyService.GetCryptosByNameOrSymbol("btc").Value.FirstOrDefault();

                decimal netValueTotalBRL = 0, netValueTotalUSD = 0, netValueTotalBTC = 0;


                foreach (CryptoStatementView cryptoView in cryptoStatements.Value)
                {
                    decimal netValue = cryptoView.NumberOfShares * cryptoView.MarketPrice;
                    decimal netValueUSD = netValue / dollarExchangeRate;
                    decimal netValueBTC = netValue / cryptoStatementView.MarketPrice;

                    API.Model.Response.v2.Crypto.CryptoStatementVM portfolioViewModel = new API.Model.Response.v2.Crypto.CryptoStatementVM();
                    portfolioViewModel.GuidOperation = cryptoView.CryptoCurrencyGuid;
                    portfolioViewModel.IdStock = cryptoView.IdProductUser;
                    portfolioViewModel.IdCountry = (int)CountryType.Brazil;
                    portfolioViewModel.Company = cryptoView.Company;
                    portfolioViewModel.Segment = cryptoView.Segment;
                    portfolioViewModel.Symbol = cryptoView.Symbol.ToUpper();
                    portfolioViewModel.LogoUrl = cryptoView.LogoUrl;

                    portfolioViewModel.PerformancePerc = GetSignal(cryptoView.PerformancePerc) + cryptoView.PerformancePerc.ToString("n2", new CultureInfo("pt-br"));
                    portfolioViewModel.PerformancePercN = cryptoView.PerformancePerc;

                    if (cryptoView.AveragePrice != null && cryptoView.AveragePrice > 0)
                    {
                        var variation = ((cryptoView.MarketPrice - cryptoView.AveragePrice.Value) / cryptoView.AveragePrice.Value) * 100;
                        portfolioViewModel.YourPerformancePerc = GetSignal(variation) + variation.ToString("n2", new CultureInfo("pt-br"));

                        var variationReal = ((cryptoView.MarketPrice / 100) * variation);

                        var variationDolar = (((cryptoView.MarketPrice / dollarExchangeRate) / 100) * variation);

                        portfolioViewModel.VariationInDolarN = variationDolar;
                        portfolioViewModel.VariationInDolar = variationDolar.ToString("n2", new CultureInfo("pt-br"));

                        portfolioViewModel.VariationInRealN = variationReal;
                        portfolioViewModel.VariationInReal = variationReal.ToString("n2", new CultureInfo("pt-br"));

                        portfolioViewModel.AveragePriceN = cryptoView.AveragePrice.Value;
                        portfolioViewModel.AveragePrice = cryptoView.AveragePrice.Value.ToString("n2", new CultureInfo("pt-br"));
                    }
                    else
                    {
                        portfolioViewModel.YourPerformancePerc = "N/A";
                        portfolioViewModel.VariationInDolar = "N/A";
                        portfolioViewModel.VariationInReal = "N/A";
                        portfolioViewModel.AveragePrice = "N/A";
                    }

                    portfolioViewModel.MarketPrice = cryptoView.MarketPrice.ToString("n2", new CultureInfo("pt-br"));
                    portfolioViewModel.MarketPriceN = cryptoView.MarketPrice;
                    portfolioViewModel.MarketPriceUSD = (cryptoView.MarketPrice / dollarExchangeRate).ToString("n2", new CultureInfo("pt-br"));
                    portfolioViewModel.MarketPriceUSDN = (cryptoView.MarketPrice / dollarExchangeRate);

                    portfolioViewModel.MarketPriceBTC = (cryptoView.MarketPrice / cryptoStatementView.MarketPrice).ToString("n8", new CultureInfo("pt-br"));
                    portfolioViewModel.MarketPriceBTCN = (cryptoView.MarketPrice / cryptoStatementView.MarketPrice);

                    portfolioViewModel.NumberOfShares = cryptoView.NumberOfShares.ToString("0.#############################", new CultureInfo("pt-br"));
                    portfolioViewModel.PerformancePercN = cryptoView.PerformancePerc;
                    portfolioViewModel.NumberOfSharesN = cryptoView.NumberOfShares;

                    portfolioViewModel.Total = netValue.ToString("n2", new CultureInfo("pt-br"));
                    portfolioViewModel.TotalN = netValue;
                    portfolioViewModel.TotalUSD = netValueUSD.ToString("n2", new CultureInfo("pt-br"));
                    portfolioViewModel.TotalUSDN = netValueUSD;
                    portfolioViewModel.TotalBTC = netValueBTC.ToString("n8", new CultureInfo("pt-br"));
                    portfolioViewModel.TotalBTCN = netValueBTC;
                    portfolioViewModel.Broker = cryptoView.FinancialInstitutionName;

                    resultModel.Add(portfolioViewModel);


                    netValueTotalBRL = netValueTotalBRL + netValue;
                    netValueTotalUSD = netValueTotalUSD + netValueUSD;
                    netValueTotalBTC = netValueTotalBTC + netValueBTC;

                }

                portfolioStatementWrapperVM.TotalInReal = netValueTotalBRL.ToString("n2", new CultureInfo("pt-br"));
                portfolioStatementWrapperVM.TotalInDolar = netValueTotalUSD.ToString("n2", new CultureInfo("pt-br"));
                portfolioStatementWrapperVM.TotalInBTC = netValueTotalBTC.ToString("n8", new CultureInfo("pt-br"));
                portfolioStatementWrapperVM.StocksStatement = resultModel;

                result.Value = portfolioStatementWrapperVM;
                result.Success = true;
            }

            return result;
        }

        public ResultResponseObject<IEnumerable<CryptoBrokerVM>> GetCryptosBroker()
        {
            ResultServiceObject<IEnumerable<CryptoBrokerView>> resultService = null;
         
            using (_uow.Create())
            {
                resultService = _financialProductService.GetCryptosBroker(_globalAuthenticationService.IdUser);
            }

            ResultResponseObject<IEnumerable<CryptoBrokerVM>> result = _mapper.Map<ResultResponseObject<IEnumerable<CryptoBrokerVM>>>(resultService);

            return result;

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

        public ResultResponseObject<IEnumerable<CryptoInfoVM>> GetCryptosInfo(string symbolOrName)
        {
            ResultServiceObject<IEnumerable<CryptoStatementView>> resultService = null;

            using (_uow.Create())
            {
                resultService = _cryptoCurrencyService.GetCryptosByNameOrSymbol(symbolOrName);
            }

            ResultResponseObject<IEnumerable<CryptoInfoVM>> result = _mapper.Map<ResultResponseObject<IEnumerable<CryptoInfoVM>>>(resultService);

            return result;

        }


        public ResultResponseObject<IEnumerable<API.Model.Response.CryptoMarketMoverVM>> GetMarketMoverByType(CryptoMakertMoversType cryptoMakertMoversType)
        {
            ResultResponseObject<IEnumerable<API.Model.Response.CryptoMarketMoverVM>> resultResponseObject = new ResultResponseObject<IEnumerable<API.Model.Response.CryptoMarketMoverVM>>();

            string resultFromCache = _cacheService.GetFromCache(string.Concat("CryptoMarketMover", cryptoMakertMoversType.ToString()));

            if (resultFromCache == null)
            {
                using (_uow.Create())
                {
                    ResultServiceObject<IEnumerable<CryptoStatementView>> resultServiceObject;

                    if (cryptoMakertMoversType == CryptoMakertMoversType.Gainers)
                    {
                        resultServiceObject = _cryptoCurrencyService.GetMarketMoverByType(true);
                    }
                    else
                    {
                        resultServiceObject = _cryptoCurrencyService.GetMarketMoverByType(false);
                    }

                    resultResponseObject = _mapper.Map<ResultResponseObject<IEnumerable<API.Model.Response.CryptoMarketMoverVM>>>(resultServiceObject);

                    _cacheService.SaveOnCache(string.Concat("CryptoMarketMover", cryptoMakertMoversType.ToString()), TimeSpan.FromMinutes(3), JsonConvert.SerializeObject(resultResponseObject));
                }
            }
            else
            {
                resultResponseObject = JsonConvert.DeserializeObject<ResultResponseObject<IEnumerable<API.Model.Response.CryptoMarketMoverVM>>>(resultFromCache);
                resultResponseObject.Success = true;
            }


            return resultResponseObject;
        }

        public ResultResponseObject<IEnumerable<API.Model.Response.v2.CryptoMarketMoverVM>> GetMarketMoverByTypeV2(CryptoMakertMoversType cryptoMakertMoversType)
        {
            ResultResponseObject<IEnumerable<API.Model.Response.v2.CryptoMarketMoverVM>> resultResponseObject = new ResultResponseObject<IEnumerable<API.Model.Response.v2.CryptoMarketMoverVM>>();

            string resultFromCache = _cacheService.GetFromCache(string.Concat("CryptoMarketMoverV2", cryptoMakertMoversType.ToString()));

            if (resultFromCache == null)
            {
                using (_uow.Create())
                {
                    ResultServiceObject<IEnumerable<CryptoStatementView>> resultServiceObject;

                    if (cryptoMakertMoversType == CryptoMakertMoversType.Gainers)
                    {
                        resultServiceObject = _cryptoCurrencyService.GetMarketMoverByType(true);
                    }
                    else
                    {
                        resultServiceObject = _cryptoCurrencyService.GetMarketMoverByType(false);
                    }

                    resultResponseObject = _mapper.Map<ResultResponseObject<IEnumerable<API.Model.Response.v2.CryptoMarketMoverVM>>>(resultServiceObject);

                    _cacheService.SaveOnCache(string.Concat("CryptoMarketMoverV2", cryptoMakertMoversType.ToString()), TimeSpan.FromMinutes(3), JsonConvert.SerializeObject(resultResponseObject));
                }
            }
            else
            {
                resultResponseObject = JsonConvert.DeserializeObject<ResultResponseObject<IEnumerable<API.Model.Response.v2.CryptoMarketMoverVM>>>(resultFromCache);
                resultResponseObject.Success = true;
            }


            return resultResponseObject;
        }

        public ResultResponseObject<FinancialProductAddVM> UpdateAveragePrice(FinancialProductAddVM financialProductAddVM)
        {
            ResultServiceObject<ProductUser> resultService = null;

            using (_uow.Create())
            {
                decimal averagePrice = 0;
                decimal.TryParse(financialProductAddVM.AveragePrice.Replace(".", string.Empty), NumberStyles.Currency, new CultureInfo("pt-br"), out averagePrice);

                resultService = _financialProductService.UpdateAveragePrice(financialProductAddVM.FinancialInstitutionGuid, averagePrice);
            }

            ResultResponseObject<FinancialProductAddVM> result = _mapper.Map<ResultResponseObject<FinancialProductAddVM>>(resultService);

            return result;
        }
    }
}
