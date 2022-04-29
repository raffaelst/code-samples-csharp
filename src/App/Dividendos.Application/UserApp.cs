using AutoMapper;
using Dividendos.API.Model.Request;
using Dividendos.API.Model.Response.Common;
using Dividendos.Application.Base;
using Dividendos.Application.Interface;
using Dividendos.CrossCutting.Identity.Models;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Repository.Interface.UoW;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Dividendos.API.Model.Messages;
using Dividendos.Service.Interface;
using System;
using K.Logger;
using Dividendos.Entity.Views;
using Dividendos.Application.Interface.Model;
using System.Globalization;
using System.Text;
using Newtonsoft.Json;
using Dividendos.Entity.Enum;
using Dividendos.RDStation.Interface;
using Dividendos.API.Model.Response;

namespace Dividendos.Application
{
    public class UserApp : BaseApp, IUserApp
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        private readonly IUnitOfWork _uow;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotificationService _notificationService;
        private readonly IGlobalAuthenticationService _globalAuthenticationService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly int _emailWelcomeTemplateId = 1;
        private readonly int _emailStatisticsTemplateId = 2;
        private readonly int _emailRecoveryPassTemplateId = 6;
        private readonly ILogger _logger;
        private readonly IPortfolioService _portfolioService;
        private readonly ISettingsService _settingsService;
        private readonly IDeviceService _deviceService;
        private readonly INotificationHistoricalService _notificationHistoricalService;
        private readonly ICacheService _cacheService;
        private readonly IFinancialProductService _financialProductService;
        private readonly ICryptoCurrencyService _cryptoCurrencyService;
        private readonly ISystemSettingsService _systemSettingsService;
        private readonly IPortfolioPerformanceService _portfolioPerformanceService;
        private readonly IStockService _stockService;
        private readonly IOperationService _operationService;
        private readonly IPerformanceStockService _performanceStockService;
        private readonly IHolidayService _holidayService;
        private readonly IRDStationHelper _rDStationHelper;
        private readonly ITraderService _traderService;
        private readonly ISubPortfolioService _subPortfolioService;
        private readonly ICryptoSubPortfolioService _cryptoSubPortfolioService;
        private readonly ICryptoPortfolioService _cryptoPortfolioService;

        public UserApp(IUserService userService,
            IMapper mapper,
            IUnitOfWork uow,
            UserManager<ApplicationUser> userManager,
            INotificationService notificationService,
            SignInManager<ApplicationUser> signInManager,
            IGlobalAuthenticationService globalAuthenticationService,
            IEmailTemplateService emailTemplateService,
            ILogger logger,
            IPortfolioService portfolioService,
            IDeviceService deviceService,
            ISettingsService settingsService,
            INotificationHistoricalService notificationHistoricalService,
            ICacheService cacheService,
            IFinancialProductService financialProductService,
            ICryptoCurrencyService cryptoCurrencyService,
            ISystemSettingsService systemSettingsService,
            IPortfolioPerformanceService portfolioPerformanceService,
            IStockService stockService,
            IOperationService operationService,
            IPerformanceStockService performanceStockService,
            IHolidayService holidayService,
            IRDStationHelper rDStationHelper,
            ITraderService traderService,
            ISubPortfolioService subPortfolioService,
            ICryptoSubPortfolioService cryptoSubPortfolioService,
            ICryptoPortfolioService cryptoPortfolioService)
        {
            _userService = userService;
            _mapper = mapper;
            _uow = uow;
            _userManager = userManager;
            _signInManager = signInManager;
            _notificationService = notificationService;
            _globalAuthenticationService = globalAuthenticationService;
            _emailTemplateService = emailTemplateService;
            _logger = logger;
            _portfolioService = portfolioService;
            _deviceService = deviceService;
            _settingsService = settingsService;
            _notificationHistoricalService = notificationHistoricalService;
            _cacheService = cacheService;
            _financialProductService = financialProductService;
            _cryptoCurrencyService = cryptoCurrencyService;
            _systemSettingsService = systemSettingsService;
            _portfolioPerformanceService = portfolioPerformanceService;
            _stockService = stockService;
            _operationService = operationService;
            _performanceStockService = performanceStockService;
            _holidayService = holidayService;
            _rDStationHelper = rDStationHelper;
            _traderService = traderService;
            _subPortfolioService = subPortfolioService;
            _cryptoSubPortfolioService = cryptoSubPortfolioService;
            _cryptoPortfolioService = cryptoPortfolioService;
        }

