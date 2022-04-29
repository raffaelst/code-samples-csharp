using K.UnitOfWorkBase;
using Dividendos.Repository.Interface;
using Dividendos.Repository.Interface.UoW;

namespace Dividendos.Repository.Repository
{
    public class UnitOfWork : UnitOfWorkBase, IUnitOfWork
    {
        public UnitOfWork(string connectionString) : base(connectionString)
        {

        }

        public IUserRepository UserRepository
        {
            get => new UserRepository(this);
        }

        public IDividendRepository DividendRepository
        {
            get => new DividendRepository(this);
        }
        public IDividendViewRepository DividendViewRepository
        {
            get => new DividendViewRepository(this);
        }
        public IPortfolioViewRepository PortfolioViewRepository
        {
            get => new PortfolioViewRepository(this);
        }

        public IPortfolioStatementViewRepository PortfolioStatementViewRepository
        {
            get => new PortfolioStatementViewRepository(this);
        }

        public IStockStatementViewRepository StockStatementViewRepository
        {
            get => new StockStatementViewRepository(this);
        }

        public IDividendTypeRepository DividendTypeRepository
        {
            get => new DividendTypeRepository(this);
        }

        public IStockTypeRepository StockTypeRepository
        {
            get => new StockTypeRepository(this);
        }

        public IIndicatorSeriesRepository IndicatorSeriesRepository
        {
            get => new IndicatorSeriesRepository(this);
        }

        public IIndicatorSeriesViewRepository IndicatorSeriesViewRepository
        {
            get => new IndicatorSeriesViewRepository(this);
        }

        public IOperationRepository OperationRepository
        {
            get => new OperationRepository(this);
        }
        public IOperationItemRepository OperationItemRepository
        {
            get => new OperationItemRepository(this);
        }
        public IPerformanceStockRepository PerformanceStockRepository
        {
            get => new PerformanceStockRepository(this);
        }
        public IPortfolioPerformanceRepository PortfolioPerformanceRepository
        {
            get => new PortfolioPerformanceRepository(this);
        }

        public IStockRepository StockRepository
        {
            get => new StockRepository(this);
        }

        public IEmailTemplateRepository EmailTemplateRepository
        {
            get => new EmailTemplateRepository(this);
        }

        public ISectorRepository SectorRepository
        {
            get => new SectorRepository(this);
        }

        public ISectorViewRepository SectorViewRepository
        {
            get => new SectorViewRepository(this);
        }

        public IStockAllocationRepository StockAllocationRepository
        {
            get => new StockAllocationRepository(this);
        }

        public ISegmentRepository SegmentRepository
        {
            get => new SegmentRepository(this);
        }

        public ISegmentViewRepository SegmentViewRepository
        {
            get => new SegmentViewRepository(this);
        }

        public ISubsectorRepository SubsectorRepository
        {
            get => new SubsectorRepository(this);
        }

        public ISubsectorViewRepository SubsectorViewRepository
        {
            get => new SubsectorViewRepository(this);
        }

        public ITraderRepository TraderRepository
        {
            get => new TraderRepository(this);
        }

        public IPortfolioRepository PortfolioRepository
        {
            get => new PortfolioRepository(this);
        }

        public ISubPortfolioRepository SubPortfolioRepository
        {
            get => new SubPortfolioRepository(this);
        }

        public ISubPortfolioOperationRepository SubPortfolioOperationRepository
        {
            get => new SubPortfolioOperationRepository(this);
        }

        public ISettingsRepository SettingsRepository
        {
            get => new SettingsRepository(this);
        }

        public IDeviceRepository DeviceRepository
        {
            get => new DeviceRepository(this);
        }

        public IOperationViewRepository OperationViewRepository
        {
            get => new OperationViewRepository(this);
        }

        public ICompanyViewRepository CompanyViewRepository
        {
            get => new CompanyViewRepository(this);
        }
        public IOperationHistRepository OperationHistRepository
        {
            get => new OperationHistRepository(this);
        }
        public IOperationItemHistRepository OperationItemHistRepository
        {
            get => new OperationItemHistRepository(this);
        }

        public ISyncQueueRepository SyncQueueRepository
        {
            get => new SyncQueueRepository(this);
        }

        public IDividendCalendarRepository DividendCalendarRepository
        {
            get => new DividendCalendarRepository(this);
        }

        public IDividendCalendarViewRepository DividendCalendarViewRepository
        {
            get => new DividendCalendarViewRepository(this);
        }
        
