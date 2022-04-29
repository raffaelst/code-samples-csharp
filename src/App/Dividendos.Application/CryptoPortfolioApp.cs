using AutoMapper;
using Dividendos.API.Model.Request.Crypto;
using Dividendos.API.Model.Response.Common;
using Dividendos.API.Model.Response.Crypto;
using Dividendos.Application.Base;
using Dividendos.Application.Interface;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Enum;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Application
{
    public class CryptoPortfolioApp : BaseApp, ICryptoPortfolioApp
    {
        private readonly IGlobalAuthenticationService _globalAuthenticationService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly ICryptoPortfolioService _cryptoPortfolioService;
        private readonly ITraderService _traderService;
        private readonly ICryptoPortfolioPerformanceService _cryptoPortfolioPerformanceService;
        private readonly ICryptoCurrencyService _cryptoCurrencyService;
        private readonly ICryptoTransactionService _cryptoTransactionService;
        private readonly ICryptoTransactionItemService _cryptoTransactionItemService;
        private readonly ICryptoCurrencyPerformanceService _cryptoCurrencyPerformanceService;
        private readonly ICryptoSubPortfolioService _cryptoSubPortfolioService;
        private readonly IFiatCurrencyService _fiatCurrencyService;

        public CryptoPortfolioApp(
            IMapper mapper,
            IUnitOfWork uow,
            IGlobalAuthenticationService globalAuthenticationService,
            ICryptoPortfolioService cryptoPortfolioService,
            ITraderService traderService,
            ICryptoPortfolioPerformanceService cryptoPortfolioPerformanceService,
            ICryptoCurrencyService cryptoCurrencyService,
            ICryptoTransactionService cryptoTransactionService,
            ICryptoTransactionItemService cryptoTransactionItemService,
            ICryptoCurrencyPerformanceService cryptoCurrencyPerformanceService,
            ICryptoSubPortfolioService cryptoSubPortfolioService,
            IFiatCurrencyService fiatCurrencyService)
        {
            _mapper = mapper;
            _globalAuthenticationService = globalAuthenticationService;
            _uow = uow;
            _cryptoPortfolioService = cryptoPortfolioService;
            _traderService = traderService;
            _cryptoPortfolioPerformanceService = cryptoPortfolioPerformanceService;
            _cryptoCurrencyService = cryptoCurrencyService;
            _cryptoTransactionService = cryptoTransactionService;
            _cryptoTransactionItemService = cryptoTransactionItemService;
            _cryptoCurrencyPerformanceService = cryptoCurrencyPerformanceService;
            _cryptoSubPortfolioService = cryptoSubPortfolioService;
            _fiatCurrencyService = fiatCurrencyService;
        }

        public ResultResponseObject<CryptoPortfolioVM> CreateManualPortfolio(CryptoPortolioRequest cryptoPortolioRequest)
        {
            ResultResponseObject<CryptoPortfolioVM> resultResponseCryptoPortfolio = new ResultResponseObject<CryptoPortfolioVM>();

            using (_uow.Create())
            {
                ResultServiceObject<Trader> resultService = _traderService.SaveTrader("0", "0", _globalAuthenticationService.IdUser, false, true, TraderTypeEnum.CryptoManual);

                if (resultService.Value != null)
                {
                    CryptoPortfolio cryptoPortfolio = new CryptoPortfolio();
                    cryptoPortfolio.Name = cryptoPortolioRequest.Name;
                    cryptoPortfolio.IdFiatCurrency = cryptoPortolioRequest.IdFiatCurrency;
                    cryptoPortfolio.Manual = true;
                    cryptoPortfolio.IdTrader = resultService.Value.IdTrader;

                    _cryptoPortfolioService.Insert(cryptoPortfolio);

                    resultResponseCryptoPortfolio.Value = new CryptoPortfolioVM { CryptoPortfolioGuid = cryptoPortfolio.GuidCryptoPortfolio, Name = cryptoPortfolio.Name, ManualPortfolio = cryptoPortfolio.Manual };
                    resultResponseCryptoPortfolio.Success = true;
                }
            }

            return resultResponseCryptoPortfolio;
        }

        public ResultResponseObject<CryptoPortfolioViewWrapperVM> GetCryptoPortfolioViewWrapper()
        {
            CryptoPortfolioViewWrapperVM cryptoPortfolioViewWrapperVM = new CryptoPortfolioViewWrapperVM();
            List<CryptoPortfolioViewVM> resultModel = new List<CryptoPortfolioViewVM>();
            ResultResponseObject<CryptoPortfolioViewWrapperVM> portfolioViewWrapperVM = new ResultResponseObject<CryptoPortfolioViewWrapperVM>();

            using (_uow.Create())
            {
                ResultServiceObject<IEnumerable<CryptoPortfolioView>> result = _cryptoPortfolioService.GetByUser(_globalAuthenticationService.IdUser);

                if (result.Success && result.Value != null && result.Value.Count() > 0)
                {
                    foreach (CryptoPortfolioView cryptoPortfolioView in result.Value)
                    {
                        CryptoPortfolioViewVM portfolioViewModel = PortfolioParseToReturn(cryptoPortfolioView);

                        resultModel.Add(portfolioViewModel);
                    }
                    cryptoPortfolioViewWrapperVM.CryptoPortfolioViews = resultModel;
                    portfolioViewWrapperVM.Value = cryptoPortfolioViewWrapperVM;
                }

                portfolioViewWrapperVM.Success = true;
            }

            return portfolioViewWrapperVM;
        }

        private CryptoPortfolioViewVM PortfolioParseToReturn(CryptoPortfolioView cryptoPortfolioView)
        {
            decimal perc = cryptoPortfolioView.PerformancePerc * 100;
            decimal percTwr = cryptoPortfolioView.PerformancePercTWR * 100;
            
            CryptoPortfolioViewVM portfolioViewModel = new CryptoPortfolioViewVM();
            portfolioViewModel.Name = cryptoPortfolioView.Name;
            portfolioViewModel.CalculationDate = cryptoPortfolioView.CalculationDate.ToString("dd/MM");
            portfolioViewModel.GuidCryptoPortfolioSubPortfolio = cryptoPortfolioView.GuidCryptoPortfolioSubPortfolio;
            portfolioViewModel.LatestNetValue = GetSignal(cryptoPortfolioView.LatestNetValue) + cryptoPortfolioView.LatestNetValue.ToString("n2", new CultureInfo("pt-br"));
            portfolioViewModel.NetValue = GetSignal(cryptoPortfolioView.NetValue) + cryptoPortfolioView.NetValue.ToString("n2", new CultureInfo("pt-br"));
            portfolioViewModel.Profit = GetSignal(cryptoPortfolioView.Profit) + cryptoPortfolioView.Profit.ToString("n2", new CultureInfo("pt-br"));
            portfolioViewModel.TotalMarket = string.Format("{0} {1}", _fiatCurrencyService.GetCurrencySymbol((FiatCurrencyEnum)cryptoPortfolioView.IdFiatCurrency) , cryptoPortfolioView.TotalMarket.ToString("n2", new CultureInfo("pt-br")));
            portfolioViewModel.LastUpdated = DateTime.Now.ToString("HH:mm:ss");

            portfolioViewModel.Total = cryptoPortfolioView.Total.ToString("n2", new CultureInfo("pt-br"));
            portfolioViewModel.PerformancePerc = GetSignal(perc) + perc.ToString("n2", new CultureInfo("pt-br"));
            portfolioViewModel.PreviousNetValue = GetSignal(cryptoPortfolioView.PreviousNetValue) + cryptoPortfolioView.PreviousNetValue.ToString("n2", new CultureInfo("pt-br"));
            portfolioViewModel.PerformancePercTWR = GetSignal(percTwr) + percTwr.ToString("n2", new CultureInfo("pt-br"));
            portfolioViewModel.IdFiatCurrency = cryptoPortfolioView.IdFiatCurrency;
            portfolioViewModel.IsPortfolio = cryptoPortfolioView.IsPortfolio;

            return portfolioViewModel;
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

        public ResultResponseBase CalculatePerformance()
        {
            ResultResponseBase resultResponseBase = new ResultResponseBase();

            using (_uow.Create())
            {
                ResultServiceObject<IEnumerable<CryptoPortfolio>> resultPort = _cryptoPortfolioService.GetByUser(_globalAuthenticationService.IdUser, true);

                if (resultPort.Success && resultPort.Value != null && resultPort.Value.Count() > 0)
                {
                    foreach (CryptoPortfolio portfolio in resultPort.Value)
                    {
                        _cryptoPortfolioPerformanceService.CalculatePerformance(portfolio.IdCryptoPortfolio, _cryptoPortfolioService, _cryptoCurrencyService, _cryptoTransactionService, _cryptoCurrencyPerformanceService);
                    }
                }

                resultResponseBase.Success = true;
            }

            return resultResponseBase;
        }

        public ResultResponseObject<CryptoCurrencyStatementWrapperVM> GetCryptoCurrencyStatementWrapperVM(Guid guidCryptoportfolioSub)
        {
            CryptoCurrencyStatementWrapperVM cryptoCurrencyStatementWrapperVM = null;
            ResultResponseObject<CryptoCurrencyStatementWrapperVM> resultServiceObject = new ResultResponseObject<CryptoCurrencyStatementWrapperVM>();

            using (_uow.Create())
            {
                ResultServiceObject<CryptoPortfolio> resultCryptoPortfolio = _cryptoPortfolioService.GetByGuid(guidCryptoportfolioSub);
                ResultServiceObject<CryptoSubPortfolio> resultSubCryptoPortfolio = _cryptoSubPortfolioService.GetByGuid(guidCryptoportfolioSub);
                Trader trader = null;

                if (resultSubCryptoPortfolio.Success && resultCryptoPortfolio.Success)
                {
                    cryptoCurrencyStatementWrapperVM = new CryptoCurrencyStatementWrapperVM();

                    if (resultCryptoPortfolio.Value == null)
                    {
                        ResultServiceObject<CryptoPortfolio> resultPort = _cryptoPortfolioService.GetById(resultSubCryptoPortfolio.Value.IdCryptoPortfolio);

                        if (resultPort.Success)
                        {
                            trader = _traderService.GetById(resultPort.Value.IdTrader).Value;

                            if (trader != null)
                            {
                                cryptoCurrencyStatementWrapperVM.LastSyncDate = trader.LastSync;
                            }
                        }
                    }
                    else
                    {
                        trader = _traderService.GetById(resultCryptoPortfolio.Value.IdTrader).Value;

                        if (trader != null)
                        {
                            cryptoCurrencyStatementWrapperVM.LastSyncDate = trader.LastSync;
                        }
                    }

                    List<CryptoCurrencyStatementVM> resultModel = new List<CryptoCurrencyStatementVM>();
                    ResultServiceObject<IEnumerable<CryptoCurrencyStatementView>> result = new ResultServiceObject<IEnumerable<CryptoCurrencyStatementView>>();
                    CryptoPortfolio cryptoPortfolio = resultCryptoPortfolio.Value;
                    CryptoSubPortfolio cryptoSubportfolio = resultSubCryptoPortfolio.Value;

                    if (cryptoPortfolio != null)
                    {
                        cryptoCurrencyStatementWrapperVM.GuidCryptoPortfolioSubPortfolio = cryptoPortfolio.GuidCryptoPortfolio;
                        cryptoCurrencyStatementWrapperVM.ManualPortfolio = cryptoPortfolio.Manual;
                        cryptoCurrencyStatementWrapperVM.LastUpdated = DateTime.Now.ToString("HH:mm:ss");
                        cryptoCurrencyStatementWrapperVM.IdFiatCurrency = cryptoPortfolio.IdFiatCurrency;
                        result = _cryptoPortfolioService.GetByCryptoPortfolio(cryptoPortfolio.GuidCryptoPortfolio);
                    }
                    else if (cryptoSubportfolio != null)
                    {
                        cryptoCurrencyStatementWrapperVM.GuidCryptoPortfolioSubPortfolio = cryptoSubportfolio.GuidCryptoSubPortfolio;
                        cryptoCurrencyStatementWrapperVM.LastUpdated = DateTime.Now.ToString("HH:mm:ss");

                        resultCryptoPortfolio = _cryptoPortfolioService.GetById(cryptoSubportfolio.IdCryptoPortfolio);

                        if (resultCryptoPortfolio.Success)
                        {
                            cryptoCurrencyStatementWrapperVM.ManualPortfolio = resultCryptoPortfolio.Value.Manual;
                            cryptoCurrencyStatementWrapperVM.IdFiatCurrency = resultCryptoPortfolio.Value.IdFiatCurrency;
                            cryptoPortfolio = resultCryptoPortfolio.Value;
                        }

                        result = _cryptoPortfolioService.GetByCryptoSubportfolio(cryptoSubportfolio.GuidCryptoSubPortfolio);
                    }

                    cryptoCurrencyStatementWrapperVM.TraderTypeId = trader.TraderTypeID;
                    cryptoCurrencyStatementWrapperVM.CanEdit = trader.TraderTypeID == (int)TraderTypeEnum.Avenue || trader.TraderTypeID == (int)TraderTypeEnum.Toro;

                    if (result.Success && result.Value != null && result.Value.Count() > 0)
                    {
                        foreach (CryptoCurrencyStatementView cryptoCurrencyStatementView in result.Value)
                        {
                            decimal perc = cryptoCurrencyStatementView.PerformancePerc * 100;
                            CryptoCurrencyStatementVM cryptoCurrencyStatementVM = new CryptoCurrencyStatementVM();
                            cryptoCurrencyStatementVM.GuidCryptoCurrency = cryptoCurrencyStatementView.GuidCryptoCurrency;
                            cryptoCurrencyStatementVM.GuidCryptoTransaction = cryptoCurrencyStatementView.GuidCryptoTransaction;
                            cryptoCurrencyStatementVM.Name = cryptoCurrencyStatementView.Name;
                            cryptoCurrencyStatementVM.Logo = cryptoCurrencyStatementView.Logo;
                            cryptoCurrencyStatementVM.PerformancePerc = GetSignal(perc) + perc.ToString("n2", new CultureInfo("pt-br"));
                            cryptoCurrencyStatementVM.AveragePrice = string.Format("{0} {1}", _fiatCurrencyService.GetCurrencySymbol((FiatCurrencyEnum)cryptoPortfolio.IdFiatCurrency), cryptoCurrencyStatementView.AveragePrice.ToString("n2", new CultureInfo("pt-br")));
                            cryptoCurrencyStatementVM.MarketPrice = string.Format("{0} {1}", _fiatCurrencyService.GetCurrencySymbol((FiatCurrencyEnum)cryptoPortfolio.IdFiatCurrency), cryptoCurrencyStatementView.MarketPrice.ToString("n2", new CultureInfo("pt-br")));
                            cryptoCurrencyStatementVM.NetValue = GetSignal(cryptoCurrencyStatementView.NetValue) + cryptoCurrencyStatementView.NetValue.ToString("n2", new CultureInfo("pt-br"));
                            cryptoCurrencyStatementVM.Quantity = cryptoCurrencyStatementView.Quantity.ToString("0.#############################", new CultureInfo("pt-br"));
                            cryptoCurrencyStatementVM.TotalMarket = string.Format("{0} {1}", _fiatCurrencyService.GetCurrencySymbol((FiatCurrencyEnum)cryptoPortfolio.IdFiatCurrency), cryptoCurrencyStatementView.TotalMarket.ToString("n2", new CultureInfo("pt-br")));
                            cryptoCurrencyStatementVM.Total = string.Format("{0} {1}", _fiatCurrencyService.GetCurrencySymbol((FiatCurrencyEnum)cryptoPortfolio.IdFiatCurrency), cryptoCurrencyStatementView.Total.ToString("n2", new CultureInfo("pt-br")));

                            cryptoCurrencyStatementVM.PerformancePercN = perc;
                            cryptoCurrencyStatementVM.AveragePriceN = cryptoCurrencyStatementView.AveragePrice;
                            cryptoCurrencyStatementVM.MarketPriceN = cryptoCurrencyStatementView.MarketPrice;
                            cryptoCurrencyStatementVM.NetValueN = cryptoCurrencyStatementView.NetValue;
                            cryptoCurrencyStatementVM.QuantityN = cryptoCurrencyStatementView.Quantity;
                            cryptoCurrencyStatementVM.TotalMarketN = cryptoCurrencyStatementView.TotalMarket;
                            cryptoCurrencyStatementVM.TotalN = cryptoCurrencyStatementView.Total;

                            resultModel.Add(cryptoCurrencyStatementVM);
                        }

                        cryptoCurrencyStatementWrapperVM.CryptoCurrencyStatement = resultModel;
                    }
                }
            }

            resultServiceObject.Value = cryptoCurrencyStatementWrapperVM;
            resultServiceObject.Success = true;

            return resultServiceObject;
        }

        public ResultResponseObject<CryptoCurrencyStatementVM> GetCryptoCurrencyStatementView(Guid guidCryptoPortfolio, Guid guidCryptoCurrency)
        {
            ResultResponseObject<CryptoCurrencyStatementVM> resultServiceObject = new ResultResponseObject<CryptoCurrencyStatementVM>();

            using (_uow.Create())
            {
                ResultServiceObject<CryptoPortfolio> resultCryptoPortfolio = _cryptoPortfolioService.GetByGuid(guidCryptoPortfolio);
                ResultServiceObject<CryptoSubPortfolio> resultCryptoSubPortfolio = _cryptoSubPortfolioService.GetByGuid(guidCryptoPortfolio);
                Guid guidCryptoPortfolioTmp = Guid.Empty;
                int idFiatCurrency = 1;

                if (resultCryptoSubPortfolio.Success && resultCryptoPortfolio.Success)
                {
                    CryptoPortfolio cryptoPortfolio = resultCryptoPortfolio.Value;
                    CryptoSubPortfolio cryptoSubportfolio = resultCryptoSubPortfolio.Value;

                    if (cryptoPortfolio != null)
                    {
                        guidCryptoPortfolioTmp = cryptoPortfolio.GuidCryptoPortfolio;
                        idFiatCurrency = cryptoPortfolio.IdFiatCurrency;
                    }
                    else
                    {
                        ResultServiceObject<CryptoPortfolio> cryptoPortfolioDb = _cryptoPortfolioService.GetById(cryptoSubportfolio.IdCryptoPortfolio);

                        if (cryptoPortfolioDb.Value != null)
                        {
                            guidCryptoPortfolioTmp = cryptoPortfolioDb.Value.GuidCryptoPortfolio;
                            idFiatCurrency = cryptoPortfolioDb.Value.IdFiatCurrency;
                        }
                    }
                }

                ResultServiceObject<CryptoCurrencyStatementView> resultCryptoCurrencyStatementView = _cryptoPortfolioService.GetByGuidCurrency(guidCryptoPortfolioTmp, guidCryptoCurrency);

                if (resultCryptoCurrencyStatementView.Success && resultCryptoCurrencyStatementView.Value != null)
                {
                    CryptoCurrencyStatementView cryptoCurrencyStatementView = resultCryptoCurrencyStatementView.Value;
                    ResultServiceObject<IEnumerable<CryptoCurrencyStatementView>> result = new ResultServiceObject<IEnumerable<CryptoCurrencyStatementView>>();


                    decimal perc = cryptoCurrencyStatementView.PerformancePerc * 100;
                    CryptoCurrencyStatementVM cryptoCurrencyStatementVM = new CryptoCurrencyStatementVM();
                    cryptoCurrencyStatementVM.GuidCryptoCurrency = cryptoCurrencyStatementView.GuidCryptoCurrency;
                    cryptoCurrencyStatementVM.GuidCryptoPortfolio = guidCryptoPortfolioTmp;
                    cryptoCurrencyStatementVM.GuidCryptoTransaction = cryptoCurrencyStatementView.GuidCryptoTransaction;
                    cryptoCurrencyStatementVM.Name = cryptoCurrencyStatementView.Name;
                    cryptoCurrencyStatementVM.Logo = cryptoCurrencyStatementView.Logo;
                    cryptoCurrencyStatementVM.PerformancePerc = GetSignal(perc) + perc.ToString("n2", new CultureInfo("pt-br"));
                    cryptoCurrencyStatementVM.AveragePrice = string.Format("{0} {1}", _fiatCurrencyService.GetCurrencySymbol((FiatCurrencyEnum)idFiatCurrency), cryptoCurrencyStatementView.AveragePrice.ToString("n2", new CultureInfo("pt-br")));

                    cryptoCurrencyStatementVM.NetValue = GetSignal(cryptoCurrencyStatementView.NetValue) + cryptoCurrencyStatementView.NetValue.ToString("n2", new CultureInfo("pt-br"));
                    cryptoCurrencyStatementVM.Quantity = cryptoCurrencyStatementView.Quantity.ToString("0.#############################", new CultureInfo("pt-br"));
                    cryptoCurrencyStatementVM.TotalMarket = string.Format("{0} {1}", _fiatCurrencyService.GetCurrencySymbol((FiatCurrencyEnum)idFiatCurrency), cryptoCurrencyStatementView.TotalMarket.ToString("n2", new CultureInfo("pt-br")));
                    cryptoCurrencyStatementVM.Total = string.Format("{0} {1}", _fiatCurrencyService.GetCurrencySymbol((FiatCurrencyEnum)idFiatCurrency), cryptoCurrencyStatementView.Total.ToString("n2", new CultureInfo("pt-br")));

                    cryptoCurrencyStatementVM.PerformancePercN = perc;
                    cryptoCurrencyStatementVM.AveragePriceN = cryptoCurrencyStatementView.AveragePrice;

                    cryptoCurrencyStatementVM.NetValueN = cryptoCurrencyStatementView.NetValue;
                    cryptoCurrencyStatementVM.QuantityN = cryptoCurrencyStatementView.Quantity;
                    cryptoCurrencyStatementVM.TotalMarketN = cryptoCurrencyStatementView.TotalMarket;
                    cryptoCurrencyStatementVM.TotalN = cryptoCurrencyStatementView.Total;
                    cryptoCurrencyStatementVM.IdFiatCurrency = idFiatCurrency;
                    cryptoCurrencyStatementVM.MarketPrice = string.Format("{0} {1}", _fiatCurrencyService.GetCurrencySymbol((FiatCurrencyEnum)idFiatCurrency), cryptoCurrencyStatementView.MarketPrice.ToString("n2", new CultureInfo("pt-br")));
                    cryptoCurrencyStatementVM.MarketPriceN = cryptoCurrencyStatementView.MarketPrice;

                    resultServiceObject.Value = cryptoCurrencyStatementVM;
                }
            }

            resultServiceObject.Success = true;
            return resultServiceObject;
        }

        public ResultResponseBase Disable(Guid guidCryptoPortfolio)
        {
            ResultResponseBase resultResponseBase = new ResultResponseBase();
            ResultServiceBase resultServiceBase = new ResultServiceBase();
            using (_uow.Create())
            {
                ResultServiceObject<CryptoPortfolio> resultServiceObject = _cryptoPortfolioService.GetByGuid(guidCryptoPortfolio);

                if (resultServiceObject.Value != null)
                {
                    ResultServiceObject<IEnumerable<CryptoSubPortfolio>> resultCryptoSubPortfolio = _cryptoSubPortfolioService.GetByIdCryptoPortfolio(resultServiceObject.Value.IdCryptoPortfolio);

                    foreach (var item in resultCryptoSubPortfolio.Value)
                    {
                        _cryptoSubPortfolioService.Disable(item);
                    }

                    _cryptoPortfolioService.Disable(resultServiceObject.Value);

                    ResultServiceObject<Trader> resultTrader = _traderService.GetById(resultServiceObject.Value.IdTrader);

                    if (resultTrader.Value != null)
                    {
                        _traderService.Disable(resultTrader.Value.IdTrader);
                    }
                }
            }

            resultResponseBase = _mapper.Map<ResultResponseBase>(resultServiceBase);

            return resultResponseBase;
        }

        public ResultResponseBase UpdateName(Guid guidCryptoPortfolio, CryptoPortfolioEditVM cryptoPortfolioEditVM)
        {
            ResultResponseBase resultResponseBase = new ResultResponseBase();

            using (_uow.Create())
            {
                ResultServiceObject<CryptoPortfolio> resultServiceObject = _cryptoPortfolioService.GetByGuid(guidCryptoPortfolio);

                if (resultServiceObject.Value != null)
                {
                    _cryptoPortfolioService.UpdateName(resultServiceObject.Value.IdCryptoPortfolio, cryptoPortfolioEditVM.Name);
                }

                resultResponseBase.Success = true;
            }

            return resultResponseBase;
        }

        public ResultResponseObject<CryptoSubportfolioWrapperVM> GetCryptoPortfolioContentSimple(Guid guidCryptoPortfolio)
        {
            ResultResponseObject<CryptoSubportfolioWrapperVM> resultReponse = new ResultResponseObject<CryptoSubportfolioWrapperVM>();

            using (_uow.Create())
            {
                ResultServiceObject<IEnumerable<CryptoSubportfolioItemView>> resultOperationService = _cryptoPortfolioService.GetSubViewByCryptoPortfolio(guidCryptoPortfolio);

                if (resultOperationService.Value != null && resultOperationService.Value.Count() > 0)
                {
                    CryptoSubportfolioWrapperVM cryptoSubportfolioWrapperVM = new CryptoSubportfolioWrapperVM();
                    cryptoSubportfolioWrapperVM.GuidCryptoPortfolio = guidCryptoPortfolio;

                    List<CryptoSubportfolioItemAddVM> cryptoSubportfolioItemAddVMs = new List<CryptoSubportfolioItemAddVM>();

                    foreach (CryptoSubportfolioItemView cryptoCurrencyStatementView in resultOperationService.Value)
                    {
                        CryptoSubportfolioItemAddVM cryptoSubportfolioItemAddVM = new CryptoSubportfolioItemAddVM();
                        cryptoSubportfolioItemAddVM.GuidCryptoTransaction = cryptoCurrencyStatementView.GuidCryptoTransaction;
                        cryptoSubportfolioItemAddVM.Logo = cryptoCurrencyStatementView.Logo;
                        cryptoSubportfolioItemAddVM.Selected = false;
                        cryptoSubportfolioItemAddVM.CryptoName = cryptoCurrencyStatementView.CryptoName;
                        cryptoSubportfolioItemAddVMs.Add(cryptoSubportfolioItemAddVM);
                    }

                    cryptoSubportfolioWrapperVM.CryptoSubportfolioItems = cryptoSubportfolioItemAddVMs;
                    resultReponse.Value = cryptoSubportfolioWrapperVM;
                }
                resultReponse.Success = true;
            }

            return resultReponse;
        }

        public ResultResponseObject<CryptoSubportfolioWrapperVM> GetCryptoSubportfolioContentSimple(Guid guidCryptoPortfolio, Guid guidCryptoSubportfolio)
        {
            ResultResponseObject<CryptoSubportfolioWrapperVM> resultReponse = new ResultResponseObject<CryptoSubportfolioWrapperVM>();

            using (_uow.Create())
            {
                CryptoSubPortfolio cryptoSubPortfolio =  _cryptoSubPortfolioService.GetByGuid(guidCryptoSubportfolio).Value;

                if (cryptoSubPortfolio != null)
                {
                    CryptoSubportfolioWrapperVM cryptoSubportfolioWrapperVM = new CryptoSubportfolioWrapperVM();
                    cryptoSubportfolioWrapperVM.GuidCryptoPortfolio = guidCryptoPortfolio;
                    cryptoSubportfolioWrapperVM.GuidCryptoSubportfolio = guidCryptoSubportfolio;
                    cryptoSubportfolioWrapperVM.Name = cryptoSubPortfolio.Name;

                    ResultServiceObject<IEnumerable<CryptoSubportfolioItemView>> resultOperationService = _cryptoPortfolioService.GetBySubCryptoPortfolio(cryptoSubPortfolio.IdCryptoPortfolio, cryptoSubPortfolio.IdCryptoSubPortfolio);

                    if (resultOperationService.Value != null && resultOperationService.Value.Count() > 0)
                    {
                        cryptoSubportfolioWrapperVM.GuidCryptoPortfolio = guidCryptoPortfolio;

                        List<CryptoSubportfolioItemAddVM> cryptoSubportfolioItemAddVMs = new List<CryptoSubportfolioItemAddVM>();

                        foreach (CryptoSubportfolioItemView cryptoCurrencyStatementView in resultOperationService.Value)
                        {
                            CryptoSubportfolioItemAddVM cryptoSubportfolioItemAddVM = new CryptoSubportfolioItemAddVM();
                            cryptoSubportfolioItemAddVM.GuidCryptoTransaction = cryptoCurrencyStatementView.GuidCryptoTransaction;
                            cryptoSubportfolioItemAddVM.Logo = cryptoCurrencyStatementView.Logo;
                            cryptoSubportfolioItemAddVM.Selected = cryptoCurrencyStatementView.Selected;
                            cryptoSubportfolioItemAddVM.CryptoName = cryptoCurrencyStatementView.CryptoName;
                            cryptoSubportfolioItemAddVMs.Add(cryptoSubportfolioItemAddVM);
                        }

                        cryptoSubportfolioWrapperVM.CryptoSubportfolioItems = cryptoSubportfolioItemAddVMs;
                        resultReponse.Value = cryptoSubportfolioWrapperVM;
                    }

                    resultReponse.Success = true;
                }
            }

            return resultReponse;
        }


    }
}
