using AutoMapper;
using Dividendos.Application.Base;
using Dividendos.Application.Interface;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using Dividendos.Passfolio.Interface;
using Dividendos.API.Model.Response.Common;
using Dividendos.API.Model.Request.BrokerIntegration;
using Dividendos.API.Model.Response.BrokerIntegration;
using Dividendos.Avenue.Interface;
using System.Threading.Tasks;
using Dividendos.Avenue.Interface.Model;
using Dividendos.Toro.Interface;
using Dividendos.Toro.Interface.Model;
using Dividendos.Entity.Entities;
using System.Collections.Generic;
using System.Linq;
using Dividendos.API.Model.Response;
using Dividendos.Entity.Enum;
using Dividendos.Entity.Model;
using System;
using K.Logger;
using Dividendos.NuInvest.Interface;
using Dividendos.NuInvest.Interface.Model;
using Dividendos.Xp.Interface;
using Dividendos.InvestidorB3.Interface;
using Dividendos.API.Model.Response.v1.PortalInvestidorB3;
using Dividendos.Xp.Interface.Model;
using Dividendos.InvestidorB3.Interface.Model.Response.UpdatedProduct;
using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json;
using Dividendos.Application.Interface.Model;
using Dividendos.Rico.Interface;
using Dividendos.Rico.Interface.Model;
using Dividendos.Clear.Interface;
using Dividendos.Clear.Interface.Model;
using System.Globalization;
using Dividendos.API.Model.Request.Operation;

namespace Dividendos.Application
{
    public class BrokerIntegrationApp : BaseApp, IBrokerIntegrationApp
    {
        private readonly IPassfolioHelper _passfolioHelper;
        private readonly IAvenueHelper _avenueHelper;
        private readonly IToroHelper _toroHelper;
        private readonly IDividendService _dividendService;
        private readonly IOperationService _operationService;
        private readonly IOperationItemService _operationItemService;
        private readonly IUnitOfWork _uow;
        private readonly ILogger _logger;
        private readonly ICipherService _cipherService;
        private readonly IScrapySchedulerService _scrapySchedulerService;
        private readonly ITraderService _traderService;
        private readonly IStockService _stockService;
        private readonly IPortfolioService _portfolioService;
        private readonly IDeviceService _deviceService;
        private readonly ISettingsService _settingsService;
        private readonly INotificationService _notificationService;
        private readonly INotificationHistoricalService _notificationHistoricalService;
        private readonly ICacheService _cacheService;
        private readonly IGlobalAuthenticationService _globalAuthenticationService;
        private readonly IFinancialProductService _financialProductService;
        private readonly IDividendCalendarService _dividendCalendarService;
        private readonly IPerformanceStockService _performanceStockService;
        private readonly INuInvestHelper _nuInvestHelper;
        private readonly IXpHelper _xpHelper;
        private readonly IImportInvestidorB3Helper _iImportInvestidorB3Helper;
        private readonly IDividendTypeService _dividendTypeService;
        private readonly ISubscriptionService _subscriptionService;
        private readonly IHolidayService _holidayService;
        private readonly IContactDetailsService _contactDetailsService;
        private readonly IContactPhoneService _contactPhoneService;
        private readonly IRicoHelper _ricoHelper;
        private readonly IClearHelper _clearHelper;
        private readonly IOperationHistService _operationHistService;
        private readonly IOperationItemHistService _operationItemHistService;
        private readonly ISystemSettingsService _systemSettingsService;
        private readonly IPortfolioPerformanceService _portfolioPerformanceService;

        public BrokerIntegrationApp(IPassfolioHelper passfolioHelper, IAvenueHelper avenueHelper, IToroHelper toroHelper,
            IDividendService dividendService,
            IOperationService operationService,
            IOperationItemService operationItemService,
            IUnitOfWork uow,
            ILogger logger,
            ICipherService cipherService,
            IScrapySchedulerService scrapySchedulerService,
            ITraderService traderService,
            IStockService stockService,
            IPortfolioService portfolioService,
            IDeviceService deviceService,
            ISettingsService settingsService,
            INotificationService notificationService,
            INotificationHistoricalService notificationHistoricalService,
            ICacheService cacheService,
            IGlobalAuthenticationService globalAuthenticationService,
            IDividendCalendarService dividendCalendarService,
            IFinancialProductService financialProductService,
            IPerformanceStockService performanceStockService,
            INuInvestHelper nuInvestHelper,
            IXpHelper xpHelper,
            IImportInvestidorB3Helper iImportInvestidorB3Helper,
            IDividendTypeService dividendTypeService,
            ISubscriptionService subscriptionService,
            IHolidayService holidayService,
            IContactDetailsService contactDetailsService,
            IContactPhoneService contactPhoneService,
            IRicoHelper ricoHelper,
            IClearHelper clearHelper,
            IOperationHistService operationHistService,
            IOperationItemHistService operationItemHistService,
            ISystemSettingsService systemSettingsService,
            IPortfolioPerformanceService portfolioPerformanceService)
        {
            _passfolioHelper = passfolioHelper;
            _avenueHelper = avenueHelper;
            _toroHelper = toroHelper;
            _dividendService = dividendService;
            _operationService = operationService;
            _operationItemService = operationItemService;
            _uow = uow;
            _logger = logger;
            _cipherService = cipherService;
            _scrapySchedulerService = scrapySchedulerService;
            _traderService = traderService;
            _stockService = stockService;
            _portfolioService = portfolioService;
            _deviceService = deviceService;
            _settingsService = settingsService;
            _notificationService = notificationService;
            _notificationHistoricalService = notificationHistoricalService;
            _cacheService = cacheService;
            _globalAuthenticationService = globalAuthenticationService;
            _financialProductService = financialProductService;
            _dividendCalendarService = dividendCalendarService;
            _performanceStockService = performanceStockService;
            _nuInvestHelper = nuInvestHelper;
            _xpHelper = xpHelper;
            _iImportInvestidorB3Helper = iImportInvestidorB3Helper;
            _dividendTypeService = dividendTypeService;
            _subscriptionService = subscriptionService;
            _holidayService = holidayService;
            _contactDetailsService = contactDetailsService;
            _contactPhoneService = contactPhoneService;
            _ricoHelper = ricoHelper;
            _clearHelper = clearHelper;
            _operationHistService = operationHistService;
            _operationItemHistService = operationItemHistService;
            _systemSettingsService = systemSettingsService;
            _portfolioPerformanceService = portfolioPerformanceService;
        }

        public void TesteInvestidorB3()
        {
            var token = _iImportInvestidorB3Helper.GetAutorizationToken();
            _iImportInvestidorB3Helper.Healthcheck(token);
            _iImportInvestidorB3Helper.PositionEquities(token, "31171035896", DateTime.Now.AddDays(-5), 1);
            _iImportInvestidorB3Helper.UdpateProduct(token, InvestidorB3.Interface.Model.Request.Product.AssetsTrading, DateTime.Now.AddDays(-5), DateTime.Now.AddDays(-2), 1);
        }

        public ResultResponseObject<PassfolioAddResponse> AuthenticateOnPassfolio(PassfolioAddRequest passfolioAddRequest)
        {
            ResultResponseObject<PassfolioAddResponse> resultResponseObject = new ResultResponseObject<PassfolioAddResponse>() { Success = false };

            string auth = _passfolioHelper.Session(passfolioAddRequest.Email, passfolioAddRequest.Password);

            if (!string.IsNullOrEmpty(auth))
            {
                var authenticators = _passfolioHelper.Authenticators(auth);

                _passfolioHelper.SendCode(auth, authenticators.id);

                _ = _logger.SendInformationAsync(new { UserID = _globalAuthenticationService.IdUser, Content = JsonConvert.SerializeObject(passfolioAddRequest) });

                PassfolioAddResponse passfolioAddResponse = new PassfolioAddResponse() { Auth = auth, AuthenticatorID = authenticators.id, Email = passfolioAddRequest.Email };

                resultResponseObject = new ResultResponseObject<PassfolioAddResponse>() { Success = true, Value = passfolioAddResponse };
            }
            else
            {
                resultResponseObject = new ResultResponseObject<PassfolioAddResponse>() { Success = false };
                resultResponseObject.ErrorMessages.Add("Suas credenciais de acesso a Passfolio estão incorretas. Verifique as informações e tente novamente.");
            }

            return resultResponseObject;
        }