        public ISystemSettingsRepository SystemSettingsRepository
        {
            get => new SystemSettingsRepository(this);
        }

        public ISupportChannelRepository SupportChannelRepository
        {
            get => new SupportChannelRepository(this);
        }

        public IInvestmentsSpecialistRepository InvestmentsSpecialistRepository
        {
            get => new InvestmentsSpecialistRepository(this);
        }

        public ISuggestedPortfolioRepository SuggestedPortfolioRepository
        {
            get => new SuggestedPortfolioRepository(this);
        }

        public ISuggestedPortfolioItemRepository SuggestedPortfolioItemRepository
        {
            get => new SuggestedPortfolioItemRepository(this);
        }

        public ICompanyRepository CompanyRepository
        {
            get => new CompanyRepository(this);
        }

        public ILogoRepository LogoRepository
        {
            get => new LogoRepository(this);
        }
 		public ICryptoCurrencyRepository CryptoCurrencyRepository
        {
            get => new CryptoCurrencyRepository(this);
        }

        public IFinancialInstitutionRepository FinancialInstitutionRepository
        {
            get => new FinancialInstitutionRepository(this);
        }

        public IProductCategoryRepository ProductCategoryRepository
        {
            get => new ProductCategoryRepository(this);
        }

        public IProductRepository ProductRepository
        {
            get => new ProductRepository(this);
        }

        public IProductUserRepository ProductUserRepository
        {
            get => new ProductUserRepository(this);
        }

        public IProductUserViewRepository ProductUserViewRepository
        {
            get => new ProductUserViewRepository(this);
        }

        public ISubscriptionRepository SubscriptionRepository
        {
            get => new SubscriptionRepository(this);
        }

        public ISubscriptionTypeRepository SubscriptionTypeRepository
        {
            get => new SubscriptionTypeRepository(this);
        }

        public IFollowStockRepository FollowStockRepository
        {
            get => new FollowStockRepository(this);
        }

        public IAffiliationRepository AffiliationRepository
        {
            get => new AffiliationRepository(this);
        }


        public IMarketMoverRepository MarketMoverRepository
        {
            get => new MarketMoverRepository(this);
        }

        public IMarketMoverViewRepository MarketMoverViewRepository
        {
            get => new MarketMoverViewRepository(this);
        }

        public IHealthCheckRepository HealthCheckRepository
        {
            get => new HealthCheckRepository(this);
        }

        public INotificationHistoricalRepository NotificationHistoricalRepository
        {
            get => new NotificationHistoricalRepository(this);
        }

        public ICeiLogRepository CeiLogRepository
        {
            get => new CeiLogRepository(this);
        }

        public IFollowStockViewRepository FollowStockViewRepository
        {
            get => new FollowStockViewRepository(this);
        }

        public IFollowStockAlertRepository FollowStockAlertRepository
        {
            get => new FollowStockAlertRepository(this);
        }

        public IFollowStockAlertViewRepository FollowStockAlertViewRepository
        {
            get => new FollowStockAlertViewRepository(this);
        }

        public IExtraContentNotificationRepository ExtraContentNotificationRepository
        {
            get => new ExtraContentNotificationRepository(this);
        }

        public IDividendInfoViewRepository DividendInfoViewRepository
        {
            get => new DividendInfoViewRepository(this);
        }

        public IHolidayRepository HolidayRepository
        {
            get => new HolidayRepository(this);
        }
        
        public IAdvertiserRepository AdvertiserRepository
        {
            get => new AdvertiserRepository(this);
        }

        public IAdvertiserDetailsRepository AdvertiserDetailsRepository
        {
            get => new AdvertiserDetailsRepository(this);
        }

        public IMilkingCowsViewRepository MilkingCowsViewRepository
        {
            get => new MilkingCowsViewRepository(this);
        }

        public ICryptoCurrencyViewRepository CryptoCurrencyViewRepository
        {
            get => new CryptoCurrencyViewRepository(this);
        }

        public ITutorialRepository TutorialRepository
        {
            get => new TutorialRepository(this);
        }

        public IPartnerRepository PartnerRepository
        {
            get => new PartnerRepository(this);
        }

        public IPartnerRedeemRepository PartnerRedeemRepository
        {
            get => new PartnerRedeemRepository(this);
        }

        public IDividendCalendarWaitApprovalRepository DividendCalendarWaitApprovalRepository
        {
            get => new DividendCalendarWaitApprovalRepository(this);
        }

