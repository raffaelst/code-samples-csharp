using K.UnitOfWorkBase;
using System;
using System.Data;

namespace Dividendos.Repository.Interface.UoW
{
    public interface IUnitOfWork : IUnitOfWorkBase
    {
        IUserRepository UserRepository { get; }
        IDividendRepository DividendRepository { get; }
        IDividendViewRepository DividendViewRepository { get; }
        IPortfolioViewRepository PortfolioViewRepository { get; }
        IPortfolioStatementViewRepository PortfolioStatementViewRepository { get; }
        IStockStatementViewRepository StockStatementViewRepository { get; }
        IIndicatorSeriesRepository IndicatorSeriesRepository { get; }
        IIndicatorSeriesViewRepository IndicatorSeriesViewRepository { get; }
        IStockTypeRepository StockTypeRepository { get; }
        IDividendTypeRepository DividendTypeRepository { get; }
        IOperationRepository OperationRepository { get; }
        IOperationItemRepository OperationItemRepository { get; }
        IPerformanceStockRepository PerformanceStockRepository { get; }
        IPortfolioPerformanceRepository PortfolioPerformanceRepository { get; }
        IStockRepository StockRepository { get; }
        IEmailTemplateRepository EmailTemplateRepository { get; }
        ISegmentRepository SegmentRepository { get; }
        ISegmentViewRepository SegmentViewRepository { get; }
        ISectorRepository SectorRepository { get; }
        ISectorViewRepository SectorViewRepository { get; }
        IStockAllocationRepository StockAllocationRepository { get; }
        ISubsectorRepository SubsectorRepository { get; }
        ISubsectorViewRepository SubsectorViewRepository { get; }
        ITraderRepository TraderRepository { get; }
        IPortfolioRepository PortfolioRepository { get; }
        ISubPortfolioRepository SubPortfolioRepository { get; }
        ISubPortfolioOperationRepository SubPortfolioOperationRepository { get; }
        ISettingsRepository SettingsRepository { get; }
        IDeviceRepository DeviceRepository { get; }
        IOperationViewRepository OperationViewRepository { get; }
        ICompanyViewRepository CompanyViewRepository { get; }
        IOperationHistRepository OperationHistRepository { get; }
        IOperationItemHistRepository OperationItemHistRepository { get; }
        ISyncQueueRepository SyncQueueRepository { get; }
        IDividendCalendarRepository DividendCalendarRepository { get; }

        IDividendCalendarViewRepository DividendCalendarViewRepository { get; }

        ISystemSettingsRepository SystemSettingsRepository { get; }

        ISupportChannelRepository SupportChannelRepository { get; }

        IInvestmentsSpecialistRepository InvestmentsSpecialistRepository { get; }

        ISuggestedPortfolioRepository SuggestedPortfolioRepository { get; }

        ISuggestedPortfolioItemRepository SuggestedPortfolioItemRepository { get; }

        ICryptoCurrencyRepository CryptoCurrencyRepository { get; }

        IFinancialInstitutionRepository FinancialInstitutionRepository { get; }
        IProductCategoryRepository ProductCategoryRepository { get; }
        ILogoRepository LogoRepository { get; }
        IProductUserRepository ProductUserRepository { get; }
        IProductRepository ProductRepository { get; }
        IProductUserViewRepository ProductUserViewRepository { get; }

        ICompanyRepository CompanyRepository { get; }

        ISubscriptionTypeRepository SubscriptionTypeRepository { get; }

        ISubscriptionRepository SubscriptionRepository { get; }

        IFollowStockRepository FollowStockRepository { get; }


        IAffiliationRepository AffiliationRepository { get; }


        IMarketMoverRepository MarketMoverRepository { get; }

        IMarketMoverViewRepository MarketMoverViewRepository { get; }

        IHealthCheckRepository HealthCheckRepository { get; }

        INotificationHistoricalRepository NotificationHistoricalRepository { get; }
        ICeiLogRepository CeiLogRepository { get; }

        IFollowStockViewRepository FollowStockViewRepository { get; }

        IFollowStockAlertRepository FollowStockAlertRepository { get; }
        IFollowStockAlertViewRepository FollowStockAlertViewRepository { get; }

        IExtraContentNotificationRepository ExtraContentNotificationRepository { get; }

        IDividendInfoViewRepository DividendInfoViewRepository { get; }
        IHolidayRepository HolidayRepository { get; }

        IAdvertiserDetailsRepository AdvertiserDetailsRepository { get; }
        IAdvertiserRepository AdvertiserRepository { get; }
        IMilkingCowsViewRepository MilkingCowsViewRepository { get; }

        ICryptoCurrencyViewRepository CryptoCurrencyViewRepository { get; }

        ITutorialRepository TutorialRepository { get; }

        IPartnerRepository PartnerRepository { get; }

        IPartnerRedeemRepository PartnerRedeemRepository { get; }

        IDividendCalendarWaitApprovalRepository DividendCalendarWaitApprovalRepository { get; }

        IAdvertiserExternalDetailRepository AdvertiserExternalDetailRepository { get; }

        IAdvertiserExternalRepository AdvertiserExternalRepository { get; }

        IRelevantFactRepository RelevantFactRepository { get; }

        IScrapySchedulerRepository ScrapySchedulerRepository { get; }

        IStockSplitRepository StockSplitRepository { get; }

        IScrapyAgentRepository ScrapyAgentRepository { get; }

        IInitialOfferRepository InitialOfferRepository { get; }

        IGoalRepository GoalRepository { get; }

        IGoalViewRepository GoalViewRepository { get; }

        ISuggestedPortfolioItemViewRepository SuggestedPortfolioItemViewRepository { get; }
        IRelevantFactViewRepository RelevantFactViewRepository {get;}

        ITaggedUserRepository TaggedUserRepository {get;}

        IStockViewRepository StockViewRepository { get; }
        ICompanyIndicatorsRepository CompanyIndicatorsRepository { get; }
        ICompanyIndicatorsViewRepository CompanyIndicatorsViewRepository { get; }
        IInsightViewRepository InsightViewRepository { get; }
        ICryptoPortfolioPerformanceRepository CryptoPortfolioPerformanceRepository { get; }
        ICryptoCurrencyPerformanceRepository CryptoCurrencyPerformanceRepository { get; }
        ICryptoPortfolioRepository CryptoPortfolioRepository { get; }
        ICryptoTransactionItemRepository CryptoTransactionItemRepository { get; }
        ICryptoTransactionRepository CryptoTransactionRepository { get; }
        IFiatCurrencyRepository FiatCurrencyRepository { get; }
        ICryptoSubPortfolioTransactionRepository CryptoSubPortfolioTransactionRepository { get; }
        ICryptoPortfolioViewRepository CryptoPortfolioViewRepository { get; }
        ITraderViewRepository TraderViewRepository { get; }
        ICryptoCurrencyStatementViewRepository CryptoCurrencyStatementViewRepository { get; }
        ICryptoSubPortfolioRepository CryptoSubPortfolioRepository { get; }
        ICryptoTransactionViewRepository CryptoTransactionViewRepository { get; }
        IContactPhoneRepository ContactPhoneRepository { get; }
        IContactDetailsRepository ContactDetailsRepository { get; }

        IInfluencerViewRepository InfluencerViewRepository { get; }

        IVideoTutorialRepository VideoTutorialRepository { get; }
        IResearchProductRepository ResearchProductRepository { get; }
    }
}
