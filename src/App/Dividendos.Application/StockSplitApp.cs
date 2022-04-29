using AutoMapper;
using Dividendos.API.Model.Request.Operation;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using Dividendos.API.Model.Response.StockSplit;
using Dividendos.API.Model.Response.v1;
using Dividendos.Application.Base;
using Dividendos.Application.Interface;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Enum;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using Dividendos.InvestingCom.Interface;
using Dividendos.InvestingCom.Interface.Model;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using K.Logger;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Dividendos.Application
{
    public class StockSplitApp : BaseApp, IStockSplitApp
    {
        private readonly IStockSplitService _stockSplitService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly ICacheService _cacheService;
        private readonly IStockService _stockService;
        private readonly INotificationHistoricalService _notificationHistoricalService;
        private readonly IDeviceService _deviceService;
        private readonly ISettingsService _settingsService;
        private readonly INotificationService _notificationService;
        private readonly ILogger _logger;
        private readonly IGlobalAuthenticationService _globalAuthenticationService;
        private readonly IInvestingComHelper _iInvestingComHelper;
        private readonly IOperationService _operationService;
        private readonly ICompanyService _companyService;
        private readonly IPortfolioService _portfolioService;
        private readonly IOperationItemService _operationItemService;
        private readonly IOperationHistService _operationHistService;
        private readonly IOperationItemHistService _operationItemHistService;
        private readonly ISystemSettingsService _systemSettingsService;
        private readonly IPortfolioPerformanceService _portfolioPerformanceService;
        private readonly IPerformanceStockService _performanceStockService;
        private readonly IHolidayService _holidayService;

        public StockSplitApp(IMapper mapper,
            IUnitOfWork uow,
            IStockSplitService stockSplitService,
            ICacheService cacheService,
            IStockService stockService,
            INotificationHistoricalService notificationHistoricalService,
            IDeviceService deviceService,
            ISettingsService settingsService,
            INotificationService notificationService,
            ILogger logger,
            IGlobalAuthenticationService globalAuthenticationService,
            IInvestingComHelper iInvestingComHelper,
            IOperationService operationService,
            ICompanyService companyService,
            IPortfolioService portfolioService,
            IOperationHistService operationHistService,
            IOperationItemHistService operationItemHistService,
            IOperationItemService operationItemService,
            ISystemSettingsService systemSettingsService,
            IPortfolioPerformanceService portfolioPerformanceService,
            IPerformanceStockService performanceStockService,
            IHolidayService holidayService)
        {
            _stockSplitService = stockSplitService;
            _mapper = mapper;
            _uow = uow;
            _cacheService = cacheService;
            _stockService = stockService;
            _notificationHistoricalService = notificationHistoricalService;
            _deviceService = deviceService;
            _settingsService = settingsService;
            _notificationService = notificationService;
            _logger = logger;
            _globalAuthenticationService = globalAuthenticationService;
            _iInvestingComHelper = iInvestingComHelper;
            _operationService = operationService;
            _companyService = companyService;
            _portfolioService = portfolioService;
            _operationItemService = operationItemService;
            _operationHistService = operationHistService;
            _operationItemHistService = operationItemHistService;
            _systemSettingsService = systemSettingsService;
            _portfolioPerformanceService = portfolioPerformanceService;
            _performanceStockService = performanceStockService;
            _holidayService = holidayService;
        }

        public ResultResponseObject<IEnumerable<StockSplitVM>> Get(bool onlyMyStocks, DateTime startDate, DateTime endDate)
        {
            ResultResponseObject<IEnumerable<StockSplitVM>> result = null;

            return result;
        }

        public ResultResponseObject<IEnumerable<StockSplitVM>> GetByGuidAndDate(Guid stockGuid, DateTime startDate, DateTime endDate)
        {
            ResultResponseObject<IEnumerable<StockSplitVM>> result = null;

            using (_uow.Create())
            {
                ResultServiceObject<IEnumerable<StockSplit>> resultService = _stockSplitService.GetByGuidAndDate(stockGuid, startDate, endDate);

                result = _mapper.Map<ResultResponseObject<IEnumerable<StockSplitVM>>>(resultService);
            }

            return result;
        }


        public void CheckAndSendNotificationAboutSplit()
        {
            ResultServiceObject<IEnumerable<StockSplit>> resultService;

            using (_uow.Create())
            {
                resultService = _stockSplitService.Get(false, null, DateTime.Now.AddDays(-3).Date, DateTime.Now.AddDays(-3).Date);
            }

            foreach (var itemStockSplit in resultService.Value)
            {
                ResultServiceObject<IEnumerable<string>> resultUsersServiceObject;

                ResultServiceObject<Stock> resultStockServiceObject;

                //Get list of user that have this stock in portfolio
                using (_uow.Create())
                {
                    resultUsersServiceObject = _stockService.GetAllUsersWithStock(itemStockSplit.StockID);
                    resultStockServiceObject = _stockService.GetById(itemStockSplit.StockID);
                }

                foreach (var itemUser in resultUsersServiceObject.Value)
                {
                    using (_uow.Create())
                    {
                        string operationType = "AGRUPADA";

                        if (itemStockSplit.Unfolded)
                        {
                            operationType = "DESDOBRADA";
                        }

                        string pushMessage = string.Format("App Dividendos.me alerta! A ação {0} (que você tem na carteira) vai ser {4} na data {1} na proporção {2} para {3}. Fique atento!", resultStockServiceObject.Value.Symbol, itemStockSplit.DateSplit.ToString("dd/MM/yyyy"), itemStockSplit.ProportionFrom.ToString("n2", new CultureInfo("pt-br")), itemStockSplit.ProportionTo.ToString("n2", new CultureInfo("pt-br")), operationType);
                        string pushMessageTitle = "Vai acontecer algo importante com a sua carteira!";

                        ResultServiceObject<IEnumerable<Device>> devices = _deviceService.GetByUser(itemUser);

                        //_notificationHistoricalService.New(pushMessageTitle, pushMessage, itemUser, AppScreenNameEnum.HomeDiscovery.ToString(), PushRedirectTypeEnum.Internal.ToString(), null, _cacheService);

                        foreach (Device itemDevice in devices.Value)
                        {
                            try
                            {
                                //_notificationService.SendPush(pushMessageTitle, pushMessage, itemDevice, new PushRedirect() { PushRedirectType = PushRedirectTypeEnum.Internal, AppScreenName = AppScreenNameEnum.HomeDiscovery });
                            }
                            catch (Exception exception)
                            {
                                _logger.SendErrorAsync(exception);
                            }
                        }
                    }
                }
            }
        }

        public void ImportStockSplit(int idCountry)
        {
            DateTime now = DateTime.Now;

            List<StockSplitImport> stockSplitImports = _iInvestingComHelper.GetSplitsEvents(idCountry, now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month));

            if (stockSplitImports != null && stockSplitImports.Count() > 0)
            {
                foreach (StockSplitImport stockSplitImport in stockSplitImports)
                {
                    using (_uow.Create())
                    {
                        ResultServiceObject<Stock> resultStock = _stockService.GetBySymbolOrLikeOldSymbol(stockSplitImport.Symbol, idCountry);

                        if (resultStock.Value != null)
                        {

                            ResultServiceObject<StockSplit> resultStockSplit = _stockSplitService.GetBy(resultStock.Value.IdStock, stockSplitImport.SplitDate, idCountry);

                            if (resultStockSplit.Value == null)
                            {
                                StockSplit stockSplit = new StockSplit();
                                stockSplit.DateSplit = stockSplitImport.SplitDate;
                                stockSplit.StockID = resultStock.Value.IdStock;
                                stockSplit.IdCountry = idCountry;
                                stockSplit.ProportionFrom = stockSplitImport.ProportionFrom;
                                stockSplit.ProportionTo = stockSplitImport.ProportionTo;

                                _stockSplitService.Add(stockSplit);
                            }
                        }
                    }
                }
            }
        }

        public ResultResponseObject<StockSplitWrapperVM> GetStockSplits()
        {
            return GetStockSplits(_globalAuthenticationService.IdUser);
        }

        public ResultResponseObject<StockSplitWrapperVM> GetStockSplits(string idUser)
        {
            ResultResponseObject<StockSplitWrapperVM> resultResponseObject = new ResultResponseObject<StockSplitWrapperVM>();
            resultResponseObject.Success = true;

            using (_uow.Create())
            {
                DateTime limitDate = new DateTime(2021, 11, 10);
                ResultServiceObject<IEnumerable<Operation>> resultOperation = _operationService.GetOperationSplits(idUser, limitDate);

                if (resultOperation.Value != null && resultOperation.Value.Count() > 0)
                {
                    StockSplitWrapperVM stockSplitWrapperVM = new StockSplitWrapperVM();
                    stockSplitWrapperVM.StockSplits = new List<StockSplitVM>();

                    foreach (Operation operation in resultOperation.Value)
                    {
                        ResultServiceObject<CompanyView> resultCompanyView = _companyService.GetCompanyLogoDetails(operation.IdStock);
                        ResultServiceObject<StockSplit> resultStockSplit = _stockSplitService.GetLatestByIdStock(operation.IdStock);
                        StockSplit stockSplit = resultStockSplit.Value;

                        if (stockSplit != null && resultCompanyView.Value != null)
                        {
                            StockSplitVM stockSplitVM = new StockSplitVM();
                            decimal numberOfSharesAfter = 0;
                            decimal averagePriceAfter = 0;

                            if (stockSplit.ProportionFrom > stockSplit.ProportionTo)
                            {
                                if (stockSplit.ProportionFrom > 1 && stockSplit.ProportionFrom < 2)
                                {
                                    stockSplitVM.EventName = "Bonificação";
                                }
                                else
                                {
                                    stockSplitVM.EventName = "Desdobramento";
                                }

                                numberOfSharesAfter = operation.NumberOfShares * stockSplit.ProportionFrom;
                                averagePriceAfter = operation.AveragePrice / stockSplit.ProportionFrom;
                            }
                            else
                            {
                                stockSplitVM.EventName = "Agrupamento";
                                numberOfSharesAfter = operation.NumberOfShares / stockSplit.ProportionTo;
                                averagePriceAfter = operation.AveragePrice * stockSplit.ProportionTo;
                            }

                            if (stockSplit.IdCountry == 1)
                            {
                                numberOfSharesAfter = Math.Floor(numberOfSharesAfter);
                            }

                            stockSplitVM.AveragePrice = operation.AveragePrice.ToString("n2", new CultureInfo("pt-br"));
                            stockSplitVM.AveragePriceAfter = averagePriceAfter.ToString("n2", new CultureInfo("pt-br"));
                            stockSplitVM.NumberOfShares = operation.NumberOfShares.ToString("0.#############################", new CultureInfo("pt-br"));
                            stockSplitVM.NumberOfSharesAfter = numberOfSharesAfter.ToString("n5", new CultureInfo("pt-br"));
                            stockSplitVM.IdCountry = stockSplit.IdCountry;
                            stockSplitVM.ProportionFrom = stockSplit.ProportionFrom.ToString("g29", new CultureInfo("pt-br"));
                            stockSplitVM.ProportionTo = stockSplit.ProportionTo.ToString("g29", new CultureInfo("pt-br"));
                            stockSplitVM.Symbol = resultCompanyView.Value.Symbol;
                            stockSplitVM.Logo = resultCompanyView.Value.Logo;
                            stockSplitVM.Company = resultCompanyView.Value.Company;
                            stockSplitVM.Segment = resultCompanyView.Value.Segment;
                            stockSplitVM.IdOperation = operation.IdOperation;
                            stockSplitVM.GuidOperation = operation.GuidOperation;

                            ResultServiceObject<Portfolio> resultServicePortfolio = _portfolioService.GetById(operation.IdPortfolio);

                            if (resultServicePortfolio.Value != null)
                            {
                                stockSplitVM.PortfolioName = resultServicePortfolio.Value.Name;
                            }

                            stockSplitWrapperVM.StockSplits.Add(stockSplitVM);
                        }
                    }

                    resultResponseObject.Value = stockSplitWrapperVM;
                }
            }

            return resultResponseObject;
        }

        public ResultResponseObject<OperationEditAvgPriceVM> ApplyStockSplit(Guid guidOperation, OperationEditAvgPriceVM operationEditVM)
        {
            ResultResponseObject<OperationEditAvgPriceVM> resultResponse = new ResultResponseObject<OperationEditAvgPriceVM>();
            resultResponse.Success = false;

            using (_uow.Create())
            {
                ResultServiceObject<Operation> resultOperation = _operationService.GetByGuid(guidOperation);

                if (resultOperation.Value != null)
                {
                    ResultServiceObject<Portfolio> resultPortfolio = _portfolioService.GetById(resultOperation.Value.IdPortfolio);

                    if (resultPortfolio.Value != null)
                    {
                        DateTime? splitDate = null;

                        ResultServiceObject<IEnumerable<OperationItem>> resultOperationItem = _operationItemService.GetActiveByPortfolio(resultOperation.Value.IdPortfolio, true, resultOperation.Value.IdStock);

                        if (resultOperationItem.Value != null && resultOperationItem.Value.Count() > 0)
                        {
                            long idStock = resultOperationItem.Value.First().IdStock;
                            ResultServiceObject<Stock> resultStock = _stockService.GetById(idStock);

                            if (resultStock.Value != null)
                            {
                                List<OperationItem> operationItems = resultOperationItem.Value.ToList();
                                List<OperationItem> operationItemsSplit = _stockSplitService.ApplyStockSplit(ref operationItems, resultStock.Value.IdCountry);

                                if (operationItemsSplit != null && operationItemsSplit.Count() > 0)
                                {
                                    splitDate = operationItemsSplit.First().LastSplitDate;

                                    foreach (OperationItem operationItemSplit in operationItemsSplit)
                                    {
                                        if (resultPortfolio.Value.ManualPortfolio)
                                        {
                                            OperationEditVM operationEdit = new OperationEditVM();
                                            operationEdit.AcquisitionPrice = operationItemSplit.AcquisitionPrice.ToString("n2", new CultureInfo("pt-br"));
                                            operationEdit.Broker = operationItemSplit.HomeBroker;
                                            operationEdit.IdOperationItem = operationItemSplit.IdOperationItem;
                                            operationEdit.IdStock = operationItemSplit.IdStock;
                                            operationEdit.NumberOfShares = operationItemSplit.NumberOfShares.ToString("0.#############################", new CultureInfo("pt-br"));
                                            operationEdit.OperationDate = operationItemSplit.EventDate.Value.ToString("dd/MM/yyyy", new CultureInfo("pt-br"));
                                            operationEdit.Price = operationItemSplit.AveragePrice.ToString("n2", new CultureInfo("pt-br"));

                                            if (operationItemSplit.IdOperationType == 1)
                                            {
                                                _operationService.EditBuyOperation(resultPortfolio.Value.GuidPortfolio, operationEdit, _portfolioService, _operationItemService, _systemSettingsService, _portfolioPerformanceService, _stockService, _performanceStockService, _holidayService, _operationService);
                                            }
                                            else
                                            {
                                                _operationService.EditSellOperation(resultPortfolio.Value.GuidPortfolio, operationEdit, new ResultResponseBase(), _portfolioService, _operationItemService, _systemSettingsService, _portfolioPerformanceService, _stockService, _performanceStockService, _holidayService, _operationService);
                                            }

                                            _operationItemService.UpdateSplitDate(operationItemSplit.IdOperationItem, operationItemSplit.LastSplitDate);
                                        }
                                        else
                                        {
                                            _operationItemService.Update(operationItemSplit.IdOperationItem, operationItemSplit.NumberOfShares, operationItemSplit.AveragePrice, operationItemSplit.LastSplitDate);
                                        }
                                    }
                                }
                            }
                        }

                        if (!resultPortfolio.Value.ManualPortfolio)
                        {
                            resultResponse = _operationService.UpdateOperation(guidOperation, operationEditVM, _portfolioService, _operationService, _operationItemService, _operationItemHistService, _operationHistService, _systemSettingsService, _portfolioPerformanceService, _stockService, _performanceStockService, _holidayService, _logger, splitDate);
                        }

                        _cacheService.DeleteOnCache(string.Concat("HasStockSplit:", _globalAuthenticationService.IdUser));
                    }
                }

            }

            return resultResponse;
        }


        public ResultResponseObject<Guid> DiscardStockSplit(Guid guidOperation)
        {
            ResultResponseObject<Guid> resultResponse = new ResultResponseObject<Guid>();
            resultResponse.Value = guidOperation;
            resultResponse.Success = false;

            using (_uow.Create())
            {
                ResultServiceObject<Operation> resultOperation = _operationService.GetByGuid(guidOperation);

                if (resultOperation.Value != null)
                {
                    ResultServiceObject<Portfolio> resultPortfolio = _portfolioService.GetById(resultOperation.Value.IdPortfolio);

                    if (resultPortfolio.Value != null)
                    {
                        ResultServiceObject<IEnumerable<OperationItem>> resultOperationItem = _operationItemService.GetActiveByPortfolio(resultOperation.Value.IdPortfolio, true, resultOperation.Value.IdStock);

                        if (resultOperationItem.Value != null && resultOperationItem.Value.Count() > 0)
                        {
                            foreach (OperationItem operationItem in resultOperationItem.Value)
                            {
                                _operationItemService.UpdateSplitDate(operationItem.IdOperationItem, DateTime.Now.AddDays(1).Date);
                            }
                        }

                        _cacheService.DeleteOnCache(string.Concat("HasStockSplit:", _globalAuthenticationService.IdUser));
                    }
                }
            }

            return resultResponse;
        }
    }
}