using AutoMapper;
using K.Logger;
using Dividendos.API.Model.Response.Common;

using Dividendos.Application.Base;
using Dividendos.Application.Interface;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Finance.Interface;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Request;
using Newtonsoft.Json;
using Dividendos.Entity.Enum;

namespace Dividendos.Application
{
    public class TraderApp : BaseApp, ITraderApp
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly IPortfolioService _portfolioService;
        private readonly ITraderService _traderService;
        private readonly ILogger _logger;
        private readonly ICipherService _cipherService;
        private readonly IGlobalAuthenticationService _globalAuthenticationService;
        private readonly ISubPortfolioService _subPortfolioService;
        private readonly IDeviceService _deviceService;
        private readonly IUserService _userService;
        private readonly ISubscriptionService _subscriptionService;
        private readonly INotificationService _notificationService;
        private readonly INotificationHistoricalService _notificationHistoricalService;
        private readonly ICacheService _cacheService;
        private readonly IFinancialProductService _financialProductService;
        private readonly ICryptoPortfolioService _cryptoPortfolioService;
        private readonly ICryptoSubPortfolioService _cryptoSubPortfolioService;

        public TraderApp(IMapper mapper,
            IUnitOfWork uow,
            ITraderService traderService,
            ILogger logger,
            ICipherService cipherService,
            IGlobalAuthenticationService globalAuthenticationService,
            IPortfolioService portfolioService,
            ISubPortfolioService subPortfolioService,
            IDeviceService deviceService,
            IUserService userService,
            ISubscriptionService subscriptionService,
            INotificationService notificationService,
            INotificationHistoricalService notificationHistoricalService,
            ICacheService cacheService,
            IFinancialProductService financialProductService,
            ICryptoPortfolioService cryptoPortfolioService,
            ICryptoSubPortfolioService cryptoSubPortfolioService)
        {
            _mapper = mapper;
            _uow = uow;
            _traderService = traderService;
            _portfolioService = portfolioService;
            _logger = logger;
            _cipherService = cipherService;
            _globalAuthenticationService = globalAuthenticationService;
            _subPortfolioService = subPortfolioService;
            _deviceService = deviceService;
            _userService = userService;
            _subscriptionService = subscriptionService;
            _notificationService = notificationService;
            _notificationHistoricalService = notificationHistoricalService;
            _cacheService = cacheService;
            _financialProductService = financialProductService;
            _cryptoPortfolioService = cryptoPortfolioService;
            _cryptoSubPortfolioService = cryptoSubPortfolioService;
        }

        public ResultResponseObject<TraderVM> Delete(Guid traderGuid)
        {
            ResultServiceObject<Trader> trader;

            using (_uow.Create())
            {
                trader = _traderService.GetByUserAndGuidTrader(_globalAuthenticationService.IdUser, traderGuid);

                if (trader.Value != null)
                {
                    //remove financial products by trader
                    ResultServiceObject<IEnumerable<ProductUserView>> resultProducts = _financialProductService.GetProductsByTrader(trader.Value.IdTrader);

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

                    if (trader.Value != null)
                    {
                        _traderService.Disable(trader.Value.IdTrader);
                    }
                }
            }

            ResultResponseObject<TraderVM> resultReponse = _mapper.Map<ResultResponseObject<TraderVM>>(trader);

            return resultReponse;
        }

        public ResultResponseObject<Trader> GetTraderById(long idTrader)
        {
            ResultServiceObject<Trader> resultServiceDomain;

            using (_uow.Create())
            {
                resultServiceDomain = _traderService.GetById(idTrader);
            }

            ResultResponseObject<Trader> resultResponseDomain = _mapper.Map<ResultResponseObject<Trader>>(resultServiceDomain);

            return resultResponseDomain;
        }

        public ResultResponseObject<IEnumerable<API.Model.Response.v4.TraderSummaryVM>> GetByLoggedUserV4()
        {
            ResultResponseObject<IEnumerable<API.Model.Response.v5.TraderSummaryVM>> resultResponseObjectV5 = this.GetByLoggedUserV5();

            ResultResponseObject<IEnumerable<API.Model.Response.v4.TraderSummaryVM>> resultResponseObjectV4 = _mapper.Map<ResultResponseObject<IEnumerable<API.Model.Response.v4.TraderSummaryVM>>>(resultResponseObjectV5);

            return resultResponseObjectV4;
        }

