using Dividendos.Application;
using Dividendos.Application.Interface;
using Dividendos.B3;
using Dividendos.B3.Interface;
using Dividendos.Bacen;
using Dividendos.Bacen.Interface;
using Dividendos.Finance;
using Dividendos.Finance.Interface;
using Dividendos.Repository.Context;
using Dividendos.Repository.GenericRepository;
using Dividendos.Repository.Interface;
using Dividendos.Repository.Interface.GenericRepository;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Repository.Repository;
using Dividendos.Service;
using Dividendos.Service.Interface;
using Dividendos.TradeMap;
using Dividendos.TradeMap.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Dividendos.MercadoBitcoin;
using Dividendos.StatusInvest.Interface;
using Dividendos.StatusInvest;
using Dividendos.Nasdaq;
using Dividendos.Passfolio;
using Dividendos.Binance;
using Dividendos.Biscoint;
using Dividendos.Avenue.Interface;
using Dividendos.Avenue;
using Dividendos.Toro.Interface;
using Dividendos.Toro;
using Dividendos.InvestingCom.Interface;
using Dividendos.InvestingCom;
using Dividendos.AWS;
using Dividendos.NuInvest.Interface;
using Dividendos.NuInvest;
using Dividendos.Xp.Interface;
using Dividendos.Xp;
using Dividendos.Rico.Interface;
using Dividendos.Rico;
using Dividendos.Clear.Interface;
using Dividendos.Clear;
using Dividendos.BitPreco;
using Dividendos.BitPreco.Interface;

