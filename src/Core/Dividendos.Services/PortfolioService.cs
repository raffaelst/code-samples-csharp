using FluentValidation.Results;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Repository.Interface;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using Dividendos.Service.Validator.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dividendos.Entity.Views;
using Dividendos.Entity.Enum;
using K.Logger;
using Dividendos.API.Model.Request.Operation;
using System.Globalization;
using System.Text;

namespace Dividendos.Service
{
    public class PortfolioService : BaseService, IPortfolioService
    {
        //public List<string> ChangedCeiItems { get; set; }
        //public List<string> DividendCeiItems { get; set; }

        public PortfolioService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public ResultServiceObject<IEnumerable<Portfolio>> GetLastPortfoliosWithoutCalculation()
        {
            ResultServiceObject<IEnumerable<Portfolio>> resultService = new ResultServiceObject<IEnumerable<Portfolio>>();

            IEnumerable<Portfolio> portfolios = _uow.PortfolioRepository.GetLastPortfoliosWithoutCalculation(DateTime.Now.Date);

            resultService.Value = portfolios;

            return resultService;
        }

        public ResultServiceObject<Portfolio> GetByTrader(long idTrader)
        {
            ResultServiceObject<Portfolio> resultService = new ResultServiceObject<Portfolio>();

            IEnumerable<Portfolio> portfolios = _uow.PortfolioRepository.Select(p => p.IdTrader == idTrader);

            resultService.Value = portfolios.LastOrDefault();

            return resultService;
        }

        public ResultServiceObject<Portfolio> GetByTraderActive(long idTrader)
        {
            ResultServiceObject<Portfolio> resultService = new ResultServiceObject<Portfolio>();

            IEnumerable<Portfolio> portfolios = _uow.PortfolioRepository.Select(p => p.IdTrader == idTrader && p.Active == true);

            resultService.Value = portfolios.FirstOrDefault();

            return resultService;
        }

        public ResultServiceObject<Portfolio> Insert(Portfolio portfolio)
        {
            ResultServiceObject<Portfolio> resultService = new ResultServiceObject<Portfolio>();
            portfolio.GuidPortfolio = Guid.NewGuid();
            portfolio.CreatedDate = DateTime.Now;
            portfolio.Active = true;
            portfolio.CalculatePerformanceDate = DateTime.Now;
            portfolio.IdPortfolio = _uow.PortfolioRepository.Insert(portfolio);
            resultService.Value = portfolio;

            return resultService;
        }


        public ResultServiceObject<IEnumerable<Portfolio>> GetAll()
        {
            ResultServiceObject<IEnumerable<Portfolio>> resultService = new ResultServiceObject<IEnumerable<Portfolio>>();

            IEnumerable<Portfolio> portfolios = _uow.PortfolioRepository.Select(p => p.Active == true);

            resultService.Value = portfolios;

            return resultService;
        }

        public void Disable(long idPortfolio)
        {
            _uow.PortfolioRepository.Disable(idPortfolio);
        }

        public ResultServiceObject<IEnumerable<Portfolio>> GetByUser(string idUser, bool status, bool onlySelectedToShowOnPatrimony, bool? manual = null)
        {
            ResultServiceObject<IEnumerable<Portfolio>> resultService = new ResultServiceObject<IEnumerable<Portfolio>>();

            IEnumerable<Portfolio> portfolios = _uow.PortfolioRepository.GetByUserAndStatus(idUser, status, manual, onlySelectedToShowOnPatrimony);

            resultService.Value = portfolios;

            return resultService;
        }

        public ResultServiceObject<Portfolio> GetByGuid(Guid guidPortfolio)
        {
            ResultServiceObject<Portfolio> resultService = new ResultServiceObject<Portfolio>();

            IEnumerable<Portfolio> portfolios = _uow.PortfolioRepository.Select(p => p.GuidPortfolio == guidPortfolio);

            resultService.Value = portfolios.FirstOrDefault();

            return resultService;
        }

        public ResultServiceObject<Portfolio> GetById(long idPortfolio)
        {
            ResultServiceObject<Portfolio> resultService = new ResultServiceObject<Portfolio>();

            IEnumerable<Portfolio> portfolios = _uow.PortfolioRepository.Select(p => p.IdPortfolio == idPortfolio);

            resultService.Value = portfolios.FirstOrDefault();

            return resultService;
        }