        public ResultResponseObject<IEnumerable<API.Model.Response.v5.TraderSummaryVM>> GetByLoggedUserV5()
        {
            List<API.Model.Response.v5.TraderSummaryVM> traderSummaryVMs = new List<API.Model.Response.v5.TraderSummaryVM>();


            using (_uow.Create())
            {
                ResultServiceObject<Subscription> subscription = _subscriptionService.GetByUser(_globalAuthenticationService.IdUser);
                bool hasSubscription = false;

                if (subscription.Value != null && subscription.Value.Active && (subscription.Value.SubscriptionTypeID.Equals((int)SubscriptionTypeEnum.Gold) ||
                                                                                subscription.Value.SubscriptionTypeID.Equals((int)SubscriptionTypeEnum.Annuity)))
                {
                    hasSubscription = true;
                }

                ResultServiceObject<IEnumerable<Trader>> resultTrader = _traderService.GetByUserActive(_globalAuthenticationService.IdUser);

                List<Trader> tradersAutomatic = new List<Trader>();
                List<Trader> tradersManual = new List<Trader>();

                if (resultTrader.Success && resultTrader.Value != null && resultTrader.Value.Count() > 0)
                {
                    tradersAutomatic = resultTrader.Value.Where(traderTmp => traderTmp.ManualPortfolio == false).ToList();
                    tradersManual = resultTrader.Value.Where(traderTmp => traderTmp.ManualPortfolio == true).ToList();
                }

                if (tradersAutomatic != null && tradersAutomatic.Count() > 0)
                {
                    foreach (Trader itemTrader in tradersAutomatic)
                    {
                        if (itemTrader.TraderTypeID.Equals((int)TraderTypeEnum.MercadoBitcoin))
                        {
                            string portfolioName = "Carteira (Mercado Bitcoin)";

                            API.Model.Response.v5.PortfolioVM portfolioVM = new API.Model.Response.v5.PortfolioVM() { GuidPortfolio = itemTrader.GuidTrader, IdCountry = (int)CountryEnum.Brazil, Name = portfolioName };

                            traderSummaryVMs.Add(new API.Model.Response.v5.TraderSummaryVM()
                            {
                                GuidTrader = itemTrader.GuidTrader,
                                Document = itemTrader.Identifier,
                                Password = itemTrader.Password,
                                Name = portfolioName,
                                PortfolioPrincipal = portfolioVM,
                                SubPortfolios = null,
                                ManualPortfolio = false,
                                BlockedCEI = itemTrader.BlockedCei,
                                IsAutomaticIntegration = true,
                                TraderType = API.Model.Response.v5.TraderType.MercadoBitcoin,
                                DateLastSync = itemTrader.LastSync.ToString("dd/MM/yyyy")
                            }); ;
                        }
                        else if (itemTrader.TraderTypeID.Equals((int)TraderTypeEnum.Binance))
                        {
                            string portfolioName = "Carteira (Binance)";

                            API.Model.Response.v5.PortfolioVM portfolioVM = new API.Model.Response.v5.PortfolioVM() { GuidPortfolio = itemTrader.GuidTrader, IdCountry = (int)CountryEnum.EUA, Name = portfolioName };

                            traderSummaryVMs.Add(new API.Model.Response.v5.TraderSummaryVM()
                            {
                                GuidTrader = itemTrader.GuidTrader,
                                Document = itemTrader.Identifier,
                                Password = itemTrader.Password,
                                Name = portfolioName,
                                PortfolioPrincipal = portfolioVM,
                                SubPortfolios = null,
                                ManualPortfolio = false,
                                BlockedCEI = itemTrader.BlockedCei,
                                IsAutomaticIntegration = true,
                                TraderType = API.Model.Response.v5.TraderType.Binance,
                                DateLastSync = itemTrader.LastSync.ToString("dd/MM/yyyy")
                            }); ;
                        }
                        else if (itemTrader.TraderTypeID.Equals((int)TraderTypeEnum.BitcoinTrade))
                        {
                            string portfolioName = "Carteira (BitcoinTrade)";

                            API.Model.Response.v5.PortfolioVM portfolioVM = new API.Model.Response.v5.PortfolioVM() { GuidPortfolio = itemTrader.GuidTrader, IdCountry = (int)CountryEnum.EUA, Name = portfolioName };

                            traderSummaryVMs.Add(new API.Model.Response.v5.TraderSummaryVM()
                            {
                                GuidTrader = itemTrader.GuidTrader,
                                Document = itemTrader.Identifier,
                                Password = itemTrader.Password,
                                Name = portfolioName,
                                PortfolioPrincipal = portfolioVM,
                                SubPortfolios = null,
                                ManualPortfolio = false,
                                BlockedCEI = itemTrader.BlockedCei,
                                IsAutomaticIntegration = true,
                                TraderType = API.Model.Response.v5.TraderType.BitcoinTrade,
                                DateLastSync = itemTrader.LastSync.ToString("dd/MM/yyyy")
                            }); ;
                        }
                        else
                        {
                            ResultServiceObject<Portfolio> resultPortfolio = _portfolioService.GetByTraderActive(itemTrader.IdTrader);

                            if (resultPortfolio.Value != null)
                            {
                                if (itemTrader.TraderTypeID.Equals((int)TraderTypeEnum.RendaVariavelAndTesouroDiretoCEI))
                                {
                                    ResultResponseObject<API.Model.Response.v5.PortfolioVM> resultResponsePortfolio = _mapper.Map<ResultResponseObject<API.Model.Response.v5.PortfolioVM>>(resultPortfolio);

                                    ResultServiceObject<IEnumerable<SubPortfolio>> resultSubPortfolio = _subPortfolioService.GetByPortfolio(resultPortfolio.Value.IdPortfolio);

                                    ResultResponseObject<IEnumerable<SubPortfolioVM>> subPortfolioVM = _mapper.Map<ResultResponseObject<IEnumerable<SubPortfolioVM>>>(resultSubPortfolio);


                                    string portfolioName = string.Format("Carteira {0}", MaskCnpjCpf(itemTrader.Identifier));

                                    traderSummaryVMs.Add(new API.Model.Response.v5.TraderSummaryVM()
                                    {
                                        GuidTrader = itemTrader.GuidTrader,
                                        Document = itemTrader.Identifier,
                                        Password = itemTrader.Password,
                                        Name = portfolioName,
                                        PortfolioPrincipal = resultResponsePortfolio.Value,
                                        SubPortfolios = subPortfolioVM.Value,
                                        ManualPortfolio = false,
                                        BlockedCEI = itemTrader.BlockedCei,
                                        IsAutomaticIntegration = true,
                                        TraderType = API.Model.Response.v5.TraderType.RendaVariavelAndTesouroDiretoCEI,
                                        DateLastSync = itemTrader.LastSync.ToString("dd/MM/yyyy")
                                    });
                                }
                                else if (itemTrader.TraderTypeID.Equals((int)TraderTypeEnum.Passfolio))
                                {
                                    ResultResponseObject<API.Model.Response.v5.PortfolioVM> resultResponsePortfolio = _mapper.Map<ResultResponseObject<API.Model.Response.v5.PortfolioVM>>(resultPortfolio);

                                    ResultServiceObject<IEnumerable<ProductUserView>> productUserView = _financialProductService.GetProductsByCategoryAndTrader(ProductCategoryEnum.CryptoCurrencies, itemTrader.IdTrader);

                                    if (productUserView.Value != null && productUserView.Value.Count() > 0)
                                    {
                                        resultResponsePortfolio.Value.HasCrypto = true;
                                    }


                                    ResultServiceObject<IEnumerable<SubPortfolio>> resultSubPortfolio = _subPortfolioService.GetByPortfolio(resultPortfolio.Value.IdPortfolio);

                                    ResultResponseObject<IEnumerable<SubPortfolioVM>> subPortfolioVM = _mapper.Map<ResultResponseObject<IEnumerable<SubPortfolioVM>>>(resultSubPortfolio);


                                    string portfolioName = string.Format("Passfolio ({0})", itemTrader.Identifier);

                                    traderSummaryVMs.Add(new API.Model.Response.v5.TraderSummaryVM()
                                    {
                                        GuidTrader = itemTrader.GuidTrader,
                                        Document = itemTrader.Identifier,
                                        Password = itemTrader.Password,
                                        Name = portfolioName,
                                        PortfolioPrincipal = resultResponsePortfolio.Value,
                                        SubPortfolios = subPortfolioVM.Value,
                                        ManualPortfolio = false,
                                        BlockedCEI = itemTrader.BlockedCei,
                                        IsAutomaticIntegration = true,
                                        TraderType = API.Model.Response.v5.TraderType.Passfolio,
                                        DateLastSync = itemTrader.LastSync.ToString("dd/MM/yyyy")
                                    });
                                }
                            }
                        }
                    }
                }

                if (tradersManual != null && tradersManual.Count() > 0)
                {
                    List<API.Model.Response.v5.PortfolioVM> manualPortfolios = new List<API.Model.Response.v5.PortfolioVM>();
                    List<SubPortfolioVM> manualSubPortfolios = new List<SubPortfolioVM>();

                    foreach (Trader itemTrader in tradersManual)
                    {
                        ResultServiceObject<Portfolio> resultPortfolio = _portfolioService.GetByTraderActive(itemTrader.IdTrader);

                        if (resultPortfolio.Value != null)
                        {
                            if (resultPortfolio.Value.IdCountry.Equals((int)CountryEnum.EUA) && !hasSubscription)
                            {
                                continue;
                            }

                            ResultResponseObject<API.Model.Response.v5.PortfolioVM> resultResponsePortfolio = _mapper.Map<ResultResponseObject<API.Model.Response.v5.PortfolioVM>>(resultPortfolio);

                            manualPortfolios.Add(resultResponsePortfolio.Value);

                            ResultServiceObject<IEnumerable<SubPortfolio>> resultSubPortfolio = _subPortfolioService.GetByPortfolio(resultPortfolio.Value.IdPortfolio);

                            if (resultSubPortfolio.Success && resultSubPortfolio.Value != null && resultSubPortfolio.Value.Count() > 0)
                            {
                                foreach (SubPortfolio subPortfolio in resultSubPortfolio.Value)
                                {
                                    SubPortfolioVM subPortfolioVM = new SubPortfolioVM();
                                    subPortfolioVM.GuidPortfolio = resultPortfolio.Value.GuidPortfolio;
                                    subPortfolioVM.Name = subPortfolio.Name;
                                    subPortfolioVM.GuidSubPortfolio = subPortfolio.GuidSubPortfolio;

                                    manualSubPortfolios.Add(subPortfolioVM);
                                }
                            }
                        }
                    }

                    if (manualPortfolios.Count > 0)
                    {
                        traderSummaryVMs.Add(new API.Model.Response.v5.TraderSummaryVM()
                        {
                            GuidTrader = Guid.Empty,
                            Document = "",
                            Password = "",
                            Name = "Carteira Manual",
                            PortfolioPrincipal = null,
                            SubPortfolios = null,
                            BlockedCEI = false,
                            ManualPortfolio = true,
                            ManualPortfolios = manualPortfolios,
                            ManualSubPortfolios = manualSubPortfolios,
                            IsAutomaticIntegration = false,
                            TraderType = API.Model.Response.v5.TraderType.RendaVariavelManual
                        });
                    }
                }


            }

            ResultResponseObject<IEnumerable<API.Model.Response.v5.TraderSummaryVM>> resultResponseDomain = new ResultResponseObject<IEnumerable<API.Model.Response.v5.TraderSummaryVM>>() { Value = traderSummaryVMs };
            resultResponseDomain.Success = true;

            return resultResponseDomain;
        }