        public IAdvertiserExternalRepository AdvertiserExternalRepository
        {
            get => new AdvertiserExternalRepository(this);
        }

        public IAdvertiserExternalDetailRepository AdvertiserExternalDetailRepository
        {
            get => new AdvertiserExternalDetailRepository(this);
        }

        public IRelevantFactRepository RelevantFactRepository
        {
            get => new RelevantFactRepository(this);
        }

        public IScrapySchedulerRepository ScrapySchedulerRepository
        {
            get => new ScrapySchedulerRepository(this);
        }

        public IStockSplitRepository StockSplitRepository
        {
            get => new StockSplitRepository(this);
        }

        public IScrapyAgentRepository ScrapyAgentRepository
        {
            get => new ScrapyAgentRepository(this);
        }

        public IInitialOfferRepository InitialOfferRepository
        {
            get => new InitialOfferRepository(this);
        }

        public IGoalRepository GoalRepository
        {
            get => new GoalRepository(this);
        }

        public IGoalViewRepository GoalViewRepository
        {
            get => new GoalViewRepository(this);
        }

        public ISuggestedPortfolioItemViewRepository SuggestedPortfolioItemViewRepository
        {
            get => new SuggestedPortfolioItemViewRepository(this);
        }
        public IRelevantFactViewRepository RelevantFactViewRepository
        {
            get => new RelevantFactViewRepository(this);
        }
        
        public ITaggedUserRepository TaggedUserRepository
        {
            get => new TaggedUserRepository(this);
        }

        public IStockViewRepository StockViewRepository
        {
            get => new StockViewRepository(this);
        }

        public ICompanyIndicatorsRepository CompanyIndicatorsRepository
        {
            get => new CompanyIndicatorsRepository(this);
        }

        public ICompanyIndicatorsViewRepository CompanyIndicatorsViewRepository
        {
            get => new CompanyIndicatorsViewRepository(this);
        }

        public IInsightViewRepository InsightViewRepository
        {
            get => new InsightViewRepository(this);
        }
        public ICryptoPortfolioPerformanceRepository CryptoPortfolioPerformanceRepository
        {
            get => new CryptoPortfolioPerformanceRepository(this);
        }
        public ICryptoCurrencyPerformanceRepository CryptoCurrencyPerformanceRepository
        {
            get => new CryptoCurrencyPerformanceRepository(this);
        }
        public ICryptoPortfolioRepository CryptoPortfolioRepository
        {
            get => new CryptoPortfolioRepository(this);
        }
        public ICryptoTransactionItemRepository CryptoTransactionItemRepository
        {
            get => new CryptoTransactionItemRepository(this);
        }
        public ICryptoTransactionRepository CryptoTransactionRepository
        {
            get => new CryptoTransactionRepository(this);
        }
        public IFiatCurrencyRepository FiatCurrencyRepository
        {
            get => new FiatCurrencyRepository(this);
        }
        public ICryptoSubPortfolioRepository CryptoSubPortfolioRepository
        {
            get => new CryptoSubPortfolioRepository(this);
        }
        public ICryptoSubPortfolioTransactionRepository CryptoSubPortfolioTransactionRepository
        {
            get => new CryptoSubPortfolioTransactionRepository(this);
        }

        public ICryptoPortfolioViewRepository CryptoPortfolioViewRepository
        {
            get => new CryptoPortfolioViewRepository(this);
        }

        public ITraderViewRepository TraderViewRepository
        {
            get => new TraderViewRepository(this);
        }

        public ICryptoCurrencyStatementViewRepository CryptoCurrencyStatementViewRepository
        {
            get => new CryptoCurrencyStatementViewRepository(this);
        }

        public ICryptoTransactionViewRepository CryptoTransactionViewRepository
        {
            get => new CryptoTransactionViewRepository(this);
        }

        public IContactPhoneRepository ContactPhoneRepository
        {
            get => new ContactPhoneRepository(this);
        }

        public IContactDetailsRepository ContactDetailsRepository
        {
            get => new ContactDetailsRepository(this);
        }

        public IInfluencerViewRepository InfluencerViewRepository
        {
            get => new InfluencerViewRepository(this);
        }

        public IVideoTutorialRepository VideoTutorialRepository
        {
            get => new VideoTutorialRepository(this);
        }

        public IResearchProductRepository ResearchProductRepository
        {
            get => new ResearchProductRepository(this);
        }
    }
}
