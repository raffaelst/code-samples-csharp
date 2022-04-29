using AutoMapper;

using Dividendos.API.Model.Request;
using Dividendos.API.Model.Request.Device;
using Dividendos.API.Model.Request.FollowStock;
using Dividendos.API.Model.Request.FreeRecommendations;
using Dividendos.API.Model.Request.InvestmentAdvisor;
using Dividendos.API.Model.Request.Notification;
using Dividendos.API.Model.Request.Settings;
using Dividendos.API.Model.Request.Stock;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using Dividendos.API.Model.Response.Company;
using Dividendos.API.Model.Response.Crypto;
using Dividendos.API.Model.Response.FinancialProducts;
using Dividendos.API.Model.Response.Influencer;
using Dividendos.API.Model.Response.Insight;
using Dividendos.API.Model.Response.MilkingCows;
using Dividendos.API.Model.Response.Notification;
using Dividendos.API.Model.Response.NotificationHistorical;
using Dividendos.API.Model.Response.Purchase;
using Dividendos.API.Model.Response.StockSplit;
using Dividendos.API.Model.Response.v1;
using Dividendos.API.Model.Response.v1.InvestmentAdvisor;
using Dividendos.API.Model.Response.v1.ResearchProduct;
using Dividendos.Application.Interface.Model;
using Dividendos.B3.Interface.Model;
using Dividendos.CrossCutting.Identity.Models;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Enum;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using Dividendos.Passfolio.Interface.Model;
using Dividendos.TradeMap.Interface.Model;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Dividendos.CrossCutting.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ResultServiceBase, ResultResponseBase>();
            CreateMap<ResultServiceString, ResultResponseStringModel>();

            CreateMap<UserRegisterVM, ApplicationUser>().ForMember(
                dest => dest.PasswordHash,
                opt => opt.MapFrom(src => src.Password)
            )
            .ForMember(
                dest => dest.NormalizedEmail,
                opt => opt.MapFrom(src => src.Email.ToLower())
            )
            .ForMember(
                dest => dest.UserName,
                opt => opt.MapFrom(src => src.Email.ToLower())
            )
            .ForMember(
                dest => dest.NormalizedUserName,
                opt => opt.MapFrom(src => src.Email.ToLower())
            )
            .ForMember(
                dest => dest.PhoneNumber,
                opt => opt.MapFrom(src => src.PhoneNumber)
            );

            CreateMap<ResultServiceObject<Trader>, ResultResponseObject<TraderVM>>();

            CreateMap<ResultServiceObject<IEnumerable<Stock>>, ResultResponseObject<IEnumerable<StockVM>>>();

            CreateMap<ResultServiceObject<IEnumerable<Stock>>, ResultResponseObject<IEnumerable<StockSymbolVM>>>();
            CreateMap<ResultServiceObject<IEnumerable<StockView>>, ResultResponseObject<IEnumerable<StockSymbolVM>>>();
            CreateMap<ResultServiceObject<IEnumerable<Stock>>, ResultResponseObject<IEnumerable<Dividendos.API.Model.Response.v3.StockSymbolVM>>>();
            CreateMap<ResultServiceObject<IEnumerable<StockView>>, ResultResponseObject<IEnumerable<Dividendos.API.Model.Response.v3.StockSymbolVM>>>();
            CreateMap<ResultServiceObject<IEnumerable<CryptoStatementView>>, ResultResponseObject<IEnumerable<StockVM>>>();
            CreateMap<ResultServiceObject<IEnumerable<StockView>>, ResultResponseObject<IEnumerable<StockVM>>>();

            CreateMap<StockView, StockVM>().ForMember(
               dest => dest.Logo,
               opt => opt.MapFrom(src => src.Logo)
           )
           .ForMember(
               dest => dest.GuidStock,
               opt => opt.MapFrom(src => src.GuidStock)
           )
           .ForMember(
               dest => dest.Symbol,
               opt => opt.MapFrom(src => src.Symbol)
           )
           .ForMember(
               dest => dest.Name,
               opt => opt.MapFrom(src => src.CompanyName)
           );

            CreateMap<CryptoStatementView, StockVM>().ForMember(
                dest => dest.Logo,
                opt => opt.MapFrom(src => src.Logo)
            )
            .ForMember(
                dest => dest.GuidStock,
                opt => opt.MapFrom(src => src.CryptoCurrencyGuid)
            )
            .ForMember(
                dest => dest.Symbol,
                opt => opt.MapFrom(src => src.Symbol)
            )
            .ForMember(
                dest => dest.IsCrypto,
                opt => opt.MapFrom(src => true)
            )
            .ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(src => src.Description)
            );

            CreateMap<SubPortfolio, PortfolioModel>().ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(src => src.Name)
            )
            .ForMember(
                dest => dest.GuidPortfolioSub,
                opt => opt.MapFrom(src => src.GuidSubPortfolio)
            );

            CreateMap<ResultServiceObject<IEnumerable<SubPortfolio>>, ResultResponseObject<IEnumerable<PortfolioModel>>>();

            CreateMap<ResultServiceObject<ApplicationUser>, ResultResponseObject<UserVM>>();
            CreateMap<ResultServiceObject<IEnumerable<ApplicationUser>>, ResultResponseObject<IEnumerable<UserVM>>>();
            CreateMap<ResultServiceObject<IEnumerable<ApplicationUser>>, ResultResponseObject<IEnumerable<UserDataVM>>>();
            CreateMap<ApplicationUser, API.Model.Response.v3.UserVM>().ForMember(dest => dest.NotificationAmount, act => act.Ignore());

            CreateMap<ResultServiceObject<ApplicationUser>, ResultResponseObject<Dividendos.API.Model.Response.v3.UserVM>>();
            CreateMap<ApplicationUser, UserDataVM>();

            CreateMap<ResultServiceObject<IEnumerable<SubPortfolio>>, ResultResponseObject<IEnumerable<PortfolioVM>>>();

            CreateMap<ResultServiceObject<IEnumerable<SubPortfolio>>, ResultResponseObject<IEnumerable<SubPortfolioVM>>>();

            CreateMap<ResultServiceObject<SubPortfolio>, ResultResponseObject<SubPortfolioVM>>();

            CreateMap<ResultServiceObject<IEnumerable<SubPortfolio>>, ResultResponseObject<IEnumerable<SubPortfolioVM>>>();

            CreateMap<ResultServiceObject<Portfolio>, ResultResponseObject<PortfolioVM>>();

            CreateMap<ResultServiceBase, ResultResponseBase>();

            CreateMap<SubPortfolio, SubPortfolioVM>();

            CreateMap<DeviceVM, Device>();

            CreateMap<ProductUserView, FinancialProductDetailVM>()
            .ForMember(
                dest => dest.CurrentValue,
                opt => opt.MapFrom(src => src.CurrentValue.ToString("n2", new CultureInfo("pt-br")))
            );


            //CreateMap<ProductUser, FinancialProductResponse>();
            CreateMap<ResultServiceObject<ProductUser>, ResultResponseObject<FinancialProductResponse>>();

            CreateMap<ResultServiceObject<ProductUser>, ResultResponseObject<FinancialProductAddVM>>();
            CreateMap<ProductUser, FinancialProductAddVM>();

            CreateMap<MarketMoverView, MarketMoverVM>()
            .ForMember(
                dest => dest.MarketPrice,
                opt => opt.MapFrom(src => src.MarketPrice.ToString("n2", new CultureInfo("pt-br")))
            )
            .ForMember(
                dest => dest.Value,
                opt => opt.MapFrom(src => src.Value.ToString("n2", new CultureInfo("pt-br")))
            );

            CreateMap<SubPortfolioVM, SubPortfolio>();
            CreateMap<ResultServiceObject<IEnumerable<SubPortfolio>>, ResultResponseObject<IEnumerable<SubPortfolioVM>>>().ReverseMap();
            CreateMap<ResultServiceObject<IEnumerable<OperationView>>, ResultResponseObject<IEnumerable<OperationBasicVM>>>();
            CreateMap<ResultServiceObject<IEnumerable<Trader>>, ResultResponseObject<IEnumerable<TraderVM>>>().ReverseMap();
            CreateMap<Trader, TraderVM>();
            CreateMap<ResultServiceObject<IEnumerable<Company>>, ResultResponseObject<IEnumerable<CompanyVM>>>().ReverseMap();
            CreateMap<Company, CompanyVM>();
            CreateMap<ResultServiceObject<Settings>, ResultResponseObject<SettingsEditVM>>();
            CreateMap<ResultServiceObject<IEnumerable<DividendCalendarView>>, ResultResponseObject<IEnumerable<DividendCalendarVM>>>();
            CreateMap<ResultServiceObject<IEnumerable<SupportChannel>>, ResultResponseObject<IEnumerable<SupportChannelVM>>>();

            CreateMap<ResultServiceObject<IEnumerable<InvestmentsSpecialist>>, ResultResponseObject<IEnumerable<InvestmentsSpecialistVM>>>();
            CreateMap<ResultServiceObject<IEnumerable<SuggestedPortfolio>>, ResultResponseObject<IEnumerable<SuggestedPortfolioVM>>>();
            CreateMap<ResultServiceObject<IEnumerable<SuggestedPortfolioItemView>>, ResultResponseObject<IEnumerable<SuggestedPortfolioItemVM>>>();

            CreateMap<SuggestedPortfolioItemView, SuggestedPortfolioItemVM>()
                .ForMember(
               dest => dest.Price,
               opt => opt.MapFrom(src => src.MarketPrice.ToString("n2", new CultureInfo("pt-br")))
            );

            CreateMap<ResultServiceObject<IEnumerable<ProductUserView>>, ResultResponseObject<IEnumerable<FinancialProductDetailVM>>>();

            CreateMap<ResultServiceObject<IEnumerable<IndicatorSeries>>, ResultResponseObject<IEnumerable<IndicatorVM>>>();
            CreateMap<ResultServiceObject<IEnumerable<IndicatorSeriesView>>, ResultResponseObject<IEnumerable<IndicatorVM>>>();
            CreateMap<ResultServiceObject<IEnumerable<AffiliateProduct>>, ResultResponseObject<IEnumerable<AffiliateProductDetailVM>>>();

            CreateMap<ResultServiceObject<IEnumerable<MarketMoverView>>, ResultResponseObject<IEnumerable<MarketMoverVM>>>();

            CreateMap<ResultServiceObject<IEnumerable<ProductCategory>>, ResultResponseObject<IEnumerable<FinancialProductCategoryVM>>>();
            CreateMap<ResultServiceObject<IEnumerable<Product>>, ResultResponseObject<IEnumerable<FinancialProductVM>>>();
            CreateMap<ResultServiceObject<IEnumerable<FinancialInstitution>>, ResultResponseObject<IEnumerable<FinancialInstitutionVM>>>();
            CreateMap<ResultServiceObject<NotificationHistorical>, ResultResponseObject<NotificationHistoricalVM>>();
            CreateMap<ResultServiceObject<IEnumerable<NotificationHistorical>>, ResultResponseObject<IEnumerable<NotificationHistoricalVM>>>();
            CreateMap<ResultServiceObject<FollowStock>, ResultResponseObject<FollowStockVM>>();
            CreateMap<ResultServiceObject<FollowStockAlert>, ResultResponseObject<FollowStockAlertResponseVM>>();
            CreateMap<FollowStockAlert, FollowStockAlertResponseVM>().ForMember(
               dest => dest.FollowStockType,
               opt => opt.MapFrom(src => (FollowStockType)src.FollowStockTypeID)
           )
                .ForMember(
               dest => dest.TargetPrice,
               opt => opt.MapFrom(src => src.TargetPrice.ToString("n2", new CultureInfo("pt-br")))
           );

            CreateMap<ResultServiceObject<FollowStockAlertView>, ResultResponseObject<FollowStockAlertResponseVM>>();
            CreateMap<FollowStockAlertView, FollowStockAlertResponseVM>().ForMember(
               dest => dest.FollowStockType,
               opt => opt.MapFrom(src => (FollowStockType)src.FollowStockTypeID)
           )
                .ForMember(
               dest => dest.TargetPrice,
               opt => opt.MapFrom(src => (src.IsStockType ? src.TargetPrice.ToString("n2", new CultureInfo("pt-br")) : src.TargetPrice.ToString("N8", new CultureInfo("pt-br"))))
           );

            CreateMap<FollowStockVM, FollowStock>().ReverseMap();
            CreateMap<FollowStockAlertVM, FollowStockAlert>();
            CreateMap<ResultServiceObject<IEnumerable<FollowStockAlert>>, ResultResponseObject<IEnumerable<FollowStockAlertResponseVM>>>();
            CreateMap<ResultServiceObject<IEnumerable<FollowStockAlertView>>, ResultResponseObject<IEnumerable<FollowStockAlertResponseVM>>>();
            CreateMap<FollowStockView, FollowStockResponseVM>()
           .ForMember(
               dest => dest.MarketPrice,
               opt => opt.MapFrom(src => (src.IsStockType ? src.MarketPrice.ToString("n2", new CultureInfo("pt-br")) : src.MarketPrice.ToString("N8", new CultureInfo("pt-br"))))
           ).ForMember(
               dest => dest.Symbol,
               opt => opt.MapFrom(src => src.Symbol.ToUpper())
           ).ForMember(
               dest => dest.CountryType,
               opt => opt.MapFrom(src => (CountryType)src.IdCountry)
           ).ForMember(
               dest => dest.PerformancePerc,
               opt => opt.MapFrom(src => (src.IsStockType ? src.LastChangePerc * 100 : src.LastChangePerc).ToString("n2", new CultureInfo("pt-br")))
           );

            CreateMap<ResultServiceObject<FollowStockView>, ResultResponseObject<FollowStockResponseVM>>();
            CreateMap<ResultServiceObject<IEnumerable<FollowStockView>>, ResultResponseObject<IEnumerable<FollowStockResponseVM>>>();


            CreateMap<PushContentVM, ExtraContentNotification>().ForMember(
               dest => dest.ExtraContentNotificationTypeID,
               opt => opt.MapFrom(src => (PushContentType)src.PushContentType)
           ).ForMember(
               dest => dest.PushRedirectType,
               opt => opt.MapFrom(src => src.PushRedirectType.ToString())
           ).ForMember(
               dest => dest.AppScreenName,
               opt => opt.MapFrom(src => src.AppScreenNameType.ToString())
           );

            CreateMap<ResultServiceObject<ExtraContentNotification>, ResultResponseObject<PushContentResponseVM>>();
            CreateMap<ResultServiceObject<IEnumerable<DividendView>>, ResultResponseObject<IEnumerable<API.Model.Response.v2.NextDividendVM>>>();
            CreateMap<ResultServiceObject<IEnumerable<DividendView>>, ResultResponseObject<IEnumerable<NextDividendVM>>>();
            CreateMap<ResultServiceObject<DividendView>, ResultResponseObject<API.Model.Response.v2.NextDividendVM>>();
            CreateMap<ResultServiceObject<DividendView>, ResultResponseObject<NextDividendVM>>();
            CreateMap<DividendView, API.Model.Response.v2.NextDividendVM>()
              .ForMember(
                  dest => dest.NetValue,
                  opt => opt.MapFrom(src => src.NetValue.ToString("n2", new CultureInfo("pt-br")))
              ).ForMember(
                  dest => dest.GrossValue,
                  opt => opt.MapFrom(src => src.GrossValue.ToString("n2", new CultureInfo("pt-br")))
              )
              .ForMember(
                  dest => dest.PaymentDate,
                  opt => opt.MapFrom(src => (src.PaymentDate.HasValue ? src.PaymentDate.Value.ToString("dd/MM") : "Data Indef"))
              );


            CreateMap<DividendView, NextDividendVM>()
              .ForMember(
                  dest => dest.NetValue,
                  opt => opt.MapFrom(src => src.NetValue.ToString("n2", new CultureInfo("pt-br")))
              ).ForMember(
                  dest => dest.GrossValue,
                  opt => opt.MapFrom(src => src.GrossValue.ToString("n2", new CultureInfo("pt-br")))
              )
              .ForMember(
                  dest => dest.PaymentDate,
                  opt => opt.MapFrom(src => (src.PaymentDate.HasValue ? src.PaymentDate.Value.ToString("dd/MM") : "Data Indef"))
              );

            CreateMap<API.Model.Response.v2.PortfolioViewWrapperVM, API.Model.Response.v3.PortfolioViewWrapperVM>();
            CreateMap<API.Model.Response.v2.PortfolioViewVM, API.Model.Response.v4.PortfolioViewVM>();

            CreateMap<DividendCalendarView, DividendCalendarVM>()
              .ForMember(
                  dest => dest.DataCom,
                  opt => opt.MapFrom(src => (src.DataCom.ToString("dd/MM/yy")))
                  ).ForMember(
                  dest => dest.PaymentDate,
                  opt => opt.MapFrom(src => (src.PaymentDate.HasValue ? src.PaymentDate.Value.ToString("dd/MM/yy") : " - "))
                  ).ForMember(
                  dest => dest.Value,
                  opt => opt.MapFrom(src => src.Value.ToString("N4", new CultureInfo("pt-br")))
                  ).ForMember(
                  dest => dest.Yield,
                  opt => opt.MapFrom(src => src.Yield.ToString("N2", new CultureInfo("pt-br"))));

            CreateMap<ResultServiceObject<Advertiser>, ResultResponseObject<AdvertiserVM>>();
            CreateMap<ResultServiceObject<IEnumerable<Advertiser>>, ResultResponseObject<IEnumerable<AdvertiserVM>>>();
            CreateMap<ResultServiceObject<AdvertiserDetails>, ResultResponseObject<AdvertiserDetailsVM>>();
            CreateMap<ResultServiceObject<Portfolio>, ResultResponseObject<API.Model.Response.v4.PortfolioVM>>();
            CreateMap<Portfolio, API.Model.Response.v4.PortfolioVM>();
            CreateMap<ResultServiceObject<Portfolio>, ResultResponseObject<API.Model.Response.v5.PortfolioVM>>();
            CreateMap<Portfolio, API.Model.Response.v5.PortfolioVM>();
            CreateMap<ResultResponseObject<IEnumerable<API.Model.Response.v4.TraderSummaryVM>>, ResultResponseObject<IEnumerable<Dividendos.API.Model.Response.v5.TraderSummaryVM>>>();

            CreateMap<ResultServiceObject<Portfolio>, ResultResponseObject<API.Model.Response.v6.PortfolioVM>>();
            CreateMap<Portfolio, API.Model.Response.v6.PortfolioVM>();

            CreateMap<ResultServiceObject<Portfolio>, ResultResponseObject<API.Model.Response.v7.PortfolioVM>>();
            CreateMap<Portfolio, API.Model.Response.v7.PortfolioVM>();

            CreateMap<ResultServiceObject<IEnumerable<MilkingCowsView>>, ResultResponseObject<IEnumerable<MilkingCowsVM>>>();
            CreateMap<Dividendos.Passfolio.Interface.Model.Ticker, Dividendos.MercadoBitcoin.Interface.Model.Ticker>();

            CreateMap<Dividendos.Binance.Interface.Model.Ticker, Dividendos.MercadoBitcoin.Interface.Model.Ticker>();

            CreateMap<Dividendos.MercadoBitcoin.Interface.Model.Ticker, Dividendos.Passfolio.Interface.Model.Ticker>();


            CreateMap<ResultServiceObject<IEnumerable<CryptoBrokerView>>, ResultResponseObject<IEnumerable<CryptoBrokerVM>>>();
            CreateMap<ResultServiceObject<CryptoBrokerView>, ResultResponseObject<CryptoBrokerVM>>();
            CreateMap<ResultServiceObject<IEnumerable<CryptoStatementView>>, ResultResponseObject<IEnumerable<CryptoInfoVM>>>();
            CreateMap<CryptoStatementView, CryptoInfoVM>().ForMember(
                  dest => dest.Description,
                  opt => opt.MapFrom(src => src.Description)).ForMember(
                  dest => dest.Symbol,
                  opt => opt.MapFrom(src => src.Symbol.ToUpper()));

            CreateMap<ResultServiceObject<IEnumerable<CryptoStatementView>>, ResultResponseObject<IEnumerable<CryptoInfoAutoCompleteVM>>>();
            CreateMap<ResultServiceObject<IEnumerable<Tutorial>>, ResultResponseObject<IEnumerable<TutorialVM>>>();
            CreateMap<ResultServiceObject<IEnumerable<VideoTutorial>>, ResultResponseObject<IEnumerable<VideoTutorialVM>>>();
            CreateMap<VideoTutorial, VideoTutorialVM>();

            CreateMap<ResultServiceObject<Partner>, ResultResponseObject<PartnerVM>>();
            CreateMap<ResultServiceObject<IEnumerable<Partner>>, ResultResponseObject<IEnumerable<PartnerVM>>>();
            CreateMap<ResultServiceObject<PartnerRedeem>, ResultResponseObject<ReedemVM>>();


            CreateMap<ReedemVM, PartnerRedeem>().ReverseMap();


            CreateMap<DividendCalendar, DividendCalendarWaitApproval>().ReverseMap();

            CreateMap<ResultServiceObject<IEnumerable<AdvertiserExternal>>, ResultResponseObject<IEnumerable<AdvertiserExternalVM>>>();
            CreateMap<ResultServiceObject<IEnumerable<AdvertiserExternalDetail>>, ResultResponseObject<IEnumerable<AdvertiserExternalDetailVM>>>();
            CreateMap<ResultServiceObject<AdvertiserExternalDetail>, ResultResponseObject<AdvertiserExternalDetailVM>>();

            CreateMap<ResultServiceObject<IEnumerable<RelevantFact>>, ResultResponseObject<IEnumerable<RelevantFactDetailVM>>>();

            CreateMap<ResultServiceObject<IEnumerable<RelevantFact>>, ResultResponseObject<IEnumerable<RelevantFactVM>>>();

            CreateMap<ResultServiceObject<IEnumerable<RelevantFactView>>, ResultResponseObject<IEnumerable<RelevantFactVM>>>();
            CreateMap<RelevantFactView, RelevantFactVM>().ForMember(
              dest => dest.ReferenceDate,
              opt => opt.MapFrom(src => src.ReferenceDate.ToString("dd/MM/yy")));
            CreateMap<RelevantFact, RelevantFactVM>().ForMember(
              dest => dest.ReferenceDate,
              opt => opt.MapFrom(src => src.ReferenceDate.ToString("dd/MM/yy")));
            CreateMap<ResultServiceObject<RelevantFactView>, ResultResponseObject<RelevantFactVM>>();
            CreateMap<ResultServiceObject<RelevantFact>, ResultResponseObject<RelevantFactVM>>();
            CreateMap<RelevantFactAdd, RelevantFact>();
            CreateMap<ResultServiceObject<RelevantFact>, ResultResponseObject<RelevantFactDetailVM>>();
            CreateMap<ResultServiceObject<IEnumerable<CryptoStatementView>>, ResultResponseObject<IEnumerable<CryptoMarketMoverVM>>>();

            CreateMap<CryptoStatementView, CryptoMarketMoverVM>().ForMember(
                  dest => dest.Symbol,
                  opt => opt.MapFrom(src => src.Symbol.ToUpper())).ForMember(
                  dest => dest.MarketPrice,
                  opt => opt.MapFrom(src => src.MarketPrice.ToString("N8", new CultureInfo("pt-br")))).ForMember(
                  dest => dest.PercentChange24h,
                  opt => opt.MapFrom(src => string.Concat(GetSignal(src.PercentChange24h), src.PercentChange24h.ToString("N2", new CultureInfo("pt-br"))))).ForMember(
                  dest => dest.PercentChange1h,
                  opt => opt.MapFrom(src => string.Concat(GetSignal(src.PercentChange1h), src.PercentChange1h.ToString("N2", new CultureInfo("pt-br")))))
                  .ForMember(
                  dest => dest.PercentChange7d,
                  opt => opt.MapFrom(src => string.Concat(GetSignal(src.PercentChange7d), src.PercentChange7d.ToString("N2", new CultureInfo("pt-br")))));

            CreateMap<ResultServiceObject<IEnumerable<CryptoStatementView>>, ResultResponseObject<IEnumerable<CryptoMarketMoverVM>>>();

            CreateMap<ResultServiceObject<IEnumerable<CryptoStatementView>>, ResultResponseObject<IEnumerable<API.Model.Response.v2.CryptoMarketMoverVM>>>();
            CreateMap<CryptoStatementView, API.Model.Response.v2.CryptoMarketMoverVM>().ForMember(
                  dest => dest.Symbol,
                  opt => opt.MapFrom(src => src.Symbol.ToUpper())).ForMember(
                  dest => dest.LogoUrl,
                  opt => opt.MapFrom(src => src.LogoUrl)).ForMember(
                  dest => dest.MarketPrice,
                  opt => opt.MapFrom(src => src.MarketPrice.ToString("N8", new CultureInfo("pt-br")))).ForMember(
                  dest => dest.PercentChange24h,
                  opt => opt.MapFrom(src => string.Concat(GetSignal(src.PercentChange24h), src.PercentChange24h.ToString("N2", new CultureInfo("pt-br"))))).ForMember(
                  dest => dest.PercentChange1h,
                  opt => opt.MapFrom(src => string.Concat(GetSignal(src.PercentChange1h), src.PercentChange1h.ToString("N2", new CultureInfo("pt-br")))))
                  .ForMember(
                  dest => dest.PercentChange7d,
                  opt => opt.MapFrom(src => string.Concat(GetSignal(src.PercentChange7d), src.PercentChange7d.ToString("N2", new CultureInfo("pt-br")))));

            CreateMap<StockPassfolioOperation, StockOperation>();
            CreateMap<StockPassfolioOperation, StockOperationView>();
            CreateMap<ResultServiceObject<IEnumerable<StockSplit>>, ResultResponseObject<IEnumerable<StockSplitVM>>>();


            CreateMap<StockSplit, StockSplitVM>().ForMember(
              dest => dest.DateSplit,
              opt => opt.MapFrom(src => src.DateSplit.ToString("dd/MM/yy")))
                .ForMember(
              dest => dest.ProportionFrom,
              opt => opt.MapFrom(src => src.ProportionFrom.ToString("g29", new CultureInfo("pt-br"))))
                .ForMember(
              dest => dest.ProportionTo,
              opt => opt.MapFrom(src => src.ProportionTo.ToString("g29", new CultureInfo("pt-br"))));


            CreateMap<DividendCalendarView, DividendNextDataComVM>();

            CreateMap<InitialOffer, InitialOfferVM>();

            CreateMap<ResultServiceObject<Goal>, ResultResponseObject<GoalVM>>();

            CreateMap<Goal, GoalVM>();

            CreateMap<IEnumerable<InsightView>, IEnumerable<InsightsResponse>>();
            CreateMap<InsightView, InsightsResponse>();
            

            CreateMap<ResultServiceObject<TaggedUser>, ResultResponseObject<TaggedUserVM>>();
            CreateMap<TaggedUser, TaggedUserVM>();
            CreateMap<TaggedUserVM, TaggedUser>();

            CreateMap<ResultServiceObject<IEnumerable<GoalView>>, ResultResponseObject<IEnumerable<GoalVM>>>();
            CreateMap<ResultServiceObject<GoalView>, ResultResponseObject<GoalVM>>();
            CreateMap<GoalView, GoalVM>().ForMember(
              dest => dest.Limit,
              opt => opt.MapFrom(src => src.Limit.ToString("dd/MM/yy"))).ForMember(
              dest => dest.CurrentValue,
              opt => opt.MapFrom(src => src.ValueReached.ToString("N2", new CultureInfo("pt-br")))).ForMember(
              dest => dest.TotalValue,
              opt => opt.MapFrom(src => src.Value.ToString("N2", new CultureInfo("pt-br")))).ForMember(
              dest => dest.PercentageReached,
              opt => opt.MapFrom(src => src.PercentageReached.ToString("n2", new CultureInfo("pt-br"))));

            CreateMap<InvestmentAdvisorVideoRequest, InvestmentAdvisorVideo>();

            CreateMap<ResultServiceObject<IEnumerable<InvestmentAdvisorVideo>>, ResultResponseObject<IEnumerable<InvestmentAdvisorVideoResponse>>>();
            CreateMap<ResultServiceObject<InvestmentAdvisorVideo>, ResultResponseObject<InvestmentAdvisorVideoResponse>>();
            CreateMap<ResultServiceObject<IEnumerable<InvestmentAdvisorFreeRecommendationView>>, ResultResponseObject<IEnumerable<FreeRecommendationResponse>>>();
            CreateMap<ResultServiceObject<InvestmentAdvisorFreeRecommendationView>, ResultResponseObject<FreeRecommendationResponse>>();
            CreateMap<InvestmentAdvisorFreeRecommendationView, FreeRecommendationResponse>().ForMember(
                  dest => dest.MarketPrice,
                  opt => opt.MapFrom(src => src.MarketPrice.ToString("N2", new CultureInfo("pt-br"))));
            CreateMap<ResultServiceObject<IEnumerable<ApplicationUser>>, ResultResponseObject<IEnumerable<ApplicationUser>>>();
            CreateMap<ResultServiceObject<Subscription>, ResultResponseObject<Subscribe>>();
            CreateMap<Subscription, Subscribe>();
            CreateMap<IEnumerable<FreeRecommendationRequest>, IEnumerable<InvestmentAdvisorFreeRecommendation>>();
            CreateMap<FreeRecommendationRequest, InvestmentAdvisorFreeRecommendation>().ForMember(
               dest => dest.InvestmentAdvisorFreeRecommendationTypeID,
               opt => opt.MapFrom(src => ((int)src.InvestmentAdvisorFreeRecommendationType)));

            CreateMap<ResultResponseObject<API.Model.Response.v2.PortfolioStatementWrapperVM>, ResultResponseObject<API.Model.Response.v3.PortfolioStatementWrapperVM>>();
            CreateMap<ResultResponseObject<IEnumerable<PortfolioStatementViewModel>>, ResultResponseObject<IEnumerable<API.Model.Response.v4.PortfolioStatementViewModel>>>();

            CreateMap<ResultServiceObject<InvestmentAdvisorFreeRecommendation>, ResultResponseObject<FreeRecommendationRequest>>().ReverseMap();
            CreateMap<InvestmentAdvisorFreeRecommendation, FreeRecommendationRequest>().ReverseMap();

            CreateMap<ResultServiceObject<IEnumerable<InfluencerView>>, ResultResponseObject<IEnumerable<InfluencerVM>>>().ReverseMap();
            CreateMap<InfluencerView, InfluencerVM>().ReverseMap();
            CreateMap<ResultServiceObject<IEnumerable<ResearchProduct>>, ResultResponseObject<IEnumerable<ResearchProductVM>>>();

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
    }
}