        public ResultResponseObject<IEnumerable<API.Model.Response.v6.TraderSummaryVM>> GetByLoggedUserV6()
        {
            List<API.Model.Response.v6.TraderSummaryVM> traderSummaryVMs = new List<API.Model.Response.v6.TraderSummaryVM>();


            using (_uow.Create())
            {
                ResultServiceObject<Subscription> subscription = _subscriptionService.GetByUser(_globalAuthenticationService.IdUser);
                bool hasSubscription = false;

                if (subscription.Value != null && subscription.Value.Active && (subscription.Value.SubscriptionTypeID.Equals((int)SubscriptionTypeEnum.Gold) ||
                                                                                subscription.Value.SubscriptionTypeID.Equals((int)SubscriptionTypeEnum.Annuity)))
                {
                    hasSubscription = true;
                }

                ResultServiceObject<IEnumerable<Trader>> resultTrader = _traderService.GetByUserActive(_globalAuthenticationService.IdUser);

                List<Trader> tradersAutomatic = new List<Trader>();
                List<Trader> tradersManual = new List<Trader>();

                if (resultTrader.Success && resultTrader.Value != null && resultTrader.Value.Count() > 0)
                {
                    tradersAutomatic = resultTrader.Value.Where(traderTmp => traderTmp.ManualPortfolio == false).ToList();
                    tradersManual = resultTrader.Value.Where(traderTmp => traderTmp.ManualPortfolio == true).ToList();
                }

                if (tradersAutomatic != null && tradersAutomatic.Count() > 0)
                {
                    foreach (Trader itemTrader in tradersAutomatic)
                    {
                        if (itemTrader.TraderTypeID.Equals((int)TraderTypeEnum.MercadoBitcoin))
                        {
                            API.Model.Response.v6.PortfolioVM portfolioVM = new API.Model.Response.v6.PortfolioVM() { GuidPortfolio = itemTrader.GuidTrader, IdCountry = (int)CountryEnum.Brazil, Name = "Carteira" };

                            traderSummaryVMs.Add(new API.Model.Response.v6.TraderSummaryVM()
                            {
                                GuidTrader = itemTrader.GuidTrader,
                                Document = itemTrader.Identifier,
                                Password = itemTrader.Password,
                                Name = "Mercado Bitcoin",
                                PortfolioPrincipal = portfolioVM,
                                SubPortfolios = null,
                                ManualPortfolio = false,
                                BlockedCEI = itemTrader.BlockedCei,
                                IsAutomaticIntegration = true,
                                TraderType = API.Model.Response.v6.TraderType.MercadoBitcoin,
                                DateLastSync = itemTrader.LastSync.ToString("dd/MM/yyyy")
                            }); ;
                        }
                        else if (itemTrader.TraderTypeID.Equals((int)TraderTypeEnum.Binance))
                        {
                            API.Model.Response.v6.PortfolioVM portfolioVM = new API.Model.Response.v6.PortfolioVM() { GuidPortfolio = itemTrader.GuidTrader, IdCountry = (int)CountryEnum.EUA, Name = "Carteira" };

                            traderSummaryVMs.Add(new API.Model.Response.v6.TraderSummaryVM()
                            {
                                GuidTrader = itemTrader.GuidTrader,
                                Document = itemTrader.Identifier,
                                Password = itemTrader.Password,
                                Name = "Binance",
                                PortfolioPrincipal = portfolioVM,
                                SubPortfolios = null,
                                ManualPortfolio = false,
                                BlockedCEI = itemTrader.BlockedCei,
                                IsAutomaticIntegration = true,
                                TraderType = API.Model.Response.v6.TraderType.Binance,
                                DateLastSync = itemTrader.LastSync.ToString("dd/MM/yyyy")
                            }); ;
                        }
                        else if (itemTrader.TraderTypeID.Equals((int)TraderTypeEnum.BitcoinTrade))
                        {
                            API.Model.Response.v6.PortfolioVM portfolioVM = new API.Model.Response.v6.PortfolioVM() { GuidPortfolio = itemTrader.GuidTrader, IdCountry = (int)CountryEnum.EUA, Name = "Carteira" };

                            traderSummaryVMs.Add(new API.Model.Response.v6.TraderSummaryVM()
                            {
                                GuidTrader = itemTrader.GuidTrader,
                                Document = itemTrader.Identifier,
                                Password = itemTrader.Password,
                                Name = "BitcoinTrade",
                                PortfolioPrincipal = portfolioVM,
                                SubPortfolios = null,
                                ManualPortfolio = false,
                                BlockedCEI = itemTrader.BlockedCei,
                                IsAutomaticIntegration = true,
                                TraderType = API.Model.Response.v6.TraderType.BitcoinTrade,
                                DateLastSync = itemTrader.LastSync.ToString("dd/MM/yyyy")
                            }); ;
                        }
                        else if (itemTrader.TraderTypeID.Equals((int)TraderTypeEnum.Coinbase))
                        {
                            API.Model.Response.v6.PortfolioVM portfolioVM = new API.Model.Response.v6.PortfolioVM() { GuidPortfolio = itemTrader.GuidTrader, IdCountry = (int)CountryEnum.EUA, Name = "Carteira" };

                            traderSummaryVMs.Add(new API.Model.Response.v6.TraderSummaryVM()
                            {
                                GuidTrader = itemTrader.GuidTrader,
                                Document = itemTrader.Identifier,
                                Password = itemTrader.Password,
                                Name = "Coinbase",
                                PortfolioPrincipal = portfolioVM,
                                SubPortfolios = null,
                                ManualPortfolio = false,
                                BlockedCEI = itemTrader.BlockedCei,
                                IsAutomaticIntegration = true,
                                TraderType = API.Model.Response.v6.TraderType.Coinbase,
                                DateLastSync = itemTrader.LastSync.ToString("dd/MM/yyyy")
                            }); ;
                        }
                        else if (itemTrader.TraderTypeID.Equals((int)TraderTypeEnum.BitcoinToYou))
                        {
                            API.Model.Response.v6.PortfolioVM portfolioVM = new API.Model.Response.v6.PortfolioVM() { GuidPortfolio = itemTrader.GuidTrader, IdCountry = (int)CountryEnum.EUA, Name = "Carteira" };

                            traderSummaryVMs.Add(new API.Model.Response.v6.TraderSummaryVM()
                            {
                                GuidTrader = itemTrader.GuidTrader,
                                Document = itemTrader.Identifier,
                                Password = itemTrader.Password,
                                Name = "BitcoinToYou",
                                PortfolioPrincipal = portfolioVM,
                                SubPortfolios = null,
                                ManualPortfolio = false,
                                BlockedCEI = itemTrader.BlockedCei,
                                IsAutomaticIntegration = true,
                                TraderType = API.Model.Response.v6.TraderType.BitcoinToYou,
                                DateLastSync = itemTrader.LastSync.ToString("dd/MM/yyyy")
                            }); ;
                        }
                        else
                        {
                            ResultServiceObject<Portfolio> resultPortfolio = _portfolioService.GetByTraderActive(itemTrader.IdTrader);

                            if (resultPortfolio.Value != null)
                            {
                                if (itemTrader.TraderTypeID.Equals((int)TraderTypeEnum.RendaVariavelAndTesouroDiretoCEI))
                                {
                                    ResultResponseObject<API.Model.Response.v6.PortfolioVM> resultResponsePortfolio = _mapper.Map<ResultResponseObject<API.Model.Response.v6.PortfolioVM>>(resultPortfolio);

                                    ResultServiceObject<IEnumerable<SubPortfolio>> resultSubPortfolio = _subPortfolioService.GetByPortfolio(resultPortfolio.Value.IdPortfolio);

                                    ResultResponseObject<IEnumerable<SubPortfolioVM>> subPortfolioVM = _mapper.Map<ResultResponseObject<IEnumerable<SubPortfolioVM>>>(resultSubPortfolio);


                                    string portfolioName = MaskCnpjCpf(itemTrader.Identifier);

                                    traderSummaryVMs.Add(new API.Model.Response.v6.TraderSummaryVM()
                                    {
                                        GuidTrader = itemTrader.GuidTrader,
                                        Document = itemTrader.Identifier,
                                        Password = itemTrader.Password,
                                        Name = portfolioName,
                                        PortfolioPrincipal = resultResponsePortfolio.Value,
                                        SubPortfolios = subPortfolioVM.Value,
                                        ManualPortfolio = false,
                                        BlockedCEI = itemTrader.BlockedCei,
                                        IsAutomaticIntegration = true,
                                        TraderType = API.Model.Response.v6.TraderType.RendaVariavelAndTesouroDiretoCEI,
                                        DateLastSync = itemTrader.LastSync.ToString("dd/MM/yyyy")
                                    });
                                }
                                else if (itemTrader.TraderTypeID.Equals((int)TraderTypeEnum.Passfolio))
                                {
                                    ResultResponseObject<API.Model.Response.v6.PortfolioVM> resultResponsePortfolio = _mapper.Map<ResultResponseObject<API.Model.Response.v6.PortfolioVM>>(resultPortfolio);

                                    ResultServiceObject<IEnumerable<ProductUserView>> productUserView = _financialProductService.GetProductsByCategoryAndTrader(ProductCategoryEnum.CryptoCurrencies, itemTrader.IdTrader);

                                    if (productUserView.Value != null && productUserView.Value.Count() > 0)
                                    {
                                        resultResponsePortfolio.Value.HasCrypto = true;
                                    }


                                    ResultServiceObject<IEnumerable<SubPortfolio>> resultSubPortfolio = _subPortfolioService.GetByPortfolio(resultPortfolio.Value.IdPortfolio);

                                    ResultResponseObject<IEnumerable<SubPortfolioVM>> subPortfolioVM = _mapper.Map<ResultResponseObject<IEnumerable<SubPortfolioVM>>>(resultSubPortfolio);


                                    string portfolioName = itemTrader.Identifier;

                                    traderSummaryVMs.Add(new API.Model.Response.v6.TraderSummaryVM()
                                    {
                                        GuidTrader = itemTrader.GuidTrader,
                                        Document = itemTrader.Identifier,
                                        Password = itemTrader.Password,
                                        Name = portfolioName,
                                        PortfolioPrincipal = resultResponsePortfolio.Value,
                                        SubPortfolios = subPortfolioVM.Value,
                                        ManualPortfolio = false,
                                        BlockedCEI = itemTrader.BlockedCei,
                                        IsAutomaticIntegration = true,
                                        TraderType = API.Model.Response.v6.TraderType.Passfolio,
                                        DateLastSync = itemTrader.LastSync.ToString("dd/MM/yyyy")
                                    });
                                }
                            }
                        }
                    }
                }

                if (tradersManual != null && tradersManual.Count() > 0)
                {
                    foreach (Trader itemTrader in tradersManual)
                    {
                        List<SubPortfolioVM> manualSubPortfolios = new List<SubPortfolioVM>();

                        ResultServiceObject<Portfolio> resultPortfolio = _portfolioService.GetByTraderActive(itemTrader.IdTrader);

                        if (resultPortfolio.Value != null)
                        {
                            if (resultPortfolio.Value.IdCountry.Equals((int)CountryEnum.EUA) && !hasSubscription)
                            {
                                continue;
                            }

                            ResultResponseObject<API.Model.Response.v6.PortfolioVM> resultResponsePortfolio = _mapper.Map<ResultResponseObject<API.Model.Response.v6.PortfolioVM>>(resultPortfolio);

                            ResultServiceObject<IEnumerable<SubPortfolio>> resultSubPortfolio = _subPortfolioService.GetByPortfolio(resultPortfolio.Value.IdPortfolio);

                            if (resultSubPortfolio.Success && resultSubPortfolio.Value != null && resultSubPortfolio.Value.Count() > 0)
                            {
                                foreach (SubPortfolio subPortfolio in resultSubPortfolio.Value)
                                {
                                    SubPortfolioVM subPortfolioVM = new SubPortfolioVM();
                                    subPortfolioVM.GuidPortfolio = resultPortfolio.Value.GuidPortfolio;
                                    subPortfolioVM.Name = subPortfolio.Name;
                                    subPortfolioVM.GuidSubPortfolio = subPortfolio.GuidSubPortfolio;

                                    manualSubPortfolios.Add(subPortfolioVM);
                                }
                            }

                            traderSummaryVMs.Add(new API.Model.Response.v6.TraderSummaryVM()
                            {
                                GuidTrader = Guid.Empty,
                                Document = "",
                                Password = "",
                                Name = "Carteira Manual",
                                PortfolioPrincipal = resultResponsePortfolio.Value,
                                SubPortfolios = manualSubPortfolios,
                                BlockedCEI = false,
                                ManualPortfolio = true,
                                IsAutomaticIntegration = false,
                                TraderType = API.Model.Response.v6.TraderType.RendaVariavelManual
                            });
                        }
                    }
                }
            }

            ResultResponseObject<IEnumerable<API.Model.Response.v6.TraderSummaryVM>> resultResponseDomain = new ResultResponseObject<IEnumerable<API.Model.Response.v6.TraderSummaryVM>>() { Value = traderSummaryVMs };
            resultResponseDomain.Success = true;

            return resultResponseDomain;
        }

