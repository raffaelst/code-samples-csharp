
using Dividendos.Entity.Entities;
using Dividendos.Entity.Enum;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using K.Logger;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Service.Interface
{
    public interface IPortfolioService : IBaseService
    {
        ResultServiceObject<IEnumerable<Portfolio>> GetLastPortfoliosWithoutCalculation();
        ResultServiceObject<Portfolio> GetByTraderActive(long idTrader);
        ResultServiceObject<Portfolio> Insert(Portfolio portfolio);

        ResultServiceObject<IEnumerable<Portfolio>> GetAll();

        ResultServiceObject<IEnumerable<Portfolio>> GetByUser(string idUser, bool status, bool onlySelectedToShowOnPatrimony, bool? manual = null);
        ResultServiceObject<Portfolio> GetByGuid(Guid guidPortfolio);

        void Disable(long idPortfolio);
        ResultServiceObject<Portfolio> GetById(long idPortfolio);
        ResultServiceObject<IEnumerable<PortfolioView>> GetByUser(string idUser);
        ResultServiceObject<IEnumerable<PortfolioStatementView>> GetByPortfolio(Guid guidPortfolio, int? idStockType);
        ResultServiceObject<IEnumerable<PortfolioStatementView>> GetBySubportfolio(Guid guidSubportfolio, int? idStockType);
        ResultServiceObject<IEnumerable<PortfolioStatementView>> GetZeroPriceByUser(string idUser);
        void CalculatePerformance(long idPortfolio, ISystemSettingsService _systemSettingsService, IPortfolioPerformanceService _portfolioPerformanceService, IStockService _stockService, IOperationService _operationService, IPerformanceStockService _performanceStockService, IHolidayService _holidayService, decimal totalLossProfit = 0);
        ResultServiceObject<StockStatementView> GetByIdStock(Guid guidPortfolio, long idStock);
        ResultServiceObject<IEnumerable<PortfolioStatementView>> GetByPortfolioSubscription(Guid guidPortfolio);
        ResultServiceObject<IEnumerable<PortfolioStatementView>> GetBySubportfolioSubscription(Guid guidSubportfolio);
        ResultServiceObject<Portfolio> GetByIdOperationItem(long idOperationItem);
        ResultServiceObject<Portfolio> GetByIdOperationItemHist(long idOperationItemHist);

        ResultServiceObject<bool> HasSubscription(Guid guidPortfolioOrSubportfolio);
        PortfolioPerformance CalculateSubPortfolioPerformance(PortfolioPerformance portfolioPerformance, long idSubPortfolio, IPortfolioPerformanceService _portfolioPerformanceService, IPerformanceStockService _performanceStockService, IOperationService _operationService, ISubPortfolioOperationService _subPortfolioOperationService);
        void CalculateSubPortfolioPerc(PortfolioPerformance portfolioPerformance, long idSubPortfolio, IPerformanceStockService _performanceStockService, IOperationService _operationService, ISubPortfolioOperationService _subPortfolioOperationService);
        ResultServiceObject<IEnumerable<PortfolioView>> GetSubportfolioPerformance(Guid guidPortfolioSub, DateTime? startDate, DateTime? endDate);
        void UpdateCalculatePerformanceDate(long idPortfolio, DateTime dateTime);
        void UpdateName(long idPortfolio, string name);
        void SaveTesouroDireto(IEnumerable<TesouroDiretoImportView> tesouroDiretoImports, long idTrader, IFinancialProductService _financialProductService);

        void SaveCDB(IEnumerable<TesouroDiretoImportView> cdbImports, long idTrader, IFinancialProductService _financialProductService);

        void SaveFunds(IEnumerable<TesouroDiretoImportView> fundsImports, long idTrader, IFinancialProductService _financialProductService);

        ImportCeiResultView FinishImportCei(string identifier, string password, string idUser, bool automaticProcess, ImportCeiResultView importCeiResult,
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
                                        bool newCei = false
                                        );

        Portfolio SavePortfolio(Trader trader, CountryEnum countryEnum, string sufixoCarteira, out bool newPortfolio);

        void GroupStocks(long idPortfolio, IEnumerable<Stock> stocks, List<StockOperationView> lstStockPortfolio, ref List<StockOperationView> lstStockOperationRef, ref List<StockOperationView> lstStockPortfolioGrouped, ref List<StockOperationView> lstStockOperationBuyGrouped, ref List<StockOperationView> lstStockOperationSellGrouped, bool newPortfolio, DateTime lastSync,
            IOperationService _operationService, IOperationItemService _operationItemService, IOperationItemHistService _operationItemHistService, IPortfolioService _portfolioService,
                            IOperationHistService _operationHistService, ISystemSettingsService _systemSettingsService, IPortfolioPerformanceService _portfolioPerformanceService, IStockService _stockService, IPerformanceStockService _performanceStockService, IHolidayService _holidayService, IStockSplitService _stockSplitService, out decimal totalLossProfit, out List<string> changedCeiItems);

        void CheckDivergence(long idPortfolio, List<StockOperationView> lstStockPortfolioGrouped, List<StockOperationView> lstStockOperationBuyGrouped, List<StockOperationView> lstStockOperationSellGrouped, List<StockOperationView> lstStockOperationCei, ref List<StockOperationView> lstStocks, bool fromCEI,
                                    IOperationItemService _operationItemService, IOperationService _operationService, IEnumerable<Stock> stocks);

        void SaveOperation(IEnumerable<Stock> stocks, List<StockOperationView> lstStocks, List<StockOperationView> lstStocksSell, List<StockOperationView> lstStocksItem, Portfolio portfolio, string idUser, DateTime lastSync, IOperationService _operationService, IOperationItemService _operationItemService, ILogger _logger, IOperationItemHistService _operationItemHistService, IPortfolioService _portfolioService,
                            IOperationHistService _operationHistService, ISystemSettingsService _systemSettingsService, IPortfolioPerformanceService _portfolioPerformanceService, IStockService _stockService, IPerformanceStockService _performanceStockService, IHolidayService _holidayService, List<StockOperationView> lstStockAvgPrice, IStockSplitService _stockSplitService, bool newPortfolio, out decimal totalLossProfit, out List<string> changedCeiItems);

        void SaveOperationPassfolio(IEnumerable<Stock> stocks, List<StockOperationView> lstStocks, List<StockOperationView> lstStocksSell, List<StockOperationView> lstStocksItem, Portfolio portfolio, string idUser, DateTime lastSync, IOperationService _operationService, IOperationItemService _operationItemService, ILogger _logger, IOperationItemHistService _operationItemHistService, IPortfolioService _portfolioService,
                            IOperationHistService _operationHistService, ISystemSettingsService _systemSettingsService, IPortfolioPerformanceService _portfolioPerformanceService, IStockService _stockService, IPerformanceStockService _performanceStockService, IHolidayService _holidayService, IStockSplitService _stockSplitService, out decimal totalLossProfit, out List<string> changedCeiItems);

        void SaveDividend(IEnumerable<Stock> stocks, List<DividendImportView> lstDividendImport, Portfolio portfolio, string idUser, IDividendService _dividendService, IDividendTypeService _dividendTypeService, ILogger _logger, out List<string> dividendCeiItems);

        void SendNotificationImportation(bool automaticProcess, string idUser, bool imported, string errorMessage, IDeviceService _deviceService, INotificationHistoricalService _notificationHistoricalService, ICacheService _cacheService, INotificationService _notificationService, ILogger _logger, bool processFromCei = false);
        void SendNotificationNewItensOnPortfolio(string idUser, bool dividendNotification, string detailsAboutChangesMessage, ISettingsService _settingsService, IDeviceService _deviceService, INotificationHistoricalService _notificationHistoricalService, ICacheService _cacheService, INotificationService _notificationService, ILogger _logger, bool showOnPatrimony, bool restoreDvidends = false);
    
        void GroupStocksPassfolio(long idPortfolio, IEnumerable<Stock> stocks, List<StockOperationView> lstStockPortfolio, ref List<StockOperationView> lstStockOperationRef, ref List<StockOperationView> lstStockPortfolioGrouped, ref List<StockOperationView> lstStockOperationBuyGrouped, ref List<StockOperationView> lstStockOperationSellGrouped, bool newPortfolio, DateTime lastSync,
            IOperationService _operationService, IOperationItemService _operationItemService, IOperationItemHistService _operationItemHistService, IPortfolioService _portfolioService,
                            IOperationHistService _operationHistService, ISystemSettingsService _systemSettingsService, IPortfolioPerformanceService _portfolioPerformanceService, IStockService _stockService, IPerformanceStockService _performanceStockService, IHolidayService _holidayService, IStockSplitService _stockSplitService, out decimal totalLossProfit, out List<string> changedCeiItems);

        decimal? GetTotalPortfolio(long idPortfolio, DateTime limitDate, long? idStock = null, int? idStockType = null);

        bool HasZeroPrice(string idUser, string identifier, string password, DateTime deployDate);
        ResultServiceObject<Portfolio> Update(Portfolio portfolio);
        void SaveDebentures(IEnumerable<TesouroDiretoImportView> debenturesImports, long idTrader, IFinancialProductService _financialProductService);
        ResultServiceObject<Portfolio> GetByTrader(long idTrader);
    }
}
