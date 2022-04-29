using AutoMapper;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using Dividendos.Application.Base;
using Dividendos.Application.Interface;
using Dividendos.B3.Interface;
using Dividendos.B3.Interface.Model;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Enum;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using Dividendos.InvestidorB3.Interface;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using K.Logger;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Dividendos.Application
{
    public class ScrapySchedulerApp : BaseApp, IScrapySchedulerApp
    {
        private readonly IUnitOfWork _uow;
        private readonly IGlobalAuthenticationService _globalAuthenticationService;
        private readonly IImportB3Helper _iImportB3Helper;
        private readonly ITraderService _traderService;
        private readonly ICipherService _cipherService;
        private readonly IStockService _stockService;
        private readonly ISystemSettingsService _systemSettingsService;
        private readonly IPortfolioPerformanceService _portfolioPerformanceService;
        private readonly IOperationService _operationService;
        private readonly IPerformanceStockService _performanceStockService;
        private readonly IHolidayService _holidayService;
        private readonly IOperationHistService _operationHistService;
        private readonly IOperationItemHistService _operationItemHistService;
        private readonly ILogger _logger;
        private readonly IOperationItemService _operationItemService;
        private readonly IPortfolioService _portfolioService;
        private readonly IDividendService _dividendService;
        private readonly IDividendTypeService _dividendTypeService;
        private readonly IFinancialProductService _financialProductService;
        private readonly IDeviceService _deviceService;
        private readonly ISettingsService _settingsService;
        private readonly INotificationHistoricalService _notificationHistoricalService;
        private readonly ICacheService _cacheService;
        private readonly INotificationService _notificationService;
        private readonly IScrapySchedulerService _scrapySchedulerService;
        private readonly IScrapyAgentService _scrapyAgentService;
        private readonly ISubscriptionService _subscriptionService;
        private readonly IDividendCalendarService _dividendCalendarService;
        private readonly IStockSplitService _stockSplitService;
        private readonly IImportInvestidorB3Helper _iImportInvestidorB3Helper;

        public ScrapySchedulerApp(IUnitOfWork uow,
            IGlobalAuthenticationService globalAuthenticationService,
            IImportB3Helper iImportB3Helper,
            ITraderService traderService,
            ICipherService cipherService,
            IStockService stockService,
            ISystemSettingsService systemSettingsService,
            IPortfolioPerformanceService portfolioPerformanceService,
            IOperationService operationService,
            IPerformanceStockService performanceStockService,
            IHolidayService holidayService,
            IOperationHistService operationHistService,
            IOperationItemHistService operationItemHistService,
            ILogger logger,
            IOperationItemService operationItemService,
            IPortfolioService portfolioService,
            IDividendService dividendService,
            IDividendTypeService dividendTypeService,
            IFinancialProductService financialProductService,
            IDeviceService deviceService,
            ISettingsService settingsService,
            INotificationHistoricalService notificationHistoricalService,
            ICacheService cacheService,
            INotificationService notificationService,
            IScrapySchedulerService scrapySchedulerService,
            IScrapyAgentService scrapyAgentService,
            ISubscriptionService subscriptionService,
            IDividendCalendarService dividendCalendarService,
            IStockSplitService stockSplitService,
            IImportInvestidorB3Helper iImportInvestidorB3Helper)
        {
            _uow = uow;
            _globalAuthenticationService = globalAuthenticationService;
            _iImportB3Helper = iImportB3Helper;
            _traderService = traderService;
            _cipherService = cipherService;
            _stockService = stockService;
            _systemSettingsService = systemSettingsService;
            _portfolioPerformanceService = portfolioPerformanceService;
            _operationService = operationService;
            _performanceStockService = performanceStockService;
            _holidayService = holidayService;
            _operationHistService = operationHistService;
            _operationItemHistService = operationItemHistService;
            _logger = logger;
            _operationItemService = operationItemService;
            _portfolioService = portfolioService;
            _dividendService = dividendService;
            _dividendTypeService = dividendTypeService;
            _financialProductService = financialProductService;
            _deviceService = deviceService;
            _settingsService = settingsService;
            _notificationHistoricalService = notificationHistoricalService;
            _cacheService = cacheService;
            _notificationService = notificationService;
            _scrapySchedulerService = scrapySchedulerService;
            _scrapyAgentService = scrapyAgentService;
            _subscriptionService = subscriptionService;
            _dividendCalendarService = dividendCalendarService;
            _stockSplitService = stockSplitService;
            _iImportInvestidorB3Helper = iImportInvestidorB3Helper;
        }

        public async Task RunParallelCei(double defaulTimeout, string agentName, int amountItems)
        {
            if (!CheckCeiOffline())
            {
                var cancellationTokenSource = new CancellationTokenSource();
                CancellationToken cancellationToken = cancellationTokenSource.Token;
                cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(Convert.ToDouble(defaulTimeout)));
                List<ScrapyScheduler> scrapySchedulers = new List<ScrapyScheduler>();
                using (_uow.Create())
                {
                    ResultServiceObject<int> resultServiceCountScrapy = _scrapySchedulerService.CountJobsRunningOrAwaiting(agentName);
                    int totalPerAgent = resultServiceCountScrapy.Value;
                    int totalNextRun = amountItems - totalPerAgent;

                    if (totalNextRun > 0)
                    {
                        ResultServiceObject<IEnumerable<ScrapyScheduler>> resultServiceScrapy = _scrapySchedulerService.GetNextScrapyItems(totalNextRun);

                        if (resultServiceScrapy.Value != null && resultServiceScrapy.Value.Count() > 0)
                        {
                            foreach (ScrapyScheduler scrapyScheduler in resultServiceScrapy.Value)
                            {
                                scrapyScheduler.Agent = agentName;
                                scrapyScheduler.Status = (int)ScrapySchedulerStatusEnum.Running;
                                scrapyScheduler.StartDate = DateTime.Now;
                                scrapyScheduler.WaitingTime = DateTime.Now.Date.Add(scrapyScheduler.StartDate.Value.Subtract(scrapyScheduler.CreatedDate));
                                _scrapySchedulerService.UpdateRunning(scrapyScheduler.IdScrapyScheduler, scrapyScheduler.Status, scrapyScheduler.Agent, scrapyScheduler.StartDate.Value, scrapyScheduler.WaitingTime.Value);

                                scrapySchedulers.Add(scrapyScheduler);
                            }
                        }
                    }
                }

                if (scrapySchedulers != null && scrapySchedulers.Count > 0)
                {
                    Func<string, string, string, bool, string, CancellationTokenSource, ImportCeiResult> funcImportCei = _iImportB3Helper.ImportCei;
                    var tpTasks = new List<(ScrapyScheduler, Task<ImportCeiResult>)>();
                    DateTime now = DateTime.Now;

                    foreach (ScrapyScheduler scrapyScheduler in scrapySchedulers)
                    {
                        DateTime? lastEventDate = null;
                        string eventDate = string.Empty;

                        using (_uow.Create())
                        {
                            lastEventDate = _operationItemService.GetLastEventDate(scrapyScheduler.IdUser, scrapyScheduler.Identifier, scrapyScheduler.Password, TraderTypeEnum.RendaVariavelAndTesouroDiretoCEI);

                            if (lastEventDate.HasValue)
                            {
                                ResultServiceObject<Trader> resultTrader = _traderService.GetByIdentifierAndUserActive(scrapyScheduler.Identifier, scrapyScheduler.IdUser, TraderTypeEnum.RendaVariavelAndTesouroDiretoCEI);

                                if (lastEventDate.Value.Date < now.Date && lastEventDate.Value.Date < resultTrader.Value.LastSync)
                                {
                                    lastEventDate = lastEventDate.Value.AddDays(1);
                                }

                                eventDate = lastEventDate.Value.ToString("dd/MM/yyyy");
                            }
                        }

                        Task<ImportCeiResult> task = new Task<ImportCeiResult>(() => funcImportCei(scrapyScheduler.Identifier, scrapyScheduler.Password, scrapyScheduler.IdUser, scrapyScheduler.AutomaticImport, eventDate, cancellationTokenSource), cancellationToken);
                        tpTasks.Add((scrapyScheduler, task));
                    }

                    var tasks = tpTasks.Select(t => t.Item2).ToList();

                    foreach (var task in tasks)
                    {
                        task.Start();
                    }

                    while (tasks.Count > 0)
                    {
                        try
                        {
                            // Fetch first completed Task
                            Task<ImportCeiResult> currentCompleted = await Task.WhenAny(tasks);

                            var tpFound = tpTasks.FirstOrDefault(tp => tp.Item2.Id == currentCompleted.Id);
                            ScrapyScheduler scrapyScheduler = tpFound.Item1;

                            // Compare Condition
                            if (currentCompleted.Status == TaskStatus.RanToCompletion)
                            {
                                tasks.Remove(currentCompleted);
                                using (_uow.Create())
                                {
                                    FinishTask(scrapyScheduler, currentCompleted, ScrapySchedulerStatusEnum.Completed);
                                }
                            }
                            else if (currentCompleted.Status == TaskStatus.Canceled || currentCompleted.Status == TaskStatus.Faulted)
                            {
                                tasks.Remove(currentCompleted);
                                using (_uow.Create())
                                {
                                    FinishTask(scrapyScheduler, currentCompleted, ScrapySchedulerStatusEnum.Canceled);

                                    //ResultServiceObject<Trader> resultTrader = _traderService.GetByIdentifierAndUser(scrapyScheduler.Identifier, scrapyScheduler.IdUser, TraderTypeEnum.RendaVariavelAndTesouroDiretoCEI);

                                    //if (resultTrader.Value != null)
                                    //{
                                    //    ResultServiceObject<ScrapyAgent> resultScrapyAgent = _scrapyAgentService.GetByTrader(resultTrader.Value.IdTrader);

                                    //    if (resultScrapyAgent.Value == null)
                                    //    {
                                    //        ScrapyAgent scrapyAgent = new ScrapyAgent();
                                    //        scrapyAgent.IdTrader = resultTrader.Value.IdTrader;

                                    //        scrapyAgent = _scrapyAgentService.Add(scrapyAgent).Value;
                                    //    }
                                    //}
                                }
                            }
                        }
                        catch (OperationCanceledException e)
                        {
                            //Console.WriteLine($"{nameof(OperationCanceledException)} thrown with message: {e.Message}");
                        }
                        catch (Exception ex)
                        {

                        }
                        finally
                        {
                            //tokenSource2.Dispose();
                        }
                    }
                }
            }
        }

        private void FinishTask(ScrapyScheduler scrapySchedulerFinish, Task<ImportCeiResult> currentCompleted, ScrapySchedulerStatusEnum scrapySchedulerStatusEnum)
        {
            ImportCeiResult importCeiResult = null;
            bool retry = false;
            string json = string.Empty;

            if (currentCompleted.Status == TaskStatus.RanToCompletion)
            {
                importCeiResult = currentCompleted.Result;

                if (importCeiResult != null)
                {
                    ImportCeiResultView importCeiResultView = ConvertCeiResult(importCeiResult);

                    if ((string.IsNullOrWhiteSpace(importCeiResult.Json) && !importCeiResult.ErrorCEI) || importCeiResult.ErrorCode == 102)
                    {
                        scrapySchedulerStatusEnum = ScrapySchedulerStatusEnum.Canceled;
                        retry = true;
                    }
                    else
                    {
                        importCeiResultView = _portfolioService.FinishImportCei(importCeiResult.Identifier, importCeiResult.Password, importCeiResult.IdUser, importCeiResult.AutomaticProcess, importCeiResultView, _traderService, _cipherService, _stockService, _systemSettingsService, _portfolioPerformanceService, _operationService, _performanceStockService, _holidayService, _operationHistService, _operationItemHistService, _logger, _operationItemService, _portfolioService, _dividendService, _dividendTypeService, _financialProductService, _deviceService, _settingsService, _notificationHistoricalService, _cacheService, _notificationService, _dividendCalendarService, _stockSplitService);
                        json = Regex.Replace(importCeiResult.Json, @"\s+", "");
                    }
                }
            }

            if (scrapySchedulerFinish != null)
            {
                scrapySchedulerFinish.Status = (int)scrapySchedulerStatusEnum;
                scrapySchedulerFinish.FinishDate = DateTime.Now;
                scrapySchedulerFinish.ExecutionTime = DateTime.Now.Date.Add(scrapySchedulerFinish.FinishDate.Value.Subtract(scrapySchedulerFinish.StartDate.Value));
                scrapySchedulerFinish.Results = json;

                if (scrapySchedulerStatusEnum == ScrapySchedulerStatusEnum.Completed)
                {
                    scrapySchedulerFinish.Sent = true;
                    scrapySchedulerFinish.TimedOut = false;
                }
                else if (scrapySchedulerStatusEnum == ScrapySchedulerStatusEnum.Canceled)
                {
                    scrapySchedulerFinish.Sent = false;
                    scrapySchedulerFinish.TimedOut = true;
                }

                _scrapySchedulerService.UpdateFinishTask(scrapySchedulerFinish.IdScrapyScheduler, scrapySchedulerFinish.Status, scrapySchedulerFinish.FinishDate.Value, scrapySchedulerFinish.ExecutionTime.Value, scrapySchedulerFinish.Results, scrapySchedulerFinish.Sent, scrapySchedulerFinish.TimedOut, string.Empty);
                //_scrapySchedulerService.Update(scrapySchedulerFinish);
            }

            if (retry && importCeiResult != null)
            {
                _scrapySchedulerService.CreateTask(importCeiResult.Identifier, importCeiResult.Password, importCeiResult.IdUser, importCeiResult.AutomaticProcess, _traderService, _scrapySchedulerService, _subscriptionService);
            }
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

        public void RenewBlockedTasks(string resetTime)
        {
            ResultServiceObject<IEnumerable<ScrapyScheduler>> resultJobs = new ResultServiceObject<IEnumerable<ScrapyScheduler>>();

            using (_uow.Create())
            {
                resultJobs = _scrapySchedulerService.GetJobsRunning();

                if (resultJobs.Value != null && resultJobs.Value.Count() > 0)
                {
                    double resetSeconds = Convert.ToDouble(resetTime);
                    resultJobs.Value = resultJobs.Value.Where(scp => DateTime.Now.Subtract(scp.StartDate.Value).TotalSeconds >= resetSeconds && scp.Agent != "Clear");
                }
            }

            if (resultJobs.Value != null && resultJobs.Value.Count() > 0)
            {
                foreach (ScrapyScheduler scrapyScheduler in resultJobs.Value)
                {
                    using (_uow.Create())
                    {
                        scrapyScheduler.Agent = string.Empty;
                        scrapyScheduler.Status = (int)ScrapySchedulerStatusEnum.Awaiting;
                        _scrapySchedulerService.UpdateRenewTask(scrapyScheduler.IdScrapyScheduler, scrapyScheduler.Status, scrapyScheduler.Agent);
                    }
                }
            }
        }

        public void DeleteOldCompletedTasks(int days)
        {
            ResultServiceObject<IEnumerable<ScrapyScheduler>> resultJobs = new ResultServiceObject<IEnumerable<ScrapyScheduler>>();

            using (_uow.Create())
            {
                resultJobs = _scrapySchedulerService.GetCompletedTasks(days);
            }

            if (resultJobs.Value != null && resultJobs.Value.Count() > 0)
            {
                foreach (ScrapyScheduler scrapyScheduler in resultJobs.Value)
                {
                    using (_uow.Create())
                    {
                        _scrapySchedulerService.Delete(scrapyScheduler);
                    }
                }
            }
        }

        private static bool CheckCeiOffline()
        {
            bool zeroPrice = false;
            DateTime timeUtc = DateTime.UtcNow;
            var brasilia = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            DateTime dtBrasilia = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, brasilia);

            TimeSpan start = new TimeSpan(1, 15, 0); //10 o'clock
            TimeSpan end = new TimeSpan(2, 3, 0); //12 o'clock
            TimeSpan now = dtBrasilia.TimeOfDay;

            if ((now > start) && (now < end))
            {
                zeroPrice = true;
            }

            return zeroPrice;
        }

        public void RunCeiDirect(string identifier, string password, string idUser)
        {
            string eventDate = string.Empty;
            DateTime? lastEventDate = null;

            using (_uow.Create())
            {
                DateTime now = DateTime.Now;
                lastEventDate = _operationItemService.GetLastEventDate(idUser, identifier, password, TraderTypeEnum.RendaVariavelAndTesouroDiretoCEI);

                if (lastEventDate.HasValue)
                {
                    ResultServiceObject<Trader> resultTrader = _traderService.GetByIdentifierAndUserActive(identifier, idUser, TraderTypeEnum.RendaVariavelAndTesouroDiretoCEI);

                    if (lastEventDate.Value.Date < now.Date && lastEventDate.Value.Date < resultTrader.Value.LastSync)
                    {
                        lastEventDate = lastEventDate.Value.AddDays(1);
                    }

                    eventDate = lastEventDate.Value.ToString("dd/MM/yyyy");
                }
            }

            ImportCeiResult importCeiResult = _iImportB3Helper.ImportCei(identifier, password, idUser, true, eventDate, null);
            ImportCeiResultView importCeiResultView = ConvertCeiResult(importCeiResult);

            using (_uow.Create())
            {
                importCeiResultView = _portfolioService.FinishImportCei(importCeiResult.Identifier, importCeiResult.Password, importCeiResult.IdUser, importCeiResult.AutomaticProcess, importCeiResultView, _traderService, _cipherService, _stockService, _systemSettingsService, _portfolioPerformanceService, _operationService, _performanceStockService, _holidayService, _operationHistService, _operationItemHistService, _logger, _operationItemService, _portfolioService, _dividendService, _dividendTypeService, _financialProductService, _deviceService, _settingsService, _notificationHistoricalService, _cacheService, _notificationService, _dividendCalendarService, _stockSplitService);
            }
        }

        public async Task RunParallelNewCei(double defaulTimeout, string agentName, int amountItems)
        {
            List<Dividendos.InvestidorB3.Interface.Model.ImportCeiResult> scrapySchedulersCompleted = new List<Dividendos.InvestidorB3.Interface.Model.ImportCeiResult>();

            //if (CheckCeiOffline())
            //{
            var cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;
            cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(Convert.ToDouble(defaulTimeout)));
            List<ScrapyScheduler> scrapySchedulers = new List<ScrapyScheduler>();
            using (_uow.Create())
            {
                ResultServiceObject<int> resultServiceCountScrapy = _scrapySchedulerService.CountJobsRunningOrAwaiting(agentName, true);
                int totalPerAgent = resultServiceCountScrapy.Value;
                int totalNextRun = amountItems - totalPerAgent;

                if (totalNextRun > 0)
                {
                    ResultServiceObject<IEnumerable<ScrapyScheduler>> resultServiceScrapy = _scrapySchedulerService.GetNextScrapyItems(totalNextRun, true);

                    if (resultServiceScrapy.Value != null && resultServiceScrapy.Value.Count() > 0)
                    {
                        foreach (ScrapyScheduler scrapyScheduler in resultServiceScrapy.Value)
                        {
                            scrapyScheduler.Agent = agentName;
                            scrapyScheduler.Status = (int)ScrapySchedulerStatusEnum.Running;
                            scrapyScheduler.StartDate = DateTime.Now;
                            scrapyScheduler.WaitingTime = DateTime.Now.Date.Add(scrapyScheduler.StartDate.Value.Subtract(scrapyScheduler.CreatedDate));
                            _scrapySchedulerService.UpdateRunning(scrapyScheduler.IdScrapyScheduler, scrapyScheduler.Status, scrapyScheduler.Agent, scrapyScheduler.StartDate.Value, scrapyScheduler.WaitingTime.Value);

                            scrapySchedulers.Add(scrapyScheduler);
                        }
                    }
                }
            }

            if (scrapySchedulers != null && scrapySchedulers.Count > 0)
            {
                Func<string, bool, DateTime, DateTime?, DateTime?, CancellationTokenSource, Dividendos.InvestidorB3.Interface.Model.ImportCeiResult> funcImportCei = _iImportInvestidorB3Helper.ImportAllInvestments;
                var tpTasks = new List<(ScrapyScheduler, Task<Dividendos.InvestidorB3.Interface.Model.ImportCeiResult>)>();
                DateTime now = DateTime.Now;
                DateTime endReferenceDate = now;

                foreach (ScrapyScheduler scrapyScheduler in scrapySchedulers)
                {
                    DateTime? lastEventDate = null;
                    DateTime? lastSync = null;

                    using (_uow.Create())
                    {
                        ResultServiceObject<Trader> resultTraderNewCei = _traderService.GetByIdentifierAndUserActive(scrapyScheduler.Identifier, scrapyScheduler.IdUser, TraderTypeEnum.RendaVariavelAndTesouroDiretoNewCEI);
                        Portfolio portfolio = null;

                        if (resultTraderNewCei.Value != null)
                        {
                            portfolio = _portfolioService.GetByTrader(resultTraderNewCei.Value.IdTrader).Value;
                        }

                        if (portfolio == null || !portfolio.Active)
                        {
                            ResultServiceObject<Trader> resultTraderOldCei = _traderService.GetByIdentifierAndUserActive(scrapyScheduler.Identifier, scrapyScheduler.IdUser, TraderTypeEnum.RendaVariavelAndTesouroDiretoCEI);

                            if (resultTraderOldCei.Value == null)
                            {
                                resultTraderOldCei = _traderService.GetLatestInactiveCei(scrapyScheduler.IdUser);
                            }

                            if (resultTraderOldCei.Value != null)
                            {
                                lastEventDate = _operationItemService.GetLastEventDate(resultTraderOldCei.Value.IdUser, resultTraderOldCei.Value.Identifier, resultTraderOldCei.Value.Password, TraderTypeEnum.RendaVariavelAndTesouroDiretoCEI, resultTraderOldCei.Value.IdTrader);
                                lastSync = resultTraderOldCei.Value.LastSync;
                            }
                        }
                        else
                        {
                            lastEventDate = _operationItemService.GetLastEventDate(resultTraderNewCei.Value.IdUser, resultTraderNewCei.Value.Identifier, resultTraderNewCei.Value.Password, TraderTypeEnum.RendaVariavelAndTesouroDiretoNewCEI);
                            lastSync = resultTraderNewCei.Value.LastSync;
                        }

                        if (lastEventDate.HasValue)
                        {
                            if (lastEventDate.Value.Date < now.Date && lastSync.HasValue && lastEventDate.Value.Date < lastSync.Value)
                            {
                                lastEventDate = lastEventDate.Value.AddDays(1);
                            }
                        }

                        bool isNightCei = CheckCeiNight();
                        endReferenceDate = _holidayService.PreviousWorkDay(1, isNightCei);

                        //endReferenceDate = _holidayService.PreviousWorkDay(1);

                        //if (CheckCeiNight())
                        //{
                        //    endReferenceDate = endReferenceDate.AddDays(-1);
                        //}
                    }

                    Task<Dividendos.InvestidorB3.Interface.Model.ImportCeiResult> task = new Task<Dividendos.InvestidorB3.Interface.Model.ImportCeiResult>(() => funcImportCei(scrapyScheduler.Identifier, scrapyScheduler.AutomaticImport, endReferenceDate, lastEventDate, lastSync, cancellationTokenSource), cancellationToken);
                    tpTasks.Add((scrapyScheduler, task));
                }

                var tasks = tpTasks.Select(t => t.Item2).ToList();

                foreach (var task in tasks)
                {
                    task.Start();
                }

                while (tasks.Count > 0)
                {

                    try
                    {
                        // Fetch first completed Task
                        Task<Dividendos.InvestidorB3.Interface.Model.ImportCeiResult> currentCompleted = await Task.WhenAny(tasks);

                        var tpFound = tpTasks.FirstOrDefault(tp => tp.Item2.Id == currentCompleted.Id);
                        ScrapyScheduler scrapyScheduler = tpFound.Item1;

                        scrapySchedulersCompleted.Add(currentCompleted.Result);

                        // Compare Condition
                        if (currentCompleted.Status == TaskStatus.RanToCompletion)
                        {
                            tasks.Remove(currentCompleted);
                            using (_uow.Create())
                            {
                                FinishTaskNewCei(scrapyScheduler, currentCompleted, ScrapySchedulerStatusEnum.Completed);
                            }
                        }
                        else if (currentCompleted.Status == TaskStatus.Canceled || currentCompleted.Status == TaskStatus.Faulted)
                        {
                            tasks.Remove(currentCompleted);
                            using (_uow.Create())
                            {
                                FinishTaskNewCei(scrapyScheduler, currentCompleted, ScrapySchedulerStatusEnum.Canceled);
                                _portfolioService.SendNotificationImportation(scrapyScheduler.AutomaticImport, scrapyScheduler.IdUser, false, "Houve um erro durante a importação. Tente novamente, caso o erro persista contate o nosso suporte", _deviceService, _notificationHistoricalService, _cacheService, _notificationService, _logger, false);
                            }
                        }
                    }
                    catch (OperationCanceledException e)
                    {
                        string po = "";
                        //Console.WriteLine($"{nameof(OperationCanceledException)} thrown with message: {e.Message}");

                        //using (_uow.Create())
                        //{
                        //    scrapyScheduler.Results = e.Message;
                        //    FinishTaskNewCei(scrapyScheduler, null, ScrapySchedulerStatusEnum.Retry);
                        //}
                    }
                    catch (Exception ex)
                    {
                        string po = "";

                        //using (_uow.Create())
                        //{
                        //    scrapyScheduler.Results = ex.Message;
                        //    FinishTaskNewCei(scrapyScheduler, null, ScrapySchedulerStatusEnum.Retry);
                        //}
                    }
                    finally
                    {
                        //tokenSource2.Dispose();
                    }
                }
            }
            //}
        }

        private void FinishTaskNewCei(ScrapyScheduler scrapySchedulerFinish, Task<Dividendos.InvestidorB3.Interface.Model.ImportCeiResult> currentCompleted, ScrapySchedulerStatusEnum scrapySchedulerStatusEnum)
        {
            Dividendos.InvestidorB3.Interface.Model.ImportCeiResult importCeiResult = null;
            bool retry = false;
            string json = string.Empty;

            if (currentCompleted != null && currentCompleted.Status == TaskStatus.RanToCompletion)
            {
                importCeiResult = currentCompleted.Result;

                if (importCeiResult != null)
                {
                    retry = importCeiResult.Retry;

                    if (!importCeiResult.Retry)
                    {
                        ImportCeiResultView importCeiResultView = ConvertNewCeiResult(importCeiResult);
                        importCeiResultView = _portfolioService.FinishImportCei(scrapySchedulerFinish.Identifier, string.Empty, scrapySchedulerFinish.IdUser, scrapySchedulerFinish.AutomaticImport, importCeiResultView, _traderService, _cipherService, _stockService, _systemSettingsService, _portfolioPerformanceService, _operationService, _performanceStockService, _holidayService, _operationHistService, _operationItemHistService, _logger, _operationItemService, _portfolioService, _dividendService, _dividendTypeService, _financialProductService, _deviceService, _settingsService, _notificationHistoricalService, _cacheService, _notificationService, _dividendCalendarService, _stockSplitService, true);
                    }
                }
            }

            if (scrapySchedulerFinish != null)
            {
                if (retry)
                {
                    scrapySchedulerStatusEnum = ScrapySchedulerStatusEnum.Retry;
                }

                scrapySchedulerFinish.Status = (int)scrapySchedulerStatusEnum;
                scrapySchedulerFinish.FinishDate = DateTime.Now;
                scrapySchedulerFinish.ExecutionTime = DateTime.Now.Date.Add(scrapySchedulerFinish.FinishDate.Value.Subtract(scrapySchedulerFinish.StartDate.Value));
                scrapySchedulerFinish.Results = importCeiResult != null ? importCeiResult.Message : string.Empty;
                scrapySchedulerFinish.ResponseBody = importCeiResult != null ? importCeiResult.Json : string.Empty;

                if (scrapySchedulerStatusEnum == ScrapySchedulerStatusEnum.Completed)
                {
                    scrapySchedulerFinish.Sent = true;
                    scrapySchedulerFinish.TimedOut = false;
                }
                else if (scrapySchedulerStatusEnum == ScrapySchedulerStatusEnum.Canceled)
                {
                    scrapySchedulerFinish.Sent = false;
                    scrapySchedulerFinish.TimedOut = true;
                }
                else if (scrapySchedulerStatusEnum == ScrapySchedulerStatusEnum.Retry)
                {
                    scrapySchedulerFinish.Sent = false;
                    scrapySchedulerFinish.TimedOut = false;
                }

                if (scrapySchedulerStatusEnum == ScrapySchedulerStatusEnum.Canceled && currentCompleted != null && currentCompleted.Exception != null)
                {
                    scrapySchedulerFinish.Results = currentCompleted.Exception.Message;
                }

                _scrapySchedulerService.UpdateFinishTask(scrapySchedulerFinish.IdScrapyScheduler, scrapySchedulerFinish.Status, scrapySchedulerFinish.FinishDate.Value, scrapySchedulerFinish.ExecutionTime.Value, scrapySchedulerFinish.Results, scrapySchedulerFinish.Sent, scrapySchedulerFinish.TimedOut, scrapySchedulerFinish.ResponseBody);
                //_scrapySchedulerService.Update(scrapySchedulerFinish);

                if (retry)
                {
                    _scrapySchedulerService.CreateTask(scrapySchedulerFinish.Identifier, scrapySchedulerFinish.Password, scrapySchedulerFinish.IdUser, importCeiResult.AutomaticProcess, _traderService, _scrapySchedulerService, _subscriptionService, false, TraderTypeEnum.RendaVariavelAndTesouroDiretoNewCEI);
                }
            }
        }

        private ImportCeiResultView ConvertNewCeiResult(Dividendos.InvestidorB3.Interface.Model.ImportCeiResult importCeiResult)
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
                foreach (Dividendos.InvestidorB3.Interface.Model.DividendImport dividendImport in importCeiResult.ListDividend)
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
                foreach (Dividendos.InvestidorB3.Interface.Model.StockOperation stockOperation in importCeiResult.ListStockOperation)
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
                foreach (Dividendos.InvestidorB3.Interface.Model.StockOperation stockOperation in importCeiResult.ListStockPortfolio)
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
                foreach (Dividendos.InvestidorB3.Interface.Model.TesouroDiretoImport tesouroDiretoImport in importCeiResult.ListTesouroDireto)
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
                foreach (Dividendos.InvestidorB3.Interface.Model.StockOperation stockOperation in importCeiResult.ListStockAveragePrice)
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

        public void RenewBlockedTasksNewCei(string resetTime)
        {
            ResultServiceObject<IEnumerable<ScrapyScheduler>> resultJobs = new ResultServiceObject<IEnumerable<ScrapyScheduler>>();

            using (_uow.Create())
            {
                resultJobs = _scrapySchedulerService.GetJobsRunning(true);

                if (resultJobs.Value != null && resultJobs.Value.Count() > 0)
                {
                    double resetSeconds = Convert.ToDouble(resetTime);
                    resultJobs.Value = resultJobs.Value.Where(scp => DateTime.Now.Subtract(scp.StartDate.Value).TotalSeconds >= resetSeconds);
                }
            }

            if (resultJobs.Value != null && resultJobs.Value.Count() > 0)
            {
                foreach (ScrapyScheduler scrapyScheduler in resultJobs.Value)
                {
                    using (_uow.Create())
                    {
                        scrapyScheduler.Agent = string.Empty;
                        scrapyScheduler.Status = (int)ScrapySchedulerStatusEnum.Awaiting;
                        _scrapySchedulerService.UpdateRenewTask(scrapyScheduler.IdScrapyScheduler, scrapyScheduler.Status, scrapyScheduler.Agent);
                    }
                }
            }
        }

        public void DeleteOldCompletedTasksNewCei(int days)
        {
            ResultServiceObject<IEnumerable<ScrapyScheduler>> resultJobs = new ResultServiceObject<IEnumerable<ScrapyScheduler>>();

            using (_uow.Create())
            {
                resultJobs = _scrapySchedulerService.GetCompletedTasks(days, true);
            }

            if (resultJobs.Value != null && resultJobs.Value.Count() > 0)
            {
                foreach (ScrapyScheduler scrapyScheduler in resultJobs.Value)
                {
                    using (_uow.Create())
                    {
                        _scrapySchedulerService.Delete(scrapyScheduler);
                    }
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
            TimeSpan end = new TimeSpan(12, 0, 0); //6 o'clock
            TimeSpan now = dtBrasilia.TimeOfDay;

            if ((now > start) && (now < end))
            {
                zeroPrice = true;
            }

            return zeroPrice;
        }

        public void RunNewCeiDirect(string identifier, string idUser)
        {
            string eventDate = string.Empty;
            DateTime? lastEventDate = null;
            DateTime? lastSync = null;
            DateTime endReferenceDate = DateTime.Now;

            using (_uow.Create())
            {
                DateTime now = DateTime.Now;
                //lastEventDate = _operationItemService.GetLastEventDate(idUser, identifier, string.Empty, TraderTypeEnum.RendaVariavelAndTesouroDiretoNewCEI);

                //if (lastEventDate.HasValue)
                //{
                //    ResultServiceObject<Trader> resultTrader = _traderService.GetByIdentifierAndUserActive(identifier, idUser, TraderTypeEnum.RendaVariavelAndTesouroDiretoNewCEI);

                //    if (lastEventDate.Value.Date < now.Date && lastEventDate.Value.Date < resultTrader.Value.LastSync)
                //    {
                //        lastEventDate = lastEventDate.Value.AddDays(1);
                //    }

                //    eventDate = lastEventDate.Value.ToString("dd/MM/yyyy");
                //}


                ResultServiceObject<Trader> resultTraderNewCei = _traderService.GetByIdentifierAndUserActive(identifier, idUser, TraderTypeEnum.RendaVariavelAndTesouroDiretoNewCEI);

                Portfolio portfolio = null;

                if (resultTraderNewCei.Value != null)
                {
                    portfolio = _portfolioService.GetByTrader(resultTraderNewCei.Value.IdTrader).Value;
                }

                if (portfolio == null || !portfolio.Active)
                {
                    ResultServiceObject<Trader> resultTraderOldCei = _traderService.GetByIdentifierAndUserActive(identifier, idUser, TraderTypeEnum.RendaVariavelAndTesouroDiretoCEI);

                    if (resultTraderOldCei.Value == null)
                    {
                        resultTraderOldCei = _traderService.GetLatestInactiveCei(idUser);
                    }

                    if (resultTraderOldCei.Value != null)
                    {
                        lastEventDate = _operationItemService.GetLastEventDate(resultTraderOldCei.Value.IdUser, resultTraderOldCei.Value.Identifier, resultTraderOldCei.Value.Password, TraderTypeEnum.RendaVariavelAndTesouroDiretoCEI, resultTraderOldCei.Value.IdTrader);
                        lastSync = resultTraderOldCei.Value.LastSync;
                    }
                }
                else
                {
                    lastEventDate = _operationItemService.GetLastEventDate(resultTraderNewCei.Value.IdUser, resultTraderNewCei.Value.Identifier, resultTraderNewCei.Value.Password, TraderTypeEnum.RendaVariavelAndTesouroDiretoNewCEI);
                    lastSync = resultTraderNewCei.Value.LastSync;
                }

                if (lastEventDate.HasValue)
                {
                    if (lastEventDate.Value.Date < now.Date && lastSync.HasValue && lastEventDate.Value.Date < lastSync.Value)
                    {
                        lastEventDate = lastEventDate.Value.AddDays(1);
                    }
                }

                bool isNightCei = CheckCeiNight();
                endReferenceDate = _holidayService.PreviousWorkDay(1, isNightCei);
            }

            Dividendos.InvestidorB3.Interface.Model.ImportCeiResult importCeiResult = _iImportInvestidorB3Helper.ImportAllInvestments(identifier, true, endReferenceDate, lastEventDate, lastSync, null);
            ImportCeiResultView importCeiResultView = ConvertNewCeiResult(importCeiResult);

            using (_uow.Create())
            {
                importCeiResultView = _portfolioService.FinishImportCei(identifier, string.Empty, idUser, true, importCeiResultView, _traderService, _cipherService, _stockService, _systemSettingsService, _portfolioPerformanceService, _operationService, _performanceStockService, _holidayService, _operationHistService, _operationItemHistService, _logger, _operationItemService, _portfolioService, _dividendService, _dividendTypeService, _financialProductService, _deviceService, _settingsService, _notificationHistoricalService, _cacheService, _notificationService, _dividendCalendarService, _stockSplitService, true);
            }
        }

        public void EnqueueNewB3Traders()
        {
            using (_uow.Create())
            {
                IEnumerable<Trader> traders = _traderService.GetTradersInactiveB3().Value;

                if (traders != null && traders.Count() > 0)
                {
                    foreach (Trader trader in traders)
                    {
                        _scrapySchedulerService.CreateTask(trader.Identifier, trader.Password, trader.IdUser, false, _traderService, _scrapySchedulerService, _subscriptionService, false, TraderTypeEnum.RendaVariavelAndTesouroDiretoNewCEI);

                        trader.Active = true;

                        _traderService.Update(trader);
                    }
                }
            }
        }
    }
}