        public ResultResponseObject<IEnumerable<TraderSummaryVM>> GetByLoggedUser()
        {
            ResultResponseObject<IEnumerable<API.Model.Response.v4.TraderSummaryVM>> traderSummaryVMs = this.GetByLoggedUserV4();

            ResultResponseObject<IEnumerable<TraderSummaryVM>> resultReponse = _mapper.Map<ResultResponseObject<IEnumerable<TraderSummaryVM>>>(traderSummaryVMs);

            return resultReponse;
        }

        public ResultResponseObject<TraderVM> ChangeTraderCredentials(string identifier, string password, TraderTypeEnum traderTypeEnun)
        {
            ResultServiceObject<Trader> resultService;

            using (_uow.Create())
            {
                resultService = _traderService.SaveTrader(identifier, password, _globalAuthenticationService.IdUser, false, false, traderTypeEnun);
            }

            ResultResponseObject<TraderVM> resultReponse = _mapper.Map<ResultResponseObject<TraderVM>>(resultService);

            return resultReponse;
        }


        private string MaskCnpjCpf(string value)
        {
            string result = string.Empty;

            if (value.Length == 14)
            {
                result = value.Insert(2, ".").Insert(6, ".").Insert(10, "/").Insert(15, "-");
            }
            if (value.Length == 11)
            {
                result = value.Insert(3, ".").Insert(7, ".").Insert(11, "-");
            }
            if ((value.Length != 11) && (value.Length != 14))
            {
                result = value;
            }

            return result;
        }