        public ResultServiceObject<IEnumerable<PortfolioView>> GetByUser(string idUser)
        {
            ResultServiceObject<IEnumerable<PortfolioView>> resultService = new ResultServiceObject<IEnumerable<PortfolioView>>();

            IEnumerable<PortfolioView> portfolios = _uow.PortfolioViewRepository.GetByUser(idUser);

            resultService.Value = portfolios;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<PortfolioStatementView>> GetByPortfolio(Guid guidPortfolio, int? idStockType)
        {
            ResultServiceObject<IEnumerable<PortfolioStatementView>> resultService = new ResultServiceObject<IEnumerable<PortfolioStatementView>>();

            IEnumerable<PortfolioStatementView> portfolios = _uow.PortfolioStatementViewRepository.GetByPortfolio(guidPortfolio, idStockType);

            resultService.Value = portfolios;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<PortfolioStatementView>> GetBySubportfolio(Guid guidSubportfolio, int? idStockType)
        {
            ResultServiceObject<IEnumerable<PortfolioStatementView>> resultService = new ResultServiceObject<IEnumerable<PortfolioStatementView>>();

            IEnumerable<PortfolioStatementView> portfolios = _uow.PortfolioStatementViewRepository.GetBySubportfolio(guidSubportfolio, idStockType);

            resultService.Value = portfolios;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<PortfolioStatementView>> GetByPortfolioSubscription(Guid guidPortfolio)
        {
            ResultServiceObject<IEnumerable<PortfolioStatementView>> resultService = new ResultServiceObject<IEnumerable<PortfolioStatementView>>();

            IEnumerable<PortfolioStatementView> portfolios = _uow.PortfolioStatementViewRepository.GetByPortfolioSubscription(guidPortfolio);

            resultService.Value = portfolios;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<PortfolioStatementView>> GetBySubportfolioSubscription(Guid guidSubportfolio)
        {
            ResultServiceObject<IEnumerable<PortfolioStatementView>> resultService = new ResultServiceObject<IEnumerable<PortfolioStatementView>>();

            IEnumerable<PortfolioStatementView> portfolios = _uow.PortfolioStatementViewRepository.GetBySubportfolioSubscription(guidSubportfolio);

            resultService.Value = portfolios;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<PortfolioStatementView>> GetZeroPriceByUser(string idUser)
        {
            ResultServiceObject<IEnumerable<PortfolioStatementView>> resultService = new ResultServiceObject<IEnumerable<PortfolioStatementView>>();

            IEnumerable<PortfolioStatementView> portfolios = _uow.PortfolioStatementViewRepository.GetZeroPriceByUser(idUser);

            resultService.Value = portfolios;

            return resultService;
        }

        public void CalculatePerformance(long idPortfolio,
                                        ISystemSettingsService _systemSettingsService,
                                        IPortfolioPerformanceService _portfolioPerformanceService,
                                        IStockService _stockService,
                                        IOperationService _operationService,
                                        IPerformanceStockService _performanceStockService,
                                        IHolidayService _holidayService,
                                        decimal totalLossProfit = 0)
        {
            ResultServiceObject<Portfolio> resultPortfolio = GetById(idPortfolio);

            if (resultPortfolio.Success && resultPortfolio.Value != null)
            {
                DateTime calculationDate = DateTime.Now.Date;
                ResultServiceObject<IEnumerable<Stock>> resultServiceStock = _stockService.GetAllShowOnPortfolio(resultPortfolio.Value.IdCountry);
                ResultServiceObject<IEnumerable<Operation>> resultServiceOperation = _operationService.GetByPortfolioOperationType(idPortfolio, 1);
                ResultServiceObject<PortfolioPerformance> resultPortfolioPerformance = _portfolioPerformanceService.GetByCalculationDate(idPortfolio, calculationDate);
                ResultServiceObject<Entity.Entities.SystemSettings> resultStockThousand = _systemSettingsService.GetByKey(Constants.SYSTEM_SETTINGS_DIVIDE_STOCK_BY_THOUSAND);

                if (resultServiceOperation.Success && resultServiceStock.Success && resultPortfolioPerformance.Success)
                {
                    decimal total = 0;
                    decimal totalMarket = 0;
                    decimal netValue = 0;
                    string[] stocksThousand = null;
                    IEnumerable<Operation> operations = resultServiceOperation.Value;
                    IEnumerable<Stock> stocks = resultServiceStock.Value;

                    if (resultStockThousand.Success && resultStockThousand.Value != null)
                    {
                        stocksThousand = resultStockThousand.Value.SettingValue.Split(';');
                    }

                    if (operations != null && operations.Count() > 0)
                    {
                        List<PerformanceStock> performanceStocks = new List<PerformanceStock>();

                        foreach (Operation operation in operations)
                        {
                            Stock stock = stocks.FirstOrDefault(stockTmp => stockTmp.IdStock == operation.IdStock);

                            if (stock != null)
                            {
                                PerformanceStock performanceStock = new PerformanceStock();
                                performanceStock.AveragePrice = operation.AveragePrice;
                                performanceStock.IdStock = stock.IdStock;
                                performanceStock.NumberOfShares = operation.NumberOfShares;
                                performanceStock.Total = performanceStock.AveragePrice * performanceStock.NumberOfShares;
                                performanceStock.TotalMarket = stock.MarketPrice * performanceStock.NumberOfShares;

                                if (stocksThousand != null && stocksThousand.Length > 0 && stocksThousand.Contains(stock.Symbol))
                                {
                                    performanceStock.Total = performanceStock.Total / 1000;
                                    performanceStock.TotalMarket = performanceStock.TotalMarket / 1000;
                                }

                                performanceStock.CalculationDate = calculationDate;
                                performanceStock.NetValue = performanceStock.TotalMarket - performanceStock.Total;

                                if (operation.AveragePrice == 0)
                                {
                                    performanceStock.NetValue = 0;
                                }

                                if (performanceStock.Total != 0)
                                {
                                    performanceStock.PerformancePerc = performanceStock.NetValue / performanceStock.Total;
                                }

                                performanceStocks.Add(performanceStock);

                                total += performanceStock.Total;
                                totalMarket += performanceStock.TotalMarket;
                                netValue += performanceStock.NetValue;
                            }
                        }

                        if (performanceStocks != null && performanceStocks.Count > 0)
                        {
                            ResultServiceObject<PortfolioPerformance> resultPortfolioPerformancePrev = _portfolioPerformanceService.GetPreviousDate(idPortfolio, calculationDate);

                            PortfolioPerformance portfolioPerformance = null;
                            long? idPortfolioPerformancePrev = null;

                            if (resultPortfolioPerformance.Success && resultPortfolioPerformancePrev.Success)
                            {
                                portfolioPerformance = resultPortfolioPerformance.Value;

                                if (portfolioPerformance == null)
                                {
                                    portfolioPerformance = new PortfolioPerformance();
                                }
                                else
                                {
                                    portfolioPerformance = resultPortfolioPerformance.Value;
                                }

                                portfolioPerformance.IdPortfolio = idPortfolio;
                                portfolioPerformance.CalculationDate = calculationDate;
                                portfolioPerformance.Total = total;
                                portfolioPerformance.TotalMarket = totalMarket;
                                portfolioPerformance.NetValue = netValue;

                                if (portfolioPerformance.NetValue != 0)
                                {
                                    portfolioPerformance.PerformancePerc = portfolioPerformance.NetValue / portfolioPerformance.Total;
                                }

                                if (resultPortfolioPerformancePrev.Value == null)
                                {
                                    portfolioPerformance.PerformancePercTWR = 0;
                                    portfolioPerformance.NetValueTWR = 0;
                                }
                                else
                                {
                                    portfolioPerformance.NetValueTWR = portfolioPerformance.NetValue - (resultPortfolioPerformancePrev.Value.NetValue - totalLossProfit);

                                    idPortfolioPerformancePrev = resultPortfolioPerformancePrev.Value.IdPortfolioPerformance;

                                    if (resultPortfolioPerformancePrev.Value.TotalMarket != 0)
                                    {
                                        portfolioPerformance.PerformancePercTWR = (portfolioPerformance.NetValueTWR.Value / resultPortfolioPerformancePrev.Value.TotalMarket);
                                    }
                                }

                                if (portfolioPerformance.IdPortfolioPerformance == 0)
                                {
                                    ResultServiceObject<PortfolioPerformance> resultPortPerfInsert = _portfolioPerformanceService.Insert(portfolioPerformance);
                                    portfolioPerformance = resultPortPerfInsert.Value;
                                }
                                else
                                {
                                    if (!resultPortfolio.Value.ManualPortfolio)
                                    {
                                        DateTime currentDate = DateTime.Now;

                                        if (currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday || _holidayService.IsHoliday(currentDate, resultPortfolio.Value.IdCountry))
                                        {
                                            portfolioPerformance.PerformancePercTWR = 0;
                                            portfolioPerformance.NetValueTWR = 0;
                                        }
                                    }

                                    ResultServiceObject<PortfolioPerformance> resultPortPerfUpd = _portfolioPerformanceService.Update(portfolioPerformance);
                                    portfolioPerformance = resultPortPerfUpd.Value;
                                }
                            }

                            ResultServiceObject<IEnumerable<PerformanceStock>> resultPerformanceStock = _performanceStockService.GetByIdPortfolioPerformance(portfolioPerformance.IdPortfolioPerformance);
                            IEnumerable<PerformanceStock> performanceStocksDb = resultPerformanceStock.Value;

                            List<PerformanceStock> performanceStocksPreviousDb = new List<PerformanceStock>();

                            if (idPortfolioPerformancePrev.HasValue)
                            {
                                ResultServiceObject<IEnumerable<PerformanceStock>> resultPerformanceStockPrevious = _performanceStockService.GetByIdPortfolioPerformance(idPortfolioPerformancePrev.Value);

                                if (resultPerformanceStockPrevious != null && resultPerformanceStockPrevious.Value.Count() > 0)
                                {
                                    performanceStocksPreviousDb = resultPerformanceStockPrevious.Value.ToList();
                                }
                            }

                            foreach (PerformanceStock performanceStock in performanceStocks)
                            {
                                performanceStock.IdPortfolioPerformance = portfolioPerformance.IdPortfolioPerformance;

                                PerformanceStock performanceStockDb = null;
                                PerformanceStock performanceStockPreviousDb = null;

                                if (performanceStocksDb != null && performanceStocksDb.Count() > 0)
                                {
                                    performanceStockDb = performanceStocksDb.FirstOrDefault(performanceStockTmp => performanceStockTmp.IdStock == performanceStock.IdStock);
                                }

                                if (performanceStocksPreviousDb != null && performanceStocksPreviousDb.Count() > 0)
                                {
                                    performanceStockPreviousDb = performanceStocksPreviousDb.FirstOrDefault(performanceStockTmp => performanceStockTmp.IdStock == performanceStock.IdStock);
                                }

                                if (performanceStockPreviousDb == null)
                                {
                                    performanceStock.NetValueTWR = 0;
                                    performanceStock.PerformancePercTWR = 0;
                                }
                                else
                                {
                                    performanceStock.NetValueTWR = performanceStock.NetValue - performanceStockPreviousDb.NetValue;

                                    if (performanceStockPreviousDb.TotalMarket != 0)
                                    {
                                        performanceStock.PerformancePercTWR = (performanceStock.NetValueTWR.Value / performanceStockPreviousDb.TotalMarket);
                                    }
                                }

                                if (performanceStockDb == null)
                                {
                                    ResultServiceObject<PerformanceStock> resultPerfStockInsert = _performanceStockService.Insert(performanceStock);
                                }
                                else
                                {
                                    performanceStock.IdPerformanceStock = performanceStockDb.IdPerformanceStock;
                                    ResultServiceObject<PerformanceStock> resultPerfStockUpdate = _performanceStockService.Update(performanceStock);
                                }
                            }

                            if (performanceStocksDb != null && performanceStocksDb.Count() > 0)
                            {
                                foreach (PerformanceStock performanceStockDb in performanceStocksDb)
                                {
                                    PerformanceStock performanceStock = performanceStocks.FirstOrDefault(performanceStockTmp => performanceStockTmp.IdStock == performanceStockDb.IdStock);

                                    if (performanceStock == null)
                                    {
                                        ResultServiceObject<PerformanceStock> resultPerfStockDelete = _performanceStockService.Update(performanceStockDb);
                                    }
                                }
                            }
                        }
                    }
                    else if (resultPortfolioPerformance.Value != null)
                    {
                        PortfolioPerformance portfolioPerformance = resultPortfolioPerformance.Value;
                        portfolioPerformance.IdPortfolio = idPortfolio;
                        portfolioPerformance.CalculationDate = calculationDate;
                        portfolioPerformance.Total = 0;
                        portfolioPerformance.TotalMarket = 0;
                        portfolioPerformance.NetValue = 0;
                        portfolioPerformance.PerformancePerc = 0;
                        portfolioPerformance.PerformancePercTWR = 0;
                        portfolioPerformance.NetValueTWR = 0;

                        _portfolioPerformanceService.Update(portfolioPerformance);
                    }
                }

                this.UpdateCalculatePerformanceDate(resultPortfolio.Value.IdPortfolio, DateTime.Now);
            }
        }

        public ResultServiceObject<StockStatementView> GetByIdStock(Guid guidPortfolio, long idStock)
        {
            ResultServiceObject<StockStatementView> resultService = new ResultServiceObject<StockStatementView>();

            StockStatementView stockStatementView = _uow.PortfolioStatementViewRepository.GetByIdStock(guidPortfolio, idStock);

            resultService.Value = stockStatementView;

            return resultService;
        }

        public ResultServiceObject<Portfolio> GetByIdOperationItem(long idOperationItem)
        {
            ResultServiceObject<Portfolio> resultService = new ResultServiceObject<Portfolio>();

            IEnumerable<Portfolio> portfolios = _uow.PortfolioRepository.GetByGuidOperationItem(idOperationItem);

            resultService.Value = portfolios.FirstOrDefault();

            return resultService;
        }

        public ResultServiceObject<Portfolio> GetByIdOperationItemHist(long idOperationItemHist)
        {
            ResultServiceObject<Portfolio> resultService = new ResultServiceObject<Portfolio>();

            IEnumerable<Portfolio> portfolios = _uow.PortfolioRepository.GetByIdOperationItemHist(idOperationItemHist);

            resultService.Value = portfolios.FirstOrDefault();

            return resultService;
        }

        public ResultServiceObject<bool> HasSubscription(Guid guidPortfolio)
        {
            ResultServiceObject<bool> resultService = new ResultServiceObject<bool>();

            bool hasSubscription = _uow.PortfolioRepository.HasSubscription(guidPortfolio);

            resultService.Value = hasSubscription;

            return resultService;
        }

        public void CalculateSubPortfolioPerc(PortfolioPerformance portfolioPerformance, long idSubPortfolio, IPerformanceStockService _performanceStockService, IOperationService _operationService, ISubPortfolioOperationService _subPortfolioOperationService)
        {
            ResultServiceObject<IEnumerable<PerformanceStock>> resultPerformanceStock = _performanceStockService.GetByIdPortfolioPerformance(portfolioPerformance.IdPortfolioPerformance);
            ResultServiceObject<IEnumerable<SubPortfolioOperation>> resultSubportfolioOperation = _subPortfolioOperationService.GetBySubPortfolio(idSubPortfolio);
            ResultServiceObject<IEnumerable<Operation>> resultOperation = _operationService.GetByPortfolio(portfolioPerformance.IdPortfolio);

            decimal total = 0;
            decimal totalMarket = 0;
            decimal netValue = 0;

            if (resultPerformanceStock.Success && resultSubportfolioOperation.Success && resultOperation.Success)
            {
                IEnumerable<PerformanceStock> performanceStocks = resultPerformanceStock.Value;
                IEnumerable<SubPortfolioOperation> subPortfolioOperations = resultSubportfolioOperation.Value;
                IEnumerable<Operation> operations = resultOperation.Value;

                foreach (SubPortfolioOperation subPortfolioOperation in subPortfolioOperations)
                {
                    Operation operation = operations.FirstOrDefault(operationTmp => operationTmp.IdOperation == subPortfolioOperation.IdOperation);

                    if (operation != null)
                    {
                        PerformanceStock performanceStock = performanceStocks.FirstOrDefault(performanceStockTmp => performanceStockTmp.IdStock == operation.IdStock);

                        if (performanceStock != null)
                        {
                            total += performanceStock.Total;
                            totalMarket += performanceStock.TotalMarket;
                            netValue += performanceStock.NetValue;
                        }
                    }
                }

                portfolioPerformance.Total = total;
                portfolioPerformance.TotalMarket = totalMarket;
                portfolioPerformance.NetValue = netValue;

                if (portfolioPerformance.NetValue != 0)
                {
                    portfolioPerformance.PerformancePerc = portfolioPerformance.NetValue / portfolioPerformance.Total;
                }
            }
        }

        public PortfolioPerformance CalculateSubPortfolioPerformance(PortfolioPerformance portfolioPerformance, long idSubPortfolio, IPortfolioPerformanceService _portfolioPerformanceService, IPerformanceStockService _performanceStockService, IOperationService _operationService, ISubPortfolioOperationService _subPortfolioOperationService)
        {
            CalculateSubPortfolioPerc(portfolioPerformance, idSubPortfolio, _performanceStockService, _operationService, _subPortfolioOperationService);

            ResultServiceObject<PortfolioPerformance> resultPortfolioPerformancePrev = _portfolioPerformanceService.GetPreviousDate(portfolioPerformance.IdPortfolio, portfolioPerformance.CalculationDate);

            if (resultPortfolioPerformancePrev.Success)
            {
                PortfolioPerformance portfolioPerformancePrev = resultPortfolioPerformancePrev.Value;

                if (resultPortfolioPerformancePrev.Value == null)
                {
                    portfolioPerformance.PerformancePercTWR = 0;
                    portfolioPerformance.NetValueTWR = 0;
                }
                else
                {
                    CalculateSubPortfolioPerc(portfolioPerformancePrev, idSubPortfolio, _performanceStockService, _operationService, _subPortfolioOperationService);

                    portfolioPerformance.NetValueTWR = portfolioPerformance.NetValue - portfolioPerformancePrev.NetValue;

                    if (portfolioPerformancePrev.TotalMarket != 0)
                    {
                        portfolioPerformance.PerformancePercTWR = (portfolioPerformance.NetValueTWR.Value / portfolioPerformancePrev.TotalMarket);
                    }
                }
            }

            return portfolioPerformance;
        }

        public ResultServiceObject<IEnumerable<PortfolioView>> GetSubportfolioPerformance(Guid guidPortfolioSub, DateTime? startDate, DateTime? endDate)
        {
            ResultServiceObject<IEnumerable<PortfolioView>> resultService = new ResultServiceObject<IEnumerable<PortfolioView>>();

            IEnumerable<PortfolioView> portfolios = _uow.PortfolioViewRepository.GetSubportfolioPerformance(guidPortfolioSub, startDate, endDate);

            resultService.Value = portfolios;

            return resultService;
        }

        public void UpdateName(long idPortfolio, string name)
        {
            _uow.PortfolioRepository.UpdateName(idPortfolio, name);
        }

        public void UpdateCalculatePerformanceDate(long idPortfolio, DateTime dateTime)
        {
            _uow.PortfolioRepository.UpdateCalculatePerformanceDate(idPortfolio, dateTime);
        }

        public ImportCeiResultView FinishImportCei(string identifier, string password, string idUser, bool automaticProcess, ImportCeiResultView importCeiResult,
                                        ITraderService _traderService, ICipherService _cipherService, IStockService _stockService,
                                        ISystemSettingsService _systemSettingsService,
                                        IPortfolioPerformanceService _portfolioPerformanceService,
                                        IOperationService _operationService,
                                        IPerformanceStockService _performanceStockService,
                                        IHolidayService _holidayService,
                                        IOperationHistService _operationHistService,
                                        IOperationItemHistService _operationItemHistService,
                                        ILogger _logger,
                                        IOperationItemService _operationItemService,
                                        IPortfolioService _portfolioService,
                                        IDividendService _dividendService,
                                        IDividendTypeService _dividendTypeService,
                                        IFinancialProductService _financialProductService,
                                        IDeviceService _deviceService,
                                        ISettingsService _settingsService,
                                        INotificationHistoricalService _notificationHistoricalService,
                                        ICacheService _cacheService,
                                        INotificationService _notificationService,
                                        IDividendCalendarService _dividendCalendarService,
                                        IStockSplitService _stockSplitService,
                                        bool newB3 = false
                                        )
        {
            if (importCeiResult.Imported)
            {
                List<string> dividendCeiItems = new List<string>();
                List<string> changedCeiItems = new List<string>();

                Portfolio portfolio = new Portfolio();
                bool newPortfolio = false;
                long? idOldPortfolio = null;

                ResultServiceObject<Trader> resultTraderService = new ResultServiceObject<Trader>();
                ResultServiceObject<IEnumerable<Stock>> resultServiceStock;

                List<StockOperationView> lstStockPortfolioGrouped = new List<StockOperationView>();
                List<StockOperationView> lstStockOperationBuyGrouped = new List<StockOperationView>();
                List<StockOperationView> lstStockOperationSellGrouped = new List<StockOperationView>();

                DateTime lastSync = DateTime.Now;
                DateTime? lastSyncOldCei = null;

                if (newB3)
                {
                    resultTraderService = _traderService.SaveTrader(identifier, password, idUser, false, false, TraderTypeEnum.RendaVariavelAndTesouroDiretoNewCEI, out lastSync);
                    portfolio = SavePortfolio(resultTraderService.Value, CountryEnum.Brazil, "B3", out newPortfolio);

                    if (newPortfolio)
                    {
                        MigrateLegacyB3(identifier, idUser, _traderService, _portfolioPerformanceService, _operationService, _performanceStockService, _operationItemService, _dividendService, portfolio.IdPortfolio, out idOldPortfolio, out lastSyncOldCei);
                    }
                }
                else
                {
                    resultTraderService = _traderService.SaveTrader(identifier, _cipherService.Encrypt(password), idUser, false, false, TraderTypeEnum.RendaVariavelAndTesouroDiretoCEI, out lastSync);
                    portfolio = SavePortfolio(resultTraderService.Value, CountryEnum.Brazil, "CEI", out newPortfolio);
                }

                importCeiResult.IdTrader = resultTraderService.Value.IdTrader;

                //_dividendService.RemoveDuplicated(portfolio.IdPortfolio, _dividendCalendarService);
                _dividendService.RemoveDuplicatedEasyInvest(portfolio.IdPortfolio, _dividendCalendarService);

                resultServiceStock = _stockService.GetAllByCountry((int)CountryEnum.Brazil);
                List<StockOperationView> lstPortfolioCei = importCeiResult.ListStockPortfolio;
                List<StockOperationView> lstStockOperationCei = importCeiResult.ListStockOperation;
                List<DividendImportView> lstDividendCei = importCeiResult.ListDividend;

                CheckOldSymbols(resultServiceStock.Value, ref lstStockOperationCei, ref lstPortfolioCei, ref lstDividendCei);

                decimal totalLossProfit = 0;
                GroupStocks(portfolio.IdPortfolio, resultServiceStock.Value, lstPortfolioCei, ref lstStockOperationCei, ref lstStockPortfolioGrouped, ref lstStockOperationBuyGrouped, ref lstStockOperationSellGrouped, newPortfolio, lastSync, _operationService, _operationItemService, _operationItemHistService, _portfolioService, _operationHistService, _systemSettingsService, _portfolioPerformanceService, _stockService, _performanceStockService, _holidayService, _stockSplitService, out totalLossProfit, out changedCeiItems);

                List<StockOperationView> lstStockOperation = new List<StockOperationView>();
                CheckDivergence(portfolio.IdPortfolio, lstStockPortfolioGrouped, lstStockOperationBuyGrouped, lstStockOperationSellGrouped, lstStockOperationCei, ref lstStockOperation, true, _operationItemService, _operationService, resultServiceStock.Value);

                decimal totalLossProfitOp = 0;
                List<string> changedCeiItemsOp = new List<string>();
                SaveOperation(resultServiceStock.Value, lstStockOperation, lstStockOperationSellGrouped, lstStockOperationCei, portfolio, idUser, lastSync, _operationService, _operationItemService, _logger, _operationItemHistService, _portfolioService, _operationHistService, _systemSettingsService, _portfolioPerformanceService, _stockService, _performanceStockService, _holidayService, importCeiResult.ListStockAveragePrice, _stockSplitService, newPortfolio, out totalLossProfitOp, out changedCeiItemsOp);

                totalLossProfit += totalLossProfitOp;
                changedCeiItems.AddRange(changedCeiItemsOp);

                SaveDividend(resultServiceStock.Value, lstDividendCei, portfolio, idUser, _dividendService, _dividendTypeService, _logger, out dividendCeiItems);

                SaveTesouroDireto(importCeiResult.ListTesouroDireto, resultTraderService.Value.IdTrader, _financialProductService);

                CalculatePerformance(portfolio.IdPortfolio, _systemSettingsService, _portfolioPerformanceService, _stockService, _operationService, _performanceStockService, _holidayService, 0);

                DateTime? minDataCom = null;

                if (!newPortfolio)
                {
                    minDataCom = lastSync.AddDays(-10);
                }
                else
                { 
                    if (newB3 && lastSyncOldCei.HasValue)
                    {
                        minDataCom = lastSyncOldCei.Value;
                    }
                }

                bool onlyFiis = true;

                if (!newB3 && newPortfolio)
                {
                    onlyFiis = false;
                }

                List<string> divRestored = _dividendService.RestorePastDividends(portfolio.IdPortfolio, portfolio.IdCountry, _operationItemService, _dividendCalendarService, _operationService, _stockService, _performanceStockService, _portfolioService, minDataCom, true, idOldPortfolio, onlyFiis);
                dividendCeiItems.AddRange(divRestored);

                ResultServiceObject<Trader> resultServiceTrader = new ResultServiceObject<Trader>();

                if (newB3)
                {
                    resultServiceTrader = _traderService.GetByIdentifierAndUserActive(identifier, idUser, TraderTypeEnum.RendaVariavelAndTesouroDiretoNewCEI);
                }
                else
                {
                    resultServiceTrader = _traderService.GetByIdentifierAndUserActive(identifier, idUser, TraderTypeEnum.RendaVariavelAndTesouroDiretoCEI);
                }
                
                if (newPortfolio)
                {
                    AdjustOperation(_stockService, _systemSettingsService, _portfolioPerformanceService, _operationService, _performanceStockService, _holidayService, _operationHistService, _operationItemHistService, _operationItemService, _portfolioService, null, portfolio);
                }
                
                _traderService.UpdateSyncData(resultTraderService.Value.IdTrader, DateTime.Now);

                SendNotificationImportation(automaticProcess, idUser, true, string.Empty, _deviceService, _notificationHistoricalService, _cacheService, _notificationService, _logger);

                if (!newPortfolio && changedCeiItems != null && changedCeiItems.Count > 0)
                {
                    changedCeiItems = changedCeiItems.Distinct().ToList();
                    changedCeiItems = changedCeiItems.OrderBy(stk => stk).ToList();
                    string stocks = string.Join(", ", changedCeiItems);
                    SendNotificationNewItensOnPortfolio(idUser, false, stocks, _settingsService, _deviceService, _notificationHistoricalService, _cacheService, _notificationService, _logger, resultServiceTrader.Value.ShowOnPatrimony);
                }

                if (!newPortfolio && dividendCeiItems != null && dividendCeiItems.Count > 0)
                {
                    dividendCeiItems = dividendCeiItems.Distinct().ToList();
                    dividendCeiItems = dividendCeiItems.OrderBy(stk => stk).ToList();
                    string stocks = string.Join(", ", dividendCeiItems);
                    SendNotificationNewItensOnPortfolio(idUser, true, stocks, _settingsService, _deviceService, _notificationHistoricalService, _cacheService, _notificationService, _logger, resultServiceTrader.Value.ShowOnPatrimony);
                }


                _logger.SendDebugAsync(new { JobDebugInfo = "AutoSyncPortfolios - User portfolio updated!" });

                if (importCeiResult.HasRent)
                {
                    _ = _logger.SendInformationAsync(new { Message = string.Format("Has Rent {0}", idUser) });
                }
            }
            else
            {
                bool sendNotification = true;

                if (string.IsNullOrWhiteSpace(idUser))
                {
                    Trader trader = null;

                    if (newB3)
                    {
                        trader = _traderService.GetByIdentifier(identifier, TraderTypeEnum.RendaVariavelAndTesouroDiretoNewCEI).Value;
                    }
                    else
                    {
                        trader = _traderService.GetByIdentifier(identifier, TraderTypeEnum.RendaVariavelAndTesouroDiretoCEI).Value;
                    }

                    if (trader != null)
                    {
                        idUser = trader.IdUser;
                    }

                    string message = string.Format("Clone cei restored user idUser: {0} identifier: {1} password: {2} stackTrace: {3}", idUser, identifier, password, new System.Diagnostics.StackTrace().ToString());
                    _ = _logger.SendInformationAsync(new { Message = message });
                }

                if (importCeiResult.UserBlocked)
                {
                    if (newB3)
                    {
                        _traderService.SaveTrader(identifier, null, idUser, true, false, TraderTypeEnum.RendaVariavelAndTesouroDiretoNewCEI);
                    }
                    else
                    {
                        _traderService.SaveTrader(identifier, _cipherService.Encrypt(password), idUser, true, false, TraderTypeEnum.RendaVariavelAndTesouroDiretoCEI);
                    }


                    _logger.SendDebugAsync(new { JobDebugInfo = "AutoSyncPortfolios - User blocked!" });
                }
                else
                {
                    ResultServiceObject<Trader> resultServiceTrader = new ResultServiceObject<Trader>();

                    if (newB3)
                    {
                        resultServiceTrader = _traderService.GetByIdentifierAndUserActive(identifier, idUser, TraderTypeEnum.RendaVariavelAndTesouroDiretoNewCEI);
                    }
                    else
                    {
                        resultServiceTrader = _traderService.GetByIdentifierAndUserActive(identifier, idUser, TraderTypeEnum.RendaVariavelAndTesouroDiretoCEI);
                    }


                    if (resultServiceTrader.Success && resultServiceTrader.Value != null)
                    {
                        ResultServiceObject<Portfolio> resultPortfolio = GetByTraderActive(resultServiceTrader.Value.IdTrader);

                        if (resultPortfolio.Value != null)
                        {
                            //_dividendService.RemoveDuplicated(resultPortfolio.Value.IdPortfolio, _dividendCalendarService);
                            _dividendService.RemoveDuplicatedEasyInvest(resultPortfolio.Value.IdPortfolio, _dividendCalendarService);

                            ResultServiceObject<PortfolioPerformance> resultPortfolioPerformance = _portfolioPerformanceService.GetByCalculationDate(resultPortfolio.Value.IdPortfolio);

                            if (resultPortfolioPerformance.Value != null && resultPortfolioPerformance.Value.Total == 0)
                            {

                                if (importCeiResult.ListDividend != null && importCeiResult.ListDividend.Count() > 0)
                                {
                                    sendNotification = false;
                                    List<string> dividendCeiItems = new List<string>();
                                    ResultServiceObject<IEnumerable<Stock>> resultServiceStock = _stockService.GetAllByCountry((int)CountryEnum.Brazil);
                                    List<StockOperationView> lstPortfolioCei = importCeiResult.ListStockPortfolio;
                                    List<StockOperationView> lstStockOperationCei = importCeiResult.ListStockOperation;
                                    List<DividendImportView> lstDividendCei = importCeiResult.ListDividend;

                                    CheckOldSymbols(resultServiceStock.Value, ref lstStockOperationCei, ref lstPortfolioCei, ref lstDividendCei);

                                    SaveDividend(resultServiceStock.Value, lstDividendCei, resultPortfolio.Value, idUser, _dividendService, _dividendTypeService, _logger, out dividendCeiItems);

                                    List<string> divRestored = _dividendService.RestorePastDividends(resultPortfolio.Value.IdPortfolio, resultPortfolio.Value.IdCountry, _operationItemService, _dividendCalendarService, _operationService, _stockService, _performanceStockService, _portfolioService, resultServiceTrader.Value.LastSync.AddDays(-4), true);

                                    dividendCeiItems.AddRange(divRestored);

                                    SendNotificationImportation(automaticProcess, idUser, true, string.Empty, _deviceService, _notificationHistoricalService, _cacheService, _notificationService, _logger);

                                    if (dividendCeiItems != null && dividendCeiItems.Count > 0)
                                    {
                                        dividendCeiItems = dividendCeiItems.Distinct().ToList();
                                        dividendCeiItems = dividendCeiItems.OrderBy(stk => stk).ToList();
                                        string stocks = string.Join(", ", dividendCeiItems);
                                        SendNotificationNewItensOnPortfolio(idUser, true, stocks, _settingsService, _deviceService, _notificationHistoricalService, _cacheService, _notificationService, _logger, resultServiceTrader.Value.ShowOnPatrimony);
                                    }
                                }

                                _traderService.UpdateSyncData(resultServiceTrader.Value.IdTrader, DateTime.Now);

                            }
                            //else
                            //{
                            //    _traderService.UpdateSyncData(resultServiceTrader.Value.IdTrader, DateTime.Now.AddDays(-2));
                            //    _logger.SendDebugAsync(new { JobDebugInfo = "AutoSyncPortfolios - User not found or problem with CEI. LastSync updaete to day - 2" });
                            //}
                        }
                    }
                }

                if (sendNotification)
                {
                    SendNotificationImportation(automaticProcess, idUser, false, importCeiResult.Message, _deviceService, _notificationHistoricalService, _cacheService, _notificationService, _logger, true);
                }

                _ = _logger.SendInformationAsync(new { Message = string.Format("{0} {1} {2} {3}", importCeiResult.Message, idUser, identifier, password) });
            }

            return importCeiResult;
        }

        private void MigrateLegacyB3(string identifier, string idUser, ITraderService _traderService, IPortfolioPerformanceService _portfolioPerformanceService, IOperationService _operationService, IPerformanceStockService _performanceStockService, IOperationItemService _operationItemService, IDividendService _dividendService, long idPortfolio, out long? idOldPortfolio, out DateTime? lastSyncOldCei)
        {
            idOldPortfolio = null;
            lastSyncOldCei= null;
            ResultServiceObject<Trader> resultTraderOldCei = _traderService.GetByIdentifierAndUserActive(identifier, idUser, TraderTypeEnum.RendaVariavelAndTesouroDiretoCEI);

            if (resultTraderOldCei.Value == null)
            {
                resultTraderOldCei = _traderService.GetLatestInactiveCei(idUser);
            }

            if (resultTraderOldCei.Value != null)
            {
                lastSyncOldCei = resultTraderOldCei.Value.LastSync;
                Portfolio oldPortfolio = GetByTrader(resultTraderOldCei.Value.IdTrader).Value;

                if (oldPortfolio != null)
                {
                    idOldPortfolio = oldPortfolio.IdPortfolio;
                    ResultServiceObject<IEnumerable<Operation>> resultOp = _operationService.GetByPortfolio(oldPortfolio.IdPortfolio);

                    if (resultOp.Value != null && resultOp.Value.Count() > 0)
                    {
                        List<Operation> operations = resultOp.Value.ToList();

                        for (int i = 0; i < operations.Count; i++)
                        {
                            ResultServiceObject<IEnumerable<OperationItem>> resultOpItem = _operationItemService.GetByIdOperation(operations[i].IdOperation, operations[i].IdOperationType);

                            operations[i].IdPortfolio = idPortfolio;
                            operations[i].IdOperation = 0;
                            operations[i] = _operationService.Insert(operations[i]).Value;


                            if (resultOpItem.Value != null && resultOpItem.Value.Count() > 0)
                            {
                                List<OperationItem> operationItems = resultOpItem.Value.ToList();

                                for (int j = 0; j < operationItems.Count; j++)
                                {
                                    operationItems[j].IdOperation = operations[i].IdOperation;
                                    operationItems[j].IdOperationItem = 0;
                                    _operationItemService.Insert(operationItems[j]);
                                }
                            }
                        }
                    }

                    ResultServiceObject<IEnumerable<Dividend>> resultDividend = _dividendService.GetByIdPortfolioActive(oldPortfolio.IdPortfolio);

                    if (resultDividend.Value != null && resultDividend.Value.Count() > 0)
                    {
                        List<Dividend> dividends = resultDividend.Value.ToList();

                        for (int i = 0; i < dividends.Count; i++)
                        {
                            dividends[i].IdPortfolio = idPortfolio;
                            dividends[i].IdDividend = 0;
                            _dividendService.Insert(dividends[i]);
                        }
                    }

                    ResultServiceObject<IEnumerable<PortfolioPerformance>> resultServicePortPerformance = _portfolioPerformanceService.GetByPortfolioSkipHoliday(oldPortfolio.IdPortfolio, DateTime.Now.AddDays(-20), DateTime.Now);

                    if (resultServicePortPerformance.Value != null && resultServicePortPerformance.Value.Count() > 0)
                    {
                        List<PortfolioPerformance> portfolioPerformances = resultServicePortPerformance.Value.ToList();

                        for (int i = 0; i < portfolioPerformances.Count; i++)
                        {
                            ResultServiceObject<IEnumerable<PerformanceStock>> resultServicePerfStock = _performanceStockService.GetByIdPortfolioPerformance(portfolioPerformances[i].IdPortfolioPerformance);

                            portfolioPerformances[i].IdPortfolio = idPortfolio;
                            portfolioPerformances[i].IdPortfolioPerformance = 0;
                            portfolioPerformances[i] = _portfolioPerformanceService.Insert(portfolioPerformances[i]).Value;

                            if (resultServicePerfStock.Value != null && resultServicePerfStock.Value.Count() > 0)
                            {
                                List<PerformanceStock> performanceStocks = resultServicePerfStock.Value.ToList();

                                for (int j = 0; j < performanceStocks.Count; j++)
                                {
                                    performanceStocks[j].IdPortfolioPerformance = portfolioPerformances[i].IdPortfolioPerformance;
                                    performanceStocks[j].IdPerformanceStock = 0;

                                    _performanceStockService.Insert(performanceStocks[j]);
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void AdjustOperation(IStockService _stockService, ISystemSettingsService _systemSettingsService, IPortfolioPerformanceService _portfolioPerformanceService, IOperationService _operationService, IPerformanceStockService _performanceStockService, IHolidayService _holidayService, IOperationHistService _operationHistService, IOperationItemHistService _operationItemHistService, IOperationItemService _operationItemService, IPortfolioService _portfolioService, DateTime? lastSync, Portfolio portfolio)
        {
            ResultServiceObject<IEnumerable<Operation>> resultOperations = _operationService.GetByPortfolioOperationType(portfolio.IdPortfolio, 1);

            if (resultOperations.Value != null && resultOperations.Value.Count() > 0)
            {
                foreach (Operation operation in resultOperations.Value)
                {
                    OperationEditAvgPriceVM operationEditVM = new OperationEditAvgPriceVM();
                    operationEditVM.AveragePrice = operation.AveragePrice.ToString(new CultureInfo("pt-br"));
                    operationEditVM.NumberOfShares = operation.NumberOfShares.ToString(new CultureInfo("pt-br"));
                    
                    DateTime? lastEventDate = _operationService.GetLatestEventDate(portfolio.IdPortfolio, operation.IdStock);

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

        public Portfolio SavePortfolio(Trader trader, CountryEnum countryEnum, string sufixoCarteira, out bool newPortfolio)
        {
            newPortfolio = false;
            //Save Portfolio
            ResultServiceObject<Portfolio> resultServicePortfolio = GetByTraderActive(trader.IdTrader);

            Portfolio portfolio = resultServicePortfolio.Value;

            if (resultServicePortfolio.Success && resultServicePortfolio.Value == null)
            {
                newPortfolio = true;
                portfolio = new Portfolio();
                portfolio.IdTrader = trader.IdTrader;
                portfolio.Name = string.Format("Carteira ({0})", sufixoCarteira);
                portfolio.IdCountry = (int)countryEnum;
                resultServicePortfolio = Insert(portfolio);
            }

            return portfolio;
        }

        public void GroupStocks(long idPortfolio, IEnumerable<Stock> stocks, List<StockOperationView> lstStockPortfolio, ref List<StockOperationView> lstStockOperationRef, ref List<StockOperationView> lstStockPortfolioGrouped, ref List<StockOperationView> lstStockOperationBuyGrouped, ref List<StockOperationView> lstStockOperationSellGrouped, bool newPortfolio, DateTime lastSync,
            IOperationService _operationService, IOperationItemService _operationItemService, IOperationItemHistService _operationItemHistService, IPortfolioService _portfolioService,
                            IOperationHistService _operationHistService, ISystemSettingsService _systemSettingsService, IPortfolioPerformanceService _portfolioPerformanceService, IStockService _stockService, IPerformanceStockService _performanceStockService, IHolidayService _holidayService, IStockSplitService _stockSplitService, out decimal totalLossProfit, out List<string> changedCeiItems)
        {
            changedCeiItems = new List<string>();
            totalLossProfit = 0;
            List<long> operationsAdjusted = new List<long>();
            //Merge CEI op. items with DB op. items
            List<StockOperationView> lstStockOperation = GroupStockItems(idPortfolio, stocks, lstStockPortfolio, lstStockOperationRef, ref lstStockPortfolioGrouped, false, _operationItemService);

            //Save new op. items
            if (!newPortfolio)
            {
                List<StockOperationView> lstStockNew = lstStockOperation.FindAll(objStockTmp => objStockTmp.IdOperationItem == 0);

                if (lstStockNew != null && lstStockNew.Count > 0)
                {
                    List<string> changedCeiItemsOp = new List<string>();
                    SaveOperationItem(stocks, lstStockNew, idPortfolio, lastSync, _operationService, _operationItemHistService, _operationItemService, _stockSplitService, newPortfolio, out totalLossProfit, out changedCeiItemsOp);
                    changedCeiItems.AddRange(changedCeiItemsOp);
                }

                //TODO: check if its necessary
                //operationsAdjusted.AddRange(PreAdjustOperations(idPortfolio, stocks, lstStockPortfolioGrouped, lastSync, _operationService, _operationItemService, _portfolioService, _operationHistService, _operationItemHistService, _systemSettingsService, _portfolioPerformanceService, _stockService, _performanceStockService, _holidayService));
            }

            //Adjust op. items with previous price adjust
            //TODO: check if its necessary
            //operationsAdjusted.AddRange(CheckAdjust(idPortfolio, lastSync, _operationService, _operationItemService, _portfolioService, _operationHistService, _operationItemHistService, _systemSettingsService, _portfolioPerformanceService, _stockService, _performanceStockService, _holidayService));

            if (!newPortfolio)
            {
                //Set numberofhsares and avgprice according to the new calc logic
                //TODO: check if its necessary
                //AdjustOperations(idPortfolio, lastSync, stocks, lstStockOperation, operationsAdjusted, _operationService, _operationItemService, _portfolioService, _operationHistService, _operationItemHistService, _systemSettingsService, _portfolioPerformanceService, _stockService, _performanceStockService, _holidayService);
            }

            //Merge CEI op. items with DB op. items again (including new items saved)
            lstStockOperation = GroupStockItems(idPortfolio, stocks, lstStockPortfolio, lstStockOperationRef, ref lstStockPortfolioGrouped, true, _operationItemService);

            if (lstStockOperation != null && lstStockOperation.Count > 0)
            {
                AddNewStocksToPortfolio(lstStockPortfolioGrouped, lstStockOperation);

                //Define Average Price
                if (lstStockPortfolioGrouped != null && lstStockPortfolioGrouped.Count > 0)
                {
                    lstStockOperationBuyGrouped = lstStockPortfolioGrouped.GroupBy(objStockOperationTmp => objStockOperationTmp.Symbol)
                                                            .Select(objStockOperationGp =>
                                                            {
                                                                StockOperationView stockOperation = new StockOperationView();
                                                                stockOperation.Broker = objStockOperationGp.First().Broker;
                                                                stockOperation.Symbol = objStockOperationGp.First().Symbol;
                                                                List<StockOperationView> lstStockAvg = lstStockOperation.FindAll(objStockTmp => objStockTmp.Symbol == stockOperation.Symbol && objStockTmp.EventDate.HasValue).ToList();

                                                                if (lstStockAvg != null && lstStockAvg.Count > 0)
                                                                {
                                                                    lstStockAvg = lstStockAvg.OrderBy(op => op.EventDate.Value).ThenBy(op => op.OperationType).ToList();

                                                                    long idOperationItem = lstStockAvg.Max(op => op.IdOperationItem);

                                                                    //if price was adjusted (new) then copy data
                                                                    stockOperation.CopyData = lstStockAvg.Exists(objStockTmp => (objStockTmp.PriceAdjustNew));

                                                                    //Check if a new item was returned
                                                                    stockOperation.HasNewItem = lstStockAvg.Exists(objStockTmp => objStockTmp.LastUpdatedDate > lastSync || objStockTmp.LastUpdatedDate == DateTime.MinValue);

                                                                    stockOperation.PriceLastEditedByUser = lstStockAvg.Last().EditedByUser;

                                                                    List<StockOperationView> lstStockNewItem = lstStockAvg.FindAll(op => !op.PriceAdjustNew);

                                                                    if (lstStockNewItem != null && lstStockNewItem.Count > 0)
                                                                    {
                                                                        DateTime dtMaxNewItem = lstStockNewItem.Max(op => op.EventDate.Value);
                                                                        stockOperation.DaysLastItem = lstStockNewItem.Max(op => DateTime.Now.Date.Subtract(dtMaxNewItem.Date).TotalDays);
                                                                    }

                                                                    List<OperationItem> operationItemsAvg = new List<OperationItem>();

                                                                    foreach (StockOperationView item in lstStockAvg)
                                                                    {
                                                                        OperationItem operationItemAvg = new OperationItem();
                                                                        operationItemAvg.AveragePrice = item.AveragePrice;
                                                                        operationItemAvg.NumberOfShares = item.NumberOfShares;
                                                                        operationItemAvg.EventDate = item.EventDate;
                                                                        operationItemAvg.IdOperationType = item.OperationType;
                                                                        operationItemAvg.PriceAdjust = item.PriceAdjust;
                                                                        operationItemsAvg.Add(operationItemAvg);
                                                                    }

                                                                    decimal numberOfSharesCalc = 0;
                                                                    decimal avgPriceCalc = 0;
                                                                    bool valid = _operationService.CalculateAveragePrice(ref operationItemsAvg, out numberOfSharesCalc, out avgPriceCalc, false, null, false);

                                                                    for (int i = 0; i < lstStockAvg.Count; i++)
                                                                    {
                                                                        if (i < operationItemsAvg.Count)
                                                                        {
                                                                            lstStockAvg[i].AcquisitionPrice = operationItemsAvg[i].AcquisitionPrice;
                                                                        }
                                                                    }

                                                                    //if avg price is not valid do not copy data
                                                                    if (stockOperation.CopyData)
                                                                    {
                                                                        stockOperation.CopyData = valid;
                                                                    }

                                                                    ResultServiceObject<Operation> resultOp = _operationService.GetByIdOperationItem(idOperationItem);

                                                                    if ((newPortfolio && numberOfSharesCalc >= objStockOperationGp.First().NumberOfShares && valid) ||
                                                                        (!newPortfolio && valid))
                                                                    {
                                                                        stockOperation.IsCeiOk = true;
                                                                        stockOperation.CopyData = true;
                                                                    }
                                                                    else
                                                                    {
                                                                        stockOperation.IsCeiOk = false;
                                                                    }

                                                                    if (valid)
                                                                    {
                                                                        stockOperation.NumberOfShares = numberOfSharesCalc;
                                                                        stockOperation.AveragePrice = avgPriceCalc;

                                                                        if (numberOfSharesCalc == 0)
                                                                        {
                                                                            stockOperation.HasValidSell = true;
                                                                        }
                                                                    }

                                                                    //Copy Acquisition Price
                                                                    for (int i = 0; i < lstStockAvg.Count; i++)
                                                                    {
                                                                        if (i < operationItemsAvg.Count)
                                                                        {
                                                                            lstStockAvg[i].AcquisitionPrice = operationItemsAvg[i].AcquisitionPrice;
                                                                        }
                                                                    }
                                                                }

                                                                return stockOperation;
                                                            }).ToList();
                }


                List<StockOperationView> lstStockOperationSell = lstStockOperation.Where(objStkTmp => objStkTmp.OperationType == 2).ToList();

                if (lstStockOperationSell != null && lstStockOperationSell.Count > 0)
                {
                    lstStockOperationSellGrouped = lstStockOperationSell.GroupBy(objStockOperationTmp => objStockOperationTmp.Symbol)
                                                            .Select(objStockOperationGp =>
                                                            {
                                                                StockOperationView stockOperation = new StockOperationView();
                                                                stockOperation.Broker = objStockOperationGp.First().Broker;
                                                                stockOperation.Symbol = objStockOperationGp.First().Symbol;
                                                                stockOperation.NumberOfShares = objStockOperationGp.Sum(c => c.NumberOfShares);
                                                                stockOperation.AcquisitionPrice = objStockOperationGp.First().AcquisitionPrice;

                                                                List<StockOperationView> lstStockAvg = lstStockOperationSell.FindAll(objStockTmp => objStockTmp.Symbol == stockOperation.Symbol);

                                                                decimal totalShares = 0;
                                                                decimal totalAvgPrice = 0;
                                                                if (lstStockAvg != null && lstStockAvg.Count > 0)
                                                                {
                                                                    foreach (StockOperationView objStockAvg in lstStockAvg)
                                                                    {
                                                                        totalShares += objStockAvg.NumberOfShares;
                                                                        totalAvgPrice += objStockAvg.AveragePrice * objStockAvg.NumberOfShares;
                                                                    }

                                                                    if (totalShares != 0)
                                                                    {
                                                                        stockOperation.AveragePrice = totalAvgPrice / totalShares;
                                                                    }

                                                                }

                                                                #region Set Acquistion Price

                                                                List<StockOperationView> lstStockAvgAll = lstStockOperation.FindAll(objStockTmp => objStockTmp.Symbol == stockOperation.Symbol && objStockTmp.EventDate.HasValue).ToList();

                                                                if (lstStockAvgAll != null && lstStockAvgAll.Count > 0)
                                                                {
                                                                    lstStockAvgAll = lstStockAvgAll.OrderBy(op => op.EventDate.Value).ThenBy(op => op.OperationType).ToList();
                                                                    List<OperationItem> operationItemsAvg = new List<OperationItem>();

                                                                    foreach (StockOperationView item in lstStockAvgAll)
                                                                    {
                                                                        OperationItem operationItemAvg = new OperationItem();
                                                                        operationItemAvg.AveragePrice = item.AveragePrice;
                                                                        operationItemAvg.NumberOfShares = item.NumberOfShares;
                                                                        operationItemAvg.EventDate = item.EventDate;
                                                                        operationItemAvg.IdOperationType = item.OperationType;
                                                                        operationItemAvg.PriceAdjust = item.PriceAdjust;
                                                                        operationItemsAvg.Add(operationItemAvg);
                                                                    }

                                                                    decimal numberOfSharesCalc = 0;
                                                                    decimal avgPriceCalc = 0;
                                                                    bool valid = _operationService.CalculateAveragePrice(ref operationItemsAvg, out numberOfSharesCalc, out avgPriceCalc, false, null, false);

                                                                    for (int i = 0; i < lstStockAvgAll.Count; i++)
                                                                    {
                                                                        if (i < operationItemsAvg.Count)
                                                                        {
                                                                            lstStockAvgAll[i].AcquisitionPrice = operationItemsAvg[i].AcquisitionPrice;
                                                                        }
                                                                    }
                                                                }

                                                                #endregion

                                                                return stockOperation;
                                                            }).ToList();
                }
            }

            lstStockOperationRef = lstStockOperation;
        }

        public void CheckDivergence(long idPortfolio, List<StockOperationView> lstStockPortfolioGrouped, List<StockOperationView> lstStockOperationBuyGrouped, List<StockOperationView> lstStockOperationSellGrouped, List<StockOperationView> lstStockOperationCei, ref List<StockOperationView> lstStocks, bool fromCEI,
                                    IOperationItemService _operationItemService, IOperationService _operationService, IEnumerable<Stock> stocks)
        {
            List<OperationItemView> operationItemsViewDb = new List<OperationItemView>();
            ResultServiceObject<IEnumerable<OperationItemView>> resultOperationItem = _operationItemService.GetOperationItemsByIdPortfolio(idPortfolio);

            if (resultOperationItem.Success && resultOperationItem.Value != null && resultOperationItem.Value.Count() > 0)
            {
                operationItemsViewDb = resultOperationItem.Value.ToList();
            }

            if (lstStockPortfolioGrouped != null && lstStockPortfolioGrouped.Count > 0)
            {
                foreach (StockOperationView objStockPortfolio in lstStockPortfolioGrouped)
                {
                    Stock stock = stocks.FirstOrDefault(st => st.Symbol == objStockPortfolio.Symbol);

                    if (stock != null)
                    {
                        StockOperationView objStockOpFound = null;

                        if (lstStockOperationBuyGrouped != null && lstStockOperationBuyGrouped.Count > 0)
                        {
                            objStockOpFound = lstStockOperationBuyGrouped.Find(objStockOp => objStockOp.Symbol == objStockPortfolio.Symbol);
                        }

                        if (objStockOpFound != null)
                        {
                            objStockPortfolio.HasNewItem = objStockOpFound.HasNewItem;
                            objStockPortfolio.CopyData = objStockOpFound.CopyData;
                            objStockPortfolio.IsCeiOk = objStockOpFound.IsCeiOk;
                            objStockPortfolio.PriceLastEditedByUser = objStockOpFound.PriceLastEditedByUser;
                            objStockPortfolio.DaysLastItem = objStockOpFound.DaysLastItem;
                            objStockPortfolio.EditedByUser = objStockOpFound.EditedByUser;

                            if (fromCEI)
                            {
                                if (_operationService.HasRecentSubscription(stock.IdCompany, idPortfolio))
                                {
                                    objStockPortfolio.PriceLastEditedByUser = false;
                                }
                                else
                                {
                                    if (!objStockOpFound.PriceLastEditedByUser)
                                    {
                                        if (objStockPortfolio.NumberOfShares == objStockOpFound.NumberOfShares)
                                        {
                                            objStockPortfolio.AveragePrice = objStockOpFound.AveragePrice;
                                        }

                                        if (objStockOpFound.IsCeiOk)
                                        {
                                            if (objStockOpFound.DaysLastItem <= 6)
                                            {
                                                if (objStockOpFound.CopyData && (objStockOpFound.NumberOfShares != 0 || objStockOpFound.HasValidSell))
                                                {
                                                    objStockPortfolio.NumberOfShares = objStockOpFound.NumberOfShares;
                                                    objStockPortfolio.AveragePrice = objStockOpFound.AveragePrice;
                                                }
                                            }
                                            else
                                            {
                                                objStockPortfolio.AveragePrice = objStockOpFound.AveragePrice;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                objStockPortfolio.PriceLastEditedByUser = false;
                            }

                            //if (!objStockOpFound.PriceLastEditedByUser && fromCEI)
                            //{
                            //    if (objStockPortfolio.NumberOfShares == objStockOpFound.NumberOfShares)
                            //    {
                            //        objStockPortfolio.AveragePrice = objStockOpFound.AveragePrice;
                            //    }

                            //    if (objStockOpFound.IsCeiOk && objStockOpFound.DaysLastItem <= 5)
                            //    {
                            //        if (objStockOpFound.CopyData)
                            //        {
                            //            objStockPortfolio.NumberOfShares = objStockOpFound.NumberOfShares;
                            //            objStockPortfolio.AveragePrice = objStockOpFound.AveragePrice;
                            //        }
                            //    }
                            //}
                        }

                        lstStocks.Add(objStockPortfolio);
                    }
                }
            }

            if (lstStockOperationBuyGrouped != null && lstStockOperationBuyGrouped.Count > 0)
            {
                foreach (StockOperationView objStockOperation in lstStockOperationBuyGrouped)
                {
                    StockOperationView objStockPortfolioFound = null;
                    StockOperationView objStock = null;

                    if (lstStockPortfolioGrouped != null && lstStockPortfolioGrouped.Count > 0)
                    {
                        objStockPortfolioFound = lstStockPortfolioGrouped.Find(objStockPortTmp => objStockPortTmp.Symbol == objStockOperation.Symbol);
                    }

                    if (lstStocks != null && lstStocks.Count > 0)
                    {
                        objStock = lstStocks.Find(objStockTmp => objStockTmp.Symbol == objStockOperation.Symbol);
                    }

                    if (objStockPortfolioFound != null && objStockPortfolioFound.NumberOfShares == objStockOperation.NumberOfShares && objStock == null)
                    {
                        lstStocks.Add(objStockOperation);
                    }
                }
            }
        }


        private List<long> PreAdjustOperations(long idPortfolio, IEnumerable<Stock> stocks, List<StockOperationView> lstStockPortfolioGrouped, DateTime lastSync, IOperationService _operationService, IOperationItemService _operationItemService, IPortfolioService _portfolioService,
                            IOperationHistService _operationHistService, IOperationItemHistService _operationItemHistService, ISystemSettingsService _systemSettingsService, IPortfolioPerformanceService _portfolioPerformanceService, IStockService _stockService, IPerformanceStockService _performanceStockService, IHolidayService _holidayService)
        {
            List<long> operationsAdjusted = new List<long>();
            ResultServiceObject<IEnumerable<Operation>> resultOperations = _operationService.GetByPortfolioOperationType(idPortfolio, 1);

            if (resultOperations.Value != null && resultOperations.Value.Count() > 0)
            {
                foreach (Operation operation in resultOperations.Value)
                {
                    if (!_operationService.IsAdjusted(operation.IdOperation))
                    {
                        ResultServiceObject<IEnumerable<OperationItem>> resultOperationItem = _operationItemService.GetActiveByPortfolio(idPortfolio, true);

                        if (resultOperationItem.Value != null && resultOperationItem.Value.Count() > 0)
                        {
                            List<OperationItem> operationItemsFound = resultOperationItem.Value.Where(opItem => opItem.EventDate.HasValue && opItem.IdStock == operation.IdStock).ToList();

                            if (operationItemsFound != null && operationItemsFound.Count > 0)
                            {
                                bool updated = false;
                                operationsAdjusted.AddRange(PreAdjustOperation(stocks, lstStockPortfolioGrouped, lastSync, operation, operationItemsFound, out updated, _operationService, _portfolioService, _operationItemService, _operationHistService, _operationItemHistService, _systemSettingsService, _portfolioPerformanceService, _stockService, _performanceStockService, _holidayService));
                            }
                        }
                    }
                }
            }

            return operationsAdjusted;
        }

        private List<long> PreAdjustOperation(IEnumerable<Stock> stocks, List<StockOperationView> lstStockPortfolioGrouped, DateTime lastSync, Operation operation, List<OperationItem> operationItemsFound, out bool updated, IOperationService _operationService, IPortfolioService _portfolioService, IOperationItemService _operationItemService,
                            IOperationHistService _operationHistService, IOperationItemHistService _operationItemHistService, ISystemSettingsService _systemSettingsService, IPortfolioPerformanceService _portfolioPerformanceService, IStockService _stockService, IPerformanceStockService _performanceStockService, IHolidayService _holidayService)
        {
            List<long> operationsAdjusted = new List<long>();
            updated = false;
            decimal numberOfSharesCalc = 0;
            decimal avgPriceCalc = 0;
            DateTime? eventDateMatch = null;
            DateTime? lastUpdateDate = null;

            Stock stock = stocks.FirstOrDefault(stockTmp => stockTmp.IdStock == operation.IdStock);

            if (stock != null)
            {
                StockOperationView stockPortfolio = lstStockPortfolioGrouped.FirstOrDefault(op => op.Symbol == stock.Symbol);

                if (stockPortfolio != null)
                {
                    bool valid = _operationService.CalculateAveragePriceMatch(ref operationItemsFound, stockPortfolio.NumberOfShares, out numberOfSharesCalc, out avgPriceCalc, out lastUpdateDate, out eventDateMatch);

                    if (stockPortfolio.NumberOfShares == numberOfSharesCalc && stockPortfolio.NumberOfShares != operation.NumberOfShares && avgPriceCalc > 0 && valid)
                    {
                        operation.NumberOfShares = numberOfSharesCalc;
                        operation.AveragePrice = avgPriceCalc;
                        operation.Active = true;
                        _operationService.Update(operation);
                        updated = true;

                        DateTime? dateMax = eventDateMatch;

                        if (lastUpdateDate.HasValue)
                        {
                            lastSync = lastUpdateDate.Value;
                        }

                        OperationEditAvgPriceVM operationEditVM = new OperationEditAvgPriceVM();
                        operationEditVM.AveragePrice = operation.AveragePrice.ToString(new CultureInfo("pt-br"));
                        operationEditVM.NumberOfShares = operation.NumberOfShares.ToString(new CultureInfo("pt-br"));

                        //Adjust price for the new calc logic
                        _operationService.Adjust(operation.GuidOperation, operationEditVM, _portfolioService, _operationService, _operationItemService,
                            _operationHistService, _operationItemHistService, _systemSettingsService, _portfolioPerformanceService, _stockService, _performanceStockService, _holidayService, dateMax, false, true, lastSync, false);

                        _operationItemService.InactivePriceAdjust(operation.IdOperation);

                        operationsAdjusted.Add(operation.IdOperation);
                    }
                }
            }

            return operationsAdjusted;
        }

        private List<StockOperationView> GroupStockItems(long idPortfolio, IEnumerable<Stock> stocks, List<StockOperationView> lstStockPortfolio, List<StockOperationView> lstStockOperationRef, ref List<StockOperationView> lstStockPortfolioGrouped, bool inserted,
            IOperationItemService _operationItemService)
        {
            List<OperationItem> operationItemsDb = new List<OperationItem>();
            ResultServiceObject<IEnumerable<OperationItem>> resultOperationItem = _operationItemService.GetActiveByPortfolio(idPortfolio, true);
            List<StockOperationView> lstStockOperation = lstStockOperationRef;

            if (resultOperationItem.Success && resultOperationItem.Value != null && resultOperationItem.Value.Count() > 0)
            {
                operationItemsDb = resultOperationItem.Value.ToList();

                foreach (OperationItem operationItemDb in resultOperationItem.Value)
                {
                    if (lstStockOperation != null && lstStockOperation.Count > 0)
                    {
                        Stock stock = stocks.FirstOrDefault(stock => stock.IdStock == operationItemDb.IdStock);

                        if (stock == null)
                        {
                            continue;
                        }

                        StockOperationView stockOperationFound = lstStockOperation.FirstOrDefault(stockOperationTmp =>
                        {
                            string symbol = stock.Symbol;

                            if (stockOperationTmp.AveragePrice == operationItemDb.AveragePrice &&
                                stockOperationTmp.EventDate.HasValue == operationItemDb.EventDate.HasValue &&
                                stockOperationTmp.EventDate.Value.Date == operationItemDb.EventDate.Value.Date &&
                                stockOperationTmp.NumberOfShares == operationItemDb.NumberOfShares &&
                                stockOperationTmp.OperationType == operationItemDb.IdOperationType &&
                                stockOperationTmp.Market == operationItemDb.Market &&
                                stockOperationTmp.Expire == operationItemDb.Expire &&
                                stockOperationTmp.Factor == operationItemDb.Factor &&
                                stockOperationTmp.StockSpec == operationItemDb.StockSpec &&
                                stockOperationTmp.Symbol == symbol &&
                                (stockOperationTmp.IdOperationItem == 0 || (inserted && stockOperationTmp.IdOperationItem == operationItemDb.IdOperationItem)))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        });

                        if (stockOperationFound != null)
                        {
                            stockOperationFound.IdOperationItem = operationItemDb.IdOperationItem;
                            stockOperationFound.PriceAdjust = operationItemDb.PriceAdjust;
                            stockOperationFound.PriceAdjustNew = operationItemDb.PriceAdjustNew;
                            stockOperationFound.LastUpdatedDate = operationItemDb.LastUpdatedDate;
                            stockOperationFound.EditedByUser = operationItemDb.EditedByUser;
                            stockOperationFound.EventDate = operationItemDb.EventDate;
                        }
                        else
                        {
                            StockOperationView stockOperation = new StockOperationView();
                            stockOperation.IdOperationItem = operationItemDb.IdOperationItem;
                            stockOperation.AveragePrice = operationItemDb.AveragePrice;
                            stockOperation.Broker = operationItemDb.HomeBroker;
                            stockOperation.EventDate = operationItemDb.EventDate;
                            stockOperation.NumberOfShares = operationItemDb.NumberOfShares;
                            stockOperation.OperationType = operationItemDb.IdOperationType;
                            stockOperation.Market = operationItemDb.Market;
                            stockOperation.Expire = operationItemDb.Expire;
                            stockOperation.Factor = operationItemDb.Factor;
                            stockOperation.StockSpec = operationItemDb.StockSpec;
                            stockOperation.PriceAdjust = operationItemDb.PriceAdjust;
                            stockOperation.PriceAdjustNew = operationItemDb.PriceAdjustNew;
                            stockOperation.LastUpdatedDate = operationItemDb.LastUpdatedDate;
                            stockOperation.EditedByUser = operationItemDb.EditedByUser;
                            stockOperation.Symbol = stocks.FirstOrDefault(stock => stock.IdStock == operationItemDb.IdStock).Symbol;

                            lstStockOperation.Add(stockOperation);
                        }
                    }
                    else
                    {
                        lstStockOperation = new List<StockOperationView>();
                        StockOperationView stockOperation = new StockOperationView();
                        stockOperation.IdOperationItem = operationItemDb.IdOperationItem;
                        stockOperation.AveragePrice = operationItemDb.AveragePrice;
                        stockOperation.Broker = operationItemDb.HomeBroker;
                        stockOperation.EventDate = operationItemDb.EventDate;
                        stockOperation.NumberOfShares = operationItemDb.NumberOfShares;
                        stockOperation.OperationType = operationItemDb.IdOperationType;
                        stockOperation.Market = operationItemDb.Market;
                        stockOperation.Expire = operationItemDb.Expire;
                        stockOperation.Factor = operationItemDb.Factor;
                        stockOperation.StockSpec = operationItemDb.StockSpec;
                        stockOperation.PriceAdjust = operationItemDb.PriceAdjust;
                        stockOperation.PriceAdjustNew = operationItemDb.PriceAdjustNew;
                        stockOperation.LastUpdatedDate = operationItemDb.LastUpdatedDate;
                        stockOperation.EditedByUser = operationItemDb.EditedByUser;
                        stockOperation.Symbol = stocks.FirstOrDefault(stock => stock.IdStock == operationItemDb.IdStock).Symbol;

                        lstStockOperation.Add(stockOperation);
                    }
                }
            }

            if (lstStockPortfolio != null && lstStockPortfolio.Count > 0)
            {
                lstStockPortfolioGrouped = lstStockPortfolio.GroupBy(objStockPortfolioTmp => objStockPortfolioTmp.Symbol)
                                                            .Select(objStockPortfolioGp => new StockOperationView
                                                            {
                                                                Broker = objStockPortfolioGp.First().Broker,
                                                                Symbol = objStockPortfolioGp.First().Symbol,
                                                                NumberOfShares = objStockPortfolioGp.Sum(c => c.NumberOfShares),
                                                                AveragePrice = objStockPortfolioGp.First().AveragePrice,
                                                            }).ToList();
            }

            return lstStockOperation;
        }

        private static void AddNewStocksToPortfolio(List<StockOperationView> lstStockPortfolioGrouped, List<StockOperationView> lstStockOperation)
        {
            if (lstStockPortfolioGrouped == null)
            {
                lstStockPortfolioGrouped = new List<StockOperationView>();
            }

            List<StockOperationView> stocksOperationGpNew = lstStockOperation.Where(op => op.OperationType == 1 && !op.PriceAdjustNew && op.EventDate.HasValue && DateTime.Now.Subtract(op.EventDate.Value).TotalDays <= 6)
                                                                       .GroupBy(op => op.Symbol).Select(objStockOpGp => new StockOperationView
                                                                       {
                                                                           Broker = objStockOpGp.First().Broker,
                                                                           Symbol = objStockOpGp.First().Symbol,
                                                                           NumberOfShares = objStockOpGp.Sum(c => c.NumberOfShares),
                                                                           AveragePrice = 0,
                                                                       }).ToList();

            if (stocksOperationGpNew != null && stocksOperationGpNew.Count > 0)
            {
                foreach (StockOperationView stockOpNew in stocksOperationGpNew)
                {
                    if (lstStockPortfolioGrouped.Count == 0)
                    {
                        lstStockPortfolioGrouped.Add(stockOpNew);
                    }
                    else if (!lstStockPortfolioGrouped.Exists(op => op.Symbol == stockOpNew.Symbol))
                    {
                        lstStockPortfolioGrouped.Add(stockOpNew);
                    }
                }
            }
        }

        private void SaveOperationItem(IEnumerable<Stock> stocks, List<StockOperationView> lstStocksItem, long idPortfolio, DateTime lastSync, IOperationService _operationService, IOperationItemHistService _operationItemHistService, IOperationItemService _operationItemService, IStockSplitService _stockSplitService, bool newPortfolio, out decimal totalLossProfit, out List<string> changedCeiItems)
        {
            changedCeiItems = new List<string>();
            totalLossProfit = 0;
            ResultServiceObject<IEnumerable<Operation>> operationsDb = _operationService.GetByPortfolio(idPortfolio);

            if (operationsDb.Value != null && operationsDb.Value.Count() > 0)
            {
                //ResultServiceObject<IEnumerable<OperationItem>> resultOperationItem = _operationItemService.GetActiveByPortfolio(idPortfolio);
                //ResultServiceObject<IEnumerable<OperationItemView>> resultOperationItem = _operationItemService.GetAllItemViewByPortfolio(idPortfolio, 1, true);
                ResultServiceObject<IEnumerable<OperationItemHist>> operationItemsHists = _operationItemHistService.GetActiveByPortfolio(idPortfolio);

                if (lstStocksItem != null && lstStocksItem.Count() > 0)
                {
                    foreach (StockOperationView stockOperationItem in lstStocksItem)
                    {
                        if (stockOperationItem.IdOperationItem == 0)
                        {
                            Stock stock = stocks.FirstOrDefault(st => st.Symbol == stockOperationItem.Symbol);

                            if (stock != null)
                            {
                                Operation operation = operationsDb.Value.FirstOrDefault(op => op.IdStock == stock.IdStock && op.IdOperationType == stockOperationItem.OperationType);

                                if (operation != null)
                                {
                                    OperationItemHist operationItemHistFound = null;

                                    if (operationItemsHists.Value != null && operationItemsHists.Value.Count() > 0)
                                    {
                                        operationItemHistFound = operationItemsHists.Value.FirstOrDefault(stockOperationTmp =>
                                        {
                                            long idStock = stocks.FirstOrDefault(stockTmp => stockTmp.Symbol == stockOperationItem.Symbol).IdStock;

                                            if (stockOperationTmp.AveragePrice == stockOperationItem.AveragePrice &&
                                                stockOperationTmp.EventDate == stockOperationItem.EventDate &&
                                                stockOperationTmp.NumberOfShares == stockOperationItem.NumberOfShares &&
                                                stockOperationTmp.IdOperationType == stockOperationItem.OperationType &&
                                                stockOperationTmp.Market == stockOperationItem.Market &&
                                                stockOperationTmp.Expire == stockOperationItem.Expire &&
                                                stockOperationTmp.Factor == stockOperationItem.Factor &&
                                                stockOperationTmp.StockSpec == stockOperationItem.StockSpec &&
                                                stockOperationTmp.IdStock == idStock)
                                            {
                                                return true;
                                            }
                                            else
                                            {
                                                return false;
                                            }
                                        });
                                    }

                                    DateTime? lastUpdatedDate = null;

                                    if (operationItemHistFound != null && operationItemHistFound.Active)
                                    {
                                        lastUpdatedDate = operationItemHistFound.LastUpdatedDate;
                                        operationItemHistFound.Active = false;

                                        if (stockOperationItem.AcquisitionPrice == 0)
                                        {
                                            stockOperationItem.AcquisitionPrice = operationItemHistFound.AcquisitionPrice;
                                        }

                                        _operationItemHistService.Update(operationItemHistFound);
                                    }
                                    //else if (DateTime.Now.Subtract(stockOperationItem.EventDate.Value).TotalDays >= 7)
                                    //{
                                    //    lastUpdatedDate = lastSync.AddMinutes(-1);
                                    //}

                                    OperationItem operationItem = InsertOperationItem(stock, operation, stockOperationItem, _operationService, _operationItemService, _stockSplitService, lastUpdatedDate, newPortfolio);

                                    if (!operationItem.PriceAdjust && !operationItem.PriceAdjustNew)
                                    {
                                        if (!changedCeiItems.Exists(stk => stk == stock.Symbol))
                                        {
                                            changedCeiItems.Add(stock.Symbol);
                                        }

                                        if (operationItem.IdOperationType == 2 && !operationItem.PriceAdjustNew && !operationItem.PriceAdjust)
                                        {
                                            decimal lossProfit = (operationItem.AveragePrice - operationItem.AcquisitionPrice) * operationItem.NumberOfShares;
                                            totalLossProfit += lossProfit;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void AdjustOperations(long idPortfolio, DateTime lastSync, IEnumerable<Stock> stocks, List<StockOperationView> lstStockOperation, List<long> operationsAdjusted, IOperationService _operationService, IOperationItemService _operationItemService, IPortfolioService _portfolioService,
                            IOperationHistService _operationHistService, IOperationItemHistService _operationItemHistService, ISystemSettingsService _systemSettingsService, IPortfolioPerformanceService _portfolioPerformanceService, IStockService _stockService, IPerformanceStockService _performanceStockService, IHolidayService _holidayService)
        {
            ResultServiceObject<IEnumerable<Operation>> resultOperations = _operationService.GetByPortfolioOperationType(idPortfolio, 1);

            if (resultOperations.Value != null && resultOperations.Value.Count() > 0)
            {
                foreach (Operation operation in resultOperations.Value)
                {
                    Stock stock = stocks.FirstOrDefault(stockTmp => stockTmp.IdStock == operation.IdStock);

                    //do not include items adjusted previously
                    if ((operationsAdjusted.Count == 0 || !operationsAdjusted.Exists(idOperation => idOperation == operation.IdOperation)) && stock != null && stock.ShowOnPortolio)
                    {
                        ResultServiceObject<IEnumerable<OperationItem>> resultOperationItem = _operationItemService.GetActiveByPortfolio(idPortfolio, true);

                        if (resultOperationItem.Value != null && resultOperationItem.Value.Count() > 0)
                        {
                            List<OperationItem> operationItemsFound = resultOperationItem.Value.Where(opItem => opItem.EventDate.HasValue && opItem.IdStock == operation.IdStock).ToList();

                            if (operationItemsFound != null && operationItemsFound.Count > 0)
                            {
                                decimal numberOfSharesCalc = 0;
                                decimal avgPriceCalc = 0;
                                _operationService.CalculateAveragePrice(ref operationItemsFound, out numberOfSharesCalc, out avgPriceCalc, true, lastSync, false);

                                //double check if numberofshares and avgprice are ok
                                //if ((!_operationService.IsAdjusted(operation.IdOperation)) || (numberOfSharesCalc != operation.NumberOfShares && avgPriceCalc != operation.AveragePrice))

                                if (!_operationService.IsAdjusted(operation.IdOperation))
                                {
                                    //get max date excluding new items
                                    DateTime? dateMax = null;

                                    operationItemsFound = operationItemsFound.Where(opItem => !opItem.PriceAdjust).ToList();

                                    if (operationItemsFound != null && operationItemsFound.Count > 0)
                                    {
                                        operationItemsFound = operationItemsFound.Where(op =>
                                        {
                                            bool exist = false;

                                            if ((!op.PriceAdjust && op.EventDate.HasValue && (op.LastUpdatedDate < lastSync || op.PriceAdjustNew)))
                                            {
                                                exist = true;
                                            }
                                            else
                                            {
                                                exist = false;
                                            }

                                            return exist;
                                        }).ToList();

                                        if (operationItemsFound != null && operationItemsFound.Count > 0)
                                        {
                                            dateMax = operationItemsFound.Max(op => op.EventDate.Value);
                                        }
                                    }

                                    OperationEditAvgPriceVM operationEditVM = new OperationEditAvgPriceVM();
                                    operationEditVM.AveragePrice = operation.AveragePrice.ToString(new CultureInfo("pt-br"));
                                    operationEditVM.NumberOfShares = operation.NumberOfShares.ToString(new CultureInfo("pt-br"));

                                    //adjust according to the new logic
                                    _operationService.Adjust(operation.GuidOperation, operationEditVM, _portfolioService, _operationService, _operationItemService,
                                        _operationHistService, _operationItemHistService, _systemSettingsService, _portfolioPerformanceService, _stockService, _performanceStockService, _holidayService, dateMax, false, true, lastSync, false);
                                }
                            }
                        }
                    }
                }
            }
        }

        private List<long> CheckAdjust(long idPortfolio, DateTime lastSync, IOperationService _operationService, IOperationItemService _operationItemService, IPortfolioService _portfolioService,
                            IOperationHistService _operationHistService, IOperationItemHistService _operationItemHistService, ISystemSettingsService _systemSettingsService, IPortfolioPerformanceService _portfolioPerformanceService, IStockService _stockService, IPerformanceStockService _performanceStockService, IHolidayService _holidayService)
        {
            List<long> operationsAdjusted = new List<long>();
            ResultServiceObject<IEnumerable<Operation>> resultOperations = _operationService.GetByPortfolioOperationType(idPortfolio, 1);

            if (resultOperations.Value != null && resultOperations.Value.Count() > 0)
            {
                ResultServiceObject<IEnumerable<OperationItem>> resultOperationItem = _operationItemService.GetActiveByPortfolio(idPortfolio, true);

                if (resultOperationItem.Value != null && resultOperationItem.Value.Count() > 0)
                {
                    foreach (Operation operation in resultOperations.Value)
                    {
                        List<OperationItem> operationItemsFound = resultOperationItem.Value.Where(opItem => opItem.EventDate.HasValue && opItem.IdStock == operation.IdStock).ToList();

                        if (operationItemsFound != null && operationItemsFound.Count > 0)
                        {
                            //items with old price adjust logic
                            OperationItem opItemAdjustOld = operationItemsFound.LastOrDefault(op => op.PriceAdjust);
                            OperationItem opItemAdjustNew = operationItemsFound.LastOrDefault(op => op.PriceAdjustNew);

                            bool adjust = false;

                            if (opItemAdjustOld != null && opItemAdjustNew != null && opItemAdjustOld.IdOperationItem > opItemAdjustNew.IdOperationItem)
                            {
                                adjust = true;
                            }
                            else if (opItemAdjustOld != null && opItemAdjustNew == null)
                            {
                                adjust = true;
                            }

                            if (adjust)
                            {
                                //Get max date excluding new items
                                DateTime? dateMax = null;
                                List<OperationItem> operationItems = resultOperationItem.Value.Where(opItem => opItem.EventDate.HasValue && opItem.IdStock == operation.IdStock && !opItem.PriceAdjust).ToList();

                                if (operationItems != null && operationItems.Count > 0)
                                {
                                    operationItems = operationItems.Where(op =>
                                    {
                                        bool exist = false;

                                        if ((!op.PriceAdjust && opItemAdjustOld != null && op.EventDate.HasValue && (opItemAdjustOld.EventDate.HasValue &&
                                              op.EventDate.Value <= opItemAdjustOld.EventDate.Value || op.LastUpdatedDate <= lastSync || op.PriceAdjustNew)))
                                        {
                                            exist = true;
                                        }
                                        else
                                        {
                                            exist = false;
                                        }

                                        return exist;
                                    }).ToList();

                                    if (operationItems != null && operationItems.Count > 0)
                                    {
                                        dateMax = operationItems.Max(op => op.EventDate.Value);
                                    }
                                }

                                OperationEditAvgPriceVM operationEditVM = new OperationEditAvgPriceVM();
                                operationEditVM.AveragePrice = operation.AveragePrice.ToString(new CultureInfo("pt-br"));
                                operationEditVM.NumberOfShares = operation.NumberOfShares.ToString(new CultureInfo("pt-br"));

                                //Adjust price for the new calc logic
                                _operationService.Adjust(operation.GuidOperation, operationEditVM, _portfolioService, _operationService, _operationItemService,
                                    _operationHistService, _operationItemHistService, _systemSettingsService, _portfolioPerformanceService, _stockService, _performanceStockService, _holidayService, dateMax, false, true, lastSync, false);

                                //exclude old price adjust items
                                operationItemsFound = operationItemsFound.Where(opItem => opItem.PriceAdjust).ToList();

                                if (operationItemsFound != null && operationItemsFound.Count > 0)
                                {
                                    foreach (OperationItem operationItemFound in operationItemsFound)
                                    {
                                        operationItemFound.Active = false;
                                        _operationItemService.Update(operationItemFound);
                                    }
                                }

                                operationsAdjusted.Add(operation.IdOperation);
                            }
                        }
                    }
                }
            }

            return operationsAdjusted;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stocks"></param>
        /// <param name="lstStocks">Stocks Buy type grouped (Operation)</param>
        /// <param name="lstStocksSell">Stocks Buy type grouped (Operation)</param>
        /// <param name="lstStocksItem">Operation Items (Buy and Sell)</param>
        /// <param name="portfolio"></param>
        public void SaveOperation(IEnumerable<Stock> stocks, List<StockOperationView> lstStocks, List<StockOperationView> lstStocksSell, List<StockOperationView> lstStocksItem, Portfolio portfolio, string idUser, DateTime lastSync, IOperationService _operationService, IOperationItemService _operationItemService, ILogger _logger, IOperationItemHistService _operationItemHistService, IPortfolioService _portfolioService,
                            IOperationHistService _operationHistService, ISystemSettingsService _systemSettingsService, IPortfolioPerformanceService _portfolioPerformanceService, IStockService _stockService, IPerformanceStockService _performanceStockService, IHolidayService _holidayService, List<StockOperationView> lstStockAvgPrice, IStockSplitService _stockSplitService, bool newPortfolio, out decimal totalSold, out List<string> changedCeiItems)
        {
            totalSold = 0;
            changedCeiItems = new List<string>();
            List<Operation> operationsExclude = new List<Operation>();
            //Save Operation
            ResultServiceObject<IEnumerable<Operation>> resultServiceOperation = _operationService.GetByPortfolio(portfolio.IdPortfolio);
            ResultServiceObject<IEnumerable<OperationItem>> resultServiceOperationItem = _operationItemService.GetActiveByPortfolio(portfolio.IdPortfolio, true);

            if (resultServiceOperation.Success && resultServiceOperationItem.Success)
            {
                IEnumerable<Operation> operations = resultServiceOperation.Value;
                List<OperationItem> operationItems = resultServiceOperationItem.Value.ToList();

                if (lstStocks != null && lstStocks.Count > 0)
                {
                    if (operations != null && operations.Count() > 0)
                    {
                        foreach (Operation operation in operations)
                        {
                            Stock stock = stocks.FirstOrDefault(stockTmp => stockTmp.IdStock == operation.IdStock);

                            StockOperationView stockOperation = lstStocks.FirstOrDefault(stockOperationTmp => stockOperationTmp.Symbol == stock.Symbol);
                            StockOperationView stockOperationSell = null;

                            if (lstStocksSell.Count > 0)
                            {
                                stockOperationSell = lstStocksSell.FirstOrDefault(stockOperationTmp => stockOperationTmp.Symbol == stock.Symbol);
                            }

                            if ((stockOperation == null || stockOperation.NumberOfShares == 0) && (operation.IdOperationType == 1 && operation.Active))
                            {
                                if ((lstStocks.Any(stockTmp => stockTmp.Broker.Trim() == operation.HomeBroker.Trim()) || lstStocks.Count > 0))
                                {
                                    operation.Active = false;
                                    _operationService.Update(operation.IdOperation, false);
                                    //operationsExclude.Add(operation);
                                }
                                else
                                {
                                    _ = _logger.SendInformationAsync(new { Message = string.Format("Verificar operation inactive: {0} {1} ", operation.IdOperation, operation.HomeBroker) });
                                }
                            }
                            else if (stockOperation != null && !operation.Active && stockOperation.NumberOfShares != 0)
                            {
                                operation.Active = true;
                                _operationService.Update(operation.IdOperation, true);
                            }
                        }
                    }

                    SaveOperation(stocks, lstStocks, lstStocksItem, portfolio, operations, operationItems, 1, idUser, lastSync, _operationService, _operationItemService, _portfolioService,
                                    _operationHistService, _operationItemHistService, _systemSettingsService, _portfolioPerformanceService, _stockService, _performanceStockService, _holidayService, _logger, lstStockAvgPrice, out changedCeiItems);

                    List<string> changedCeiItemsItemSell = new List<string>();
                    SaveOperation(stocks, lstStocksSell, lstStocksItem, portfolio, operations, operationItems, 2, idUser, lastSync, _operationService, _operationItemService, _portfolioService,
                                    _operationHistService, _operationItemHistService, _systemSettingsService, _portfolioPerformanceService, _stockService, _performanceStockService, _holidayService, _logger, lstStockAvgPrice, out changedCeiItemsItemSell);

                    if (lstStocksItem != null && lstStocksItem.Count > 0)
                    {
                        if (lstStocksItem != null && lstStocksItem.Count > 0)
                        {
                            List<string> changedCeiItemsItem = new List<string>();
                            SaveOperationItem(stocks, lstStocksItem, portfolio.IdPortfolio, lastSync, _operationService, _operationItemHistService, _operationItemService, _stockSplitService, newPortfolio, out totalSold, out changedCeiItemsItem);
                            changedCeiItems.AddRange(changedCeiItemsItem);
                        }
                    }
                }
                else
                {
                    if (operations != null && operations.Count() > 0)
                    {
                        foreach (Operation operation in operations)
                        {
                            Stock stock = stocks.FirstOrDefault(stockTmp => stockTmp.IdStock == operation.IdStock);

                            if (stock != null)
                            {
                                StockOperationView stockOperationSell = null;

                                if (lstStocksSell.Count > 0)
                                {
                                    stockOperationSell = lstStocksSell.FirstOrDefault(stockOperationTmp => stockOperationTmp.Symbol == stock.Symbol);
                                }

                                if (operation.IdOperationType == 1 && operation.Active && stockOperationSell != null && stockOperationSell.NumberOfShares >= operation.NumberOfShares)
                                {
                                    operation.Active = false;
                                    _operationService.Update(operation.IdOperation, false);
                                    //operationsExclude.Add(operation);
                                }
                            }
                        }

                        _ = _logger.SendInformationAsync(new { Message = string.Format("All Inactive {0}", idUser) });
                    }
                }
            }
        }

        public void SaveOperationPassfolio(IEnumerable<Stock> stocks, List<StockOperationView> lstStocks, List<StockOperationView> lstStocksSell, List<StockOperationView> lstStocksItem, Portfolio portfolio, string idUser, DateTime lastSync, IOperationService _operationService, IOperationItemService _operationItemService, ILogger _logger, IOperationItemHistService _operationItemHistService, IPortfolioService _portfolioService,
                            IOperationHistService _operationHistService, ISystemSettingsService _systemSettingsService, IPortfolioPerformanceService _portfolioPerformanceService, IStockService _stockService, IPerformanceStockService _performanceStockService, IHolidayService _holidayService, IStockSplitService _stockSplitService, out decimal totalSold, out List<string> changedCeiItems)
        {
            totalSold = 0;
            changedCeiItems = new List<string>();
            List<Operation> operationsExclude = new List<Operation>();
            //Save Operation
            ResultServiceObject<IEnumerable<Operation>> resultServiceOperation = _operationService.GetByPortfolio(portfolio.IdPortfolio);
            ResultServiceObject<IEnumerable<OperationItem>> resultServiceOperationItem = _operationItemService.GetActiveByPortfolio(portfolio.IdPortfolio, true);

            if (resultServiceOperation.Success && resultServiceOperationItem.Success)
            {
                IEnumerable<Operation> operations = resultServiceOperation.Value;
                List<OperationItem> operationItems = resultServiceOperationItem.Value.ToList();

                if (operations != null && operations.Count() > 0)
                {
                    foreach (Operation operation in operations)
                    {
                        Stock stock = stocks.FirstOrDefault(stockTmp => stockTmp.IdStock == operation.IdStock);

                        StockOperationView stockOperation = lstStocks.FirstOrDefault(stockOperationTmp => stockOperationTmp.Symbol == stock.Symbol);
                        StockOperationView stockOperationSell = null;

                        if (lstStocksSell.Count > 0)
                        {
                            stockOperationSell = lstStocksSell.FirstOrDefault(stockOperationTmp => stockOperationTmp.Symbol == stock.Symbol);
                        }

                        if ((stockOperation == null || stockOperation.NumberOfShares == 0) && (operation.IdOperationType == 1 && operation.Active))
                        {
                            operation.Active = false;
                            _operationService.Update(operation.IdOperation, false);
                            //operationsExclude.Add(operation);
                        }
                        else if (stockOperation != null && !operation.Active && stockOperation != null && stockOperation.NumberOfShares != 0)
                        {
                            operation.Active = true;
                            _operationService.Update(operation.IdOperation, true);
                        }
                    }
                }

                SaveOperationPassfolio(stocks, lstStocks, lstStocksItem, portfolio, operations, operationItems, 1, idUser, lastSync, _operationService, _operationItemService, _portfolioService,
                                _operationHistService, _operationItemHistService, _systemSettingsService, _portfolioPerformanceService, _stockService, _performanceStockService, _holidayService, _logger, out changedCeiItems);

                List<string> changedCeiItemsItemSell = new List<string>();
                SaveOperationPassfolio(stocks, lstStocksSell, lstStocksItem, portfolio, operations, operationItems, 2, idUser, lastSync, _operationService, _operationItemService, _portfolioService,
                                _operationHistService, _operationItemHistService, _systemSettingsService, _portfolioPerformanceService, _stockService, _performanceStockService, _holidayService, _logger, out changedCeiItemsItemSell);

                if (lstStocksItem != null && lstStocksItem.Count > 0)
                {
                    if (lstStocksItem != null && lstStocksItem.Count > 0)
                    {
                        List<string> changedCeiItemsItem = new List<string>();
                        SaveOperationItemPassfolio(stocks, lstStocksItem, portfolio.IdPortfolio, lastSync, _operationService, _operationItemHistService, _operationItemService, _stockSplitService, out totalSold, out changedCeiItemsItem);
                        changedCeiItems.AddRange(changedCeiItemsItem);
                    }
                }
            }
        }

        private void SaveOperationPassfolio(IEnumerable<Stock> stocks, List<StockOperationView> lstStocks, List<StockOperationView> lstStocksItem, Portfolio portfolio, IEnumerable<Operation> operations, List<OperationItem> operationItemsDb, int idOperationType, string idUser, DateTime lastSync, IOperationService _operationService, IOperationItemService _operationItemService, IPortfolioService _portfolioService,
                            IOperationHistService _operationHistService, IOperationItemHistService _operationItemHistService, ISystemSettingsService _systemSettingsService, IPortfolioPerformanceService _portfolioPerformanceService, IStockService _stockService, IPerformanceStockService _performanceStockService, IHolidayService _holidayService, ILogger _logger, out List<string> changedCeiItems)
        {
            changedCeiItems = new List<string>();
            foreach (StockOperationView stockOperation in lstStocks)
            {
                Stock stock = stocks.FirstOrDefault(stockTmp => stockTmp.Symbol == stockOperation.Symbol);

                if (stock != null && ((idOperationType == 2) || (idOperationType == 1)))
                {
                    if (operations == null || operations.Count() == 0)
                    {
                        Operation operationDb = InsertOperation(portfolio, stockOperation, stock, idOperationType, _operationService);
                    }
                    else
                    {
                        Operation operation = operations.FirstOrDefault(operationTmp => operationTmp.IdStock == stock.IdStock && operationTmp.IdOperationType == idOperationType);

                        if (operation == null)
                        {
                            Operation operationDb = InsertOperation(portfolio, stockOperation, stock, idOperationType, _operationService);
                        }
                        else if (operation.Active)
                        {
                            List<StockOperationView> lstStockItemFound = lstStocksItem.Where(stockItemTmp => stockItemTmp.Symbol == stock.Symbol).ToList();

                            //Check if any new operation was included
                            bool operationModified = OperationModified(lstStockItemFound, operationItemsDb, stock);

                            if (stockOperation.AveragePrice != 0)
                            {
                                operation.AveragePrice = stockOperation.AveragePrice;
                            }

                            operation.NumberOfShares = stockOperation.NumberOfShares;
                            operation.HomeBroker = stockOperation.Broker;

                            if (operation.NumberOfShares == 0)
                            {
                                operation.Active = false;
                            }
                            else
                            {
                                operation.Active = true;
                            }

                            ResultServiceObject<Operation> resultOperationUpdate = _operationService.Update(operation);

                            _ = _logger.SendInformationAsync(new { Message = string.Format("Stock updated: {0} P.M: {1} / {2} Qtde: {3} / {4}", stockOperation.Symbol, operation.AveragePrice, stockOperation.AveragePrice, operation.NumberOfShares, stockOperation.NumberOfShares) });
                        }
                    }
                }
                else
                {
                    if (stock == null)
                    {
                        if (!string.IsNullOrWhiteSpace(stockOperation.Symbol))
                        {
                            if (stockOperation.Symbol.Count() > 5 && !char.IsNumber(stockOperation.Symbol[4]))
                            {
                                _ = _logger.SendInformationAsync(new { Message = string.Format("Option not found: {0}", stockOperation.Symbol) });
                            }
                            else
                            {
                                _ = _logger.SendInformationAsync(new { Message = string.Format("Stock not found {0} : {1} : {2}", idOperationType, stockOperation.Symbol, idUser) });
                            }
                        }
                    }
                    else
                    {
                        _ = _logger.SendInformationAsync(new { Message = string.Format("Stock not inserted : {0} {1} {2}", stock.Symbol, idOperationType, stock.ShowOnPortolio) });
                    }
                }
            }
        }

        private void SaveOperation(IEnumerable<Stock> stocks, List<StockOperationView> lstStocks, List<StockOperationView> lstStocksItem, Portfolio portfolio, IEnumerable<Operation> operations, List<OperationItem> operationItemsDb, int idOperationType, string idUser, DateTime lastSync, IOperationService _operationService, IOperationItemService _operationItemService, IPortfolioService _portfolioService,
                            IOperationHistService _operationHistService, IOperationItemHistService _operationItemHistService, ISystemSettingsService _systemSettingsService, IPortfolioPerformanceService _portfolioPerformanceService, IStockService _stockService, IPerformanceStockService _performanceStockService, IHolidayService _holidayService, ILogger _logger, List<StockOperationView> lstStockAvgPrice, out List<string> changedCeiItems)
        {
            changedCeiItems = new List<string>();
            foreach (StockOperationView stockOperation in lstStocks)
            {
                Stock stock = stocks.FirstOrDefault(stockTmp => stockTmp.Symbol == stockOperation.Symbol);

                if (stock != null && ((idOperationType == 2) || (idOperationType == 1)))
                {
                    if (operations == null || operations.Count() == 0)
                    {
                        //Set default avg price if was not calculated
                        if (stockOperation.AveragePrice == 0)
                        {
                            decimal avgPrice = CheckCeiAveragePrice(stock.Symbol, lstStockAvgPrice);

                            if (avgPrice == 0)
                            {
                                avgPrice = stock.MarketPrice;
                            }

                            stockOperation.AveragePrice = avgPrice;
                            stockOperation.HasNewItem = true;
                        }

                        Operation operationDb = InsertOperation(portfolio, stockOperation, stock, idOperationType, _operationService);
                    }
                    else
                    {
                        Operation operation = operations.FirstOrDefault(operationTmp => operationTmp.IdStock == stock.IdStock && operationTmp.IdOperationType == idOperationType);

                        if (operation == null)
                        {
                            //Set default avg price if was not calculated
                            if (stockOperation.AveragePrice == 0)
                            {
                                decimal avgPrice = CheckCeiAveragePrice(stock.Symbol, lstStockAvgPrice);

                                if (avgPrice == 0)
                                {
                                    avgPrice = stock.MarketPrice;
                                }

                                stockOperation.AveragePrice = avgPrice;
                                stockOperation.HasNewItem = true;
                            }

                            Operation operationDb = InsertOperation(portfolio, stockOperation, stock, idOperationType, _operationService);
                        }
                        else if (operation.Active)
                        {
                            List<StockOperationView> lstStockItemFound = lstStocksItem.Where(stockItemTmp => stockItemTmp.Symbol == stock.Symbol).ToList();

                            //Check if any new operation was included
                            bool operationModified = OperationModified(lstStockItemFound, operationItemsDb, stock);

                            //Set default avg price if was not calculated
                            if (stockOperation.AveragePrice == 0)
                            {
                                if (operation.AveragePrice == 0)
                                {
                                    decimal avgPrice = CheckCeiAveragePrice(stock.Symbol, lstStockAvgPrice);

                                    if (avgPrice == 0)
                                    {
                                        avgPrice = stock.MarketPrice;
                                    }

                                    operation.AveragePrice = avgPrice;
                                    stockOperation.HasNewItem = true;
                                }
                            }

                            //1st Priority: Not Edited By User (if price was last edited by user it will not be update)
                            if (((stockOperation.AveragePrice != 0 && Math.Round(stockOperation.AveragePrice, 2) != Math.Round(operation.AveragePrice, 2) && stockOperation.NumberOfShares != operation.NumberOfShares)
                                || (stockOperation.NumberOfShares != operation.NumberOfShares) || stockOperation.HasNewItem) && !stockOperation.PriceLastEditedByUser)
                            {
                                if (stockOperation.AveragePrice != 0)
                                {
                                    operation.AveragePrice = stockOperation.AveragePrice;
                                }

                                operation.NumberOfShares = stockOperation.NumberOfShares;
                                operation.HomeBroker = stockOperation.Broker;

                                if (operation.NumberOfShares == 0)
                                {
                                    operation.Active = false;
                                }
                                else
                                {
                                    operation.Active = true;
                                }

                                ResultServiceObject<Operation> resultOperationUpdate = _operationService.Update(operation);

                                if (stock.ShowOnPortolio && (idOperationType != 2 || stockOperation.HasNewItem))
                                {
                                    changedCeiItems.Add(stock.Symbol);
                                }

                                _ = _logger.SendInformationAsync(new { Message = string.Format("Stock updated: {0} P.M: {1} / {2} Qtde: {3} / {4}", stockOperation.Symbol, operation.AveragePrice, stockOperation.AveragePrice, operation.NumberOfShares, stockOperation.NumberOfShares) });
                            }
                        }
                    }
                }
                else
                {
                    if (stock == null)
                    {
                        if (!string.IsNullOrWhiteSpace(stockOperation.Symbol))
                        {
                            if (stockOperation.Symbol.Count() > 5 && !char.IsNumber(stockOperation.Symbol[4]))
                            {
                                _ = _logger.SendInformationAsync(new { Message = string.Format("Option not found: {0}", stockOperation.Symbol) });
                            }
                            else
                            {
                                _ = _logger.SendInformationAsync(new { Message = string.Format("Stock not found {0} : {1} : {2}", idOperationType, stockOperation.Symbol, idUser) });
                            }
                        }
                    }
                    else
                    {
                        _ = _logger.SendInformationAsync(new { Message = string.Format("Stock not inserted : {0} {1} {2}", stock.Symbol, idOperationType, stock.ShowOnPortolio) });
                    }
                }
            }
        }

        private OperationItem InsertOperationItem(Stock stock, Operation operationDb, StockOperationView stockItem, IOperationService _operationService, IOperationItemService _operationItemService, IStockSplitService _stockSplitService, DateTime? lastUpdatedDate = null, bool? newPortfolio = null)
        {
            OperationItem operationItem = new OperationItem();
            operationItem.AveragePrice = stockItem.AveragePrice;
            if (stockItem.EventDate.HasValue)
            {
                if (stockItem.EventDate.Value.TimeOfDay == TimeSpan.Zero)
                {
                    operationItem.EventDate = stockItem.EventDate.Value.Add(DateTime.Now.TimeOfDay);
                }
                else
                {
                    operationItem.EventDate = stockItem.EventDate.Value;
                }
            }
            operationItem.HomeBroker = !string.IsNullOrWhiteSpace(stockItem.Broker) ? stockItem.Broker.Trim() : null;
            operationItem.IdOperation = operationDb.IdOperation;
            operationItem.IdOperationType = stockItem.OperationType;
            operationItem.IdStock = stock.IdStock;
            operationItem.NumberOfShares = stockItem.NumberOfShares;
            operationItem.Market = !string.IsNullOrWhiteSpace(stockItem.Market) ? stockItem.Market.Trim() : null;
            operationItem.Expire = !string.IsNullOrWhiteSpace(stockItem.Expire) ? stockItem.Expire.Trim() : null;
            operationItem.Factor = !string.IsNullOrWhiteSpace(stockItem.Factor) ? stockItem.Factor.Trim() : null;
            operationItem.StockSpec = !string.IsNullOrWhiteSpace(stockItem.StockSpec) ? stockItem.StockSpec.Trim() : null;
            operationItem.AcquisitionPrice = stockItem.AcquisitionPrice;

            if (operationItem.IdOperationType == 2 && operationItem.AcquisitionPrice == 0)
            {
                ResultServiceObject<Operation> resultOperationBuy = _operationService.GetByPortfolioAndIdStock(operationDb.IdPortfolio, operationItem.IdStock, 1);

                if (resultOperationBuy.Value != null && resultOperationBuy.Value.Active)
                {
                    operationItem.AcquisitionPrice = resultOperationBuy.Value.AveragePrice;
                }
            }

            List<OperationItem> operationItems = new List<OperationItem>();
            operationItems.Add(operationItem);

            if (newPortfolio.HasValue)
            {
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

                if (newPortfolio.Value)
                {
                    _stockSplitService.ApplyStockSplit(ref operationItems, stock.IdCountry);
                    operationItem = operationItems.First();
                }
            }

            operationItem = _operationItemService.Insert(operationItem, lastUpdatedDate).Value;

            return operationItem;
        }

        private Operation InsertOperation(Portfolio portfolio, StockOperationView stockOperation, Stock stock, int idOperationType, IOperationService _operationService)
        {
            Operation operation = new Operation();
            operation.AveragePrice = stockOperation.AveragePrice;
            operation.NumberOfShares = stockOperation.NumberOfShares;
            operation.HomeBroker = stockOperation.Broker;
            operation.IdPortfolio = portfolio.IdPortfolio;
            operation.IdStock = stock.IdStock;
            operation.IdOperationType = idOperationType;

            if (stockOperation.NumberOfShares > 0)
            {
                operation.Active = true;

            }
            else
            {
                operation.Active = false;
            }

            ResultServiceObject<Operation> resultOperationInsert = _operationService.Insert(operation);

            return resultOperationInsert.Value;
        }

        public void SaveDividend(IEnumerable<Stock> stocks, List<DividendImportView> lstDividendImport, Portfolio portfolio, string idUser, IDividendService _dividendService, IDividendTypeService _dividendTypeService, ILogger _logger, out List<string> dividendCeiItems)
        {
            decimal delta = 1;
            dividendCeiItems = new List<string>();
            List<Dividend> lstDivFuture = new List<Dividend>();

            if (lstDividendImport != null && lstDividendImport.Count > 0)
            {
                ResultServiceObject<IEnumerable<Dividend>> resultServiceDividendFuture = _dividendService.GetByFutureDate(portfolio.IdPortfolio);

                if (resultServiceDividendFuture.Value != null && resultServiceDividendFuture.Value.Count() > 0)
                {
                    lstDivFuture = resultServiceDividendFuture.Value.Where(div => !(div.HomeBroker == "Sistema" && div.IdDividendType == 2)).ToList();

                    if (lstDivFuture != null && lstDivFuture.Count() > 0)
                    {
                        foreach (Dividend dividend in lstDivFuture)
                        {
                            _dividendService.Delete(dividend);
                        }
                    }
                }

                StringBuilder stringBuilder = new StringBuilder();

                //Save Dividend
                ResultServiceObject<IEnumerable<DividendView>> resultServiceDividend = _dividendService.GetAllAutomaticByPortfolio(portfolio.IdPortfolio);
                ResultServiceObject<IEnumerable<DividendType>> resultServiceDividendType = _dividendTypeService.GetAll();

                if (resultServiceDividend.Success && resultServiceDividendType.Success)
                {
                    IEnumerable<DividendView> dividends = resultServiceDividend.Value;
                    IEnumerable<DividendType> dividendTypes = resultServiceDividendType.Value;

                    foreach (DividendImportView dividendImport in lstDividendImport)
                    {
                        if (dividendCeiItems == null)
                        {
                            dividendCeiItems = new List<string>();
                        }

                        if (dividendImport.PaymentDate.HasValue && dividendImport.PaymentDate.Value <= DateTime.MinValue)
                        {
                            dividendImport.PaymentDate = null;
                        }

                        DividendType dividendType = dividendTypes.FirstOrDefault(dividendTypeTmp => dividendTypeTmp.IdDividendType != 8 &&
                                                                                                    dividendTypeTmp.IdDividendType != 9 &&
                                                                                                    dividendTypeTmp.IdDividendType != 10 &&
                                                                                                    (dividendTypeTmp.NameB3.ToLower() == dividendImport.DividendType.ToLower() ||
                                                                                                    dividendTypeTmp.NameNewB3Copy.ToLower().Contains(dividendImport.DividendType.ToLower())));
                        Stock stock = stocks.FirstOrDefault(stockTmp => stockTmp.Symbol == dividendImport.Symbol);

                        if (dividendType == null)
                        {
                            string st = "po";
                        }

                        if (dividendType != null)
                        {
                            if ((dividends == null || dividends.Count() == 0))
                            {
                                if (dividendType != null && stock != null)
                                {
                                    InsertDividend(portfolio, dividendImport, stock, dividendType, idUser, _dividendService);

                                    if (!dividendCeiItems.Exists(divStk => divStk == stock.Symbol))
                                    {
                                        dividendCeiItems.Add(stock.Symbol);
                                    }
                                }
                                else
                                {
                                    if (stock == null)
                                    {
                                        _ = _logger.SendInformationAsync(new { Message = string.Format("Dividend Stock not found: {0} : {1}", dividendImport.Symbol, idUser) });
                                    }

                                    if (dividendType == null)
                                    {
                                        _ = _logger.SendInformationAsync(new { Message = string.Format("Dividend type not found: {0}", dividendImport.DividendType) });
                                    }
                                }

                            }
                            else
                            {
                                DividendView dividend = dividends.FirstOrDefault(dividendTmp =>
                                {
                                    bool found = false;

                                    if (stock != null && dividendTmp.IdStock == stock.IdStock)
                                    {
                                        bool equalsNetValue = Math.Abs(dividendTmp.NetValue - dividendImport.NetValue) < delta;

                                        if (dividendTmp.PaymentDate.HasValue && dividendImport.PaymentDate.HasValue)
                                        {
                                            if (dividendTmp.PaymentDate.Value == dividendImport.PaymentDate.Value)
                                            {
                                                string brokerDb = string.Empty;
                                                string brokerImport = string.Empty;

                                                if (!string.IsNullOrWhiteSpace(dividendTmp.HomeBroker))
                                                {
                                                    brokerDb = dividendTmp.HomeBroker.Replace("S.A.", string.Empty).ToLower().Replace(".", string.Empty).Replace("-", string.Empty).Trim();
                                                }

                                                if (!string.IsNullOrWhiteSpace(dividendImport.Broker))
                                                {
                                                    brokerImport = dividendImport.Broker.Replace("S.A.", string.Empty).ToLower().Replace(".", string.Empty).Replace("-", string.Empty).Trim();
                                                }

                                                if (equalsNetValue && dividendTmp.IdDividendType == dividendType.IdDividendType && brokerDb == brokerImport)
                                                {
                                                    found = true;
                                                }
                                                else if (stock.IdStockType == 2 && dividendTmp.IdDividendType == dividendType.IdDividendType && brokerDb == brokerImport)
                                                {
                                                    found = true;
                                                }
                                                else if (dividendTmp.IdDividendType == dividendType.IdDividendType && dividendTmp.HomeBroker == "Sistema")
                                                {
                                                    found = true;
                                                }
                                            }
                                        }

                                        if (!dividendTmp.PaymentDate.HasValue && !dividendImport.PaymentDate.HasValue)
                                        {
                                            if (equalsNetValue)
                                            {
                                                found = true;
                                            }
                                        }
                                    }

                                    return found;
                                }
                                );

                                if (dividend == null && dividendType != null)
                                {
                                    if (dividendType != null && stock != null)
                                    {
                                        InsertDividend(portfolio, dividendImport, stock, dividendType, idUser, _dividendService);

                                        #region Check if Dividend was found on deleted list

                                        Dividend dividendFutureFound = lstDivFuture.FirstOrDefault(dividendTmp =>
                                        {
                                            bool found = false;

                                            if (stock != null && dividendTmp.IdStock == stock.IdStock)
                                            {
                                                bool equalsNetValue = Math.Abs(dividendTmp.NetValue - dividendImport.NetValue) < delta;

                                                if (dividendTmp.PaymentDate.HasValue && dividendImport.PaymentDate.HasValue)
                                                {
                                                    if (dividendTmp.PaymentDate.Value == dividendImport.PaymentDate.Value)
                                                    {
                                                        string brokerDb = string.Empty;
                                                        string brokerImport = string.Empty;

                                                        if (!string.IsNullOrWhiteSpace(dividendTmp.HomeBroker))
                                                        {
                                                            brokerDb = dividendTmp.HomeBroker.Replace("S.A.", string.Empty).ToLower().Replace(".", string.Empty).Replace("-", string.Empty).Trim();
                                                        }

                                                        if (!string.IsNullOrWhiteSpace(dividendImport.Broker))
                                                        {
                                                            brokerImport = dividendImport.Broker.Replace("S.A.", string.Empty).ToLower().Replace(".", string.Empty).Replace("-", string.Empty).Trim();
                                                        }

                                                        if (equalsNetValue && dividendTmp.IdDividendType == dividendType.IdDividendType && brokerDb == brokerImport)
                                                        {
                                                            found = true;
                                                        }
                                                        else if (stock.IdStockType == 2 && dividendTmp.IdDividendType == dividendType.IdDividendType && brokerDb == brokerImport)
                                                        {
                                                            found = true;
                                                        }
                                                    }
                                                }

                                                if (!dividendTmp.PaymentDate.HasValue && !dividendImport.PaymentDate.HasValue)
                                                {
                                                    if (equalsNetValue)
                                                    {
                                                        found = true;
                                                    }
                                                }
                                            }

                                            return found;
                                        });

                                        if (dividendFutureFound == null && dividendType != null)
                                        {
                                            if (!dividendCeiItems.Exists(divStk => divStk == stock.Symbol))
                                            {
                                                dividendCeiItems.Add(stock.Symbol);
                                            }
                                        }

                                        #endregion
                                    }
                                    else
                                    {
                                        if (stock == null)
                                        {
                                            _ = _logger.SendInformationAsync(new { Message = string.Format("Dividend Stock not found: {0} : {1}", dividendImport.Symbol, idUser) });
                                        }

                                        if (dividendType == null)
                                        {
                                            _ = _logger.SendInformationAsync(new { Message = string.Format("Dividend type not found: {0}", dividendImport.DividendType) });
                                        }
                                    }
                                }
                                else if (dividend != null && dividend.HomeBroker == "Sistema")
                                {
                                    if (dividendImport.PaymentDate.HasValue && dividend.PaymentDate == dividendImport.PaymentDate.Value && dividend.IdDividendType == dividendType.IdDividendType && dividend.IdStock == stock.IdStock)
                                    {
                                        ResultServiceObject<Dividend> resultDividendDb = _dividendService.GetById(dividend.IdDividend);

                                        if (resultDividendDb.Value != null)
                                        {
                                            _dividendService.Delete(resultDividendDb.Value);
                                        }

                                        InsertDividend(portfolio, dividendImport, stock, dividendType, idUser, _dividendService);
                                    }
                                }
                                else if (dividend != null && stock.IdStockType == 2)
                                {
                                    ResultServiceObject<Dividend> resultDividendDb = _dividendService.GetById(dividend.IdDividend);

                                    if (resultDividendDb.Success && resultDividendDb.Value != null)
                                    {
                                        resultDividendDb.Value.NetValue = dividendImport.NetValue;
                                        resultDividendDb.Value.GrossValue = dividendImport.GrossValue;
                                        resultDividendDb.Value.BaseQuantity = dividendImport.BaseQuantity;

                                        ResultServiceObject<Dividend> resultDividendInsert = _dividendService.Update(resultDividendDb.Value);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void InsertDividend(Portfolio portfolio, DividendImportView dividendImport, Stock stock, DividendType dividendType, string idUser, IDividendService _dividendService)
        {
            Dividend dividend = new Dividend();
            dividend.NetValue = dividendImport.NetValue;
            dividend.GrossValue = dividendImport.GrossValue;
            dividend.BaseQuantity = dividendImport.BaseQuantity;
            dividend.PaymentDate = dividendImport.PaymentDate;
            dividend.HomeBroker = dividendImport.Broker;
            dividend.IdPortfolio = portfolio.IdPortfolio;
            dividend.IdStock = stock.IdStock;
            dividend.IdDividendType = dividendType.IdDividendType;
            dividend.AutomaticImport = true;
            dividend.Active = true;
            ResultServiceObject<Dividend> resultDividendInsert = _dividendService.Insert(dividend);
        }

        public void SaveTesouroDireto(IEnumerable<TesouroDiretoImportView> tesouroDiretoImports, long idTrader, IFinancialProductService _financialProductService)
        {
            List<ProductUser> productUsers = new List<ProductUser>();

            ResultServiceObject<IEnumerable<ProductUserView>> productsUserView = _financialProductService.GetProductsByCategoryAndTrader(ProductCategoryEnum.TesouroDireto, idTrader);

            //Zerando posicao
            foreach (ProductUserView itemProductUserView in productsUserView.Value)
            {
                TesouroDiretoImportView tesouroDiretoImport = tesouroDiretoImports.FirstOrDefault(item => item.Symbol.Equals(itemProductUserView.ExternalName));

                if (tesouroDiretoImport == null)
                {
                    productUsers.Add(new ProductUser()
                    {
                        Active = false,
                        CreatedDate = itemProductUserView.CreatedDate,
                        CurrentValue = 0,
                        FinancialInstitutionID = itemProductUserView.FinancialInstitutionID,
                        ProductID = itemProductUserView.ProductID,
                        ProductUserGuid = itemProductUserView.ProductUserGuid,
                        ProductUserID = itemProductUserView.ProductUserID,
                        TraderID = itemProductUserView.TraderID
                    });
                }
            }


            foreach (TesouroDiretoImportView itemTesouroDiretoImport in tesouroDiretoImports)
            {
                bool addNewItem = true;

                foreach (ProductUserView itemProductUserView in productsUserView.Value)
                {
                    if (itemTesouroDiretoImport.Symbol.Equals(itemProductUserView.ExternalName) &&
                        itemTesouroDiretoImport.Broker.Equals(itemProductUserView.FinancialInstitution))
                    {
                        productUsers.Add(new ProductUser()
                        {
                            Active = true,
                            CreatedDate = itemProductUserView.CreatedDate,
                            CurrentValue = itemTesouroDiretoImport.NetValue,
                            FinancialInstitutionID = itemProductUserView.FinancialInstitutionID,
                            ProductID = itemProductUserView.ProductID,
                            ProductUserGuid = itemProductUserView.ProductUserGuid,
                            ProductUserID = itemProductUserView.ProductUserID,
                            TraderID = itemProductUserView.TraderID
                        });

                        addNewItem = false;

                        break;
                    }
                }

                if (addNewItem)
                {
                    ResultServiceObject<FinancialInstitution> financialInstitutionResult = _financialProductService.GetFinancialInstitutionByExternalCode(itemTesouroDiretoImport.Broker);
                    FinancialInstitution financialInstitution = new FinancialInstitution();

                    if (financialInstitutionResult.Value == null)
                    {
                        financialInstitution = new FinancialInstitution() { Name = itemTesouroDiretoImport.Broker, ExternalCode = itemTesouroDiretoImport.Broker };

                        _financialProductService.InsertFinancialInstitution(financialInstitution);
                    }
                    else
                    {
                        financialInstitution.FinancialInstitutionID = financialInstitutionResult.Value.FinancialInstitutionID;
                    }

                    ResultServiceObject<Product> productResult = _financialProductService.AddNewProductIfNotExist(itemTesouroDiretoImport.Symbol, ProductCategoryEnum.TesouroDireto);

                    productUsers.Add(new ProductUser()
                    {
                        CurrentValue = itemTesouroDiretoImport.NetValue,
                        FinancialInstitutionID = financialInstitution.FinancialInstitutionID,
                        ProductID = productResult.Value.ProductID,
                        TraderID = idTrader
                    });
                }
            }

            foreach (ProductUser item in productUsers)
            {
                _financialProductService.InsertOrUpdate(item);
            }
        }

        public void SaveCDB(IEnumerable<TesouroDiretoImportView> cdbImports, long idTrader, IFinancialProductService _financialProductService)
        {
            List<ProductUser> productUsers = new List<ProductUser>();

            ResultServiceObject<IEnumerable<ProductUserView>> productsUserView = _financialProductService.GetProductsByCategoryAndTrader(ProductCategoryEnum.CDB, idTrader);

            //Zerando posicao
            foreach (ProductUserView itemProductUserView in productsUserView.Value)
            {
                TesouroDiretoImportView tesouroDiretoImport = cdbImports.FirstOrDefault(item => item.Symbol.Equals(itemProductUserView.ExternalName));

                if (tesouroDiretoImport == null)
                {
                    productUsers.Add(new ProductUser()
                    {
                        Active = false,
                        CreatedDate = itemProductUserView.CreatedDate,
                        CurrentValue = 0,
                        FinancialInstitutionID = itemProductUserView.FinancialInstitutionID,
                        ProductID = itemProductUserView.ProductID,
                        ProductUserGuid = itemProductUserView.ProductUserGuid,
                        ProductUserID = itemProductUserView.ProductUserID,
                        TraderID = itemProductUserView.TraderID
                    });
                }
            }


            foreach (TesouroDiretoImportView itemTesouroDiretoImport in cdbImports)
            {
                bool addNewItem = true;

                foreach (ProductUserView itemProductUserView in productsUserView.Value)
                {
                    if (itemTesouroDiretoImport.Symbol.Equals(itemProductUserView.ExternalName) &&
                        itemTesouroDiretoImport.Broker.Equals(itemProductUserView.FinancialInstitution))
                    {
                        productUsers.Add(new ProductUser()
                        {
                            Active = true,
                            CreatedDate = itemProductUserView.CreatedDate,
                            CurrentValue = itemTesouroDiretoImport.NetValue,
                            FinancialInstitutionID = itemProductUserView.FinancialInstitutionID,
                            ProductID = itemProductUserView.ProductID,
                            ProductUserGuid = itemProductUserView.ProductUserGuid,
                            ProductUserID = itemProductUserView.ProductUserID,
                            TraderID = itemProductUserView.TraderID
                        });

                        addNewItem = false;

                        break;
                    }
                }

                if (addNewItem)
                {
                    ResultServiceObject<FinancialInstitution> financialInstitutionResult = _financialProductService.GetFinancialInstitutionByExternalCode(itemTesouroDiretoImport.Broker);
                    FinancialInstitution financialInstitution = new FinancialInstitution();

                    if (financialInstitutionResult.Value == null)
                    {
                        financialInstitution = new FinancialInstitution() { Name = itemTesouroDiretoImport.Broker, ExternalCode = itemTesouroDiretoImport.Broker };

                        _financialProductService.InsertFinancialInstitution(financialInstitution);
                    }
                    else
                    {
                        financialInstitution.FinancialInstitutionID = financialInstitutionResult.Value.FinancialInstitutionID;
                    }

                    ResultServiceObject<Product> productResult = _financialProductService.AddNewProductIfNotExist(itemTesouroDiretoImport.Symbol, ProductCategoryEnum.CDB);

                    productUsers.Add(new ProductUser()
                    {
                        CurrentValue = itemTesouroDiretoImport.NetValue,
                        FinancialInstitutionID = financialInstitution.FinancialInstitutionID,
                        ProductID = productResult.Value.ProductID,
                        TraderID = idTrader
                    });
                }
            }

            foreach (ProductUser item in productUsers)
            {
                _financialProductService.InsertOrUpdate(item);
            }
        }

        public void SaveFunds(IEnumerable<TesouroDiretoImportView> fundsImports, long idTrader, IFinancialProductService _financialProductService)
        {
            List<ProductUser> productUsers = new List<ProductUser>();

            ResultServiceObject<IEnumerable<ProductUserView>> productsUserView = _financialProductService.GetProductsByCategoryAndTrader(ProductCategoryEnum.Funds, idTrader);

            //Zerando posicao
            foreach (ProductUserView itemProductUserView in productsUserView.Value)
            {
                TesouroDiretoImportView tesouroDiretoImport = fundsImports.FirstOrDefault(item => item.Symbol.Equals(itemProductUserView.ExternalName));

                if (tesouroDiretoImport == null)
                {
                    productUsers.Add(new ProductUser()
                    {
                        Active = false,
                        CreatedDate = itemProductUserView.CreatedDate,
                        CurrentValue = 0,
                        FinancialInstitutionID = itemProductUserView.FinancialInstitutionID,
                        ProductID = itemProductUserView.ProductID,
                        ProductUserGuid = itemProductUserView.ProductUserGuid,
                        ProductUserID = itemProductUserView.ProductUserID,
                        TraderID = itemProductUserView.TraderID
                    });
                }
            }


            foreach (TesouroDiretoImportView itemTesouroDiretoImport in fundsImports)
            {
                bool addNewItem = true;

                foreach (ProductUserView itemProductUserView in productsUserView.Value)
                {
                    if (itemTesouroDiretoImport.Symbol.Equals(itemProductUserView.ExternalName) &&
                        itemTesouroDiretoImport.Broker.Equals(itemProductUserView.FinancialInstitution))
                    {
                        productUsers.Add(new ProductUser()
                        {
                            Active = true,
                            CreatedDate = itemProductUserView.CreatedDate,
                            CurrentValue = itemTesouroDiretoImport.NetValue,
                            FinancialInstitutionID = itemProductUserView.FinancialInstitutionID,
                            ProductID = itemProductUserView.ProductID,
                            ProductUserGuid = itemProductUserView.ProductUserGuid,
                            ProductUserID = itemProductUserView.ProductUserID,
                            TraderID = itemProductUserView.TraderID
                        });

                        addNewItem = false;

                        break;
                    }
                }

                if (addNewItem)
                {
                    ResultServiceObject<FinancialInstitution> financialInstitutionResult = _financialProductService.GetFinancialInstitutionByExternalCode(itemTesouroDiretoImport.Broker);
                    FinancialInstitution financialInstitution = new FinancialInstitution();

                    if (financialInstitutionResult.Value == null)
                    {
                        financialInstitution = new FinancialInstitution() { Name = itemTesouroDiretoImport.Broker, ExternalCode = itemTesouroDiretoImport.Broker };

                        _financialProductService.InsertFinancialInstitution(financialInstitution);
                    }
                    else
                    {
                        financialInstitution.FinancialInstitutionID = financialInstitutionResult.Value.FinancialInstitutionID;
                    }

                    ResultServiceObject<Product> productResult = _financialProductService.AddNewProductIfNotExist(itemTesouroDiretoImport.Symbol, ProductCategoryEnum.Funds);

                    productUsers.Add(new ProductUser()
                    {
                        CurrentValue = itemTesouroDiretoImport.NetValue,
                        FinancialInstitutionID = financialInstitution.FinancialInstitutionID,
                        ProductID = productResult.Value.ProductID,
                        TraderID = idTrader
                    });
                }
            }

            foreach (ProductUser item in productUsers)
            {
                _financialProductService.InsertOrUpdate(item);
            }
        }

        public void SaveDebentures(IEnumerable<TesouroDiretoImportView> debenturesImports, long idTrader, IFinancialProductService _financialProductService)
        {
            List<ProductUser> productUsers = new List<ProductUser>();

            ResultServiceObject<IEnumerable<ProductUserView>> productsUserView = _financialProductService.GetProductsByCategoryAndTrader(ProductCategoryEnum.Debenture, idTrader);

            //Zerando posicao
            foreach (ProductUserView itemProductUserView in productsUserView.Value)
            {
                TesouroDiretoImportView tesouroDiretoImport = debenturesImports.FirstOrDefault(item => item.Symbol.Equals(itemProductUserView.ExternalName));

                if (tesouroDiretoImport == null)
                {
                    productUsers.Add(new ProductUser()
                    {
                        Active = false,
                        CreatedDate = itemProductUserView.CreatedDate,
                        CurrentValue = 0,
                        FinancialInstitutionID = itemProductUserView.FinancialInstitutionID,
                        ProductID = itemProductUserView.ProductID,
                        ProductUserGuid = itemProductUserView.ProductUserGuid,
                        ProductUserID = itemProductUserView.ProductUserID,
                        TraderID = itemProductUserView.TraderID
                    });
                }
            }


            foreach (TesouroDiretoImportView itemTesouroDiretoImport in debenturesImports)
            {
                bool addNewItem = true;

                foreach (ProductUserView itemProductUserView in productsUserView.Value)
                {
                    if (itemTesouroDiretoImport.Symbol.Equals(itemProductUserView.ExternalName) &&
                        itemTesouroDiretoImport.Broker.Equals(itemProductUserView.FinancialInstitution))
                    {
                        productUsers.Add(new ProductUser()
                        {
                            Active = true,
                            CreatedDate = itemProductUserView.CreatedDate,
                            CurrentValue = itemTesouroDiretoImport.NetValue,
                            FinancialInstitutionID = itemProductUserView.FinancialInstitutionID,
                            ProductID = itemProductUserView.ProductID,
                            ProductUserGuid = itemProductUserView.ProductUserGuid,
                            ProductUserID = itemProductUserView.ProductUserID,
                            TraderID = itemProductUserView.TraderID
                        });

                        addNewItem = false;

                        break;
                    }
                }

                if (addNewItem)
                {
                    ResultServiceObject<FinancialInstitution> financialInstitutionResult = _financialProductService.GetFinancialInstitutionByExternalCode(itemTesouroDiretoImport.Broker);
                    FinancialInstitution financialInstitution = new FinancialInstitution();

                    if (financialInstitutionResult.Value == null)
                    {
                        financialInstitution = new FinancialInstitution() { Name = itemTesouroDiretoImport.Broker, ExternalCode = itemTesouroDiretoImport.Broker };

                        _financialProductService.InsertFinancialInstitution(financialInstitution);
                    }
                    else
                    {
                        financialInstitution.FinancialInstitutionID = financialInstitutionResult.Value.FinancialInstitutionID;
                    }

                    ResultServiceObject<Product> productResult = _financialProductService.AddNewProductIfNotExist(itemTesouroDiretoImport.Symbol, ProductCategoryEnum.Funds);

                    productUsers.Add(new ProductUser()
                    {
                        CurrentValue = itemTesouroDiretoImport.NetValue,
                        FinancialInstitutionID = financialInstitution.FinancialInstitutionID,
                        ProductID = productResult.Value.ProductID,
                        TraderID = idTrader
                    });
                }
            }

            foreach (ProductUser item in productUsers)
            {
                _financialProductService.InsertOrUpdate(item);
            }
        }

        public void SendNotificationImportation(bool automaticProcess, string idUser, bool imported, string errorMessage, IDeviceService _deviceService, INotificationHistoricalService _notificationHistoricalService, ICacheService _cacheService, INotificationService _notificationService, ILogger _logger, bool processFromCei = false)
        {
            ResultServiceObject<IEnumerable<Device>> devices = _deviceService.GetByUser(idUser);

            string pushMessage = string.Empty;
            string pushMessageTitle = string.Empty;

            if (imported)
            {
                pushMessage = "O processo de importação da sua carteira foi concluído com sucesso! Acesse o App Dividendos.me para visualizar suas informações.";
                pushMessageTitle = "Importação manual concluída";

                if (automaticProcess)
                {
                    pushMessage = "O processo de importação da sua carteira foi concluído com sucesso! (Esta atualização é diária e automática)";
                    pushMessageTitle = "Importação automática concluída";
                }
            }
            else
            {
                pushMessage = errorMessage;
                pushMessageTitle = "Falha na Importação manual";

                if (automaticProcess)
                {
                    pushMessage = errorMessage;
                    pushMessageTitle = "Falha na Importação automática";
                }

                pushMessage = string.IsNullOrWhiteSpace(pushMessage) ? "Ocorreu uma falha durante a integração ou seus ativos ainda não estão no CEI (B3). Por favor, tente novamente em alguns minutos." : pushMessage;
            }

            if (devices.Success && !automaticProcess)
            {
                if (imported)
                {
                    _notificationHistoricalService.New(pushMessageTitle, pushMessage, idUser, AppScreenNameEnum.Wallets.ToString(), PushRedirectTypeEnum.Internal.ToString(), null, _cacheService);
                }
                else
                {
                    if (processFromCei)
                    {
                        _notificationHistoricalService.New(pushMessageTitle, pushMessage, idUser, null, PushRedirectTypeEnum.External.ToString(), "https://ceiapp.b3.com.br/cei_responsivo/login.aspx", _cacheService);
                    }
                    else
                    {
                        _notificationHistoricalService.New(pushMessageTitle, pushMessage, idUser, AppScreenNameEnum.Wallets.ToString(), PushRedirectTypeEnum.Internal.ToString(), null, _cacheService);
                    }
                }

                foreach (Device itemDevice in devices.Value)
                {
                    try
                    {
                        if (imported)
                        {
                            _notificationService.SendPush(pushMessageTitle, pushMessage, itemDevice, new PushRedirect() { PushRedirectType = Entity.Enum.PushRedirectTypeEnum.Internal, AppScreenName = AppScreenNameEnum.Wallets });
                        }
                        else
                        {
                            if (processFromCei)
                            {
                                _notificationService.SendPush(pushMessageTitle, pushMessage, itemDevice, new PushRedirect() { PushRedirectType = Entity.Enum.PushRedirectTypeEnum.External, ExternalRedirectURL = "https://ceiapp.b3.com.br/cei_responsivo/login.aspx" });
                            }
                            else
                            {
                                _notificationService.SendPush(pushMessageTitle, pushMessage, itemDevice, new PushRedirect() { PushRedirectType = Entity.Enum.PushRedirectTypeEnum.Internal, AppScreenName = AppScreenNameEnum.Wallets });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _ = _logger.SendErrorAsync(ex);
                    }
                }
            }
        }

        public void SendNotificationNewItensOnPortfolio(string idUser,
            bool dividendNotification,
            string detailsAboutChangesMessage,
            ISettingsService _settingsService,
            IDeviceService _deviceService,
            INotificationHistoricalService _notificationHistoricalService,
            ICacheService _cacheService,
            INotificationService _notificationService,
            ILogger _logger,
            bool showOnPatrimony,
            bool restoreDvidends = false)
        {
            if (showOnPatrimony)
            {
                ResultServiceObject<Settings> settings = _settingsService.GetByUser(idUser);

                string pushMessage = string.Empty;
                string pushMessageTitle = string.Empty;


                if (restoreDvidends)
                {
                    if (settings.Value == null || settings.Value.PushNewDividend)
                    {
                        pushMessage = string.Format("🎉 Provento(s) recuperado(s): {0}. Veja mais detalhes no App Dividendos.me! 💰", detailsAboutChangesMessage);
                        pushMessageTitle = "🤑 Tem novidade nos seus dividendos 🤑";
                    }
                }
                else if (dividendNotification)
                {
                    if (settings.Value == null || settings.Value.PushNewDividend)
                    {
                        pushMessage = string.Format("🎉 Você tem o(s) seguinte(s) provento(s) agendado(s): {0}. Veja mais detalhes no App Dividendos.me! 💰", detailsAboutChangesMessage);
                        pushMessageTitle = "🤑 Tem novidade nos seus dividendos 🤑";
                    }
                }
                else
                {
                    if (settings.Value == null || settings.Value.PushChangeInPortfolio)
                    {
                        pushMessage = string.Format("Detectamos o(s) seguinte(s) ativo(s) alterado(s) na sua carteira: {0}. Veja mais detalhes no App Dividendos.me!", detailsAboutChangesMessage);
                        pushMessageTitle = "Tem novidade nos seus ativos 🧐";
                    }
                }

                if (!string.IsNullOrEmpty(pushMessage) && !string.IsNullOrEmpty(pushMessageTitle))
                {
                    ResultServiceObject<IEnumerable<Device>> devices = _deviceService.GetByUser(idUser);

                    _notificationHistoricalService.New(pushMessageTitle, pushMessage, idUser, AppScreenNameEnum.HomeToday.ToString(), PushRedirectTypeEnum.Internal.ToString(), null, _cacheService);

                    foreach (Device itemDevice in devices.Value)
                    {
                        try
                        {
                            _notificationService.SendPush(pushMessageTitle, pushMessage, itemDevice, new PushRedirect() { PushRedirectType = PushRedirectTypeEnum.Internal, AppScreenName = AppScreenNameEnum.HomeToday });
                        }
                        catch (Exception exception)
                        {
                            _logger.SendErrorAsync(exception);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Check if new operations were included
        /// </summary>
        /// <param name="lstStocksItem"></param>
        /// <param name="operationItemsDb"></param>
        /// <param name="stock"></param>
        /// <returns></returns>
        private static bool OperationModified(List<StockOperationView> lstStocksItem, List<OperationItem> operationItemsDb, Stock stock)
        {
            bool modified = false;

            if (lstStocksItem != null && lstStocksItem.Count > 0)
            {
                foreach (StockOperationView stockItem in lstStocksItem)
                {
                    bool found = operationItemsDb.Exists(stockOperationFound =>
                    {
                        if (stockOperationFound.AveragePrice == stockItem.AveragePrice &&
                            stockOperationFound.EventDate == stockItem.EventDate &&
                            stockOperationFound.NumberOfShares == stockItem.NumberOfShares &&
                            stockOperationFound.IdOperationType == stockItem.OperationType &&
                            stockOperationFound.Market == stockItem.Market &&
                            stockOperationFound.Expire == stockItem.Expire &&
                            stockOperationFound.Factor == stockItem.Factor &&
                            stockOperationFound.StockSpec == stockItem.StockSpec &&
                            stockOperationFound.IdStock == stock.IdStock)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    });

                    if (!found)
                    {
                        modified = true;
                        break;
                    }
                }
            }

            return modified;
        }

        private void SaveOperationItemPassfolio(IEnumerable<Stock> stocks, List<StockOperationView> lstStocksItem, long idPortfolio, DateTime lastSync, IOperationService _operationService, IOperationItemHistService _operationItemHistService, IOperationItemService _operationItemService, IStockSplitService _stockSplitService, out decimal totalLossProfit, out List<string> changedCeiItems)
        {
            changedCeiItems = new List<string>();
            totalLossProfit = 0;
            ResultServiceObject<IEnumerable<Operation>> operationsDb = _operationService.GetByPortfolio(idPortfolio);

            if (operationsDb.Value != null && operationsDb.Value.Count() > 0)
            {
                ResultServiceObject<IEnumerable<OperationItem>> resultOperationItem = _operationItemService.GetAllActiveByPortfolio(idPortfolio);
                ResultServiceObject<IEnumerable<OperationItemHist>> operationItemsHists = _operationItemHistService.GetActiveByPortfolio(idPortfolio);

                if (lstStocksItem != null && lstStocksItem.Count() > 0)
                {
                    foreach (StockOperationView stockOperationItem in lstStocksItem)
                    {
                        Stock stock = stocks.FirstOrDefault(st => st.Symbol == stockOperationItem.Symbol);

                        if (stock != null)
                        {
                            Operation operation = operationsDb.Value.FirstOrDefault(op => op.IdStock == stock.IdStock && op.IdOperationType == stockOperationItem.OperationType);

                            if (operation != null)
                            {
                                OperationItem operationItemFound =null;
                                OperationItem operationItem = null;
                                if (resultOperationItem.Value != null && resultOperationItem.Value.Count() > 0)
                                {
                                    operationItemFound = resultOperationItem.Value.FirstOrDefault(stockOperationTmp =>
                                    {
                                        long idStock = stocks.FirstOrDefault(stockTmp => stockTmp.Symbol == stockOperationItem.Symbol).IdStock;

                                        if (stockOperationTmp.AveragePrice == stockOperationItem.AveragePrice &&
                                            stockOperationTmp.EventDate == stockOperationItem.EventDate &&
                                            stockOperationTmp.NumberOfShares == stockOperationItem.NumberOfShares &&
                                            stockOperationTmp.IdOperationType == stockOperationItem.OperationType &&
                                            stockOperationTmp.IdStock == idStock)
                                        {
                                            return true;
                                        }
                                        else
                                        {
                                            return false;
                                        }
                                    });
                                }

                                if (operationItemFound == null)
                                {
                                    operationItem = InsertOperationItem(stock, operation, stockOperationItem, _operationService, _operationItemService, _stockSplitService);                                        
                                }

                                if (operationItem != null && !operationItem.PriceAdjust && !operationItem.PriceAdjustNew)
                                {
                                    if (!changedCeiItems.Exists(stk => stk == stock.Symbol))
                                    {
                                        changedCeiItems.Add(stock.Symbol);
                                    }

                                    if (operationItem.IdOperationType == 2 && !operationItem.PriceAdjustNew && !operationItem.PriceAdjust)
                                    {
                                        decimal lossProfit = (operationItem.AveragePrice - operationItem.AcquisitionPrice) * operationItem.NumberOfShares;
                                        totalLossProfit += lossProfit;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    
    
        public void GroupStocksPassfolio(long idPortfolio, IEnumerable<Stock> stocks, List<StockOperationView> lstStockPortfolio, ref List<StockOperationView> lstStockOperationRef, ref List<StockOperationView> lstStockPortfolioGrouped, ref List<StockOperationView> lstStockOperationBuyGrouped, ref List<StockOperationView> lstStockOperationSellGrouped, bool newPortfolio, DateTime lastSync,
            IOperationService _operationService, IOperationItemService _operationItemService, IOperationItemHistService _operationItemHistService, IPortfolioService _portfolioService,
                            IOperationHistService _operationHistService, ISystemSettingsService _systemSettingsService, IPortfolioPerformanceService _portfolioPerformanceService, IStockService _stockService, IPerformanceStockService _performanceStockService, IHolidayService _holidayService, IStockSplitService _stockSplitService, out decimal totalLossProfit, out List<string> changedCeiItems)
        {
            changedCeiItems = new List<string>();
            totalLossProfit = 0;
            List<long> operationsAdjusted = new List<long>();
            //Merge CEI op. items with DB op. items
            List<StockOperationView> lstStockOperation = GroupStockItems(idPortfolio, stocks, lstStockPortfolio, lstStockOperationRef, ref lstStockPortfolioGrouped, false, _operationItemService);

            //Save new op. items
            if (!newPortfolio)
            {
                List<StockOperationView> lstStockNew = lstStockOperation.FindAll(objStockTmp => objStockTmp.IdOperationItem == 0);

                if (lstStockNew != null && lstStockNew.Count > 0)
                {
                    List<string> changedCeiItemsOp = new List<string>();
                    SaveOperationItemPassfolio(stocks, lstStockNew, idPortfolio, lastSync, _operationService, _operationItemHistService, _operationItemService, _stockSplitService, out totalLossProfit, out changedCeiItemsOp);
                    changedCeiItems.AddRange(changedCeiItemsOp);
                }

                //operationsAdjusted.AddRange(PreAdjustOperations(idPortfolio, stocks, lstStockPortfolioGrouped, lastSync, _operationService, _operationItemService, _portfolioService, _operationHistService, _operationItemHistService, _systemSettingsService, _portfolioPerformanceService, _stockService, _performanceStockService, _holidayService));
            }

            //Adjust op. items with previous price adjust
            //operationsAdjusted.AddRange(CheckAdjust(idPortfolio, lastSync, _operationService, _operationItemService, _portfolioService, _operationHistService, _operationItemHistService, _systemSettingsService, _portfolioPerformanceService, _stockService, _performanceStockService, _holidayService));

            //if (!newPortfolio)
            //{
            //    //Set numberofhsares and avgprice according to the new calc logic
            //    AdjustOperations(idPortfolio, lastSync, stocks, lstStockOperation, operationsAdjusted, _operationService, _operationItemService, _portfolioService, _operationHistService, _operationItemHistService, _systemSettingsService, _portfolioPerformanceService, _stockService, _performanceStockService, _holidayService);
            //}

            //Merge CEI op. items with DB op. items again (including new items saved)
            lstStockOperation = GroupStockItems(idPortfolio, stocks, lstStockPortfolio, lstStockOperationRef, ref lstStockPortfolioGrouped, true, _operationItemService);

            if (lstStockOperation != null && lstStockOperation.Count > 0)
            {
                AddNewStocksToPortfolio(lstStockPortfolioGrouped, lstStockOperation);

                //Define Average Price
                if (lstStockPortfolioGrouped != null && lstStockPortfolioGrouped.Count > 0)
                {
                    lstStockOperationBuyGrouped = lstStockPortfolioGrouped.GroupBy(objStockOperationTmp => objStockOperationTmp.Symbol)
                                                            .Select(objStockOperationGp =>
                                                            {
                                                                StockOperationView stockOperation = new StockOperationView();
                                                                stockOperation.Broker = objStockOperationGp.First().Broker;
                                                                stockOperation.Symbol = objStockOperationGp.First().Symbol;
                                                                List<StockOperationView> lstStockAvg = lstStockOperation.FindAll(objStockTmp => objStockTmp.Symbol == stockOperation.Symbol && objStockTmp.EventDate.HasValue).ToList();

                                                                if (lstStockAvg != null && lstStockAvg.Count > 0)
                                                                {
                                                                    lstStockAvg = lstStockAvg.OrderBy(op => op.EventDate.Value).ThenBy(op => op.OperationType).ToList();

                                                                    long idOperationItem = lstStockAvg.Max(op => op.IdOperationItem);

                                                                    //if price was adjusted (new) then copy data
                                                                    stockOperation.CopyData = lstStockAvg.Exists(objStockTmp => (objStockTmp.PriceAdjustNew));

                                                                    //Check if a new item was returned
                                                                    stockOperation.HasNewItem = lstStockAvg.Exists(objStockTmp => objStockTmp.LastUpdatedDate > lastSync);

                                                                    stockOperation.PriceLastEditedByUser = lstStockAvg.Last().EditedByUser;

                                                                    List<StockOperationView> lstStockNewItem = lstStockAvg.FindAll(op => !op.PriceAdjustNew);

                                                                    if (lstStockNewItem != null && lstStockNewItem.Count > 0)
                                                                    {
                                                                        DateTime dtMaxNewItem = lstStockNewItem.Max(op => op.EventDate.Value);
                                                                        stockOperation.DaysLastItem = lstStockNewItem.Max(op => DateTime.Now.Date.Subtract(dtMaxNewItem.Date).TotalDays);
                                                                    }

                                                                    List<OperationItem> operationItemsAvg = new List<OperationItem>();

                                                                    foreach (StockOperationView item in lstStockAvg)
                                                                    {
                                                                        OperationItem operationItemAvg = new OperationItem();
                                                                        operationItemAvg.AveragePrice = item.AveragePrice;
                                                                        operationItemAvg.NumberOfShares = item.NumberOfShares;
                                                                        operationItemAvg.EventDate = item.EventDate;
                                                                        operationItemAvg.IdOperationType = item.OperationType;
                                                                        operationItemAvg.PriceAdjust = item.PriceAdjust;
                                                                        operationItemsAvg.Add(operationItemAvg);
                                                                    }

                                                                    decimal numberOfSharesCalc = 0;
                                                                    decimal avgPriceCalc = 0;
                                                                    bool valid = _operationService.CalculateAveragePrice(ref operationItemsAvg, out numberOfSharesCalc, out avgPriceCalc, false, null, false);

                                                                    for (int i = 0; i < lstStockAvg.Count; i++)
                                                                    {
                                                                        if (i < operationItemsAvg.Count)
                                                                        {
                                                                            lstStockAvg[i].AcquisitionPrice = operationItemsAvg[i].AcquisitionPrice;
                                                                        }
                                                                    }

                                                                    //if avg price is not valid do not copy data
                                                                    if (stockOperation.CopyData)
                                                                    {
                                                                        stockOperation.CopyData = valid;
                                                                    }

                                                                    ResultServiceObject<Operation> resultOp = _operationService.GetByIdOperationItem(idOperationItem);

                                                                    if ((newPortfolio && numberOfSharesCalc >= objStockOperationGp.First().NumberOfShares && valid) ||
                                                                        (!newPortfolio && (stockOperation.CopyData || (resultOp.Value == null || (resultOp.Value.IdOperation == 0 || !resultOp.Value.Active)))))
                                                                    {
                                                                        stockOperation.IsCeiOk = true;
                                                                        stockOperation.CopyData = true;
                                                                    }
                                                                    else
                                                                    {
                                                                        stockOperation.IsCeiOk = false;
                                                                    }

                                                                    if (valid)
                                                                    {
                                                                        stockOperation.NumberOfShares = numberOfSharesCalc;
                                                                        stockOperation.AveragePrice = avgPriceCalc;
                                                                    }

                                                                    //Copy Acquisition Price
                                                                    for (int i = 0; i < lstStockAvg.Count; i++)
                                                                    {
                                                                        if (i < operationItemsAvg.Count)
                                                                        {
                                                                            lstStockAvg[i].AcquisitionPrice = operationItemsAvg[i].AcquisitionPrice;
                                                                        }
                                                                    }
                                                                }

                                                                return stockOperation;
                                                            }).ToList();
                }


                List<StockOperationView> lstStockOperationSell = lstStockOperation.Where(objStkTmp => objStkTmp.OperationType == 2).ToList();

                if (lstStockOperationSell != null && lstStockOperationSell.Count > 0)
                {
                    lstStockOperationSellGrouped = lstStockOperationSell.GroupBy(objStockOperationTmp => objStockOperationTmp.Symbol)
                                                            .Select(objStockOperationGp =>
                                                            {
                                                                StockOperationView stockOperation = new StockOperationView();
                                                                stockOperation.Broker = objStockOperationGp.First().Broker;
                                                                stockOperation.Symbol = objStockOperationGp.First().Symbol;
                                                                stockOperation.NumberOfShares = objStockOperationGp.Sum(c => c.NumberOfShares);
                                                                stockOperation.AcquisitionPrice = objStockOperationGp.First().AcquisitionPrice;

                                                                List<StockOperationView> lstStockAvg = lstStockOperationSell.FindAll(objStockTmp => objStockTmp.Symbol == stockOperation.Symbol);

                                                                decimal totalShares = 0;
                                                                decimal totalAvgPrice = 0;
                                                                if (lstStockAvg != null && lstStockAvg.Count > 0)
                                                                {
                                                                    foreach (StockOperationView objStockAvg in lstStockAvg)
                                                                    {
                                                                        totalShares += objStockAvg.NumberOfShares;
                                                                        totalAvgPrice += objStockAvg.AveragePrice * objStockAvg.NumberOfShares;
                                                                    }

                                                                    if (totalShares != 0)
                                                                    {
                                                                        stockOperation.AveragePrice = totalAvgPrice / totalShares;
                                                                    }

                                                                }

                                                                #region Set Acquistion Price

                                                                List<StockOperationView> lstStockAvgAll = lstStockOperation.FindAll(objStockTmp => objStockTmp.Symbol == stockOperation.Symbol && objStockTmp.EventDate.HasValue).ToList();

                                                                if (lstStockAvgAll != null && lstStockAvgAll.Count > 0)
                                                                {
                                                                    lstStockAvgAll = lstStockAvgAll.OrderBy(op => op.EventDate.Value).ThenBy(op => op.OperationType).ToList();
                                                                    List<OperationItem> operationItemsAvg = new List<OperationItem>();

                                                                    foreach (StockOperationView item in lstStockAvgAll)
                                                                    {
                                                                        OperationItem operationItemAvg = new OperationItem();
                                                                        operationItemAvg.AveragePrice = item.AveragePrice;
                                                                        operationItemAvg.NumberOfShares = item.NumberOfShares;
                                                                        operationItemAvg.EventDate = item.EventDate;
                                                                        operationItemAvg.IdOperationType = item.OperationType;
                                                                        operationItemAvg.PriceAdjust = item.PriceAdjust;
                                                                        operationItemsAvg.Add(operationItemAvg);
                                                                    }

                                                                    decimal numberOfSharesCalc = 0;
                                                                    decimal avgPriceCalc = 0;
                                                                    bool valid = _operationService.CalculateAveragePrice(ref operationItemsAvg, out numberOfSharesCalc, out avgPriceCalc, false, null, false);

                                                                    for (int i = 0; i < lstStockAvgAll.Count; i++)
                                                                    {
                                                                        if (i < operationItemsAvg.Count)
                                                                        {
                                                                            lstStockAvgAll[i].AcquisitionPrice = operationItemsAvg[i].AcquisitionPrice;
                                                                        }
                                                                    }
                                                                }

                                                                #endregion

                                                                return stockOperation;
                                                            }).ToList();
                }
            }

            lstStockOperationRef = lstStockOperation;
        }

        public decimal? GetTotalPortfolio(long idPortfolio, DateTime limitDate, long? idStock = null, int? idStockType = null)
        {
            return _uow.PortfolioRepository.GetTotalPortfolio(idPortfolio, limitDate, idStock, idStockType);
        }

        private decimal CheckCeiAveragePrice(string symbol, List<StockOperationView> lstStockAvgPrice)
        {
            decimal avgPrice = 0;

            if ((lstStockAvgPrice != null && lstStockAvgPrice.Count > 0))
            {
                lstStockAvgPrice = lstStockAvgPrice.Where(avg => avg.Symbol == symbol).ToList();

                if ((lstStockAvgPrice != null && lstStockAvgPrice.Count > 0))
                {
                    List<StockOperationView> lstStockAvgPriceGrouped = lstStockAvgPrice.GroupBy(objStockOperationTmp => objStockOperationTmp.Symbol)
                                        .Select(objStockOperationGp =>
                                        {
                                            StockOperationView stockOperation = new StockOperationView();
                                            stockOperation.Broker = objStockOperationGp.First().Broker;
                                            stockOperation.Symbol = objStockOperationGp.First().Symbol;



                                            List<StockOperationView> lstStockAvg = lstStockAvgPrice.FindAll(objStockTmp => objStockTmp.Symbol == stockOperation.Symbol);

                                            decimal totalShares = 0;
                                            decimal totalAvgPrice = 0;

                                            if (lstStockAvg != null && lstStockAvg.Count > 0)
                                            {
                                                foreach (StockOperationView objStockAvg in lstStockAvg)
                                                {
                                                    totalShares += (objStockAvg.NumberOfShares - objStockAvg.NumberOfSellShares);
                                                    totalAvgPrice += (objStockAvg.AveragePrice * objStockAvg.NumberOfShares) - (objStockAvg.AveragePrice * objStockAvg.NumberOfSellShares);
                                                }

                                                if (totalShares > 0)
                                                {
                                                    stockOperation.AveragePrice = totalAvgPrice / totalShares;
                                                }

                                            }

                                            return stockOperation;
                                        }).ToList();



                    StockOperationView stockAvg = lstStockAvgPriceGrouped.FirstOrDefault(stck => stck.Symbol == symbol);

                    if (stockAvg != null)
                    {
                        avgPrice = stockAvg.AveragePrice;
                    }
                }
            }

            return avgPrice;
        }

        public bool HasZeroPrice(string idUser, string identifier, string password, DateTime deployDate)
        {
            return _uow.PortfolioRepository.HasZeroPrice(idUser, identifier, password, deployDate);
        }

        public ResultServiceObject<Portfolio> Update(Portfolio portfolio)
        {
            ResultServiceObject<Portfolio> resultService = new ResultServiceObject<Portfolio>();

            resultService.Value = _uow.PortfolioRepository.Update(portfolio);

            return resultService;
        }

        private void CheckOldSymbols(IEnumerable<Stock> stocks, ref List<StockOperationView> lstStockOperationRef, ref List<StockOperationView> lstStockPortfolioGrouped, ref List<DividendImportView> lstDividendImport)
        {
            if (stocks != null && stocks.Count() > 0)
            {
                List<Stock> stocksOtherSymbols = stocks.Where(stk => !string.IsNullOrWhiteSpace(stk.OldSymbols)).ToList();

                if (stocksOtherSymbols != null && stocksOtherSymbols.Count() > 0)
                {
                    foreach (Stock stkSymbol in stocksOtherSymbols)
                    {
                        if (lstStockOperationRef != null && lstStockOperationRef.Count > 0)
                        {
                            List<StockOperationView> stockOpOldSymbols = lstStockOperationRef.Where(stkOp => stkSymbol.OldSymbols.Contains(stkOp.Symbol)).ToList();

                            if (stockOpOldSymbols != null && stockOpOldSymbols.Count > 0)
                            {
                                foreach (StockOperationView stockOpOldSymbol in stockOpOldSymbols)
                                {
                                    stockOpOldSymbol.Symbol = stkSymbol.Symbol;
                                }
                            }
                        }

                        if (lstStockPortfolioGrouped != null && lstStockPortfolioGrouped.Count > 0)
                        {
                            List<StockOperationView> stkPortOldSymbols = lstStockPortfolioGrouped.Where(stkOp => stkSymbol.OldSymbols.Contains(stkOp.Symbol)).ToList();

                            if (stkPortOldSymbols != null && stkPortOldSymbols.Count > 0)
                            {
                                foreach (StockOperationView stkPortOldSymbol in stkPortOldSymbols)
                                {
                                    stkPortOldSymbol.Symbol = stkSymbol.Symbol;
                                }
                            }
                        }

                        if (lstDividendImport != null && lstDividendImport.Count > 0)
                        {
                            List<DividendImportView> dividendOldSymbols = lstDividendImport.Where(stkOp => stkSymbol.OldSymbols.Contains(stkOp.Symbol)).ToList();

                            if (dividendOldSymbols != null && dividendOldSymbols.Count > 0)
                            {
                                foreach (DividendImportView dividendOldSymbol in dividendOldSymbols)
                                {
                                    dividendOldSymbol.Symbol = stkSymbol.Symbol;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