        public ResultResponseObject<UserVM> GetAccountDetails()
        {
            ResultResponseObject<UserVM> resultResponse;

            using (_uow.Create())
            {
                ResultServiceObject<ApplicationUser> resultServiceAspNetUser = _userService.GetAccountDetails(_globalAuthenticationService.IdUser);
                resultResponse = _mapper.Map<ResultResponseObject<UserVM>>(resultServiceAspNetUser);
            }

            return resultResponse;
        }

        public ResultResponseObject<Dividendos.API.Model.Response.v3.UserVM> GetAccountDetailsWithNotificationAmount()
        {
            ResultResponseObject<Dividendos.API.Model.Response.v3.UserVM> resultResponse = null;

            using (_uow.Create())
            {
                ResultServiceObject<ApplicationUser> resultServiceAspNetUser = _userService.GetAccountDetails(_globalAuthenticationService.IdUser);
                resultResponse = _mapper.Map<ResultResponseObject<Dividendos.API.Model.Response.v3.UserVM>>(resultServiceAspNetUser);
                var resultNotificationAmount = _userService.GetNotificationAmount(_globalAuthenticationService.IdUser);
                resultResponse.Value.NotificationAmount = resultNotificationAmount.Value;
                _userService.UpdateLastAccessDate(_globalAuthenticationService.IdUser);
            }

            return resultResponse;
        }

        public ResultResponseBase DeleteAccount(string userID)
        {
            ResultResponseBase resultResponseBase = new ResultResponseBase() { Success = false };

            using (_uow.Create())
            {
                if (string.IsNullOrEmpty(userID))
                {
                    userID = _globalAuthenticationService.IdUser;

                    _userService.ExcludeAccount(_globalAuthenticationService.IdUser);
                }
                else
                {
                    _userService.ExcludeAccount(userID);
                }

                var traders = _traderService.GetByUserActive(_globalAuthenticationService.IdUser);

                foreach (var itemTrader in traders.Value)
                {
                    _traderService.DisableCascade(itemTrader.IdTrader, _portfolioService, _subPortfolioService, _financialProductService, _cryptoPortfolioService, _cryptoSubPortfolioService);
                }

                resultResponseBase = new ResultResponseBase() { Success = true };
            }

            return resultResponseBase;
        }

        public ResultResponseStringModel RegisterNewUserFromAffiliate(UserRegisterAffiliateVM userRegisterAffiliate)
        {
            UserRegisterVM userRegister = new UserRegisterVM();
            userRegister.Email = userRegisterAffiliate.Email;
            userRegister.Name = userRegisterAffiliate.Name;
            userRegister.Password = userRegisterAffiliate.Password;
            var user = RegisterNewUserBase(userRegister, userRegisterAffiliate.AffiliateGuid);

            return user;
        }


        public ResultResponseStringModel RegisterNewUser(UserRegisterVM userRegister)
        {
            ResultResponseStringModel resultResponseStringModel = new ResultResponseStringModel();

            resultResponseStringModel = RegisterNewUserBase(userRegister, null);

            return resultResponseStringModel;
        }