        public void SendAlertToTraderBlocked()
        {
            _logger.SendDebugAsync(new { JobDebugInfo = "Iniciando SendAlertToTraderBlocked" });

            ResultServiceObject<IEnumerable<Trader>> traders;

            //Get trader that set auto sync
            using (_uow.Create())
            {
                traders = _traderService.GetAllBlockedAutomatic();

                //send push

                foreach (var itemTrader in traders.Value)
                {
                    string account = MaskCnpjCpf(itemTrader.Identifier);

                    if (itemTrader.TraderTypeID == (int)TraderTypeEnum.Binance)
                    {
                        account = "Binance";
                    }
                    else if (itemTrader.TraderTypeID == (int)TraderTypeEnum.MercadoBitcoin)
                    {
                        account = "Mercado Bitcoin";
                    }
                    else if (itemTrader.TraderTypeID == (int)TraderTypeEnum.BitcoinTrade)
                    {
                        account = "BitcoinTrade";
                    }

                    ResultServiceObject<IEnumerable<Device>> devices = new ResultServiceObject<IEnumerable<Device>>();

                    devices = _deviceService.GetByUser(itemTrader.IdUser);

                    string title = @"Urgente: Ação necessária!";
                    string message = $"Atenção! Sua crendencial de acesso correspondente a conta {account} está expirada. Entre no App Dividendos.me e ajuste para continuar com suas informações atualizadas.";

                    _notificationHistoricalService.New(title, message, itemTrader.IdUser, AppScreenNameEnum.Wallets.ToString(), PushRedirectTypeEnum.Internal.ToString(), null, _cacheService);

                    foreach (var itemDevice in devices.Value)
                    {
                        try
                        {
                            _notificationService.SendPush(title, message, itemDevice, new PushRedirect() { PushRedirectType = PushRedirectTypeEnum.Internal, AppScreenName = AppScreenNameEnum.Wallets });
                        }
                        catch (Exception ex)
                        {
                            _ = _logger.SendErrorAsync(ex);
                        }
                    }
                }
            }

            _logger.SendDebugAsync(new { JobDebugInfo = "finalizado SendAlertToTraderBlocked" });
        }

        public ResultResponseObject<TraderVM> GetBlockedTrader()
        {
            ResultServiceObject<Trader> resultServiceDomain;

            using (_uow.Create())
            {
                resultServiceDomain = _traderService.GetBlockedByUser(_globalAuthenticationService.IdUser);
            }

            ResultResponseObject<TraderVM> resultReponse = _mapper.Map<ResultResponseObject<TraderVM>>(resultServiceDomain);

            return resultReponse;
        }