        public async Task<ResultResponseObject<AvenueAddResponse>> AuthenticateOnAvenue(AvenueAddRequest avenueAddRequest)
        {
            _logger.SendDebugAsync(avenueAddRequest);

            ResultResponseObject<AvenueAddResponse> resultResponseObject = new ResultResponseObject<AvenueAddResponse>() { Success = false };

            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://api-integrations.dividendos.me/BrokerIntegration/avenue-internal"))
                {
                    request.Headers.TryAddWithoutValidation("Authorization", string.Concat("Bearer ", _globalAuthenticationService.GetCurrentAccessToken()));

                    request.Content = new StringContent(JsonConvert.SerializeObject(avenueAddRequest));
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                    var response = await httpClient.SendAsync(request);

                    _logger.SendDebugAsync(response);

                    resultResponseObject = JsonConvert.DeserializeObject<ResultResponseObject<AvenueAddResponse>>(response.Content.ReadAsStringAsync().Result);


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

        public async Task<ResultResponseObject<AvenueAddResponse>> AuthenticateOnAvenueInternal(AvenueAddRequest avenueAddRequest)
        {
            ResultResponseObject<AvenueAddResponse> resultResponseObject = new ResultResponseObject<AvenueAddResponse>() { Success = false };

            ImportAvenueResult importAvenueResult = await _avenueHelper.ValidateUser(avenueAddRequest.Email, avenueAddRequest.Password);

            if (importAvenueResult.Success)
            {
                AvenueAddResponse avenueAddResponse = new AvenueAddResponse() { Auth = avenueAddRequest.Password, SessionId = importAvenueResult.Session, Email = avenueAddRequest.Email, Challenge = importAvenueResult.Challenge };

                resultResponseObject = new ResultResponseObject<AvenueAddResponse>() { Success = true, Value = avenueAddResponse };
            }
            else
            {
                resultResponseObject = new ResultResponseObject<AvenueAddResponse>() { Success = false };
                resultResponseObject.ErrorMessages.Add("Suas credenciais de acesso a Avenue estão incorretas. Verifique as informações e tente novamente.");
            }

            return resultResponseObject;
        }

        public ResultResponseObject<ToroAddResponse> AuthenticateOnToro(ToroAddRequest toroAddRequest)
        {
            ResultResponseObject<ToroAddResponse> resultResponseObject = new ResultResponseObject<ToroAddResponse>() { Success = false };
            ScrapyScheduler scrapyScheduler = null;

            using (_uow.Create())
            {
                scrapyScheduler = new ScrapyScheduler();
                scrapyScheduler.Agent = "Toro";
                scrapyScheduler.AutomaticImport = true;
                scrapyScheduler.CreatedDate = DateTime.Now;
                scrapyScheduler.ExecutionTime = scrapyScheduler.CreatedDate;
                scrapyScheduler.FinishDate = scrapyScheduler.CreatedDate;
                scrapyScheduler.Identifier = toroAddRequest.Email;
                scrapyScheduler.Password = toroAddRequest.Password;
                scrapyScheduler.IdUser = string.IsNullOrWhiteSpace(toroAddRequest.IdUser) ? _globalAuthenticationService.IdUser : toroAddRequest.IdUser;
                scrapyScheduler.Priority = 1;
                scrapyScheduler.Sent = true;
                scrapyScheduler.StartDate = scrapyScheduler.CreatedDate;
                scrapyScheduler.Status = (int)ScrapySchedulerStatusEnum.Completed;
                scrapyScheduler.TimedOut = false;
                scrapyScheduler.WaitingTime = scrapyScheduler.CreatedDate;
                scrapyScheduler.IdTraderType = (long)TraderTypeEnum.Toro;
                _scrapySchedulerService.AddBrokerLog(scrapyScheduler);
            }

            ImportToroResult importToroResult = _toroHelper.ValidateUser(toroAddRequest.Email, toroAddRequest.Password, toroAddRequest.Token);

            if (importToroResult.Success)
            {

                ToroAddResponse toroAddResponse = new ToroAddResponse { Auth = toroAddRequest.Password, Email = toroAddRequest.Email };

                resultResponseObject = new ResultResponseObject<ToroAddResponse>() { Success = true, Value = toroAddResponse };
            }
            else
            {
                resultResponseObject = new ResultResponseObject<ToroAddResponse>() { Success = false };
                resultResponseObject.ErrorMessages.Add("Suas credenciais de acesso a Toro estão incorretas. Verifique as informações e tente novamente.");
            }

            using (_uow.Create())
            {
                if (scrapyScheduler != null)
                {
                    scrapyScheduler.Status = (int)ScrapySchedulerStatusEnum.Completed;
                    scrapyScheduler.Results = importToroResult.ApiResult;
                    _scrapySchedulerService.Update(scrapyScheduler);
                }
            }

            return resultResponseObject;
        }


        public ResultResponseObject<TraderVM> ImportFromToro(ToroAddRequest toroAddRequest)
        {
            return ImportFromToro(toroAddRequest.Email, toroAddRequest.Password, toroAddRequest.Token, _globalAuthenticationService.IdUser);
        }

        public ResultResponseObject<TraderVM> ImportFromToro(string email, string password, string token, string idUser)
        {
            ResultResponseObject<TraderVM> resultResponseObject = new ResultResponseObject<TraderVM>() { Success = false };
            ScrapyScheduler scrapyScheduler = null;
            string exceptionMessage = string.Empty;

            try
            {
                using (_uow.Create())
                {
                    scrapyScheduler = new ScrapyScheduler();
                    scrapyScheduler.Agent = "Toro";
                    scrapyScheduler.AutomaticImport = true;
                    scrapyScheduler.CreatedDate = DateTime.Now;
                    scrapyScheduler.ExecutionTime = scrapyScheduler.CreatedDate;
                    scrapyScheduler.FinishDate = scrapyScheduler.CreatedDate;
                    scrapyScheduler.Identifier = email;
                    scrapyScheduler.Password = password;
                    scrapyScheduler.IdUser = idUser;
                    scrapyScheduler.Priority = 1;
                    scrapyScheduler.Sent = true;
                    scrapyScheduler.StartDate = scrapyScheduler.CreatedDate;
                    scrapyScheduler.Status = (int)ScrapySchedulerStatusEnum.Completed;
                    scrapyScheduler.TimedOut = false;
                    scrapyScheduler.WaitingTime = scrapyScheduler.CreatedDate;
                    scrapyScheduler.IdTraderType = (long)TraderTypeEnum.Toro;
                    _scrapySchedulerService.AddBrokerLog(scrapyScheduler);
                }


                DateTime? lastEventDate = null;

                using (_uow.Create())
                {
                    lastEventDate = _operationItemService.GetLastEventDate(idUser, email, password, TraderTypeEnum.Toro);
                }

                ImportToroResult importToroResult = _toroHelper.ImportToro(email, password, lastEventDate, token);


                using (_uow.Create())
                {
                    if (scrapyScheduler != null)
                    {
                        scrapyScheduler.Status = (int)ScrapySchedulerStatusEnum.Completed;
                        scrapyScheduler.Results = importToroResult.ApiResult;
                        _scrapySchedulerService.Update(scrapyScheduler);
                    }
                }

                using (_uow.Create())
                {

                    if (importToroResult.Success)
                    {
                        bool newPortfolio = false;
                        List<string> dividendCeiItems = new List<string>();
                        List<string> changedCeiItems = new List<string>();
                        ResultServiceObject<Trader> resultTraderService = new ResultServiceObject<Trader>();
                        Portfolio portfolio = null;
                        DateTime? lastSync = null;

                        if ((importToroResult.Orders != null && importToroResult.Orders.Count > 0) || (importToroResult.Dividends != null && importToroResult.Dividends.Count > 0))
                        {
                            DateTime lastSyncOut = DateTime.Now;
                            resultTraderService = _traderService.SaveTrader(email, _cipherService.Encrypt(password), idUser, false, false, TraderTypeEnum.Toro, out lastSyncOut);
                            lastSync = lastSyncOut;

                            portfolio = _portfolioService.SavePortfolio(resultTraderService.Value, CountryEnum.Brazil, "Toro", out newPortfolio);

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

                            if (importToroResult.Orders != null && importToroResult.Orders.Count > 0)
                            {
                                foreach (ToroOrder toroOrder in importToroResult.Orders)
                                {
                                    Stock stock = _stockService.GetBySymbolOrLikeOldSymbol(toroOrder.Symbol, 1).Value;

                                    if (stock != null)
                                    {
                                        OperationItem operationItem = new OperationItem();
                                        operationItem.EventDate = toroOrder.EventDate;
                                        operationItem.IdStock = stock.IdStock;
                                        operationItem.AveragePrice = toroOrder.AveragePrice;
                                        operationItem.IdOperationType = toroOrder.IdOperationType;
                                        operationItem.NumberOfShares = toroOrder.NumberOfShares;
                                        operationItem.TransactionId = toroOrder.TransactionId;
                                        operationItem.HomeBroker = "Toro";

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

                                        operationItems.Add(operationItem);
                                    }
                                    else
                                    {
                                        _ = _logger.SendInformationAsync(new { Message = string.Format("Stock not found {0} : {1} : {2}", toroOrder.IdOperationType, toroOrder.Symbol, idUser) });
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

                                operations = operationItems.GroupBy(objStockOperationTmp => objStockOperationTmp.IdStock).Select(objStockOperationGp =>
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

                            if (dividends != null && dividends.Count > 0)
                            {
                                foreach (Dividend dividend in dividends)
                                {
                                    if (dividend.IdDividend == 0)
                                    {
                                        dividend.AutomaticImport = true;
                                        _dividendService.Insert(dividend);
                                    }
                                    else
                                    {
                                        _dividendService.Update(dividend);
                                    }
                                }
                            }

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
                                        operation.HomeBroker = "Toro";
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
                        }

                        if (resultTraderService.Value == null)
                        {
                            resultTraderService = _traderService.GetByIdentifierAndUserActive(email, idUser, TraderTypeEnum.Toro);

                            if (resultTraderService.Value == null)
                            {
                                DateTime outLastSync = DateTime.Now;
                                resultTraderService = _traderService.SaveTrader(email, _cipherService.Encrypt(password), idUser, false, false, TraderTypeEnum.Toro, out outLastSync);
                                lastSync = outLastSync;
                            }
                        }

                        if (portfolio == null)
                        {
                            if (resultTraderService.Value != null)
                            {
                                portfolio = _portfolioService.GetByTraderActive(resultTraderService.Value.IdTrader).Value;
                            }
                        }

                        if (importToroResult.Bonds != null && importToroResult.Bonds.Count() > 0)
                        {
                            List<Entity.Views.TesouroDiretoImportView> tesouroDiretoImportViews = new List<Entity.Views.TesouroDiretoImportView>();

                            foreach (var itemBonds in importToroResult.Bonds)
                            {
                                if (itemBonds.Issuer.Equals("Secretaria do Tesouro Nacional"))
                                {
                                    tesouroDiretoImportViews.Add(new Entity.Views.TesouroDiretoImportView() { Broker = itemBonds.Issuer, NetValue = itemBonds.Value, Symbol = itemBonds.Name });
                                }
                            }

                            _portfolioService.SaveTesouroDireto(tesouroDiretoImportViews, resultTraderService.Value.IdTrader, _financialProductService);

                            List<Entity.Views.TesouroDiretoImportView> cdbImportViews = new List<Entity.Views.TesouroDiretoImportView>();

                            foreach (var itemBonds in importToroResult.Bonds)
                            {
                                if (!itemBonds.Issuer.Equals("Secretaria do Tesouro Nacional"))
                                {
                                    cdbImportViews.Add(new Entity.Views.TesouroDiretoImportView() { Broker = itemBonds.Issuer, NetValue = itemBonds.Value, Symbol = itemBonds.Name });
                                }
                            }


                            _portfolioService.SaveCDB(cdbImportViews, resultTraderService.Value.IdTrader, _financialProductService);
                        }

                        if (importToroResult.Funds != null && importToroResult.Funds.Count() > 0)
                        {
                            List<Entity.Views.TesouroDiretoImportView> fundsImportViews = new List<Entity.Views.TesouroDiretoImportView>();

                            foreach (var itemFunds in importToroResult.Funds)
                            {
                                fundsImportViews.Add(new Entity.Views.TesouroDiretoImportView() { Broker = "TORO CTVM LTDA.", NetValue = itemFunds.Value, Symbol = itemFunds.Name });
                            }

                            _portfolioService.SaveFunds(fundsImportViews, resultTraderService.Value.IdTrader, _financialProductService);
                        }

                        if (portfolio != null)
                        {
                            if (newPortfolio)
                            {
                                lastSync = null;
                            }
                            else if (!lastSync.HasValue)
                            {
                                lastSync = resultTraderService.Value.LastSync;
                            }

                            List<string> divs = _dividendService.RestorePastDividends(portfolio.IdPortfolio, 1, _operationItemService, _dividendCalendarService, _operationService, _stockService, _performanceStockService, _portfolioService, lastSync);
                            dividendCeiItems.AddRange(divs);
                            _traderService.UpdateSyncData(resultTraderService.Value.IdTrader, DateTime.Now);
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
                exceptionMessage = string.Format("ToroException {0} : {1} : {2} : {3}", ex.Message, ex.InnerException, ex.StackTrace, idUser);
                _ = _logger.SendInformationAsync(new { Message = string.Format("ToroException {0} : {1} : {2} : {3}", ex.Message, ex.InnerException, ex.StackTrace, idUser) });
            }


            if (!string.IsNullOrWhiteSpace(exceptionMessage))
            {
                using (_uow.Create())
                {
                    if (scrapyScheduler != null)
                    {
                        scrapyScheduler.Status = (int)ScrapySchedulerStatusEnum.Canceled;
                        scrapyScheduler.Results = exceptionMessage;
                        _scrapySchedulerService.Update(scrapyScheduler);
                    }
                }
            }

            return resultResponseObject;
        }

        public ResultResponseObject<TraderVM> ImportFromNuInvest(NuInvestAddRequest nuInvestAddRequest)
        {
            ResultResponseObject<TraderVM> resultResponseObject = new ResultResponseObject<TraderVM>() { Success = false };

            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://api-integrations.dividendos.me/BrokerIntegration/nuinvest-internal"))
                {
                    request.Headers.TryAddWithoutValidation("Authorization", string.Concat("Bearer ", _globalAuthenticationService.GetCurrentAccessToken()));

                    request.Content = new StringContent(JsonConvert.SerializeObject(nuInvestAddRequest));
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                    var response = httpClient.SendAsync(request).Result;

                    resultResponseObject = JsonConvert.DeserializeObject<ResultResponseObject<TraderVM>>(response.Content.ReadAsStringAsync().Result);

                    if (response.IsSuccessStatusCode)
                    {
                        resultResponseObject.Success = true;
                    }
                }
            }

            return resultResponseObject;
        }
        
        public ResultResponseObject<TraderVM> ImportFromNuInvestInternal(NuInvestAddRequest nuInvestAddRequest)
        {
            return ImportFromNuInvest(_globalAuthenticationService.IdUser, nuInvestAddRequest.Identifier, nuInvestAddRequest.Password, nuInvestAddRequest.Token);
        }

        public ResultResponseObject<Autorize> PortalInvestidorB3AutorizeUser(string document)
        {
            ResultResponseObject<Autorize> resultResponseObject = new ResultResponseObject<Autorize>();

            //por enquanto não usaremos o documento cpf para nada. No futuro, quando a página estiver recebendo essa informação, poderemos concatenar a url antes de devolver para o front.   
            resultResponseObject.Success = true;
            resultResponseObject.Value = new Autorize() { Document = document, URL = _iImportInvestidorB3Helper.GetURLAuthB3() };

            return resultResponseObject;
        }

        public ResultResponseObject<TraderVM> ImportFromNuInvest(string idUser, string identifier, string password, string token)
        {
            ResultResponseObject<TraderVM> resultResponseObject = new ResultResponseObject<TraderVM>() { Success = false };
            ScrapyScheduler scrapyScheduler = null;
            string exceptionMessage = string.Empty;

            try
            {
                DateTime? lastEventDate = null;

                using (_uow.Create())
                {
                    DateTime now = DateTime.Now;
                    lastEventDate = _operationItemService.GetLastEventDate(idUser, identifier, password, TraderTypeEnum.NuInvest);

                    if (lastEventDate.HasValue)
                    {
                        ResultServiceObject<Trader> resultTrader = _traderService.GetByIdentifierAndUserActive(identifier, idUser, TraderTypeEnum.NuInvest);

                        if (lastEventDate.Value.Date < now.Date && lastEventDate.Value.Date < resultTrader.Value.LastSync.Date)
                        {
                            lastEventDate = lastEventDate.Value.AddDays(1);
                        }
                    }
                }

                using (_uow.Create())
                {
                    scrapyScheduler = new ScrapyScheduler();
                    scrapyScheduler.Agent = "NuInvest";
                    scrapyScheduler.AutomaticImport = true;
                    scrapyScheduler.CreatedDate = DateTime.Now;
                    scrapyScheduler.ExecutionTime = scrapyScheduler.CreatedDate;
                    scrapyScheduler.FinishDate = scrapyScheduler.CreatedDate;
                    scrapyScheduler.Identifier = identifier;
                    scrapyScheduler.Password = password;
                    scrapyScheduler.IdUser = idUser;
                    scrapyScheduler.Priority = 1;
                    scrapyScheduler.Sent = true;
                    scrapyScheduler.StartDate = scrapyScheduler.CreatedDate;
                    scrapyScheduler.Status = (int)ScrapySchedulerStatusEnum.Running;
                    scrapyScheduler.TimedOut = false;
                    scrapyScheduler.WaitingTime = scrapyScheduler.CreatedDate;
                    scrapyScheduler.IdTraderType = (long)TraderTypeEnum.NuInvest;
                    _scrapySchedulerService.AddBrokerLog(scrapyScheduler);
                }

                bool getContactDetails = false;
                bool getContactPhone = false;
                ContactDetails contactDetailsDb = null;
                ResultServiceObject<IEnumerable<ContactPhone>> resultContactPhone = null;

                using (_uow.Create())
                {
                    contactDetailsDb = _contactDetailsService.GetByIdSourceInfoAndIdUser((int)SourceInfoEnum.NuInvest, idUser).Value;
                    resultContactPhone = _contactPhoneService.GetAllByIdSourceInfoAndIdUser((int)SourceInfoEnum.NuInvest, idUser);
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

                ImportNuInvestResult importNuInvestResult = _nuInvestHelper.ImportFromNuInvest(identifier, password, token, lastEventDate, getContactDetails);

                using (_uow.Create())
                {
                    if (scrapyScheduler != null)
                    {
                        scrapyScheduler.ResponseBody = importNuInvestResult.ResponseBody;
                        scrapyScheduler.Status = (int)ScrapySchedulerStatusEnum.Completed;
                        scrapyScheduler.Results = importNuInvestResult.ApiResult;
                        _scrapySchedulerService.Update(scrapyScheduler);
                    }
                }

                using (_uow.Create())
                {
                    if (importNuInvestResult.Success)
                    {
                        bool newPortfolio = false;
                        List<string> dividendCeiItems = new List<string>();
                        List<string> changedCeiItems = new List<string>();
                        ResultServiceObject<Trader> resultTraderService = new ResultServiceObject<Trader>();
                        Portfolio portfolio = null;
                        DateTime? lastSync = null;

                        if ((importNuInvestResult.Orders != null && importNuInvestResult.Orders.Count > 0))
                        {
                            importNuInvestResult.Orders = importNuInvestResult.Orders.OrderBy(ord => ord.EventDate).ThenBy(ord => ord.IdOperationType).ToList();
                            DateTime lastSyncOut = DateTime.Now;
                            resultTraderService = _traderService.SaveTrader(identifier, _cipherService.Encrypt(password), idUser, false, false, TraderTypeEnum.NuInvest, out lastSyncOut);
                            lastSync = lastSyncOut;

                            portfolio = _portfolioService.SavePortfolio(resultTraderService.Value, CountryEnum.Brazil, "NuInvest", out newPortfolio);

                            List<OperationItem> operationItems = new List<OperationItem>();
                            List<Dividend> dividends = new List<Dividend>();

                            ResultServiceObject<IEnumerable<OperationItem>> resultOperationItem = new ResultServiceObject<IEnumerable<OperationItem>>();
                            ResultServiceObject<IEnumerable<Dividend>> resultServiceDividend = new ResultServiceObject<IEnumerable<Dividend>>();
                            ResultServiceObject<IEnumerable<Operation>> resultServiceOperation = new ResultServiceObject<IEnumerable<Operation>>();

                            if (!newPortfolio)
                            {
                                if (lastEventDate.HasValue)
                                {
                                    _operationItemService.DeleteFromDate(portfolio.IdPortfolio, lastEventDate.Value.Date);
                                }

                                resultOperationItem = _operationItemService.GetActiveByPortfolio(portfolio.IdPortfolio, true);
                                resultServiceDividend = _dividendService.GetAutomaticByIdPortfolio(portfolio.IdPortfolio);
                                resultServiceOperation = _operationService.GetByPortfolio(portfolio.IdPortfolio);
                            }

                            if (importNuInvestResult.Orders != null && importNuInvestResult.Orders.Count > 0)
                            {
                                foreach (NuInvestOrder nuInvestOrder in importNuInvestResult.Orders)
                                {
                                    Stock stock = _stockService.GetBySymbolOrLikeOldSymbol(nuInvestOrder.Symbol, 1).Value;

                                    if (stock != null)
                                    {
                                        bool exists = false;

                                        if (!string.IsNullOrWhiteSpace(nuInvestOrder.TransactionId))
                                        {
                                            if (resultOperationItem.Value != null && resultOperationItem.Value.Count() > 0)
                                            {
                                                exists = resultOperationItem.Value.Any(ord => ord.TransactionId == nuInvestOrder.TransactionId);
                                            }
                                        }

                                        if (!exists)
                                        {
                                            OperationItem operationItem = new OperationItem();
                                            operationItem.EventDate = nuInvestOrder.EventDate;
                                            operationItem.IdStock = stock.IdStock;
                                            operationItem.AveragePrice = nuInvestOrder.AveragePrice;
                                            operationItem.IdOperationType = nuInvestOrder.IdOperationType;
                                            operationItem.NumberOfShares = nuInvestOrder.NumberOfShares;
                                            operationItem.TransactionId = nuInvestOrder.TransactionId;
                                            operationItem.HomeBroker = "NuInvest";

                                            if (!newPortfolio && operationItem.IdOperationItem == 0)
                                            {
                                                changedCeiItems.Add(stock.Symbol);
                                            }

                                            operationItems.Add(operationItem);
                                        }
                                    }
                                    else
                                    {
                                        _ = _logger.SendInformationAsync(new { Message = string.Format("Stock not found {0} : {1} : {2}", nuInvestOrder.IdOperationType, nuInvestOrder.Symbol, idUser) });
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


                                if (importNuInvestResult.OrdersAvgPrice != null && importNuInvestResult.OrdersAvgPrice.Count > 0)
                                {
                                    foreach (NuInvestOrder nuInvestOrder in importNuInvestResult.OrdersAvgPrice)
                                    {
                                        Stock stock = _stockService.GetBySymbolOrLikeOldSymbol(nuInvestOrder.Symbol, 1).Value;

                                        if (stock != null)
                                        {
                                            bool found = operationItems.Exists(ord => ord.IdStock == stock.IdStock && ord.IdOperationType == 1);

                                            if (!found)
                                            {
                                                OperationItem operationItem = new OperationItem();
                                                operationItem.EventDate = nuInvestOrder.EventDate;
                                                operationItem.IdStock = stock.IdStock;
                                                operationItem.AveragePrice = nuInvestOrder.AveragePrice;
                                                operationItem.IdOperationType = nuInvestOrder.IdOperationType;
                                                operationItem.NumberOfShares = nuInvestOrder.NumberOfShares;
                                                operationItem.TransactionId = nuInvestOrder.TransactionId;
                                                operationItem.HomeBroker = "NuInvest";

                                                if (!newPortfolio && operationItem.IdOperationItem == 0)
                                                {
                                                    changedCeiItems.Add(stock.Symbol);
                                                }

                                                operationItems.Add(operationItem);
                                            }
                                        }
                                    }

                                }

                                //List<OperationItem> operationItemManual = operationItems.Where(op => string.IsNullOrWhiteSpace(op.TransactionId)).ToList();
                                //operationItems = operationItems.GroupBy(x => x.TransactionId).Select(x => x.FirstOrDefault()).ToList();

                                //operationItems.AddRange(operationItemManual);

                                operations = operationItems.GroupBy(objStockOperationTmp => objStockOperationTmp.IdStock).Select(objStockOperationGp =>
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

                            if (dividends != null && dividends.Count > 0)
                            {
                                foreach (Dividend dividend in dividends)
                                {
                                    if (dividend.IdDividend == 0)
                                    {
                                        dividend.AutomaticImport = true;
                                        _dividendService.Insert(dividend);
                                    }
                                    else
                                    {
                                        _dividendService.Update(dividend);
                                    }
                                }
                            }

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
                                        operation.HomeBroker = "NuInvest";
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
                        }

                        if (resultTraderService.Value == null)
                        {
                            resultTraderService = _traderService.GetByIdentifierAndUserActive(identifier, idUser, TraderTypeEnum.NuInvest);

                            if (resultTraderService.Value == null)
                            {
                                DateTime outLastSync = DateTime.Now;
                                resultTraderService = _traderService.SaveTrader(identifier, _cipherService.Encrypt(password), idUser, false, false, TraderTypeEnum.NuInvest, out outLastSync);
                                lastSync = outLastSync;
                            }
                        }

                        if (portfolio == null)
                        {
                            if (resultTraderService.Value != null)
                            {
                                portfolio = _portfolioService.GetByTraderActive(resultTraderService.Value.IdTrader).Value;
                            }
                        }

                        if (importNuInvestResult.Bonds != null && importNuInvestResult.Bonds.Count() > 0)
                        {
                            List<Entity.Views.TesouroDiretoImportView> tesouroDiretoImportViews = new List<Entity.Views.TesouroDiretoImportView>();

                            foreach (var itemBonds in importNuInvestResult.Bonds)
                            {
                                if (itemBonds.InvestmenType.Equals("TD"))
                                {
                                    tesouroDiretoImportViews.Add(new Entity.Views.TesouroDiretoImportView() { Broker = "Nu Invest", NetValue = itemBonds.Value, Symbol = itemBonds.Issuer });
                                }
                            }

                            _portfolioService.SaveTesouroDireto(tesouroDiretoImportViews, resultTraderService.Value.IdTrader, _financialProductService);

                            List<Entity.Views.TesouroDiretoImportView> cdbImportViews = new List<Entity.Views.TesouroDiretoImportView>();

                            foreach (var itemBonds in importNuInvestResult.Bonds)
                            {
                                if (itemBonds.InvestmenType.Equals("CDB"))
                                {
                                    string name = string.Format("{0}: {1}", itemBonds.Issuer, itemBonds.Name);

                                    cdbImportViews.Add(new Entity.Views.TesouroDiretoImportView() { Broker = "Nu Invest", NetValue = itemBonds.Value, Symbol = name });
                                }
                            }

                            _portfolioService.SaveCDB(cdbImportViews, resultTraderService.Value.IdTrader, _financialProductService);

                            List<Entity.Views.TesouroDiretoImportView> debenturesImportViews = new List<Entity.Views.TesouroDiretoImportView>();

                            foreach (var itemBonds in importNuInvestResult.Bonds)
                            {
                                if (itemBonds.InvestmenType.Equals("DEB"))
                                {
                                    string name = string.Format("{0}: {1}", itemBonds.Issuer, itemBonds.Name);

                                    debenturesImportViews.Add(new Entity.Views.TesouroDiretoImportView() { Broker = "Nu Invest", NetValue = itemBonds.Value, Symbol = name });
                                }
                            }

                            _portfolioService.SaveDebentures(debenturesImportViews, resultTraderService.Value.IdTrader, _financialProductService);
                        }

                        if (portfolio != null)
                        {
                            if (newPortfolio)
                            {
                                lastSync = null;
                            }
                            else if (!lastSync.HasValue)
                            {
                                lastSync = resultTraderService.Value.LastSync;
                            }

                            List<string> divs = _dividendService.RestorePastDividends(portfolio.IdPortfolio, 1, _operationItemService, _dividendCalendarService, _operationService, _stockService, _performanceStockService, _portfolioService, lastSync);
                            dividendCeiItems.AddRange(divs);
                            _traderService.UpdateSyncData(resultTraderService.Value.IdTrader, DateTime.Now);
                        }


                        if (importNuInvestResult.ContactDetails != null && getContactDetails)
                        {
                            ContactDetails contactDetails = new ContactDetails();
                            contactDetails.AddressNumber = importNuInvestResult.ContactDetails.AddressNumber;
                            contactDetails.AdressType = importNuInvestResult.ContactDetails.AdressType;
                            contactDetails.BankDepositAmount = importNuInvestResult.ContactDetails.BankDepositAmount;
                            contactDetails.BirthCity = importNuInvestResult.ContactDetails.BirthCity;
                            contactDetails.BirthDate = importNuInvestResult.ContactDetails.BirthDate;
                            contactDetails.City = importNuInvestResult.ContactDetails.City;
                            contactDetails.CompanyDocumentNumber = importNuInvestResult.ContactDetails.CompanyDocumentNumber;
                            contactDetails.CompanyName = importNuInvestResult.ContactDetails.CompanyName;
                            contactDetails.Complement = importNuInvestResult.ContactDetails.Complement;
                            contactDetails.DocumentNumber = importNuInvestResult.ContactDetails.DocumentNumber;
                            contactDetails.Email = importNuInvestResult.ContactDetails.Email;
                            contactDetails.Gender = importNuInvestResult.ContactDetails.Gender;
                            contactDetails.IdSourceInfo = (int)SourceInfoEnum.NuInvest;
                            contactDetails.IdUser = idUser;
                            contactDetails.MonthlyIncome = importNuInvestResult.ContactDetails.MonthlyIncome;
                            contactDetails.MotherName = importNuInvestResult.ContactDetails.MotherName;
                            contactDetails.Name = importNuInvestResult.ContactDetails.Name;
                            contactDetails.Neighborhood = importNuInvestResult.ContactDetails.Neighborhood;
                            contactDetails.OcupationDesc = importNuInvestResult.ContactDetails.OcupationDesc;
                            contactDetails.PatrimonialTotalAmount = importNuInvestResult.ContactDetails.PatrimonialTotalAmount;
                            contactDetails.PostalCode = importNuInvestResult.ContactDetails.PostalCode;
                            contactDetails.SpouseDocumentNumber = importNuInvestResult.ContactDetails.SpouseDocumentNumber;
                            contactDetails.SpouseName = importNuInvestResult.ContactDetails.SpouseName;
                            contactDetails.StateCode = importNuInvestResult.ContactDetails.StateCode;
                            contactDetails.StreetName = importNuInvestResult.ContactDetails.StreetName;

                            _contactDetailsService.Insert(contactDetails);
                        }

                        if (importNuInvestResult.ContactPhones != null && importNuInvestResult.ContactPhones.Count > 0 && getContactPhone)
                        {
                            foreach (NuInvestContactPhone nuInvestContactPhone in importNuInvestResult.ContactPhones)
                            {
                                ContactPhone contactPhone = new ContactPhone();
                                contactPhone.IdUser = idUser;
                                contactPhone.IdSourceInfo = (int)SourceInfoEnum.NuInvest;
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
                        resultResponseObject.ErrorMessages.Add(importNuInvestResult.Message);

                        _portfolioService.SendNotificationImportation(false, idUser, false, importNuInvestResult.Message, _deviceService, _notificationHistoricalService, _cacheService, _notificationService, _logger);
                    }
                }

                if (importNuInvestResult.Warnings != null && importNuInvestResult.Warnings.Count > 0)
                {
                    using (_uow.Create())
                    {
                        SendAdminNotification(string.Format("Identifier: {0} NuInvest Examples: {1} ", identifier, string.Join(", ", importNuInvestResult.Warnings.ToArray())));
                    }
                }

            }
            catch (Exception ex)
            {
                exceptionMessage = string.Format("NuInvestException {0} : {1} : {2} : {3}", ex.Message, ex.InnerException, ex.StackTrace, idUser);
                _ = _logger.SendInformationAsync(new { Message = string.Format("NuInvestException {0} : {1} : {2} : {3}", ex.Message, ex.InnerException, ex.StackTrace, idUser) });
            }

            if (!string.IsNullOrWhiteSpace(exceptionMessage))
            {
                using (_uow.Create())
                {
                    if (scrapyScheduler != null)
                    {
                        scrapyScheduler.Status = (int)ScrapySchedulerStatusEnum.Canceled;
                        scrapyScheduler.Results = exceptionMessage;
                        _scrapySchedulerService.Update(scrapyScheduler);
                    }
                }
            }

            return resultResponseObject;
        }

        public async Task<ResultResponseObject<TraderVM>> ImportFromXpAsync(XpAddRequest xpAddRequest)
        {
            ResultResponseObject<TraderVM> resultResponseObject = new ResultResponseObject<TraderVM>() { Success = false };

            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://api-integrations.dividendos.me/BrokerIntegration/xp-internal"))
                {
                    request.Headers.TryAddWithoutValidation("Authorization", string.Concat("Bearer ", _globalAuthenticationService.GetCurrentAccessToken()));

                    request.Content = new StringContent(JsonConvert.SerializeObject(xpAddRequest));
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                    var response = httpClient.SendAsync(request).Result;

                    resultResponseObject = JsonConvert.DeserializeObject<ResultResponseObject<TraderVM>>(response.Content.ReadAsStringAsync().Result);

                    if (response.IsSuccessStatusCode)
                    {
                        resultResponseObject.Success = true;
                    }
                }
            }

            return resultResponseObject;
        }

        public async Task<ResultResponseObject<TraderVM>> ImportFromXpInternalAsync(XpAddRequest xpAddRequest)
        {
            ResultResponseObject<TraderVM> resultResponseObject = await ImportFromXptAsync(_globalAuthenticationService.IdUser, xpAddRequest.Identifier, xpAddRequest.Password, xpAddRequest.Token);

            return resultResponseObject;
        }

        public async Task<ResultResponseObject<B3Platform>> CheckB3Platform()
        {
            ResultResponseObject<B3Platform> resultResponseObject = new ResultResponseObject<B3Platform>() { Success = true, Value = new B3Platform() { NewB3Enabe = true, LegacyCEIEnable = true } };

            return resultResponseObject;
        }

        public async Task<ResultResponseObject<TraderVM>> ImportFromXptAsync(string idUser, string account, string password, string xpToken)
        {
            ResultResponseObject<TraderVM> resultResponseObject = new ResultResponseObject<TraderVM>() { Success = false };
            ScrapyScheduler scrapyScheduler = null;
            string exceptionMessage = string.Empty;

            try
            {
                using (_uow.Create())
                {
                    scrapyScheduler = new ScrapyScheduler();
                    scrapyScheduler.Agent = "Xp";
                    scrapyScheduler.AutomaticImport = true;
                    scrapyScheduler.CreatedDate = DateTime.Now;
                    scrapyScheduler.ExecutionTime = scrapyScheduler.CreatedDate;
                    scrapyScheduler.FinishDate = scrapyScheduler.CreatedDate;
                    scrapyScheduler.Identifier = account;
                    scrapyScheduler.Password = password;
                    scrapyScheduler.IdUser = idUser;
                    scrapyScheduler.Priority = 1;
                    scrapyScheduler.Sent = true;
                    scrapyScheduler.StartDate = scrapyScheduler.CreatedDate;
                    scrapyScheduler.Status = (int)ScrapySchedulerStatusEnum.Completed;
                    scrapyScheduler.TimedOut = false;
                    scrapyScheduler.WaitingTime = scrapyScheduler.CreatedDate;
                    scrapyScheduler.IdTraderType = (long)TraderTypeEnum.Xp;
                    _scrapySchedulerService.AddBrokerLog(scrapyScheduler);
                }

                ImportXpResult importXpResult = await _xpHelper.ImportFromXptAsync(account, password, xpToken);

                using (_uow.Create())
                {
                    if (scrapyScheduler != null)
                    {
                        scrapyScheduler.ResponseBody = importXpResult.ResponseBody;
                        scrapyScheduler.Status = (int)ScrapySchedulerStatusEnum.Completed;
                        scrapyScheduler.Results = importXpResult.ApiResult;
                        _scrapySchedulerService.Update(scrapyScheduler);
                    }
                }

                using (_uow.Create())
                {
                    if (importXpResult.Success)
                    {
                        bool newPortfolio = false;
                        List<string> dividendCeiItems = new List<string>();
                        List<string> changedCeiItems = new List<string>();
                        ResultServiceObject<Trader> resultTraderService = new ResultServiceObject<Trader>();
                        Portfolio portfolio = null;
                        DateTime? lastSync = null;

                        if ((importXpResult.Orders != null && importXpResult.Orders.Count > 0))
                        {
                            importXpResult.Orders = importXpResult.Orders.OrderBy(ord => ord.EventDate).ThenBy(ord => ord.IdOperationType).ToList();
                            DateTime lastSyncOut = DateTime.Now;
                            resultTraderService = _traderService.SaveTrader(account, _cipherService.Encrypt(password), idUser, false, false, TraderTypeEnum.Xp, out lastSyncOut);
                            lastSync = lastSyncOut;

                            portfolio = _portfolioService.SavePortfolio(resultTraderService.Value, CountryEnum.Brazil, "Xp", out newPortfolio);

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


                            foreach (XpOrder xpOrder in importXpResult.Orders)
                            {
                                Stock stock = _stockService.GetBySymbolOrLikeOldSymbol(xpOrder.Symbol, 1).Value;

                                if (stock != null)
                                {
                                    OperationItem operationItem = new OperationItem();
                                    operationItem.EventDate = xpOrder.EventDate;
                                    operationItem.IdStock = stock.IdStock;
                                    operationItem.AveragePrice = xpOrder.AveragePrice;
                                    operationItem.IdOperationType = xpOrder.IdOperationType;
                                    operationItem.NumberOfShares = xpOrder.NumberOfShares;
                                    operationItem.TransactionId = xpOrder.TransactionId;
                                    operationItem.HomeBroker = "Xp";

                                    if (resultOperationItem.Value != null && resultOperationItem.Value.Count() > 0)
                                    {
                                        OperationItem operationItemDb = resultOperationItem.Value.FirstOrDefault(op => op.IdOperationType == xpOrder.IdOperationType && op.IdStock == stock.IdStock && !op.EditedByUser);

                                        if (operationItemDb != null)
                                        {
                                            operationItem = operationItemDb;
                                        }
                                    }

                                    if (!newPortfolio && operationItem.IdOperationItem == 0)
                                    {
                                        changedCeiItems.Add(stock.Symbol);
                                    }

                                    operationItems.Add(operationItem);
                                }
                                else
                                {
                                    _ = _logger.SendInformationAsync(new { Message = string.Format("Stock not found {0} : {1} : {2}", xpOrder.IdOperationType, xpOrder.Symbol, idUser) });
                                }
                            }

                            List<Operation> operations = new List<Operation>();

                            if (operationItems != null && operationItems.Count() > 0)
                            {
                                //if (resultOperationItem.Value != null && resultOperationItem.Value.Count() > 0)
                                //{
                                //    operationItems.AddRange(resultOperationItem.Value);
                                //}

                                //List<OperationItem> operationItemManual = operationItems.Where(op => string.IsNullOrWhiteSpace(op.TransactionId)).ToList();
                                //operationItems = operationItems.GroupBy(x => x.TransactionId).Select(x => x.FirstOrDefault()).ToList();

                                //operationItems.AddRange(operationItemManual);

                                operations = operationItems.GroupBy(objStockOperationTmp => objStockOperationTmp.IdStock).Select(objStockOperationGp =>
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

                            if (dividends != null && dividends.Count > 0)
                            {
                                foreach (Dividend dividend in dividends)
                                {
                                    if (dividend.IdDividend == 0)
                                    {
                                        dividend.AutomaticImport = true;
                                        _dividendService.Insert(dividend);
                                    }
                                    else
                                    {
                                        _dividendService.Update(dividend);
                                    }
                                }
                            }

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
                                        operation.HomeBroker = "Xp";
                                        _operationService.Insert(operation);
                                    }
                                    else
                                    {
                                        _operationService.Update(operation);
                                    }
                                }
                            }

                            if (resultServiceOperation.Value != null && resultServiceOperation.Value.Count() > 0)
                            {
                                foreach (Operation operation in resultServiceOperation.Value)
                                {
                                    Stock stock = _stockService.GetById(operation.IdStock).Value;

                                    if (stock != null)
                                    {
                                        XpOrder xpOrder = importXpResult.Orders.Where(op => op.Symbol == stock.Symbol).FirstOrDefault();

                                        if (xpOrder == null)
                                        {
                                            operation.Active = false;
                                            _operationService.Update(operation);
                                        }
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
                        }

                        if (importXpResult.Dividends != null && importXpResult.Dividends.Count > 0)
                        {
                            DateTime lastSyncOut = DateTime.Now;
                            resultTraderService = _traderService.SaveTrader(account, _cipherService.Encrypt(password), idUser, false, false, TraderTypeEnum.Xp, out lastSyncOut);
                            IEnumerable<DividendType> resultServiceDividendType = _dividendTypeService.GetAll().Value;
                            ResultServiceObject<IEnumerable<Dividend>> resultServiceDividend = new ResultServiceObject<IEnumerable<Dividend>>();

                            if (!newPortfolio)
                            {
                                resultServiceDividend = _dividendService.GetAutomaticByIdPortfolio(portfolio.IdPortfolio);
                            }

                            foreach (XpDividend xpDividend in importXpResult.Dividends)
                            {
                                Stock stock = _stockService.GetBySymbolOrLikeOldSymbol(xpDividend.Symbol, 1).Value;

                                if (stock != null)
                                {
                                    Dividend dividend = new Dividend();
                                    dividend.NetValue = xpDividend.NetValue;
                                    dividend.HomeBroker = "Xp";
                                    dividend.PaymentDate = xpDividend.EventDate;
                                    dividend.IdStock = stock.IdStock;
                                    dividend.IdPortfolio = portfolio.IdPortfolio;

                                    DividendType dividendType = resultServiceDividendType.FirstOrDefault(divType => divType.Name.ToLower().Contains(xpDividend.Type.ToLower()));

                                    if (dividendType == null)
                                    {
                                        dividend.IdDividendType = (int)DividendTypeEnum.Dividend;
                                    }
                                    else
                                    {
                                        dividend.IdDividendType = dividendType.IdDividendType;
                                    }

                                    if (resultServiceDividend.Value != null && resultServiceDividend.Value.Count() > 0)
                                    {
                                        Dividend dividendDb = resultServiceDividend.Value.FirstOrDefault(div => div.TransactionId == dividend.TransactionId || (div.PaymentDate.HasValue && div.IdStock == dividend.IdStock && div.IdDividendType == dividend.IdDividendType && div.PaymentDate.Value.Date == dividend.PaymentDate.Value.Date));

                                        if (dividendDb != null)
                                        {
                                            dividend = dividendDb;
                                        }
                                    }

                                    if (!newPortfolio && dividend.IdDividend == 0)
                                    {
                                        dividendCeiItems.Add(stock.Symbol);

                                        dividend.AutomaticImport = true;
                                        _dividendService.Insert(dividend);
                                    }
                                }
                                else
                                {
                                    _ = _logger.SendInformationAsync(new { Message = string.Format("Stock not found {0} : {1}", xpDividend.Symbol, idUser) });
                                }
                            }
                        }

                        if (resultTraderService.Value == null)
                        {
                            resultTraderService = _traderService.GetByIdentifierAndUserActive(account, idUser, TraderTypeEnum.NuInvest);

                            if (resultTraderService.Value == null)
                            {
                                DateTime outLastSync = DateTime.Now;
                                resultTraderService = _traderService.SaveTrader(account, _cipherService.Encrypt(password), idUser, false, false, TraderTypeEnum.Xp, out outLastSync);
                                lastSync = outLastSync;
                            }
                        }

                        if (portfolio == null)
                        {
                            if (resultTraderService.Value != null)
                            {
                                portfolio = _portfolioService.GetByTraderActive(resultTraderService.Value.IdTrader).Value;
                            }
                        }

                        if (importXpResult.Bonds != null && importXpResult.Bonds.Count() > 0)
                        {
                            List<Entity.Views.TesouroDiretoImportView> tesouroDiretoImportViews = new List<Entity.Views.TesouroDiretoImportView>();

                            foreach (var itemBonds in importXpResult.Bonds)
                            {
                                if (itemBonds.Issuer.Equals("Secretaria do Tesouro Nacional"))
                                {
                                    tesouroDiretoImportViews.Add(new Entity.Views.TesouroDiretoImportView() { Broker = itemBonds.Issuer, NetValue = itemBonds.Value, Symbol = itemBonds.Name });
                                }
                            }

                            _portfolioService.SaveTesouroDireto(tesouroDiretoImportViews, resultTraderService.Value.IdTrader, _financialProductService);

                            List<Entity.Views.TesouroDiretoImportView> cdbImportViews = new List<Entity.Views.TesouroDiretoImportView>();

                            foreach (var itemBonds in importXpResult.Bonds)
                            {
                                if (!itemBonds.Issuer.Equals("Secretaria do Tesouro Nacional"))
                                {
                                    cdbImportViews.Add(new Entity.Views.TesouroDiretoImportView() { Broker = itemBonds.Issuer, NetValue = itemBonds.Value, Symbol = itemBonds.Name });
                                }
                            }


                            _portfolioService.SaveCDB(cdbImportViews, resultTraderService.Value.IdTrader, _financialProductService);
                        }

                        if (importXpResult.Funds != null && importXpResult.Funds.Count() > 0)
                        {
                            List<Entity.Views.TesouroDiretoImportView> fundsImportViews = new List<Entity.Views.TesouroDiretoImportView>();

                            foreach (var itemFunds in importXpResult.Funds)
                            {
                                fundsImportViews.Add(new Entity.Views.TesouroDiretoImportView() { Broker = "XP INVESTIMENTOS", NetValue = itemFunds.Value, Symbol = itemFunds.Name });
                            }

                            _portfolioService.SaveFunds(fundsImportViews, resultTraderService.Value.IdTrader, _financialProductService);
                        }



                        if (portfolio != null)
                        {
                            if (newPortfolio)
                            {
                                lastSync = null;
                            }
                            else if (!lastSync.HasValue)
                            {
                                lastSync = resultTraderService.Value.LastSync;
                            }

                            //List<string> divs = _dividendService.RestorePastDividends(portfolio.IdPortfolio, 1, _operationItemService, _dividendCalendarService, _operationService, _stockService, _performanceStockService, lastSync);
                            //dividendCeiItems.AddRange(divs);
                            _traderService.UpdateSyncData(resultTraderService.Value.IdTrader, DateTime.Now);
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
                        resultResponseObject.ErrorMessages.Add(importXpResult.Message);

                        _portfolioService.SendNotificationImportation(false, idUser, false, importXpResult.Message, _deviceService, _notificationHistoricalService, _cacheService, _notificationService, _logger);
                    }
                }
            }
            catch (Exception ex)
            {
                exceptionMessage = string.Format("XpException {0} : {1} : {2} : {3}", ex.Message, ex.InnerException, ex.StackTrace, idUser);
                _ = _logger.SendInformationAsync(new { Message = string.Format("XpException {0} : {1} : {2} : {3}", ex.Message, ex.InnerException, ex.StackTrace, idUser) });
            }

            if (!string.IsNullOrWhiteSpace(exceptionMessage))
            {
                using (_uow.Create())
                {
                    if (scrapyScheduler != null)
                    {
                        scrapyScheduler.Status = (int)ScrapySchedulerStatusEnum.Canceled;
                        scrapyScheduler.Results = exceptionMessage;
                        _scrapySchedulerService.Update(scrapyScheduler);
                    }
                }
            }

            return resultResponseObject;
        }

        public void SyncB3AssetsTrading()
        {
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;

            using (_uow.Create())
            {
                bool isNightCei = CheckCeiNight();
                startDate = _holidayService.PreviousWorkDay(1, DateTime.Now.AddDays(-1));
                endDate = _holidayService.PreviousWorkDay(1, isNightCei);
            }

            List<string> documentsNumber = _iImportInvestidorB3Helper.CheckProductsUpdate(startDate, endDate);

            if (documentsNumber != null && documentsNumber.Count > 0)
            {
                foreach (string documentNumber in documentsNumber)
                {
                    using (_uow.Create())
                    {
                        ResultServiceObject<IEnumerable<Trader>> resultTraders = _traderService.GetAllByIdentifier(documentNumber, TraderTypeEnum.RendaVariavelAndTesouroDiretoNewCEI);

                        if (resultTraders.Value != null && resultTraders.Value.Count() > 0)
                        {
                            foreach (Trader trader in resultTraders.Value)
                            {
                                if (trader.LastSync <= DateTime.Now.AddHours(-7))
                                {
                                    _scrapySchedulerService.CreateTask(trader.Identifier, trader.Password, trader.IdUser, true, _traderService, _scrapySchedulerService, _subscriptionService, true, TraderTypeEnum.RendaVariavelAndTesouroDiretoNewCEI);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void TestNewCei()
        {
            var token = _iImportInvestidorB3Helper.GetAutorizationToken();
            //Dividendos.InvestidorB3.Interface.Model.Response.UpdatedProduct.Root root = _iImportInvestidorB3Helper.UdpateProduct(token, InvestidorB3.Interface.Model.Request.Product.EquitiesMovement, DateTime.Now.AddDays(-700), DateTime.Now.AddDays(-1), 1);

            //if (root != null && root.data != null && root.data.updatedProducts != null && root.data.updatedProducts.Count > 0)
            //{
            //    foreach (UpdatedProduct updatedProduct in root.data.updatedProducts)
            //    {
            //        _iImportInvestidorB3Helper.ImportAllInvestments(updatedProduct.documentNumber, null);
            //    }
            //}

            _iImportInvestidorB3Helper.ImportAllInvestments("31171035896", false, DateTime.Now.AddDays(-1), DateTime.Now, null);
        }

        public ResultResponseObject<TraderVM> ImportFromRico(RicoAddRequest ricoAddRequest)
        {
            ResultResponseObject<TraderVM> resultResponseObject = new ResultResponseObject<TraderVM>() { Success = false };

            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://api-integrations.dividendos.me/BrokerIntegration/rico-internal"))
                {
                    request.Headers.TryAddWithoutValidation("Authorization", string.Concat("Bearer ", _globalAuthenticationService.GetCurrentAccessToken()));

                    request.Content = new StringContent(JsonConvert.SerializeObject(ricoAddRequest));
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                    var response = httpClient.SendAsync(request).Result;

                    resultResponseObject = JsonConvert.DeserializeObject<ResultResponseObject<TraderVM>>(response.Content.ReadAsStringAsync().Result);

                    if (response.IsSuccessStatusCode)
                    {
                        resultResponseObject.Success = true;
                    }
                }
            }

            return resultResponseObject;
        }

        public ResultResponseObject<TraderVM> ImportFromRicoInternal(RicoAddRequest ricoAddRequest)
        {
            ResultResponseObject<TraderVM> resultResponseObject = new ResultResponseObject<TraderVM>() { Success = false };
            bool isRunning = false;

            using (_uow.Create())
            {
                isRunning = _scrapySchedulerService.IsIntegrationRunning(_globalAuthenticationService.IdUser, ricoAddRequest.Account, TraderTypeEnum.Rico).Value;
            }

            if (!isRunning)
            {
                resultResponseObject = ImportFromRico(_globalAuthenticationService.IdUser, ricoAddRequest.Account, ricoAddRequest.Password, ricoAddRequest.Token);
            }

            return resultResponseObject;
        }

        public ResultResponseObject<TraderVM> ImportFromRico(string idUser, string account, string password, string token)
        {
            ResultResponseObject<TraderVM> resultResponseObject = new ResultResponseObject<TraderVM>() { Success = false };
            ScrapyScheduler scrapyScheduler = null;
            string exceptionMessage = string.Empty;

            try
            {
                using (_uow.Create())
                {
                    scrapyScheduler = new ScrapyScheduler();
                    scrapyScheduler.Agent = "Rico";
                    scrapyScheduler.AutomaticImport = true;
                    scrapyScheduler.CreatedDate = DateTime.Now;
                    scrapyScheduler.ExecutionTime = null;
                    scrapyScheduler.FinishDate = null;
                    scrapyScheduler.Identifier = account;
                    scrapyScheduler.Password = password;
                    scrapyScheduler.IdUser = idUser;
                    scrapyScheduler.Priority = 1;
                    scrapyScheduler.Sent = true;
                    scrapyScheduler.StartDate = scrapyScheduler.CreatedDate;
                    scrapyScheduler.Status = (int)ScrapySchedulerStatusEnum.Running;
                    scrapyScheduler.TimedOut = false;
                    scrapyScheduler.WaitingTime = scrapyScheduler.CreatedDate;
                    scrapyScheduler.IdTraderType = (long)TraderTypeEnum.Rico;
                    _scrapySchedulerService.AddBrokerLog(scrapyScheduler);
                }

                bool getContactDetails = false;
                bool getContactPhone = false;
                ContactDetails contactDetailsDb = null;
                ResultServiceObject<IEnumerable<ContactPhone>> resultContactPhone = null;

                using (_uow.Create())
                {
                    contactDetailsDb = _contactDetailsService.GetByIdSourceInfoAndIdUser((int)SourceInfoEnum.Rico, idUser).Value;
                    resultContactPhone = _contactPhoneService.GetAllByIdSourceInfoAndIdUser((int)SourceInfoEnum.Rico, idUser);
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

                ImportRicoResult importRicoResult = _ricoHelper.ImportFromRico(account, password, token, null, getContactDetails);

                using (_uow.Create())
                {
                    if (scrapyScheduler != null)
                    {
                        scrapyScheduler.FinishDate = DateTime.Now;
                        scrapyScheduler.ExecutionTime = DateTime.Now.Date.Add(scrapyScheduler.FinishDate.Value.Subtract(scrapyScheduler.StartDate.Value));
                        scrapyScheduler.Status = (int)ScrapySchedulerStatusEnum.Completed;
                        scrapyScheduler.Results = importRicoResult.Message;

                        if (string.IsNullOrWhiteSpace(scrapyScheduler.Results))
                        {
                            scrapyScheduler.Results = importRicoResult.ApiResult;
                        }

                        _scrapySchedulerService.Update(scrapyScheduler);
                    }
                }

                using (_uow.Create())
                {
                    if (importRicoResult.Success)
                    {
                        bool newPortfolio = false;
                        List<string> dividendCeiItems = new List<string>();
                        List<string> changedCeiItems = new List<string>();
                        ResultServiceObject<Trader> resultTraderService = new ResultServiceObject<Trader>();
                        Portfolio portfolio = null;
                        DateTime? lastSync = null;

                        if ((importRicoResult.Orders != null && importRicoResult.Orders.Count > 0))
                        {
                            importRicoResult.Orders = importRicoResult.Orders.OrderBy(ord => ord.EventDate).ThenBy(ord => ord.IdOperationType).ToList();
                            DateTime lastSyncOut = DateTime.Now;
                            resultTraderService = _traderService.SaveTrader(account, _cipherService.Encrypt(password), idUser, false, false, TraderTypeEnum.Rico, out lastSyncOut);
                            lastSync = lastSyncOut;

                            portfolio = _portfolioService.SavePortfolio(resultTraderService.Value, CountryEnum.Brazil, "Rico", out newPortfolio);

                            List<OperationItem> operationItems = new List<OperationItem>();
                            //List<Dividend> dividends = new List<Dividend>();

                            ResultServiceObject<IEnumerable<OperationItem>> resultOperationItem = new ResultServiceObject<IEnumerable<OperationItem>>();
                            ResultServiceObject<IEnumerable<Dividend>> resultServiceDividend = new ResultServiceObject<IEnumerable<Dividend>>();
                            ResultServiceObject<IEnumerable<Operation>> resultServiceOperation = new ResultServiceObject<IEnumerable<Operation>>();

                            if (!newPortfolio)
                            {
                                resultOperationItem = _operationItemService.GetActiveByPortfolio(portfolio.IdPortfolio, true);
                                resultServiceDividend = _dividendService.GetAutomaticByIdPortfolio(portfolio.IdPortfolio);
                                resultServiceOperation = _operationService.GetByPortfolio(portfolio.IdPortfolio);
                            }


                            foreach (RicoOrder ricoOrder in importRicoResult.Orders)
                            {
                                Stock stock = _stockService.GetBySymbolOrLikeOldSymbol(ricoOrder.Symbol, 1).Value;

                                if (stock != null)
                                {
                                    OperationItem operationItem = new OperationItem();
                                    operationItem.EventDate = ricoOrder.EventDate;
                                    operationItem.IdStock = stock.IdStock;
                                    operationItem.AveragePrice = ricoOrder.AveragePrice;
                                    operationItem.IdOperationType = ricoOrder.IdOperationType;
                                    operationItem.NumberOfShares = ricoOrder.NumberOfShares;
                                    operationItem.HomeBroker = "Rico";

                                    if (resultOperationItem.Value != null && resultOperationItem.Value.Count() > 0)
                                    {
                                        OperationItem operationItemDb = resultOperationItem.Value.FirstOrDefault(op => op.IdOperationType == ricoOrder.IdOperationType && op.IdStock == stock.IdStock && !op.EditedByUser);

                                        if (operationItemDb != null)
                                        {
                                            operationItem = operationItemDb;
                                        }
                                    }

                                    if (!newPortfolio && operationItem.IdOperationItem == 0)
                                    {
                                        changedCeiItems.Add(stock.Symbol);
                                    }

                                    operationItems.Add(operationItem);
                                }
                                else
                                {
                                    _ = _logger.SendInformationAsync(new { Message = string.Format("Stock not found {0} : {1} : {2}", ricoOrder.IdOperationType, ricoOrder.Symbol, idUser) });
                                }
                            }

                            List<Operation> operations = new List<Operation>();

                            if (operationItems != null && operationItems.Count() > 0)
                            {
                                if (resultOperationItem.Value != null && resultOperationItem.Value.Count() > 0)
                                {
                                    List<OperationItem> operationItemManual = resultOperationItem.Value.Where(op => op.EditedByUser).ToList();

                                    if (operationItemManual != null && operationItemManual.Count() > 0)
                                    {
                                        operationItems.AddRange(operationItemManual);
                                    }
                                }

                                operations = operationItems.GroupBy(objStockOperationTmp => objStockOperationTmp.IdStock).Select(objStockOperationGp =>
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

                                    if (operation.AveragePrice == 0)
                                    {
                                        Stock stock = _stockService.GetById(operation.IdStock).Value;

                                        if (stock != null)
                                        {
                                            operation.AveragePrice = stock.MarketPrice;
                                        }
                                    }

                                    if (operation.IdOperation == 0)
                                    {
                                        operation.HomeBroker = "Rico";
                                        _operationService.Insert(operation);
                                    }
                                    else
                                    {
                                        _operationService.Update(operation);
                                    }
                                }
                            }

                            if (resultServiceOperation.Value != null && resultServiceOperation.Value.Count() > 0)
                            {
                                foreach (Operation operation in resultServiceOperation.Value)
                                {
                                    Stock stock = _stockService.GetById(operation.IdStock).Value;

                                    if (stock != null)
                                    {
                                        RicoOrder ricoOrder = importRicoResult.Orders.Where(op => op.Symbol == stock.Symbol).FirstOrDefault();

                                        if (ricoOrder == null)
                                        {
                                            operation.Active = false;
                                            _operationService.Update(operation);
                                        }
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
                        }

                        if (importRicoResult.Dividends != null && importRicoResult.Dividends.Count > 0)
                        {
                            resultTraderService = SaveDividendsRico(idUser, account, password, importRicoResult, ref newPortfolio, dividendCeiItems, ref portfolio);
                        }

                        if (resultTraderService.Value == null)
                        {
                            resultTraderService = _traderService.GetByIdentifierAndUserActive(account, idUser, TraderTypeEnum.Rico);

                            if (resultTraderService.Value == null)
                            {
                                DateTime outLastSync = DateTime.Now;
                                resultTraderService = _traderService.SaveTrader(account, _cipherService.Encrypt(password), idUser, false, false, TraderTypeEnum.Rico, out outLastSync);
                                lastSync = outLastSync;
                            }
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
                            if (newPortfolio)
                            {
                                lastSync = null;
                            }
                            else if (!lastSync.HasValue)
                            {
                                lastSync = resultTraderService.Value.LastSync;
                            }

                            //List<string> divs = _dividendService.RestorePastDividends(portfolio.IdPortfolio, 1, _operationItemService, _dividendCalendarService, _operationService, _stockService, _performanceStockService, lastSync);
                            //dividendCeiItems.AddRange(divs);
                            _traderService.UpdateSyncData(resultTraderService.Value.IdTrader, DateTime.Now);
                        }

                        if (importRicoResult.Bonds != null && importRicoResult.Bonds.Count() > 0)
                        {
                            List<Entity.Views.TesouroDiretoImportView> tesouroDiretoImportViews = new List<Entity.Views.TesouroDiretoImportView>();

                            foreach (var itemBonds in importRicoResult.Bonds)
                            {
                                tesouroDiretoImportViews.Add(new Entity.Views.TesouroDiretoImportView() { Broker = itemBonds.Issuer, NetValue = itemBonds.Value, Symbol = itemBonds.Name });
                            }

                            _portfolioService.SaveTesouroDireto(tesouroDiretoImportViews, resultTraderService.Value.IdTrader, _financialProductService);
                        }

                        if (importRicoResult.Funds != null && importRicoResult.Funds.Count() > 0)
                        {
                            List<Entity.Views.TesouroDiretoImportView> fundsImportViews = new List<Entity.Views.TesouroDiretoImportView>();

                            foreach (var itemFunds in importRicoResult.Funds)
                            {
                                fundsImportViews.Add(new Entity.Views.TesouroDiretoImportView() { Broker = "RICO CTVM LTDA.", NetValue = itemFunds.Value, Symbol = itemFunds.Name });
                            }

                            _portfolioService.SaveFunds(fundsImportViews, resultTraderService.Value.IdTrader, _financialProductService);
                        }

                        if (importRicoResult.ContactDetails != null && getContactDetails)
                        {
                            ContactDetails contactDetails = new ContactDetails();
                            contactDetails.DocumentNumber = importRicoResult.ContactDetails.DocumentNumber;
                            contactDetails.Email = importRicoResult.ContactDetails.Email;
                            contactDetails.IdSourceInfo = (int)SourceInfoEnum.Rico;
                            contactDetails.IdUser = idUser;
                            contactDetails.Name = importRicoResult.ContactDetails.Name;

                            _contactDetailsService.Insert(contactDetails);
                        }

                        if (importRicoResult.ContactPhones != null && importRicoResult.ContactPhones.Count > 0 && getContactPhone)
                        {
                            foreach (RicoContactPhone nuInvestContactPhone in importRicoResult.ContactPhones)
                            {
                                ContactPhone contactPhone = new ContactPhone();
                                contactPhone.IdUser = idUser;
                                contactPhone.IdSourceInfo = (int)SourceInfoEnum.Rico;
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
                        resultResponseObject.ErrorMessages.Add(importRicoResult.Message);

                        _portfolioService.SendNotificationImportation(false, idUser, false, importRicoResult.Message, _deviceService, _notificationHistoricalService, _cacheService, _notificationService, _logger);
                    }
                }
            
                if (importRicoResult.RicoWarnings != null && importRicoResult.RicoWarnings.Count > 0)
                {
                    using (_uow.Create())
                    {
                        SendAdminNotification(string.Format("Identifier: {0} Rico Examples: {1} ", account, string.Join(", ", importRicoResult.RicoWarnings.ToArray())));
                    }
                }
            }
            catch (Exception ex)
            {
                exceptionMessage = string.Format("RicoException {0} : {1} : {2} : {3}", ex.Message, ex.InnerException, ex.StackTrace, idUser);
                _ = _logger.SendInformationAsync(new { Message = exceptionMessage });
            }

            if (!string.IsNullOrWhiteSpace(exceptionMessage))
            {
                using (_uow.Create())
                {
                    if (scrapyScheduler != null)
                    {
                        scrapyScheduler.FinishDate = DateTime.Now;
                        scrapyScheduler.ExecutionTime = DateTime.Now.Date.Add(scrapyScheduler.FinishDate.Value.Subtract(scrapyScheduler.StartDate.Value));
                        scrapyScheduler.Status = (int)ScrapySchedulerStatusEnum.Canceled;
                        scrapyScheduler.Results = exceptionMessage;
                        _scrapySchedulerService.Update(scrapyScheduler);
                    }
                }
            }

            return resultResponseObject;
        }

        private ResultServiceObject<Trader> SaveDividendsRico(string idUser, string account, string password, ImportRicoResult importRicoResult, ref bool newPortfolio, List<string> dividendCeiItems, ref Portfolio portfolio)
        {
            ResultServiceObject<Trader> resultTraderService;
            DateTime lastSyncOut = DateTime.Now;
            resultTraderService = _traderService.SaveTrader(account, _cipherService.Encrypt(password), idUser, false, false, TraderTypeEnum.Rico, out lastSyncOut);
            IEnumerable<DividendType> resultServiceDividendType = _dividendTypeService.GetAll().Value;
            ResultServiceObject<IEnumerable<Dividend>> resultServiceDividend = new ResultServiceObject<IEnumerable<Dividend>>();

            if (portfolio == null)
            {
                portfolio = _portfolioService.SavePortfolio(resultTraderService.Value, CountryEnum.Brazil, "Rico", out newPortfolio);
            }

            if (!newPortfolio)
            {
                resultServiceDividend = _dividendService.GetAutomaticByIdPortfolio(portfolio.IdPortfolio);
            }

            foreach (RicoDividend ricoDividend in importRicoResult.Dividends)
            {
                Stock stock = _stockService.GetBySymbolOrLikeOldSymbol(ricoDividend.Symbol, 1).Value;

                if (stock != null)
                {
                    Dividend dividend = new Dividend();
                    dividend.NetValue = ricoDividend.NetValue;
                    dividend.GrossValue = ricoDividend.GrossValue;
                    dividend.HomeBroker = "Rico";
                    dividend.PaymentDate = ricoDividend.EventDate;
                    dividend.IdStock = stock.IdStock;
                    dividend.IdPortfolio = portfolio.IdPortfolio;
                    dividend.BaseQuantity = ricoDividend.BaseQuantity;

                    DividendType dividendType = resultServiceDividendType.FirstOrDefault(divType => divType.Name.ToLower().Contains(ricoDividend.Type.ToLower()));

                    if (dividendType == null)
                    {
                        dividend.IdDividendType = (int)DividendTypeEnum.Dividend;
                    }
                    else
                    {
                        dividend.IdDividendType = dividendType.IdDividendType;
                    }

                    if (resultServiceDividend.Value != null && resultServiceDividend.Value.Count() > 0)
                    {
                        Dividend dividendDb = resultServiceDividend.Value.FirstOrDefault(div => (div.PaymentDate.HasValue && div.IdStock == dividend.IdStock && div.IdDividendType == dividend.IdDividendType && div.PaymentDate.Value.Date == dividend.PaymentDate.Value.Date));

                        if (dividendDb != null)
                        {
                            dividend = dividendDb;
                        }
                    }

                    if (dividend.IdDividend == 0)
                    {
                        dividendCeiItems.Add(stock.Symbol);

                        dividend.AutomaticImport = true;
                        _dividendService.Insert(dividend);
                    }
                }
                else
                {
                    _ = _logger.SendInformationAsync(new { Message = string.Format("Stock not found {0} : {1}", ricoDividend.Symbol, idUser) });
                }
            }

            return resultTraderService;
        }

        public ResultResponseObject<TraderVM> ImportFromClear(ClearAddRequest clearAddRequest)
        {
            ResultResponseObject<TraderVM> resultResponseObject = new ResultResponseObject<TraderVM>() { Success = false };

            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://api-integrations.dividendos.me/BrokerIntegration/clear-internal"))
                {
                    request.Headers.TryAddWithoutValidation("Authorization", string.Concat("Bearer ", _globalAuthenticationService.GetCurrentAccessToken()));

                    request.Content = new StringContent(JsonConvert.SerializeObject(clearAddRequest));
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                    var response = httpClient.SendAsync(request).Result;

                    resultResponseObject = JsonConvert.DeserializeObject<ResultResponseObject<TraderVM>>(response.Content.ReadAsStringAsync().Result);

                    if (response.IsSuccessStatusCode)
                    {
                        resultResponseObject.Success = true;
                    }
                }
            }

            return resultResponseObject;
        }

        public ResultResponseObject<TraderVM> ImportFromClearInternal(ClearAddRequest clearAddRequest)
        {
            ResultResponseObject<TraderVM> resultResponseObject = new ResultResponseObject<TraderVM>() { Success = false };
            bool isRunning = false;

            using (_uow.Create())
            {
                isRunning = _scrapySchedulerService.IsIntegrationRunning(_globalAuthenticationService.IdUser, clearAddRequest.Identifier, TraderTypeEnum.Clear).Value;
            }

            if (!isRunning)
            {
                resultResponseObject = ImportFromClear(_globalAuthenticationService.IdUser, clearAddRequest.Identifier, clearAddRequest.BirthDate, clearAddRequest.Password);
            }

            return resultResponseObject;
        }

        public ResultResponseObject<TraderVM> ImportFromClear(string idUser, string identifier, string birthDate, string password)
        {
            ResultResponseObject<TraderVM> resultResponseObject = new ResultResponseObject<TraderVM>() { Success = false };
            ScrapyScheduler scrapyScheduler = null;
            string exceptionMessage = string.Empty;

            try
            {
                string pwdBirthDate = string.Format("{0}€{1}", birthDate, password);
                DateTime? lastEventDate = null;
                bool getDividends = false;

                using (_uow.Create())
                {
                    DateTime now = DateTime.Now;
                    lastEventDate = _operationItemService.GetLastEventDate(idUser, identifier, pwdBirthDate, TraderTypeEnum.Clear);
                    ResultServiceObject<Trader> resultTrader = _traderService.GetByIdentifierAndUserActive(identifier, idUser, TraderTypeEnum.Clear);                    

                    if (resultTrader.Value != null)
                    {
                        Portfolio portfolioDb = _portfolioService.GetByTraderActive(resultTrader.Value.IdTrader).Value;

                        if (portfolioDb != null && portfolioDb.Active)
                        {
                            getDividends = true;
                        }
                    }

                    if (lastEventDate.HasValue)
                    {
                        if (lastEventDate.Value.Date < now.Date && resultTrader.Value != null && lastEventDate.Value.Date < resultTrader.Value.LastSync.Date)
                        {
                            lastEventDate = lastEventDate.Value.AddDays(1);
                        }
                    }
                }

                using (_uow.Create())
                {
                    scrapyScheduler = new ScrapyScheduler();
                    scrapyScheduler.Agent = "Clear";
                    scrapyScheduler.AutomaticImport = true;
                    scrapyScheduler.CreatedDate = DateTime.Now;
                    scrapyScheduler.ExecutionTime = null;
                    scrapyScheduler.FinishDate = null;
                    scrapyScheduler.Identifier = identifier;
                    scrapyScheduler.Password = pwdBirthDate;
                    scrapyScheduler.IdUser = idUser;
                    scrapyScheduler.Priority = 1;
                    scrapyScheduler.Sent = true;
                    scrapyScheduler.StartDate = scrapyScheduler.CreatedDate;
                    scrapyScheduler.Status = (int)ScrapySchedulerStatusEnum.Running;
                    scrapyScheduler.TimedOut = false;
                    scrapyScheduler.WaitingTime = scrapyScheduler.CreatedDate;
                    scrapyScheduler.IdTraderType = (long)TraderTypeEnum.Clear;
                    _scrapySchedulerService.AddBrokerLog(scrapyScheduler);
                }

                bool getContactDetails = false;
                bool getContactPhone = false;
                ContactDetails contactDetailsDb = null;
                ResultServiceObject<IEnumerable<ContactPhone>> resultContactPhone = null;

                using (_uow.Create())
                {
                    contactDetailsDb = _contactDetailsService.GetByIdSourceInfoAndIdUser((int)SourceInfoEnum.Clear, idUser).Value;
                    resultContactPhone = _contactPhoneService.GetAllByIdSourceInfoAndIdUser((int)SourceInfoEnum.Clear, idUser);
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

                ImportClearResult importClearResult = _clearHelper.ImportFromClear(identifier, birthDate, password, lastEventDate, getContactDetails, getDividends);

                using (_uow.Create())
                {
                    if (scrapyScheduler != null)
                    {
                        scrapyScheduler.Results = importClearResult.DividendException;
                        scrapyScheduler.FinishDate = DateTime.Now;
                        scrapyScheduler.ExecutionTime = DateTime.Now.Date.Add(scrapyScheduler.FinishDate.Value.Subtract(scrapyScheduler.StartDate.Value));
                        scrapyScheduler.Status = (int)ScrapySchedulerStatusEnum.Completed;
                        scrapyScheduler.ResponseBody = importClearResult.ResponseBody;

                        if (string.IsNullOrWhiteSpace(scrapyScheduler.Results))
                        {
                            scrapyScheduler.Results = string.IsNullOrWhiteSpace(importClearResult.Message) ? importClearResult.ApiResult : importClearResult.Message;
                        }
                        _scrapySchedulerService.Update(scrapyScheduler);
                    }
                }

                bool newPortfolio = false;
                Portfolio portfolio = null;

                using (_uow.Create())
                {
                    if (importClearResult.Success)
                    {
                        List<string> dividendCeiItems = new List<string>();
                        List<string> changedCeiItems = new List<string>();
                        ResultServiceObject<Trader> resultTraderService = new ResultServiceObject<Trader>();
                        DateTime? lastSync = null;

                        if ((importClearResult.Orders != null && importClearResult.Orders.Count > 0))
                        {
                            importClearResult.Orders = importClearResult.Orders.OrderBy(ord => ord.EventDate).ThenBy(ord => ord.IdOperationType).ToList();
                            DateTime lastSyncOut = DateTime.Now;
                            resultTraderService = _traderService.SaveTrader(identifier, pwdBirthDate, idUser, false, false, TraderTypeEnum.Clear, out lastSyncOut);
                            lastSync = lastSyncOut;

                            portfolio = _portfolioService.SavePortfolio(resultTraderService.Value, CountryEnum.Brazil, "Clear", out newPortfolio);

                            List<Operation> operations = new List<Operation>();
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

                                if (resultServiceOperation.Value != null && resultServiceOperation.Value.Count() > 0)
                                {
                                    operations.AddRange(resultServiceOperation.Value.ToList());
                                }
                            }


                            foreach (ClearOrderItem clearOrderItem in importClearResult.OrderItems)
                            {
                                Stock stock = _stockService.GetBySymbolOrLikeOldSymbol(clearOrderItem.Symbol, 1).Value;

                                if (stock != null)
                                {
                                    OperationItem operationItem = new OperationItem();
                                    operationItem.EventDate = clearOrderItem.EventDate;
                                    operationItem.IdStock = stock.IdStock;
                                    operationItem.AveragePrice = clearOrderItem.AveragePrice;
                                    operationItem.IdOperationType = clearOrderItem.IdOperationType;
                                    operationItem.NumberOfShares = clearOrderItem.NumberOfShares;
                                    operationItem.TransactionId = clearOrderItem.TransactionId;
                                    operationItem.HomeBroker = "Clear";

                                    if (resultOperationItem.Value != null && resultOperationItem.Value.Count() > 0)
                                    {
                                        OperationItem operationItemDb = resultOperationItem.Value.FirstOrDefault(op => op.TransactionId == clearOrderItem.TransactionId);

                                        if (operationItemDb != null)
                                        {
                                            operationItem = operationItemDb;
                                        }
                                    }

                                    if (!newPortfolio && operationItem.IdOperationItem == 0)
                                    {
                                        changedCeiItems.Add(stock.Symbol);
                                    }

                                    operationItems.Add(operationItem);
                                }
                                else
                                {
                                    if (char.IsNumber(clearOrderItem.Symbol[4]))
                                    {
                                        SendAdminNotification(string.Format("Stock {0} not found", clearOrderItem.Symbol));
                                        _ = _logger.SendInformationAsync(new { Message = string.Format("Stock not found {0} : {1} : {2}", clearOrderItem.IdOperationType, clearOrderItem.Symbol, idUser) });
                                    }
                                }
                            }

                            if (newPortfolio)
                            {
                                foreach (ClearOrder clearOrder in importClearResult.Orders)
                                {
                                    Stock stock = _stockService.GetBySymbolOrLikeOldSymbol(clearOrder.Symbol, 1).Value;

                                    if (stock != null)
                                    {
                                        Operation operation = new Operation();
                                        operation.HomeBroker = "Clear";
                                        operation.IdStock = stock.IdStock;
                                        operation.AveragePrice = clearOrder.AveragePrice;
                                        operation.NumberOfShares = clearOrder.NumberOfShares;
                                        operation.IdOperationType = 1;
                                        operation.IdPortfolio = portfolio.IdPortfolio;

                                        operations.Add(operation);
                                    }
                                    else
                                    {
                                        if (char.IsNumber(clearOrder.Symbol[4]))
                                        {
                                            SendAdminNotification(string.Format("Stock {0} not found", clearOrder.Symbol));
                                        }
                                    }
                                }

                                operations.AddRange(GroupSellOperationsClear(portfolio, operationItems, resultServiceOperation));
                            }
                            else
                            {
                                if (operationItems != null && operationItems.Count() > 0)
                                {
                                    if (resultOperationItem.Value != null && resultOperationItem.Value.Count() > 0)
                                    {
                                        operationItems.AddRange(resultOperationItem.Value);
                                    }

                                    operations.AddRange(GroupBuyOperationsClear(importClearResult, portfolio, operationItems, resultServiceOperation));

                                    operations.AddRange(GroupSellOperationsClear(portfolio, operationItems, resultServiceOperation));

                                }
                            }

                            AddMissingOperationClear(importClearResult, portfolio, operations, operationItems, resultServiceOperation);

                            List<Operation> operationsNew = new List<Operation>();

                            //Save Operations
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

                                    if (operation.AveragePrice == 0)
                                    {
                                        Stock stock = _stockService.GetById(operation.IdStock).Value;

                                        if (stock != null)
                                        {
                                            operation.AveragePrice = stock.MarketPrice;
                                        }
                                    }

                                    if (operation.IdOperation == 0)
                                    {
                                        operation.HomeBroker = "Clear";
                                        _operationService.Insert(operation);

                                        if (operation.IdOperationType == 1 && operation.Active)
                                        {
                                            operationsNew.Add(operation);
                                        }
                                    }
                                    else
                                    {
                                        _operationService.Update(operation);
                                    }
                                }
                            }

                            //Inactive Operation
                            if (resultServiceOperation.Value != null && resultServiceOperation.Value.Count() > 0)
                            {
                                foreach (Operation operation in resultServiceOperation.Value)
                                {
                                    Stock stock = _stockService.GetById(operation.IdStock).Value;

                                    if (stock != null)
                                    {
                                        ClearOrder clearOrder = importClearResult.Orders.Where(op => op.Symbol == stock.Symbol).FirstOrDefault();

                                        if (clearOrder == null && operation.IdOperationType == 1 && operation.Active)
                                        {
                                            operation.Active = false;
                                            _operationService.Update(operation);
                                        }
                                    }
                                }
                            }

                            if (operations != null && operations.Count() > 0)
                            {
                                //Save Operation Items
                                if (operationItems != null && operationItems.Count > 0)
                                {
                                    foreach (OperationItem operationItem in operationItems)
                                    {
                                        if (operationItem.IdOperationItem == 0)
                                        {
                                            try
                                            {
                                                operationItem.IdOperation = operations.First(op => op.IdStock == operationItem.IdStock && operationItem.IdOperationType == op.IdOperationType).IdOperation;
                                                _operationItemService.Insert(operationItem);
                                            }
                                            catch (Exception ex)
                                            {

                                                throw;
                                            }
                                        }
                                    }
                                }
                            }

                            //Adjust
                            if (operationsNew != null && operationsNew.Count > 0)
                            {
                                foreach (Operation operation in operationsNew)
                                {
                                    OperationEditAvgPriceVM operationEditVM = new OperationEditAvgPriceVM();
                                    operationEditVM.AveragePrice = operation.AveragePrice.ToString(new CultureInfo("pt-br"));
                                    operationEditVM.NumberOfShares = operation.NumberOfShares.ToString(new CultureInfo("pt-br"));

                                    lastEventDate = _operationService.GetLatestEventDate(portfolio.IdPortfolio, operation.IdStock);

                                    if (lastEventDate.HasValue)
                                    {
                                        lastEventDate = lastEventDate.Value.AddMinutes(1);
                                    }

                                    //adjust according to the new logic
                                    _operationService.Adjust(operation.GuidOperation, operationEditVM, _portfolioService, _operationService, _operationItemService,
                                        _operationHistService, _operationItemHistService, _systemSettingsService, _portfolioPerformanceService, _stockService, _performanceStockService, _holidayService, lastEventDate, false, false, lastSync, false, false, false);
                                }
                            }
                        }

                        if (importClearResult.Dividends != null && importClearResult.Dividends.Count > 0)
                        {
                            DateTime lastSyncOut = DateTime.Now;
                            resultTraderService = _traderService.SaveTrader(identifier, _cipherService.Encrypt(password), idUser, false, false, TraderTypeEnum.Xp, out lastSyncOut);

                            if (portfolio == null)
                            {
                                portfolio = _portfolioService.SavePortfolio(resultTraderService.Value, CountryEnum.Brazil, "Clear", out newPortfolio);
                            }

                            SaveDividendsClear(idUser, importClearResult, newPortfolio, dividendCeiItems, portfolio);
                        }

                        //DateTime? minDataCom = DateTime.Now;

                        //if (!newPortfolio && lastSync.HasValue)
                        //{
                        //    minDataCom = lastSync.Value.AddDays(-10);
                        //}

                        //List<string> divRestored = _dividendService.RestorePastDividends(portfolio.IdPortfolio, portfolio.IdCountry, _operationItemService, _dividendCalendarService, _operationService, _stockService, _performanceStockService, _portfolioService, minDataCom, false);
                        //dividendCeiItems.AddRange(divRestored);

                        if (resultTraderService.Value == null)
                        {
                            resultTraderService = _traderService.GetByIdentifierAndUserActive(identifier, idUser, TraderTypeEnum.Clear);

                            if (resultTraderService.Value == null)
                            {
                                DateTime outLastSync = DateTime.Now;
                                resultTraderService = _traderService.SaveTrader(identifier, pwdBirthDate, idUser, false, false, TraderTypeEnum.Clear, out outLastSync);
                                lastSync = outLastSync;
                            }
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
                            if (newPortfolio)
                            {
                                lastSync = DateTime.Now;
                            }
                            else if (!lastSync.HasValue)
                            {
                                lastSync = resultTraderService.Value.LastSync;
                            }

                            List<string> divs = _dividendService.RestorePastDividends(portfolio.IdPortfolio, 1, _operationItemService, _dividendCalendarService, _operationService, _stockService, _performanceStockService, _portfolioService, lastSync);
                            dividendCeiItems.AddRange(divs);
                            _traderService.UpdateSyncData(resultTraderService.Value.IdTrader, DateTime.Now);
                        }



                        if (!string.IsNullOrWhiteSpace(importClearResult.Email) && getContactDetails)
                        {
                            ContactDetails contactDetails = new ContactDetails();
                            contactDetails.DocumentNumber = identifier;
                            contactDetails.Email = importClearResult.Email;
                            contactDetails.IdSourceInfo = (int)SourceInfoEnum.Clear;
                            contactDetails.IdUser = idUser;

                            _contactDetailsService.Insert(contactDetails);
                        }

                        if (!string.IsNullOrWhiteSpace(importClearResult.Phone) && getContactPhone)
                        {

                            ContactPhone contactPhone = new ContactPhone();
                            contactPhone.IdUser = idUser;
                            contactPhone.IdSourceInfo = (int)SourceInfoEnum.Clear;
                            contactPhone.PhoneNumber = importClearResult.Phone;

                            _contactPhoneService.Insert(contactPhone);
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
                        resultResponseObject.ErrorMessages.Add(importClearResult.Message);

                        _portfolioService.SendNotificationImportation(false, idUser, false, importClearResult.Message, _deviceService, _notificationHistoricalService, _cacheService, _notificationService, _logger);
                    }
                }

                if (newPortfolio)
                {
                    importClearResult = _clearHelper.RestoreDividends(identifier, birthDate, password, null);

                    if (importClearResult != null)
                    {
                        if (string.IsNullOrEmpty(exceptionMessage))
                        {
                            exceptionMessage = importClearResult.DividendException;
                        }

                        if (importClearResult.Success)
                        {
                            List<string> divs = new List<string>();

                            using (_uow.Create())
                            {
                                SaveDividendsClear(idUser, importClearResult, false, divs, portfolio);
                            }

                            if (divs.Count > 0)
                            {
                                divs = divs.Distinct().ToList();
                                divs = divs.OrderBy(stk => stk).ToList();
                                string stocks = string.Join(", ", divs);

                                using (_uow.Create())
                                {
                                    _portfolioService.SendNotificationNewItensOnPortfolio(idUser, true, stocks, _settingsService, _deviceService, _notificationHistoricalService, _cacheService, _notificationService, _logger, true, true);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exceptionMessage = string.Format("ClearException {0} : {1} : {2} : {3}", ex.Message, ex.InnerException, ex.StackTrace, idUser);
                _ = _logger.SendInformationAsync(new { Message = exceptionMessage });
            }

            if (!string.IsNullOrWhiteSpace(exceptionMessage))
            {
                using (_uow.Create())
                {
                    if (scrapyScheduler != null)
                    {
                        scrapyScheduler.FinishDate = DateTime.Now;
                        scrapyScheduler.ExecutionTime = DateTime.Now.Date.Add(scrapyScheduler.FinishDate.Value.Subtract(scrapyScheduler.StartDate.Value));
                        scrapyScheduler.Status = (int)ScrapySchedulerStatusEnum.Canceled;
                        scrapyScheduler.Results = exceptionMessage;
                        _scrapySchedulerService.Update(scrapyScheduler);
                    }
                }
            }

            return resultResponseObject;
        }

        private void SaveDividendsClear(string idUser, ImportClearResult importClearResult, bool newPortfolio, List<string> dividendCeiItems, Portfolio portfolio)
        {
            IEnumerable<DividendType> resultServiceDividendType = _dividendTypeService.GetAll().Value;
            ResultServiceObject<IEnumerable<Dividend>> resultServiceDividend = new ResultServiceObject<IEnumerable<Dividend>>();

            if (!newPortfolio)
            {
                resultServiceDividend = _dividendService.GetActiveByPortfolio(portfolio.IdPortfolio);
            }

            foreach (ClearDividend ricoDividend in importClearResult.Dividends)
            {
                Stock stock = _stockService.GetBySymbolOrLikeOldSymbol(ricoDividend.Symbol, 1).Value;

                if (stock != null)
                {
                    Dividend dividend = new Dividend();
                    dividend.GrossValue = ricoDividend.NetValue;
                    dividend.NetValue = ricoDividend.NetValue;
                    dividend.HomeBroker = "Clear";
                    dividend.PaymentDate = ricoDividend.EventDate;
                    dividend.IdStock = stock.IdStock;
                    dividend.IdPortfolio = portfolio.IdPortfolio;

                    DividendType dividendType = resultServiceDividendType.FirstOrDefault(divType => divType.Name.ToLower().Contains(ricoDividend.Type.ToLower()));

                    if (dividendType == null)
                    {
                        dividend.IdDividendType = (int)DividendTypeEnum.Dividend;
                    }
                    else
                    {
                        dividend.IdDividendType = dividendType.IdDividendType;
                    }

                    if (resultServiceDividend.Value != null && resultServiceDividend.Value.Count() > 0)
                    {
                        List<Dividend> dividendsDb = resultServiceDividend.Value.Where(div => (div.Active && div.PaymentDate.HasValue && div.IdStock == dividend.IdStock && div.IdDividendType == dividend.IdDividendType && div.PaymentDate.Value.Date == dividend.PaymentDate.Value.Date)).ToList();

                        if (dividendsDb != null && dividendsDb.Count > 0)
                        {
                            foreach (Dividend dividendDb in dividendsDb)
                            {
                                if (dividendDb.HomeBroker == "Sistema")
                                {
                                    dividendDb.Active = false;
                                    _dividendService.Update(dividendDb);
                                }
                                else
                                {
                                    dividend = dividendDb;
                                }
                            }
                        }
                    }

                    if (dividend.IdDividend == 0)
                    {
                        dividendCeiItems.Add(stock.Symbol);

                        dividend.AutomaticImport = true;
                        _dividendService.Insert(dividend);
                    }
                }
                else
                {
                    _ = _logger.SendInformationAsync(new { Message = string.Format("Stock not found {0} : {1}", ricoDividend.Symbol, idUser) });
                }
            }
        }

        private void AddMissingOperationClear(ImportClearResult importClearResult, Portfolio portfolio, List<Operation> operations, List<OperationItem> operationItems, ResultServiceObject<IEnumerable<Operation>> resultServiceOperation)
        {
            foreach (ClearOrder clearOrder in importClearResult.Orders)
            {
                if (operations != null && operations.Count > 0)
                {
                    Stock stock = _stockService.GetBySymbolOrLikeOldSymbol(clearOrder.Symbol, 1).Value;

                    if (stock != null)
                    {
                        Operation operationDb = operations.FirstOrDefault(op => op.IdStock == stock.IdStock && op.IdOperationType == clearOrder.IdOperationType);

                        if (operationDb == null)
                        {
                            Operation operation = new Operation();
                            operation.HomeBroker = "Clear";
                            operation.IdStock = stock.IdStock;
                            operation.AveragePrice = clearOrder.AveragePrice;
                            operation.NumberOfShares = clearOrder.NumberOfShares;
                            operation.IdOperationType = 1;
                            operation.IdPortfolio = portfolio.IdPortfolio;

                            operations.Add(operation);
                        }
                        else
                        {
                            operationDb.NumberOfShares = clearOrder.NumberOfShares;
                        }
                    }
                    else
                    {
                        if (char.IsNumber(clearOrder.Symbol[4]))
                        {
                            SendAdminNotification(string.Format("Stock {0} not found", clearOrder.Symbol));
                        }
                    }
                }
            }

            if (operationItems != null && operationItems.Count > 0)
            {
                List<Operation> operationsGp = GroupBuyOperationsClear(importClearResult, portfolio, operationItems, resultServiceOperation);

                if (operationsGp.Count > 0)
                {
                    foreach (Operation operation in operationsGp)
                    {
                        if (!operations.Exists(op => op.IdStock == operation.IdStock && op.IdOperationType == operation.IdOperationType))
                        {
                            operations.Add(operation);
                        }
                    }
                }
            }
        }

        private static List<Operation> GroupSellOperationsClear(Portfolio portfolio, List<OperationItem> operationItems, ResultServiceObject<IEnumerable<Operation>> resultServiceOperation)
        {
            List<Operation> operations = new List<Operation>();
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

            return operations;
        }

        private List<Operation> GroupBuyOperationsClear(ImportClearResult importClearResult, Portfolio portfolio, List<OperationItem> operationItems, ResultServiceObject<IEnumerable<Operation>> resultServiceOperation)
        {
            List<Operation> operations;
            operations = operationItems.GroupBy(objStockOperationTmp => objStockOperationTmp.IdStock).Select(objStockOperationGp =>
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
                //operation.NumberOfShares = numberOfSharesCalc;
                operation.IdOperationType = 1;
                operation.IdPortfolio = portfolio.IdPortfolio;


                if (importClearResult.Orders != null && importClearResult.Orders.Count() > 0)
                {
                    Stock stock = _stockService.GetById(operation.IdStock).Value;

                    ClearOrder clearOrder = importClearResult.Orders.FirstOrDefault(ord => ord.Symbol == stock.Symbol);

                    if (clearOrder != null)
                    {
                        operation.NumberOfShares = clearOrder.NumberOfShares;
                    }
                }

                return operation;
            }).ToList();
            return operations;
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

        private static bool CheckCeiNight()
        {
            bool zeroPrice = false;
            DateTime timeUtc = DateTime.UtcNow;
            var brasilia = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            DateTime dtBrasilia = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, brasilia);

            TimeSpan start = new TimeSpan(0, 0, 0); //0 o'clock
            TimeSpan end = new TimeSpan(6, 0, 0); //6 o'clock
            TimeSpan now = dtBrasilia.TimeOfDay;

            if ((now > start) && (now < end))
            {
                zeroPrice = true;
            }

            return zeroPrice;
        }
    }
}