        private ResultResponseStringModel RegisterNewUserBase(UserRegisterVM userRegister, string influencerAffiliator)
        {
            ResultResponseStringModel resultResponseStringModel = new ResultResponseStringModel();

            userRegister.Email = userRegister.Email.Trim();

            ApplicationUser aspNetUsers = _mapper.Map<ApplicationUser>(userRegister);
            aspNetUsers.InfluencerAffiliatorGuid = influencerAffiliator;
            aspNetUsers.Created = DateTime.Now;

            using (_uow.Create())
            {
                ResultServiceObject<ApplicationUser> resultServiceAspNetUser = _userService.GetByEmail(userRegister.Email, true);

                if (resultServiceAspNetUser.Success && resultServiceAspNetUser.Value == null)
                {
                    var result = _userManager.CreateAsync(aspNetUsers, userRegister.Password).Result;

                    if (result.Succeeded)
                    {
                        ApplicationUser appUser = _userManager.Users.SingleOrDefault(r => r.Email == userRegister.Email && (r.Excluded == null || r.Excluded == false));

                        //Create settings
                        _settingsService.InitAndCreate(appUser.Id);

                        resultResponseStringModel.Success = true;

                        _rDStationHelper.SendEvent(userRegister.Name, userRegister.Email, appUser.Id, null, userRegister.PhoneNumber, RDStation.Interface.Model.Request.EventType.AccountCreated);
                    }
                    else
                    {
                        resultResponseStringModel.ErrorMessages.Add("Falha ao criar o usuário, tente novamente mais tarde");
                    }
                }
                else if (!resultServiceAspNetUser.Value.Excluded.HasValue || resultServiceAspNetUser.Value.Excluded.Value == false)
                {
                    resultResponseStringModel.ErrorMessages.Add("E-mail já informado. Recupere a senha para ter acesso.");
                }
                else
                {
                    _userService.ReactivateAccount(resultServiceAspNetUser.Value.Id);
                    _userService.UpdateUserNameAndPhoneNumber(resultServiceAspNetUser.Value.Id, userRegister.Name, userRegister.PhoneNumber);
                    _userService.UpdatePassword(resultServiceAspNetUser.Value.Id, _userManager.PasswordHasher.HashPassword(resultServiceAspNetUser.Value, userRegister.Password));
                    resultResponseStringModel.Success = true;
                }
            }


            return resultResponseStringModel;
        }

        public ResultResponseObject<IEnumerable<ApplicationUser>> GetUserByMail(string email)
        {
            ResultServiceObject<IEnumerable<ApplicationUser>> resultServiceAspNetUser = null;

            using (_uow.Create())
            {
                resultServiceAspNetUser = _userService.GetAccountPerEmail(email);
            }

            ResultResponseObject<IEnumerable<ApplicationUser>> resultResponseObject = _mapper.Map<ResultResponseObject<IEnumerable<ApplicationUser>>>(resultServiceAspNetUser);

            return resultResponseObject;
        }

        public ResultResponseStringModel RecoveryPassword(string email)
        {
            ResultResponseStringModel resultResponseStringModel = new ResultResponseStringModel();

            email = email.Trim();

            ApplicationUser appUser = _userManager.Users.SingleOrDefault(r => r.Email == email && (r.Excluded == null || r.Excluded == false));

            if (appUser != null)
            {
                //var result = _userManager.GeneratePasswordResetTokenAsync(appUser);
                Random generator = new Random();
                string code = generator.Next(0, 999999).ToString("D6");
                string mailTemplate;

                using (_uow.Create())
                {
                    _userService.SaveRecoveryPasswordToken(appUser.Id, code);
                    mailTemplate = _emailTemplateService.GetById(_emailRecoveryPassTemplateId).Value.Template;
                }

                _notificationService.SendMail(email, "Recuperação de senha", string.Format(mailTemplate, code));
            }

            resultResponseStringModel.Success = true;

            return resultResponseStringModel;
        }

        public ResultResponseStringModel ResetPassword(string email, string token, string newPassword)
        {
            ResultResponseStringModel resultResponseStringModel = new ResultResponseStringModel();

            email = email.Trim();

            ApplicationUser appUser = _userManager.Users.SingleOrDefault(r => r.Email == email);

            if (!string.IsNullOrEmpty(appUser.RecoveryPasswordToken) && appUser.RecoveryPasswordToken.Equals(token))
            {
                resultResponseStringModel.Success = true;

                using (_uow.Create())
                {
                    //_userService.SaveRecoveryPasswordToken(appUser.Id, string.Empty);
                    _userService.UpdatePassword(appUser.Id, _userManager.PasswordHasher.HashPassword(appUser, newPassword));
                }

            }
            else
            {
                resultResponseStringModel.ErrorMessages.Add(AuthMessage.InvalidRefreshToken);

                appUser = null;
            }


            return resultResponseStringModel;
        }

        public ResultResponseStringModel ChangePassword(string currentPassword, string newPassword)
        {
            ResultResponseStringModel resultResponseStringModel = new ResultResponseStringModel();

            using (_uow.Create())
            {
                ResultServiceObject<ApplicationUser> resultServiceAspNetUser = _userService.GetByID(_globalAuthenticationService.IdUser);

                var passValidation = _signInManager.PasswordSignInAsync(resultServiceAspNetUser.Value.Email, currentPassword, false, true).Result;

                if (!passValidation.Succeeded)
                {
                    resultResponseStringModel.ErrorMessages.Add(AuthMessage.InvalidLogin);
                }
                else
                {
                    _userService.UpdatePassword(resultServiceAspNetUser.Value.Id, _userManager.PasswordHasher.HashPassword(resultServiceAspNetUser.Value, newPassword));

                    resultResponseStringModel.Success = true;
                }
            }

            return resultResponseStringModel;
        }