        public ResultResponseObject<IEnumerable<API.Model.Response.v7.TraderSummaryVM>> GetByLoggedUserV7()
        {
            List<API.Model.Response.v7.TraderSummaryVM> traderSummaryVMs = new List<API.Model.Response.v7.TraderSummaryVM>();


            using (_uow.Create())
            {
                ResultServiceObject<Subscription> subscription = _subscriptionService.GetByUser(_globalAuthenticationService.IdUser);
                bool hasSubscription = false;

                if (subscription.Value != null && subscription.Value.Active && (subscription.Value.SubscriptionTypeID.Equals((int)SubscriptionTypeEnum.Gold) ||
                                                                                subscription.Value.SubscriptionTypeID.Equals((int)SubscriptionTypeEnum.Annuity)))
                {
                    hasSubscription = true;
                }

                ResultServiceObject<IEnumerable<Trader>> resultTrader = _traderService.GetByUserActive(_globalAuthenticationService.IdUser);

                List<Trader> tradersAutomatic = new List<Trader>();
                List<Trader> tradersManual = new List<Trader>();

                if (resultTrader.Success && resultTrader.Value != null && resultTrader.Value.Count() > 0)
                {
                    tradersAutomatic = resultTrader.Value.Where(traderTmp => traderTmp.ManualPortfolio == false).ToList();
                    tradersManual = resultTrader.Value.Where(traderTmp => traderTmp.ManualPortfolio == true).ToList();
                }

                if (tradersAutomatic != null && tradersAutomatic.Count() > 0)
                {
                    foreach (Trader itemTrader in tradersAutomatic)
                    {
                        if (itemTrader.TraderTypeID.Equals((int)TraderTypeEnum.MercadoBitcoin))
                        {
                            API.Model.Response.v7.PortfolioVM portfolioVM = new API.Model.Response.v7.PortfolioVM() { GuidPortfolio = itemTrader.GuidTrader, IdCountry = (int)CountryEnum.Brazil, Name = "Carteira" };

                            traderSummaryVMs.Add(new API.Model.Response.v7.TraderSummaryVM()
                            {
                                GuidTrader = itemTrader.GuidTrader,
                                Document = itemTrader.Identifier,
                                Name = "Mercado Bitcoin",
                                PortfolioPrincipal = portfolioVM,
                                SubPortfolios = null,
                                ManualPortfolio = false,
                                BlockedCEI = itemTrader.BlockedCei,
                                IsAutomaticIntegration = true,
                                TraderType = API.Model.Response.v7.TraderType.MercadoBitcoin,
                                DateLastSync = itemTrader.LastSync.ToString("dd/MM/yyyy")
                            }); ;
                        }
                        if (itemTrader.TraderTypeID.Equals((int)TraderTypeEnum.BitPreco))
                        {
                            API.Model.Response.v7.PortfolioVM portfolioVM = new API.Model.Response.v7.PortfolioVM() { GuidPortfolio = itemTrader.GuidTrader, IdCountry = (int)CountryEnum.Brazil, Name = "Carteira" };

                            traderSummaryVMs.Add(new API.Model.Response.v7.TraderSummaryVM()
                            {
                                GuidTrader = itemTrader.GuidTrader,
                                Document = itemTrader.Identifier,
                                Name = "BitPreço",
                                PortfolioPrincipal = portfolioVM,
                                SubPortfolios = null,
                                ManualPortfolio = false,
                                BlockedCEI = itemTrader.BlockedCei,
                                IsAutomaticIntegration = true,
                                TraderType = API.Model.Response.v7.TraderType.BitPreco,
                                DateLastSync = itemTrader.LastSync.ToString("dd/MM/yyyy")
                            }); ;
                        }
                        else if (itemTrader.TraderTypeID.Equals((int)TraderTypeEnum.Binance))
                        {
                            API.Model.Response.v7.PortfolioVM portfolioVM = new API.Model.Response.v7.PortfolioVM() { GuidPortfolio = itemTrader.GuidTrader, IdCountry = (int)CountryEnum.EUA, Name = "Carteira" };

                            traderSummaryVMs.Add(new API.Model.Response.v7.TraderSummaryVM()
                            {
                                GuidTrader = itemTrader.GuidTrader,
                                Document = itemTrader.Identifier,
                                Password = itemTrader.Password,
                                Name = "Binance",
                                PortfolioPrincipal = portfolioVM,
                                SubPortfolios = null,
                                ManualPortfolio = false,
                                BlockedCEI = itemTrader.BlockedCei,
                                IsAutomaticIntegration = true,
                                TraderType = API.Model.Response.v7.TraderType.Binance,
                                DateLastSync = itemTrader.LastSync.ToString("dd/MM/yyyy")
                            }); ;
                        }
                        else if (itemTrader.TraderTypeID.Equals((int)TraderTypeEnum.BitcoinTrade))
                        {
                            API.Model.Response.v7.PortfolioVM portfolioVM = new API.Model.Response.v7.PortfolioVM() { GuidPortfolio = itemTrader.GuidTrader, IdCountry = (int)CountryEnum.EUA, Name = "Carteira" };

                            traderSummaryVMs.Add(new API.Model.Response.v7.TraderSummaryVM()
                            {
                                GuidTrader = itemTrader.GuidTrader,
                                Document = itemTrader.Identifier,
                                Name = "BitcoinTrade",
                                PortfolioPrincipal = portfolioVM,
                                SubPortfolios = null,
                                ManualPortfolio = false,
                                BlockedCEI = itemTrader.BlockedCei,
                                IsAutomaticIntegration = true,
                                TraderType = API.Model.Response.v7.TraderType.BitcoinTrade,
                                DateLastSync = itemTrader.LastSync.ToString("dd/MM/yyyy")
                            }); ;
                        }
                        else if (itemTrader.TraderTypeID.Equals((int)TraderTypeEnum.Coinbase))
                        {
                            API.Model.Response.v7.PortfolioVM portfolioVM = new API.Model.Response.v7.PortfolioVM() { GuidPortfolio = itemTrader.GuidTrader, IdCountry = (int)CountryEnum.EUA, Name = "Carteira" };

                            traderSummaryVMs.Add(new API.Model.Response.v7.TraderSummaryVM()
                            {
                                GuidTrader = itemTrader.GuidTrader,
                                Document = itemTrader.Identifier,
                                Name = "Coinbase",
                                PortfolioPrincipal = portfolioVM,
                                SubPortfolios = null,
                                ManualPortfolio = false,
                                BlockedCEI = itemTrader.BlockedCei,
                                IsAutomaticIntegration = true,
                                TraderType = API.Model.Response.v7.TraderType.Coinbase,
                                DateLastSync = itemTrader.LastSync.ToString("dd/MM/yyyy")
                            }); ;
                        }
                        else if (itemTrader.TraderTypeID.Equals((int)TraderTypeEnum.BitcoinToYou))
                        {
                            API.Model.Response.v7.PortfolioVM portfolioVM = new API.Model.Response.v7.PortfolioVM() { GuidPortfolio = itemTrader.GuidTrader, IdCountry = (int)CountryEnum.EUA, Name = "Carteira" };

                            traderSummaryVMs.Add(new API.Model.Response.v7.TraderSummaryVM()
                            {
                                GuidTrader = itemTrader.GuidTrader,
                                Document = itemTrader.Identifier,
                                Name = "BitcoinToYou",
                                PortfolioPrincipal = portfolioVM,
                                SubPortfolios = null,
                                ManualPortfolio = false,
                                BlockedCEI = itemTrader.BlockedCei,
                                IsAutomaticIntegration = true,
                                TraderType = API.Model.Response.v7.TraderType.BitcoinToYou,
                                DateLastSync = itemTrader.LastSync.ToString("dd/MM/yyyy")
                            }); ;
                        }
                        else
                        {
                            ResultServiceObject<Portfolio> resultPortfolio = _portfolioService.GetByTraderActive(itemTrader.IdTrader);

                            if (resultPortfolio.Value != null)
                            {
                                if (itemTrader.TraderTypeID.Equals((int)TraderTypeEnum.RendaVariavelAndTesouroDiretoCEI))
                                {
                                    ResultResponseObject<API.Model.Response.v7.PortfolioVM> resultResponsePortfolio = _mapper.Map<ResultResponseObject<API.Model.Response.v7.PortfolioVM>>(resultPortfolio);

                                    ResultServiceObject<IEnumerable<SubPortfolio>> resultSubPortfolio = _subPortfolioService.GetByPortfolio(resultPortfolio.Value.IdPortfolio);

                                    ResultResponseObject<IEnumerable<SubPortfolioVM>> subPortfolioVM = _mapper.Map<ResultResponseObject<IEnumerable<SubPortfolioVM>>>(resultSubPortfolio);


                                    string portfolioName = MaskCnpjCpf(itemTrader.Identifier);

                                    traderSummaryVMs.Add(new API.Model.Response.v7.TraderSummaryVM()
                                    {
                                        GuidTrader = itemTrader.GuidTrader,
                                        Document = itemTrader.Identifier,
                                        Password = itemTrader.Password,
                                        Name = portfolioName,
                                        PortfolioPrincipal = resultResponsePortfolio.Value,
                                        SubPortfolios = subPortfolioVM.Value,
                                        ManualPortfolio = false,
                                        BlockedCEI = itemTrader.BlockedCei,
                                        IsAutomaticIntegration = true,
                                        TraderType = API.Model.Response.v7.TraderType.RendaVariavelAndTesouroDiretoCEI,
                                        DateLastSync = itemTrader.LastSync.ToString("dd/MM/yyyy")
                                    });
                                }
                                else if (itemTrader.TraderTypeID.Equals((int)TraderTypeEnum.Passfolio))
                                {
                                    ResultResponseObject<API.Model.Response.v7.PortfolioVM> resultResponsePortfolio = _mapper.Map<ResultResponseObject<API.Model.Response.v7.PortfolioVM>>(resultPortfolio);

                                    ResultServiceObject<IEnumerable<ProductUserView>> productUserView = _financialProductService.GetProductsByCategoryAndTrader(ProductCategoryEnum.CryptoCurrencies, itemTrader.IdTrader);

                                    if (productUserView.Value != null && productUserView.Value.Count() > 0)
                                    {
                                        resultResponsePortfolio.Value.HasCrypto = true;
                                    }


                                    ResultServiceObject<IEnumerable<SubPortfolio>> resultSubPortfolio = _subPortfolioService.GetByPortfolio(resultPortfolio.Value.IdPortfolio);

                                    ResultResponseObject<IEnumerable<SubPortfolioVM>> subPortfolioVM = _mapper.Map<ResultResponseObject<IEnumerable<SubPortfolioVM>>>(resultSubPortfolio);


                                    string portfolioName = itemTrader.Identifier;

                                    traderSummaryVMs.Add(new API.Model.Response.v7.TraderSummaryVM()
                                    {
                                        GuidTrader = itemTrader.GuidTrader,
                                        Document = itemTrader.Identifier,
                                        Password = itemTrader.Password,
                                        Name = portfolioName,
                                        PortfolioPrincipal = resultResponsePortfolio.Value,
                                        SubPortfolios = subPortfolioVM.Value,
                                        ManualPortfolio = false,
                                        BlockedCEI = itemTrader.BlockedCei,
                                        IsAutomaticIntegration = true,
                                        TraderType = API.Model.Response.v7.TraderType.Passfolio,
                                        DateLastSync = itemTrader.LastSync.ToString("dd/MM/yyyy")
                                    });
                                }
                                else if (itemTrader.TraderTypeID.Equals((int)TraderTypeEnum.Avenue))
                                {
                                    ResultResponseObject<API.Model.Response.v7.PortfolioVM> resultResponsePortfolio = _mapper.Map<ResultResponseObject<API.Model.Response.v7.PortfolioVM>>(resultPortfolio);

                                    ResultServiceObject<IEnumerable<SubPortfolio>> resultSubPortfolio = _subPortfolioService.GetByPortfolio(resultPortfolio.Value.IdPortfolio);

                                    ResultResponseObject<IEnumerable<SubPortfolioVM>> subPortfolioVM = _mapper.Map<ResultResponseObject<IEnumerable<SubPortfolioVM>>>(resultSubPortfolio);

                                    string portfolioName = itemTrader.Identifier;

                                    traderSummaryVMs.Add(new API.Model.Response.v7.TraderSummaryVM()
                                    {
                                        GuidTrader = itemTrader.GuidTrader,
                                        Document = itemTrader.Identifier,
                                        Password = itemTrader.Password,
                                        Name = portfolioName,
                                        PortfolioPrincipal = resultResponsePortfolio.Value,
                                        SubPortfolios = subPortfolioVM.Value,
                                        ManualPortfolio = false,
                                        BlockedCEI = itemTrader.BlockedCei,
                                        IsAutomaticIntegration = true,
                                        TraderType = API.Model.Response.v7.TraderType.Avenue,
                                        DateLastSync = itemTrader.LastSync.ToString("dd/MM/yyyy"),
                                        SyncDelayed = itemTrader.LastSync.Date < DateTime.Now.Date ? true : false
                                    });
                                }
                                else if (itemTrader.TraderTypeID.Equals((int)TraderTypeEnum.Toro))
                                {
                                    ResultResponseObject<API.Model.Response.v7.PortfolioVM> resultResponsePortfolio = _mapper.Map<ResultResponseObject<API.Model.Response.v7.PortfolioVM>>(resultPortfolio);

                                    ResultServiceObject<IEnumerable<SubPortfolio>> resultSubPortfolio = _subPortfolioService.GetByPortfolio(resultPortfolio.Value.IdPortfolio);

                                    ResultResponseObject<IEnumerable<SubPortfolioVM>> subPortfolioVM = _mapper.Map<ResultResponseObject<IEnumerable<SubPortfolioVM>>>(resultSubPortfolio);

                                    string portfolioName = itemTrader.Identifier;

                                    traderSummaryVMs.Add(new API.Model.Response.v7.TraderSummaryVM()
                                    {
                                        GuidTrader = itemTrader.GuidTrader,
                                        Document = itemTrader.Identifier,
                                        Password = itemTrader.Password,
                                        Name = portfolioName,
                                        PortfolioPrincipal = resultResponsePortfolio.Value,
                                        SubPortfolios = subPortfolioVM.Value,
                                        ManualPortfolio = false,
                                        BlockedCEI = itemTrader.BlockedCei,
                                        IsAutomaticIntegration = true,
                                        TraderType = API.Model.Response.v7.TraderType.Toro,
                                        DateLastSync = itemTrader.LastSync.ToString("dd/MM/yyyy"),
                                        SyncDelayed = itemTrader.LastSync.Date < DateTime.Now.Date ? true : false
                                    });
                                }
                                else if (itemTrader.TraderTypeID.Equals((int)TraderTypeEnum.NuInvest))
                                {
                                    ResultResponseObject<API.Model.Response.v7.PortfolioVM> resultResponsePortfolio = _mapper.Map<ResultResponseObject<API.Model.Response.v7.PortfolioVM>>(resultPortfolio);

                                    ResultServiceObject<IEnumerable<SubPortfolio>> resultSubPortfolio = _subPortfolioService.GetByPortfolio(resultPortfolio.Value.IdPortfolio);

                                    ResultResponseObject<IEnumerable<SubPortfolioVM>> subPortfolioVM = _mapper.Map<ResultResponseObject<IEnumerable<SubPortfolioVM>>>(resultSubPortfolio);

                                    string portfolioName = itemTrader.Identifier;

                                    traderSummaryVMs.Add(new API.Model.Response.v7.TraderSummaryVM()
                                    {
                                        GuidTrader = itemTrader.GuidTrader,
                                        Document = itemTrader.Identifier,
                                        Password = itemTrader.Password,
                                        Name = portfolioName,
                                        PortfolioPrincipal = resultResponsePortfolio.Value,
                                        SubPortfolios = subPortfolioVM.Value,
                                        ManualPortfolio = false,
                                        BlockedCEI = itemTrader.BlockedCei,
                                        IsAutomaticIntegration = true,
                                        TraderType = API.Model.Response.v7.TraderType.NuInvest,
                                        DateLastSync = itemTrader.LastSync.ToString("dd/MM/yyyy"),
                                        SyncDelayed = itemTrader.LastSync.Date < DateTime.Now.Date ? true : false
                                    });
                                }
                                else if (itemTrader.TraderTypeID.Equals((int)TraderTypeEnum.Xp))
                                {
                                    ResultResponseObject<API.Model.Response.v7.PortfolioVM> resultResponsePortfolio = _mapper.Map<ResultResponseObject<API.Model.Response.v7.PortfolioVM>>(resultPortfolio);

                                    ResultServiceObject<IEnumerable<SubPortfolio>> resultSubPortfolio = _subPortfolioService.GetByPortfolio(resultPortfolio.Value.IdPortfolio);

                                    ResultResponseObject<IEnumerable<SubPortfolioVM>> subPortfolioVM = _mapper.Map<ResultResponseObject<IEnumerable<SubPortfolioVM>>>(resultSubPortfolio);

                                    string portfolioName = itemTrader.Identifier;

                                    traderSummaryVMs.Add(new API.Model.Response.v7.TraderSummaryVM()
                                    {
                                        GuidTrader = itemTrader.GuidTrader,
                                        Document = itemTrader.Identifier,
                                        Password = itemTrader.Password,
                                        Name = portfolioName,
                                        PortfolioPrincipal = resultResponsePortfolio.Value,
                                        SubPortfolios = subPortfolioVM.Value,
                                        ManualPortfolio = false,
                                        BlockedCEI = itemTrader.BlockedCei,
                                        IsAutomaticIntegration = true,
                                        TraderType = API.Model.Response.v7.TraderType.Xp,
                                        DateLastSync = itemTrader.LastSync.ToString("dd/MM/yyyy"),
                                        SyncDelayed = itemTrader.LastSync.Date < DateTime.Now.Date ? true : false
                                    });
                                }
                                else if (itemTrader.TraderTypeID.Equals((int)TraderTypeEnum.RendaVariavelAndTesouroDiretoNewCEI))
                                {
                                    ResultResponseObject<API.Model.Response.v7.PortfolioVM> resultResponsePortfolio = _mapper.Map<ResultResponseObject<API.Model.Response.v7.PortfolioVM>>(resultPortfolio);

                                    ResultServiceObject<IEnumerable<SubPortfolio>> resultSubPortfolio = _subPortfolioService.GetByPortfolio(resultPortfolio.Value.IdPortfolio);

                                    ResultResponseObject<IEnumerable<SubPortfolioVM>> subPortfolioVM = _mapper.Map<ResultResponseObject<IEnumerable<SubPortfolioVM>>>(resultSubPortfolio);


                                    string portfolioName = MaskCnpjCpf(itemTrader.Identifier);

                                    traderSummaryVMs.Add(new API.Model.Response.v7.TraderSummaryVM()
                                    {
                                        GuidTrader = itemTrader.GuidTrader,
                                        Document = itemTrader.Identifier,
                                        Password = itemTrader.Password,
                                        Name = portfolioName,
                                        PortfolioPrincipal = resultResponsePortfolio.Value,
                                        SubPortfolios = subPortfolioVM.Value,
                                        ManualPortfolio = false,
                                        BlockedCEI = itemTrader.BlockedCei,
                                        IsAutomaticIntegration = true,
                                        TraderType = API.Model.Response.v7.TraderType.RendaVariavelAndTesouroDiretoNewCEI,
                                        DateLastSync = itemTrader.LastSync.ToString("dd/MM/yyyy")
                                    });
                                }
                                else if (itemTrader.TraderTypeID.Equals((int)TraderTypeEnum.Rico))
                                {
                                    ResultResponseObject<API.Model.Response.v7.PortfolioVM> resultResponsePortfolio = _mapper.Map<ResultResponseObject<API.Model.Response.v7.PortfolioVM>>(resultPortfolio);

                                    ResultServiceObject<IEnumerable<SubPortfolio>> resultSubPortfolio = _subPortfolioService.GetByPortfolio(resultPortfolio.Value.IdPortfolio);

                                    ResultResponseObject<IEnumerable<SubPortfolioVM>> subPortfolioVM = _mapper.Map<ResultResponseObject<IEnumerable<SubPortfolioVM>>>(resultSubPortfolio);

                                    string portfolioName = itemTrader.Identifier;

                                    traderSummaryVMs.Add(new API.Model.Response.v7.TraderSummaryVM()
                                    {
                                        GuidTrader = itemTrader.GuidTrader,
                                        Document = itemTrader.Identifier,
                                        Password = itemTrader.Password,
                                        Name = portfolioName,
                                        PortfolioPrincipal = resultResponsePortfolio.Value,
                                        SubPortfolios = subPortfolioVM.Value,
                                        ManualPortfolio = false,
                                        BlockedCEI = itemTrader.BlockedCei,
                                        IsAutomaticIntegration = true,
                                        TraderType = API.Model.Response.v7.TraderType.Rico,
                                        DateLastSync = itemTrader.LastSync.ToString("dd/MM/yyyy"),
                                        SyncDelayed = itemTrader.LastSync.Date < DateTime.Now.Date ? true : false
                                    });
                                }
                                else if (itemTrader.TraderTypeID.Equals((int)TraderTypeEnum.Clear))
                                {
                                    ResultResponseObject<API.Model.Response.v7.PortfolioVM> resultResponsePortfolio = _mapper.Map<ResultResponseObject<API.Model.Response.v7.PortfolioVM>>(resultPortfolio);

                                    ResultServiceObject<IEnumerable<SubPortfolio>> resultSubPortfolio = _subPortfolioService.GetByPortfolio(resultPortfolio.Value.IdPortfolio);

                                    ResultResponseObject<IEnumerable<SubPortfolioVM>> subPortfolioVM = _mapper.Map<ResultResponseObject<IEnumerable<SubPortfolioVM>>>(resultSubPortfolio);

                                    string portfolioName = itemTrader.Identifier;

                                    traderSummaryVMs.Add(new API.Model.Response.v7.TraderSummaryVM()
                                    {
                                        GuidTrader = itemTrader.GuidTrader,
                                        Document = itemTrader.Identifier,
                                        Password = itemTrader.Password,
                                        Name = portfolioName,
                                        PortfolioPrincipal = resultResponsePortfolio.Value,
                                        SubPortfolios = subPortfolioVM.Value,
                                        ManualPortfolio = false,
                                        BlockedCEI = itemTrader.BlockedCei,
                                        IsAutomaticIntegration = true,
                                        TraderType = API.Model.Response.v7.TraderType.Clear,
                                        DateLastSync = itemTrader.LastSync.ToString("dd/MM/yyyy"),
                                        SyncDelayed = itemTrader.LastSync.Date < DateTime.Now.Date ? true : false
                                    });
                                }
                            }
                        }
                    }
                }

                if (tradersManual != null && tradersManual.Count() > 0)
                {
                    foreach (Trader itemTrader in tradersManual)
                    {
                        if (itemTrader.TraderTypeID.Equals((int)TraderTypeEnum.RendaVariavelManual))
                        {
                            List<SubPortfolioVM> manualSubPortfolios = new List<SubPortfolioVM>();

                            ResultServiceObject<Portfolio> resultPortfolio = _portfolioService.GetByTraderActive(itemTrader.IdTrader);

                            if (resultPortfolio.Value != null)
                            {
                                if (resultPortfolio.Value.IdCountry.Equals((int)CountryEnum.EUA) && !hasSubscription)
                                {
                                    continue;
                                }

                                ResultResponseObject<API.Model.Response.v7.PortfolioVM> resultResponsePortfolio = _mapper.Map<ResultResponseObject<API.Model.Response.v7.PortfolioVM>>(resultPortfolio);

                                ResultServiceObject<IEnumerable<SubPortfolio>> resultSubPortfolio = _subPortfolioService.GetByPortfolio(resultPortfolio.Value.IdPortfolio);

                                if (resultSubPortfolio.Success && resultSubPortfolio.Value != null && resultSubPortfolio.Value.Count() > 0)
                                {
                                    foreach (SubPortfolio subPortfolio in resultSubPortfolio.Value)
                                    {
                                        SubPortfolioVM subPortfolioVM = new SubPortfolioVM();
                                        subPortfolioVM.GuidPortfolio = resultPortfolio.Value.GuidPortfolio;
                                        subPortfolioVM.Name = subPortfolio.Name;
                                        subPortfolioVM.GuidSubPortfolio = subPortfolio.GuidSubPortfolio;

                                        manualSubPortfolios.Add(subPortfolioVM);
                                    }
                                }

                                traderSummaryVMs.Add(new API.Model.Response.v7.TraderSummaryVM()
                                {
                                    GuidTrader = Guid.Empty,
                                    Document = "",
                                    Password = "",
                                    Name = "Carteira Manual",
                                    PortfolioPrincipal = resultResponsePortfolio.Value,
                                    SubPortfolios = manualSubPortfolios,
                                    BlockedCEI = false,
                                    ManualPortfolio = true,
                                    IsAutomaticIntegration = false,
                                    TraderType = API.Model.Response.v7.TraderType.RendaVariavelManual
                                });
                            }
                        }
                        else if (itemTrader.TraderTypeID.Equals((int)TraderTypeEnum.CryptoManual))
                        {
                            CryptoPortfolio crypto = _cryptoPortfolioService.GetByIdTrader(itemTrader.IdTrader).Value;

                            if (crypto != null)
                            {
                                List<SubPortfolioVM> manualSubPortfolios = new List<SubPortfolioVM>();
                                API.Model.Response.v7.PortfolioVM portfolioVM = new API.Model.Response.v7.PortfolioVM();
                                portfolioVM.GuidPortfolio = crypto.GuidCryptoPortfolio;
                                portfolioVM.IdCountry = crypto.IdFiatCurrency;
                                portfolioVM.ManualPortfolio = true;
                                portfolioVM.Name = crypto.Name;

                                ResultServiceObject<IEnumerable<CryptoSubPortfolio>> resultCryptoSubPortfolios = _cryptoSubPortfolioService.GetByIdCryptoPortfolio(crypto.IdCryptoPortfolio);
                                
                                if (resultCryptoSubPortfolios != null && resultCryptoSubPortfolios.Value.Count() > 0)
                                {
                                    foreach (CryptoSubPortfolio cryptoSubPortfolio in resultCryptoSubPortfolios.Value)
                                    {
                                        SubPortfolioVM subPortfolioVM = new SubPortfolioVM();
                                        subPortfolioVM.GuidPortfolio = crypto.GuidCryptoPortfolio;
                                        subPortfolioVM.GuidSubPortfolio = cryptoSubPortfolio.GuidCryptoSubPortfolio;
                                        subPortfolioVM.Name = cryptoSubPortfolio.Name;
                                        manualSubPortfolios.Add(subPortfolioVM);
                                    }
                                }

                                traderSummaryVMs.Add(new API.Model.Response.v7.TraderSummaryVM()
                                {
                                    GuidTrader = itemTrader.GuidTrader,
                                    Document = "",
                                    Password = "",
                                    Name = "Cripto Manual",
                                    PortfolioPrincipal = portfolioVM,
                                    SubPortfolios = manualSubPortfolios,
                                    BlockedCEI = false,
                                    ManualPortfolio = true,
                                    IsAutomaticIntegration = false,
                                    TraderType = API.Model.Response.v7.TraderType.CryptoManual
                                });
                            }
                        }
                    }
                }
            }

            ResultResponseObject<IEnumerable<API.Model.Response.v7.TraderSummaryVM>> resultResponseDomain = new ResultResponseObject<IEnumerable<API.Model.Response.v7.TraderSummaryVM>>() { Value = traderSummaryVMs };
            resultResponseDomain.Success = true;

            return resultResponseDomain;
        }

