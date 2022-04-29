using AutoMapper;
using K.Logger;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using Dividendos.Application.Base;
using Dividendos.Application.Interface;
using Dividendos.Application.Interface.Model;
using Dividendos.B3.Interface;
using Dividendos.B3.Interface.Model;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Dividendos.API.Model.Response.v2;
using Dividendos.API.Model.Request.Portfolio;
using Dividendos.API.Model.Response.v3;
using Dividendos.Entity.Enum;
using Dividendos.MercadoBitcoin.Interface;
using Dividendos.MercadoBitcoin.Interface.Model;
using System.Text;
using Dividendos.API.Model.Request.Operation;
using Dividendos.Passfolio.Interface;
using Dividendos.API.Model.Request.BrokerIntegration;
using Dividendos.Passfolio.Interface.Model;
using Dividendos.Binance;
using Dividendos.Binance.Interface.Model;
using System.Net.Http;
using System.Net.Http.Headers;
using Dividendos.BitcoinTrade;
using Dividendos.CoinMarketCap;
using Dividendos.Coinbase;
using Dividendos.BitcoinToYou;
using Dividendos.Biscoint;
using Dividendos.Avenue.Interface;
using Dividendos.Avenue.Interface.Model;
using Dividendos.AWS;
using Dividendos.BitPreco.Interface;

namespace Dividendos.Application
{
    public class PortfolioApp : BaseApp, IPortfolioApp
    {
        public List<string> ChangedCeiItems { get; set; }
        public List<string> DividendCeiItems { get; set; }

        private readonly IGlobalAuthenticationService _globalAuthenticationService;
        private readonly IImportB3Helper _iImportB3Helper;
        private readonly ITraderService _traderService;
        private readonly IIndicatorSeriesService _indicatorSeriesService;
        private readonly IStockService _stockService;
        private readonly ISectorService _sectorService;
        private readonly ISegmentService _segmentService;
        private readonly ISubsectorService _subsectorService;
        private readonly IDividendService _dividendService;
        private readonly IDividendTypeService _dividendTypeService;
        private readonly IOperationService _operationService;
        private readonly IOperationItemService _operationItemService;
        private readonly IStockTypeService _stockTypeService;
        private readonly IPortfolioService _portfolioService;
        private readonly ISubPortfolioService _subPortfolioService;
        private readonly ISubPortfolioOperationService _subPortfolioOperationService;
        private readonly IPortfolioPerformanceService _portfolioPerformanceService;
        private readonly IPerformanceStockService _performanceStockService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly ILogger _logger;
        private readonly ICipherService _cipherService;
        private readonly IDeviceService _deviceService;
        private readonly ISettingsService _settingsService;
        private readonly IAccountInfo _accountInfoClient;
        private readonly IFinancialProductService _financialProductService;
        private readonly ISystemSettingsService _systemSettingsService;
        private readonly IOperationItemHistService _operationItemHistService;
        private readonly IOperationHistService _operationHistService;
        private readonly ISubscriptionService _subscriptionService;
        private readonly ICryptoCurrencyService _cryptoCurrencyService;
        private readonly INotificationService _notificationService;
        private readonly INotificationHistoricalService _notificationHistoricalService;
        private readonly ISyncQueueService _syncQueueService;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly ICeiLogService _ceiLogService;
        private readonly ICacheService _cacheService;
        private readonly IPassfolioHelper _passfolioHelper;
        private readonly IBinanceHelper _binanceHelper;
        private readonly IBitcoinTradeHelper _bitcoinTradeHelper;
        private readonly IHolidayService _holidayService;
        private readonly ICoinMarketCapHelper _coinMarketCapHelper;
        private readonly ILogoService _logoService;
        private readonly IScrapySchedulerService _scrapySchedulerService;
        private readonly ICoinbaseHelper _coinbaseHelper;
        private readonly IBitcoinToYouHelper _bitcoinToYouHelper;
        private readonly IScrapyAgentService _scrapyAgentService;
        private readonly IDividendCalendarService _dividendCalendarService;
        private readonly IBiscointHelper _biscointHelper;
        private readonly IAvenueHelper _avenueHelper;
        private readonly IStockSplitService _stockSplitService;
        private readonly IS3Service _iS3Service;
        private readonly ICryptoPortfolioService _cryptoPortfolioService;
        private readonly ICryptoSubPortfolioService _cryptoSubPortfolioService;
        private readonly IContactDetailsService _contactDetailsService;
        private readonly IContactPhoneService _contactPhoneService;
        private readonly ICryptoPortfolioPerformanceService _cryptoPortfolioPerformanceService;
        private readonly IBitPrecoHelper _bitPrecoHelper;

        public PortfolioApp(ICryptoCurrencyService cryptoCurrencyService,
            ITraderService traderService,
            IStockService stockService,
            ISectorService sectorService,
            ISegmentService segmentService,
            IIndicatorSeriesService indicatorSeriesService,
            ISubsectorService subsectorService,
            IPortfolioService portfolioService,
            ISubPortfolioService subPortfolioService,
            ISubPortfolioOperationService subPortfolioOperationService,
            IPortfolioPerformanceService portfolioPerformanceService,
            IPerformanceStockService performanceStockService,
            IDividendService dividendService,
            IDividendTypeService dividendTypeService,
            IStockTypeService stockTypeService,
            IOperationService operationService,
            IOperationItemService operationItemService,
            IOperationItemHistService operationItemHistService,
            IOperationHistService operationHistService,
            IImportB3Helper iImportB3Helper,
            IMapper mapper,
            IUnitOfWork uow,
            IGlobalAuthenticationService globalAuthenticationService,
            ILogger logger,
            ICipherService cipherService,
            IDeviceService deviceService,
            ISettingsService settingsService,
            IAccountInfo accountInfoClient,
            IFinancialProductService financialProductService,
            ISystemSettingsService systemSettingsService,
            ISubscriptionService subscriptionService,
            INotificationService notificationService,
            INotificationHistoricalService notificationHistoricalService,
            ISyncQueueService syncQueueService,
            IEmailTemplateService emailTemplateService,
            ICeiLogService ceiLogService,
            ICacheService cacheService,
            IPassfolioHelper passfolioHelper,
            IBinanceHelper binanceHelper,
            IBitcoinTradeHelper bitcoinTradeHelper,
            IHolidayService holidayService,
            ICoinMarketCapHelper coinMarketCapHelper,
            ILogoService logoService,
            IScrapySchedulerService scrapySchedulerService,
            ICoinbaseHelper coinbaseHelper,
            IBitcoinToYouHelper bitcoinToYouHelper,
            IScrapyAgentService scrapyAgentService,
            IBiscointHelper biscointHelper,
            IDividendCalendarService dividendCalendarService,
            IAvenueHelper avenueHelper,
            IStockSplitService stockSplitService,
            IS3Service iS3Service,
            ICryptoPortfolioService cryptoPortfolioService,
            ICryptoSubPortfolioService cryptoSubPortfolioService,
            IContactDetailsService contactDetailsService,
            IContactPhoneService contactPhoneService,
            ICryptoPortfolioPerformanceService cryptoPortfolioPerformanceService,
            IBitPrecoHelper bitPrecoHelper)
        {
            _cryptoCurrencyService = cryptoCurrencyService;
            _stockService = stockService;
            _sectorService = sectorService;
            _subsectorService = subsectorService;
            _segmentService = segmentService;
            _portfolioService = portfolioService;
            _subPortfolioService = subPortfolioService;
            _subPortfolioOperationService = subPortfolioOperationService;
            _portfolioPerformanceService = portfolioPerformanceService;
            _performanceStockService = performanceStockService;
            _dividendService = dividendService;
            _dividendTypeService = dividendTypeService;
            _operationService = operationService;
            _operationItemService = operationItemService;
            _stockTypeService = stockTypeService;
            _indicatorSeriesService = indicatorSeriesService;
            _operationItemHistService = operationItemHistService;
            _operationHistService = operationHistService;
            _traderService = traderService;
            _iImportB3Helper = iImportB3Helper;
            _mapper = mapper;
            _globalAuthenticationService = globalAuthenticationService;
            _uow = uow;
            _logger = logger;
            _cipherService = cipherService;
            _deviceService = deviceService;
            _settingsService = settingsService;
            _accountInfoClient = accountInfoClient;
            _financialProductService = financialProductService;
            _systemSettingsService = systemSettingsService;
            _subscriptionService = subscriptionService;
            _notificationService = notificationService;
            _notificationHistoricalService = notificationHistoricalService;
            _syncQueueService = syncQueueService;
            _emailTemplateService = emailTemplateService;
            _ceiLogService = ceiLogService;
            _cacheService = cacheService;
            _passfolioHelper = passfolioHelper;
            _binanceHelper = binanceHelper;
            _bitcoinTradeHelper = bitcoinTradeHelper;
            _holidayService = holidayService;
            _coinMarketCapHelper = coinMarketCapHelper;
            _logoService = logoService;
            _scrapySchedulerService = scrapySchedulerService;
            _coinbaseHelper = coinbaseHelper;
            _bitcoinToYouHelper = bitcoinToYouHelper;
            _scrapyAgentService = scrapyAgentService;
            _biscointHelper = biscointHelper;
            _dividendCalendarService = dividendCalendarService;
            _avenueHelper = avenueHelper;
            _stockSplitService = stockSplitService;
            _iS3Service = iS3Service;
            _cryptoPortfolioService = cryptoPortfolioService;
            _cryptoSubPortfolioService = cryptoSubPortfolioService;
            _contactDetailsService = contactDetailsService;
            _contactPhoneService = contactPhoneService;
            _cryptoPortfolioPerformanceService = cryptoPortfolioPerformanceService;
            _bitPrecoHelper = bitPrecoHelper;
        }


        public void AutoSyncPortfolios()
        {
            _logger.SendDebugAsync(new { JobDebugInfo = "Iniciando AutoSyncPortfolios" });

            ResultServiceObject<Trader> trader;

            //Get trader that set auto sync
            using (_uow.Create())
            {
                trader = _traderService.GetByOlderSyncDate();
            }

            SelectAutomaticImportProcess(trader.Value.TraderTypeID, trader.Value, trader.Value.IdUser, trader.Value.LastSync, true);
        }

        private void SelectAutomaticImportProcess(long traderTypeID, Trader trader, string idUser, DateTime lastSync, bool hangfire = false)
        {
            switch (traderTypeID)
            {
                case (long)TraderTypeEnum.RendaVariavelAndTesouroDiretoCEI:
                    {
                        if (lastSync <= DateTime.Now.AddHours(-7))
                        {
                            using (_uow.Create())
                            {
                                ImportCeiResult importCeiResult = this.ImportStocksAndTesouroDiretoCeiBase(trader.Identifier, _cipherService.Decrypt(trader.Password), idUser, true, hangfire);
                            }
                        }
                    }
                    break;
                case (long)TraderTypeEnum.RendaVariavelManual:
                    break;
                case (long)TraderTypeEnum.MercadoBitcoin:
                    {
                        if (lastSync <= DateTime.Now.AddHours(-1))
                        {
                            this.ImportMercadoBitcoin(trader.Identifier, trader.Password, idUser, true);
                        }
                    }
                    break;
                case (long)TraderTypeEnum.Passfolio:
                    {
                        if (lastSync <= DateTime.Now.AddHours(-1))
                        {
                            this.ImportFromPasfolio(trader.Identifier, trader.Password, idUser, true);
                        }
                    }
                    break;
                case (long)TraderTypeEnum.Binance:
                    {
                        if (lastSync <= DateTime.Now.AddHours(-1))
                        {
                            this.ImportBinance(trader.Identifier, trader.Password, idUser, true);
                        }
                    }
                    break;
                case (long)TraderTypeEnum.BitcoinTrade:
                    {
                        if (lastSync <= DateTime.Now.AddHours(-1))
                        {
                            this.ImportBitcoinTrade(trader.Identifier, trader.Password, idUser, true);
                        }
                    }
                    break;
                case (long)TraderTypeEnum.Coinbase:
                    {
                        if (lastSync <= DateTime.Now.AddHours(-1))
                        {
                            this.ImportCoinbase(trader.Identifier, trader.Password, idUser, true);
                        }
                    }
                    break;
                case (long)TraderTypeEnum.BitcoinToYou:
                    {
                        if (lastSync <= DateTime.Now.AddHours(-1))
                        {
                            this.ImportBitcoinToYou(trader.Identifier, trader.Password, idUser, true);
                        }
                    }
                    break;
                case (long)TraderTypeEnum.Biscoint:
                    {
                        if (lastSync <= DateTime.Now.AddHours(-1))
                        {
                            this.ImportBiscoint(trader.Identifier, trader.Password, idUser, true);
                        }
                    }
                    break;
                case (long)TraderTypeEnum.BitPreco:
                    {
                        if (lastSync <= DateTime.Now.AddHours(-1))
                        {
                            this.ImportBitPreco(trader.Identifier, trader.Password, idUser, true);
                        }
                    }
                    break;
                case (long)TraderTypeEnum.RendaVariavelAndTesouroDiretoNewCEI:

                    if (lastSync <= DateTime.Now.AddHours(-7))
                    {
                        using (_uow.Create())
                        {
                            ImportCeiResult importCeiResult = this.ImportStocksAndTesouroDiretoCeiBase(trader.Identifier, trader.Password, idUser, true, hangfire, TraderTypeEnum.RendaVariavelAndTesouroDiretoNewCEI);
                        }
                    }

                    break;
                default:
                    break;
            }
        }

        public void ImportFromPasfolio(string email, string auth, string idUser, bool automaticProcess)
        {
            ImportPassfolioResult importPassfolioResult = _passfolioHelper.Import(auth, _logger);

            _ = _logger.SendInformationAsync(new { UserID = idUser, Content = JsonConvert.SerializeObject(importPassfolioResult) });

            using (_uow.Create())
            {
                if (importPassfolioResult.Imported)
                {
                    List<string> dividendCeiItems = new List<string>();
                    List<string> changedCeiItems = new List<string>();
                    ResultServiceObject<Trader> resultTraderService;
                    ResultServiceObject<IEnumerable<Stock>> resultServiceStock;

                    List<StockOperationView> lstStockPortfolioGrouped = new List<StockOperationView>();
                    List<StockOperationView> lstStockOperationBuyGrouped = new List<StockOperationView>();
                    List<StockOperationView> lstStockOperationSellGrouped = new List<StockOperationView>();

                    DateTime lastSync = DateTime.Now;
                    resultTraderService = _traderService.SaveTrader(email, _cipherService.Encrypt(auth), idUser, false, false, TraderTypeEnum.Passfolio, out lastSync);
                    importPassfolioResult.IdTrader = resultTraderService.Value.IdTrader;

                    bool newPortfolio = false;
                    Portfolio portfolio = _portfolioService.SavePortfolio(resultTraderService.Value, CountryEnum.EUA, "Passfolio", out newPortfolio);

                    //_dividendService.RemoveDuplicated(portfolio.IdPortfolio, _dividendCalendarService);

                    resultServiceStock = _stockService.GetAllByCountry((int)CountryEnum.EUA);

                    _operationItemService.DeleteAllByPortfolio(portfolio.IdPortfolio);

                    List<StockOperationView> lstStockOperationPassFolio = _mapper.Map<List<StockOperationView>>(importPassfolioResult.ListStockOperation);
                    decimal totalLossProfit = 0;
                    _portfolioService.GroupStocksPassfolio(portfolio.IdPortfolio, resultServiceStock.Value, _mapper.Map<List<StockOperationView>>(importPassfolioResult.ListStockPortfolio), ref lstStockOperationPassFolio, ref lstStockPortfolioGrouped, ref lstStockOperationBuyGrouped, ref lstStockOperationSellGrouped, newPortfolio, lastSync, _operationService, _operationItemService, _operationItemHistService, _portfolioService, _operationHistService, _systemSettingsService, _portfolioPerformanceService, _stockService, _performanceStockService, _holidayService, _stockSplitService, out totalLossProfit, out changedCeiItems);

                    List<StockOperationView> lstStockOperation = new List<StockOperationView>();

                    _portfolioService.CheckDivergence(portfolio.IdPortfolio, lstStockPortfolioGrouped, lstStockOperationBuyGrouped, lstStockOperationSellGrouped, lstStockOperationPassFolio, ref lstStockOperation, false, _operationItemService, _operationService, resultServiceStock.Value);
                    decimal totalLossProfitOp = 0;
                    _portfolioService.SaveOperationPassfolio(resultServiceStock.Value, lstStockOperation, lstStockOperationSellGrouped, lstStockOperationPassFolio, portfolio, idUser, lastSync, _operationService, _operationItemService, _logger, _operationItemHistService, _portfolioService, _operationHistService, _systemSettingsService, _portfolioPerformanceService, _stockService, _performanceStockService, _holidayService, _stockSplitService, out totalLossProfitOp, out changedCeiItems);

                    totalLossProfit += totalLossProfitOp;

                    List<string> divs = _dividendService.RestorePastDividends(portfolio.IdPortfolio, 2, _operationItemService, _dividendCalendarService, _operationService, _stockService, _performanceStockService, _portfolioService, lastSync);
                    dividendCeiItems.AddRange(divs);

                    //_portfolioService.SaveDividend(resultServiceStock.Value, _mapper.Map<List<DividendImportView>>(importPassfolioResult.ListDividend), portfolio, idUser, _dividendService, _dividendTypeService, _logger, out dividendCeiItems);

                    //Save or update products
                    ResultServiceObject<IEnumerable<ProductUserView>> productsUser = _financialProductService.GetProductsByCategoryAndTrader(ProductCategoryEnum.CryptoCurrencies, resultTraderService.Value.IdTrader);

                    var financialInstitution = _financialProductService.GetFinancialInstitutionByExternalCode("Passfolio").Value.FinancialInstitutionID;

                    if (importPassfolioResult.ListCrypto.Count.Equals(0))
                    {
                        foreach (var itemProductsUser in productsUser.Value)
                        {
                            ProductUser product = new ProductUser(itemProductsUser);
                            product.CurrentValue = 0;
                            product.Active = false;
                            _financialProductService.Update(product);
                        }
                    }
                    else
                    {
                        //Itens para adicionar ou editar
                        foreach (var itemCrypto in importPassfolioResult.ListCrypto)
                        {
                            bool addItem = true;

                            foreach (var itemProductsUser in productsUser.Value)
                            {
                                if (itemCrypto.Currency.ToLower().Equals(itemProductsUser.ExternalName.ToLower()))
                                {
                                    addItem = false;

                                    ProductUser product = new ProductUser(itemProductsUser);
                                    product.CurrentValue = itemCrypto.Amount;

                                    if (itemCrypto.Amount == 0)
                                    {
                                        product.Active = false;
                                    }

                                    if (itemProductsUser.CurrentValue != product.CurrentValue)
                                    {
                                        _financialProductService.Update(product);
                                    }
                                }
                            }

                            if (addItem)
                            {
                                var product = _financialProductService.GetFinancialProductByExternalCode(itemCrypto.Currency.ToLower());

                                if (product.Value != null)
                                {
                                    ProductUser productUser = new ProductUser();
                                    productUser.CurrentValue = itemCrypto.Amount;
                                    productUser.FinancialInstitutionID = financialInstitution;
                                    productUser.TraderID = resultTraderService.Value.IdTrader;
                                    productUser.ProductID = product.Value.ProductID;
                                    _financialProductService.Insert(productUser);
                                }
                            }

                        }

                        //Itens para remover
                        foreach (var itemProductsUser in productsUser.Value)
                        {
                            bool removeItem = true;

                            foreach (var itemCrypto in importPassfolioResult.ListCrypto)
                            {
                                if (itemCrypto.Currency.ToLower().Equals(itemProductsUser.ExternalName))
                                {
                                    removeItem = false;
                                }
                            }

                            if (removeItem)
                            {
                                ProductUser product = new ProductUser(itemProductsUser);
                                product.CurrentValue = 0;
                                _financialProductService.Update(product);
                            }
                        }
                    }

                    _portfolioService.SendNotificationImportation(automaticProcess, idUser, true, string.Empty, _deviceService, _notificationHistoricalService, _cacheService, _notificationService, _logger);

                    _portfolioService.CalculatePerformance(portfolio.IdPortfolio, _systemSettingsService, _portfolioPerformanceService, _stockService, _operationService, _performanceStockService, _holidayService, 0);

                    ResultServiceObject<Trader> resultServiceTrader = _traderService.GetByIdentifierAndUserActive(email, idUser, TraderTypeEnum.Passfolio);

                    _traderService.UpdateSyncData(resultTraderService.Value.IdTrader, DateTime.Now);

                    ResultServiceObject<bool> resultRec = _operationService.RecoveryAveragePrice(portfolio.IdPortfolio, _operationHistService, _operationItemHistService);

                    if (resultRec.Success && resultRec.Value)
                    {
                        _ = _logger.SendInformationAsync(new { Message = string.Format("Recover Avg Price {0}", _globalAuthenticationService.IdUser) });
                    }

                    _logger.SendDebugAsync(new { JobDebugInfo = "AutoSyncPortfolios (Passfolio) - User portfolio updated!" });

                    if (!newPortfolio && changedCeiItems != null && changedCeiItems.Count > 0)
                    {
                        changedCeiItems = changedCeiItems.Distinct().ToList();
                        changedCeiItems = changedCeiItems.OrderBy(stk => stk).ToList();
                        string stocks = string.Join(", ", changedCeiItems);
                        //_portfolioService.SendNotificationNewItensOnPortfolio(idUser, false, stocks, _settingsService, _deviceService, _notificationHistoricalService, _cacheService, _notificationService, _logger);
                    }

                    if (!newPortfolio && dividendCeiItems != null && dividendCeiItems.Count > 0)
                    {
                        dividendCeiItems = dividendCeiItems.Distinct().ToList();
                        dividendCeiItems = dividendCeiItems.OrderBy(stk => stk).ToList();
                        string stocks = string.Join(", ", dividendCeiItems);
                        _portfolioService.SendNotificationNewItensOnPortfolio(idUser, true, stocks, _settingsService, _deviceService, _notificationHistoricalService, _cacheService, _notificationService, _logger, resultTraderService.Value.ShowOnPatrimony);
                    }
                }
                else
                {
                    if (importPassfolioResult.PasswordWrong)
                    {
                        _traderService.SaveTrader(email, _cipherService.Encrypt(auth), idUser, true, false, TraderTypeEnum.Passfolio);

                        _logger.SendDebugAsync(new { JobDebugInfo = "AutoSyncPortfolios (Passfolio) - User blocked!" });
                    }

                    _portfolioService.SendNotificationImportation(automaticProcess, idUser, false, importPassfolioResult.Message, _deviceService, _notificationHistoricalService, _cacheService, _notificationService, _logger);

                    _ = _logger.SendInformationAsync(new { Message = string.Format("{0} {1} {2} {3}", importPassfolioResult.Message, idUser, email, auth) });
                }
            }
        }