        public void SendEmailWithDailyStatistics()
        {

            _logger.SendDebugAsync(new { JobDebugInfo = "Iniciando SendEmailWithDailyStatistics" });

            ResultServiceObject<IEnumerable<ApplicationUser>> users;
            ResultServiceObject<EmailTemplate> emailBody;


            //Get trader that set auto sync
            using (_uow.Create())
            {
                users = _userService.GetAllUserWithSendDailySummaryMailEnable();
                emailBody = _emailTemplateService.GetById(_emailStatisticsTemplateId);
            }

            //send push

            foreach (var itemUser in users.Value)
            {
                string userName = itemUser.Name;
                StringBuilder walletsSummary = new StringBuilder();

                StockTypeChart financialProductsChart = new StockTypeChart();

                financialProductsChart.ListChartLabelValue = new List<ChartLabelValue>();
                decimal totalCryptos = 0;

                using (_uow.Create())
                {
                    ResultServiceObject<IEnumerable<PortfolioView>> result = _portfolioService.GetByUser(itemUser.Id);

                    var resultCryptosService = _financialProductService.GetAllProductsByUserAndType(itemUser.Id, ProductCategoryEnum.CryptoCurrencies);
                    ResultServiceObject<IEnumerable<CryptoCurrency>> cryptoCurrencies = _cryptoCurrencyService.GetAll();

                    if (result.Success && result.Value != null && result.Value.Count() > 0)
                    {
                        foreach (PortfolioView portfolioView in result.Value)
                        {
                            if (portfolioView.IsPortfolio)
                            {
                                _portfolioService.CalculatePerformance(portfolioView.IdPortfolio, _systemSettingsService, _portfolioPerformanceService, _stockService, _operationService, _performanceStockService, _holidayService);
                            }

                            decimal perc = portfolioView.PerformancePerc * 100;
                            decimal percTwr = portfolioView.PerformancePercTWR * 100;

                            string walletName = portfolioView.Name;
                            string date = portfolioView.CalculationDate.ToString("dd/MM");
                            string latestNetValue = GetSignal(portfolioView.LatestNetValue) + portfolioView.LatestNetValue.ToString("n2", new CultureInfo("pt-br"));
                            string netValue = GetSignal(portfolioView.NetValue) + portfolioView.NetValue.ToString("n2", new CultureInfo("pt-br"));
                            string totalMarket = portfolioView.TotalMarket.ToString("n2", new CultureInfo("pt-br"));
                            string total = portfolioView.Total.ToString("n2", new CultureInfo("pt-br"));
                            string performancePerc = GetSignal(perc) + perc.ToString("n2", new CultureInfo("pt-br"));
                            string performancePercTWR = GetSignal(percTwr) + percTwr.ToString("n2", new CultureInfo("pt-br"));
                            string currencySymbol = "R$";

                            if (portfolioView.IdCountry.Equals((int)CountryEnum.EUA))
                            {
                                currencySymbol = "$";
                            }

                            walletsSummary.Append(string.Format(@"<br>
																		<br>
																		Carteira {0} em {1}
																		<ul>
																		
																		  
																		  <li style='margin:0;margin-bottom:10px;color:#888;font-family:'Gotham SSm','Helvetica Neue',Helvetica,Arial,sans-serif;font-weight:400;padding:0;text-align:left;line-height:20px;font-size:16px'>Valor investido: {8} {2} - Valor atual: {8} {3}</li>
																		  
																		  <li style='margin:0;margin-bottom:10px;color:#888;font-family:'Gotham SSm','Helvetica Neue',Helvetica,Arial,sans-serif;font-weight:400;padding:0;text-align:left;line-height:20px;font-size:16px'>Rentabilidade: {8} {4} - Rentabilidade: {5} %</li>
																		  
																		  <li style='margin:0;margin-bottom:10px;color:#888;font-family:'Gotham SSm','Helvetica Neue',Helvetica,Arial,sans-serif;font-weight:400;padding:0;text-align:left;line-height:20px;font-size:16px'>Rentabilidade Diária: {8} {6} - Rentabilidade Diária: {7} %</li>
																		 
																		</ul> ", walletName, date, total, totalMarket, netValue, performancePerc, latestNetValue, performancePercTWR, currencySymbol));
                        }


                        //Criptos
                        if (resultCryptosService.Value != null && resultCryptosService.Value.Count() > 0)
                        {
                            foreach (var itemProductUser in resultCryptosService.Value)
                            {
                                var itemFounded = financialProductsChart.ListChartLabelValue.FirstOrDefault(item => item.Label.Equals(itemProductUser.ProductName));

                                if (itemFounded == null)
                                {
                                    ChartLabelValue dataItem = new ChartLabelValue() { Label = itemProductUser.ProductName, InternalLabel = itemProductUser.ExternalName, InternalValue = itemProductUser.CurrentValue };
                                    financialProductsChart.ListChartLabelValue.Add(dataItem);
                                }
                                else
                                {
                                    itemFounded.InternalValue = itemProductUser.CurrentValue + itemFounded.InternalValue;
                                }
                            }

                            walletsSummary.Append(string.Format(@"<br>
														<br>
														Carteira de cripto ativos em {0}
														<ul>", DateTime.Now.ToString("dd/MM")));

                            foreach (var itemChart in financialProductsChart.ListChartLabelValue)
                            {
                                var crypto = cryptoCurrencies.Value.Where(productUser => productUser.Name.Equals(itemChart.InternalLabel)).FirstOrDefault();

                                var currentValue = itemChart.InternalValue * crypto.MarketPrice;

                                totalCryptos += currentValue;

                                itemChart.Value = currentValue.ToString("n2", new CultureInfo("pt-BR"));

                                walletsSummary.Append(string.Format(@"<li style='margin:0;margin-bottom:10px;color:#888;font-family:'Gotham SSm','Helvetica Neue',Helvetica,Arial,sans-serif;font-weight:400;padding:0;text-align:left;line-height:20px;font-size:16px'>{0}: R$ {1} ({2} Unidades)</li>", itemChart.Label, itemChart.Value, itemChart.InternalValue.ToString("n8", new CultureInfo("pt-BR"))));
                            }

                            walletsSummary.Append(string.Format(@"
                                                        </ul>                                                        
                                                        <br>
														Total investido em cripto ativos: R$ {0}", totalCryptos.ToString("n2", new CultureInfo("pt-br"))));
                        }

                        string body = string.Format(emailBody.Value.Template, userName, walletsSummary);

                        try
                        {
                            _notificationService.SendMail(itemUser.Email, "Seu resumo diário do App Dividendos", body);
                        }
                        catch (Exception exception)
                        {
                            _logger.SendErrorAsync(exception);
                        }
                    }
                }
            }


            _logger.SendDebugAsync(new { JobDebugInfo = "finalizando SendEmailWithDailyStatistics" });
        }

        public void SendPushWithDailyStatistics()
        {
            _logger.SendDebugAsync(new { JobDebugInfo = "Iniciando SendPushWithDailyStatistics" });

            ResultServiceObject<IEnumerable<ApplicationUser>> users;

            //Get trader that set auto sync
            using (_uow.Create())
            {
                users = _userService.GetAllUserWithPortfolioActive();
            }

            foreach (var itemUser in users.Value)
            {
                string userName = itemUser.Name;
                StringBuilder walletsSummary = new StringBuilder();

                StockTypeChart financialProductsChart = new StockTypeChart();

                financialProductsChart.ListChartLabelValue = new List<ChartLabelValue>();

                using (_uow.Create())
                {
                    ResultServiceObject<IEnumerable<PortfolioView>> result = _portfolioService.GetByUser(itemUser.Id);

                    if (result.Success && result.Value != null && result.Value.Count() > 0)
                    {
                        int amountOfPortfoliosWithValorization = 0;

                        foreach (PortfolioView portfolioView in result.Value)
                        {
                            if (portfolioView.IsPortfolio)
                            {
                                _portfolioService.CalculatePerformance(portfolioView.IdPortfolio, _systemSettingsService, _portfolioPerformanceService, _stockService, _operationService, _performanceStockService, _holidayService);
                            }

                            decimal perc = portfolioView.PerformancePerc * 100;
                            decimal percTwr = portfolioView.PerformancePercTWR * 100;

                            string walletName = portfolioView.Name;
                            string latestNetValue = GetSignal(portfolioView.LatestNetValue) + portfolioView.LatestNetValue.ToString("n2", new CultureInfo("pt-br"));
                            string netValue = GetSignal(portfolioView.NetValue) + portfolioView.NetValue.ToString("n2", new CultureInfo("pt-br"));
                            string totalMarket = portfolioView.TotalMarket.ToString("n2", new CultureInfo("pt-br"));
                            string total = portfolioView.Total.ToString("n2", new CultureInfo("pt-br"));
                            string performancePerc = GetSignal(perc) + perc.ToString("n2", new CultureInfo("pt-br"));
                            string performancePercTWR = GetSignal(percTwr) + percTwr.ToString("n2", new CultureInfo("pt-br"));
                            string currencySymbol = "R$";

                            if (portfolioView.IdCountry.Equals((int)CountryEnum.EUA))
                            {
                                currencySymbol = "$";
                            }

                            if (percTwr >= decimal.Parse("0.3"))
                            {
                                amountOfPortfoliosWithValorization++;
                                walletsSummary.Append(string.Format(@"{0} se valorizou {1}% ({2} {3}) ", walletName, performancePercTWR, currencySymbol, latestNetValue));
                            }
                        }

                        if (amountOfPortfoliosWithValorization != 0)
                        {
                            string pushMessageTitle = "O App Dividendos.me tem uma boa notícia para terminar o dia!";

                            string message = "As seguintes carteiras tiveram valorização hoje: {0}";

                            if (amountOfPortfoliosWithValorization == 1)
                            {
                                message = "Esta carteira teve valorização hoje: {0}";
                            }

                            string pushMessage = string.Format(message, walletsSummary);

                            ResultServiceObject<IEnumerable<Device>> devices = _deviceService.GetByUser(itemUser.Id);

                            _notificationHistoricalService.New(pushMessageTitle, pushMessage, itemUser.Id, AppScreenNameEnum.HomeStocks.ToString(), PushRedirectTypeEnum.Internal.ToString(), null, _cacheService);

                            foreach (Device itemDevice in devices.Value)
                            {
                                try
                                {
                                    _notificationService.SendPush(pushMessageTitle, pushMessage, itemDevice, new PushRedirect() { PushRedirectType = PushRedirectTypeEnum.Internal, AppScreenName = AppScreenNameEnum.HomeStocks });
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


            _logger.SendDebugAsync(new { JobDebugInfo = "finalizando SendPushWithDailyStatistics" });
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

        public ResultResponseBase ChangeUserData(API.Model.Request.v2.User.UserVM userVM)
        {
            ResultResponseBase resultResponseBase = new ResultResponseBase() { Success = true } ;

            using (_uow.Create())
            {
                _userService.UpdateUserNameAndPhoneNumber(_globalAuthenticationService.IdUser, userVM.Name, userVM.PhoneNumber);
            }

            return resultResponseBase;
        }

        public ResultResponseBase ChangeUserName(string name)
        {
            ResultResponseBase resultResponseBase = new ResultResponseBase() { Success = true };

            using (_uow.Create())
            {
                _userService.UpdateUserName(_globalAuthenticationService.IdUser, name);
            }

            return resultResponseBase;
        }

        public ResultResponseObject<IEnumerable<UserVM>> GetAccountPerEmail(string email)
        {
            ResultResponseObject<IEnumerable<UserVM>> resultResponse;

            using (_uow.Create())
            {
                ResultServiceObject<IEnumerable<ApplicationUser>> resultServiceAspNetUser = _userService.GetAccountPerEmail(email);

                resultResponse = _mapper.Map<ResultResponseObject<IEnumerable<UserVM>>>(resultServiceAspNetUser);
            }

            return resultResponse;
        }

        public ResultResponseObject<IEnumerable<UserDataVM>> GetAccountPerNameEmailOrPhoneNumber(string filter)
        {
            ResultResponseObject<IEnumerable<UserDataVM>> resultResponse;

            using (_uow.Create())
            {
                ResultServiceObject<IEnumerable<ApplicationUser>> resultServiceAspNetUser = _userService.GetAccountPerNameEmailOrPhoneNumber(filter);

                resultResponse = _mapper.Map<ResultResponseObject<IEnumerable<UserDataVM>>>(resultServiceAspNetUser);
            }

            return resultResponse;
        }
    }
}