        public ResultResponseObject<TraderVM> CreateNewB3Trader(string documentNumber)
        {
            ResultResponseObject<TraderVM> resultReponse = new ResultResponseObject<TraderVM>();

            using (_uow.Create())
            {
                Trader trader = _traderService.GetByIdentifierAndUser(documentNumber, _globalAuthenticationService.IdUser, TraderTypeEnum.RendaVariavelAndTesouroDiretoNewCEI).Value;

                if (trader == null)
                {
                    trader = new Trader();
                    trader.Identifier = documentNumber;
                    trader.Active = false;
                    trader.BlockedCei = false;
                    trader.ManualPortfolio = false;
                    trader.Password = string.Empty;
                    trader.IdUser = _globalAuthenticationService.IdUser;
                    trader.TraderTypeID = (int)TraderTypeEnum.RendaVariavelAndTesouroDiretoNewCEI;

                    trader = _traderService.Insert(trader).Value;
                }

                TraderVM traderVM = new TraderVM();
                traderVM.Active = trader.Active;
                traderVM.GuidTrader = trader.GuidTrader;
                traderVM.Identifier = trader.Identifier;
                traderVM.Password = trader.Password;
                traderVM.TraderTypeID = (int)trader.TraderTypeID;

                resultReponse.Value = traderVM;
                resultReponse.Success = true;

            }

            return resultReponse;
        }
    }
}