        private ResultResponseObject<TraderVM> ImportMercadoBitcoin(string identifier, string password, string idUser, bool automaticProcess)
        {
            ResultResponseObject<TraderVM> resultResponseObject = null;

            RequestHeader requestHeader = new RequestHeader(identifier, password);
            var responseMercadoBitcoin = _accountInfoClient.GetAsync(requestHeader).Result;

            using (_uow.Create())
            {
                if (responseMercadoBitcoin != null &&
                    responseMercadoBitcoin.status_code.Equals(100))
                {
                    MercadoBitcoin.Interface.Model.Response.IntegrationRoot responseApiMercadoBitCoin = JsonConvert.DeserializeObject<MercadoBitcoin.Interface.Model.Response.IntegrationRoot>(responseMercadoBitcoin.Content);

                    ResultServiceObject<Trader> resultTraderService = _traderService.SaveTrader(identifier, _cipherService.Encrypt(password), idUser, false, false, TraderTypeEnum.MercadoBitcoin);

                    //Save or update products
                    ResultServiceObject<IEnumerable<ProductUserView>> productsUser = _financialProductService.GetProductsByCategoryAndTrader(ProductCategoryEnum.CryptoCurrencies, resultTraderService.Value.IdTrader);

                    ResultServiceObject<IEnumerable<ProductUserView>> exchangeAccount = _financialProductService.GetProductsByCategoryAndTrader(ProductCategoryEnum.Account, resultTraderService.Value.IdTrader);

                    var financialInstitution = _financialProductService.GetFinancialInstitutionByExternalCode("MBitcoin").Value.FinancialInstitutionID;

                    //atom
                    var productATOM = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("atom"));
                    var atom = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.atom.total);

                    if (productATOM != null)
                    {
                        ProductUser productUser = new ProductUser(productATOM);

                        if (atom == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != atom)
                        {
                            productUser.CurrentValue = atom;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (atom != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = atom;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("atom").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //alice
                    var productAlice = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("alice"));
                    var alice = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.alice.total);

                    if (productAlice != null)
                    {
                        ProductUser productUser = new ProductUser(productAlice);

                        if (alice == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != alice)
                        {
                            productUser.CurrentValue = alice;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (alice != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = alice;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("alice").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //perp
                    var productPERP = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("perp"));
                    var perp = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.perp.total);

                    if (productPERP != null)
                    {
                        ProductUser productUser = new ProductUser(productPERP);

                        if (perp == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != perp)
                        {
                            productUser.CurrentValue = perp;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (perp != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = perp;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("perp").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //dao
                    var productDOGE = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("doge"));
                    var doge = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.doge.total);

                    if (productDOGE != null)
                    {
                        ProductUser productUser = new ProductUser(productDOGE);

                        if (doge == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != doge)
                        {
                            productUser.CurrentValue = doge;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (doge != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = doge;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("doge").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //fil
                    var productFIL = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("fil"));
                    var fil = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.fil.total);

                    if (productFIL != null)
                    {
                        ProductUser productUser = new ProductUser(productFIL);

                        if (fil == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != fil)
                        {
                            productUser.CurrentValue = fil;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (fil != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = fil;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("fil").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //fil
                    var productICP = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("icp"));
                    var icp = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.icp.total);

                    if (productICP != null)
                    {
                        ProductUser productUser = new ProductUser(productICP);

                        if (icp == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != icp)
                        {
                            productUser.CurrentValue = icp;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (icp != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = icp;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("icp").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //dot
                    var productDOT = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("dot"));
                    var dot = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.dot.total);

                    if (productDOT != null)
                    {
                        ProductUser productUser = new ProductUser(productDOT);

                        if (dot == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != dot)
                        {
                            productUser.CurrentValue = dot;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (dot != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = dot;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("dot").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //avax
                    var productAVAX = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("avax"));
                    var avax = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.avax.total);

                    if (productAVAX != null)
                    {
                        ProductUser productUser = new ProductUser(productAVAX);

                        if (avax == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != avax)
                        {
                            productUser.CurrentValue = avax;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (avax != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = avax;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("avax").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //qnt
                    var productQNT = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("qnt"));
                    var qnt = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.qnt.total);

                    if (productQNT != null)
                    {
                        ProductUser productUser = new ProductUser(productQNT);

                        if (qnt == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != qnt)
                        {
                            productUser.CurrentValue = qnt;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (qnt != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = qnt;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("qnt").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //rad
                    var productRAD = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("rad"));
                    var rad = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.rad.total);

                    if (productRAD != null)
                    {
                        ProductUser productUser = new ProductUser(productRAD);

                        if (rad == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != rad)
                        {
                            productUser.CurrentValue = rad;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (rad != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = rad;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("rad").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //req
                    var productREQ = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("req"));
                    var req = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.req.total);

                    if (productREQ != null)
                    {
                        ProductUser productUser = new ProductUser(productREQ);

                        if (req == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != req)
                        {
                            productUser.CurrentValue = req;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (req != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = req;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("req").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }


                    //sand
                    var productSAND = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("sand"));
                    var sand = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.sand.total);

                    if (productSAND != null)
                    {
                        ProductUser productUser = new ProductUser(productSAND);

                        if (sand == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != sand)
                        {
                            productUser.CurrentValue = sand;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (sand != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = sand;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("sand").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //shib
                    var productSLP = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("slp"));
                    var slp = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.slp.total);

                    if (productSLP != null)
                    {
                        ProductUser productUser = new ProductUser(productSLP);

                        if (slp == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != slp)
                        {
                            productUser.CurrentValue = slp;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (slp != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = slp;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("slp").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //shib
                    var productSHIB = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("shib"));
                    var shib = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.shib.total);

                    if (productSHIB != null)
                    {
                        ProductUser productUser = new ProductUser(productSHIB);

                        if (shib == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != shib)
                        {
                            productUser.CurrentValue = shib;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (shib != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = shib;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("shib").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //xlm
                    var productXLM = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("xlm"));
                    var xlm = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.xlm.total);

                    if (productXLM != null)
                    {
                        ProductUser productUser = new ProductUser(productXLM);

                        if (xlm == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != xlm)
                        {
                            productUser.CurrentValue = xlm;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (xlm != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = xlm;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("xlm").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //ygg
                    var productXTZ = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("xtz"));
                    var xtz = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.xtz.total);

                    if (productXTZ != null)
                    {
                        ProductUser productUser = new ProductUser(productXTZ);

                        if (xtz == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != xtz)
                        {
                            productUser.CurrentValue = xtz;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (xtz != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = xtz;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("xtz").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //ygg
                    var productYGG = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("ygg"));
                    var ygg = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.ygg.total);

                    if (productYGG != null)
                    {
                        ProductUser productUser = new ProductUser(productYGG);

                        if (ygg == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != ygg)
                        {
                            productUser.CurrentValue = ygg;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (ygg != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = ygg;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("ygg").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //sol
                    var productSOL = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("sol"));
                    var sol = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.sol.total);

                    if (productSOL != null)
                    {
                        ProductUser productUser = new ProductUser(productSOL);

                        if (sol == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != sol)
                        {
                            productUser.CurrentValue = sol;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (sol != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = sol;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("sol").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //yfi
                    var productYFI = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("yfi"));
                    var yfi = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.yfi.total);

                    if (productYFI != null)
                    {
                        ProductUser productUser = new ProductUser(productYFI);

                        if (yfi == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != yfi)
                        {
                            productUser.CurrentValue = yfi;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (yfi != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = yfi;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("yfi").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //wbx
                    var productWBX = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("wbx"));
                    var wbx = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.wbx.total);

                    if (productWBX != null)
                    {
                        ProductUser productUser = new ProductUser(productWBX);

                        if (wbx == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != wbx)
                        {
                            productUser.CurrentValue = wbx;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (wbx != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = wbx;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("wbx").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //wbtc
                    var productWBTC = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("wbtc"));
                    var wbtc = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.wbtc.total);

                    if (productWBTC != null)
                    {
                        ProductUser productUser = new ProductUser(productWBTC);

                        if (wbtc == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != wbtc)
                        {
                            productUser.CurrentValue = wbtc;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (wbtc != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = wbtc;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("wbtc").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //omg
                    var productSushi = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("sushi"));
                    var sushi = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.sushi.total);

                    if (productSushi != null)
                    {
                        ProductUser productUser = new ProductUser(productSushi);

                        if (sushi == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != sushi)
                        {
                            productUser.CurrentValue = sushi;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (sushi != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = sushi;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("sushi").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //omg
                    var productOMG = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("omg"));
                    var omg = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.omg.total);

                    if (productOMG != null)
                    {
                        ProductUser productUser = new ProductUser(productOMG);

                        if (omg == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != omg)
                        {
                            productUser.CurrentValue = omg;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (omg != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = omg;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("omg").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //mkr
                    var productMKR = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("mkr"));
                    var mkr = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.mkr.total);

                    if (productMKR != null)
                    {
                        ProductUser productUser = new ProductUser(productMKR);

                        if (mkr == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != mkr)
                        {
                            productUser.CurrentValue = mkr;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (mkr != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = mkr;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("mkr").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //matic
                    var productMATIC = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("matic"));
                    var matic = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.matic.total);

                    if (productMATIC != null)
                    {
                        ProductUser productUser = new ProductUser(productMATIC);

                        if (matic == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != matic)
                        {
                            productUser.CurrentValue = matic;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (matic != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = matic;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("matic").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //comp
                    var productCOMP = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("comp"));
                    var comp = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.comp.total);

                    if (productCOMP != null)
                    {
                        ProductUser productUser = new ProductUser(productCOMP);

                        if (comp == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != comp)
                        {
                            productUser.CurrentValue = comp;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (comp != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = comp;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("comp").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //bnt
                    var productBNT = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("bnt"));
                    var bnt = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.bnt.total);

                    if (productBNT != null)
                    {
                        ProductUser productUser = new ProductUser(productBNT);

                        if (bnt == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != bnt)
                        {
                            productUser.CurrentValue = bnt;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (bnt != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = bnt;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("bnt").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //band
                    var productBAND = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("band"));
                    var band = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.band.total);

                    if (productBAND != null)
                    {
                        ProductUser productUser = new ProductUser(productBAND);

                        if (band == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != band)
                        {
                            productUser.CurrentValue = band;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (band != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = band;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("band").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //bal
                    var productBAL = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("bal"));
                    var bal = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.bal.total);

                    if (productBAL != null)
                    {
                        ProductUser productUser = new ProductUser(productBAL);

                        if (bal == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != bal)
                        {
                            productUser.CurrentValue = bal;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (bal != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = bal;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("bal").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //ankr
                    var productANKR = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("ankr"));
                    var ankr = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.ankr.total);

                    if (productANKR != null)
                    {
                        ProductUser productUser = new ProductUser(productANKR);

                        if (ankr == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != ankr)
                        {
                            productUser.CurrentValue = ankr;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (ankr != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = ankr;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("ankr").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //ADA
                    var productADA = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("ada"));
                    var ada = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.ada.total);

                    if (productADA != null)
                    {
                        ProductUser productUser = new ProductUser(productADA);

                        if (ada == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != ada)
                        {
                            productUser.CurrentValue = ada;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (ada != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = ada;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("ada").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //JUVFT
                    var productJUV = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("juv"));
                    var juv = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.juvft.total);

                    if (productJUV != null)
                    {
                        ProductUser productUser = new ProductUser(productJUV);

                        if (juv == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != juv)
                        {
                            productUser.CurrentValue = juv;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (juv != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = juv;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("juv").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //PSGFT
                    var productPSG = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("psg"));
                    var psg = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.psgft.total);

                    if (productPSG != null)
                    {
                        ProductUser productUser = new ProductUser(productPSG);

                        if (psg == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != psg)
                        {
                            productUser.CurrentValue = psg;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (psg != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = psg;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("psg").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //ren
                    var productREN = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("ren"));
                    var ren = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.Ren.total);

                    if (productREN != null)
                    {
                        ProductUser productUser = new ProductUser(productREN);

                        if (ren == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != ren)
                        {
                            productUser.CurrentValue = ren;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (ren != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = ren;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("ren").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //dai
                    var productDAI = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("dai"));
                    var dai = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.DAI.total);

                    if (productDAI != null)
                    {
                        ProductUser productUser = new ProductUser(productDAI);

                        if (dai == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != dai)
                        {
                            productUser.CurrentValue = dai;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (dai != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = dai;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("dai").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //aave
                    var productAAXE = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("aave"));
                    var aave = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.Aave.total);

                    if (productAAXE != null)
                    {
                        ProductUser productUser = new ProductUser(productAAXE);

                        if (aave == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != aave)
                        {
                            productUser.CurrentValue = aave;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (aave != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = aave;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("aave").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //axs
                    var productAXS = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("axs"));
                    var axs = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.Axs.total);

                    if (productAXS != null)
                    {
                        ProductUser productUser = new ProductUser(productAXS);

                        if (axs == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != axs)
                        {
                            productUser.CurrentValue = axs;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (axs != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = axs;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("axs").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //bat
                    var productBAT = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("bat"));
                    var bat = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.Bat.total);

                    if (productBAT != null)
                    {
                        ProductUser productUser = new ProductUser(productBAT);

                        if (bat == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != bat)
                        {
                            productUser.CurrentValue = bat;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (bat != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = bat;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("bat").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //crv
                    var productCRV = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("crv"));
                    var crv = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.Crv.total);

                    if (productCRV != null)
                    {
                        ProductUser productUser = new ProductUser(productCRV);

                        if (crv == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != crv)
                        {
                            productUser.CurrentValue = crv;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (crv != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = crv;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("crv").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //enj
                    var productENJ = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("enj"));
                    var enj = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.Enj.total);

                    if (productENJ != null)
                    {
                        ProductUser productUser = new ProductUser(productENJ);

                        if (enj == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != enj)
                        {
                            productUser.CurrentValue = enj;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (enj != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = enj;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("enj").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }


                    //grt
                    var productGRT = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("grt"));
                    var grt = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.Grt.total);

                    if (productGRT != null)
                    {
                        ProductUser productUser = new ProductUser(productGRT);

                        if (grt == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != grt)
                        {
                            productUser.CurrentValue = grt;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (grt != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = grt;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("grt").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //knc
                    var productKNC = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("knc"));
                    var knc = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.Knc.total);

                    if (productKNC != null)
                    {
                        ProductUser productUser = new ProductUser(productKNC);

                        if (knc == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != knc)
                        {
                            productUser.CurrentValue = knc;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (knc != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = knc;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("knc").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //mana
                    var productMANA = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("mana"));
                    var mana = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.Mana.total);

                    if (productMANA != null)
                    {
                        ProductUser productUser = new ProductUser(productMANA);

                        if (mana == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != mana)
                        {
                            productUser.CurrentValue = mana;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (mana != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = mana;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("mana").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //snx
                    var productSNX = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("snx"));
                    var snx = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.Snx.total);

                    if (productSNX != null)
                    {
                        ProductUser productUser = new ProductUser(productSNX);

                        if (snx == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != snx)
                        {
                            productUser.CurrentValue = snx;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (snx != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = snx;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("snx").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //uni
                    var productUNI = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("uni"));
                    var uni = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.Uni.total);

                    if (productUNI != null)
                    {
                        ProductUser productUser = new ProductUser(productUNI);

                        if (uni == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != uni)
                        {
                            productUser.CurrentValue = uni;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (uni != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = uni;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("uni").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //zrx
                    var productZRX = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("zrx"));
                    var zrx = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.Zrx.total);

                    if (productZRX != null)
                    {
                        ProductUser productUser = new ProductUser(productZRX);

                        if (zrx == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != zrx)
                        {
                            productUser.CurrentValue = zrx;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (zrx != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = zrx;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("zrx").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //btc
                    var productBTC = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("btc"));
                    var btc = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.btc.total);

                    if (productBTC != null)
                    {
                        ProductUser productUser = new ProductUser(productBTC);

                        if (btc == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != btc)
                        {
                            productUser.CurrentValue = btc;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (btc != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = btc;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("btc").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //bch
                    var productBCH = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("bch"));
                    var bch = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.bch.total);

                    if (productBCH != null)
                    {
                        ProductUser productUser = new ProductUser(productBCH);

                        if (bch == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != bch)
                        {
                            productUser.CurrentValue = bch;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (bch != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = bch;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("bch").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //eth
                    var productETH = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("eth"));
                    var eth = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.eth.total);

                    if (productETH != null)
                    {
                        ProductUser productUser = new ProductUser(productETH);

                        if (eth == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != eth)
                        {
                            productUser.CurrentValue = eth;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (eth != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = eth;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("eth").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //ltc
                    var productLTC = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("ltc"));
                    var ltc = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.ltc.total);

                    if (productLTC != null)
                    {
                        ProductUser productUser = new ProductUser(productLTC);

                        if (ltc == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != ltc)
                        {
                            productUser.CurrentValue = ltc;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (ltc != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = ltc;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("ltc").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //xrp
                    var productXRP = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("xrp"));
                    var xrp = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.xrp.total);

                    if (productXRP != null)
                    {
                        ProductUser productUser = new ProductUser(productXRP);

                        if (xrp == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != xrp)
                        {
                            productUser.CurrentValue = xrp;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (xrp != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = xrp;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("xrp").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //usdc
                    var productUSDC = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("usdc"));
                    var usdc = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.usdc.total);

                    if (productUSDC != null)
                    {
                        ProductUser productUser = new ProductUser(productUSDC);

                        if (usdc == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != usdc)
                        {
                            productUser.CurrentValue = usdc;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (usdc != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = usdc;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("usdc").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //link
                    var productLink = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("link"));
                    var link = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.link.total);

                    if (productLink != null)
                    {
                        ProductUser productUser = new ProductUser(productLink);

                        if (link == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != link)
                        {
                            productUser.CurrentValue = link;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (link != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = link;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("link").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //chz
                    var productChz = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("chz"));
                    var chz = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.chz.total);

                    if (productChz != null)
                    {
                        ProductUser productUser = new ProductUser(productChz);

                        if (chz == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != chz)
                        {
                            productUser.CurrentValue = chz;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (chz != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = chz;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("chz").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //brl - Saldo na corretora
                    var productBRL = exchangeAccount.Value.FirstOrDefault(item => item.ExternalName.Equals("Conta Corretora"));
                    var brl = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.brl.available);

                    if (productBRL != null)
                    {
                        ProductUser productUser = new ProductUser(productBRL);

                        if (brl == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != brl)
                        {
                            productUser.CurrentValue = brl;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (brl != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = brl;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("Conta Corretora").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //paxg
                    var productPaxg = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("paxg"));
                    var paxg = decimal.Parse(responseApiMercadoBitCoin.response_data.balance.paxg.total);

                    if (productPaxg != null)
                    {
                        ProductUser productUser = new ProductUser(productPaxg);

                        if (paxg == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != paxg)
                        {
                            productUser.CurrentValue = paxg;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (paxg != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = paxg;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("paxg").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    _portfolioService.SendNotificationImportation(automaticProcess, idUser, true, string.Empty, _deviceService, _notificationHistoricalService, _cacheService, _notificationService, _logger);

                    _logger.SendDebugAsync(new { JobDebugInfo = "ImportMercadoBitcoin - User portfolio updated!" });

                    resultResponseObject = _mapper.Map<ResultResponseObject<TraderVM>>(resultTraderService);
                }
                else if (responseMercadoBitcoin != null &&
                    responseMercadoBitcoin.status_code.Equals(201))
                {

                    ResultServiceObject<Trader> resultServiceTrader = _traderService.GetByIdentifierAndUserActive(identifier, idUser, TraderTypeEnum.MercadoBitcoin);

                    if (resultServiceTrader.Success && resultServiceTrader.Value != null)
                    {
                        _traderService.SaveTrader(identifier, _cipherService.Encrypt(password), idUser, true, false, TraderTypeEnum.MercadoBitcoin);

                        _logger.SendDebugAsync(new { JobDebugInfo = "ImportMercadoBitcoin - User credentials invalid! Update lastSync date." });
                    }


                    string mensagem = "Valor IDENTIFICADOR incorreto. Verifique a informação e tente novamente.";

                    _portfolioService.SendNotificationImportation(automaticProcess, idUser, false, mensagem, _deviceService, _notificationHistoricalService, _cacheService, _notificationService, _logger);

                    _ = _logger.SendInformationAsync(new { Message = string.Format("{0} {1} {2} {3}", mensagem, idUser, identifier, password) });

                    resultResponseObject = new ResultResponseObject<TraderVM>();
                    resultResponseObject.Success = false;
                    resultResponseObject.ErrorMessages.Add(mensagem);
                }
                else if (responseMercadoBitcoin != null &&
                    responseMercadoBitcoin.status_code.Equals(202))
                {

                    ResultServiceObject<Trader> resultServiceTrader = _traderService.GetByIdentifierAndUserActive(identifier, idUser, TraderTypeEnum.MercadoBitcoin);

                    if (resultServiceTrader.Success && resultServiceTrader.Value != null)
                    {
                        _traderService.SaveTrader(identifier, _cipherService.Encrypt(password), idUser, true, false, TraderTypeEnum.MercadoBitcoin);

                        _logger.SendDebugAsync(new { JobDebugInfo = "ImportMercadoBitcoin - User credentials invalid! Update lastSync date." });
                    }

                    string mensagem = "Valor SEGREDO incorreto. Verifique a informação e tente novamente.";

                    _portfolioService.SendNotificationImportation(automaticProcess, idUser, false, mensagem, _deviceService, _notificationHistoricalService, _cacheService, _notificationService, _logger);

                    _ = _logger.SendInformationAsync(new { Message = string.Format("{0} {1} {2} {3}", mensagem, idUser, identifier, password) });

                    resultResponseObject = new ResultResponseObject<TraderVM>();
                    resultResponseObject.Success = false;
                    resultResponseObject.ErrorMessages.Add(mensagem);
                }
                else
                {
                    string mensagem = "Não foi possível realizar a operação de sincronização. Tente novamente mais tarde.";

                    _portfolioService.SendNotificationImportation(automaticProcess, idUser, false, mensagem, _deviceService, _notificationHistoricalService, _cacheService, _notificationService, _logger);

                    resultResponseObject = new ResultResponseObject<TraderVM>();
                    resultResponseObject.Success = false;
                    resultResponseObject.ErrorMessages.Add(mensagem);
                }
            }

            return resultResponseObject;
        }


        private ResultResponseObject<TraderVM> ImportBitcoinTrade(string identifier, string password, string idUser, bool automaticProcess)
        {
            ResultResponseObject<TraderVM> resultResponseObject = null;

            Dividendos.BitcoinTrade.Interface.Model.Root balanceResponses = _bitcoinTradeHelper.GetUserPosition(password, _logger);


            using (_uow.Create())
            {
                if (balanceResponses.code == null || !balanceResponses.code.Equals("invalidApiKey"))
                {
                    ResultServiceObject<Trader> resultTraderService = _traderService.SaveTrader(identifier, _cipherService.Encrypt(password), idUser, false, false, TraderTypeEnum.BitcoinTrade);

                    //Save or update products
                    ResultServiceObject<IEnumerable<ProductUserView>> productsUser = _financialProductService.GetProductsByCategoryAndTrader(ProductCategoryEnum.CryptoCurrencies, resultTraderService.Value.IdTrader);

                    var financialInstitution = _financialProductService.GetFinancialInstitutionByExternalCode("BitcoinTrade").Value.FinancialInstitutionID;

                    //import
                    foreach (var itemPortfolio in balanceResponses.data)
                    {
                        var product = productsUser.Value.FirstOrDefault(item => item.ExternalName.ToLower().Equals(itemPortfolio.currency_code.ToLower()));

                        if (product != null)
                        {
                            ProductUser productUser = new ProductUser(product);

                            if (itemPortfolio.available_amount == 0)
                            {
                                productUser.Active = false;
                            }

                            if (productUser.CurrentValue != itemPortfolio.available_amount)
                            {
                                productUser.CurrentValue = itemPortfolio.available_amount;
                                _financialProductService.Update(productUser);
                            }
                        }
                        else
                        {
                            if (itemPortfolio.available_amount != 0)
                            {
                                ProductUser productUser = new ProductUser();
                                productUser.CurrentValue = itemPortfolio.available_amount;
                                productUser.FinancialInstitutionID = financialInstitution;
                                productUser.TraderID = resultTraderService.Value.IdTrader;

                                var productBase = _financialProductService.GetFinancialProductByExternalCode(itemPortfolio.currency_code.ToLower());

                                if (productBase.Value != null)
                                {
                                    productUser.ProductID = productBase.Value.ProductID;
                                    _financialProductService.Insert(productUser);
                                }
                                else if (productBase.Value == null)
                                {
                                    _logger.SendDebugAsync(new { Binance = string.Concat("BitcoinTrade - Asset not registred: ", itemPortfolio.currency_code) });
                                }
                            }
                        }
                    }

                    //remove
                    foreach (var itemOnUserWallet in productsUser.Value)
                    {
                        if (!balanceResponses.data.Exists(item => item.currency_code.ToLower().Equals(itemOnUserWallet.ExternalName.ToLower())))
                        {
                            ProductUser productUser = new ProductUser(itemOnUserWallet);
                            productUser.CurrentValue = 0;
                            productUser.Active = false;
                            _financialProductService.Update(productUser);
                        }
                    }

                    _portfolioService.SendNotificationImportation(automaticProcess, idUser, true, string.Empty, _deviceService, _notificationHistoricalService, _cacheService, _notificationService, _logger);

                    _logger.SendDebugAsync(new { JobDebugInfo = "BitcoinTrade - User portfolio updated!" });

                    resultResponseObject = _mapper.Map<ResultResponseObject<TraderVM>>(resultTraderService);
                }
                else
                {
                    if (!automaticProcess)
                    {
                        ResultServiceObject<Trader> resultServiceTrader = _traderService.GetByIdentifierAndUserActive(identifier, idUser, TraderTypeEnum.Binance);

                        if (resultServiceTrader.Success && resultServiceTrader.Value != null)
                        {
                            _traderService.SaveTrader(identifier, _cipherService.Encrypt(password), idUser, true, false, TraderTypeEnum.Binance);

                            _logger.SendDebugAsync(new { JobDebugInfo = "ImportBinance - User credentials invalid! Update lastSync date." });
                        }
                    }

                    string mensagem = "A API Key está incorreta. Verifique a informação e tente novamente.";

                    _portfolioService.SendNotificationImportation(automaticProcess, idUser, false, mensagem, _deviceService, _notificationHistoricalService, _cacheService, _notificationService, _logger);

                    _ = _logger.SendInformationAsync(new { Message = string.Format("{0} {1} {2} {3}", mensagem, idUser, identifier, password) });

                    resultResponseObject = new ResultResponseObject<TraderVM>();
                    resultResponseObject.Success = false;
                    resultResponseObject.ErrorMessages.Add(mensagem);
                }
            }

            return resultResponseObject;
        }

        private ResultResponseObject<TraderVM> ImportCoinbase(string identifier, string password, string idUser, bool automaticProcess)
        {
            ResultResponseObject<TraderVM> resultResponseObject = null;

            List<Coinbase.Interface.Model.BalanceResponse> balances = _coinbaseHelper.GetUserPosition(identifier, password, _logger);


            using (_uow.Create())
            {
                if (balances != null)
                {
                    ResultServiceObject<Trader> resultTraderService = _traderService.SaveTrader(identifier, _cipherService.Encrypt(password), idUser, false, false, TraderTypeEnum.Coinbase);

                    //Save or update products
                    ResultServiceObject<IEnumerable<ProductUserView>> productsUser = _financialProductService.GetProductsByCategoryAndTrader(ProductCategoryEnum.CryptoCurrencies, resultTraderService.Value.IdTrader);

                    var financialInstitution = _financialProductService.GetFinancialInstitutionByExternalCode("Coinbase").Value.FinancialInstitutionID;

                    //import
                    foreach (var itemPortfolio in balances)
                    {
                        var product = productsUser.Value.FirstOrDefault(item => item.ExternalName.ToLower().Equals(itemPortfolio.asset.ToLower()));

                        if (product != null)
                        {
                            ProductUser productUser = new ProductUser(product);

                            if (itemPortfolio.free == 0)
                            {
                                productUser.Active = false;
                            }

                            if (productUser.CurrentValue != itemPortfolio.free)
                            {
                                productUser.CurrentValue = itemPortfolio.free;
                                _financialProductService.Update(productUser);
                            }
                        }
                        else
                        {
                            if (itemPortfolio.free != 0)
                            {
                                ProductUser productUser = new ProductUser();
                                productUser.CurrentValue = itemPortfolio.free;
                                productUser.FinancialInstitutionID = financialInstitution;
                                productUser.TraderID = resultTraderService.Value.IdTrader;

                                var productBase = _financialProductService.GetFinancialProductByExternalCode(itemPortfolio.asset.ToLower());

                                if (productBase.Value != null)
                                {
                                    productUser.ProductID = productBase.Value.ProductID;
                                    _financialProductService.Insert(productUser);
                                }
                                else if (productBase.Value == null)
                                {
                                    _logger.SendDebugAsync(new { Binance = string.Concat("Coinbase - Asset not registred: ", itemPortfolio.asset) });
                                }
                            }
                        }
                    }

                    //remove
                    foreach (var itemOnUserWallet in productsUser.Value)
                    {
                        if (!balances.Exists(item => item.asset.ToLower().Equals(itemOnUserWallet.ExternalName.ToLower())))
                        {
                            ProductUser productUser = new ProductUser(itemOnUserWallet);
                            productUser.CurrentValue = 0;
                            productUser.Active = false;
                            _financialProductService.Update(productUser);
                        }
                    }

                    _portfolioService.SendNotificationImportation(automaticProcess, idUser, true, string.Empty, _deviceService, _notificationHistoricalService, _cacheService, _notificationService, _logger);

                    _logger.SendDebugAsync(new { JobDebugInfo = "Coinbase - User portfolio updated!" });

                    resultResponseObject = _mapper.Map<ResultResponseObject<TraderVM>>(resultTraderService);
                }
                else
                {
                    //if (!automaticProcess)
                    //{
                    //    ResultServiceObject<Trader> resultServiceTrader = _traderService.GetByIdentifierAndUser(identifier, idUser, TraderTypeEnum.Binance);

                    //    if (resultServiceTrader.Success && resultServiceTrader.Value != null)
                    //    {
                    //        _traderService.SaveTrader(identifier, _cipherService.Encrypt(password), idUser, true, false, TraderTypeEnum.Binance);

                    //        _logger.SendDebugAsync(new { JobDebugInfo = "Coinbase - User credentials invalid! Update lastSync date." });
                    //    }
                    //}

                    //string mensagem = "A API Key está incorreta. Verifique a informação e tente novamente.";

                    //this.SendNotificationImportation(automaticProcess, idUser, false, mensagem);

                    //_ = _logger.SendInformationAsync(new { Message = string.Format("{0} {1} {2} {3}", mensagem, idUser, identifier, password) });

                    //resultResponseObject = new ResultResponseObject<TraderVM>();
                    //resultResponseObject.Success = false;
                    //resultResponseObject.ErrorMessages.Add(mensagem);
                }
            }

            return resultResponseObject;
        }

        private ResultResponseObject<TraderVM> ImportBitPreco(string identifier, string password, string idUser, bool automaticProcess)
        {
            ResultResponseObject<TraderVM> resultResponseObject = null;

            BitPreco.Interface.Model.Root balances = _bitPrecoHelper.GetUserPosition(identifier, password, _logger);


            using (_uow.Create())
            {
                if (balances != null && balances.success)
                {
                    ResultServiceObject<Trader> resultTraderService = _traderService.SaveTrader(identifier, _cipherService.Encrypt(password), idUser, false, false, TraderTypeEnum.BitPreco);

                    //Save or update products
                    ResultServiceObject<IEnumerable<ProductUserView>> productsUser = _financialProductService.GetProductsByCategoryAndTrader(ProductCategoryEnum.CryptoCurrencies, resultTraderService.Value.IdTrader);

                    var financialInstitution = _financialProductService.GetFinancialInstitutionByExternalCode("BitPreco").Value.FinancialInstitutionID;

                    //uni
                    var productSLP = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("slp"));
                    var slp = balances.SLP + balances.SLP_locked;

                    if (productSLP != null)
                    {
                        ProductUser productUser = new ProductUser(productSLP);

                        if (slp == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != slp)
                        {
                            productUser.CurrentValue = slp;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (slp != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = slp;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("slp").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //uni
                    var productUNI = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("uni"));
                    var uni = balances.UNI + balances.UNI_locked;

                    if (productUNI != null)
                    {
                        ProductUser productUser = new ProductUser(productUNI);

                        if (uni == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != uni)
                        {
                            productUser.CurrentValue = uni;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (uni != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = uni;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("uni").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }
                    //sol
                    var productSOL = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("sol"));
                    var sol = balances.SOL + balances.SOL_locked;

                    if (productSOL != null)
                    {
                        ProductUser productUser = new ProductUser(productSOL);

                        if (sol == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != sol)
                        {
                            productUser.CurrentValue = sol;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (sol != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = sol;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("sol").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //usdt
                    var productUSDC = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("usdc"));
                    var usdc = balances.USDC + balances.USDC_locked;

                    if (productUSDC != null)
                    {
                        ProductUser productUser = new ProductUser(productUSDC);

                        if (usdc == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != usdc)
                        {
                            productUser.CurrentValue = usdc;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (usdc != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = usdc;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("usdc").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //usdt
                    var productUSDT = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("usdt"));
                    var usdt = balances.USDT + balances.USDT_locked;

                    if (productUSDT != null)
                    {
                        ProductUser productUser = new ProductUser(productUSDT);

                        if (usdt == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != usdt)
                        {
                            productUser.CurrentValue = usdt;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (usdt != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = usdt;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("usdt").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //cake
                    var productETH = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("eth"));
                    var eth = balances.ETH + balances.ETH_locked;

                    if (productETH != null)
                    {
                        ProductUser productUser = new ProductUser(productETH);

                        if (eth == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != eth)
                        {
                            productUser.CurrentValue = eth;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (eth != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = eth;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("eth").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //cake
                    var productCAKE = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("cake"));
                    var cake = balances.CAKE + balances.CAKE_locked;

                    if (productCAKE != null)
                    {
                        ProductUser productUser = new ProductUser(productCAKE);

                        if (cake == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != cake)
                        {
                            productUser.CurrentValue = cake;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (cake != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = cake;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("cake").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //brl
                    var productBRL = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("brl"));
                    var brl = balances.BRL + balances.BRL_locked;

                    if (productBRL != null)
                    {
                        ProductUser productUser = new ProductUser(productBRL);

                        if (brl == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != brl)
                        {
                            productUser.CurrentValue = brl;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (brl != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = brl;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("brl").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //bnb
                    var productBNB = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("bnb"));
                    var bnb = balances.BNB + balances.BNB_locked;

                    if (productBNB != null)
                    {
                        ProductUser productUser = new ProductUser(productBNB);

                        if (bnb == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != bnb)
                        {
                            productUser.CurrentValue = bnb;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (bnb != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = bnb;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("bnb").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //axs
                    var productAXS = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("axs"));
                    var axs = balances.AXS + balances.AXS_locked;

                    if (productAXS != null)
                    {
                        ProductUser productUser = new ProductUser(productAXS);

                        if (axs == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != axs)
                        {
                            productUser.CurrentValue = axs;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (axs != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = axs;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("axs").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //ada
                    var productADA = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("ada"));
                    var ada = balances.ADA + balances.ADA_locked;

                    if (productADA != null)
                    {
                        ProductUser productUser = new ProductUser(productADA);

                        if (ada == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != ada)
                        {
                            productUser.CurrentValue = ada;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (ada != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = ada;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("ada").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    //btc
                    var productBTC = productsUser.Value.FirstOrDefault(item => item.ExternalName.Equals("btc"));
                    var btc = balances.BTC + balances.BTC_locked;

                    if (productBTC != null)
                    {
                        ProductUser productUser = new ProductUser(productBTC);

                        if (btc == 0)
                        {
                            productUser.Active = false;
                        }

                        if (productUser.CurrentValue != btc)
                        {
                            productUser.CurrentValue = btc;
                            _financialProductService.Update(productUser);
                        }
                    }
                    else
                    {
                        if (btc != 0)
                        {
                            ProductUser productUser = new ProductUser();
                            productUser.CurrentValue = btc;
                            productUser.FinancialInstitutionID = financialInstitution;
                            productUser.TraderID = resultTraderService.Value.IdTrader;
                            productUser.ProductID = _financialProductService.GetFinancialProductByExternalCode("btc").Value.ProductID;
                            _financialProductService.Insert(productUser);
                        }
                    }

                    _portfolioService.SendNotificationImportation(automaticProcess, idUser, true, string.Empty, _deviceService, _notificationHistoricalService, _cacheService, _notificationService, _logger);

                    _logger.SendDebugAsync(new { JobDebugInfo = "BitPreco - User portfolio updated!" });

                    resultResponseObject = _mapper.Map<ResultResponseObject<TraderVM>>(resultTraderService);
                }
                else
                {
                    if (!automaticProcess)
                    {
                        ResultServiceObject<Trader> resultServiceTrader = _traderService.GetByIdentifierAndUser(identifier, idUser, TraderTypeEnum.BitPreco);

                        if (resultServiceTrader.Success && resultServiceTrader.Value != null)
                        {
                            _traderService.SaveTrader(identifier, _cipherService.Encrypt(password), idUser, true, false, TraderTypeEnum.BitPreco);

                            _logger.SendDebugAsync(new { JobDebugInfo = "BitPreco - User credentials invalid! Update lastSync date." });
                        }
                    }

                    string mensagem = "As credenciais de acesso estão incorretas. Verifique a informação e tente novamente.";

                    _portfolioService.SendNotificationImportation(automaticProcess, idUser, false, mensagem, _deviceService, _notificationHistoricalService, _cacheService, _notificationService, _logger);

                    _ = _logger.SendInformationAsync(new { Message = string.Format("{0} {1} {2} {3}", mensagem, idUser, identifier, password) });

                    resultResponseObject = new ResultResponseObject<TraderVM>();
                    resultResponseObject.Success = false;
                    resultResponseObject.ErrorMessages.Add(mensagem);
                }
            }

            return resultResponseObject;
        }

        private ResultResponseObject<TraderVM> ImportBiscoint(string identifier, string password, string idUser, bool automaticProcess)
        {
            ResultResponseObject<TraderVM> resultResponseObject = null;

            Biscoint.Interface.Model.Root balances = _biscointHelper.GetUserPosition(identifier, password, _logger);


            using (_uow.Create())
            {
                if (balances != null)
                {
                    ResultServiceObject<Trader> resultTraderService = _traderService.SaveTrader(identifier, _cipherService.Encrypt(password), idUser, false, false, TraderTypeEnum.Biscoint);

                    //Save or update products
                    ResultServiceObject<IEnumerable<ProductUserView>> productsUser = _financialProductService.GetProductsByCategoryAndTrader(ProductCategoryEnum.CryptoCurrencies, resultTraderService.Value.IdTrader);

                    var financialInstitution = _financialProductService.GetFinancialInstitutionByExternalCode("Biscoint").Value.FinancialInstitutionID;

                    //import
                    // foreach (var itemPortfolio in balances.available)
                    // {
                    //     var product = productsUser.Value.FirstOrDefault(item => item.ExternalName.ToLower().Equals(itemPortfolio.asset.ToLower()));

                    //     if (product != null)
                    //     {
                    //         ProductUser productUser = new ProductUser(product);

                    //         if (itemPortfolio.amount == 0)
                    //         {
                    //             productUser.Active = false;
                    //         }

                    //         if (productUser.CurrentValue != itemPortfolio.amount)
                    //         {
                    //             productUser.CurrentValue = itemPortfolio.amount;
                    //             _financialProductService.Update(productUser);
                    //         }
                    //     }
                    //     else
                    //     {
                    //         if (itemPortfolio.amount != 0)
                    //         {
                    //             ProductUser productUser = new ProductUser();
                    //             productUser.CurrentValue = itemPortfolio.amount;
                    //             productUser.FinancialInstitutionID = financialInstitution;
                    //             productUser.TraderID = resultTraderService.Value.IdTrader;

                    //             var productBase = _financialProductService.GetFinancialProductByExternalCode(itemPortfolio.asset.ToLower());

                    //             if (productBase.Value != null)
                    //             {
                    //                 productUser.ProductID = productBase.Value.ProductID;
                    //                 _financialProductService.Insert(productUser);
                    //             }
                    //             else if (productBase.Value == null)
                    //             {
                    //                 _logger.SendDebugAsync(new { Binance = string.Concat("Biscoint - Asset not registred: ", itemPortfolio.asset) });
                    //             }
                    //         }
                    //     }
                    // }

                    // //remove
                    // foreach (var itemOnUserWallet in productsUser.Value)
                    // {
                    //     if (!balances.available.Exists(item => item.asset.ToLower().Equals(itemOnUserWallet.ExternalName.ToLower())))
                    //     {
                    //         ProductUser productUser = new ProductUser(itemOnUserWallet);
                    //         productUser.CurrentValue = 0;
                    //         productUser.Active = false;
                    //         _financialProductService.Update(productUser);
                    //     }
                    // }

                    _portfolioService.SendNotificationImportation(automaticProcess, idUser, true, string.Empty, _deviceService, _notificationHistoricalService, _cacheService, _notificationService, _logger);

                    _logger.SendDebugAsync(new { JobDebugInfo = "Biscoint - User portfolio updated!" });

                    resultResponseObject = _mapper.Map<ResultResponseObject<TraderVM>>(resultTraderService);
                }
                else
                {
                    //if (!automaticProcess)
                    //{
                    //    ResultServiceObject<Trader> resultServiceTrader = _traderService.GetByIdentifierAndUser(identifier, idUser, TraderTypeEnum.Binance);

                    //    if (resultServiceTrader.Success && resultServiceTrader.Value != null)
                    //    {
                    //        _traderService.SaveTrader(identifier, _cipherService.Encrypt(password), idUser, true, false, TraderTypeEnum.Binance);

                    //        _logger.SendDebugAsync(new { JobDebugInfo = "Coinbase - User credentials invalid! Update lastSync date." });
                    //    }
                    //}

                    //string mensagem = "A API Key está incorreta. Verifique a informação e tente novamente.";

                    //this.SendNotificationImportation(automaticProcess, idUser, false, mensagem);

                    //_ = _logger.SendInformationAsync(new { Message = string.Format("{0} {1} {2} {3}", mensagem, idUser, identifier, password) });

                    //resultResponseObject = new ResultResponseObject<TraderVM>();
                    //resultResponseObject.Success = false;
                    //resultResponseObject.ErrorMessages.Add(mensagem);
                }
            }

            return resultResponseObject;
        }

        private ResultResponseObject<TraderVM> ImportBitcoinToYou(string identifier, string password, string idUser, bool automaticProcess)
        {
            ResultResponseObject<TraderVM> resultResponseObject = null;

            BitcoinToYou.Interface.Model.Root balances = _bitcoinToYouHelper.GetUserPosition(identifier, password, _logger);


            using (_uow.Create())
            {
                if (!balances.InvalidCredencial)
                {
                    ResultServiceObject<Trader> resultTraderService = _traderService.SaveTrader(identifier, _cipherService.Encrypt(password), idUser, false, false, TraderTypeEnum.BitcoinToYou);

                    //Save or update products
                    ResultServiceObject<IEnumerable<ProductUserView>> productsUser = _financialProductService.GetProductsByCategoryAndTrader(ProductCategoryEnum.CryptoCurrencies, resultTraderService.Value.IdTrader);

                    var financialInstitution = _financialProductService.GetFinancialInstitutionByExternalCode("BitcoinToYou").Value.FinancialInstitutionID;

                    foreach (var itemPortfolio in balances.available)
                    {
                        var product = productsUser.Value.FirstOrDefault(item => item.ExternalName.ToLower().Equals(itemPortfolio.asset.ToLower()));

                        if (product != null)
                        {
                            ProductUser productUser = new ProductUser(product);

                            if (itemPortfolio.amount == 0)
                            {
                                productUser.Active = false;
                            }

                            if (productUser.CurrentValue != itemPortfolio.amount)
                            {
                                productUser.CurrentValue = itemPortfolio.amount;
                                _financialProductService.Update(productUser);
                            }
                        }
                        else
                        {
                            if (itemPortfolio.amount != 0)
                            {
                                ProductUser productUser = new ProductUser();
                                productUser.CurrentValue = itemPortfolio.amount;
                                productUser.FinancialInstitutionID = financialInstitution;
                                productUser.TraderID = resultTraderService.Value.IdTrader;

                                var productBase = _financialProductService.GetFinancialProductByExternalCode(itemPortfolio.asset.ToLower());

                                if (productBase.Value != null)
                                {
                                    productUser.ProductID = productBase.Value.ProductID;
                                    _financialProductService.Insert(productUser);
                                }
                                else if (productBase.Value == null)
                                {
                                    _logger.SendDebugAsync(new { Binance = string.Concat("BitcoinToYou - Asset not registred: ", itemPortfolio.asset) });
                                }
                            }
                        }
                    }

                    //remove
                    foreach (var itemOnUserWallet in productsUser.Value)
                    {
                        if (!balances.available.Exists(item => item.asset.ToLower().Equals(itemOnUserWallet.ExternalName.ToLower())))
                        {
                            ProductUser productUser = new ProductUser(itemOnUserWallet);
                            productUser.CurrentValue = 0;
                            productUser.Active = false;
                            _financialProductService.Update(productUser);
                        }
                    }

                    _portfolioService.SendNotificationImportation(automaticProcess, idUser, true, string.Empty, _deviceService, _notificationHistoricalService, _cacheService, _notificationService, _logger);

                    _logger.SendDebugAsync(new { JobDebugInfo = "BitcoinToYou - User portfolio updated!" });

                    resultResponseObject = _mapper.Map<ResultResponseObject<TraderVM>>(resultTraderService);
                }
                else
                {
                    if (!automaticProcess)
                    {
                        ResultServiceObject<Trader> resultServiceTrader = _traderService.GetByIdentifierAndUserActive(identifier, idUser, TraderTypeEnum.BitcoinToYou);

                        if (resultServiceTrader.Success && resultServiceTrader.Value != null)
                        {
                            //_traderService.SaveTrader(identifier, _cipherService.Encrypt(password), idUser, true, false, TraderTypeEnum.BitcoinToYou);

                            _logger.SendDebugAsync(new { JobDebugInfo = "Bitcointoyou - User credentials invalid! Update lastSync date." });
                        }
                    }

                    string mensagem = "A API Key ou Secret estão incorretos. Verifique a informação e tente novamente.";

                    _portfolioService.SendNotificationImportation(automaticProcess, idUser, false, mensagem, _deviceService, _notificationHistoricalService, _cacheService, _notificationService, _logger);

                    _ = _logger.SendInformationAsync(new { Message = string.Format("{0} {1} {2} {3}", mensagem, idUser, identifier, password) });

                    //resultResponseObject = new ResultResponseObject<TraderVM>();
                    //resultResponseObject.Success = false;
                    //resultResponseObject.ErrorMessages.Add(mensagem);
                }
            }

            return resultResponseObject;
        }

        private ResultResponseObject<TraderVM> ImportBinance(string identifier, string password, string idUser, bool automaticProcess)
        {
            ResultResponseObject<TraderVM> resultResponseObject = null;

            List<BalanceResponse> balanceResponses = _binanceHelper.GetUserPosition(identifier, password, _logger);


            using (_uow.Create())
            {
                if (balanceResponses != null && balanceResponses.Count > 0)
                {
                    ResultServiceObject<Trader> resultTraderService = _traderService.SaveTrader(identifier, _cipherService.Encrypt(password), idUser, false, false, TraderTypeEnum.Binance);

                    //Save or update products
                    ResultServiceObject<IEnumerable<ProductUserView>> productsUser = _financialProductService.GetProductsByCategoryAndTrader(ProductCategoryEnum.CryptoCurrencies, resultTraderService.Value.IdTrader);

                    var financialInstitution = _financialProductService.GetFinancialInstitutionByExternalCode("BINANCE").Value.FinancialInstitutionID;

                    //import
                    foreach (var itemPortfolio in balanceResponses)
                    {
                        var product = productsUser.Value.FirstOrDefault(item => item.ExternalName.ToLower().Equals(itemPortfolio.asset.ToLower()));

                        if (product != null)
                        {
                            ProductUser productUser = new ProductUser(product);
                            if (itemPortfolio.free == 0)
                            {
                                productUser.Active = false;
                            }

                            if (productUser.CurrentValue != itemPortfolio.free)
                            {
                                productUser.CurrentValue = itemPortfolio.free;
                                _financialProductService.Update(productUser);
                            }
                        }
                        else
                        {
                            if (itemPortfolio.free != 0)
                            {
                                ProductUser productUser = new ProductUser();
                                productUser.CurrentValue = itemPortfolio.free;
                                productUser.FinancialInstitutionID = financialInstitution;
                                productUser.TraderID = resultTraderService.Value.IdTrader;

                                var productBase = _financialProductService.GetFinancialProductByExternalCode(itemPortfolio.asset.ToLower());

                                if (productBase.Value != null)
                                {
                                    productUser.ProductID = productBase.Value.ProductID;
                                    _financialProductService.Insert(productUser);
                                }
                                else if (productBase.Value == null)
                                {
                                    this.GetCryptoDetails(itemPortfolio.asset);
                                }
                            }
                        }
                    }

                    //remove
                    foreach (var itemOnUserWallet in productsUser.Value)
                    {
                        if (!balanceResponses.Exists(item => item.asset.ToLower().Equals(itemOnUserWallet.ExternalName.ToLower())))
                        {
                            ProductUser productUser = new ProductUser(itemOnUserWallet);
                            productUser.CurrentValue = 0;
                            productUser.Active = false;
                            _financialProductService.Update(productUser);
                        }
                    }

                    _portfolioService.SendNotificationImportation(automaticProcess, idUser, true, string.Empty, _deviceService, _notificationHistoricalService, _cacheService, _notificationService, _logger);

                    _logger.SendDebugAsync(new { JobDebugInfo = "ImportBinance - User portfolio updated!" });

                    resultResponseObject = _mapper.Map<ResultResponseObject<TraderVM>>(resultTraderService);
                }
                else
                {
                    string mensagem = "A API Key ou Secret Key estão incorretos. Verifique a informação e tente novamente.";

                    _portfolioService.SendNotificationImportation(automaticProcess, idUser, false, mensagem, _deviceService, _notificationHistoricalService, _cacheService, _notificationService, _logger);

                    _ = _logger.SendInformationAsync(new { Message = string.Format("{0} {1} {2} {3}", mensagem, idUser, identifier, password) });

                    resultResponseObject = new ResultResponseObject<TraderVM>();
                    resultResponseObject.Success = false;
                    resultResponseObject.ErrorMessages.Add(mensagem);
                }
            }

            return resultResponseObject;
        }

        public void GetCryptoDetails(string symbol)
        {
            ResultServiceObject<IEnumerable<Entity.Entities.CryptoCurrency>> cryptoCurrencies = _cryptoCurrencyService.GetAll();

            List<Tuple<string, string, string, byte[]>> tickers = _coinMarketCapHelper.GetLogo(symbol);

            foreach (var item in tickers)
            {
                Entity.Entities.CryptoCurrency cryptoCurrency = cryptoCurrencies.Value.FirstOrDefault(itemSelect => itemSelect.Name.ToLower().Equals(item.Item1.ToLower()));

                if (cryptoCurrency == null)
                {
                    Logo logo = new Logo();
                    logo.CompanyCode = item.Item1;
                    logo.LogoImage = item.Item2;
                    logo.URL = _iS3Service.PutImage(item.Item4, string.Concat(symbol, ".png")).Result;
                    logo = _logoService.Insert(logo).Value;

                    Entity.Entities.CryptoCurrency cryptoCurrencyAdd = new Entity.Entities.CryptoCurrency();
                    cryptoCurrencyAdd.LogoID = logo.IdLogo;
                    cryptoCurrencyAdd.Name = item.Item1.ToLower();
                    cryptoCurrencyAdd.MarketPrice = 0;
                    cryptoCurrencyAdd.Variation = 0;
                    cryptoCurrencyAdd.PercentChange1h = 0;
                    cryptoCurrencyAdd.PercentChange24h = 0;
                    cryptoCurrencyAdd.PercentChange7d = 0;
                    cryptoCurrencyAdd.PercentChange30d = 0;
                    cryptoCurrencyAdd.PercentChange60d = 0;
                    cryptoCurrencyAdd.PercentChange90d = 0;
                    _cryptoCurrencyService.Insert(cryptoCurrencyAdd);

                    _financialProductService.AddProduct(new Product() { Description = item.Item3, ExternalName = item.Item1.ToLower(), ProductGuid = Guid.NewGuid(), ProductCategoryID = 2 });
                }
            }
        }

        public ResultResponseObject<TraderVM> CreateAutomaticPortfolio(string identifier, string password)
        {
            ResultServiceObject<Trader> resultTraderService = new ResultServiceObject<Trader>();
            DateTime dtStart = DateTime.Now;

            using (_uow.Create())
            {
                ImportCeiResult importCeiResult = this.ImportStocksAndTesouroDiretoCeiBase(identifier, password, _globalAuthenticationService.IdUser, false);
            }

            ResultResponseObject<TraderVM> resultResponseObject = _mapper.Map<ResultResponseObject<TraderVM>>(resultTraderService);
            return resultResponseObject;
        }

        public ResultResponseObject<TraderVM> ImportMercadoBitcoin(string identifier, string password)
        {
            return this.ImportMercadoBitcoin(identifier, password, _globalAuthenticationService.IdUser, false);
        }

        public ResultResponseObject<TraderVM> ImportBinance(string identifier, string password)
        {
            return this.ImportBinance(identifier, password, _globalAuthenticationService.IdUser, false);
        }

        public ResultResponseObject<TraderVM> ImportBitcoinTrade(string apiKey)
        {
            return this.ImportBitcoinTrade(apiKey, apiKey, _globalAuthenticationService.IdUser, false);
        }

        public ResultResponseObject<TraderVM> ImportCoinbase(string identifier, string password)
        {
            return this.ImportCoinbase(identifier, password, _globalAuthenticationService.IdUser, false);
        }

        public ResultResponseObject<TraderVM> ImportBitcoinToYou(string identifier, string password)
        {
            return this.ImportBitcoinToYou(identifier, password, _globalAuthenticationService.IdUser, false);
        }

        public ResultResponseObject<TraderVM> ImportBiscoint(string identifier, string password)
        {
            return this.ImportBiscoint(identifier, password, _globalAuthenticationService.IdUser, false);
        }

        public ResultResponseObject<TraderVM> ImportBitPreco(string identifier, string password)
        {
            return this.ImportBitPreco(identifier, password, _globalAuthenticationService.IdUser, false);
        }

        public ResultResponseBase DoANewSyncIfNecessary()
        {
            ResultServiceObject<IEnumerable<Trader>> traders;

            using (_uow.Create())
            {
                traders = _traderService.GetByUserActiveAutomatic(_globalAuthenticationService.IdUser);
            }

            if (traders.Value != null && traders.Value.Count() > 0)
            {
                foreach (Trader traderResult in traders.Value)
                {
                    SelectAutomaticImportProcess(traderResult.TraderTypeID, traderResult, traderResult.IdUser, traderResult.LastSync);
                }
            }

            using (_uow.Create())
            {
                ResultServiceObject<IEnumerable<Portfolio>> resultPortfolio = _portfolioService.GetByUser(_globalAuthenticationService.IdUser, true, false, true);

                if (resultPortfolio.Value != null && resultPortfolio.Value.Count() > 0)
                {
                    List<string> dividends = new List<string>();

                    foreach (Portfolio portfolio in resultPortfolio.Value)
                    {
                        //_dividendService.RemoveDuplicated(portfolio.IdPortfolio, _dividendCalendarService);

                        List<string> divs = _dividendService.RestorePastDividends(portfolio.IdPortfolio, portfolio.IdCountry, _operationItemService, _dividendCalendarService, _operationService, _stockService, _performanceStockService, _portfolioService, portfolio.CalculatePerformanceDate);

                        ResultServiceObject<Trader> resultServiceTrader = _traderService.GetById(portfolio.IdTrader);

                        if (resultServiceTrader != null &&
                            resultServiceTrader.Value != null && resultServiceTrader.Value.ShowOnPatrimony)
                        {
                            dividends.AddRange(divs);
                        }

                        if (!portfolio.RestoredDividends.HasValue)
                        {
                            portfolio.CalculatePerformanceDate = DateTime.Now;
                            portfolio.RestoredDividends = true;
                            _portfolioService.Update(portfolio);
                        }
                    }

                    if (dividends != null && dividends.Count > 0)
                    {
                        dividends = dividends.Distinct().ToList();
                        dividends = dividends.OrderBy(stk => stk).ToList();
                        string stocks = string.Join(", ", dividends);
                        _portfolioService.SendNotificationNewItensOnPortfolio(_globalAuthenticationService.IdUser, true, stocks, _settingsService, _deviceService, _notificationHistoricalService, _cacheService, _notificationService, _logger, true);
                    }
                }
            }

            return new ResultResponseBase() { Success = true };
        }

        public ResultResponseObject<SyncQueue> RunSyncFromCEI(string serverName)
        {
            ResultResponseObject<SyncQueue> resultResponseObject = null;
            ResultServiceObject<SyncQueue> resultServiceObject = null;
            bool sendEmail = false;
            string emailBody = string.Empty;
            DateTime dtStart = DateTime.Now;

            //Get item
            using (_uow.Create())
            {
                int timeRandom = new Random().Next(50, 200);
                //Fazer um pequeno time (milesimos de segundo) aqui para evitar pegar o mesmo. Timer randonico
                Thread.Sleep(timeRandom);

                resultServiceObject = _syncQueueService.GetLastAvailable();

                //Mark as InUse
                if (resultServiceObject.Value != null)
                {
                    resultServiceObject.Value.InUse = true;
                    resultServiceObject.Value.ServerName = serverName;
                    _syncQueueService.Update(resultServiceObject.Value);
                    dtStart = DateTime.Now;
                }


                //call import
                if (resultServiceObject.Value != null)
                {
                    //using (_uow.Create())
                    //{
                    ResultServiceObject<Trader> trader = _traderService.GetById(resultServiceObject.Value.IdTrader);

                    if (trader.Value.Active && !trader.Value.BlockedCei)
                    {
                        ImportCeiResult importCeiResult = new ImportCeiResult() { Imported = false };

                        try
                        {
                            importCeiResult = this.ImportStocksAndTesouroDiretoCeiBase(trader.Value.Identifier, trader.Value.Password, trader.Value.IdUser, resultServiceObject.Value.AutomaticImport);
                        }
                        catch (Exception ex)
                        {
                            _logger.SendErrorAsync(ex);
                        }

                        if (importCeiResult.Imported)
                        {
                            //mark as done or attempts
                            resultServiceObject.Value.Done = true;
                            resultServiceObject.Value.ExecutionTime = DateTime.Now.Date.Add(DateTime.Now.Subtract(dtStart));
                            resultServiceObject.Value.Message = importCeiResult.Message;
                            _syncQueueService.Update(resultServiceObject.Value);
                        }
                        else
                        {
                            //mark as done or attempts
                            if (importCeiResult.ErrorCEI)
                            {
                                if (resultServiceObject.Value.Attempts > 2)
                                {
                                    resultServiceObject.Value.Done = true;
                                    sendEmail = true;
                                    ResultServiceObject<EmailTemplate> emailTemplate = _emailTemplateService.GetById(5);
                                    emailBody = string.Format(emailTemplate.Value.Template, resultServiceObject.Value.IdTrader);
                                }
                                else
                                {
                                    resultServiceObject.Value.Attempts = resultServiceObject.Value.Attempts + 1;
                                    resultServiceObject.Value.Done = false;
                                }
                            }
                            else
                            {
                                resultServiceObject.Value.Done = true;
                            }

                            resultServiceObject.Value.InUse = false;
                            resultServiceObject.Value.ExecutionTime = DateTime.Now.Date.Add(DateTime.Now.Subtract(dtStart));
                            resultServiceObject.Value.Message = importCeiResult.Message;
                            _syncQueueService.Update(resultServiceObject.Value);
                        }
                    }
                    else
                    {
                        if (resultServiceObject.Value != null)
                        {
                            resultServiceObject.Value.Done = true;
                            resultServiceObject.Value.InUse = false;
                            _syncQueueService.Update(resultServiceObject.Value);
                        }
                    }
                    //}
                }
            }

            if (sendEmail)
            {
                _notificationService.SendMail("contato@dividendos.me", "FALHA CEI", emailBody);
            }

            resultResponseObject = new ResultResponseObject<SyncQueue>();
            resultResponseObject.Success = true;

            return resultResponseObject;
        }

        public ResultResponseObject<TraderVM> ImportStocksAndTesouroDiretoCei(string identifier, string password, string idUser)
        {
            ResultResponseObject<TraderVM> resultResponseObject = null;
            ImportCeiResult importCeiResult = null;

            using (_uow.Create())
            {
                importCeiResult = this.ImportStocksAndTesouroDiretoCeiBase(identifier, password, idUser, true);
            }

            resultResponseObject = new ResultResponseObject<TraderVM>();
            resultResponseObject.Success = importCeiResult.Imported;
            resultResponseObject.ErrorMessages.Add(importCeiResult.Message);

            return resultResponseObject;
        }

        public ImportCeiResult ImportStocksAndTesouroDiretoCeiBase(string identifier, string password, string idUser, bool automaticProcess, bool hangfire = false, TraderTypeEnum traderTypeEnum = TraderTypeEnum.RendaVariavelAndTesouroDiretoCEI)
        {
            ImportCeiResult importCeiResult = new ImportCeiResult();
            importCeiResult.Imported = _scrapySchedulerService.CreateTask(identifier, password, idUser, automaticProcess, _traderService, _scrapySchedulerService, _subscriptionService, hangfire, traderTypeEnum);

            return importCeiResult;
        }

        public void CalculatePerformanceOneByOne()
        {
            _logger.SendDebugAsync(new { JobDebugInfo = "iniciando CalculatePerformanceForAllPortfolios" });

            ResultServiceObject<IEnumerable<Portfolio>> resultService = new ResultServiceObject<IEnumerable<Portfolio>>();

            using (_uow.Create())
            {
                resultService = _portfolioService.GetLastPortfoliosWithoutCalculation();
            }


            foreach (var item in resultService.Value)
            {
                using (_uow.Create())
                {
                    _portfolioService.CalculatePerformance(item.IdPortfolio, _systemSettingsService, _portfolioPerformanceService, _stockService, _operationService, _performanceStockService, _holidayService);
                }
            }

            _logger.SendDebugAsync(new { JobDebugInfo = "finalizando CalculatePerformanceForAllPortfolios" });
        }

        public ResultResponseBase Disable(Guid idportfolio)
        {
            ResultResponseBase resultResponseBase = new ResultResponseBase();
            ResultServiceBase resultServiceBase = new ResultServiceBase();
            using (_uow.Create())
            {
                ResultServiceObject<Portfolio> resultServiceObject = _portfolioService.GetByGuid(idportfolio);

                ResultServiceObject<IEnumerable<SubPortfolio>> resultSubPortfolio = _subPortfolioService.GetByPortfolio(resultServiceObject.Value.IdPortfolio);

                foreach (var item in resultSubPortfolio.Value)
                {
                    _subPortfolioService.Disable(item);
                }

                _portfolioService.Disable(resultServiceObject.Value.IdPortfolio);

                ResultServiceObject<Trader> resultTrader = _traderService.GetById(resultServiceObject.Value.IdTrader);

                //remove financial products by trader
                ResultServiceObject<IEnumerable<ProductUserView>> resultProducts = _financialProductService.GetProductsByTrader(resultServiceObject.Value.IdTrader);

                foreach (var item in resultProducts.Value)
                {
                    ProductUser productToRemove = new ProductUser()
                    {
                        Active = false,
                        CreatedDate = item.CreatedDate,
                        CurrentValue = item.CurrentValue,
                        FinancialInstitutionID = item.FinancialInstitutionID,
                        ProductID = item.ProductID,
                        ProductUserGuid = item.ProductUserGuid,
                        ProductUserID = item.ProductUserID,
                        TraderID = item.TraderID
                    };

                    _financialProductService.Update(productToRemove);
                }

                if (resultTrader.Value != null)
                {
                    _traderService.Disable(resultTrader.Value.IdTrader);
                }
            }

            resultResponseBase = _mapper.Map<ResultResponseBase>(resultServiceBase);

            return resultResponseBase;
        }

        public ResultResponseObject<List<PortfolioModel>> GetPortfoliosAndSubPortfolios()
        {
            return GetPortfoliosAndSubPortfoliosBase(false, false);
        }

        public ResultResponseObject<List<PortfolioModel>> GetPortfoliosAndSubPortfoliosV3(bool includeCryptoWallets)
        {
            return GetPortfoliosAndSubPortfoliosBase(includeCryptoWallets, false);
        }

        public ResultResponseObject<List<PortfolioModel>> GetPortfoliosAndSubPortfoliosV2()
        {
            return GetPortfoliosAndSubPortfoliosBase(true, false);
        }

        public ResultResponseObject<List<PortfolioModel>> GetPortfoliosAndSubPortfoliosBase(bool includeCryptoWallets, bool newCryptoModule)
        {
            ResultResponseObject<List<PortfolioModel>> resultServiceObject = new ResultResponseObject<List<PortfolioModel>>();
            List<PortfolioModel> lstPortfolio = new List<PortfolioModel>();

            try
            {
                ResultServiceObject<IEnumerable<Portfolio>> resultService = new ResultServiceObject<IEnumerable<Portfolio>>();
                ResultServiceObject<IEnumerable<SubPortfolio>> resultSubPortfolio = new ResultServiceObject<IEnumerable<SubPortfolio>>();

                using (_uow.Create())
                {
                    resultService = _portfolioService.GetByUser(_globalAuthenticationService.IdUser, true, false);

                    if (resultService.Success)
                    {
                        foreach (var portfolio in resultService.Value)
                        {
                            lstPortfolio.Add(new PortfolioModel { GuidPortfolioSub = portfolio.GuidPortfolio, Name = portfolio.Name, IsCrypto = false, IdCountry = portfolio.IdCountry });

                            resultSubPortfolio = _subPortfolioService.GetByPortfolio(portfolio.IdPortfolio);

                            var trader = _traderService.GetById(portfolio.IdTrader);

                            if (resultSubPortfolio.Success)
                            {
                                foreach (var subPortfolio in resultSubPortfolio.Value)
                                {
                                    lstPortfolio.Add(new PortfolioModel { GuidPortfolioSub = subPortfolio.GuidSubPortfolio, Name = subPortfolio.Name, LastSyncDate = trader.Value.LastSync, IdCountry = portfolio.IdCountry, IsCrypto = false });
                                }
                            }
                        }
                    }

                    if (includeCryptoWallets)
                    {
                        ResultServiceObject<IEnumerable<Trader>> resultTrader = _traderService.GetByUserActive(_globalAuthenticationService.IdUser);

                        foreach (var item in resultTrader.Value)
                        {
                            if (item.TraderTypeID == (int)TraderTypeEnum.Binance)
                            {
                                lstPortfolio.Add(new PortfolioModel { GuidPortfolioSub = item.GuidTrader, Name = "Binance", LastSyncDate = item.LastSync, IdCountry = 2, IsCrypto = true });
                            }
                            else if (item.TraderTypeID == (int)TraderTypeEnum.MercadoBitcoin)
                            {
                                lstPortfolio.Add(new PortfolioModel { GuidPortfolioSub = item.GuidTrader, Name = "Mercado Bitcoin", LastSyncDate = item.LastSync, IdCountry = 2, IsCrypto = true });
                            }
                            else if (item.TraderTypeID == (int)TraderTypeEnum.Passfolio)
                            {
                                lstPortfolio.Add(new PortfolioModel { GuidPortfolioSub = item.GuidTrader, Name = "Passfolio (Cripto Ativos)", LastSyncDate = item.LastSync, IdCountry = 2, IsCrypto = true });
                            }
                            else if (item.TraderTypeID == (int)TraderTypeEnum.BitcoinTrade)
                            {
                                lstPortfolio.Add(new PortfolioModel { GuidPortfolioSub = item.GuidTrader, Name = "BitcoinTrade", LastSyncDate = item.LastSync, IdCountry = 2, IsCrypto = true });
                            }
                        }

                        if (newCryptoModule)
                        {
                            ResultServiceObject<IEnumerable<CryptoPortfolio>> resultCryptoPortoflios = _cryptoPortfolioService.GetByUser(_globalAuthenticationService.IdUser, true, null);

                            if (resultCryptoPortoflios.Value != null && resultCryptoPortoflios.Value.Count() > 0)
                            {
                                foreach (CryptoPortfolio cryptoPortfolio in resultCryptoPortoflios.Value)
                                {
                                    lstPortfolio.Add(new PortfolioModel { GuidPortfolioSub = cryptoPortfolio.GuidCryptoPortfolio, Name = cryptoPortfolio.Name, IsCrypto = true, IdCountry = 2 });

                                    ResultServiceObject<IEnumerable<CryptoSubPortfolio>> resultCryptoSubPortoflios = _cryptoSubPortfolioService.GetByIdCryptoPortfolio(cryptoPortfolio.IdCryptoPortfolio);

                                    if (resultCryptoSubPortoflios.Value != null && resultCryptoSubPortoflios.Value.Count() > 0)
                                    {
                                        foreach (CryptoSubPortfolio cryptoSubPortfolio in resultCryptoSubPortoflios.Value)
                                        {
                                            lstPortfolio.Add(new PortfolioModel { GuidPortfolioSub = cryptoSubPortfolio.GuidCryptoSubPortfolio, Name = cryptoSubPortfolio.Name, IsCrypto = true, IdCountry = 2 });
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                _ = _logger.SendErrorAsync(exception);
                throw;
            }

            resultServiceObject.Value = lstPortfolio;
            resultServiceObject.Success = true;

            return resultServiceObject;
        }

        #region Charts

        public ResultResponseObject<StockAllocationChart> GetPortfolioStockAllocation(Guid guidPortfolioSub)
        {
            StockAllocationChart stockAllocationChart = new StockAllocationChart();
            ResultResponseObject<StockAllocationChart> resultServiceObject = new ResultResponseObject<StockAllocationChart>();
            List<ChartLabelValue> lstChartLabelValue = new List<ChartLabelValue>();
            stockAllocationChart.ListChartLabelValue = lstChartLabelValue;

            using (_uow.Create())
            {
                ResultServiceObject<Portfolio> resultPortfolio = _portfolioService.GetByGuid(guidPortfolioSub);
                ResultServiceObject<SubPortfolio> resultSubPortfolio = _subPortfolioService.GetByGuid(guidPortfolioSub);

                if (resultSubPortfolio.Success && resultPortfolio.Success)
                {
                    Portfolio portfolio = resultPortfolio.Value;
                    SubPortfolio subportfolio = resultSubPortfolio.Value;

                    long idPortfolio = 0;
                    bool isSub = false;

                    if (portfolio != null)
                    {
                        idPortfolio = portfolio.IdPortfolio;
                        stockAllocationChart.IdCountry = portfolio.IdCountry;
                    }
                    else if (subportfolio != null)
                    {
                        idPortfolio = subportfolio.IdPortfolio;
                        isSub = true;

                        resultPortfolio = _portfolioService.GetById(subportfolio.IdPortfolio);

                        if (resultPortfolio.Value != null)
                        {
                            stockAllocationChart.IdCountry = resultPortfolio.Value.IdCountry;
                        }
                    }


                    ResultServiceObject<PortfolioPerformance> resultPortfolioPerformance = _portfolioPerformanceService.GetByCalculationDate(idPortfolio);
                    PortfolioPerformance portfolioPerformance = null;

                    if (resultPortfolioPerformance.Success)
                    {
                        portfolioPerformance = resultPortfolioPerformance.Value;

                        if (portfolioPerformance != null)
                        {
                            ResultServiceObject<IEnumerable<StockAllocation>> resultStockAllocation = new ResultServiceObject<IEnumerable<StockAllocation>>();

                            if (isSub)
                            {
                                resultStockAllocation = _performanceStockService.GetSumSubPortfolioPerformance(portfolioPerformance.IdPortfolioPerformance, subportfolio.IdSubPortfolio);
                            }
                            else
                            {
                                resultStockAllocation = _performanceStockService.GetSumIdPortfolioPerformance(portfolioPerformance.IdPortfolioPerformance);
                            }

                            if (resultStockAllocation.Success)
                            {
                                if (resultStockAllocation.Value != null && resultStockAllocation.Value.Count() > 0)
                                {
                                    resultStockAllocation.Value = resultStockAllocation.Value.OrderBy(sector => sector.TotalMarket);

                                    foreach (StockAllocation stockAllocation in resultStockAllocation.Value)
                                    {
                                        ChartLabelValue chartLabelValue = new ChartLabelValue();
                                        chartLabelValue.Label = stockAllocation.Symbol;
                                        chartLabelValue.Value = stockAllocation.TotalMarket.ToString("n2", new CultureInfo("en-us")).Replace(",", string.Empty);
                                        lstChartLabelValue.Add(chartLabelValue);
                                    }
                                }

                                stockAllocationChart.TotalMarket = resultStockAllocation.Value.Sum(sectorTmp => sectorTmp.TotalMarket).ToString("n2", new CultureInfo("pt-br"));
                                stockAllocationChart.LastUpdated = DateTime.Now.ToString("HH:mm:ss");
                                resultServiceObject.Value = stockAllocationChart;
                            }
                        }
                    }
                }
            }

            resultServiceObject.Success = true;

            return resultServiceObject;
        }

        public ResultResponseObject<StockTypeChart> GetMainChartByLoggedUser()
        {
            ResultResponseObject<StockTypeChart> financialProductsChartReturn = new ResultResponseObject<StockTypeChart>() { Success = true };

            StockTypeChart financialProductsChart = new StockTypeChart();

            financialProductsChart.ListChartLabelValue = new List<ChartLabelValue>();

            decimal totalFixedIncome = 0, totalFunds = 0, totalCryptos = 0, totalStockPortfolios = 0, dolarQuotation = 0, euroQuotation = 0;
            bool hasSubscription = false;
            ResultServiceObject<IEnumerable<Portfolio>> portfolios;

            using (_uow.Create())
            {
                portfolios = _portfolioService.GetByUser(_globalAuthenticationService.IdUser, true, true);

                var dolar = _indicatorSeriesService.GetLatest((int)IndicatorTypeEnum.USDBRL).Value.FirstOrDefault();

                if (dolar != null)
                {
                    dolarQuotation = dolar.Points;
                }

                var euro = _indicatorSeriesService.GetLatest((int)IndicatorTypeEnum.EURBRL).Value.FirstOrDefault();

                if (euro != null)
                {
                    euroQuotation = euro.Points;
                }
            }

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

                foreach (var item in resultService.Value)
                {
                    if (item.ProductCategoryID.Equals((int)ProductCategoryEnum.CryptoCurrencies))
                    {
                        item.CurrentValue = cryptoCurrencies.Value.Where(currency => currency.Name.ToString().Equals(item.ExternalName.ToString())).Sum(totalSum => totalSum.MarketPrice) * item.CurrentValue;

                        totalCryptos = totalCryptos + item.CurrentValue;
                    }
                    else if (item.ProductCategoryID.Equals((int)ProductCategoryEnum.TesouroDireto) ||
                        item.ProductCategoryID.Equals((int)ProductCategoryEnum.CDB) ||
                        item.ProductCategoryID.Equals((int)ProductCategoryEnum.LCA) ||
                        item.ProductCategoryID.Equals((int)ProductCategoryEnum.LCI) ||
                        item.ProductCategoryID.Equals((int)ProductCategoryEnum.Savings))
                    {
                        totalFixedIncome = totalFixedIncome + item.CurrentValue;
                    }
                    else if (item.ProductCategoryID.Equals((int)ProductCategoryEnum.Funds))
                    {
                        totalFunds = totalFunds + item.CurrentValue;
                    }
                    else
                    {
                        ChartLabelValue chartItem = new ChartLabelValue() { Label = item.ProductName, Value = item.CurrentValue.ToString("n2", new CultureInfo("en-us")).Replace(",", string.Empty) };

                        financialProductsChart.ListChartLabelValue.Add(chartItem);
                    }
                }

                //criptos manuais
                var resultManualCryptos = _cryptoPortfolioService.GetByUser(_globalAuthenticationService.IdUser, true, true);

                foreach (CryptoPortfolio itemPortfolio in resultManualCryptos.Value)
                {
                    ResultServiceObject<CryptoPortfolioPerformance> resultServiceObject = _cryptoPortfolioPerformanceService.GetByCalculationDate(itemPortfolio.IdCryptoPortfolio, DateTime.Now);

                    if (resultServiceObject != null && resultServiceObject.Value != null)
                    {
                        if (itemPortfolio.IdFiatCurrency.Equals((int)FiatCurrencyEnum.BRL))
                        {
                            totalCryptos = totalCryptos + resultServiceObject.Value.TotalMarket;
                        }
                        else if (itemPortfolio.IdFiatCurrency.Equals((int)FiatCurrencyEnum.EURO))
                        {
                            var value = resultServiceObject.Value.TotalMarket * euroQuotation;
                            totalCryptos = totalCryptos + value;
                        }
                        else if (itemPortfolio.IdFiatCurrency.Equals((int)FiatCurrencyEnum.USD))
                        {
                            var value = resultServiceObject.Value.TotalMarket * dolarQuotation;
                            totalCryptos = totalCryptos + value;
                        }
                    }
                }
            }

            List<ChartLabelValue> chartLabelValueAllPortfolios = new List<ChartLabelValue>();

            foreach (Portfolio itemPortfolio in portfolios.Value)
            {
                ResultResponseObject<StockTypeChart> stockTypeChart = GetStockTypeChart(itemPortfolio.GuidPortfolio);

                if (stockTypeChart.Value != null)
                {
                    if (stockTypeChart.Value.IdCountry.Equals((int)CountryEnum.EUA) && !hasSubscription)
                    {
                        continue;
                    }

                    if (stockTypeChart.Value.IdCountry.Equals((int)CountryEnum.EUA))
                    {
                        stockTypeChart.Value.TotalMarketInternal = stockTypeChart.Value.TotalMarketInternal * dolarQuotation;

                        foreach (var item in stockTypeChart.Value.ListChartLabelValue)
                        {
                            item.InternalValue = item.InternalValue * dolarQuotation;
                        }
                    }

                    chartLabelValueAllPortfolios.AddRange(stockTypeChart.Value.ListChartLabelValue);
                    totalStockPortfolios = totalStockPortfolios + stockTypeChart.Value.TotalMarketInternal;
                }
            }


            List<ChartLabelValue> chartLabelValueResult = new List<ChartLabelValue>();


            if (totalCryptos > 0)
            {
                ChartLabelValue dataItem = new ChartLabelValue() { Label = "Criptomoedas", Value = totalCryptos.ToString("n2", new CultureInfo("en-us")).Replace(",", string.Empty) };

                financialProductsChart.ListChartLabelValue.Add(dataItem);
            }

            if (totalFixedIncome > 0)
            {
                ChartLabelValue dataItemFixedIncome = new ChartLabelValue() { Label = "Renda Fixa", Value = totalFixedIncome.ToString("n2", new CultureInfo("en-us")).Replace(",", string.Empty) };

                financialProductsChart.ListChartLabelValue.Add(dataItemFixedIncome);
            }

            if (totalFunds > 0)
            {
                ChartLabelValue dataItemFunds = new ChartLabelValue() { Label = "Fundos de Invest.", Value = totalFunds.ToString("n2", new CultureInfo("en-us")).Replace(",", string.Empty) };

                financialProductsChart.ListChartLabelValue.Add(dataItemFunds);
            }

            var totalStocks = chartLabelValueAllPortfolios.Where(item => item.StockTypeEnum.Equals((int)StockTypeEnum.Stocks)).Sum(totalSum => totalSum.InternalValue);

            if (totalStocks > 0)
            {
                chartLabelValueResult.Add(new ChartLabelValue() { Label = "Ações", Value = totalStocks.ToString("n2", new CultureInfo("en-us")).Replace(",", string.Empty) });
            }

            var totalFIIs = chartLabelValueAllPortfolios.Where(item => item.StockTypeEnum.Equals((int)StockTypeEnum.FII)).Sum(totalSum => totalSum.InternalValue);

            if (totalFIIs > 0)
            {
                chartLabelValueResult.Add(new ChartLabelValue() { Label = "Fundos Imobiliários", Value = totalFIIs.ToString("n2", new CultureInfo("en-us")).Replace(",", string.Empty) });
            }

            var totalETFs = chartLabelValueAllPortfolios.Where(item => item.StockTypeEnum.Equals((int)StockTypeEnum.ETF)).Sum(totalSum => totalSum.InternalValue);

            if (totalETFs > 0)
            {
                chartLabelValueResult.Add(new ChartLabelValue() { Label = "ETFs", Value = totalETFs.ToString("n2", new CultureInfo("en-us")).Replace(",", string.Empty) });
            }

            var totalBDRs = chartLabelValueAllPortfolios.Where(item => item.StockTypeEnum.Equals((int)StockTypeEnum.BDR)).Sum(totalSum => totalSum.InternalValue);

            if (totalBDRs > 0)
            {
                chartLabelValueResult.Add(new ChartLabelValue() { Label = "BDRs", Value = totalBDRs.ToString("n2", new CultureInfo("en-us")).Replace(",", string.Empty) });
            }

            var totalREITs = chartLabelValueAllPortfolios.Where(item => item.StockTypeEnum.Equals((int)StockTypeEnum.REIT)).Sum(totalSum => totalSum.InternalValue);

            if (totalREITs > 0)
            {
                chartLabelValueResult.Add(new ChartLabelValue() { Label = "REITs", Value = totalREITs.ToString("n2", new CultureInfo("en-us")).Replace(",", string.Empty) });
            }

            var totalFIPs = chartLabelValueAllPortfolios.Where(item => item.StockTypeEnum.Equals((int)StockTypeEnum.FIP)).Sum(totalSum => totalSum.InternalValue);

            if (totalFIPs > 0)
            {
                chartLabelValueResult.Add(new ChartLabelValue() { Label = "FIPs", Value = totalFIPs.ToString("n2", new CultureInfo("en-us")).Replace(",", string.Empty) });
            }

            financialProductsChart.ListChartLabelValue.AddRange(chartLabelValueResult);

            financialProductsChart.TotalMarket = (totalCryptos + totalStockPortfolios + totalFixedIncome + totalFunds).ToString("n2", new CultureInfo("pt-br"));
            financialProductsChart.LastUpdated = DateTime.Now.ToString("HH:mm:ss");

            financialProductsChartReturn.Value = financialProductsChart;

            return financialProductsChartReturn;
        }

        public ResultResponseObject<StockTypeChart> GetStockTypeChart(Guid guidPortfolioSub)
        {
            StockTypeChart stockTypeChart = new StockTypeChart();
            ResultResponseObject<StockTypeChart> resultServiceObject = new ResultResponseObject<StockTypeChart>();
            List<ChartLabelValue> lstChartLabelValue = new List<ChartLabelValue>();
            stockTypeChart.ListChartLabelValue = lstChartLabelValue;

            using (_uow.Create())
            {
                ResultServiceObject<Portfolio> resultPortfolio = _portfolioService.GetByGuid(guidPortfolioSub);
                ResultServiceObject<SubPortfolio> resultSubPortfolio = _subPortfolioService.GetByGuid(guidPortfolioSub);

                if (resultSubPortfolio.Success && resultPortfolio.Success)
                {
                    Portfolio portfolio = resultPortfolio.Value;
                    SubPortfolio subportfolio = resultSubPortfolio.Value;

                    long idPortfolio = 0;
                    bool isSub = false;

                    if (portfolio != null)
                    {
                        idPortfolio = portfolio.IdPortfolio;
                        stockTypeChart.IdCountry = portfolio.IdCountry;
                    }
                    else if (subportfolio != null)
                    {
                        idPortfolio = subportfolio.IdPortfolio;
                        isSub = true;
                        portfolio = _portfolioService.GetById(idPortfolio).Value;

                        resultPortfolio = _portfolioService.GetById(subportfolio.IdPortfolio);

                        if (resultPortfolio.Value != null)
                        {
                            stockTypeChart.IdCountry = resultPortfolio.Value.IdCountry;
                        }
                    }

                    ResultServiceObject<PortfolioPerformance> resultPortfolioPerformance = _portfolioPerformanceService.GetByCalculationDate(idPortfolio);
                    PortfolioPerformance portfolioPerformance = null;

                    if (resultPortfolioPerformance.Success)
                    {
                        ResultServiceObject<IEnumerable<Stock>> resultServiceStock = _stockService.GetAllByCountry(portfolio.IdCountry);
                        ResultServiceObject<IEnumerable<StockType>> resultStockType = _stockTypeService.GetAll();

                        if (resultServiceStock.Success && resultStockType.Success)
                        {
                            IEnumerable<Stock> stocks = resultServiceStock.Value;
                            portfolioPerformance = resultPortfolioPerformance.Value;
                            IEnumerable<StockType> stockTypes = resultStockType.Value;

                            if (portfolioPerformance != null)
                            {
                                if (isSub)
                                {
                                    GetSubPortfolioStockType(stockTypeChart, resultServiceObject, lstChartLabelValue, portfolioPerformance, stocks, stockTypes, subportfolio.IdSubPortfolio);
                                }
                                else
                                {
                                    GetPortfolioStockType(stockTypeChart, resultServiceObject, lstChartLabelValue, portfolioPerformance, stocks, stockTypes);
                                }
                            }
                        }
                    }
                }
            }

            resultServiceObject.Success = true;

            return resultServiceObject;
        }

        private void GetSubPortfolioStockType(StockTypeChart stockTypeChart, ResultResponseObject<StockTypeChart> resultServiceObject, List<ChartLabelValue> lstChartLabelValue, PortfolioPerformance portfolioPerformance, IEnumerable<Stock> stocks, IEnumerable<StockType> stockTypes, long idSubPortfolio)
        {
            ResultServiceObject<IEnumerable<PerformanceStock>> resultPerformanceStock = _performanceStockService.GetByIdPortfolioPerformance(portfolioPerformance.IdPortfolioPerformance);
            ResultServiceObject<IEnumerable<SubPortfolioOperation>> resultSubportfolioOperation = _subPortfolioOperationService.GetBySubPortfolio(idSubPortfolio);
            ResultServiceObject<IEnumerable<Operation>> resultOperation = _operationService.GetByPortfolio(portfolioPerformance.IdPortfolio);

            if (resultPerformanceStock.Success && resultSubportfolioOperation.Success && resultOperation.Success && resultPerformanceStock.Success)
            {
                IEnumerable<PerformanceStock> performanceStocksDb = resultPerformanceStock.Value;
                IEnumerable<SubPortfolioOperation> subPortfolioOperations = resultSubportfolioOperation.Value;
                IEnumerable<Operation> operations = resultOperation.Value;

                if (performanceStocksDb != null && performanceStocksDb.Count() > 0)
                {
                    if (stockTypes != null && stockTypes.Count() > 0)
                    {
                        decimal total = 0;

                        foreach (StockType stockType in stockTypes)
                        {
                            decimal totalMarket = 0;
                            ChartLabelValue chartLabelValue = new ChartLabelValue();
                            chartLabelValue.Label = stockType.Name;
                            chartLabelValue.StockTypeEnum = stockType.IdStockType;

                            foreach (SubPortfolioOperation subPortfolioOperation in subPortfolioOperations)
                            {
                                Operation operation = operations.FirstOrDefault(operationTmp => operationTmp.IdOperation == subPortfolioOperation.IdOperation && operationTmp.Active == true && operationTmp.IdOperationType == 1);

                                if (operation != null)
                                {
                                    PerformanceStock performanceStock = performanceStocksDb.FirstOrDefault(performanceStockTmp => performanceStockTmp.IdStock == operation.IdStock);

                                    if (performanceStock != null)
                                    {
                                        Stock stock = stocks.FirstOrDefault(stockTmp => stockTmp.IdStock == performanceStock.IdStock);

                                        if (stock != null && stock.IdStockType == stockType.IdStockType)
                                        {
                                            totalMarket += performanceStock.TotalMarket;
                                            total += performanceStock.TotalMarket;
                                        }
                                    }
                                }
                            }

                            chartLabelValue.Value = totalMarket.ToString("n2", new CultureInfo("en-us")).Replace(",", string.Empty);
                            lstChartLabelValue.Add(chartLabelValue);
                        }

                        stockTypeChart.TotalMarket = total.ToString("n2", new CultureInfo("pt-br"));
                        stockTypeChart.LastUpdated = DateTime.Now.ToString("HH:mm:ss");
                        resultServiceObject.Value = stockTypeChart;
                    }
                }
            }
        }

        private void GetPortfolioStockType(StockTypeChart stockTypeChart, ResultResponseObject<StockTypeChart> resultServiceObject, List<ChartLabelValue> lstChartLabelValue, PortfolioPerformance portfolioPerformance, IEnumerable<Stock> stocks, IEnumerable<StockType> stockTypes)
        {
            ResultServiceObject<IEnumerable<PerformanceStock>> resultPerformanceStock = _performanceStockService.GetByIdPortfolioPerformance(portfolioPerformance.IdPortfolioPerformance);
            IEnumerable<PerformanceStock> performanceStocksDb = resultPerformanceStock.Value;

            if (performanceStocksDb != null && performanceStocksDb.Count() > 0)
            {
                if (stockTypes != null && stockTypes.Count() > 0)
                {
                    foreach (StockType stockType in stockTypes)
                    {
                        decimal totalMarket = 0;
                        ChartLabelValue chartLabelValue = new ChartLabelValue();
                        chartLabelValue.Label = stockType.Name;
                        chartLabelValue.StockTypeEnum = stockType.IdStockType;

                        foreach (PerformanceStock performanceStock in performanceStocksDb)
                        {
                            Stock stock = stocks.FirstOrDefault(stockTmp => stockTmp.IdStock == performanceStock.IdStock);

                            if (stock != null && stock.IdStockType == stockType.IdStockType)
                            {
                                totalMarket += performanceStock.TotalMarket;
                            }
                        }

                        chartLabelValue.InternalValue = totalMarket;
                        chartLabelValue.Value = totalMarket.ToString("n2", new CultureInfo("en-us")).Replace(",", string.Empty);
                        lstChartLabelValue.Add(chartLabelValue);
                        stockTypeChart.TotalMarketInternal = performanceStocksDb.Sum(perfStock => perfStock.TotalMarket);
                        stockTypeChart.TotalMarket = stockTypeChart.TotalMarketInternal.ToString("n2", new CultureInfo("pt-br"));
                        stockTypeChart.LastUpdated = DateTime.Now.ToString("HH:mm:ss");
                        resultServiceObject.Value = stockTypeChart;
                    }
                }
            }
        }

        public ResultResponseObject<StockAllocationChart> GetSectorAllocationChart(Guid guidPortfolioSub)
        {
            StockAllocationChart stockAllocationChart = new StockAllocationChart();
            ResultResponseObject<StockAllocationChart> resultServiceObject = new ResultResponseObject<StockAllocationChart>();
            List<ChartLabelValue> lstChartLabelValue = new List<ChartLabelValue>();
            stockAllocationChart.ListChartLabelValue = lstChartLabelValue;

            using (_uow.Create())
            {
                ResultServiceObject<Portfolio> resultPortfolio = _portfolioService.GetByGuid(guidPortfolioSub);
                ResultServiceObject<SubPortfolio> resultSubPortfolio = _subPortfolioService.GetByGuid(guidPortfolioSub);

                if (resultSubPortfolio.Success && resultPortfolio.Success)
                {
                    Portfolio portfolio = resultPortfolio.Value;
                    SubPortfolio subportfolio = resultSubPortfolio.Value;

                    long idPortfolio = 0;
                    bool isSub = false;

                    if (portfolio != null)
                    {
                        idPortfolio = portfolio.IdPortfolio;
                        stockAllocationChart.IdCountry = portfolio.IdCountry;
                    }
                    else if (subportfolio != null)
                    {
                        idPortfolio = subportfolio.IdPortfolio;
                        isSub = true;

                        resultPortfolio = _portfolioService.GetById(subportfolio.IdPortfolio);

                        if (resultPortfolio.Value != null)
                        {
                            stockAllocationChart.IdCountry = resultPortfolio.Value.IdCountry;
                        }
                    }

                    ResultServiceObject<PortfolioPerformance> resultPortfolioPerformance = _portfolioPerformanceService.GetByCalculationDate(idPortfolio);
                    PortfolioPerformance portfolioPerformance = null;

                    if (resultPortfolioPerformance.Success)
                    {
                        portfolioPerformance = resultPortfolioPerformance.Value;

                        if (portfolioPerformance != null)
                        {
                            ResultServiceObject<IEnumerable<SectorView>> resultSectorstest = new ResultServiceObject<IEnumerable<SectorView>>();

                            if (isSub)
                            {
                                resultSectorstest = _sectorService.GetSumSubPortfolioPerformance(portfolioPerformance.IdPortfolioPerformance, subportfolio.IdSubPortfolio);
                            }
                            else
                            {
                                resultSectorstest = _sectorService.GetSumIdPortfolioPerformance(portfolioPerformance.IdPortfolioPerformance);
                            }


                            if (resultSectorstest.Success)
                            {
                                if (resultSectorstest.Value != null && resultSectorstest.Value.Count() > 0)
                                {
                                    resultSectorstest.Value = resultSectorstest.Value.OrderBy(sector => sector.TotalMarket);

                                    foreach (SectorView sector in resultSectorstest.Value)
                                    {
                                        ChartLabelValue chartLabelValue = new ChartLabelValue();
                                        chartLabelValue.Label = sector.Name;
                                        chartLabelValue.Value = sector.TotalMarket.ToString("n2", new CultureInfo("en-us")).Replace(",", string.Empty);
                                        lstChartLabelValue.Add(chartLabelValue);
                                    }
                                }

                                stockAllocationChart.TotalMarket = resultSectorstest.Value.Sum(sectorTmp => sectorTmp.TotalMarket).ToString("n2", new CultureInfo("pt-br"));
                                stockAllocationChart.LastUpdated = DateTime.Now.ToString("HH:mm:ss");
                                resultServiceObject.Value = stockAllocationChart;
                            }
                        }
                    }
                }
            }

            resultServiceObject.Success = true;

            return resultServiceObject;
        }

        public ResultResponseObject<StockAllocationChart> GetSubsectorAllocationChart(Guid guidPortfolioSub)
        {
            StockAllocationChart stockAllocationChart = new StockAllocationChart();
            ResultResponseObject<StockAllocationChart> resultServiceObject = new ResultResponseObject<StockAllocationChart>();
            List<ChartLabelValue> lstChartLabelValue = new List<ChartLabelValue>();
            stockAllocationChart.ListChartLabelValue = lstChartLabelValue;

            using (_uow.Create())
            {
                ResultServiceObject<Portfolio> resultPortfolio = _portfolioService.GetByGuid(guidPortfolioSub);
                ResultServiceObject<SubPortfolio> resultSubPortfolio = _subPortfolioService.GetByGuid(guidPortfolioSub);

                if (resultSubPortfolio.Success && resultPortfolio.Success)
                {
                    Portfolio portfolio = resultPortfolio.Value;
                    SubPortfolio subportfolio = resultSubPortfolio.Value;

                    long idPortfolio = 0;
                    bool isSub = false;

                    if (portfolio != null)
                    {
                        idPortfolio = portfolio.IdPortfolio;
                        stockAllocationChart.IdCountry = portfolio.IdCountry;
                    }
                    else if (subportfolio != null)
                    {
                        idPortfolio = subportfolio.IdPortfolio;
                        isSub = true;

                        resultPortfolio = _portfolioService.GetById(subportfolio.IdPortfolio);

                        if (resultPortfolio.Value != null)
                        {
                            stockAllocationChart.IdCountry = resultPortfolio.Value.IdCountry;
                        }
                    }


                    ResultServiceObject<PortfolioPerformance> resultPortfolioPerformance = _portfolioPerformanceService.GetByCalculationDate(idPortfolio);
                    PortfolioPerformance portfolioPerformance = null;

                    if (resultPortfolioPerformance.Success)
                    {
                        portfolioPerformance = resultPortfolioPerformance.Value;

                        if (portfolioPerformance != null)
                        {
                            ResultServiceObject<IEnumerable<SubsectorView>> resultSubsectors = new ResultServiceObject<IEnumerable<SubsectorView>>();

                            if (isSub)
                            {
                                resultSubsectors = _subsectorService.GetSumSubPortfolioPerformance(portfolioPerformance.IdPortfolioPerformance, subportfolio.IdSubPortfolio);
                            }
                            else
                            {
                                resultSubsectors = _subsectorService.GetSumIdPortfolioPerformance(portfolioPerformance.IdPortfolioPerformance);
                            }

                            if (resultSubsectors.Success)
                            {
                                if (resultSubsectors.Value != null && resultSubsectors.Value.Count() > 0)
                                {
                                    resultSubsectors.Value = resultSubsectors.Value.OrderBy(sector => sector.TotalMarket);

                                    foreach (SubsectorView subsector in resultSubsectors.Value)
                                    {
                                        ChartLabelValue chartLabelValue = new ChartLabelValue();
                                        chartLabelValue.Label = subsector.Name;
                                        chartLabelValue.Value = subsector.TotalMarket.ToString("n2", new CultureInfo("en-us")).Replace(",", string.Empty);
                                        lstChartLabelValue.Add(chartLabelValue);
                                    }
                                }

                                stockAllocationChart.TotalMarket = resultSubsectors.Value.Sum(sectorTmp => sectorTmp.TotalMarket).ToString("n2", new CultureInfo("pt-br"));
                                stockAllocationChart.LastUpdated = DateTime.Now.ToString("HH:mm:ss");
                                resultServiceObject.Value = stockAllocationChart;
                            }
                        }
                    }
                }
            }

            resultServiceObject.Success = true;

            return resultServiceObject;
        }

        public ResultResponseObject<StockAllocationChart> GetSegmentAllocationChart(Guid guidPortfolioSub)
        {
            StockAllocationChart stockAllocationChart = new StockAllocationChart();
            ResultResponseObject<StockAllocationChart> resultServiceObject = new ResultResponseObject<StockAllocationChart>();
            List<ChartLabelValue> lstChartLabelValue = new List<ChartLabelValue>();
            stockAllocationChart.ListChartLabelValue = lstChartLabelValue;

            using (_uow.Create())
            {
                ResultServiceObject<Portfolio> resultPortfolio = _portfolioService.GetByGuid(guidPortfolioSub);
                ResultServiceObject<SubPortfolio> resultSubPortfolio = _subPortfolioService.GetByGuid(guidPortfolioSub);

                if (resultSubPortfolio.Success && resultPortfolio.Success)
                {
                    Portfolio portfolio = resultPortfolio.Value;
                    SubPortfolio subportfolio = resultSubPortfolio.Value;

                    long idPortfolio = 0;
                    bool isSub = false;

                    if (portfolio != null)
                    {
                        idPortfolio = portfolio.IdPortfolio;
                        stockAllocationChart.IdCountry = portfolio.IdCountry;
                    }
                    else if (subportfolio != null)
                    {
                        idPortfolio = subportfolio.IdPortfolio;
                        isSub = true;

                        resultPortfolio = _portfolioService.GetById(subportfolio.IdPortfolio);

                        if (resultPortfolio.Value != null)
                        {
                            stockAllocationChart.IdCountry = resultPortfolio.Value.IdCountry;
                        }
                    }


                    ResultServiceObject<PortfolioPerformance> resultPortfolioPerformance = _portfolioPerformanceService.GetByCalculationDate(idPortfolio);
                    PortfolioPerformance portfolioPerformance = null;

                    if (resultPortfolioPerformance.Success)
                    {
                        portfolioPerformance = resultPortfolioPerformance.Value;

                        if (portfolioPerformance != null)
                        {
                            ResultServiceObject<IEnumerable<SegmentView>> resultSegments = new ResultServiceObject<IEnumerable<SegmentView>>();

                            if (isSub)
                            {
                                resultSegments = _segmentService.GetSumSubPortfolioPerformance(portfolioPerformance.IdPortfolioPerformance, subportfolio.IdSubPortfolio);
                            }
                            else
                            {
                                resultSegments = _segmentService.GetSumIdPortfolioPerformance(portfolioPerformance.IdPortfolioPerformance);
                            }

                            if (resultSegments.Success)
                            {

                                if (resultSegments.Value != null && resultSegments.Value.Count() > 0)
                                {
                                    resultSegments.Value = resultSegments.Value.OrderBy(sector => sector.TotalMarket);

                                    foreach (SegmentView segment in resultSegments.Value)
                                    {
                                        ChartLabelValue chartLabelValue = new ChartLabelValue();
                                        chartLabelValue.Label = segment.Name;
                                        chartLabelValue.Value = segment.TotalMarket.ToString("n2", new CultureInfo("en-us")).Replace(",", string.Empty);
                                        lstChartLabelValue.Add(chartLabelValue);
                                    }
                                }

                                stockAllocationChart.TotalMarket = resultSegments.Value.Sum(sectorTmp => sectorTmp.TotalMarket).ToString("n2", new CultureInfo("pt-br"));
                                stockAllocationChart.LastUpdated = DateTime.Now.ToString("HH:mm:ss");
                                resultServiceObject.Value = stockAllocationChart;
                            }
                        }
                    }
                }
            }

            resultServiceObject.Success = true;

            return resultServiceObject;
        }

        public ResultResponseObject<StockAllocationChart> GetSegmentAllocationChartByType(Guid guidPortfolioSub, API.Model.Request.Stock.StockType stockType)
        {
            StockTypeEnum stockTypeEnum = new StockTypeEnum();
            stockTypeEnum = (StockTypeEnum)stockType;
            StockAllocationChart stockAllocationChart = new StockAllocationChart();
            ResultResponseObject<StockAllocationChart> resultServiceObject = new ResultResponseObject<StockAllocationChart>();
            List<ChartLabelValue> lstChartLabelValue = new List<ChartLabelValue>();
            stockAllocationChart.ListChartLabelValue = lstChartLabelValue;

            using (_uow.Create())
            {
                ResultServiceObject<Portfolio> resultPortfolio = _portfolioService.GetByGuid(guidPortfolioSub);
                ResultServiceObject<SubPortfolio> resultSubPortfolio = _subPortfolioService.GetByGuid(guidPortfolioSub);

                if (resultSubPortfolio.Success && resultPortfolio.Success)
                {
                    Portfolio portfolio = resultPortfolio.Value;
                    SubPortfolio subportfolio = resultSubPortfolio.Value;

                    long idPortfolio = 0;
                    bool isSub = false;

                    if (portfolio != null)
                    {
                        idPortfolio = portfolio.IdPortfolio;
                        stockAllocationChart.IdCountry = portfolio.IdCountry;
                    }
                    else if (subportfolio != null)
                    {
                        idPortfolio = subportfolio.IdPortfolio;
                        isSub = true;

                        resultPortfolio = _portfolioService.GetById(subportfolio.IdPortfolio);

                        if (resultPortfolio.Value != null)
                        {
                            stockAllocationChart.IdCountry = resultPortfolio.Value.IdCountry;
                        }
                    }


                    ResultServiceObject<PortfolioPerformance> resultPortfolioPerformance = _portfolioPerformanceService.GetByCalculationDate(idPortfolio);
                    PortfolioPerformance portfolioPerformance = null;

                    if (resultPortfolioPerformance.Success)
                    {
                        portfolioPerformance = resultPortfolioPerformance.Value;

                        if (portfolioPerformance != null)
                        {
                            ResultServiceObject<IEnumerable<SegmentView>> resultSegments = new ResultServiceObject<IEnumerable<SegmentView>>();

                            if (isSub)
                            {
                                resultSegments = _segmentService.GetSumSubPortfolioPerformance(portfolioPerformance.IdPortfolioPerformance, subportfolio.IdSubPortfolio, stockTypeEnum);
                            }
                            else
                            {
                                resultSegments = _segmentService.GetSumIdPortfolioPerformance(portfolioPerformance.IdPortfolioPerformance, stockTypeEnum);
                            }

                            if (resultSegments.Success)
                            {

                                if (resultSegments.Value != null && resultSegments.Value.Count() > 0)
                                {
                                    resultSegments.Value = resultSegments.Value.OrderBy(sector => sector.TotalMarket);

                                    foreach (SegmentView segment in resultSegments.Value)
                                    {
                                        ChartLabelValue chartLabelValue = new ChartLabelValue();
                                        chartLabelValue.Label = segment.Name;
                                        chartLabelValue.Value = segment.TotalMarket.ToString("n2", new CultureInfo("en-us")).Replace(",", string.Empty);
                                        lstChartLabelValue.Add(chartLabelValue);
                                    }
                                }

                                stockAllocationChart.TotalMarket = resultSegments.Value.Sum(sectorTmp => sectorTmp.TotalMarket).ToString("n2", new CultureInfo("pt-br"));
                                stockAllocationChart.LastUpdated = DateTime.Now.ToString("HH:mm:ss");
                                resultServiceObject.Value = stockAllocationChart;
                            }
                        }
                    }
                }
            }

            resultServiceObject.Success = true;

            return resultServiceObject;
        }

        public ResultResponseObject<PerformanceStockChart> GetPerformanceStockChart(Guid guidPortfolioSub)
        {
            PerformanceStockChart performanceStockChart = new PerformanceStockChart();
            ResultResponseObject<PerformanceStockChart> resultServiceObject = new ResultResponseObject<PerformanceStockChart>();
            List<Dividendos.Application.Interface.Model.Dataset> lstDataset = new List<Dividendos.Application.Interface.Model.Dataset>();
            performanceStockChart.ListDataset = lstDataset;

            using (_uow.Create())
            {
                ResultServiceObject<Portfolio> resultPortfolio = _portfolioService.GetByGuid(guidPortfolioSub);
                ResultServiceObject<SubPortfolio> resultSubPortfolio = _subPortfolioService.GetByGuid(guidPortfolioSub);

                if (resultSubPortfolio.Success && resultPortfolio.Success)
                {
                    Portfolio portfolio = resultPortfolio.Value;
                    SubPortfolio subportfolio = resultSubPortfolio.Value;
                    long idPortfolio = 0;
                    bool isSub = false;

                    if (portfolio != null)
                    {
                        idPortfolio = portfolio.IdPortfolio;
                        performanceStockChart.IdCountry = portfolio.IdCountry;
                    }
                    else if (subportfolio != null)
                    {
                        idPortfolio = subportfolio.IdPortfolio;
                        isSub = true;

                        resultPortfolio = _portfolioService.GetById(subportfolio.IdPortfolio);

                        if (resultPortfolio.Value != null)
                        {
                            performanceStockChart.IdCountry = resultPortfolio.Value.IdCountry;
                        }
                    }


                    ResultServiceObject<PortfolioPerformance> resultPortfolioPerformance = _portfolioPerformanceService.GetByCalculationDate(idPortfolio);
                    PortfolioPerformance portfolioPerformance = null;

                    if (resultPortfolioPerformance.Success)
                    {
                        portfolioPerformance = resultPortfolioPerformance.Value;

                        if (portfolioPerformance != null)
                        {
                            ResultServiceObject<IEnumerable<StockAllocation>> resultStockAllocation = new ResultServiceObject<IEnumerable<StockAllocation>>();

                            if (isSub)
                            {
                                resultStockAllocation = _performanceStockService.GetSumSubPerformanceStock(portfolioPerformance.IdPortfolioPerformance, subportfolio.IdSubPortfolio);
                            }
                            else
                            {
                                resultStockAllocation = _performanceStockService.GetSumPerformanceStock(portfolioPerformance.IdPortfolioPerformance);
                            }

                            if (resultStockAllocation.Success)
                            {
                                int index = 0;
                                foreach (StockAllocation stockAllocation in resultStockAllocation.Value)
                                {
                                    if (index > 19)
                                    {
                                        index = 0;
                                    }

                                    Dividendos.Application.Interface.Model.Dataset dataset = new Dividendos.Application.Interface.Model.Dataset();
                                    dataset.SeriesName = stockAllocation.Symbol;
                                    //dataset.AnchorRadius = "4";
                                    //dataset.AnchorBorderThickness = "1";
                                    dataset.AnchorBgColor = performanceStockChart.ListColor[index];
                                    //dataset.AnchorBordercolor = performanceStockChart.ListColor[index];

                                    dataset.ListData = new List<Dividendos.Application.Interface.Model.Datum>();
                                    Dividendos.Application.Interface.Model.Datum data = new Dividendos.Application.Interface.Model.Datum();
                                    data.TotalMarket = stockAllocation.TotalMarket.ToString("n2", new CultureInfo("en-us")).Replace(",", string.Empty);
                                    decimal perc = stockAllocation.PerformancePerc * 100;
                                    data.Perc = perc.ToString("n2", new CultureInfo("en-us")).Replace(",", string.Empty);
                                    data.Radius = "0.1";

                                    dataset.ListData.Add(data);
                                    lstDataset.Add(dataset);
                                    index++;
                                }

                                performanceStockChart.TotalMarket = resultStockAllocation.Value.Sum(sectorTmp => sectorTmp.TotalMarket).ToString("n2", new CultureInfo("pt-br"));
                                performanceStockChart.LastUpdated = DateTime.Now.ToString("HH:mm:ss");
                                resultServiceObject.Value = performanceStockChart;
                            }
                        }
                    }
                }
            }

            resultServiceObject.Success = true;

            return resultServiceObject;
        }

        public ResultResponseObject<ComparativeChart> GetComparativeChart(Guid guidPortfolioSub, int calculateType, int periodType, int amountSeries)
        {
            ComparativeChart comparativeChart = new ComparativeChart();
            ResultResponseObject<ComparativeChart> resultServiceObject = new ResultResponseObject<ComparativeChart>();
            List<DatasetComp> lstDataset = new List<DatasetComp>();
            comparativeChart.ListDataset = lstDataset;

            using (_uow.Create())
            {
                ResultServiceObject<Portfolio> resultPortfolio = _portfolioService.GetByGuid(guidPortfolioSub);
                ResultServiceObject<SubPortfolio> resultSubPortfolio = _subPortfolioService.GetByGuid(guidPortfolioSub);

                if (resultSubPortfolio.Success && resultPortfolio.Success)
                {
                    Portfolio portfolio = resultPortfolio.Value;
                    SubPortfolio subportfolio = resultSubPortfolio.Value;

                    string portfolioName = string.Empty;

                    long idPortfolio = 0;
                    bool isSub = false;

                    if (portfolio != null)
                    {
                        idPortfolio = portfolio.IdPortfolio;
                        portfolioName = portfolio.Name;
                        comparativeChart.IdCountry = portfolio.IdCountry;
                    }
                    else if (subportfolio != null)
                    {
                        idPortfolio = subportfolio.IdPortfolio;
                        portfolioName = subportfolio.Name;
                        isSub = true;

                        resultPortfolio = _portfolioService.GetById(subportfolio.IdPortfolio);

                        if (resultPortfolio.Success)
                        {
                            portfolio = resultPortfolio.Value;
                            comparativeChart.IdCountry = portfolio.IdCountry;
                        }
                    }

                    List<PortfolioPerformance> lstPortfolioPerformance = new List<PortfolioPerformance>();

                    if (periodType == 1)
                    {
                        ResultServiceObject<IEnumerable<PortfolioPerformance>> resultPortfolioPerformance = _portfolioPerformanceService.GetByRangeDate(portfolio.IdPortfolio);

                        if (resultPortfolioPerformance.Success)
                        {
                            lstPortfolioPerformance = resultPortfolioPerformance.Value.ToList();
                        }
                    }
                    else if (periodType == 2)
                    {
                        DateTime currentDate = DateTime.Now;

                        for (int i = 0; i < amountSeries; i++)
                        {
                            int month = currentDate.AddMonths(-i).Month;

                            ResultServiceObject<PortfolioPerformance> resultPortfolioMonthFirst = _portfolioPerformanceService.GetByMonth(portfolio.IdPortfolio, month, true);
                            ResultServiceObject<PortfolioPerformance> resultPortfolioMonthLast = _portfolioPerformanceService.GetByMonth(portfolio.IdPortfolio, month, false);

                            if (resultPortfolioMonthFirst.Success && resultPortfolioMonthLast.Success && resultPortfolioMonthFirst.Value != null && resultPortfolioMonthLast.Value != null)
                            {
                                if (isSub)
                                {
                                    _portfolioService.CalculateSubPortfolioPerc(resultPortfolioMonthFirst.Value, subportfolio.IdSubPortfolio, _performanceStockService, _operationService, _subPortfolioOperationService);
                                    _portfolioService.CalculateSubPortfolioPerc(resultPortfolioMonthLast.Value, subportfolio.IdSubPortfolio, _performanceStockService, _operationService, _subPortfolioOperationService);
                                }

                                PortfolioPerformance portfolioPerformanceMonth = GetByPeriod(resultPortfolioMonthFirst, resultPortfolioMonthLast);
                                lstPortfolioPerformance.Add(portfolioPerformanceMonth);
                            }
                        }
                    }
                    else if (periodType == 3)
                    {
                        int year = DateTime.Now.Year;

                        ResultServiceObject<PortfolioPerformance> resultPortfolioYearFirst = _portfolioPerformanceService.GetByYear(portfolio.IdPortfolio, year, true);
                        ResultServiceObject<PortfolioPerformance> resultPortfolioYearLast = _portfolioPerformanceService.GetByYear(portfolio.IdPortfolio, year, false);

                        if (resultPortfolioYearFirst.Success && resultPortfolioYearLast.Success && resultPortfolioYearFirst.Value != null && resultPortfolioYearLast.Value != null)
                        {
                            if (isSub)
                            {
                                _portfolioService.CalculateSubPortfolioPerc(resultPortfolioYearFirst.Value, subportfolio.IdSubPortfolio, _performanceStockService, _operationService, _subPortfolioOperationService);
                                _portfolioService.CalculateSubPortfolioPerc(resultPortfolioYearLast.Value, subportfolio.IdSubPortfolio, _performanceStockService, _operationService, _subPortfolioOperationService);
                            }

                            PortfolioPerformance portfolioPerformanceYear = GetByPeriod(resultPortfolioYearFirst, resultPortfolioYearLast);
                            lstPortfolioPerformance.Add(portfolioPerformanceYear);
                        }
                    }

                    if (lstPortfolioPerformance != null && lstPortfolioPerformance.Count > 0)
                    {
                        lstPortfolioPerformance = lstPortfolioPerformance.Take(amountSeries).ToList();

                        decimal totalMarket = 0;
                        comparativeChart.ListCategoryObj = new List<CategoryObj>();
                        CategoryObj categoryObj = new CategoryObj();
                        comparativeChart.ListCategoryObj.Add(categoryObj);

                        categoryObj.ListCategory = new List<CategegoryComp>();
                        lstPortfolioPerformance = lstPortfolioPerformance.OrderBy(perf => perf.CalculationDate).ToList();


                        DatasetComp dataset = new DatasetComp();
                        dataset.SeriesName = portfolioName;
                        dataset.Hidden = 0;
                        dataset.ListData = new List<DataComp>();
                        lstDataset.Add(dataset);

                        DatasetComp datasetTotal = new DatasetComp();
                        datasetTotal.SeriesName = "Total";
                        datasetTotal.RenderAs = "line";
                        datasetTotal.ParentYAxis = "S";
                        datasetTotal.ShowValues = "0";
                        datasetTotal.Hidden = 0;
                        datasetTotal.ListData = new List<DataComp>();



                        foreach (PortfolioPerformance portfolioPerformanceFor in lstPortfolioPerformance)
                        {
                            decimal perc = 0;
                            decimal total = 0;

                            PortfolioPerformance portfolioPerformance = portfolioPerformanceFor;

                            if (isSub && periodType == 1)
                            {
                                portfolioPerformance = _portfolioService.CalculateSubPortfolioPerformance(portfolioPerformanceFor, subportfolio.IdSubPortfolio, _portfolioPerformanceService, _performanceStockService, _operationService, _subPortfolioOperationService);
                            }

                            if (calculateType == 1)
                            {
                                perc = portfolioPerformance.PerformancePercTWR * 100;
                                total = portfolioPerformance.NetValueTWR.HasValue ? portfolioPerformance.NetValueTWR.Value : portfolioPerformance.NetValue;
                            }
                            else
                            {
                                perc = portfolioPerformance.PerformancePerc * 100;
                                total = portfolioPerformance.TotalMarket;
                            }


                            DataComp data = new DataComp();
                            data.Value = perc.ToString("n2", new CultureInfo("en-us")).Replace(",", string.Empty);
                            dataset.ListData.Add(data);

                            DataComp dataTotal = new DataComp();
                            dataTotal.Value = total.ToString("n2", new CultureInfo("en-us")).Replace(",", string.Empty);
                            datasetTotal.ListData.Add(dataTotal);

                            string label = string.Empty;

                            if (periodType == 1)
                            {
                                label = portfolioPerformance.CalculationDate.ToString("dd/MM");
                            }
                            else if (periodType == 2)
                            {
                                label = new CultureInfo("pt-br").DateTimeFormat.GetAbbreviatedMonthName(portfolioPerformance.CalculationDate.Month);
                            }
                            else if (periodType == 3)
                            {
                                label = portfolioPerformance.CalculationDate.Year.ToString();
                            }

                            categoryObj.ListCategory.Add(new CategegoryComp { Label = label, CalculationDate = portfolioPerformance.CalculationDate.Date });
                        }

                        ResultServiceObject<IEnumerable<IndicatorSeriesView>> resultIbov = new ResultServiceObject<IEnumerable<IndicatorSeriesView>>();
                        ResultServiceObject<IEnumerable<IndicatorSeriesView>> resultIfix = new ResultServiceObject<IEnumerable<IndicatorSeriesView>>();
                        ResultServiceObject<IEnumerable<IndicatorSeriesView>> resultIdiv = new ResultServiceObject<IEnumerable<IndicatorSeriesView>>();
                        ResultServiceObject<IEnumerable<IndicatorSeriesView>> resultBova = new ResultServiceObject<IEnumerable<IndicatorSeriesView>>();
                        ResultServiceObject<IEnumerable<IndicatorSeriesView>> resultIvvb = new ResultServiceObject<IEnumerable<IndicatorSeriesView>>();
                        ResultServiceObject<IEnumerable<IndicatorSeriesView>> resultPoupanca = new ResultServiceObject<IEnumerable<IndicatorSeriesView>>();
                        ResultServiceObject<IEnumerable<IndicatorSeriesView>> resultCdi = new ResultServiceObject<IEnumerable<IndicatorSeriesView>>();
                        ResultServiceObject<IEnumerable<IndicatorSeriesView>> resultIpca = new ResultServiceObject<IEnumerable<IndicatorSeriesView>>();

                        if (periodType == 1)
                        {
                            resultIbov = _indicatorSeriesService.GetByRangeDate(1);
                            resultIfix = _indicatorSeriesService.GetByRangeDate(2);
                            resultIdiv = _indicatorSeriesService.GetByRangeDate(6);
                            resultBova = _indicatorSeriesService.GetByRangeDate(7);
                            resultIvvb = _indicatorSeriesService.GetByRangeDate(8);
                            resultPoupanca = _indicatorSeriesService.GetMonthRange(5);
                            resultCdi = _indicatorSeriesService.GetMonthRange(4);
                            resultIpca = _indicatorSeriesService.GetLatest(3);
                        }
                        else if (periodType == 2)
                        {
                            #region Month

                            List<IndicatorSeriesView> lstIbov = new List<IndicatorSeriesView>();
                            List<IndicatorSeriesView> lstIfix = new List<IndicatorSeriesView>();
                            List<IndicatorSeriesView> lstIdiv = new List<IndicatorSeriesView>();
                            List<IndicatorSeriesView> lstBova = new List<IndicatorSeriesView>();
                            List<IndicatorSeriesView> lstIvvb = new List<IndicatorSeriesView>();
                            List<IndicatorSeriesView> lstPoupanca = new List<IndicatorSeriesView>();
                            List<IndicatorSeriesView> lstCdi = new List<IndicatorSeriesView>();
                            List<IndicatorSeriesView> lstIpca = new List<IndicatorSeriesView>();

                            DateTime currentDate = DateTime.Now;

                            for (int i = 0; i < amountSeries; i++)
                            {
                                DateTime dateMonth = currentDate.AddMonths(-i).Date;
                                int month = currentDate.AddMonths(-i).Month;

                                lstIbov.AddRange(GetIndicatorByMonth(1, dateMonth));
                                lstIfix.AddRange(GetIndicatorByMonth(2, dateMonth));
                                lstIdiv.AddRange(GetIndicatorByMonth(6, dateMonth));
                                lstBova.AddRange(GetIndicatorByMonth(7, dateMonth));
                                lstIvvb.AddRange(GetIndicatorByMonth(8, dateMonth));

                                IndicatorSeriesView indPoupanca = _indicatorSeriesService.GetLatestByMonth(5, month).Value;
                                IndicatorSeriesView indCdi = _indicatorSeriesService.GetSumByMonth(4, month).Value;
                                IndicatorSeriesView indIpca = _indicatorSeriesService.GetLatestByMonth(3, month).Value;

                                if (indPoupanca != null)
                                {
                                    lstPoupanca.Add(indPoupanca);
                                }

                                if (indCdi != null)
                                {
                                    lstCdi.Add(indCdi);
                                }

                                if (indIpca != null)
                                {
                                    lstIpca.Add(indIpca);
                                }
                            }

                            resultIbov.Value = lstIbov;
                            resultIfix.Value = lstIfix;
                            resultIdiv.Value = lstIdiv;
                            resultBova.Value = lstBova;
                            resultIvvb.Value = lstIvvb;
                            resultPoupanca.Value = lstPoupanca;
                            resultCdi.Value = lstCdi;
                            resultIpca.Value = lstIpca;

                            #endregion
                        }
                        else if (periodType == 3)
                        {
                            List<IndicatorSeriesView> lstIbov = new List<IndicatorSeriesView>();
                            List<IndicatorSeriesView> lstIfix = new List<IndicatorSeriesView>();
                            List<IndicatorSeriesView> lstIdiv = new List<IndicatorSeriesView>();
                            List<IndicatorSeriesView> lstBova = new List<IndicatorSeriesView>();
                            List<IndicatorSeriesView> lstIvvb = new List<IndicatorSeriesView>();
                            List<IndicatorSeriesView> lstPoupanca = new List<IndicatorSeriesView>();
                            List<IndicatorSeriesView> lstCdi = new List<IndicatorSeriesView>();
                            List<IndicatorSeriesView> lstIpca = new List<IndicatorSeriesView>();

                            int year = DateTime.Now.Year;
                            int month = DateTime.Now.Month;

                            lstIbov.AddRange(GetIndicatorByYear(1, DateTime.Now));
                            lstIfix.AddRange(GetIndicatorByYear(2, DateTime.Now));
                            lstIdiv.AddRange(GetIndicatorByYear(6, DateTime.Now));
                            lstBova.AddRange(GetIndicatorByYear(7, DateTime.Now));
                            lstIvvb.AddRange(GetIndicatorByYear(8, DateTime.Now));


                            IndicatorSeriesView indPoupanca = _indicatorSeriesService.GetLatestByMonth(5, month).Value;
                            IndicatorSeriesView indCdi = _indicatorSeriesService.GetSumByYear(4, year).Value;
                            IndicatorSeriesView indIpca = _indicatorSeriesService.GetSumByYear(3, year).Value;

                            if (indPoupanca != null)
                            {
                                indPoupanca.Perc = indPoupanca.Perc * month;
                                lstPoupanca.Add(indPoupanca);
                            }

                            if (indCdi != null)
                            {
                                lstCdi.Add(indCdi);
                            }

                            if (indIpca != null)
                            {
                                lstIpca.Add(indIpca);
                            }

                            resultIbov.Value = lstIbov;
                            resultIfix.Value = lstIfix;
                            resultIdiv.Value = lstIdiv;
                            resultBova.Value = lstBova;
                            resultIvvb.Value = lstIvvb;
                            resultPoupanca.Value = lstPoupanca;
                            resultCdi.Value = lstCdi;
                            resultIpca.Value = lstIpca;
                        }


                        GetIndicatorSeries(comparativeChart, lstDataset, resultIbov, 0, periodType);
                        GetIndicatorSeries(comparativeChart, lstDataset, resultIfix, 0, periodType);
                        GetIndicatorSeries(comparativeChart, lstDataset, resultIdiv, 0, periodType);
                        GetIndicatorSeries(comparativeChart, lstDataset, resultBova, 0, periodType);
                        GetIndicatorSeries(comparativeChart, lstDataset, resultIvvb, 0, periodType);

                        lstDataset.Add(datasetTotal);


                        GetIndicatorSeries(comparativeChart, lstDataset, resultPoupanca, 1, periodType);
                        GetIndicatorSeries(comparativeChart, lstDataset, resultCdi, 1, periodType);
                        GetIndicatorSeries(comparativeChart, lstDataset, resultIpca, 1, periodType);


                        PortfolioPerformance lastPortfolioPerformance = lstPortfolioPerformance.LastOrDefault();

                        if (lastPortfolioPerformance != null)
                        {
                            totalMarket = lastPortfolioPerformance.TotalMarket;
                        }

                        comparativeChart.TotalMarket = totalMarket.ToString("n2", new CultureInfo("pt-br"));
                        comparativeChart.LastUpdated = DateTime.Now.ToString("HH:mm:ss");
                        resultServiceObject.Value = comparativeChart;
                    }
                }
            }

            resultServiceObject.Success = true;

            return resultServiceObject;
        }

        private List<IndicatorSeriesView> GetIndicatorByMonth(int idIndicatorType, DateTime dateMonth)
        {
            List<IndicatorSeriesView> lstIndicator = new List<IndicatorSeriesView>();

            ResultServiceObject<IndicatorSeriesView> resultIndicatorSeriesViewFirst = _indicatorSeriesService.GetByMonth(idIndicatorType, dateMonth, true);
            ResultServiceObject<IndicatorSeriesView> resultIndicatorSeriesViewLast = _indicatorSeriesService.GetByMonth(idIndicatorType, dateMonth, false);

            if (resultIndicatorSeriesViewFirst.Success && resultIndicatorSeriesViewLast.Success && resultIndicatorSeriesViewFirst.Value != null && resultIndicatorSeriesViewLast.Value != null)
            {
                IndicatorSeriesView indicatorSeriesViewMonth = GetIndicatorSeriesByPeriod(resultIndicatorSeriesViewFirst, resultIndicatorSeriesViewLast);
                lstIndicator.Add(indicatorSeriesViewMonth);
            }

            return lstIndicator;
        }

        private List<IndicatorSeriesView> GetIndicatorByYear(int idIndicatorType, DateTime dateYear)
        {
            List<IndicatorSeriesView> lstIndicator = new List<IndicatorSeriesView>();

            ResultServiceObject<IndicatorSeriesView> resultIndicatorSeriesViewFirst = _indicatorSeriesService.GetByYear(idIndicatorType, dateYear, true);
            ResultServiceObject<IndicatorSeriesView> resultIndicatorSeriesViewLast = _indicatorSeriesService.GetByYear(idIndicatorType, dateYear, false);

            if (resultIndicatorSeriesViewFirst.Success && resultIndicatorSeriesViewLast.Success && resultIndicatorSeriesViewFirst.Value != null && resultIndicatorSeriesViewLast.Value != null)
            {
                IndicatorSeriesView indicatorSeriesViewMonth = GetIndicatorSeriesByPeriod(resultIndicatorSeriesViewFirst, resultIndicatorSeriesViewLast);
                lstIndicator.Add(indicatorSeriesViewMonth);
            }

            return lstIndicator;
        }

        private static PortfolioPerformance GetByPeriod(ResultServiceObject<PortfolioPerformance> resultPortfolioFirst, ResultServiceObject<PortfolioPerformance> resultPortfolioLast)
        {
            PortfolioPerformance portfolioPerformance = new PortfolioPerformance();

            PortfolioPerformance portfolioPerformanceFirst = resultPortfolioFirst.Value;
            PortfolioPerformance portfolioPerformanceLast = resultPortfolioLast.Value;

            if (portfolioPerformanceFirst.IdPortfolioPerformance != portfolioPerformanceLast.IdPortfolioPerformance)
            {
                portfolioPerformance.CalculationDate = portfolioPerformanceLast.CalculationDate;
                portfolioPerformance.TotalMarket = portfolioPerformanceLast.TotalMarket;

                portfolioPerformance.NetValue = portfolioPerformanceLast.TotalMarket - portfolioPerformanceFirst.Total;

                //if (portfolioPerformanceFirst.Total != 0)
                //{
                //    portfolioPerformance.PerformancePerc = (portfolioPerformanceLast.TotalMarket / portfolioPerformanceFirst.Total) - 1;
                //}

                //if (portfolioPerformanceFirst.NetValue != 0)
                //{
                //    portfolioPerformance.PerformancePercTWR = (portfolioPerformanceLast.NetValue / portfolioPerformanceFirst.NetValue) - 1;
                //}

                portfolioPerformance.NetValueTWR = portfolioPerformanceLast.NetValue - portfolioPerformanceFirst.NetValue;

                if (portfolioPerformanceFirst.TotalMarket != 0)
                {
                    portfolioPerformance.PerformancePercTWR = (portfolioPerformance.NetValueTWR.Value / portfolioPerformanceFirst.TotalMarket);
                }

                if (portfolioPerformanceFirst.Total != 0)
                {
                    portfolioPerformance.PerformancePerc = portfolioPerformance.NetValue / portfolioPerformanceFirst.Total;
                }
            }
            else
            {
                portfolioPerformance.TotalMarket = portfolioPerformanceFirst.TotalMarket;
                portfolioPerformance.NetValue = portfolioPerformanceFirst.NetValue;
                portfolioPerformance.PerformancePercTWR = 0;
                portfolioPerformance.PerformancePerc = portfolioPerformanceFirst.PerformancePerc;
                portfolioPerformance.CalculationDate = portfolioPerformanceFirst.CalculationDate;
            }

            return portfolioPerformance;
        }

        private static IndicatorSeriesView GetIndicatorSeriesByPeriod(ResultServiceObject<IndicatorSeriesView> resultIndicatorFirst, ResultServiceObject<IndicatorSeriesView> resultIndicatorLast)
        {
            IndicatorSeriesView indicatorSeriesView = new IndicatorSeriesView();

            IndicatorSeriesView indicatorSeriesViewFirst = resultIndicatorFirst.Value;
            IndicatorSeriesView indicatorSeriesViewLast = resultIndicatorLast.Value;

            if (indicatorSeriesViewFirst.IdIndicatorSeries != indicatorSeriesViewLast.IdIndicatorSeries)
            {
                indicatorSeriesView.Name = indicatorSeriesViewFirst.Name;
                indicatorSeriesView.CalculationDate = indicatorSeriesViewLast.CalculationDate;

                if (indicatorSeriesViewFirst.Points != 0)
                {
                    indicatorSeriesView.Perc = (indicatorSeriesViewLast.Points / indicatorSeriesViewFirst.Points) - 1;
                }
            }
            else
            {
                indicatorSeriesView.Name = indicatorSeriesViewFirst.Name;
                indicatorSeriesView.CalculationDate = indicatorSeriesViewFirst.CalculationDate;
                indicatorSeriesView.Points = indicatorSeriesViewFirst.Points;
                indicatorSeriesView.Perc = indicatorSeriesViewFirst.Perc;
            }

            return indicatorSeriesView;
        }

        private static void GetIndicatorSeries(ComparativeChart performanceStockChart, List<DatasetComp> lstDataset, ResultServiceObject<IEnumerable<IndicatorSeriesView>> resultIndicator, int hidden, int periodType)
        {
            if (resultIndicator.Success)
            {
                if (resultIndicator.Value != null && resultIndicator.Value.Count() > 0)
                {
                    List<IndicatorSeriesView> indicatorSeries = resultIndicator.Value.ToList();

                    DatasetComp dataset = new DatasetComp();
                    dataset.SeriesName = indicatorSeries[0].Name;
                    dataset.Hidden = hidden;
                    dataset.ListData = new List<DataComp>();
                    lstDataset.Add(dataset);

                    foreach (CategegoryComp category in performanceStockChart.ListCategoryObj[0].ListCategory)
                    {
                        if (indicatorSeries[0].IdIndicatorType == 3)
                        {
                            DataComp data = new DataComp();
                            decimal perc = indicatorSeries[0].Perc * 100;
                            data.Value = perc.ToString("n2", new CultureInfo("en-us")).Replace(",", string.Empty);
                            dataset.ListData.Add(data);
                        }
                        else
                        {
                            IndicatorSeriesView indicatorSerie = null;

                            if (periodType == 1)
                            {
                                indicatorSerie = indicatorSeries.FirstOrDefault(ind => ind.CalculationDate.Date == category.CalculationDate.Date);
                            }
                            else if (periodType == 2)
                            {
                                indicatorSerie = indicatorSeries.FirstOrDefault(ind => ind.CalculationDate.Date.Month == category.CalculationDate.Date.Month);
                            }
                            else if (periodType == 3)
                            {
                                indicatorSerie = indicatorSeries.FirstOrDefault(ind => ind.CalculationDate.Date.Year == category.CalculationDate.Date.Year);
                            }


                            if (indicatorSerie == null)
                            {
                                DataComp data = new DataComp();
                                decimal perc = 0;
                                data.Value = perc.ToString("n2", new CultureInfo("en-us")).Replace(",", string.Empty);
                                dataset.ListData.Add(data);
                            }
                            else
                            {
                                DataComp data = new DataComp();
                                decimal perc = indicatorSerie.Perc * 100;
                                data.Value = perc.ToString("n2", new CultureInfo("en-us")).Replace(",", string.Empty);
                                dataset.ListData.Add(data);
                            }
                        }
                    }
                }
            }
        }

        public ResultResponseObject<ComparativeContainerChart> GetComparativeContainerChart(Guid guidPortfolioSub)
        {
            ComparativeContainerChart comparativeContainerChart = new ComparativeContainerChart();
            ResultResponseObject<ComparativeContainerChart> resultServiceObject = new ResultResponseObject<ComparativeContainerChart>();
            List<ChartLabelValue> lstChartLabelValue = new List<ChartLabelValue>();
            comparativeContainerChart.ListChartLabelValue = lstChartLabelValue;

            using (_uow.Create())
            {
                ResultServiceObject<Portfolio> resultPortfolio = _portfolioService.GetByGuid(guidPortfolioSub);
                ResultServiceObject<SubPortfolio> resultSubPortfolio = _subPortfolioService.GetByGuid(guidPortfolioSub);

                if (resultSubPortfolio.Success && resultPortfolio.Success)
                {
                    Portfolio portfolio = resultPortfolio.Value;
                    SubPortfolio subportfolio = resultSubPortfolio.Value;
                    string portfolioName = string.Empty;

                    long idPortfolio = 0;
                    bool isSub = false;

                    if (portfolio != null)
                    {
                        idPortfolio = portfolio.IdPortfolio;
                        portfolioName = portfolio.Name;
                        comparativeContainerChart.IdCountry = portfolio.IdCountry;
                    }
                    else if (subportfolio != null)
                    {
                        idPortfolio = subportfolio.IdPortfolio;
                        portfolioName = subportfolio.Name;
                        isSub = true;

                        resultPortfolio = _portfolioService.GetById(subportfolio.IdPortfolio);

                        if (resultPortfolio.Value != null)
                        {
                            comparativeContainerChart.IdCountry = resultPortfolio.Value.IdCountry;
                        }
                    }

                    ResultServiceObject<PortfolioPerformance> resultPortfolioPerformance = _portfolioPerformanceService.GetByCalculationDate(idPortfolio);
                    PortfolioPerformance portfolioPerformance = null;

                    if (resultPortfolioPerformance.Success)
                    {
                        portfolioPerformance = resultPortfolioPerformance.Value;

                        if (portfolioPerformance != null)
                        {
                            if (isSub)
                            {
                                portfolioPerformance = _portfolioService.CalculateSubPortfolioPerformance(portfolioPerformance, subportfolio.IdSubPortfolio, _portfolioPerformanceService, _performanceStockService, _operationService, _subPortfolioOperationService);
                            }

                            decimal perc = portfolioPerformance.PerformancePercTWR * 100;
                            ChartLabelValue chartLabelValue = new ChartLabelValue();
                            chartLabelValue.Label = portfolioName;
                            chartLabelValue.Value = perc.ToString("n2", new CultureInfo("en-us")).Replace(",", string.Empty);
                            lstChartLabelValue.Add(chartLabelValue);

                            if (comparativeContainerChart.IdCountry == (int)CountryEnum.Brazil)
                            {
                                ResultServiceObject<IndicatorSeriesView> resultIbov = _indicatorSeriesService.GetByCalculationDateView((int)IndicatorTypeEnum.IBovespa, portfolioPerformance.CalculationDate.Date, 1);
                                ResultServiceObject<IndicatorSeriesView> resultIfix = _indicatorSeriesService.GetByCalculationDateView((int)IndicatorTypeEnum.IFIX, portfolioPerformance.CalculationDate.Date, 1);
                                ResultServiceObject<IndicatorSeriesView> resultIdiv = _indicatorSeriesService.GetByCalculationDateView((int)IndicatorTypeEnum.IDIV, portfolioPerformance.CalculationDate.Date, 1);
                                ResultServiceObject<IndicatorSeriesView> resultBova = _indicatorSeriesService.GetByCalculationDateView((int)IndicatorTypeEnum.BOVA11, portfolioPerformance.CalculationDate.Date, 1);
                                ResultServiceObject<IndicatorSeriesView> resultIvvb = _indicatorSeriesService.GetByCalculationDateView((int)IndicatorTypeEnum.IVVB11, portfolioPerformance.CalculationDate.Date, 1);
                                ResultServiceObject<IndicatorSeriesView> resultPoupanca = _indicatorSeriesService.GetByCalculationDateView((int)IndicatorTypeEnum.Poupanca, portfolioPerformance.CalculationDate.Date, 1);
                                ResultServiceObject<IndicatorSeriesView> resultCdi = _indicatorSeriesService.GetByCalculationDateView((int)IndicatorTypeEnum.CDI, portfolioPerformance.CalculationDate.Date, 1);
                                ResultServiceObject<IEnumerable<IndicatorSeriesView>> resultIpca = _indicatorSeriesService.GetLatest(3);

                                CreateIndicatorLabel(lstChartLabelValue, resultIbov.Value);
                                CreateIndicatorLabel(lstChartLabelValue, resultIfix.Value);
                                CreateIndicatorLabel(lstChartLabelValue, resultIdiv.Value);
                                CreateIndicatorLabel(lstChartLabelValue, resultBova.Value);
                                CreateIndicatorLabel(lstChartLabelValue, resultIvvb.Value);
                                CreateIndicatorLabel(lstChartLabelValue, resultPoupanca.Value);
                                CreateIndicatorLabel(lstChartLabelValue, resultCdi.Value);
                                CreateIndicatorLabel(lstChartLabelValue, resultIpca.Value.FirstOrDefault());
                            }
                            else
                            {
                                ResultServiceObject<IndicatorSeriesView> resultDowJones = _indicatorSeriesService.GetByCalculationDateView((int)IndicatorTypeEnum.DowJones, portfolioPerformance.CalculationDate.Date, 1);
                                ResultServiceObject<IndicatorSeriesView> resultSP500 = _indicatorSeriesService.GetByCalculationDateView((int)IndicatorTypeEnum.SP500, portfolioPerformance.CalculationDate.Date, 1);
                                ResultServiceObject<IndicatorSeriesView> resultNasdaq = _indicatorSeriesService.GetByCalculationDateView((int)IndicatorTypeEnum.Nasdaq, portfolioPerformance.CalculationDate.Date, 1);

                                CreateIndicatorLabel(lstChartLabelValue, resultDowJones.Value);
                                CreateIndicatorLabel(lstChartLabelValue, resultSP500.Value);
                                CreateIndicatorLabel(lstChartLabelValue, resultNasdaq.Value);
                            }

                            comparativeContainerChart.TotalMarket = portfolioPerformance.TotalMarket.ToString("n2", new CultureInfo("pt-br"));
                            comparativeContainerChart.LastUpdated = DateTime.Now.ToString("HH:mm:ss");
                            resultServiceObject.Value = comparativeContainerChart;
                        }
                    }
                }
            }

            resultServiceObject.Success = true;

            return resultServiceObject;
        }

        private static void CreateIndicatorLabel(List<ChartLabelValue> lstChartLabelValue, IndicatorSeriesView indicatorSeriesView)
        {
            if (indicatorSeriesView != null)
            {
                decimal perc = indicatorSeriesView.Perc * 100;
                ChartLabelValue chartLabelValue = new ChartLabelValue();
                chartLabelValue.Label = indicatorSeriesView.Name;
                chartLabelValue.Value = perc.ToString("n2", new CultureInfo("en-us")).Replace(",", string.Empty);
                lstChartLabelValue.Add(chartLabelValue);
            }
        }

        public ResultResponseObject<IEnumerable<OperationBasicVM>> GetPortfolioContentSimple(Guid idportfolio)
        {
            ResultResponseObject<IEnumerable<OperationBasicVM>> resultReponse;

            using (_uow.Create())
            {
                ResultServiceObject<Portfolio> resultPortfolioService = _portfolioService.GetByGuid(idportfolio);
                ResultServiceObject<IEnumerable<OperationView>> resultOperationService = _operationService.GetByIdPortfolio(resultPortfolioService.Value.IdPortfolio);

                resultReponse = _mapper.Map<ResultResponseObject<IEnumerable<OperationBasicVM>>>(resultOperationService);
            }



            return resultReponse;
        }

        public ResultResponseObject<IEnumerable<OperationBasicVM>> GetSubPortfolioContentSimple(Guid idportfolio, Guid idsubportfolio)
        {
            ResultResponseObject<IEnumerable<OperationBasicVM>> resultReponse;

            using (_uow.Create())
            {
                ResultServiceObject<SubPortfolio> resultPortfolioService = _subPortfolioService.GetByGuid(idsubportfolio);
                ResultServiceObject<IEnumerable<OperationView>> resultOperationService = _operationService.GetByIdSubPortfolio(resultPortfolioService.Value.IdSubPortfolio);

                resultReponse = _mapper.Map<ResultResponseObject<IEnumerable<OperationBasicVM>>>(resultOperationService);
            }

            return resultReponse;
        }

        public ResultResponseObject<API.Model.Response.v3.PortfolioViewGroupedVM> GetPortfolioViewWrapperV3()
        {
            ResultResponseObject<API.Model.Response.v3.PortfolioViewGroupedVM> resultServiceObject = new ResultResponseObject<API.Model.Response.v3.PortfolioViewGroupedVM>();

            PortfolioViewGroupedVM portfolioViewGroupedVM = new PortfolioViewGroupedVM();

            List<API.Model.Response.v3.PortfolioViewWrapperVM> portfolioViewWrapperVMs = new List<API.Model.Response.v3.PortfolioViewWrapperVM>();

            using (_uow.Create())
            {
                //Get all portfolios and subportfolio by logged user
                ResultServiceObject<IEnumerable<PortfolioView>> result = _portfolioService.GetByUser(_globalAuthenticationService.IdUser);

                ResultServiceObject<Subscription> subscription = _subscriptionService.GetByUser(_globalAuthenticationService.IdUser);
                bool hasSubscription = false;

                if (subscription.Value != null && subscription.Value.Active && (subscription.Value.SubscriptionTypeID.Equals((int)SubscriptionTypeEnum.Gold) ||
                                                                                subscription.Value.SubscriptionTypeID.Equals((int)SubscriptionTypeEnum.Annuity)))
                {
                    hasSubscription = true;
                }

                if (result.Success && result.Value != null && result.Value.Count() > 0)
                {
                    foreach (PortfolioView portfolioView in result.Value)
                    {
                        if (portfolioView.IdCountry.Equals((int)CountryEnum.EUA) && !hasSubscription)
                        {
                            continue;
                        }


                        if (portfolioView.IsPortfolio)
                        {
                            API.Model.Response.v3.PortfolioViewWrapperVM portfolioViewWrapperVM = new API.Model.Response.v3.PortfolioViewWrapperVM();

                            portfolioViewWrapperVM.PrincipalPortfolio = PortfolioParseToReturn(portfolioView);

                            List<PortfolioView> subPortfolioViews = result.Value.Where(item => item.IsPortfolio == false && item.ParentPortfolio != null && item.ParentPortfolio.Equals(portfolioView.GuidPortfolioSubPortfolio)).ToList();
                            List<PortfolioViewVM> portfolioViewVMs = new List<PortfolioViewVM>();

                            foreach (PortfolioView itemSubPortfolioView in subPortfolioViews)
                            {
                                portfolioViewVMs.Add(PortfolioParseToReturn(itemSubPortfolioView));
                            }

                            portfolioViewWrapperVM.SubPortfolios = portfolioViewVMs;


                            portfolioViewWrapperVMs.Add(portfolioViewWrapperVM);
                        }
                    }

                    portfolioViewGroupedVM.PortfolioGroupedViews = portfolioViewWrapperVMs;
                }


                ResultServiceObject<IEnumerable<PortfolioStatementView>> resultZero = new ResultServiceObject<IEnumerable<PortfolioStatementView>>();

                resultZero = _portfolioService.GetZeroPriceByUser(_globalAuthenticationService.IdUser);

                if (resultZero.Success && resultZero.Value != null && resultZero.Value.Count() > 0)
                {
                    portfolioViewGroupedVM.ZeroQuantity = resultZero.Value.Count();
                }
            }

            resultServiceObject.Value = portfolioViewGroupedVM;
            resultServiceObject.Success = true;

            return resultServiceObject;
        }

        public ResultResponseObject<API.Model.Response.v4.PortfolioViewWrapperVM> GetPortfolioViewWrapperV4()
        {
            API.Model.Response.v2.PortfolioViewWrapperVM portfolioViewWrapperVM = this.GetPortfolioViewWrapperBase();

            API.Model.Response.v4.PortfolioViewWrapperVM portfolioViewModelv4 = _mapper.Map<API.Model.Response.v4.PortfolioViewWrapperVM>(portfolioViewWrapperVM);

            foreach (API.Model.Response.v4.PortfolioViewVM itemPortfolio in portfolioViewModelv4.PortfolioViews)
            {
                if (itemPortfolio.IsPortfolio)
                {
                    using (_uow.Create())
                    {
                        itemPortfolio.HasSubscription = _portfolioService.HasSubscription(itemPortfolio.GuidPortfolioSubPortfolio).Value;
                    }
                }
            }

            using (_uow.Create())
            {
                bool hasStockSplit = false;

                DateTime limitDate = new DateTime(2021, 11, 10);

                string resultFromCache = _cacheService.GetFromCache(string.Concat("HasStockSplit:", _globalAuthenticationService.IdUser));

                if (resultFromCache == null)
                {
                    //hasStockSplit = _stockSplitService.HasStockSplit(_globalAuthenticationService.IdUser, limitDate);
                    hasStockSplit = false;
                    _cacheService.SaveOnCache(string.Concat("HasStockSplit:", _globalAuthenticationService.IdUser), TimeSpan.FromHours(24), hasStockSplit.ToString());
                }
                else
                {
                    hasStockSplit = bool.Parse(resultFromCache);
                }

                portfolioViewModelv4.HasStockSplit = hasStockSplit;
            }

            ResultResponseObject<API.Model.Response.v4.PortfolioViewWrapperVM> resultApi = new ResultResponseObject<API.Model.Response.v4.PortfolioViewWrapperVM>();

            resultApi.Success = true;
            resultApi.Value = portfolioViewModelv4;

            return resultApi;
        }

        public ResultResponseObject<API.Model.Response.v2.PortfolioViewWrapperVM> GetPortfolioViewWrapperV2()
        {
            API.Model.Response.v2.PortfolioViewWrapperVM portfolioViewWrapperVM = this.GetPortfolioViewWrapperBase();
            ResultResponseObject<API.Model.Response.v2.PortfolioViewWrapperVM> resultApi = new ResultResponseObject<API.Model.Response.v2.PortfolioViewWrapperVM>();

            resultApi.Success = true;
            resultApi.Value = portfolioViewWrapperVM;

            return resultApi;
        }

        public API.Model.Response.v2.PortfolioViewWrapperVM GetPortfolioViewWrapperBase()
        {
            List<API.Model.Response.v2.PortfolioViewVM> resultModel = new List<API.Model.Response.v2.PortfolioViewVM>();
            API.Model.Response.v2.PortfolioViewWrapperVM portfolioViewWrapperVM = new API.Model.Response.v2.PortfolioViewWrapperVM();

            using (_uow.Create())
            {
                ResultServiceObject<IEnumerable<PortfolioView>> result = _portfolioService.GetByUser(_globalAuthenticationService.IdUser);
                ResultServiceObject<IEnumerable<PortfolioStatementView>> resultZero = new ResultServiceObject<IEnumerable<PortfolioStatementView>>();
                ResultServiceObject<Subscription> subscription = _subscriptionService.GetByUser(_globalAuthenticationService.IdUser);
                bool hasSubscription = false;

                if (subscription.Value != null && subscription.Value.Active && (subscription.Value.SubscriptionTypeID.Equals((int)SubscriptionTypeEnum.Gold) ||
                                                                                subscription.Value.SubscriptionTypeID.Equals((int)SubscriptionTypeEnum.Annuity)))
                {
                    hasSubscription = true;
                }


                if (result.Success && result.Value != null && result.Value.Count() > 0)
                {
                    foreach (PortfolioView portfolioView in result.Value)
                    {
                        if (portfolioView.IdCountry.Equals((int)CountryEnum.EUA) && !hasSubscription)
                        {
                            continue;
                        }

                        Dividendos.API.Model.Response.v2.PortfolioViewVM portfolioViewModel = PortfolioParseToReturn(portfolioView);

                        resultModel.Add(portfolioViewModel);
                    }

                    portfolioViewWrapperVM.PortfolioViews = resultModel;
                }

                resultZero = _portfolioService.GetZeroPriceByUser(_globalAuthenticationService.IdUser);

                if (resultZero.Success && resultZero.Value != null && resultZero.Value.Count() > 0)
                {
                    portfolioViewWrapperVM.ZeroQuantity = resultZero.Value.Count();
                }
            }

            return portfolioViewWrapperVM;
        }
        private Dividendos.API.Model.Response.v2.PortfolioViewVM PortfolioParseToReturn(PortfolioView portfolioView)
        {
            decimal perc = portfolioView.PerformancePerc * 100;
            decimal percTwr = portfolioView.PerformancePercTWR * 100;

            Dividendos.API.Model.Response.v2.PortfolioViewVM portfolioViewModel = new Dividendos.API.Model.Response.v2.PortfolioViewVM();
            portfolioViewModel.Name = portfolioView.Name;
            portfolioViewModel.CalculationDate = portfolioView.CalculationDate.ToString("dd/MM");
            portfolioViewModel.GuidPortfolioSubPortfolio = portfolioView.GuidPortfolioSubPortfolio;
            portfolioViewModel.LatestNetValue = GetSignal(portfolioView.LatestNetValue) + portfolioView.LatestNetValue.ToString("n2", new CultureInfo("pt-br"));
            portfolioViewModel.NetValue = GetSignal(portfolioView.NetValue) + portfolioView.NetValue.ToString("n2", new CultureInfo("pt-br"));
            portfolioViewModel.Profit = GetSignal(portfolioView.Profit) + portfolioView.Profit.ToString("n2", new CultureInfo("pt-br"));
            portfolioViewModel.TotalDividend = GetSignal(portfolioView.TotalDividend) + portfolioView.TotalDividend.ToString("n2", new CultureInfo("pt-br"));
            portfolioViewModel.TotalMarket = portfolioView.TotalMarket.ToString("n2", new CultureInfo("pt-br"));
            portfolioViewModel.LastUpdated = DateTime.Now.ToString("HH:mm:ss");

            portfolioViewModel.Total = portfolioView.Total.ToString("n2", new CultureInfo("pt-br"));
            portfolioViewModel.PerformancePerc = GetSignal(perc) + perc.ToString("n2", new CultureInfo("pt-br"));
            portfolioViewModel.PreviousNetValue = GetSignal(portfolioView.PreviousNetValue) + portfolioView.PreviousNetValue.ToString("n2", new CultureInfo("pt-br"));
            portfolioViewModel.PerformancePercTWR = GetSignal(percTwr) + percTwr.ToString("n2", new CultureInfo("pt-br"));
            portfolioViewModel.IdCountry = portfolioView.IdCountry;
            portfolioViewModel.IsPortfolio = portfolioView.IsPortfolio;

            return portfolioViewModel;
        }

        public ResultResponseObject<IEnumerable<PortfolioViewModel>> GetPortfolioView()
        {
            ResultResponseObject<IEnumerable<PortfolioViewModel>> resultServiceObject = new ResultResponseObject<IEnumerable<PortfolioViewModel>>();
            List<PortfolioViewModel> resultModel = new List<PortfolioViewModel>();

            using (_uow.Create())
            {
                ResultServiceObject<IEnumerable<PortfolioView>> result = _portfolioService.GetByUser(_globalAuthenticationService.IdUser);

                ResultServiceObject<Subscription> subscription = _subscriptionService.GetByUser(_globalAuthenticationService.IdUser);
                bool hasSubscription = false;

                if (subscription.Value != null && subscription.Value.Active && (subscription.Value.SubscriptionTypeID.Equals((int)SubscriptionTypeEnum.Gold) ||
                                                                                subscription.Value.SubscriptionTypeID.Equals((int)SubscriptionTypeEnum.Annuity)))
                {
                    hasSubscription = true;
                }

                if (result.Success && result.Value != null && result.Value.Count() > 0)
                {
                    foreach (PortfolioView portfolioView in result.Value)
                    {
                        if (portfolioView.IdCountry.Equals((int)CountryEnum.EUA) && !hasSubscription)
                        {
                            continue;
                        }

                        decimal perc = portfolioView.PerformancePerc * 100;
                        decimal percTwr = portfolioView.PerformancePercTWR * 100;

                        PortfolioViewModel portfolioViewModel = new PortfolioViewModel();
                        portfolioViewModel.Name = portfolioView.Name;
                        portfolioViewModel.CalculationDate = portfolioView.CalculationDate.ToString("dd/MM");
                        portfolioViewModel.GuidPortfolioSubPortfolio = portfolioView.GuidPortfolioSubPortfolio;
                        portfolioViewModel.LatestNetValue = GetSignal(portfolioView.LatestNetValue) + portfolioView.LatestNetValue.ToString("n2", new CultureInfo("pt-br"));
                        portfolioViewModel.NetValue = GetSignal(portfolioView.NetValue) + portfolioView.NetValue.ToString("n2", new CultureInfo("pt-br"));
                        portfolioViewModel.Profit = GetSignal(portfolioView.Profit) + portfolioView.Profit.ToString("n2", new CultureInfo("pt-br"));
                        portfolioViewModel.TotalDividend = GetSignal(portfolioView.TotalDividend) + portfolioView.TotalDividend.ToString("n2", new CultureInfo("pt-br"));
                        portfolioViewModel.TotalMarket = portfolioView.TotalMarket.ToString("n2", new CultureInfo("pt-br"));
                        portfolioViewModel.LastUpdated = DateTime.Now.ToString("HH:mm:ss");

                        portfolioViewModel.Total = portfolioView.Total.ToString("n2", new CultureInfo("pt-br"));
                        portfolioViewModel.PerformancePerc = GetSignal(perc) + perc.ToString("n2", new CultureInfo("pt-br"));
                        portfolioViewModel.PreviousNetValue = GetSignal(portfolioView.PreviousNetValue) + portfolioView.PreviousNetValue.ToString("n2", new CultureInfo("pt-br"));
                        portfolioViewModel.PerformancePercTWR = GetSignal(percTwr) + percTwr.ToString("n2", new CultureInfo("pt-br"));

                        resultModel.Add(portfolioViewModel);
                    }
                }
            }

            resultServiceObject.Value = resultModel;
            resultServiceObject.Success = true;

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

        public ResultResponseObject<IEnumerable<Dividendos.API.Model.Response.v4.PortfolioStatementViewModel>> GetPortfolioStatementViewV4(Guid guidPortfolioSub, API.Model.Request.Stock.StockType stockType)
        {
            ResultResponseObject<IEnumerable<PortfolioStatementViewModel>> resultResponseObject = this.GetPortfolioStatementView(guidPortfolioSub, stockType);
            ResultResponseObject<IEnumerable<API.Model.Response.v4.PortfolioStatementViewModel>> result = _mapper.Map<ResultResponseObject<IEnumerable<API.Model.Response.v4.PortfolioStatementViewModel>>>(resultResponseObject);

            return result;
        }

        public ResultResponseObject<IEnumerable<PortfolioStatementViewModel>> GetPortfolioStatementView(Guid guidPortfolioSub, API.Model.Request.Stock.StockType stockType)
        {
            int? idStockType = null;

            if (stockType != API.Model.Request.Stock.StockType.None)
            {
                idStockType = (int)stockType;
            }

            ResultResponseObject<IEnumerable<PortfolioStatementViewModel>> resultServiceObject = new ResultResponseObject<IEnumerable<PortfolioStatementViewModel>>();
            List<PortfolioStatementViewModel> resultModel = new List<PortfolioStatementViewModel>();

            using (_uow.Create())
            {
                ResultServiceObject<Portfolio> resultPortfolio = _portfolioService.GetByGuid(guidPortfolioSub);
                ResultServiceObject<SubPortfolio> resultSubPortfolio = _subPortfolioService.GetByGuid(guidPortfolioSub);

                if (resultSubPortfolio.Success && resultPortfolio.Success)
                {
                    ResultServiceObject<IEnumerable<PortfolioStatementView>> result = new ResultServiceObject<IEnumerable<PortfolioStatementView>>();
                    Portfolio portfolio = resultPortfolio.Value;
                    SubPortfolio subportfolio = resultSubPortfolio.Value;

                    if (portfolio != null)
                    {
                        result = _portfolioService.GetByPortfolio(portfolio.GuidPortfolio, idStockType);
                    }
                    else if (subportfolio != null)
                    {
                        result = _portfolioService.GetBySubportfolio(subportfolio.GuidSubPortfolio, idStockType);
                    }

                    if (result.Success && result.Value != null && result.Value.Count() > 0)
                    {
                        foreach (PortfolioStatementView portfolioView in result.Value)
                        {
                            decimal perc = portfolioView.PerformancePerc * 100;
                            PortfolioStatementViewModel portfolioViewModel = new PortfolioStatementViewModel();
                            portfolioViewModel.IdStock = portfolioView.IdStock;
                            portfolioViewModel.GuidOperation = portfolioView.GuidOperation;
                            portfolioViewModel.Company = portfolioView.Company;
                            portfolioViewModel.Segment = portfolioView.Segment;
                            portfolioViewModel.Symbol = portfolioView.Symbol;
                            portfolioViewModel.Logo = portfolioView.Logo;
                            portfolioViewModel.LogoURL = portfolioView.LogoURL;
                            portfolioViewModel.PerformancePerc = GetSignal(perc) + perc.ToString("n2", new CultureInfo("pt-br"));
                            portfolioViewModel.AveragePrice = portfolioView.AveragePrice.ToString("n2", new CultureInfo("pt-br"));
                            portfolioViewModel.MarketPrice = portfolioView.MarketPrice.ToString("n2", new CultureInfo("pt-br"));
                            portfolioViewModel.NetValue = GetSignal(portfolioView.NetValue) + portfolioView.NetValue.ToString("n2", new CultureInfo("pt-br"));
                            portfolioViewModel.TotalDividends = portfolioView.TotalDividends.ToString("n2", new CultureInfo("pt-br"));
                            portfolioViewModel.NumberOfShares = portfolioView.NumberOfShares.ToString("0.#############################", new CultureInfo("pt-br"));
                            portfolioViewModel.TotalMarket = portfolioView.TotalMarket.ToString("n2", new CultureInfo("pt-br"));
                            portfolioViewModel.Total = portfolioView.Total.ToString("n2", new CultureInfo("pt-br"));
                            portfolioViewModel.PerformancePercN = perc;
                            portfolioViewModel.AveragePriceN = portfolioView.AveragePrice;
                            portfolioViewModel.MarketPriceN = portfolioView.MarketPrice;
                            portfolioViewModel.NetValueN = portfolioView.NetValue;
                            portfolioViewModel.TotalDividendsN = portfolioView.TotalDividends;
                            portfolioViewModel.NumberOfSharesN = portfolioView.NumberOfShares;
                            portfolioViewModel.TotalMarketN = portfolioView.TotalMarket;
                            portfolioViewModel.TotalN = portfolioView.Total;
                            portfolioViewModel.LastUpdated = DateTime.Now.ToString("HH:mm:ss");

                            resultModel.Add(portfolioViewModel);
                        }
                    }

                }
            }

            resultServiceObject.Value = resultModel;
            resultServiceObject.Success = true;

            return resultServiceObject;
        }

        public ResultResponseObject<API.Model.Response.v3.PortfolioStatementWrapperVM> GetPortfolioStatementViewWrapperV3(Guid guidPortfolioSub)
        {
            ResultResponseObject<API.Model.Response.v2.PortfolioStatementWrapperVM> resultResponseObject = this.GetPortfolioStatementViewWrapper(guidPortfolioSub);
            ResultResponseObject<API.Model.Response.v3.PortfolioStatementWrapperVM> result = _mapper.Map<ResultResponseObject<API.Model.Response.v3.PortfolioStatementWrapperVM>>(resultResponseObject);

            return result;
        }

        public ResultResponseObject<API.Model.Response.v2.PortfolioStatementWrapperVM> GetPortfolioStatementViewWrapper(Guid guidPortfolioSub)
        {
            API.Model.Response.v2.PortfolioStatementWrapperVM portfolioStatementWrapperVM = null;
            ResultResponseObject<API.Model.Response.v2.PortfolioStatementWrapperVM> resultServiceObject = new ResultResponseObject<API.Model.Response.v2.PortfolioStatementWrapperVM>();

            using (_uow.Create())
            {
                ResultServiceObject<Portfolio> resultPortfolio = _portfolioService.GetByGuid(guidPortfolioSub);
                ResultServiceObject<SubPortfolio> resultSubPortfolio = _subPortfolioService.GetByGuid(guidPortfolioSub);
                Trader trader = null;

                if (resultSubPortfolio.Success && resultPortfolio.Success)
                {
                    portfolioStatementWrapperVM = new API.Model.Response.v2.PortfolioStatementWrapperVM();

                    if (resultPortfolio.Value == null)
                    {
                        ResultServiceObject<Portfolio> resultPort = _portfolioService.GetById(resultSubPortfolio.Value.IdPortfolio);

                        if (resultPort.Success)
                        {
                            trader = _traderService.GetById(resultPort.Value.IdTrader).Value;

                            if (trader != null)
                            {
                                portfolioStatementWrapperVM.LastSyncDate = trader.LastSync;
                            }
                        }
                    }
                    else
                    {
                        trader = _traderService.GetById(resultPortfolio.Value.IdTrader).Value;

                        if (trader != null)
                        {
                            portfolioStatementWrapperVM.LastSyncDate = trader.LastSync;
                        }
                    }

                    List<API.Model.Response.v2.StockStatementVM> resultModel = new List<API.Model.Response.v2.StockStatementVM>();
                    ResultServiceObject<IEnumerable<PortfolioStatementView>> result = new ResultServiceObject<IEnumerable<PortfolioStatementView>>();
                    Portfolio portfolio = resultPortfolio.Value;
                    SubPortfolio subportfolio = resultSubPortfolio.Value;

                    if (portfolio != null)
                    {
                        portfolioStatementWrapperVM.GuidPortfolioSubPortfolio = portfolio.GuidPortfolio;
                        portfolioStatementWrapperVM.ManualPortfolio = portfolio.ManualPortfolio;
                        portfolioStatementWrapperVM.LastUpdated = DateTime.Now.ToString("HH:mm:ss");
                        portfolioStatementWrapperVM.IdCountry = portfolio.IdCountry;
                        result = _portfolioService.GetByPortfolio(portfolio.GuidPortfolio, null);
                    }
                    else if (subportfolio != null)
                    {
                        portfolioStatementWrapperVM.GuidPortfolioSubPortfolio = subportfolio.GuidSubPortfolio;
                        portfolioStatementWrapperVM.LastUpdated = DateTime.Now.ToString("HH:mm:ss");

                        resultPortfolio = _portfolioService.GetById(subportfolio.IdPortfolio);

                        if (resultPortfolio.Success)
                        {
                            portfolioStatementWrapperVM.ManualPortfolio = resultPortfolio.Value.ManualPortfolio;
                            portfolioStatementWrapperVM.IdCountry = resultPortfolio.Value.IdCountry;
                            portfolio = resultPortfolio.Value;
                        }

                        result = _portfolioService.GetBySubportfolio(subportfolio.GuidSubPortfolio, null);
                    }

                    ResultServiceObject<Trader> resultTrader = _traderService.GetById(portfolio.IdTrader);
                    portfolioStatementWrapperVM.TraderTypeId = resultTrader.Value.TraderTypeID;
                    portfolioStatementWrapperVM.CanEdit = resultTrader.Value.TraderTypeID == (int)TraderTypeEnum.Avenue || resultTrader.Value.TraderTypeID == (int)TraderTypeEnum.Toro;

                    if (result.Success && result.Value != null && result.Value.Count() > 0)
                    {
                        foreach (PortfolioStatementView portfolioView in result.Value)
                        {
                            decimal perc = portfolioView.PerformancePerc * 100;
                            API.Model.Response.v2.StockStatementVM portfolioViewModel = new API.Model.Response.v2.StockStatementVM();
                            portfolioViewModel.IdStock = portfolioView.IdStock;
                            portfolioViewModel.GuidOperation = portfolioView.GuidOperation;
                            portfolioViewModel.Company = portfolioView.Company;
                            portfolioViewModel.Segment = portfolioView.Segment;
                            portfolioViewModel.Symbol = portfolioView.Symbol;
                            portfolioViewModel.Logo = portfolioView.Logo;
                            portfolioViewModel.PerformancePerc = GetSignal(perc) + perc.ToString("n2", new CultureInfo("pt-br"));
                            portfolioViewModel.AveragePrice = portfolioView.AveragePrice.ToString("n2", new CultureInfo("pt-br"));
                            portfolioViewModel.MarketPrice = portfolioView.MarketPrice.ToString("n2", new CultureInfo("pt-br"));
                            portfolioViewModel.NetValue = GetSignal(portfolioView.NetValue) + portfolioView.NetValue.ToString("n2", new CultureInfo("pt-br"));
                            portfolioViewModel.TotalDividends = portfolioView.TotalDividends.ToString("n2", new CultureInfo("pt-br"));
                            portfolioViewModel.NumberOfShares = portfolioView.NumberOfShares.ToString("0.#############################", new CultureInfo("pt-br"));
                            portfolioViewModel.TotalMarket = portfolioView.TotalMarket.ToString("n2", new CultureInfo("pt-br"));
                            portfolioViewModel.Total = portfolioView.Total.ToString("n2", new CultureInfo("pt-br"));
                            portfolioViewModel.LogoURL = portfolioView.LogoURL;
                            portfolioViewModel.PerformancePercN = perc;
                            portfolioViewModel.AveragePriceN = portfolioView.AveragePrice;
                            portfolioViewModel.MarketPriceN = portfolioView.MarketPrice;
                            portfolioViewModel.NetValueN = portfolioView.NetValue;
                            portfolioViewModel.TotalDividendsN = portfolioView.TotalDividends;
                            portfolioViewModel.NumberOfSharesN = portfolioView.NumberOfShares;
                            portfolioViewModel.TotalMarketN = portfolioView.TotalMarket;
                            portfolioViewModel.TotalN = portfolioView.Total;

                            resultModel.Add(portfolioViewModel);
                        }

                        portfolioStatementWrapperVM.StocksStatement = resultModel;
                    }

                    portfolioStatementWrapperVM.UseNewCei = true;
                }
            }

            resultServiceObject.Value = portfolioStatementWrapperVM;
            resultServiceObject.Success = true;

            return resultServiceObject;
        }

        public ResultResponseObject<IEnumerable<PortfolioStatementViewModel>> GetZeroPriceByUser()
        {
            ResultResponseObject<IEnumerable<PortfolioStatementViewModel>> resultServiceObject = new ResultResponseObject<IEnumerable<PortfolioStatementViewModel>>();
            List<PortfolioStatementViewModel> resultModel = new List<PortfolioStatementViewModel>();

            using (_uow.Create())
            {

                ResultServiceObject<IEnumerable<PortfolioStatementView>> result = new ResultServiceObject<IEnumerable<PortfolioStatementView>>();

                result = _portfolioService.GetZeroPriceByUser(_globalAuthenticationService.IdUser);

                if (result.Success && result.Value != null && result.Value.Count() > 0)
                {
                    foreach (PortfolioStatementView portfolioView in result.Value)
                    {
                        decimal perc = portfolioView.PerformancePerc * 100;
                        PortfolioStatementViewModel portfolioViewModel = new PortfolioStatementViewModel();
                        portfolioViewModel.IdStock = portfolioView.IdStock;
                        portfolioViewModel.GuidOperation = portfolioView.GuidOperation;
                        portfolioViewModel.Company = portfolioView.Company;
                        portfolioViewModel.Segment = portfolioView.Segment;
                        portfolioViewModel.Symbol = portfolioView.Symbol;
                        portfolioViewModel.Logo = portfolioView.Logo;
                        portfolioViewModel.PerformancePerc = GetSignal(perc) + perc.ToString("n2", new CultureInfo("pt-br"));
                        portfolioViewModel.AveragePrice = portfolioView.AveragePrice.ToString("n2", new CultureInfo("pt-br"));
                        portfolioViewModel.MarketPrice = portfolioView.MarketPrice.ToString("n2", new CultureInfo("pt-br"));
                        portfolioViewModel.NetValue = GetSignal(portfolioView.NetValue) + portfolioView.NetValue.ToString("n2", new CultureInfo("pt-br"));
                        portfolioViewModel.TotalDividends = portfolioView.TotalDividends.ToString("n2", new CultureInfo("pt-br"));
                        portfolioViewModel.NumberOfShares = portfolioView.NumberOfShares.ToString("0.#############################", new CultureInfo("pt-br"));
                        portfolioViewModel.TotalMarket = portfolioView.TotalMarket.ToString("n2", new CultureInfo("pt-br"));
                        portfolioViewModel.Total = portfolioView.Total.ToString("n2", new CultureInfo("pt-br"));

                        portfolioViewModel.PerformancePercN = perc;
                        portfolioViewModel.AveragePriceN = portfolioView.AveragePrice;
                        portfolioViewModel.MarketPriceN = portfolioView.MarketPrice;
                        portfolioViewModel.NetValueN = portfolioView.NetValue;
                        portfolioViewModel.TotalDividendsN = portfolioView.TotalDividends;
                        portfolioViewModel.NumberOfSharesN = portfolioView.NumberOfShares;
                        portfolioViewModel.TotalMarketN = portfolioView.TotalMarket;
                        portfolioViewModel.TotalN = portfolioView.Total;
                        portfolioViewModel.LastUpdated = DateTime.Now.ToString("HH:mm:ss");
                        portfolioViewModel.IdCountry = portfolioView.IdCountry;

                        resultModel.Add(portfolioViewModel);
                    }
                }

            }

            resultServiceObject.Value = resultModel;
            resultServiceObject.Success = true;

            return resultServiceObject;
        }

        public ResultResponseObject<DividendViewWrapperVM> GetDividendView(Guid guidPortfolioSub)
        {
            ResultResponseObject<DividendViewWrapperVM> resultServiceObject = new ResultResponseObject<DividendViewWrapperVM>();
            DividendViewWrapperVM dividendViewWrapperVM = new DividendViewWrapperVM();

            using (_uow.Create())
            {
                ResultServiceObject<Portfolio> resultPortfolio = _portfolioService.GetByGuid(guidPortfolioSub);
                ResultServiceObject<SubPortfolio> resultSubPortfolio = _subPortfolioService.GetByGuid(guidPortfolioSub);

                if (resultSubPortfolio.Success && resultPortfolio.Success)
                {
                    ResultServiceObject<IEnumerable<DividendDetailsView>> result = new ResultServiceObject<IEnumerable<DividendDetailsView>>();
                    Portfolio portfolio = resultPortfolio.Value;
                    SubPortfolio subportfolio = resultSubPortfolio.Value;

                    if (portfolio != null)
                    {
                        dividendViewWrapperVM.GuidPortfolioSubPortfolio = portfolio.GuidPortfolio;
                        dividendViewWrapperVM.IdPortfolio = portfolio.IdPortfolio;
                        dividendViewWrapperVM.IdCountry = portfolio.IdCountry;
                        result = _dividendService.GetDetailsByPortfolio(portfolio.IdPortfolio);
                    }
                    else if (subportfolio != null)
                    {
                        dividendViewWrapperVM.GuidPortfolioSubPortfolio = subportfolio.GuidSubPortfolio;
                        dividendViewWrapperVM.IdPortfolio = subportfolio.IdPortfolio;
                        resultPortfolio = _portfolioService.GetById(subportfolio.IdPortfolio);

                        if (resultPortfolio.Value != null)
                        {
                            dividendViewWrapperVM.IdCountry = resultPortfolio.Value.IdCountry;
                        }

                        result = _dividendService.GetDetailsBySubportfolio(subportfolio.IdSubPortfolio);
                    }

                    if (result.Success && result.Value != null && result.Value.Count() > 0)
                    {
                        dividendViewWrapperVM.DividendsCompany = new List<DividendCompanyVM>();
                        decimal totalReceived = 0;
                        decimal totalToReceive = 0;

                        List<DividendDetailsView> dividendDetailsViews = result.Value.OrderBy(div => div.Symbol).ThenByDescending(div => div.PaymentDate).ToList();

                        foreach (DividendDetailsView dividendDetailsView in dividendDetailsViews)
                        {
                            DividendDetailsVM dividendDetailsVM = new DividendDetailsVM();
                            dividendDetailsVM.DividendType = dividendDetailsView.DividendType;
                            dividendDetailsVM.IdDividend = dividendDetailsView.IdDividend;
                            dividendDetailsVM.NetValue = dividendDetailsView.NetValue.ToString("n2", new CultureInfo("pt-br"));
                            dividendDetailsVM.PaymentDate = dividendDetailsView.PaymentDate.HasValue ? dividendDetailsView.PaymentDate.Value.ToString("dd/MM/yyyy") : "Data Indef";

                            DividendCompanyVM dividendCompanyVM = dividendViewWrapperVM.DividendsCompany.FirstOrDefault(divTmp => divTmp.Symbol == dividendDetailsView.Symbol);

                            if (dividendCompanyVM == null)
                            {
                                decimal totalReceivedCp = dividendDetailsViews.Where(divTmp => divTmp.Symbol == dividendDetailsView.Symbol && (divTmp.PaymentDate.HasValue && divTmp.PaymentDate.Value <= DateTime.Now)).Sum(divTmp => divTmp.NetValue);
                                decimal totalToReceiveCp = dividendDetailsViews.Where(divTmp => divTmp.Symbol == dividendDetailsView.Symbol && ((divTmp.PaymentDate.HasValue && divTmp.PaymentDate.Value > DateTime.Now) || !divTmp.PaymentDate.HasValue)).Sum(divTmp => divTmp.NetValue);

                                totalReceived += totalReceivedCp;
                                totalToReceive += totalToReceiveCp;

                                dividendCompanyVM = new DividendCompanyVM();
                                dividendCompanyVM.Company = dividendDetailsView.Company;
                                dividendCompanyVM.IdCompany = dividendDetailsView.IdCompany;
                                dividendCompanyVM.IdStock = dividendDetailsView.IdStock;
                                dividendCompanyVM.Logo = dividendDetailsView.Logo;
                                dividendCompanyVM.Segment = dividendDetailsView.Segment;
                                dividendCompanyVM.Symbol = dividendDetailsView.Symbol;
                                dividendCompanyVM.TotalReceived = totalReceivedCp.ToString("n2", new CultureInfo("pt-br"));
                                dividendCompanyVM.TotalToReceive = totalToReceiveCp.ToString("n2", new CultureInfo("pt-br"));
                                dividendCompanyVM.Dividends = new List<DividendDetailsVM>();

                                dividendViewWrapperVM.DividendsCompany.Add(dividendCompanyVM);

                                totalReceivedCp = 0;
                                totalToReceiveCp = 0;
                            }

                            dividendCompanyVM.Dividends.Add(dividendDetailsVM);
                        }

                        dividendViewWrapperVM.TotalReceived = totalReceived.ToString("n2", new CultureInfo("pt-br"));
                        dividendViewWrapperVM.TotalToReceive = totalToReceive.ToString("n2", new CultureInfo("pt-br"));
                    }
                }
            }

            resultServiceObject.Value = dividendViewWrapperVM;
            resultServiceObject.Success = true;

            return resultServiceObject;
        }

        public ResultResponseObject<OperationSellViewWrapperVM> GetOperationSellView(Guid guidPortfolioSub)
        {
            ResultResponseObject<OperationSellViewWrapperVM> resultServiceObject = new ResultResponseObject<OperationSellViewWrapperVM>();
            OperationSellViewWrapperVM operationSellViewWrapperVM = new OperationSellViewWrapperVM();

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
                        result = _operationService.GetSellDetailsByIdPortfolio(portfolio.IdPortfolio);
                        operationSellViewWrapperVM.IdCountry = portfolio.IdCountry;
                    }
                    else if (subportfolio != null)
                    {
                        operationSellViewWrapperVM.GuidPortfolioSubPortfolio = subportfolio.GuidSubPortfolio;
                        operationSellViewWrapperVM.IdPortfolio = subportfolio.IdPortfolio;
                        result = _operationService.GetSellDetailsByIdSubportfolio(subportfolio.IdPortfolio, subportfolio.IdSubPortfolio);

                        resultPortfolio = _portfolioService.GetById(subportfolio.IdPortfolio);

                        if (resultPortfolio.Value != null)
                        {
                            operationSellViewWrapperVM.IdCountry = resultPortfolio.Value.IdCountry;
                        }
                    }

                    if (result.Success && result.Value != null && result.Value.Count() > 0)
                    {
                        operationSellViewWrapperVM.OperationsSellCompany = new List<OperationSellCompanyVM>();
                        decimal totalSold = 0;
                        string[] stocksThousand = null;
                        ResultServiceObject<Entity.Entities.SystemSettings> resultStockThousand = _systemSettingsService.GetByKey(Constants.SYSTEM_SETTINGS_DIVIDE_STOCK_BY_THOUSAND);

                        if (resultStockThousand.Success && resultStockThousand.Value != null)
                        {
                            stocksThousand = resultStockThousand.Value.SettingValue.Split(';');
                        }

                        List<OperationSellDetailsView> operationSellDetailsViews = result.Value.OrderBy(op => op.Symbol).ThenByDescending(op => op.EventDate).ToList();

                        foreach (OperationSellDetailsView operationSellDetailsView in operationSellDetailsViews)
                        {
                            OperationSellDetailsVM operationSellDetailsVM = new OperationSellDetailsVM();
                            operationSellDetailsVM.AveragePrice = operationSellDetailsView.AveragePrice.ToString("n2", new CultureInfo("pt-br"));
                            operationSellDetailsVM.IdOperationItem = operationSellDetailsView.IdOperationItem;
                            operationSellDetailsVM.NumberOfShares = operationSellDetailsView.NumberOfShares.ToString();
                            operationSellDetailsVM.EventDate = operationSellDetailsView.EventDate.HasValue ? operationSellDetailsView.EventDate.Value.ToString("dd/MM/yyyy") : "Data Indef";

                            OperationSellCompanyVM operationSellCompanyVM = operationSellViewWrapperVM.OperationsSellCompany.FirstOrDefault(op => op.Symbol == operationSellDetailsView.Symbol);

                            if (operationSellCompanyVM == null)
                            {
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

                                totalSold += totalSoldCp;

                                operationSellCompanyVM = new OperationSellCompanyVM();
                                operationSellCompanyVM.Company = operationSellDetailsView.Company;
                                operationSellCompanyVM.IdCompany = operationSellDetailsView.IdCompany;
                                operationSellCompanyVM.IdStock = operationSellDetailsView.IdStock;
                                operationSellCompanyVM.Logo = operationSellDetailsView.Logo;
                                operationSellCompanyVM.Segment = operationSellDetailsView.Segment;
                                operationSellCompanyVM.Symbol = operationSellDetailsView.Symbol;
                                operationSellCompanyVM.TotalSold = totalSoldCp.ToString("n2", new CultureInfo("pt-br"));
                                operationSellCompanyVM.Operations = new List<OperationSellDetailsVM>();

                                operationSellViewWrapperVM.OperationsSellCompany.Add(operationSellCompanyVM);

                                totalSoldCp = 0;
                            }

                            operationSellCompanyVM.Operations.Add(operationSellDetailsVM);
                        }

                        operationSellViewWrapperVM.TotalSold = totalSold.ToString("n2", new CultureInfo("pt-br"));
                    }
                }
            }

            resultServiceObject.Value = operationSellViewWrapperVM;
            resultServiceObject.Success = true;

            return resultServiceObject;
        }

        #endregion

        public ResultResponseBase CalculatePerformance()
        {
            ResultResponseBase resultResponseBase = new ResultResponseBase();

            using (_uow.Create())
            {
                ResultServiceObject<IEnumerable<Portfolio>> resultPort = _portfolioService.GetByUser(_globalAuthenticationService.IdUser, true, false);

                if (resultPort.Success && resultPort.Value != null && resultPort.Value.Count() > 0)
                {
                    foreach (Portfolio portfolio in resultPort.Value)
                    {
                        _portfolioService.CalculatePerformance(portfolio.IdPortfolio, _systemSettingsService, _portfolioPerformanceService, _stockService, _operationService, _performanceStockService, _holidayService);
                    }
                }

                resultResponseBase.Success = true;
            }

            return resultResponseBase;
        }

        public ResultResponseBase UpdateName(Guid idportfolio, PortfolioEditVM portfolioEditVM)
        {
            ResultResponseBase resultResponseBase = new ResultResponseBase();

            using (_uow.Create())
            {
                ResultServiceObject<Portfolio> resultPort = _portfolioService.GetByGuid(idportfolio);

                if (resultPort.Success && resultPort.Value != null)
                {
                    _portfolioService.UpdateName(resultPort.Value.IdPortfolio, portfolioEditVM.Name);
                }

                resultResponseBase.Success = true;
            }

            return resultResponseBase;
        }

        public ResultResponseObject<DividendChartScrollVM> GetDividendScrollChart(Guid guidPortfolioSub, int year)
        {
            ResultResponseObject<DividendChartScrollVM> resultServiceObject = new ResultResponseObject<DividendChartScrollVM>();
            DividendChartScrollVM dividendChart = new DividendChartScrollVM();
            dividendChart.Categories = new List<DividendChartScrollCategory>();
            dividendChart.Dataset = new List<Dividendos.API.Model.Response.Dataset>();


            using (_uow.Create())
            {
                ResultServiceObject<Portfolio> resultPortfolio = _portfolioService.GetByGuid(guidPortfolioSub);
                ResultServiceObject<SubPortfolio> resultSubPortfolio = _subPortfolioService.GetByGuid(guidPortfolioSub);

                if (resultSubPortfolio.Success && resultPortfolio.Success)
                {
                    Portfolio portfolio = resultPortfolio.Value;
                    SubPortfolio subportfolio = resultSubPortfolio.Value;

                    Guid guidPortfolio = Guid.Empty;

                    long idPortfolio = 0;
                    bool isSub = false;

                    if (portfolio != null)
                    {
                        idPortfolio = portfolio.IdPortfolio;
                        dividendChart.IdCountry = portfolio.IdCountry;
                    }
                    else if (subportfolio != null)
                    {
                        idPortfolio = subportfolio.IdPortfolio;
                        isSub = true;

                        resultPortfolio = _portfolioService.GetById(subportfolio.IdPortfolio);

                        if (resultPortfolio.Value != null)
                        {
                            dividendChart.IdCountry = resultPortfolio.Value.IdCountry;
                        }
                    }

                    ResultServiceObject<IEnumerable<DividendView>> resultDividend = new ResultServiceObject<IEnumerable<DividendView>>();
                    DateTime startDate = new DateTime(year, 1, 1);
                    DateTime endDate = new DateTime(year, 12, 31);

                    if (isSub)
                    {
                        resultDividend = _dividendService.GetAllActiveBySubPortfolioRangeDate(subportfolio.IdSubPortfolio, startDate, endDate);
                    }
                    else
                    {
                        resultDividend = _dividendService.GetAllActiveByPortfolioRangeDate(portfolio.IdPortfolio, startDate, endDate);
                    }


                    if (resultDividend.Success)
                    {
                        IEnumerable<DividendView> dividends = resultDividend.Value;

                        if (dividends != null && dividends.Count() > 0)
                        {

                            DividendChartScrollCategory dividendChartScrollCategory = new DividendChartScrollCategory();
                            dividendChart.Categories.Add(dividendChartScrollCategory);
                            dividendChartScrollCategory.Category = new List<CategoryCategory>();

                            List<DividendView> lstStocks = dividends.Where(divStock => divStock.IdStockType != 2).ToList();
                            List<DividendView> lstReits = dividends.Where(divStock => divStock.IdStockType == 2).ToList();

                            Dividendos.API.Model.Response.Dataset dtStocks = new API.Model.Response.Dataset();
                            dtStocks.SeriesName = "Ações";
                            dtStocks.Data = new List<API.Model.Response.Datum>();

                            Dividendos.API.Model.Response.Dataset dtReits = new API.Model.Response.Dataset();
                            dtReits.SeriesName = "FIIs";
                            dtReits.Data = new List<API.Model.Response.Datum>();

                            dividendChart.Dataset.Add(dtStocks);
                            dividendChart.Dataset.Add(dtReits);

                            for (int i = 1; i <= 12; i++)
                            {
                                string monthName = new DateTime(year, i, 1).ToString("MMM", new CultureInfo("pt-br"));
                                CategoryCategory categoryCategory = new CategoryCategory();
                                categoryCategory.Label = monthName.First().ToString().ToUpper() + monthName.Substring(1);
                                dividendChartScrollCategory.Category.Add(categoryCategory);

                                decimal sumStocks = lstStocks.Where(stkTmp => stkTmp.PaymentDate.HasValue && stkTmp.PaymentDate.Value.Month == i).Sum(stkTmp => stkTmp.NetValue);
                                decimal sumReits = lstReits.Where(stkTmp => stkTmp.PaymentDate.HasValue && stkTmp.PaymentDate.Value.Month == i).Sum(stkTmp => stkTmp.NetValue);

                                API.Model.Response.Datum datumStock = new API.Model.Response.Datum();
                                datumStock.Value = sumStocks.ToString("n2", new CultureInfo("en-us")).Replace(",", string.Empty);

                                if (sumStocks.Equals(0))
                                {
                                    datumStock.ShowValues = "0";
                                }
                                else
                                {
                                    datumStock.ShowValues = "1";
                                }

                                dtStocks.Data.Add(datumStock);

                                API.Model.Response.Datum datumReit = new API.Model.Response.Datum();
                                datumReit.Value = sumReits.ToString("n2", new CultureInfo("en-us")).Replace(",", string.Empty);

                                if (sumReits.Equals(0))
                                {
                                    datumReit.ShowValues = "0";
                                }
                                else
                                {
                                    datumReit.ShowValues = "1";
                                }

                                dtReits.Data.Add(datumReit);
                            }

                            dividendChart.TotalStocks = lstStocks.Where(stkTmp => stkTmp.PaymentDate.HasValue).Sum(stkTmp => stkTmp.NetValue).ToString("n2", new CultureInfo("pt-br"));
                            dividendChart.TotalReits = lstReits.Where(stkTmp => stkTmp.PaymentDate.HasValue).Sum(stkTmp => stkTmp.NetValue).ToString("n2", new CultureInfo("pt-br"));
                            dividendChart.LastUpdated = DateTime.Now.ToString("HH:mm:ss");
                            dividendChart.Title = string.Format("Calendário {0}", year);
                            dividendChart.Year = year.ToString();
                        }
                    }
                }
            }

            resultServiceObject.Value = dividendChart;
            resultServiceObject.Success = true;

            return resultServiceObject;
        }

        public ResultResponseObject<StockStatementViewModel> GetStockStatementView(Guid guidPortfolio, long idStock)
        {
            ResultResponseObject<StockStatementViewModel> resultServiceObject = new ResultResponseObject<StockStatementViewModel>();

            using (_uow.Create())
            {
                ResultServiceObject<Portfolio> resultPortfolio = _portfolioService.GetByGuid(guidPortfolio);
                ResultServiceObject<SubPortfolio> resultSubPortfolio = _subPortfolioService.GetByGuid(guidPortfolio);
                Guid guidPortfolioTmp = Guid.Empty;
                int idCountry = 1;

                if (resultSubPortfolio.Success && resultPortfolio.Success)
                {
                    Portfolio portfolio = resultPortfolio.Value;
                    SubPortfolio subportfolio = resultSubPortfolio.Value;

                    if (portfolio != null)
                    {
                        guidPortfolioTmp = portfolio.GuidPortfolio;
                        idCountry = portfolio.IdCountry;
                    }
                    else
                    {
                        ResultServiceObject<Portfolio> portfolioDb = _portfolioService.GetById(subportfolio.IdPortfolio);

                        if (portfolioDb.Value != null)
                        {
                            guidPortfolioTmp = portfolioDb.Value.GuidPortfolio;
                            idCountry = portfolioDb.Value.IdCountry;
                        }
                    }
                }

                ResultServiceObject<StockStatementView> resultStockSymbol = _portfolioService.GetByIdStock(guidPortfolioTmp, idStock);

                if (resultStockSymbol.Success && resultStockSymbol.Value != null)
                {
                    StockStatementView stockStatementView = resultStockSymbol.Value;
                    ResultServiceObject<IEnumerable<PortfolioStatementView>> result = new ResultServiceObject<IEnumerable<PortfolioStatementView>>();


                    decimal perc = stockStatementView.PerformancePerc * 100;
                    StockStatementViewModel stockStatementViewModel = new StockStatementViewModel();
                    stockStatementViewModel.IdStock = stockStatementView.IdStock;
                    stockStatementViewModel.GuidPortfolio = guidPortfolioTmp;
                    stockStatementViewModel.GuidOperation = stockStatementView.GuidOperation;
                    stockStatementViewModel.Company = stockStatementView.Company;
                    stockStatementViewModel.Segment = stockStatementView.Segment;
                    stockStatementViewModel.Symbol = stockStatementView.Symbol;
                    stockStatementViewModel.Logo = stockStatementView.Logo;
                    stockStatementViewModel.PerformancePerc = GetSignal(perc) + perc.ToString("n2", new CultureInfo("pt-br"));
                    stockStatementViewModel.AveragePrice = stockStatementView.AveragePrice.ToString("n2", new CultureInfo("pt-br"));

                    stockStatementViewModel.NetValue = GetSignal(stockStatementView.NetValue) + stockStatementView.NetValue.ToString("n2", new CultureInfo("pt-br"));
                    stockStatementViewModel.TotalDividends = stockStatementView.TotalDividends.ToString("n2", new CultureInfo("pt-br"));
                    stockStatementViewModel.NumberOfShares = stockStatementView.NumberOfShares.ToString("0.#############################", new CultureInfo("pt-br"));
                    stockStatementViewModel.TotalMarket = stockStatementView.TotalMarket.ToString("n2", new CultureInfo("pt-br"));
                    stockStatementViewModel.Total = stockStatementView.Total.ToString("n2", new CultureInfo("pt-br"));

                    stockStatementViewModel.PerformancePercN = perc;
                    stockStatementViewModel.AveragePriceN = stockStatementView.AveragePrice;

                    stockStatementViewModel.NetValueN = stockStatementView.NetValue;
                    stockStatementViewModel.TotalDividendsN = stockStatementView.TotalDividends;
                    stockStatementViewModel.NumberOfSharesN = stockStatementView.NumberOfShares;
                    stockStatementViewModel.TotalMarketN = stockStatementView.TotalMarket;
                    stockStatementViewModel.TotalN = stockStatementView.Total;
                    stockStatementViewModel.LastUpdated = DateTime.Now.ToString("HH:mm:ss");
                    stockStatementViewModel.IdCountry = idCountry;

                    if (stockStatementView.ShowOnPortolio)
                    {
                        stockStatementViewModel.MarketPrice = stockStatementView.MarketPrice.ToString("n2", new CultureInfo("pt-br"));
                        stockStatementViewModel.MarketPriceN = stockStatementView.MarketPrice;
                    }
                    else
                    {
                        stockStatementViewModel.MarketPrice = "0,00";
                        stockStatementViewModel.MarketPriceN = 0;
                    }

                    resultServiceObject.Value = stockStatementViewModel;
                }
            }

            resultServiceObject.Success = true;
            return resultServiceObject;
        }

        public ResultResponseObject<PortfolioVM> CreateManualPortfolio(string portfolioName, int idCountry)
        {
            ResultResponseObject<PortfolioVM> resultResponsePortfolio = new ResultResponseObject<PortfolioVM>();
            using (_uow.Create())
            {
                ResultServiceObject<Trader> resultService = _traderService.SaveTrader("0", "0", _globalAuthenticationService.IdUser, false, true, TraderTypeEnum.RendaVariavelManual);

                Portfolio portfolio = new Portfolio();
                portfolio.IdTrader = resultService.Value.IdTrader;
                portfolio.Name = portfolioName;
                portfolio.ManualPortfolio = true;
                portfolio.IdCountry = idCountry;

                ResultServiceObject<Portfolio> resultServicePortfolio = _portfolioService.Insert(portfolio);

                resultResponsePortfolio = _mapper.Map<ResultResponseObject<PortfolioVM>>(resultServicePortfolio);
                resultResponsePortfolio.Success = true;
            }

            return resultResponsePortfolio;
        }

        public void GeneratePerfomanceHistorical(Guid guidPortfolio)
        {

            using (_uow.Create())
            {
                ResultServiceObject<IEnumerable<OperationItemView>> resultOperationItem = _operationItemService.GetAllItemViewByPortfolio(5, 1, true);

                if (resultOperationItem.Success && resultOperationItem.Value != null && resultOperationItem.Value.Count() > 0)
                {
                    List<OperationItemView> operationItemsView = resultOperationItem.Value.OrderBy(opItem => opItem.EventDate.Value).ToList();

                    List<PerfomanceHistoricalView> perfomanceHistoricals = operationItemsView.GroupBy(objStockOperationTmp => objStockOperationTmp.EventDate.Value)
                                                            .Select(objStockOperationGp =>
                                                            {

                                                                PerfomanceHistoricalView perfomanceHistoricalView = new PerfomanceHistoricalView();
                                                                perfomanceHistoricalView.Name = objStockOperationGp.First().EventDate.Value.ToString();


                                                                List<OperationItemView> opItemsDate = operationItemsView.Where(op => op.EventDate.Value.Date <= objStockOperationGp.First().EventDate.Value).ToList();

                                                                decimal total = 0;

                                                                foreach (OperationItemView opItemDate in opItemsDate)
                                                                {
                                                                    if (opItemDate.IdOperationType == 1)
                                                                    {
                                                                        total += opItemDate.AveragePrice * opItemDate.NumberOfShares;
                                                                    }
                                                                    else
                                                                    {
                                                                        total -= opItemDate.AveragePrice * opItemDate.NumberOfShares;
                                                                    }
                                                                }

                                                                if (total > 0)
                                                                {
                                                                    perfomanceHistoricalView.Total = total;
                                                                }

                                                                return perfomanceHistoricalView;
                                                            }
                                                            ).ToList();
                }
            }
        }

        public ResultResponseObject<TraderVM> Validate2FAAndImportFromPassfolio(Passfolio2FARequest passfolio2FARequest)
        {
            ResultResponseObject<TraderVM> resultResponseObject;

            //Validate 2FA
            if (_passfolioHelper.SessionMFA(passfolio2FARequest.Auth, passfolio2FARequest.Code, passfolio2FARequest.AuthenticatorID))
            {
                this.ImportFromPasfolio(passfolio2FARequest.Email, passfolio2FARequest.Auth, _globalAuthenticationService.IdUser, false);
                resultResponseObject = new ResultResponseObject<TraderVM>() { Success = true };
            }
            else
            {
                resultResponseObject = new ResultResponseObject<TraderVM>() { Success = false };
                resultResponseObject.ErrorMessages.Add("Código SMS inválido! Verifique o código recebido no seu celular.");
            }

            return resultResponseObject;
        }



        public ResultResponseObject<TraderVM> ForceSync(API.Model.Response.v7.TraderType traderType, Guid traderGuid)
        {
            ResultServiceObject<Trader> trader;

            using (_uow.Create())
            {
                trader = _traderService.GetByUserAndGuidTrader(_globalAuthenticationService.IdUser, traderGuid);
            }

            if (trader.Value != null)
            {
                switch ((int)traderType)
                {
                    case (int)TraderTypeEnum.RendaVariavelAndTesouroDiretoCEI:
                        {
                            using (_uow.Create())
                            {
                                this.ImportStocksAndTesouroDiretoCeiBase(trader.Value.Identifier, trader.Value.Password, _globalAuthenticationService.IdUser, false);
                            }
                            break;
                        }
                    case (int)TraderTypeEnum.Passfolio:
                        {
                            this.ImportFromPasfolio(trader.Value.Identifier, trader.Value.Password, _globalAuthenticationService.IdUser, false);
                            break;
                        }
                    case (int)TraderTypeEnum.MercadoBitcoin:
                        {
                            this.ImportMercadoBitcoin(trader.Value.Identifier, trader.Value.Password, _globalAuthenticationService.IdUser, false);
                            break;
                        }
                    case (int)TraderTypeEnum.Binance:
                        {
                            this.ImportBinance(trader.Value.Identifier, trader.Value.Password, _globalAuthenticationService.IdUser, false);
                            break;
                        }
                    case (int)TraderTypeEnum.BitcoinTrade:
                        {
                            this.ImportBitcoinTrade(trader.Value.Identifier, trader.Value.Password, _globalAuthenticationService.IdUser, false);
                            break;
                        }
                    case (int)TraderTypeEnum.Coinbase:
                        {
                            this.ImportCoinbase(trader.Value.Identifier, trader.Value.Password, _globalAuthenticationService.IdUser, false);
                            break;
                        }
                    case (int)TraderTypeEnum.BitcoinToYou:
                        {
                            this.ImportBitcoinToYou(trader.Value.Identifier, trader.Value.Password, _globalAuthenticationService.IdUser, false);
                            break;
                        }
                    case (int)TraderTypeEnum.Biscoint:
                        {
                            this.ImportBiscoint(trader.Value.Identifier, trader.Value.Password, _globalAuthenticationService.IdUser, false);
                            break;
                        }
                    case (int)TraderTypeEnum.BitPreco:
                        {
                            this.ImportBitPreco(trader.Value.Identifier, trader.Value.Password, _globalAuthenticationService.IdUser, false);
                            break;
                        }
                    case (int)TraderTypeEnum.RendaVariavelAndTesouroDiretoNewCEI:
                        {
                            using (_uow.Create())
                            {
                                this.ImportStocksAndTesouroDiretoCeiBase(trader.Value.Identifier, trader.Value.Password, _globalAuthenticationService.IdUser, false, false, TraderTypeEnum.RendaVariavelAndTesouroDiretoNewCEI);
                            }
                            break;
                        }
                }
            }

            ResultResponseObject<TraderVM> resultReponse = _mapper.Map<ResultResponseObject<TraderVM>>(trader);

            return resultReponse;
        }

        private ImportCeiResultView ConvertCeiResult(ImportCeiResult importCeiResult)
        {
            ImportCeiResultView importCeiResultView = new ImportCeiResultView();
            importCeiResultView.Identifier = importCeiResult.Identifier;
            importCeiResultView.Password = importCeiResult.Password;
            importCeiResultView.IdUser = importCeiResult.IdUser;
            importCeiResultView.AutomaticProcess = importCeiResult.AutomaticProcess;
            importCeiResultView.Imported = importCeiResult.Imported;
            importCeiResultView.ErrorCEI = importCeiResult.ErrorCEI;
            importCeiResultView.HasRent = importCeiResult.HasRent;
            importCeiResultView.Message = importCeiResult.Message;
            importCeiResultView.UserBlocked = importCeiResult.UserBlocked;

            importCeiResultView.ListDividend = new List<DividendImportView>();
            importCeiResultView.ListStockOperation = new List<StockOperationView>();
            importCeiResultView.ListStockPortfolio = new List<StockOperationView>();
            importCeiResultView.ListTesouroDireto = new List<TesouroDiretoImportView>();
            importCeiResultView.ListStockAveragePrice = new List<StockOperationView>();

            if (importCeiResult.ListDividend != null && importCeiResult.ListDividend.Count > 0)
            {
                foreach (DividendImport dividendImport in importCeiResult.ListDividend)
                {
                    DividendImportView dividendImportView = new DividendImportView();
                    dividendImportView.BaseQtty = dividendImport.BaseQtty;
                    dividendImportView.BaseQuantity = dividendImport.BaseQuantity;
                    dividendImportView.Broker = dividendImport.Broker;
                    dividendImportView.DividendType = dividendImport.DividendType;
                    dividendImportView.GrossVal = dividendImport.GrossVal;
                    dividendImportView.GrossValue = dividendImport.GrossValue;
                    dividendImportView.NetVal = dividendImport.NetVal;
                    dividendImportView.NetValue = dividendImport.NetValue;
                    dividendImportView.PaymentDate = dividendImport.PaymentDate;
                    dividendImportView.PaymentDt = dividendImport.PaymentDt;
                    dividendImportView.Symbol = dividendImport.Symbol;

                    importCeiResultView.ListDividend.Add(dividendImportView);
                }
            }

            if (importCeiResult.ListStockOperation != null && importCeiResult.ListStockOperation.Count > 0)
            {
                foreach (StockOperation stockOperation in importCeiResult.ListStockOperation)
                {
                    StockOperationView stockOperationView = new StockOperationView();
                    stockOperationView.AcquisitionPrice = stockOperation.AcquisitionPrice;
                    stockOperationView.AveragePrice = stockOperation.AveragePrice;
                    stockOperationView.Broker = stockOperation.Broker;
                    stockOperationView.CopyData = stockOperation.CopyData;
                    stockOperationView.DaysLastItem = stockOperation.DaysLastItem;
                    stockOperationView.EditedByUser = stockOperation.EditedByUser;
                    stockOperationView.EventDate = stockOperation.EventDate;
                    stockOperationView.Expire = stockOperation.Expire;
                    stockOperationView.Factor = stockOperation.Factor;
                    stockOperationView.HasNewItem = stockOperation.HasNewItem;
                    stockOperationView.IdOperationItem = stockOperation.IdOperationItem;
                    stockOperationView.IsCeiOk = stockOperation.IsCeiOk;
                    stockOperationView.LastUpdatedDate = stockOperation.LastUpdatedDate;
                    stockOperationView.Market = stockOperation.Market;
                    stockOperationView.NumberOfBuyShares = stockOperation.NumberOfBuyShares;
                    stockOperationView.NumberOfShares = stockOperation.NumberOfShares;
                    stockOperationView.OperationType = stockOperation.OperationType;
                    stockOperationView.PriceAdjust = stockOperation.PriceAdjust;
                    stockOperationView.PriceAdjustNew = stockOperation.PriceAdjustNew;
                    stockOperationView.PriceLastEditedByUser = stockOperation.PriceLastEditedByUser;
                    stockOperationView.StockSpec = stockOperation.StockSpec;
                    stockOperationView.Symbol = stockOperation.Symbol;

                    importCeiResultView.ListStockOperation.Add(stockOperationView);
                }
            }

            if (importCeiResult.ListStockPortfolio != null && importCeiResult.ListStockPortfolio.Count > 0)
            {
                foreach (StockOperation stockOperation in importCeiResult.ListStockPortfolio)
                {
                    StockOperationView stockOperationView = new StockOperationView();
                    stockOperationView.AcquisitionPrice = stockOperation.AcquisitionPrice;
                    stockOperationView.AveragePrice = stockOperation.AveragePrice;
                    stockOperationView.Broker = stockOperation.Broker;
                    stockOperationView.CopyData = stockOperation.CopyData;
                    stockOperationView.DaysLastItem = stockOperation.DaysLastItem;
                    stockOperationView.EditedByUser = stockOperation.EditedByUser;
                    stockOperationView.EventDate = stockOperation.EventDate;
                    stockOperationView.Expire = stockOperation.Expire;
                    stockOperationView.Factor = stockOperation.Factor;
                    stockOperationView.HasNewItem = stockOperation.HasNewItem;
                    stockOperationView.IdOperationItem = stockOperation.IdOperationItem;
                    stockOperationView.IsCeiOk = stockOperation.IsCeiOk;
                    stockOperationView.LastUpdatedDate = stockOperation.LastUpdatedDate;
                    stockOperationView.Market = stockOperation.Market;
                    stockOperationView.NumberOfBuyShares = stockOperation.NumberOfBuyShares;
                    stockOperationView.NumberOfShares = stockOperation.NumberOfShares;
                    stockOperationView.OperationType = stockOperation.OperationType;
                    stockOperationView.PriceAdjust = stockOperation.PriceAdjust;
                    stockOperationView.PriceAdjustNew = stockOperation.PriceAdjustNew;
                    stockOperationView.PriceLastEditedByUser = stockOperation.PriceLastEditedByUser;
                    stockOperationView.StockSpec = stockOperation.StockSpec;
                    stockOperationView.Symbol = stockOperation.Symbol;

                    importCeiResultView.ListStockPortfolio.Add(stockOperationView);
                }
            }

            if (importCeiResult.ListTesouroDireto != null && importCeiResult.ListTesouroDireto.Count > 0)
            {
                foreach (TesouroDiretoImport tesouroDiretoImport in importCeiResult.ListTesouroDireto)
                {
                    TesouroDiretoImportView tesouroDiretoImportView = new TesouroDiretoImportView();
                    tesouroDiretoImportView.BaseQtty = tesouroDiretoImport.BaseQtty;
                    tesouroDiretoImportView.BaseQuantity = tesouroDiretoImport.BaseQuantity;
                    tesouroDiretoImportView.Broker = tesouroDiretoImport.Broker;
                    tesouroDiretoImportView.GrossVal = tesouroDiretoImport.GrossVal;
                    tesouroDiretoImportView.GrossValue = tesouroDiretoImport.GrossValue;
                    tesouroDiretoImportView.NetVal = tesouroDiretoImport.NetVal;
                    tesouroDiretoImportView.NetValue = tesouroDiretoImport.NetValue;
                    tesouroDiretoImportView.Symbol = tesouroDiretoImport.Symbol;
                    tesouroDiretoImportView.Market = tesouroDiretoImport.Market;
                    tesouroDiretoImportView.MarketValue = tesouroDiretoImport.MarketValue;
                    tesouroDiretoImportView.Period = tesouroDiretoImport.Period;
                    tesouroDiretoImportView.PeriodValue = tesouroDiretoImport.PeriodValue;
                    tesouroDiretoImportView.StockSpec = tesouroDiretoImport.StockSpec;
                    tesouroDiretoImportView.Symbol = tesouroDiretoImport.Symbol;

                    importCeiResultView.ListTesouroDireto.Add(tesouroDiretoImportView);
                }
            }

            if (importCeiResult.ListStockAveragePrice != null && importCeiResult.ListStockAveragePrice.Count > 0)
            {
                foreach (StockOperation stockOperation in importCeiResult.ListStockAveragePrice)
                {
                    StockOperationView stockOperationView = new StockOperationView();
                    stockOperationView.AveragePrice = stockOperation.AveragePrice;
                    stockOperationView.Broker = stockOperation.Broker;
                    stockOperationView.NumberOfSellShares = stockOperation.NumberOfSellShares;
                    stockOperationView.NumberOfShares = stockOperation.NumberOfShares;
                    stockOperationView.Symbol = stockOperation.Symbol;

                    importCeiResultView.ListStockAveragePrice.Add(stockOperationView);
                }
            }

            return importCeiResultView;
        }

        public void RunDelayedCeiImport()
        {
            ResultServiceObject<IEnumerable<Trader>> resultTraders = new ResultServiceObject<IEnumerable<Trader>>();
            DateTime now = DateTime.Now;

            using (_uow.Create())
            {
                int unitsAvailable = 500;
                ResultServiceObject<Entity.Entities.SystemSettings> resultUnitsAvailable = _systemSettingsService.GetByKey("UnitsAvailable");
                ResultServiceObject<int> resultServiceCountScrapy = _scrapySchedulerService.CountJobsRunningOrAwaiting();
                int totalPerAgent = resultServiceCountScrapy.Value;

                if (resultUnitsAvailable.Value != null)
                {
                    unitsAvailable = Convert.ToInt32(resultUnitsAvailable.Value.SettingValue);
                }

                int totalNextRun = unitsAvailable - totalPerAgent;

                if (totalNextRun > 0)
                {
                    if (now.DayOfWeek == DayOfWeek.Sunday || now.DayOfWeek == DayOfWeek.Saturday || _holidayService.IsHoliday(now, 1))
                    {
                        resultTraders = _traderService.GetWeekDelayedSync();

                        if (resultTraders.Value != null && resultTraders.Value.Count() > 0)
                        {
                            resultTraders.Value = resultTraders.Value.Take(totalNextRun);
                        }
                    }
                    else if (IsMidnightCei())
                    {
                        resultTraders = _traderService.GetYesterdayDelayedSync();

                        if (resultTraders.Value != null && resultTraders.Value.Count() > 0)
                        {
                            resultTraders.Value = resultTraders.Value.Take(totalNextRun);
                        }
                    }
                    else
                    {
                        resultTraders = _traderService.GetTodayDelayedSync();

                        if (totalNextRun > 65)
                        {
                            resultTraders.Value = resultTraders.Value.Take(65);
                        }
                    }
                }
            }

            if (resultTraders.Value != null && resultTraders.Value.Count() > 0)
            {
                foreach (Trader trader in resultTraders.Value)
                {
                    using (_uow.Create())
                    {
                        ImportStocksAndTesouroDiretoCeiBase(trader.Identifier, trader.Password, trader.IdUser, true, true);
                    }
                }
            }
        }

        public void RunScrapyAgent(string agentName, int amountItems)
        {
            ResultServiceObject<IEnumerable<ScrapyAgent>> resultScrapyAgent = new ResultServiceObject<IEnumerable<ScrapyAgent>>();

            using (_uow.Create())
            {
                resultScrapyAgent = _scrapyAgentService.GetAll();
            }

            if (resultScrapyAgent.Value != null && resultScrapyAgent.Value.Count() > 0)
            {
                ResultServiceObject<int> resultServiceCountScrapy = new ResultServiceObject<int>();

                using (_uow.Create())
                {
                    resultServiceCountScrapy = _scrapySchedulerService.CountJobsRunningOrAwaiting(agentName);
                }

                int totalPerAgent = resultServiceCountScrapy.Value;
                int totalNextRun = amountItems - totalPerAgent;

                if (totalNextRun > 0)
                {
                    if (!IsMidnightCei() && totalNextRun > 50)
                    {
                        totalNextRun = 50;
                        resultScrapyAgent.Value = resultScrapyAgent.Value.Take(totalNextRun);
                    }

                    if (resultScrapyAgent.Value != null && resultScrapyAgent.Value.Count() > 0)
                    {
                        foreach (ScrapyAgent scrapyAgent in resultScrapyAgent.Value)
                        {
                            string eventDate = string.Empty;
                            DateTime? lastEventDate = null;
                            ResultServiceObject<Trader> resultTrader = new ResultServiceObject<Trader>();

                            using (_uow.Create())
                            {
                                resultTrader = _traderService.GetById(scrapyAgent.IdTrader);
                                lastEventDate = _operationItemService.GetLastEventDate(resultTrader.Value.IdUser, resultTrader.Value.Identifier, resultTrader.Value.Password, TraderTypeEnum.RendaVariavelAndTesouroDiretoCEI);
                            }

                            if (resultTrader.Value != null)
                            {
                                DateTime now = DateTime.Now;

                                if (lastEventDate.HasValue)
                                {
                                    if (lastEventDate.Value.Date < now.Date && lastEventDate.Value.Date < resultTrader.Value.LastSync)
                                    {
                                        lastEventDate = lastEventDate.Value.AddDays(1);
                                    }

                                    eventDate = lastEventDate.Value.ToString("dd/MM/yyyy");
                                }

                                ImportCeiResult importCeiResult = _iImportB3Helper.ImportCei(resultTrader.Value.Identifier, resultTrader.Value.Password, resultTrader.Value.IdUser, true, eventDate, null);
                                ImportCeiResultView importCeiResultView = ConvertCeiResult(importCeiResult);

                                using (_uow.Create())
                                {
                                    importCeiResultView = _portfolioService.FinishImportCei(importCeiResult.Identifier, importCeiResult.Password, importCeiResult.IdUser, importCeiResult.AutomaticProcess, importCeiResultView, _traderService, _cipherService, _stockService, _systemSettingsService, _portfolioPerformanceService, _operationService, _performanceStockService, _holidayService, _operationHistService, _operationItemHistService, _logger, _operationItemService, _portfolioService, _dividendService, _dividendTypeService, _financialProductService, _deviceService, _settingsService, _notificationHistoricalService, _cacheService, _notificationService, _dividendCalendarService, _stockSplitService);

                                    ScrapyScheduler scrapyScheduler = new ScrapyScheduler();
                                    scrapyScheduler.Identifier = resultTrader.Value.Identifier;
                                    scrapyScheduler.Password = resultTrader.Value.Password;
                                    scrapyScheduler.IdUser = resultTrader.Value.IdUser;
                                    scrapyScheduler.AutomaticImport = true;
                                    scrapyScheduler.Priority = (int)PriorityEnum.Normal;
                                    scrapyScheduler.ScrapyAgentRun = true;
                                    scrapyScheduler.CreatedDate = now;
                                    scrapyScheduler.StartDate = now;
                                    scrapyScheduler.Status = (int)ScrapySchedulerStatusEnum.Completed;
                                    scrapyScheduler.FinishDate = DateTime.Now;
                                    scrapyScheduler.ExecutionTime = DateTime.Now.Date.Add(scrapyScheduler.FinishDate.Value.Subtract(scrapyScheduler.StartDate.Value));
                                    scrapyScheduler.Results = importCeiResult.Json;
                                    scrapyScheduler.Sent = true;
                                    scrapyScheduler.TimedOut = false;
                                    scrapyScheduler.Agent = agentName;

                                    _scrapySchedulerService.CreateFromAgent(scrapyScheduler);

                                    _scrapyAgentService.Delete(scrapyAgent);
                                }
                            }

                        }
                    }
                }
            }
        }

        private static bool IsMidnightCei()
        {
            bool zeroPrice = false;
            DateTime timeUtc = DateTime.UtcNow;
            var brasilia = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            DateTime dtBrasilia = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, brasilia);

            TimeSpan start = new TimeSpan(2, 3, 0);
            TimeSpan end = new TimeSpan(6, 30, 0);
            TimeSpan now = dtBrasilia.TimeOfDay;

            if ((now > start) && (now < end))
            {
                zeroPrice = true;
            }

            return zeroPrice;
        }

        public async Task<ResultResponseObject<TraderVM>> ImportFromAvenue(Avenue2FARequest avenue2FARequest)
        {
            ResultResponseObject<TraderVM> resultResponseObject = new ResultResponseObject<TraderVM>() { Success = false };

            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("PUT"), "https://api-integrations.dividendos.me/BrokerIntegration/avenue-2fa-internal"))
                {
                    request.Headers.TryAddWithoutValidation("Authorization", string.Concat("Bearer ", _globalAuthenticationService.GetCurrentAccessToken()));

                    request.Content = new StringContent(JsonConvert.SerializeObject(avenue2FARequest));
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                    var response = await httpClient.SendAsync(request);

                    resultResponseObject = JsonConvert.DeserializeObject<ResultResponseObject<TraderVM>>(response.Content.ReadAsStringAsync().Result);


                    if (response.IsSuccessStatusCode)
                    {
                        resultResponseObject.Success = true;
                    }
                    else
                    {
                        resultResponseObject.Success = false;
                    }
                }
            }

            return resultResponseObject;
        }

        public async Task<ResultResponseObject<TraderVM>> ImportFromAvenueInternal(Avenue2FARequest avenue2FARequest)
        {
            return await ImportFromAvenue(avenue2FARequest.Email, avenue2FARequest.Password, avenue2FARequest.Token, _globalAuthenticationService.IdUser, avenue2FARequest.Challenge, avenue2FARequest.SessionId);
        }

        public void ImportAllLogos()
        {
            ResultServiceObject<IEnumerable<Logo>> resultService = new ResultServiceObject<IEnumerable<Logo>>();
            for (int i = 8; i < 1000; i++)
            {
                using (_uow.Create())
                {
                    resultService = _logoService.GetAllWithPage(i);
                }


                if (resultService.Value.Count() > 0)
                {
                    foreach (var item in resultService.Value)
                    {
                        if (item.LogoImage.Contains("data:image/jpeg;base64,"))
                        {
                            item.URL = _iS3Service.PutImage(Convert.FromBase64String(item.LogoImage.Replace("data:image/jpeg;base64,", string.Empty)), string.Concat(item.IdLogo, ".jpeg")).Result;
                        }
                        else if (item.LogoImage.Contains("data:image/gif;base64,"))
                        {
                            item.URL = _iS3Service.PutImage(Convert.FromBase64String(item.LogoImage.Replace("data:image/gif;base64,", string.Empty)), string.Concat(item.IdLogo, ".gif")).Result;
                        }
                        else if (item.LogoImage.Contains("data:image/png;base64,"))
                        {
                            item.URL = _iS3Service.PutImage(Convert.FromBase64String(item.LogoImage.Replace("data:image/png;base64,", string.Empty)), string.Concat(item.IdLogo, ".png")).Result;
                        }

                        using (_uow.Create())
                        {
                            _logoService.Updte(item);
                        }
                    }
                }
                else
                {
                    return;
                }
            }
        }

        public async Task<ResultResponseObject<TraderVM>> ImportFromAvenue(string email, string password, string token, string idUser, string challenge, string sessiondId)
        {
            ResultResponseObject<TraderVM> resultResponseObject = new ResultResponseObject<TraderVM>() { Success = false };
            ScrapyScheduler scrapyScheduler = null;
            string exceptionMessage = string.Empty;

            try
            {
                DateTime? lastEventDate = null;
                bool getContactDetails = false;
                bool getContactPhone = false;
                ContactDetails contactDetailsDb = null;
                ResultServiceObject<IEnumerable<ContactPhone>> resultContactPhone = null;

                using (_uow.Create())
                {
                    lastEventDate = _operationItemService.GetLastEventDate(idUser, email, password, TraderTypeEnum.Avenue);
                    contactDetailsDb = _contactDetailsService.GetByIdSourceInfoAndIdUser((int)SourceInfoEnum.Avenue, idUser).Value;
                    resultContactPhone = _contactPhoneService.GetAllByIdSourceInfoAndIdUser((int)SourceInfoEnum.Avenue, idUser);
                }

                if (contactDetailsDb == null)
                {
                    getContactDetails = true;
                }

                if (resultContactPhone.Value == null || resultContactPhone.Value.Count() == 0)
                {
                    getContactDetails = true;
                    getContactPhone = true;
                }

                using (_uow.Create())
                {
                    scrapyScheduler = new ScrapyScheduler();
                    scrapyScheduler.Agent = "Avenue";
                    scrapyScheduler.AutomaticImport = true;
                    scrapyScheduler.CreatedDate = DateTime.Now;
                    scrapyScheduler.ExecutionTime = null;
                    scrapyScheduler.FinishDate = null;
                    scrapyScheduler.Identifier = email;
                    scrapyScheduler.Password = password;
                    scrapyScheduler.IdUser = idUser;
                    scrapyScheduler.Priority = 1;
                    scrapyScheduler.Sent = true;
                    scrapyScheduler.StartDate = scrapyScheduler.CreatedDate;
                    scrapyScheduler.Status = (int)ScrapySchedulerStatusEnum.Running;
                    scrapyScheduler.TimedOut = false;
                    scrapyScheduler.WaitingTime = scrapyScheduler.CreatedDate;
                    scrapyScheduler.IdTraderType = (long)TraderTypeEnum.Avenue;
                    _scrapySchedulerService.AddBrokerLog(scrapyScheduler);
                }

                ImportAvenueResult importAvenueResult = await _avenueHelper.ImportAvenue(email, password, token, challenge, sessiondId, lastEventDate, getContactDetails);


                using (_uow.Create())
                {
                    if (scrapyScheduler != null)
                    {
                        scrapyScheduler.Results = importAvenueResult.ApiResult;
                        scrapyScheduler.FinishDate = DateTime.Now;
                        scrapyScheduler.ExecutionTime = DateTime.Now.Date.Add(scrapyScheduler.FinishDate.Value.Subtract(scrapyScheduler.StartDate.Value));
                        scrapyScheduler.Status = (int)ScrapySchedulerStatusEnum.Completed;
                        _scrapySchedulerService.Update(scrapyScheduler);
                    }
                }

                using (_uow.Create())
                {

                    if (importAvenueResult.Success)
                    {
                        bool newPortfolio = false;
                        List<string> dividendCeiItems = new List<string>();
                        List<string> changedCeiItems = new List<string>();
                        DateTime? lastSync = null;
                        ResultServiceObject<Trader> resultTraderService = new ResultServiceObject<Trader>();
                        Portfolio portfolio = null;

                        if ((importAvenueResult.Orders != null && importAvenueResult.Orders.Count > 0) || (importAvenueResult.Dividends != null && importAvenueResult.Dividends.Count > 0))
                        {
                            DateTime lastSyncOut = DateTime.Now;
                            resultTraderService = _traderService.SaveTrader(email, _cipherService.Encrypt(password), idUser, false, false, TraderTypeEnum.Avenue, out lastSyncOut);
                            lastSync = lastSyncOut;

                            portfolio = _portfolioService.SavePortfolio(resultTraderService.Value, CountryEnum.EUA, "Avenue", out newPortfolio);

                            List<OperationItem> operationItems = new List<OperationItem>();
                            List<Dividend> dividends = new List<Dividend>();

                            ResultServiceObject<IEnumerable<OperationItem>> resultOperationItem = new ResultServiceObject<IEnumerable<OperationItem>>();
                            ResultServiceObject<IEnumerable<Dividend>> resultServiceDividend = new ResultServiceObject<IEnumerable<Dividend>>();
                            ResultServiceObject<IEnumerable<Operation>> resultServiceOperation = new ResultServiceObject<IEnumerable<Operation>>();

                            if (!newPortfolio)
                            {
                                resultOperationItem = _operationItemService.GetActiveByPortfolio(portfolio.IdPortfolio, true);
                                resultServiceDividend = _dividendService.GetAutomaticByIdPortfolio(portfolio.IdPortfolio);
                                resultServiceOperation = _operationService.GetByPortfolio(portfolio.IdPortfolio);
                            }

                            if (importAvenueResult.Orders != null && importAvenueResult.Orders.Count > 0)
                            {
                                foreach (AvenueOrder avenueOrder in importAvenueResult.Orders)
                                {
                                    Stock stock = _stockService.GetBySymbolOrLikeOldSymbol(avenueOrder.Symbol, 2).Value;

                                    if (stock != null)
                                    {
                                        OperationItem operationItem = new OperationItem();
                                        operationItem.EventDate = avenueOrder.EventDate;
                                        operationItem.IdStock = stock.IdStock;
                                        operationItem.AveragePrice = avenueOrder.AveragePrice;
                                        operationItem.IdOperationType = avenueOrder.IdOperationType;
                                        operationItem.NumberOfShares = avenueOrder.NumberOfShares;
                                        operationItem.TransactionId = avenueOrder.TransactionId;
                                        operationItem.HomeBroker = "Avenue";

                                        if (resultOperationItem.Value != null && resultOperationItem.Value.Count() > 0)
                                        {
                                            OperationItem operationItemDb = resultOperationItem.Value.FirstOrDefault(op => op.TransactionId == operationItem.TransactionId);

                                            if (operationItemDb != null)
                                            {
                                                operationItem = operationItemDb;
                                            }
                                        }

                                        if (!newPortfolio && operationItem.IdOperationItem == 0)
                                        {
                                            changedCeiItems.Add(stock.Symbol);
                                        }
                                        
                                        if (operationItem.IdOperationItem == 0)
                                        {
                                            List<OperationItem> operationItemsSplit = new List<OperationItem>();
                                            operationItemsSplit.Add(operationItem);

                                            ResultServiceObject<StockSplit> resultStockSplit = _stockSplitService.GetByIdStock(operationItem.IdStock, operationItem.EventDate.Value);

                                            if (resultStockSplit.Value != null)
                                            {
                                                operationItem.LastSplitDate = operationItem.EventDate.Value;
                                                operationItem.SplitApplied = false;
                                            }
                                            else
                                            {
                                                operationItem.LastSplitDate = DateTime.Now;
                                                operationItem.SplitApplied = true;
                                            }

                                            if (newPortfolio)
                                            {
                                                _stockSplitService.ApplyStockSplit(ref operationItemsSplit, stock.IdCountry);
                                                operationItem = operationItemsSplit.First();
                                            }
                                        }

                                        operationItems.Add(operationItem);
                                    }
                                    else
                                    {
                                        if (char.IsNumber(avenueOrder.Symbol[4]))
                                        {
                                            SendAdminNotification(string.Format("Stock {0} not found", avenueOrder.Symbol));
                                            _ = _logger.SendInformationAsync(new { Message = string.Format("Stock not found {0} : {1} : {2}", avenueOrder.IdOperationType, avenueOrder.Symbol, idUser) });
                                        }
                                    }
                                }
                            }

                            List<Operation> operations = new List<Operation>();


                            if (operationItems != null && operationItems.Count() > 0)
                            {
                                if (resultOperationItem.Value != null && resultOperationItem.Value.Count() > 0)
                                {
                                    operationItems.AddRange(resultOperationItem.Value);
                                }

                                List<OperationItem> operationItemManual = operationItems.Where(op => string.IsNullOrWhiteSpace(op.TransactionId)).ToList();
                                operationItems = operationItems.GroupBy(x => x.TransactionId).Select(x => x.FirstOrDefault()).ToList();

                                operationItems.AddRange(operationItemManual);

                                List<OperationItem> operationItemsSplit = _stockSplitService.ApplyStockSplit(ref operationItems, 2);

                                if (operationItemsSplit != null && operationItemsSplit.Count() > 0)
                                {
                                    foreach (OperationItem operationItemSplit in operationItemsSplit)
                                    {
                                        if (operationItemSplit.IdOperationItem != 0)
                                        {
                                            _operationItemService.Update(operationItemSplit.IdOperationItem, operationItemSplit.NumberOfShares, operationItemSplit.AveragePrice, operationItemSplit.LastSplitDate);
                                        }
                                    }
                                }

                                List<OperationItem> lstStockOperationBuy = operationItems.Where(objStkTmp => objStkTmp.IdOperationType == 1).ToList();

                                if (lstStockOperationBuy != null && lstStockOperationBuy.Count() > 0)
                                {
                                    operations = lstStockOperationBuy.GroupBy(objStockOperationTmp => objStockOperationTmp.IdStock).Select(objStockOperationGp =>
                                    {
                                        Operation operation = new Operation();

                                        if (resultServiceOperation.Value != null && resultServiceOperation.Value.Count() > 0)
                                        {
                                            Operation operationDb = resultServiceOperation.Value.FirstOrDefault(op => op.IdStock == objStockOperationGp.First().IdStock && op.IdOperationType == 1);

                                            if (operationDb != null)
                                            {
                                                operation = operationDb;
                                            }
                                        }

                                        operation.HomeBroker = objStockOperationGp.First().HomeBroker;
                                        operation.IdStock = objStockOperationGp.First().IdStock;

                                        List<OperationItem> operationItemStock = operationItems.FindAll(opItem => opItem.IdStock == operation.IdStock).OrderBy(opItem => opItem.EventDate).ToList();

                                        decimal numberOfSharesCalc = 0;
                                        decimal avgPriceCalc = 0;
                                        bool valid = _operationService.CalculateAveragePrice(ref operationItemStock, out numberOfSharesCalc, out avgPriceCalc, false, null, false);

                                        operation.AveragePrice = avgPriceCalc;
                                        operation.NumberOfShares = numberOfSharesCalc;
                                        operation.IdOperationType = 1;
                                        operation.IdPortfolio = portfolio.IdPortfolio;


                                        return operation;
                                    }).ToList();
                                }

                                List<OperationItem> lstStockOperationSell = operationItems.Where(objStkTmp => objStkTmp.IdOperationType == 2).ToList();

                                if (lstStockOperationSell != null && lstStockOperationSell.Count > 0)
                                {
                                    List<Operation> lstStockOperationSellGrouped = lstStockOperationSell.GroupBy(objStockOperationTmp => objStockOperationTmp.IdStock)
                                                                            .Select(objStockOperationGp =>
                                                                            {
                                                                                Operation operation = new Operation();

                                                                                if (resultServiceOperation.Value != null && resultServiceOperation.Value.Count() > 0)
                                                                                {
                                                                                    Operation operationDb = resultServiceOperation.Value.FirstOrDefault(op => op.IdStock == objStockOperationGp.First().IdStock && op.IdOperationType == 2);

                                                                                    if (operationDb != null)
                                                                                    {
                                                                                        operation = operationDb;
                                                                                    }
                                                                                }

                                                                                operation.HomeBroker = objStockOperationGp.First().HomeBroker;
                                                                                operation.IdStock = objStockOperationGp.First().IdStock;
                                                                                operation.NumberOfShares = objStockOperationGp.Sum(c => c.NumberOfShares);
                                                                                operation.IdPortfolio = portfolio.IdPortfolio;
                                                                                operation.IdOperationType = 2;

                                                                                decimal total = objStockOperationGp.Sum(c => c.NumberOfShares * c.AveragePrice);

                                                                                if (operation.NumberOfShares > 0)
                                                                                {
                                                                                    operation.AveragePrice = total / operation.NumberOfShares;
                                                                                }

                                                                                return operation;
                                                                            }).ToList();

                                    if (lstStockOperationSellGrouped != null && lstStockOperationSellGrouped.Count > 0)
                                    {
                                        operations.AddRange(lstStockOperationSellGrouped);
                                    }
                                }

                            }

                            //TODO: Do not import directly from avenue 
                            //if (dividends != null && dividends.Count > 0)
                            //{
                            //    foreach (Dividend dividend in dividends)
                            //    {
                            //        if (dividend.IdDividend == 0)
                            //        {
                            //            dividend.AutomaticImport = true;
                            //            _dividendService.Insert(dividend);
                            //        }
                            //        else
                            //        {
                            //            _dividendService.Update(dividend);
                            //        }
                            //    }
                            //}

                            if (operations != null && operations.Count() > 0)
                            {
                                foreach (Operation operation in operations)
                                {
                                    if (operation.NumberOfShares > 0)
                                    {
                                        operation.Active = true;
                                    }
                                    else
                                    {
                                        operation.Active = false;
                                    }

                                    if (operation.IdOperation == 0)
                                    {
                                        operation.HomeBroker = "Avenue";
                                        _operationService.Insert(operation);
                                    }
                                    else
                                    {
                                        _operationService.Update(operation);
                                    }
                                }
                            }

                            if (operationItems != null && operationItems.Count > 0)
                            {
                                foreach (OperationItem operationItem in operationItems)
                                {
                                    if (operationItem.IdOperationItem == 0)
                                    {
                                        operationItem.IdOperation = operations.First(op => op.IdStock == operationItem.IdStock && operationItem.IdOperationType == op.IdOperationType).IdOperation;
                                        _operationItemService.Insert(operationItem);
                                    }
                                    //else
                                    //{
                                    //    _operationItemService.Update(operationItem);
                                    //}
                                }
                            }


                            _portfolioService.CalculatePerformance(portfolio.IdPortfolio, _systemSettingsService, _portfolioPerformanceService, _stockService, _operationService, _performanceStockService, _holidayService, 0);
                        }

                        if (resultTraderService.Value == null)
                        {
                            resultTraderService = _traderService.GetByIdentifierAndUserActive(email, idUser, TraderTypeEnum.Avenue);
                        }

                        if (portfolio == null)
                        {
                            if (resultTraderService.Value != null)
                            {
                                portfolio = _portfolioService.GetByTraderActive(resultTraderService.Value.IdTrader).Value;
                            }
                        }

                        if (portfolio != null)
                        {
                            ResultServiceObject<IEnumerable<OperationItem>> resultOperationItem = _operationItemService.GetActiveByPortfolio(portfolio.IdPortfolio, true);

                            if (resultOperationItem.Value != null && resultOperationItem.Value.Count() > 0)
                            {
                                List<OperationItem> operationItems = resultOperationItem.Value.ToList();
                                List<OperationItem> operationItemsSplit = _stockSplitService.ApplyStockSplit(ref operationItems, 2);

                                if (operationItemsSplit != null && operationItemsSplit.Count() > 0)
                                {
                                    foreach (OperationItem operationItemSplit in operationItemsSplit)
                                    {
                                        _operationItemService.Update(operationItemSplit.IdOperationItem, operationItemSplit.NumberOfShares, operationItemSplit.AveragePrice, operationItemSplit.LastSplitDate);
                                    }

                                    _portfolioService.CalculatePerformance(portfolio.IdPortfolio, _systemSettingsService, _portfolioPerformanceService, _stockService, _operationService, _performanceStockService, _holidayService, 0);
                                }
                            }

                            //_dividendService.RemoveDuplicated(portfolio.IdPortfolio, _dividendCalendarService)
                            if (newPortfolio)
                            {
                                lastSync = null;
                            }
                            else if (!lastSync.HasValue)
                            {
                                lastSync = resultTraderService.Value.LastSync;
                            }

                            List<string> divs = _dividendService.RestorePastDividends(portfolio.IdPortfolio, 2, _operationItemService, _dividendCalendarService, _operationService, _stockService, _performanceStockService, _portfolioService, lastSync);
                            dividendCeiItems.AddRange(divs);

                            _traderService.UpdateSyncData(resultTraderService.Value.IdTrader, DateTime.Now);
                        }

                        if (importAvenueResult.ContactDetails != null && getContactDetails)
                        {
                            ContactDetails contactDetails = new ContactDetails();
                            contactDetails.AddressNumber = importAvenueResult.ContactDetails.AddressNumber;
                            contactDetails.AdressType = importAvenueResult.ContactDetails.AdressType;
                            contactDetails.BankDepositAmount = importAvenueResult.ContactDetails.BankDepositAmount;
                            contactDetails.BirthCity = importAvenueResult.ContactDetails.BirthCity;
                            contactDetails.BirthDate = importAvenueResult.ContactDetails.BirthDate;
                            contactDetails.City = importAvenueResult.ContactDetails.City;
                            contactDetails.CompanyDocumentNumber = importAvenueResult.ContactDetails.CompanyDocumentNumber;
                            contactDetails.CompanyName = importAvenueResult.ContactDetails.CompanyName;
                            contactDetails.Complement = importAvenueResult.ContactDetails.Complement;
                            contactDetails.DocumentNumber = importAvenueResult.ContactDetails.DocumentNumber;
                            contactDetails.Email = importAvenueResult.ContactDetails.Email;
                            contactDetails.Gender = importAvenueResult.ContactDetails.Gender;
                            contactDetails.IdSourceInfo = (int)SourceInfoEnum.Avenue;
                            contactDetails.IdUser = idUser;
                            contactDetails.MonthlyIncome = importAvenueResult.ContactDetails.MonthlyIncome;
                            contactDetails.MotherName = importAvenueResult.ContactDetails.MotherName;
                            contactDetails.Name = importAvenueResult.ContactDetails.Name;
                            contactDetails.Neighborhood = importAvenueResult.ContactDetails.Neighborhood;
                            contactDetails.OcupationDesc = importAvenueResult.ContactDetails.OcupationDesc;
                            contactDetails.PatrimonialTotalAmount = importAvenueResult.ContactDetails.PatrimonialTotalAmount;
                            contactDetails.PostalCode = importAvenueResult.ContactDetails.PostalCode;
                            contactDetails.SpouseDocumentNumber = importAvenueResult.ContactDetails.SpouseDocumentNumber;
                            contactDetails.SpouseName = importAvenueResult.ContactDetails.SpouseName;
                            contactDetails.StateCode = importAvenueResult.ContactDetails.StateCode;
                            contactDetails.StreetName = importAvenueResult.ContactDetails.StreetName;

                            _contactDetailsService.Insert(contactDetails);
                        }

                        if (importAvenueResult.ContactPhones != null && importAvenueResult.ContactPhones.Count > 0 && getContactPhone)
                        {
                            foreach (AvenueContactPhone nuInvestContactPhone in importAvenueResult.ContactPhones)
                            {
                                ContactPhone contactPhone = new ContactPhone();
                                contactPhone.IdUser = idUser;
                                contactPhone.IdSourceInfo = (int)SourceInfoEnum.Avenue;
                                contactPhone.AreaCode = nuInvestContactPhone.AreaCode;
                                contactPhone.CountryCode = nuInvestContactPhone.CountryCode;
                                contactPhone.PhoneNumber = nuInvestContactPhone.PhoneNumber;

                                _contactPhoneService.Insert(contactPhone);
                            }
                        }


                        _portfolioService.SendNotificationImportation(false, idUser, true, string.Empty, _deviceService, _notificationHistoricalService, _cacheService, _notificationService, _logger);

                        if (!newPortfolio && changedCeiItems != null && changedCeiItems.Count > 0)
                        {
                            changedCeiItems = changedCeiItems.Distinct().ToList();
                            changedCeiItems = changedCeiItems.OrderBy(stk => stk).ToList();
                            string stocks = string.Join(", ", changedCeiItems);
                            _portfolioService.SendNotificationNewItensOnPortfolio(idUser, false, stocks, _settingsService, _deviceService, _notificationHistoricalService, _cacheService, _notificationService, _logger, resultTraderService.Value.ShowOnPatrimony);
                        }

                        if (!newPortfolio && dividendCeiItems != null && dividendCeiItems.Count > 0)
                        {
                            dividendCeiItems = dividendCeiItems.Distinct().ToList();
                            dividendCeiItems = dividendCeiItems.OrderBy(stk => stk).ToList();
                            string stocks = string.Join(", ", dividendCeiItems);
                            _portfolioService.SendNotificationNewItensOnPortfolio(idUser, true, stocks, _settingsService, _deviceService, _notificationHistoricalService, _cacheService, _notificationService, _logger, resultTraderService.Value.ShowOnPatrimony);
                        }

                        resultResponseObject = new ResultResponseObject<TraderVM>() { Success = true };
                    }
                    else
                    {
                        resultResponseObject = new ResultResponseObject<TraderVM>() { Success = false };
                        resultResponseObject.ErrorMessages.Add("Código do token inválido");

                        _portfolioService.SendNotificationImportation(false, idUser, false, "Código do token inválido", _deviceService, _notificationHistoricalService, _cacheService, _notificationService, _logger);
                    }
                }

            }
            catch (Exception ex)
            {
                exceptionMessage = string.Format("AvenueException {0} : {1} : {2} : {3}", ex.Message, ex.InnerException, ex.StackTrace, idUser);
                _ = _logger.SendInformationAsync(new { Message = string.Format("AvenueException {0} : {1} : {2} : {3}", ex.Message, ex.InnerException, ex.StackTrace, idUser) });
            }

            if (!string.IsNullOrWhiteSpace(exceptionMessage))
            {
                using (_uow.Create())
                {
                    if (scrapyScheduler != null)
                    {
                        scrapyScheduler.Results = exceptionMessage;
                        _scrapySchedulerService.Update(scrapyScheduler);
                    }
                }
            }

            return resultResponseObject;
        }

        public ResultResponseObject<List<PortfolioModel>> GetPortfoliosAndSubPortfoliosV4(bool includeCryptoWallets)
        {
            return GetPortfoliosAndSubPortfoliosBase(includeCryptoWallets, true);
        }

        public ResultResponseObject<TraderVM> ActivateEnqueue(string documentNumber)
        {
            ResultResponseObject<TraderVM> resultReponse = new ResultResponseObject<TraderVM>();
            Trader trader = null;

            using (_uow.Create())
            {
                trader = _traderService.GetByIdentifierAndUser(documentNumber, _globalAuthenticationService.IdUser, TraderTypeEnum.RendaVariavelAndTesouroDiretoNewCEI).Value;

                if (trader != null)
                {
                    trader.Active = true;
                    trader = _traderService.Update(trader).Value;
                }
            }

            if (trader != null)
            {
                ForceSync(API.Model.Response.v7.TraderType.RendaVariavelAndTesouroDiretoNewCEI, trader.GuidTrader);

                TraderVM traderVM = new TraderVM();
                traderVM.Active = trader.Active;
                traderVM.GuidTrader = trader.GuidTrader;
                traderVM.Identifier = trader.Identifier;
                traderVM.Password = trader.Password;

                resultReponse.Value = traderVM;
                resultReponse.Success = true;
            }

            return resultReponse;
        }

        public void SendAdminNotification(string message)
        {
            ResultServiceObject<IEnumerable<Device>> devices = _deviceService.GetAdminDevices();

            foreach (Device itemDevice in devices.Value)
            {
                try
                {
                    _notificationService.SendPush("Falha na integração", message, itemDevice, null);
                }
                catch (Exception ex)
                {
                    _ = _logger.SendErrorAsync(ex);
                }
            }
        }
    }
}