namespace Dividendos.CrossCutting.IoC
{
    public static class IoCConfig
    {
        public static void Config(IServiceCollection services, string connectionString)
        {

            #region [ Commom ]
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            #endregion


            #region [ Application ]

            services.AddTransient(typeof(IUserApp), typeof(UserApp));
            services.AddTransient(typeof(IDividendApp), typeof(DividendApp));
            services.AddTransient(typeof(IPortfolioApp), typeof(PortfolioApp));
            services.AddTransient(typeof(IStockApp), typeof(StockApp));
            services.AddTransient(typeof(IOperationApp), typeof(OperationApp));            
            services.AddTransient(typeof(IIndicatorApp), typeof(IndicatorApp));
            services.AddTransient(typeof(ITraderApp), typeof(TraderApp));
            services.AddTransient(typeof(ISettingsApp), typeof(SettingsApp));
            services.AddTransient(typeof(IDeviceApp), typeof(DeviceApp));
            services.AddTransient(typeof(ISubPortfolioApp), typeof(SubPortfolioApp));
            services.AddTransient(typeof(IDashboardApp), typeof(DashboardApp));
            services.AddTransient(typeof(INotificationAPP), typeof(NotificationApp));
            services.AddTransient(typeof(ICompanyApp), typeof(CompanyApp));
            services.AddTransient(typeof(ISyncQueueApp), typeof(SyncQueueApp));
            services.AddTransient(typeof(IDividendCalendarApp), typeof(DividendCalendarApp));
            services.AddTransient(typeof(ISupportChannelApp), typeof(SupportChannelApp));
            services.AddTransient(typeof(IInvestmentsSpecialistApp), typeof(InvestmentsSpecialistApp));
            services.AddTransient(typeof(ICryptoCurrencyApp), typeof(CryptoCurrencyApp));
            services.AddTransient(typeof(IFinancialProductsApp), typeof(FinancialProductsApp));
            services.AddTransient(typeof(IPurchaseApp), typeof(PurchaseApp));
            services.AddTransient(typeof(IPortfolioChartApp), typeof(PortfolioChartApp));
            services.AddTransient(typeof(IAffiliationApp), typeof(AffiliationApp));
            services.AddTransient(typeof(IHealthCheckApp), typeof(HealthCheckApp));
            services.AddTransient(typeof(IAdvertiserApp), typeof(AdvertiserApp));
            services.AddTransient(typeof(INotificationHistoricalApp), typeof(NotificationHistoricalApp));
            services.AddTransient(typeof(IStockSubscriptionApp), typeof(StockSubscriptionApp));
            services.AddTransient(typeof(IFollowStockApp), typeof(FollowStockApp));
            services.AddTransient(typeof(IExtraContentNotificationApp), typeof(ExtraContentNotificationApp));
            services.AddTransient(typeof(IHolidayApp), typeof(HolidayApp));
            services.AddTransient(typeof(IBrokerIntegrationApp), typeof(BrokerIntegrationApp));
            services.AddTransient(typeof(IPatrimonyApp), typeof(PatrimonyApp));
            services.AddTransient(typeof(ITutorialApp), typeof(TutorialApp));
            services.AddTransient(typeof(IPartnerApp), typeof(PartnerApp));
            services.AddTransient(typeof(IRelevantFactApp), typeof(RelevantFactApp));
            services.AddTransient(typeof(IAdvertiserExternalApp), typeof(AdvertiserExternalApp));
            services.AddTransient(typeof(IScrapySchedulerApp), typeof(ScrapySchedulerApp));
            services.AddTransient(typeof(IStockSplitApp), typeof(StockSplitApp));
            services.AddTransient(typeof(IScrapyAgentApp), typeof(ScrapyAgentApp));
            services.AddTransient(typeof(IInitialOfferApp), typeof(InitialOfferApp));
            services.AddTransient(typeof(IGoalApp), typeof(GoalApp));
            services.AddTransient(typeof(ITaggedUserApp), typeof(TaggedUserApp));
            services.AddTransient(typeof(ISearchApp), typeof(SearchApp));
            services.AddTransient(typeof(ICompanyIndicatorsApp), typeof(CompanyIndicatorsApp));
            services.AddTransient(typeof(IRDStationIntegrationApp), typeof(RDStationIntegrationApp));
            services.AddTransient(typeof(IInsightsApp), typeof(InsightsApp));
            services.AddTransient(typeof(ICryptoCurrencyPerformanceApp), typeof(CryptoCurrencyPerformanceApp));
            services.AddTransient(typeof(ICryptoPortfolioPerformanceApp), typeof(CryptoPortfolioPerformanceApp));
            services.AddTransient(typeof(ICryptoPortfolioApp), typeof(CryptoPortfolioApp));
            services.AddTransient(typeof(ICryptoTransactionItemApp), typeof(CryptoTransactionItemApp));
            services.AddTransient(typeof(ICryptoTransactionApp), typeof(CryptoTransactionApp));
            services.AddTransient(typeof(IFiatCurrencyApp), typeof(FiatCurrencyApp));
            services.AddTransient(typeof(ICryptoSubPortfolioApp), typeof(CryptoSubPortfolioApp));
            services.AddTransient(typeof(IInvestmentAdvisorApp), typeof(InvestmentAdvisorApp));
            services.AddTransient(typeof(IHurstApp), typeof(HurstApp));
            services.AddTransient(typeof(ITaxesApp), typeof(TaxesApp));
            services.AddTransient(typeof(IInfluencerApp), typeof(InfluencerApp));
            services.AddTransient(typeof(IVideoTutorialApp), typeof(VideoTutorialApp));
            services.AddTransient(typeof(IResearchProductApp), typeof(ResearchProductApp));
            #endregion

            #region [ Services ]

            services.AddTransient(typeof(IOperationService), typeof(OperationService));
            services.AddTransient(typeof(IOperationItemService), typeof(OperationItemService));
            services.AddTransient(typeof(IDividendService), typeof(DividendService));
            services.AddTransient(typeof(IDividendTypeService), typeof(DividendTypeService));
            services.AddTransient(typeof(IIndicatorSeriesService), typeof(IndicatorSeriesService));
            services.AddTransient(typeof(IPortfolioService), typeof(PortfolioService));
            services.AddTransient(typeof(ISubPortfolioService), typeof(SubPortfolioService));
            services.AddTransient(typeof(ISubPortfolioOperationService), typeof(SubPortfolioOperationService));
            services.AddTransient(typeof(IPortfolioPerformanceService), typeof(PortfolioPerformanceService));
            services.AddTransient(typeof(IPerformanceStockService), typeof(PerformanceStockService));
            services.AddTransient(typeof(ITraderService), typeof(TraderService));
            services.AddTransient(typeof(IStockTypeService), typeof(StockTypeService));
            services.AddTransient(typeof(IUserService), typeof(UserService));
            services.AddTransient(typeof(IStockService), typeof(StockService));
            services.AddTransient(typeof(ISectorService), typeof(SectorService));
            services.AddTransient(typeof(ISubsectorService), typeof(SubsectorService));
            services.AddTransient(typeof(ISegmentService), typeof(SegmentService));
            services.AddTransient(typeof(ISettingsService), typeof(SettingsService));
            services.AddTransient(typeof(IGlobalAuthenticationService), typeof(GlobalAuthenticationService));
            services.AddTransient(typeof(ICipherService), typeof(CipherService));
            services.AddTransient(typeof(IDeviceService), typeof(DeviceService));
            services.AddTransient(typeof(IEmailTemplateService), typeof(EmailTemplateService));
            services.AddTransient(typeof(ICompanyService), typeof(CompanyService));
            services.AddTransient(typeof(IOperationHistService), typeof(OperationHistService));
            services.AddTransient(typeof(IOperationItemHistService), typeof(OperationItemHistService));
            services.AddTransient(typeof(IDividendCalendarService), typeof(DividendCalendarService));
            services.AddTransient(typeof(ISyncQueueService), typeof(SyncQueueService));
            services.AddTransient(typeof(IDividendCalendarViewService), typeof(DividendCalendarViewService));
            services.AddTransient(typeof(ISyncQueueService), typeof(SyncQueueService));
            services.AddTransient(typeof(ISystemSettingsService), typeof(SystemSettingsService));
            services.AddTransient(typeof(ISupportChannelService), typeof(SupportChannelService));
            services.AddTransient(typeof(IInvestmentsSpecialistService), typeof(InvestmentsSpecialistService));
            services.AddTransient(typeof(ICryptoCurrencyService), typeof(CryptoCurrencyService));
            services.AddTransient(typeof(IFinancialProductService), typeof(FinancialProductService));
            services.AddTransient(typeof(ILogoService), typeof(LogoService));
            services.AddTransient(typeof(ISubscriptionService), typeof(SubscriptionService));
            services.AddTransient(typeof(IAffiliationService), typeof(AffiliationService));
            services.AddTransient(typeof(IMarketMoverService), typeof(MarketMoverService));
            services.AddTransient(typeof(INotificationService), typeof(NotificationService));
            services.AddTransient(typeof(IHealthCheckService), typeof(HealthCheckService));
            services.AddTransient(typeof(INotificationHistoricalService), typeof(NotificationHistoricalService));
            services.AddTransient(typeof(ICeiLogService), typeof(CeiLogService));
            services.AddTransient(typeof(IFollowStockService), typeof(FollowStockService));
            services.AddTransient(typeof(IFollowStockAlertService), typeof(FollowStockAlertService));
            services.AddTransient(typeof(IExtraContentNotificationService), typeof(ExtraContentNotificationService));
            services.AddTransient(typeof(ICacheService), typeof(CacheService));
            services.AddTransient(typeof(IHolidayService), typeof(HolidayService));
            services.AddTransient(typeof(IAdvertiserService), typeof(AdvertiserService));
            services.AddTransient(typeof(ITutorialService), typeof(TutorialService));
            services.AddTransient(typeof(IPartnerService), typeof(PartnerService));
            services.AddTransient(typeof(IRelevantFactService), typeof(RelevantFactService));
            services.AddTransient(typeof(IAdvertiserExternalService), typeof(AdvertiserExternalService));
            services.AddTransient(typeof(IScrapySchedulerService), typeof(ScrapySchedulerService));
            services.AddTransient(typeof(IStockSplitService), typeof(StockSplitService));
            services.AddTransient(typeof(IScrapyAgentService), typeof(ScrapyAgentService));
            services.AddTransient(typeof(IGoalService), typeof(GoalService));
            services.AddTransient(typeof(ITaggedUserService), typeof(TaggedUserService));
            services.AddTransient(typeof(IInitialOfferService), typeof(InitialOfferService));
            services.AddTransient(typeof(IS3Service), typeof(S3Service));
            services.AddTransient(typeof(ICompanyIndicatorsService), typeof(CompanyIndicatorsService));
            services.AddTransient(typeof(IInsightService), typeof(InsightService));
            services.AddTransient(typeof(ICryptoCurrencyPerformanceService), typeof(CryptoCurrencyPerformanceService));
            services.AddTransient(typeof(ICryptoPortfolioPerformanceService), typeof(CryptoPortfolioPerformanceService));
            services.AddTransient(typeof(ICryptoPortfolioService), typeof(CryptoPortfolioService));
            services.AddTransient(typeof(ICryptoTransactionItemService), typeof(CryptoTransactionItemService));
            services.AddTransient(typeof(ICryptoTransactionService), typeof(CryptoTransactionService));
            services.AddTransient(typeof(IFiatCurrencyService), typeof(FiatCurrencyService));
            services.AddTransient(typeof(ICryptoSubPortfolioService), typeof(CryptoSubPortfolioService));
            services.AddTransient(typeof(ICryptoSubPortfolioTransactionService), typeof(CryptoSubPortfolioTransactionService));
            services.AddTransient(typeof(IContactDetailsService), typeof(ContactDetailsService));
            services.AddTransient(typeof(IContactPhoneService), typeof(ContactPhoneService));
            services.AddTransient(typeof(IInfluencerService), typeof(InfluencerService));
            services.AddTransient(typeof(IVideoTutorialService), typeof(VideoTutorialService));
            services.AddTransient(typeof(IResearchProductService), typeof(ResearchProductService));
            #endregion

            #region [ External Services ]

            services.AddTransient(typeof(ISpreadsheetsHelper), typeof(SpreadsheetsHelper));
            services.AddTransient(typeof(IImportBacenHelper), typeof(ImportBacenHelper));
            services.AddTransient(typeof(ITradeMapHelper), typeof(TradeMapHelper));
            services.AddTransient(typeof(IStatusInvestHelper), typeof(StatusInvestHelper));
            services.AddTransient(typeof(ISpreadsheetsUSAHelper), typeof(SpreadsheetsUSAHelper));
            services.AddTransient(typeof(IAvenueHelper), typeof(AvenueHelper));
            services.AddTransient(typeof(IToroHelper), typeof(ToroHelper));
            services.AddTransient(typeof(IInvestingComHelper), typeof(InvestingComHelper));
            services.AddTransient(typeof(INuInvestHelper), typeof(NuInvestHelper));
            services.AddTransient(typeof(IXpHelper), typeof(XpHelper));
            services.AddTransient(typeof(IRicoHelper), typeof(RicoHelper));
            services.AddTransient(typeof(IClearHelper), typeof(ClearHelper));

            services.AddIntegrationBitcoinToYou();
            services.AddIntegrationBitcoinTrade();
            services.AddIntegrationCoinbase();
            services.AddIntegrationCoinNext();
            services.AddIntegrationMercadoBitCoin();
            services.AddIntegrationBinance();
            services.AddIntegrationNovaDax();
            services.AddNasdaq();
            services.AddPassfolio();
            services.AddIntegrationCoinMarketCap();
            services.AddIntegrationBiscoint();
            services.AddIntegrationBitPreco();
            services.AddIexAPIs();

            #endregion

            #region [ Repository ]

            services.AddScoped<IUnitOfWork, UnitOfWork>(service => new UnitOfWork(connectionString));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IDbContextOptions, DbContextOptions<ApplicationDbContext>>();

            #endregion
        }
    }
}
