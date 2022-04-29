using AutoMapper;
using K.Logger;
using Dividendos.API.Model.Response.Common;
using Dividendos.Application.Base;
using Dividendos.Application.Interface;
using Dividendos.CrossCutting.Identity.Models;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Newtonsoft.Json;
using Dividendos.API.Model.Request.Dividend;
using System.Data.SqlTypes;
using Dividendos.API.Model.Response;
using System.Text;
using Dividendos.Entity.Enum;
using Dividendos.Clear.Interface;
using Dividendos.Rico.Interface;
using Dividendos.Clear.Interface.Model;
using Dividendos.Rico.Interface.Model;
using Dividendos.InvestidorB3.Interface;

namespace Dividendos.Application
{
    public class DividendApp : BaseApp, IDividendApp
    {
        private readonly IGlobalAuthenticationService _globalAuthenticationService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly IDividendService _dividendService;
        private readonly ILogger _logger;
        private readonly IDeviceService _deviceService;
        private readonly IUserService _userService;
        private readonly IStockService _stockService;
        private readonly ISettingsService _settingsService;
        private readonly IPortfolioService _portfolioService;
        private readonly ISubPortfolioService _subPortfolioService;
        private readonly INotificationService _notificationService;
        private readonly INotificationHistoricalService _notificationHistoricalService;
        private readonly ICacheService _cacheService;
        private readonly IIndicatorSeriesService _indicatorSeriesService;
        private readonly IPortfolioPerformanceService _portfolioPerformanceService;
        private readonly IHolidayService _holidayService;
        private readonly IPerformanceStockService _performanceStockService;
        private readonly IOperationItemService _operationItemService;
        private readonly IDividendCalendarService _dividendCalendarService;
        private readonly IOperationService _operationService;
        private readonly ITraderService _traderService;
        private readonly IRicoHelper _ricoHelper;
        private readonly IClearHelper _clearHelper;
        private readonly IDividendTypeService _dividendTypeService;
        private readonly IScrapySchedulerService _scrapySchedulerService;
        private readonly IImportInvestidorB3Helper _iImportInvestidorB3Helper;

        public DividendApp(
            IMapper mapper,
            IUnitOfWork uow,
            IGlobalAuthenticationService globalAuthenticationService,
            IDividendService dividendService,
            ILogger logger,
            IDeviceService deviceService,
            IUserService userService,
            IStockService stockService,
            ISettingsService settingsService,
            IPortfolioService portfolioService,
            ISubPortfolioService subPortfolioService,
            INotificationService notificationService,
            INotificationHistoricalService notificationHistoricalService,
            ICacheService cacheService,
            IIndicatorSeriesService indicatorSeriesService,
            IPortfolioPerformanceService portfolioPerformanceService,
            IHolidayService holidayService,
            IPerformanceStockService performanceStockService,
            IOperationItemService operationItemService,
            IDividendCalendarService dividendCalendarService,
            IOperationService operationService,
            ITraderService traderService,
            IRicoHelper ricoHelper,
            IClearHelper clearHelper,
            IDividendTypeService dividendTypeService,
            IScrapySchedulerService scrapySchedulerService,
            IImportInvestidorB3Helper iImportInvestidorB3Helper)
        {
            _mapper = mapper;
            _globalAuthenticationService = globalAuthenticationService;
            _uow = uow;
            _logger = logger;
            _dividendService = dividendService;
            _deviceService = deviceService;
            _userService = userService;
            _stockService = stockService;
            _settingsService = settingsService;
            _portfolioService = portfolioService;
            _subPortfolioService = subPortfolioService;
            _notificationService = notificationService;
            _notificationHistoricalService = notificationHistoricalService;
            _cacheService = cacheService;
            _indicatorSeriesService = indicatorSeriesService;
            _portfolioPerformanceService = portfolioPerformanceService;
            _holidayService = holidayService;
            _performanceStockService = performanceStockService;
            _operationItemService = operationItemService;
            _dividendCalendarService = dividendCalendarService;
            _operationService = operationService;
            _traderService = traderService;
            _ricoHelper = ricoHelper;
            _clearHelper = clearHelper;
            _dividendTypeService = dividendTypeService;
            _scrapySchedulerService = scrapySchedulerService;
            _iImportInvestidorB3Helper = iImportInvestidorB3Helper;
        }


        public void SendNotification(bool morningNotification)
        {
            using (_uow.Create())
            {
                ResultServiceObject<IEnumerable<DividendView>> dividends = this.GetDividendsPendingNotification(morningNotification);

                List<DividendView> dividendViews = new List<DividendView>();

                if (dividends.Success)
                {
                    List<PushNotificationView> pushNotificationViews = new List<PushNotificationView>();

                    foreach (DividendView itemDividend in dividends.Value)
                    {
                        DividendView dividendView = itemDividend;

                        IEnumerable<DividendView> groupedDividends = dividends.Value.Where(item => item.IdUser.Equals(itemDividend.IdUser) &&
                        item.IdDividendType.Equals(itemDividend.IdDividendType) &&
                        item.AlreadyIncludedToSend == false);

                        if (groupedDividends.Count() > 1)
                        {
                            dividendView.AlreadyIncludedToSend = false;
                            decimal totalValue = groupedDividends.Select(item => item.NetValue).Sum();

                            string value = string.Empty;

                            if (itemDividend.IdCountry == (long)CountryEnum.Brazil)
                            {
                                value = totalValue.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));
                            }
                            else
                            {
                                value = totalValue.ToString("C", CultureInfo.CreateSpecificCulture("en-US"));
                            }

                            var data = itemDividend.PaymentDate.Value.Date.ToString("dd/MM/yy");
                            //StringBuilder stocks = new StringBuilder();

                            List<string> symbolList = new List<string>();
                            //bool firstExecution = true;

                            foreach (var itemDividendView in groupedDividends)
                            {
                                if (morningNotification)
                                {
                                    ResultServiceObject<Dividend> resultDividend = _dividendService.GetById(itemDividendView.IdDividend);

                                    if (resultDividend.Success && resultDividend.Value != null)
                                    {
                                        resultDividend.Value.NotificationSent = true;
                                        _dividendService.Update(resultDividend.Value);
                                    }
                                }

                                itemDividendView.AlreadyIncludedToSend = true;

                                //if (firstExecution)
                                //{
                                //    firstExecution = false;
                                //}
                                //else
                                //{
                                //    stocks.Append(", ");
                                //}

                                symbolList.Add(itemDividendView.Symbol);
                                //stocks.Append(itemDividendView.Symbol);
                            }

                            string symbols = string.Empty;

                            if (symbolList != null && symbolList.Count > 0)
                            {
                                symbolList = symbolList.Distinct().ToList();
                                symbolList = symbolList.OrderBy(stk => stk).ToList();
                                symbols = string.Join(", ", symbolList);
                            }



                            if (morningNotification)
                            {
                                dividendView.GroupedNotificationTitle = string.Format("Oba! Você vai receber {0}", itemDividend.DividendType.ToLower());
                                dividendView.GroupedNotificationMessage = string.Format("Atenção! Hoje você vai receber dos ativos {0} o total de {1}. Confira sua conta!", symbols, value);
                            }
                            else
                            {
                                dividendView.GroupedNotificationTitle = string.Format("Oba! Você vai receber {0}", itemDividend.DividendType.ToLower());
                                dividendView.GroupedNotificationMessage = string.Format("Está chegando o dia! Você vai receber amanhã ({0}) dos ativos {1} o total de {2}. Fique atento!", data, symbols, value);
                            }

                            dividendViews.Add(dividendView);
                        }
                        else
                        {
                            if (morningNotification)
                            {
                                ResultServiceObject<Dividend> resultDividend = _dividendService.GetById(itemDividend.IdDividend);

                                if (resultDividend.Success && resultDividend.Value != null)
                                {
                                    resultDividend.Value.NotificationSent = true;
                                    _dividendService.Update(resultDividend.Value);
                                }
                            }

                            if (!dividendView.AlreadyIncludedToSend)
                            {
                                dividendViews.Add(dividendView);
                            }
                        }
                    }

                    foreach (DividendView itemDividend in dividendViews)
                    {
                        ResultServiceObject<IEnumerable<Device>> devices = _deviceService.GetByUser(itemDividend.IdUser);
                        ResultServiceObject<ApplicationUser> resultServiceAspNetUser = _userService.GetAccountDetails(itemDividend.IdUser);

                        if (devices.Success && itemDividend.NetValue > 0)
                        {
                            string variationTitlePush = string.Empty;
                            string variacaoMessagePush = string.Empty;

                            if (!string.IsNullOrWhiteSpace(itemDividend.GroupedNotificationTitle))
                            {
                                variationTitlePush = itemDividend.GroupedNotificationTitle;
                                variacaoMessagePush = itemDividend.GroupedNotificationMessage;
                            }
                            else
                            {
                                var useName = resultServiceAspNetUser.Value.Name;
                                var divType = itemDividend.DividendType.ToLower();
                                var symbol = itemDividend.Symbol;
                                var value = string.Empty;

                                if (itemDividend.IdCountry == (long)CountryEnum.Brazil)
                                {
                                    value = itemDividend.NetValue.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));
                                }
                                else
                                {
                                    value = itemDividend.NetValue.ToString("C", CultureInfo.CreateSpecificCulture("en-US"));
                                }

                                var data = itemDividend.PaymentDate.Value.Date.ToString("dd/MM/yy");

                                if (morningNotification)
                                {
                                    switch (DateTime.Now.DayOfWeek)
                                    {
                                        case DayOfWeek.Friday:
                                            {
                                                variationTitlePush = $"Hey! Você vai receber {divType} da {symbol}!";
                                                variacaoMessagePush = $"{useName}, É hoje! Sexta-feira com uma ótima notícia! Você vai receber {value} em {divType} da {symbol}. Isso sim é fechar bem a semana hein! Veja mais detalhes no App Dividendos.me";
                                            }
                                            break;
                                        case DayOfWeek.Monday:
                                            {
                                                variationTitlePush = $"Oba! Você vai receber {divType} da {symbol}!";
                                                variacaoMessagePush = $"Ei! Você vai receber {value} em {divType} da {symbol} hoje! É isso ai {useName}, começando a semana com dinheiro no bolso! Veja mais detalhes no App Dividendos.me";
                                            }
                                            break;
                                        case DayOfWeek.Saturday:
                                            {
                                                variationTitlePush = $"Oba! Você vai receber {divType} da {symbol}!";
                                                variacaoMessagePush = $"Ei {useName}! Você vai receber {value} em {divType} da {symbol} hoje! Veja mais detalhes no App Dividendos.me";
                                            }
                                            break;
                                        case DayOfWeek.Sunday:
                                            {
                                                variationTitlePush = $"Oba! Você vai receber {divType} da {symbol}!";
                                                variacaoMessagePush = $"Olá {useName}! Você vai receber {value} em {divType} da {symbol} hoje! Veja mais detalhes no App Dividendos.me";
                                            }
                                            break;
                                        case DayOfWeek.Thursday:
                                            {
                                                variationTitlePush = $"Oba! Você vai receber {divType} da {symbol}!";
                                                variacaoMessagePush = $"Oi {useName}! A semana tá quase acabando, mas perai, olha essa novidade: Você vai receber {value} em {divType} da {symbol} hoje! Veja mais detalhes no App Dividendos.me";
                                            }
                                            break;
                                        case DayOfWeek.Tuesday:
                                            {
                                                variationTitlePush = $"Olá! Temos uma novidade: Você vai receber {divType} da {symbol}!";
                                                variacaoMessagePush = $"Olá {useName}! Quer uma notícia boa? Segura essa: Você vai receber {value} em {divType} da {symbol} hoje! Veja mais detalhes no App Dividendos.me";
                                            }
                                            break;
                                        case DayOfWeek.Wednesday:
                                            {
                                                variationTitlePush = $"Oba! Você vai receber {divType} da {symbol}!";
                                                variacaoMessagePush = $"Hey {useName}! Quer uma notícia boa no meio da semana? Segura essa: Você vai receber {value} em {divType} da {symbol} hoje! Veja mais detalhes no App Dividendos.me";
                                            }
                                            break;
                                        default:
                                            {
                                                variationTitlePush = $"Oba! Você vai receber {divType} da {symbol}!";
                                                variacaoMessagePush = $"Ei {useName}! Você vai receber {value} em {divType} da {symbol} hoje! Veja mais detalhes no App Dividendos.me";
                                            }
                                            break;
                                    }
                                }
                                else
                                {
                                    switch (DateTime.Now.DayOfWeek)
                                    {
                                        case DayOfWeek.Saturday:
                                        case DayOfWeek.Friday:
                                            {
                                                variationTitlePush = $"Boas novidades: Você vai receber {divType} da {symbol}!";
                                                variacaoMessagePush = $"Olá {useName}! Você vai receber {value} em {divType} da {symbol} no dia {data}. Veja mais detalhes no App Dividendos.me";
                                            }
                                            break;
                                        case DayOfWeek.Monday:
                                            {
                                                variationTitlePush = $"Ei! Quer saber da novidade? Amanhã você vai receber {divType} da {symbol}!";
                                                variacaoMessagePush = $"Ei! Amanhã você vai receber {value} em {divType} da {symbol}. É isso ai {useName}, começando a semana com dinheiro no bolso! Veja mais detalhes no App Dividendos.me";
                                            }
                                            break;
                                        case DayOfWeek.Sunday:
                                            {
                                                variationTitlePush = $"Olá! Amanhã você vai receber {divType} da {symbol}!";
                                                variacaoMessagePush = $"Ei {useName}! Amanhã você vai receber {value} em {divType} da {symbol}. Veja mais detalhes no App Dividendos.me";
                                            }
                                            break;
                                        case DayOfWeek.Thursday:
                                            {
                                                variationTitlePush = $"Oba! Temos novidade: Amanhã você vai receber {divType} da {symbol}!";
                                                variacaoMessagePush = $"Oi {useName}! A semana tá quase acabando, mas perai, olha essa novidade: Amanhã você vai receber {value} em {divType} da {symbol}. Veja mais detalhes no App Dividendos.me";
                                            }
                                            break;
                                        case DayOfWeek.Tuesday:
                                            {
                                                variationTitlePush = $"Olá! Temos uma novidade: Amanhã você vai receber {divType} da {symbol}!";
                                                variacaoMessagePush = $"Olá {useName}! Quer uma notícia boa no começo da semana? Segura essa: Amanhã você vai receber {value} em {divType} da {symbol}. Veja mais detalhes no App Dividendos.me";
                                            }
                                            break;
                                        case DayOfWeek.Wednesday:
                                            {
                                                variationTitlePush = $"Oba! Amanhã você vai receber {divType} da {symbol}!";
                                                variacaoMessagePush = $"Hey {useName}! Quer uma notícia boa no meio da semana? Segura essa: Amanhã você vai receber {value} em {divType} da {symbol}. Veja mais detalhes no App Dividendos.me";
                                            }
                                            break;
                                        default:
                                            {
                                                variationTitlePush = $"Ei! Você vai receber {divType} da {symbol}!";
                                                variacaoMessagePush = $"Olá {useName}! Você vai receber {value} em {divType} da {symbol} no dia {data}. Veja mais detalhes no App Dividendos.me";
                                            }
                                            break;
                                    }
                                }
                            }

                            ResultServiceObject<Settings> settings = _settingsService.GetByUser(itemDividend.IdUser);

                            if (settings.Value == null || settings.Value.PushDividendDeposit)
                            {
                                _notificationHistoricalService.New(variationTitlePush, variacaoMessagePush, itemDividend.IdUser, AppScreenNameEnum.HomeToday.ToString(), PushRedirectTypeEnum.Internal.ToString(), null, _cacheService);
                            }

                            foreach (var itemDevice in devices.Value)
                            {
                                if (settings.Value == null || settings.Value.PushDividendDeposit)
                                {
                                    pushNotificationViews.Add(new PushNotificationView() { Device = itemDevice, Message = variacaoMessagePush, Title = variationTitlePush });
                                }
                            }
                        }
                    }

                    _notificationService.SendPushParallel(pushNotificationViews, 500, new PushRedirect() { PushRedirectType = PushRedirectTypeEnum.Internal, AppScreenName = AppScreenNameEnum.HomeToday });
                }
            }
        }

        private ResultServiceObject<IEnumerable<DividendView>> GetDividendsPendingNotification(bool morningNotification)
        {
            ResultServiceObject<IEnumerable<DividendView>> dividends = _dividendService.GetByNotificationStatusAndPaymentDate(morningNotification);
            return dividends;
        }

        public ResultResponseObject<DividendEditVM> EditDividend(long idDividend, DividendEditVM dividendEditVM)
        {
            ResultResponseObject<DividendEditVM> resultResponse = new ResultResponseObject<DividendEditVM>();
            resultResponse.Success = false;

            using (_uow.Create())
            {
                ResultServiceObject<Dividend> resultServiceObject = _dividendService.GetById(idDividend);

                if (resultServiceObject.Success && resultServiceObject.Value != null && !string.IsNullOrWhiteSpace(dividendEditVM.DividendValue) && !string.IsNullOrWhiteSpace(dividendEditVM.PaymentDate))
                {
                    Dividend dividendDb = resultServiceObject.Value;

                    decimal dividendValue = 0;
                    DateTime paymentDate = DateTime.MinValue;
                    decimal.TryParse(dividendEditVM.DividendValue.Replace(".", string.Empty), NumberStyles.Currency, new CultureInfo("pt-br"), out dividendValue);
                    DateTime.TryParseExact(dividendEditVM.PaymentDate, "dd/MM/yyyy", new CultureInfo("pt-br"), DateTimeStyles.None, out paymentDate);

                    if (dividendValue != 0 && paymentDate > SqlDateTime.MinValue)
                    {
                        dividendDb.Active = false;
                        _dividendService.Update(dividendDb);

                        Dividend dividend = dividendDb;

                        dividend.IdDividend = 0;
                        dividend.NetValue = dividendValue;
                        dividend.GrossValue = dividendValue;
                        dividend.PaymentDate = paymentDate;
                        dividend.AutomaticImport = false;
                        dividend.IdDividendType = dividendDb.IdDividendType;
                        dividend.Active = true;

                        if (dividend.PaymentDate > DateTime.Now)
                        {
                            dividend.NotificationSent = false;
                        }
                        else
                        {
                            dividend.NotificationSent = true;
                        }

                        ResultServiceObject<Dividend> resultDividendInsert = _dividendService.Insert(dividend);

                        dividendEditVM.IdDividend = dividend.IdDividend;
                        resultResponse.Success = true;
                        resultResponse.Value = dividendEditVM;

                        _ = _logger.SendInformationAsync(new { Message = string.Format("Dividend Value updated. IdDividend: {0} Value: {1} PaymentDate: {2} Inactive Id: {3}", dividend.IdDividend, dividend.NetValue, dividend.PaymentDate, dividendDb.IdDividend) });
                    }
                    else
                    {
                        _ = _logger.SendInformationAsync(new { Message = string.Format("Dividend Value updated error. IdDividend: {0} Value: {1} PaymentDate: {2}", dividendDb.IdDividend, dividendEditVM.DividendValue, dividendEditVM.PaymentDate) });
                    }
                }
                else
                {
                    _ = _logger.SendInformationAsync(new { Message = string.Format("Dividend Value updated error. IdDividend: {0} Value: {1} PaymentDate: {2}", dividendEditVM.IdDividend, dividendEditVM.DividendValue, dividendEditVM.PaymentDate) });
                }

            }

            return resultResponse;
        }

        public ResultResponseBase InactiveDividend(long idDividend)
        {
            ResultResponseBase resultResponse = new ResultResponseBase();
            resultResponse.Success = false;

            using (_uow.Create())
            {
                ResultServiceObject<Dividend> resultServiceObject = _dividendService.GetById(idDividend);

                if (resultServiceObject.Success && resultServiceObject.Value != null)
                {
                    Dividend dividendDb = resultServiceObject.Value;
                    dividendDb.Active = false;
                    _dividendService.Update(dividendDb);

                    resultResponse.Success = true;

                    _ = _logger.SendInformationAsync(new { Message = string.Format("Dividend Value inactive. IdDividend: {0}", dividendDb.IdDividend) });

                }
                else
                {
                    _ = _logger.SendInformationAsync(new { Message = string.Format("Dividend inactive error. IdDividend: {0} ", idDividend) });
                }
            }

            return resultResponse;
        }

        public ResultResponseObject<DividendAddVM> AddDividend(long idStock, DividendAddVM dividendAddVM)
        {
            ResultResponseObject<DividendAddVM> resultResponse = new ResultResponseObject<DividendAddVM>();
            resultResponse.Success = false;

            using (_uow.Create())
            {
                ResultServiceObject<Stock> resultServiceObject = _stockService.GetById(idStock);

                if (resultServiceObject.Success && resultServiceObject.Value != null && !string.IsNullOrWhiteSpace(dividendAddVM.DividendValue) && !string.IsNullOrWhiteSpace(dividendAddVM.PaymentDate))
                {
                    decimal dividendValue = 0;
                    DateTime paymentDate = DateTime.MinValue;
                    decimal.TryParse(dividendAddVM.DividendValue.Replace(".", string.Empty), NumberStyles.Currency, new CultureInfo("pt-br"), out dividendValue);
                    DateTime.TryParseExact(dividendAddVM.PaymentDate, "dd/MM/yyyy", new CultureInfo("pt-br"), DateTimeStyles.None, out paymentDate);

                    if (dividendValue != 0 && paymentDate > SqlDateTime.MinValue)
                    {
                        Dividend dividend = new Dividend();

                        dividend.IdDividend = 0;
                        dividend.IdPortfolio = dividendAddVM.IdPortfolio;
                        dividend.NetValue = dividendValue;
                        dividend.GrossValue = dividendValue;
                        dividend.IdStock = resultServiceObject.Value.IdStock;
                        dividend.PaymentDate = paymentDate;
                        dividend.AutomaticImport = false;
                        dividend.IdDividendType = dividendAddVM.IdDividendType;
                        dividend.HomeBroker = "Manual";
                        dividend.Active = true;

                        if (dividend.PaymentDate > DateTime.Now)
                        {
                            dividend.NotificationSent = false;
                        }
                        else
                        {
                            dividend.NotificationSent = true;
                        }

                        ResultServiceObject<Dividend> resultDividendInsert = _dividendService.Insert(dividend);

                        resultResponse.Success = true;
                        resultResponse.Value = dividendAddVM;

                        _ = _logger.SendInformationAsync(new { Message = string.Format("Dividend Value inserted. IdDividend: {0} Value: {1} PaymentDate: {2} ", dividend.IdDividend, dividend.NetValue, dividend.PaymentDate) });
                    }
                    else
                    {
                        _ = _logger.SendInformationAsync(new { Message = string.Format("Dividend Value inserted error. IdStock: {0} Value: {1} PaymentDate: {2}", dividendAddVM.IdStock, dividendAddVM.DividendValue, dividendAddVM.PaymentDate) });
                    }
                }
                else
                {
                    _ = _logger.SendInformationAsync(new { Message = string.Format("Dividend Value inserted error. IdStock: {0} Value: {1} PaymentDate: {2}", dividendAddVM.IdStock, dividendAddVM.DividendValue, dividendAddVM.PaymentDate) });
                }

            }

            return resultResponse;
        }

        public ResultResponseObject<DividendCalendarChart> GetDividendChart(Guid guidPortfolioSub, int? year = null)
        {
            ResultResponseObject<DividendCalendarChart> resultServiceObject = new ResultResponseObject<DividendCalendarChart>();
            DividendCalendarChart dividendChart = new DividendCalendarChart();
            dividendChart.ListProcess = new List<Process>();
            dividendChart.ListCategories = new List<Categories>();
            dividendChart.Task = new Tasks();
            dividendChart.Task.ListTask = new List<TaskClass>();

            if (!year.HasValue)
            {
                year = DateTime.Now.Year;
            }

            using (_uow.Create())
            {
                ResultServiceObject<Portfolio> resultPortfolio = _portfolioService.GetByGuid(guidPortfolioSub);
                ResultServiceObject<SubPortfolio> resultSubPortfolio = _subPortfolioService.GetByGuid(guidPortfolioSub);

                if (resultSubPortfolio.Success && resultPortfolio.Success)
                {
                    Portfolio portfolio = resultPortfolio.Value;
                    SubPortfolio subportfolio = resultSubPortfolio.Value;

                    Guid guidPortfolio = Guid.Empty;

                    long idPortfolio = 0;
                    bool isSub = false;

                    if (portfolio != null)
                    {
                        idPortfolio = portfolio.IdPortfolio;
                        dividendChart.IdCountry = portfolio.IdCountry;
                    }
                    else if (subportfolio != null)
                    {
                        idPortfolio = subportfolio.IdPortfolio;
                        isSub = true;

                        resultPortfolio = _portfolioService.GetById(subportfolio.IdPortfolio);

                        if (resultPortfolio.Value != null)
                        {
                            dividendChart.IdCountry = resultPortfolio.Value.IdCountry;
                        }
                    }

                    ResultServiceObject<IEnumerable<DividendView>> resultDividend = new ResultServiceObject<IEnumerable<DividendView>>();
                    DateTime startDate = new DateTime(year.Value, 1, 1);
                    DateTime endDate = new DateTime(year.Value, 12, 31);

                    if (isSub)
                    {
                        resultDividend = _dividendService.GetAllActiveBySubPortfolioRangeDate(subportfolio.IdSubPortfolio, startDate, endDate);
                    }
                    else
                    {
                        resultDividend = _dividendService.GetAllActiveByPortfolioRangeDate(portfolio.IdPortfolio, startDate, endDate);
                    }


                    if (resultDividend.Success)
                    {
                        IEnumerable<DividendView> dividends = resultDividend.Value;

                        if (dividends != null && dividends.Count() > 0)
                        {
                            List<DividendView> lstStocks = dividends.GroupBy(dividendGp => dividendGp.Symbol).Select(dividend => new DividendView { IdStock = dividend.First().IdStock, Symbol = dividend.First().Symbol }).ToList();

                            if (lstStocks != null && lstStocks.Count > 0)
                            {
                                foreach (DividendView dividendStock in lstStocks)
                                {
                                    Process process = new Process();
                                    process.Id = dividendStock.IdStock.ToString();
                                    process.Label = dividendStock.Symbol;
                                    dividendChart.ListProcess.Add(process);
                                }
                            }

                            List<DividendView> lstDividendGp = new List<DividendView>();

                            foreach (DividendView dividend in dividends)
                            {
                                dividend.NetValueGroup = dividend.NetValue;
                                DividendView dividendGp = null;

                                if (lstDividendGp != null && lstDividendGp.Count > 0)
                                {
                                    dividendGp = lstDividendGp.FirstOrDefault(dividendTmp => dividendTmp.IdStock == dividend.IdStock &&
                                                                                  ((dividendTmp.PaymentDate.HasValue && dividend.PaymentDate.HasValue && dividendTmp.PaymentDate.Value.Month == dividend.PaymentDate.Value.Month)
                                                                                  || (!dividendTmp.PaymentDate.HasValue && !dividend.PaymentDate.HasValue)));
                                }

                                if (dividendGp == null)
                                {
                                    lstDividendGp.Add(dividend);
                                }
                                else
                                {
                                    dividendGp.NetValueGroup += dividend.NetValueGroup;
                                }
                            }

                            decimal netValueReceived = 0;
                            decimal netValueToReceive = 0;
                            decimal netValueNoDate = 0;

                            foreach (DividendView dividend in lstDividendGp)
                            {
                                TaskClass taskClass = new TaskClass();
                                taskClass.Id = dividend.IdDividend.ToString();
                                taskClass.Processid = dividend.IdStock.ToString();
                                taskClass.Height = "25%";
                                taskClass.Toppadding = "40%";

                                string currency = "R$";

                                if (dividendChart.IdCountry == 2)
                                {
                                    currency = "$";
                                }

                                if (dividend.PaymentDate.HasValue)
                                {
                                    taskClass.Label = string.Format("<b>{0}: {1} {2}</b></br>Data: {3}</br>", dividend.DividendType, currency, dividend.NetValueGroup.ToString("n2", new CultureInfo("pt-br")), dividend.PaymentDate.Value.ToString("dd/MM/yyyy"));
                                    taskClass.Start = new DateTime(dividend.PaymentDate.Value.Year, dividend.PaymentDate.Value.Month, 1).ToString("dd/MM/yyyy");
                                    taskClass.End = new DateTime(dividend.PaymentDate.Value.Year, dividend.PaymentDate.Value.Month, DateTime.DaysInMonth(dividend.PaymentDate.Value.Year, dividend.PaymentDate.Value.Month)).ToString("dd/MM/yyyy");

                                    if (dividend.PaymentDate.Value <= DateTime.Now)
                                    {
                                        taskClass.Color = "#00ff00";
                                        netValueReceived += dividend.NetValueGroup;
                                        taskClass.Label = string.Concat(taskClass.Label, "Situação: Pago</br>");
                                    }
                                    else
                                    {
                                        taskClass.Color = "#0000cd";
                                        netValueToReceive += dividend.NetValueGroup;
                                        taskClass.Label = string.Concat(taskClass.Label, "Situação: Pagamento agendado</br>");
                                    }
                                }
                                else
                                {
                                    taskClass.Label = string.Format("<b>{0}: {1} {2}</b>", dividend.DividendType, currency, dividend.NetValueGroup.ToString("n2", new CultureInfo("pt-br")));
                                    taskClass.Start = new DateTime(dividend.DateAdded.Year + 1, 1, 1).ToString("dd/MM/yyyy");
                                    taskClass.End = new DateTime(dividend.DateAdded.Year + 1, 1, 31).ToString("dd/MM/yyyy");
                                    taskClass.Color = "#72D7B2";
                                    netValueToReceive += dividend.NetValueGroup;
                                }

                                dividendChart.Task.ListTask.Add(taskClass);
                            }

                            dividendChart.TotalReceived = netValueReceived.ToString("n2", new CultureInfo("pt-br"));
                            dividendChart.TotalToReceive = netValueToReceive.ToString("n2", new CultureInfo("pt-br"));
                            dividendChart.TotalNoDate = netValueNoDate.ToString("n2", new CultureInfo("pt-br"));
                            dividendChart.LastUpdated = DateTime.Now.ToString("HH:mm:ss");
                            dividendChart.Title = string.Format("Calendário {0}", year.Value);
                            dividendChart.Year = year.ToString();
                        }


                        Categories categories = new Categories();
                        categories.Fontcolor = "#1BCC89";
                        categories.Fontsize = "14";
                        categories.Isbold = "1";

                        categories.ListCategory = new List<Category>();
                        Category category3 = new Category();
                        categories.ListCategory.Add(category3);

                        category3.BgAlpha = "80";
                        category3.Bgcolor = "#000000";
                        category3.FontColor = "#1BCC89";
                        category3.Label = "Meses";
                        category3.Start = new DateTime(year.Value, 1, 1).ToString("dd/MM/yyyy");
                        category3.End = new DateTime(year.Value + 1, 1, 31).ToString("dd/MM/yyyy");


                        dividendChart.ListCategories.Add(categories);

                        categories = new Categories();
                        categories.ListCategory = new List<Category>();

                        for (int i = 1; i <= 12; i++)
                        {
                            string monthName = new DateTime(year.Value, i, 1).ToString("MMM", new CultureInfo("pt-br"));

                            category3 = new Category();
                            category3.BgAlpha = "80";
                            category3.Bgcolor = "#1BCC89";
                            category3.FontColor = "#080808";
                            category3.Label = monthName.First().ToString().ToUpper() + monthName.Substring(1);
                            category3.Start = new DateTime(year.Value, i, 1).ToString("dd/MM/yyyy");
                            category3.End = new DateTime(year.Value, i, 1).AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy");

                            categories.ListCategory.Add(category3);
                        }

                        category3 = new Category();
                        category3.BgAlpha = "80";
                        category3.Bgcolor = "#1BCC89";
                        category3.FontColor = "#080808";
                        category3.Label = "Ind";
                        category3.Start = new DateTime(year.Value + 1, 1, 1).ToString("dd/MM/yyyy");
                        category3.End = new DateTime(year.Value + 1, 1, 31).ToString("dd/MM/yyyy");

                        categories.ListCategory.Add(category3);

                        dividendChart.ListCategories.Add(categories);
                    }
                }
            }

            resultServiceObject.Value = dividendChart;
            resultServiceObject.Success = true;

            return resultServiceObject;
        }


        public ResultResponseObject<IEnumerable<NextDividendVM>> GetNextDividendsByLoggedUser()
        {
            ResultResponseObject<IEnumerable<API.Model.Response.v2.NextDividendVM>> resultService = this.GetNextDividendsByLoggedUserV2();

            ResultResponseObject<IEnumerable<NextDividendVM>> result = _mapper.Map<ResultResponseObject<IEnumerable<NextDividendVM>>>(resultService);

            return result;
        }

        public ResultResponseObject<IEnumerable<API.Model.Response.v2.NextDividendVM>> GetNextDividendsByLoggedUserV2()
        {
            ResultResponseObject<IEnumerable<API.Model.Response.v2.NextDividendVM>> result = null;

            using (_uow.Create())
            {
                ResultServiceObject<IEnumerable<DividendView>> resultService = _dividendService.GetNextDividendByUser(_globalAuthenticationService.IdUser, 5);

                result = _mapper.Map<ResultResponseObject<IEnumerable<API.Model.Response.v2.NextDividendVM>>>(resultService);
            }

            return result;
        }

        public ResultResponseObject<IEnumerable<API.Model.Response.v2.NextDividendVM>> GetNextDividendsByLoggedUserV3()
        {
            ResultResponseObject<IEnumerable<API.Model.Response.v2.NextDividendVM>> result = null;

            using (_uow.Create())
            {
                ResultServiceObject<IEnumerable<DividendView>> resultService = _dividendService.GetNextDividendByUser(_globalAuthenticationService.IdUser, 100);

                result = _mapper.Map<ResultResponseObject<IEnumerable<API.Model.Response.v2.NextDividendVM>>>(resultService);
            }

            return result;
        }


        public ResultResponseObject<API.Model.Response.v4.NextDividendWrapperVM> GetNextDividendsByLoggedUserV4()
        {
            API.Model.Response.v4.NextDividendWrapperVM nextDividendWrapperVM = new API.Model.Response.v4.NextDividendWrapperVM();

            using (_uow.Create())
            {
                ResultServiceObject<IEnumerable<IndicatorSeriesView>> dolarIndicator = _indicatorSeriesService.GetLatest((int)IndicatorTypeEnum.USDBRL);

                decimal dollarExchangeRate = 0;
                var dolar = dolarIndicator.Value.FirstOrDefault();

                if (dolar != null)
                {
                    dollarExchangeRate = dolarIndicator.Value.FirstOrDefault().Points;
                }

                ResultServiceObject<IEnumerable<DividendView>> resultService = _dividendService.GetNextDividendByUser(_globalAuthenticationService.IdUser, 100);

                List<API.Model.Response.v4.NextDividendVM> dividendsViewVMs = new List<API.Model.Response.v4.NextDividendVM>();

                foreach (var itemDividendView in resultService.Value)
                {
                    string grossValue = itemDividendView.GrossValue.ToString("n2", new CultureInfo("pt-br"));
                    string netValue = itemDividendView.NetValue.ToString("n2", new CultureInfo("pt-br"));

                    dividendsViewVMs.Add(new API.Model.Response.v4.NextDividendVM()
                    {
                        PaymentDate = itemDividendView.PaymentDate.HasValue ? itemDividendView.PaymentDate.Value.ToString("dd/MM") : "Data Indef",
                        DividendType = itemDividendView.DividendType,
                        BaseQuantity = itemDividendView.BaseQuantity,
                        IdCountry = (int)itemDividendView.IdCountry,
                        GuidDividend = itemDividendView.GuidDividend,
                        GrossValue = grossValue,
                        NetValue = netValue,
                        GrossValueBRL = ((int)itemDividendView.IdCountry == (int)CountryEnum.EUA ? itemDividendView.GrossValue * dollarExchangeRate : itemDividendView.GrossValue).ToString("n2", new CultureInfo("pt-br")),
                        NetValueBRL = ((int)itemDividendView.IdCountry == (int)CountryEnum.EUA ? itemDividendView.NetValue * dollarExchangeRate : itemDividendView.NetValue).ToString("n2", new CultureInfo("pt-br")),
                        Symbol = itemDividendView.Symbol,
                        StockType = (API.Model.Request.Stock.StockType)itemDividendView.IdStockType
                    });
                }

                nextDividendWrapperVM.NextDividend = dividendsViewVMs;

                if (resultService.Value.Count() > 0)
                {
                    //sum dolar
                    var dolarValue = resultService.Value.Where(item => item.IdCountry.Equals((int)CountryEnum.EUA)).Sum(item => item.NetValue);

                    //sum real
                    var realValue = resultService.Value.Where(item => item.IdCountry.Equals((int)CountryEnum.Brazil)).Sum(item => item.NetValue);

                    //total
                    nextDividendWrapperVM.Total = (realValue + (dolarValue * dollarExchangeRate)).ToString("n2", new CultureInfo("pt-br"));
                }
            }

            return new ResultResponseObject<API.Model.Response.v4.NextDividendWrapperVM>() { Value = nextDividendWrapperVM, Success = true };
        }

        public ResultResponseObject<DividendViewWrapperVM> GetDividendView(Guid guidPortfolioSub, string startDate, string endDate)
        {
            ResultResponseObject<DividendViewWrapperVM> resultServiceObject = new ResultResponseObject<DividendViewWrapperVM>();
            DividendViewWrapperVM dividendViewWrapperVM = new DividendViewWrapperVM();


            DateTime? startDateParam = null;
            DateTime? endDateParam = null;

            if (!string.IsNullOrWhiteSpace(startDate) && !string.IsNullOrWhiteSpace(endDate))
            {
                DateTime operationStartDate;

                if (DateTime.TryParseExact(startDate, "dd/MM/yyyy", new CultureInfo("pt-br"), DateTimeStyles.None, out operationStartDate))
                {
                    startDateParam = operationStartDate;
                }
                else
                {
                    resultServiceObject.ErrorMessages.Add("Data de início inválida");
                }

                DateTime operationEndDate;

                if (DateTime.TryParseExact(endDate, "dd/MM/yyyy", new CultureInfo("pt-br"), DateTimeStyles.None, out operationEndDate))
                {
                    endDateParam = operationEndDate;
                }
                else
                {
                    resultServiceObject.ErrorMessages.Add("Data fim inválida");
                }
            }

            if (resultServiceObject.ErrorMessages == null || resultServiceObject.ErrorMessages.Count() == 0)
            {
                using (_uow.Create())
                {
                    ResultServiceObject<Portfolio> resultPortfolio = _portfolioService.GetByGuid(guidPortfolioSub);
                    ResultServiceObject<SubPortfolio> resultSubPortfolio = _subPortfolioService.GetByGuid(guidPortfolioSub);

                    if (resultSubPortfolio.Success && resultPortfolio.Success)
                    {
                        ResultServiceObject<IEnumerable<DividendDetailsView>> result = new ResultServiceObject<IEnumerable<DividendDetailsView>>();
                        Portfolio portfolio = resultPortfolio.Value;
                        SubPortfolio subportfolio = resultSubPortfolio.Value;

                        if (portfolio != null)
                        {
                            dividendViewWrapperVM.GuidPortfolioSubPortfolio = portfolio.GuidPortfolio;
                            dividendViewWrapperVM.IdPortfolio = portfolio.IdPortfolio;
                            dividendViewWrapperVM.IdCountry = portfolio.IdCountry;
                            result = _dividendService.GetDetailsByPortfolio(portfolio.IdPortfolio, startDateParam, endDateParam);
                        }
                        else if (subportfolio != null)
                        {
                            dividendViewWrapperVM.GuidPortfolioSubPortfolio = subportfolio.GuidSubPortfolio;
                            dividendViewWrapperVM.IdPortfolio = subportfolio.IdPortfolio;
                            resultPortfolio = _portfolioService.GetById(subportfolio.IdPortfolio);

                            if (resultPortfolio.Value != null)
                            {
                                dividendViewWrapperVM.IdCountry = resultPortfolio.Value.IdCountry;
                            }

                            result = _dividendService.GetDetailsBySubportfolio(subportfolio.IdSubPortfolio, startDateParam, endDateParam);
                        }

                        if (result.Success && result.Value != null && result.Value.Count() > 0)
                        {
                            dividendViewWrapperVM.DividendsCompany = new List<DividendCompanyVM>();
                            decimal totalReceived = 0;
                            decimal totalToReceive = 0;

                            List<DividendDetailsView> dividendDetailsViews = result.Value.OrderBy(div => div.Symbol).ThenByDescending(div => div.PaymentDate).ToList();
                            int startYear = 0;
                            int endYear = 0;

                            if (startDateParam.HasValue)
                            {
                                startYear = startDateParam.Value.Year;
                            }
                            else
                            {
                                List<DividendDetailsView> dividendDetailsDate = dividendDetailsViews.FindAll(div => div.PaymentDate.HasValue);

                                if (dividendDetailsDate != null && dividendDetailsDate.Count > 0)
                                {
                                    startYear = dividendDetailsDate.Min(div => div.PaymentDate).Value.Year;
                                }
                            }

                            if (endDateParam.HasValue)
                            {
                                endYear = endDateParam.Value.Year;
                            }
                            else
                            {
                                List<DividendDetailsView> dividendDetailsDate = dividendDetailsViews.FindAll(div => div.PaymentDate.HasValue);

                                if (dividendDetailsDate != null && dividendDetailsDate.Count > 0)
                                {
                                    endYear = dividendDetailsDate.Max(div => div.PaymentDate).Value.Year;
                                }
                            }

                            Dictionary<string, decimal> divPerMonth = new Dictionary<string, decimal>();

                            foreach (DividendDetailsView dividendDetailsView in dividendDetailsViews)
                            {
                                DividendDetailsVM dividendDetailsVM = new DividendDetailsVM();
                                dividendDetailsVM.DividendType = dividendDetailsView.DividendType;
                                dividendDetailsVM.IdDividend = dividendDetailsView.IdDividend;
                                dividendDetailsVM.NetValue = dividendDetailsView.NetValue.ToString("n2", new CultureInfo("pt-br"));
                                dividendDetailsVM.PaymentDate = dividendDetailsView.PaymentDate.HasValue ? dividendDetailsView.PaymentDate.Value.ToString("dd/MM/yyyy") : "Data Indef";

                                DividendCompanyVM dividendCompanyVM = dividendViewWrapperVM.DividendsCompany.FirstOrDefault(divTmp => divTmp.Symbol == dividendDetailsView.Symbol);

                                if (dividendCompanyVM == null)
                                {
                                    decimal totalReceivedCp = dividendDetailsViews.Where(divTmp => divTmp.Symbol == dividendDetailsView.Symbol && (divTmp.PaymentDate.HasValue && divTmp.PaymentDate.Value <= DateTime.Now)).Sum(divTmp => divTmp.NetValue);
                                    decimal totalToReceiveCp = dividendDetailsViews.Where(divTmp => divTmp.Symbol == dividendDetailsView.Symbol && ((divTmp.PaymentDate.HasValue && divTmp.PaymentDate.Value > DateTime.Now) || !divTmp.PaymentDate.HasValue)).Sum(divTmp => divTmp.NetValue);

                                    totalReceived += totalReceivedCp;
                                    totalToReceive += totalToReceiveCp;

                                    dividendCompanyVM = new DividendCompanyVM();
                                    dividendCompanyVM.Company = dividendDetailsView.Company;
                                    dividendCompanyVM.IdCompany = dividendDetailsView.IdCompany;
                                    dividendCompanyVM.IdStock = dividendDetailsView.IdStock;
                                    dividendCompanyVM.Logo = dividendDetailsView.Logo;
                                    dividendCompanyVM.Segment = dividendDetailsView.Segment;
                                    dividendCompanyVM.Symbol = dividendDetailsView.Symbol;
                                    dividendCompanyVM.TotalReceived = totalReceivedCp.ToString("n2", new CultureInfo("pt-br"));
                                    dividendCompanyVM.TotalToReceive = totalToReceiveCp.ToString("n2", new CultureInfo("pt-br"));
                                    dividendCompanyVM.Dividends = new List<DividendDetailsVM>();

                                    dividendViewWrapperVM.DividendsCompany.Add(dividendCompanyVM);

                                    totalReceivedCp = 0;
                                    totalToReceiveCp = 0;

                                    if (startYear != 0 && endYear != 0)
                                    {
                                        dividendCompanyVM.DividendsPerMonth = new List<string>();

                                        for (int year = startYear; year <= endYear; year++)
                                        {
                                            for (int month = 1; month <= 12; month++)
                                            {
                                                decimal totalPerMonth = 0;
                                                string monthName = new DateTime(year, month, 1).ToString("MMM", new CultureInfo("pt-br")).Replace(".", "");
                                                string monthLabel = string.Format("{0}/{1}", monthName, year.ToString().Substring(2));

                                                List<DividendDetailsView> dividendDetailsMonth = dividendDetailsViews.FindAll(divTmp => divTmp.Symbol == dividendDetailsView.Symbol && divTmp.PaymentDate.HasValue && divTmp.PaymentDate.Value.Month == month && divTmp.PaymentDate.Value.Year == year);

                                                if (dividendDetailsMonth != null && dividendDetailsMonth.Count > 0)
                                                {
                                                    totalPerMonth = dividendDetailsMonth.Sum(divTmp => divTmp.NetValue);
                                                }

                                                dividendCompanyVM.DividendsPerMonth.Add(string.Format("{0}: {1}", monthLabel, totalPerMonth.ToString("n2", new CultureInfo("pt-br"))));

                                                if (divPerMonth.ContainsKey(monthLabel))
                                                {
                                                    divPerMonth[monthLabel] += totalPerMonth;
                                                }
                                                else
                                                {
                                                    divPerMonth.Add(monthLabel, totalPerMonth);
                                                }
                                            }
                                        }
                                    }
                                }

                                dividendCompanyVM.Dividends.Add(dividendDetailsVM);
                            }

                            if (divPerMonth.Count > 0)
                            {
                                dividendViewWrapperVM.DividendsPerMonth = new List<string>();

                                foreach (var item in divPerMonth)
                                {
                                    dividendViewWrapperVM.DividendsPerMonth.Add(string.Format("{0}: {1}", item.Key, item.Value.ToString("n2", new CultureInfo("pt-br"))));
                                }
                            }

                            dividendViewWrapperVM.TotalReceived = totalReceived.ToString("n2", new CultureInfo("pt-br"));
                            dividendViewWrapperVM.TotalToReceive = totalToReceive.ToString("n2", new CultureInfo("pt-br"));
                        }
                    }
                }
            }

            resultServiceObject.Value = dividendViewWrapperVM;
            resultServiceObject.Success = true;

            return resultServiceObject;
        }

        public ResultResponseObject<DividendYieldWrapperVM> GetDividendYieldList(Guid guidPortfolioSub, string startDate, string endDate, long? idStock = null, int? idStockType = null)
        {
            ResultResponseObject<DividendYieldWrapperVM> resultServiceObject = new ResultResponseObject<DividendYieldWrapperVM>();
            DividendYieldWrapperVM dividendViewWrapperVM = new DividendYieldWrapperVM();

            DateTime? startDateParam = null;
            DateTime? endDateParam = null;

            if (!string.IsNullOrWhiteSpace(startDate) && !string.IsNullOrWhiteSpace(endDate))
            {
                DateTime operationStartDate;

                if (DateTime.TryParseExact(startDate, "dd/MM/yyyy", new CultureInfo("pt-br"), DateTimeStyles.None, out operationStartDate))
                {
                    startDateParam = operationStartDate;
                }
                else
                {
                    resultServiceObject.ErrorMessages.Add("Data de início inválida");
                }

                DateTime operationEndDate;

                if (DateTime.TryParseExact(endDate, "dd/MM/yyyy", new CultureInfo("pt-br"), DateTimeStyles.None, out operationEndDate))
                {
                    endDateParam = operationEndDate;
                }
                else
                {
                    resultServiceObject.ErrorMessages.Add("Data fim inválida");
                }
            }

            if (resultServiceObject.ErrorMessages == null || resultServiceObject.ErrorMessages.Count() == 0)
            {
                using (_uow.Create())
                {
                    ResultServiceObject<Portfolio> resultPortfolio = _portfolioService.GetByGuid(guidPortfolioSub);
                    ResultServiceObject<SubPortfolio> resultSubPortfolio = _subPortfolioService.GetByGuid(guidPortfolioSub);

                    if (resultSubPortfolio.Success && resultPortfolio.Success)
                    {
                        ResultServiceObject<IEnumerable<DividendDetailsView>> result = new ResultServiceObject<IEnumerable<DividendDetailsView>>();
                        Portfolio portfolio = resultPortfolio.Value;
                        SubPortfolio subportfolio = resultSubPortfolio.Value;

                        if (portfolio != null)
                        {
                            dividendViewWrapperVM.GuidPortfolioSubPortfolio = portfolio.GuidPortfolio;
                            dividendViewWrapperVM.IdPortfolio = portfolio.IdPortfolio;
                            dividendViewWrapperVM.IdCountry = portfolio.IdCountry;
                            result = _dividendService.GetDetailsByPortfolio(portfolio.IdPortfolio, startDateParam, endDateParam, idStock, idStockType);
                        }
                        else if (subportfolio != null)
                        {
                            dividendViewWrapperVM.GuidPortfolioSubPortfolio = subportfolio.GuidSubPortfolio;
                            dividendViewWrapperVM.IdPortfolio = subportfolio.IdPortfolio;
                            resultPortfolio = _portfolioService.GetById(subportfolio.IdPortfolio);
                            portfolio = resultPortfolio.Value;

                            if (resultPortfolio.Value != null)
                            {
                                dividendViewWrapperVM.IdCountry = resultPortfolio.Value.IdCountry;
                            }

                            result = _dividendService.GetDetailsBySubportfolio(subportfolio.IdSubPortfolio, startDateParam, endDateParam, idStock, idStockType);
                        }

                        if (result.Success && result.Value != null && result.Value.Count() > 0)
                        {
                            dividendViewWrapperVM.DividendsYieldPerYear = new List<DividendYieldPerYear>();
                            dividendViewWrapperVM.DividendsYieldPerYear.AddRange(GetDividendYieldList(result.Value.ToList(), portfolio.IdPortfolio, idStock, idStockType, startDateParam, endDateParam));

                            if (dividendViewWrapperVM.DividendsYieldPerYear.Count > 0)
                            {
                                dividendViewWrapperVM.TotalValueString = dividendViewWrapperVM.DividendsYieldPerYear.Sum(div => div.TotalValue).ToString("n2", new CultureInfo("pt-br"));
                            }
                        }
                    }
                }
            }

            resultServiceObject.Value = dividendViewWrapperVM;
            resultServiceObject.Success = true;

            return resultServiceObject;
        }

        private List<DividendYieldPerYear> GetDividendYieldList(List<DividendDetailsView> dividendDetailsViews, long idPortfolio, long? idStock, int? idStockType, DateTime? startDateParam, DateTime? endDateParam, string symbol = null, string logo = null, string company = null)
        {
            List<DividendYieldPerYear> dividendsYieldPerYear = new List<DividendYieldPerYear>();
            //List<DividendDetailsView> dividendDetailsViews = result.Value.OrderBy(div => div.Symbol).ThenByDescending(div => div.PaymentDate).ToList();
            int startYear = 0;
            int endYear = 0;

            if (startDateParam.HasValue)
            {
                startYear = startDateParam.Value.Year;
            }
            else
            {
                List<DividendDetailsView> dividendDetailsDate = dividendDetailsViews.FindAll(div => div.PaymentDate.HasValue);

                if (dividendDetailsDate != null && dividendDetailsDate.Count > 0)
                {
                    startYear = dividendDetailsDate.Min(div => div.PaymentDate).Value.Year;
                }
            }

            if (endDateParam.HasValue)
            {
                endYear = endDateParam.Value.Year;
            }
            else
            {
                List<DividendDetailsView> dividendDetailsDate = dividendDetailsViews.FindAll(div => div.PaymentDate.HasValue);

                if (dividendDetailsDate != null && dividendDetailsDate.Count > 0)
                {
                    endYear = dividendDetailsDate.Max(div => div.PaymentDate).Value.Year;
                }
            }

            if (startYear < DateTime.Now.AddYears(-6).Year)
            {
                startYear = DateTime.Now.AddYears(-6).Year;
            }

            if (endYear > DateTime.Now.AddYears(2).Year)
            {
                endYear = DateTime.Now.AddYears(2).Year;
            }

            if (startYear != 0 && endYear != 0)
            {
                decimal totalDividends = 0;

                for (int year = endYear; year >= startYear; year--)
                {
                    decimal yield = 0;
                    decimal totalDivPerMonth = 0;
                    decimal outLastTotalGtZero = 0;
                    decimal lastTotalGtZero = 0;
                    DividendYieldPerYear dividendYieldPerYear = new DividendYieldPerYear();
                    dividendYieldPerYear.Symbol = symbol;
                    dividendYieldPerYear.Company = company;
                    dividendYieldPerYear.Logo = logo;
                    dividendYieldPerYear.Year = year;
                    dividendYieldPerYear.YearString = year.ToString();

                    List<DividendDetailsView> dividendDetailsMonth = new List<DividendDetailsView>();

                    dividendDetailsMonth = dividendDetailsViews.FindAll(divTmp => divTmp.PaymentDate.HasValue && divTmp.PaymentDate.Value.Month == 1 && divTmp.PaymentDate.Value.Year == year);

                    if (dividendDetailsMonth != null && dividendDetailsMonth.Count > 0)
                    {
                        totalDivPerMonth = dividendDetailsMonth.Sum(divTmp => divTmp.NetValue);

                        if (totalDivPerMonth > 0)
                        {
                            yield = CalculateDividendYield(idPortfolio, 1, year, totalDivPerMonth, lastTotalGtZero, out outLastTotalGtZero, idStock, idStockType);

                            if (outLastTotalGtZero > 0)
                            {
                                lastTotalGtZero = outLastTotalGtZero;
                            }
                        }
                    }

                    dividendYieldPerYear.JanuaryValue = totalDivPerMonth;
                    dividendYieldPerYear.JanuaryYieldString = yield.ToString("n2", new CultureInfo("pt-br"));
                    dividendYieldPerYear.JanuaryValueString = dividendYieldPerYear.JanuaryValue.ToString("n2", new CultureInfo("pt-br"));

                    dividendDetailsMonth = dividendDetailsViews.FindAll(divTmp => divTmp.PaymentDate.HasValue && divTmp.PaymentDate.Value.Month == 2 && divTmp.PaymentDate.Value.Year == year);


                    yield = 0;
                    totalDivPerMonth = 0;

                    if (dividendDetailsMonth != null && dividendDetailsMonth.Count > 0)
                    {
                        totalDivPerMonth = dividendDetailsMonth.Sum(divTmp => divTmp.NetValue);

                        if (totalDivPerMonth > 0)
                        {
                            yield = CalculateDividendYield(idPortfolio, 2, year, totalDivPerMonth, lastTotalGtZero, out outLastTotalGtZero, idStock, idStockType);

                            if (outLastTotalGtZero > 0)
                            {
                                lastTotalGtZero = outLastTotalGtZero;
                            }
                        }
                    }

                    dividendYieldPerYear.FebruaryValue = totalDivPerMonth;
                    dividendYieldPerYear.FebruaryYieldString = yield.ToString("n2", new CultureInfo("pt-br"));
                    dividendYieldPerYear.FebruaryValueString = dividendYieldPerYear.FebruaryValue.ToString("n2", new CultureInfo("pt-br"));

                    dividendDetailsMonth = dividendDetailsViews.FindAll(divTmp => divTmp.PaymentDate.HasValue && divTmp.PaymentDate.Value.Month == 3 && divTmp.PaymentDate.Value.Year == year);

                    yield = 0;
                    totalDivPerMonth = 0;

                    if (dividendDetailsMonth != null && dividendDetailsMonth.Count > 0)
                    {
                        totalDivPerMonth = dividendDetailsMonth.Sum(divTmp => divTmp.NetValue);

                        if (totalDivPerMonth > 0)
                        {
                            yield = CalculateDividendYield(idPortfolio, 3, year, totalDivPerMonth, lastTotalGtZero, out outLastTotalGtZero, idStock, idStockType);

                            if (outLastTotalGtZero > 0)
                            {
                                lastTotalGtZero = outLastTotalGtZero;
                            }
                        }
                    }

                    dividendYieldPerYear.MarchValue = totalDivPerMonth;
                    dividendYieldPerYear.MarchYieldString = yield.ToString("n2", new CultureInfo("pt-br"));
                    dividendYieldPerYear.MarchValueString = dividendYieldPerYear.MarchValue.ToString("n2", new CultureInfo("pt-br"));

                    dividendDetailsMonth = dividendDetailsViews.FindAll(divTmp => divTmp.PaymentDate.HasValue && divTmp.PaymentDate.Value.Month == 4 && divTmp.PaymentDate.Value.Year == year);

                    yield = 0;
                    totalDivPerMonth = 0;

                    if (dividendDetailsMonth != null && dividendDetailsMonth.Count > 0)
                    {
                        totalDivPerMonth = dividendDetailsMonth.Sum(divTmp => divTmp.NetValue);

                        if (totalDivPerMonth > 0)
                        {
                            yield = CalculateDividendYield(idPortfolio, 4, year, totalDivPerMonth, lastTotalGtZero, out outLastTotalGtZero, idStock, idStockType);

                            if (outLastTotalGtZero > 0)
                            {
                                lastTotalGtZero = outLastTotalGtZero;
                            }
                        }
                    }

                    dividendYieldPerYear.AprilValue = totalDivPerMonth;
                    dividendYieldPerYear.AprilYieldString = yield.ToString("n2", new CultureInfo("pt-br"));
                    dividendYieldPerYear.AprilValueString = dividendYieldPerYear.AprilValue.ToString("n2", new CultureInfo("pt-br"));

                    dividendDetailsMonth = dividendDetailsViews.FindAll(divTmp => divTmp.PaymentDate.HasValue && divTmp.PaymentDate.Value.Month == 5 && divTmp.PaymentDate.Value.Year == year);

                    if (dividendDetailsMonth != null && dividendDetailsMonth.Count > 0)
                    {
                        totalDivPerMonth = dividendDetailsMonth.Sum(divTmp => divTmp.NetValue);
                        dividendYieldPerYear.MayValue = totalDivPerMonth;

                        if (totalDivPerMonth > 0)
                        {
                            yield = CalculateDividendYield(idPortfolio, 5, year, totalDivPerMonth, lastTotalGtZero, out outLastTotalGtZero, idStock, idStockType);

                            if (outLastTotalGtZero > 0)
                            {
                                lastTotalGtZero = outLastTotalGtZero;
                            }
                        }
                    }

                    dividendYieldPerYear.MayYieldString = yield.ToString("n2", new CultureInfo("pt-br"));
                    dividendYieldPerYear.MayValueString = dividendYieldPerYear.MayValue.ToString("n2", new CultureInfo("pt-br"));

                    dividendDetailsMonth = dividendDetailsViews.FindAll(divTmp => divTmp.PaymentDate.HasValue && divTmp.PaymentDate.Value.Month == 6 && divTmp.PaymentDate.Value.Year == year);

                    yield = 0;
                    totalDivPerMonth = 0;

                    if (dividendDetailsMonth != null && dividendDetailsMonth.Count > 0)
                    {
                        totalDivPerMonth = dividendDetailsMonth.Sum(divTmp => divTmp.NetValue);

                        if (totalDivPerMonth > 0)
                        {
                            yield = CalculateDividendYield(idPortfolio, 6, year, totalDivPerMonth, lastTotalGtZero, out outLastTotalGtZero, idStock, idStockType);

                            if (outLastTotalGtZero > 0)
                            {
                                lastTotalGtZero = outLastTotalGtZero;
                            }
                        }
                    }

                    dividendYieldPerYear.JuneValue = totalDivPerMonth;
                    dividendYieldPerYear.JuneYieldString = yield.ToString("n2", new CultureInfo("pt-br"));
                    dividendYieldPerYear.JuneValueString = dividendYieldPerYear.JuneValue.ToString("n2", new CultureInfo("pt-br"));

                    dividendDetailsMonth = dividendDetailsViews.FindAll(divTmp => divTmp.PaymentDate.HasValue && divTmp.PaymentDate.Value.Month == 7 && divTmp.PaymentDate.Value.Year == year);

                    yield = 0;
                    totalDivPerMonth = 0;

                    if (dividendDetailsMonth != null && dividendDetailsMonth.Count > 0)
                    {
                        totalDivPerMonth = dividendDetailsMonth.Sum(divTmp => divTmp.NetValue);

                        if (totalDivPerMonth > 0)
                        {
                            yield = CalculateDividendYield(idPortfolio, 7, year, totalDivPerMonth, lastTotalGtZero, out outLastTotalGtZero, idStock, idStockType);

                            if (outLastTotalGtZero > 0)
                            {
                                lastTotalGtZero = outLastTotalGtZero;
                            }
                        }
                    }

                    dividendYieldPerYear.JulyValue = totalDivPerMonth;
                    dividendYieldPerYear.JulyYieldString = yield.ToString("n2", new CultureInfo("pt-br"));
                    dividendYieldPerYear.JulyValueString = dividendYieldPerYear.JulyValue.ToString("n2", new CultureInfo("pt-br"));

                    dividendDetailsMonth = dividendDetailsViews.FindAll(divTmp => divTmp.PaymentDate.HasValue && divTmp.PaymentDate.Value.Month == 8 && divTmp.PaymentDate.Value.Year == year);

                    yield = 0;
                    totalDivPerMonth = 0;

                    if (dividendDetailsMonth != null && dividendDetailsMonth.Count > 0)
                    {
                        totalDivPerMonth = dividendDetailsMonth.Sum(divTmp => divTmp.NetValue);

                        if (totalDivPerMonth > 0)
                        {
                            yield = CalculateDividendYield(idPortfolio, 8, year, totalDivPerMonth, lastTotalGtZero, out outLastTotalGtZero, idStock, idStockType);

                            if (outLastTotalGtZero > 0)
                            {
                                lastTotalGtZero = outLastTotalGtZero;
                            }
                        }
                    }

                    dividendYieldPerYear.AugustValue = totalDivPerMonth;
                    dividendYieldPerYear.AugustYieldString = yield.ToString("n2", new CultureInfo("pt-br"));
                    dividendYieldPerYear.AugustValueString = dividendYieldPerYear.AugustValue.ToString("n2", new CultureInfo("pt-br"));

                    dividendDetailsMonth = dividendDetailsViews.FindAll(divTmp => divTmp.PaymentDate.HasValue && divTmp.PaymentDate.Value.Month == 9 && divTmp.PaymentDate.Value.Year == year);

                    yield = 0;
                    totalDivPerMonth = 0;

                    if (dividendDetailsMonth != null && dividendDetailsMonth.Count > 0)
                    {
                        totalDivPerMonth = dividendDetailsMonth.Sum(divTmp => divTmp.NetValue);

                        if (totalDivPerMonth > 0)
                        {
                            yield = CalculateDividendYield(idPortfolio, 9, year, totalDivPerMonth, lastTotalGtZero, out outLastTotalGtZero, idStock, idStockType);

                            if (outLastTotalGtZero > 0)
                            {
                                lastTotalGtZero = outLastTotalGtZero;
                            }
                        }
                    }

                    dividendYieldPerYear.SeptemberValue = totalDivPerMonth;
                    dividendYieldPerYear.SeptemberYieldString = yield.ToString("n2", new CultureInfo("pt-br"));
                    dividendYieldPerYear.SeptemberValueString = dividendYieldPerYear.SeptemberValue.ToString("n2", new CultureInfo("pt-br"));

                    dividendDetailsMonth = dividendDetailsViews.FindAll(divTmp => divTmp.PaymentDate.HasValue && divTmp.PaymentDate.Value.Month == 10 && divTmp.PaymentDate.Value.Year == year);

                    yield = 0;
                    totalDivPerMonth = 0;

                    if (dividendDetailsMonth != null && dividendDetailsMonth.Count > 0)
                    {
                        totalDivPerMonth = dividendDetailsMonth.Sum(divTmp => divTmp.NetValue);

                        if (totalDivPerMonth > 0)
                        {
                            yield = CalculateDividendYield(idPortfolio, 10, year, totalDivPerMonth, lastTotalGtZero, out outLastTotalGtZero, idStock, idStockType);

                            if (outLastTotalGtZero > 0)
                            {
                                lastTotalGtZero = outLastTotalGtZero;
                            }
                        }
                    }

                    dividendYieldPerYear.OctoberValue = totalDivPerMonth;
                    dividendYieldPerYear.OctoberYieldString = yield.ToString("n2", new CultureInfo("pt-br"));
                    dividendYieldPerYear.OctoberValueString = dividendYieldPerYear.OctoberValue.ToString("n2", new CultureInfo("pt-br"));

                    dividendDetailsMonth = dividendDetailsViews.FindAll(divTmp => divTmp.PaymentDate.HasValue && divTmp.PaymentDate.Value.Month == 11 && divTmp.PaymentDate.Value.Year == year);

                    yield = 0;
                    totalDivPerMonth = 0;

                    if (dividendDetailsMonth != null && dividendDetailsMonth.Count > 0)
                    {
                        totalDivPerMonth = dividendDetailsMonth.Sum(divTmp => divTmp.NetValue);

                        if (totalDivPerMonth > 0)
                        {
                            yield = CalculateDividendYield(idPortfolio, 11, year, totalDivPerMonth, lastTotalGtZero, out outLastTotalGtZero, idStock, idStockType);

                            if (outLastTotalGtZero > 0)
                            {
                                lastTotalGtZero = outLastTotalGtZero;
                            }
                        }
                    }

                    dividendYieldPerYear.NovemberValue = totalDivPerMonth;
                    dividendYieldPerYear.NovemberYieldString = yield.ToString("n2", new CultureInfo("pt-br"));
                    dividendYieldPerYear.NovemberValueString = dividendYieldPerYear.NovemberValue.ToString("n2", new CultureInfo("pt-br"));

                    dividendDetailsMonth = dividendDetailsViews.FindAll(divTmp => divTmp.PaymentDate.HasValue && divTmp.PaymentDate.Value.Month == 12 && divTmp.PaymentDate.Value.Year == year);

                    yield = 0;
                    totalDivPerMonth = 0;

                    if (dividendDetailsMonth != null && dividendDetailsMonth.Count > 0)
                    {
                        totalDivPerMonth = dividendDetailsMonth.Sum(divTmp => divTmp.NetValue);

                        if (totalDivPerMonth > 0)
                        {
                            yield = CalculateDividendYield(idPortfolio, 12, year, totalDivPerMonth, lastTotalGtZero, out outLastTotalGtZero, idStock, idStockType);

                            if (outLastTotalGtZero > 0)
                            {
                                lastTotalGtZero = outLastTotalGtZero;
                            }
                        }
                    }

                    dividendYieldPerYear.DecemberValue = totalDivPerMonth;
                    dividendYieldPerYear.DecemberYieldString = yield.ToString("n2", new CultureInfo("pt-br"));
                    dividendYieldPerYear.DecemberValueString = dividendYieldPerYear.DecemberValue.ToString("n2", new CultureInfo("pt-br"));

                    List<DividendDetailsView> dividendDetailsYear = dividendDetailsViews.FindAll(divTmp => divTmp.PaymentDate.HasValue && divTmp.PaymentDate.Value.Year == year);

                    decimal average = 0;
                    decimal yieldAverage = 0;
                    yield = 0;

                    if (dividendDetailsYear != null && dividendDetailsYear.Count > 0)
                    {
                        decimal totalPerYear = dividendDetailsYear.Sum(divTmp => divTmp.NetValue);
                        dividendYieldPerYear.TotalValue = totalPerYear;

                        if (totalPerYear > 0)
                        {
                            yield = CalculateDividendYield(idPortfolio, 12, year, totalPerYear, lastTotalGtZero, out lastTotalGtZero, idStock, idStockType);
                            average = totalPerYear / 12;
                            yieldAverage = yield / 12;
                        }
                    }

                    dividendYieldPerYear.YieldValueString = yield.ToString("n2", new CultureInfo("pt-br"));
                    dividendYieldPerYear.TotalValueString = dividendYieldPerYear.TotalValue.ToString("n2", new CultureInfo("pt-br"));
                    dividendYieldPerYear.AverageValue = average;
                    dividendYieldPerYear.AverageValueString = average.ToString("n2", new CultureInfo("pt-br"));
                    dividendYieldPerYear.YieldValue = yield;
                    dividendYieldPerYear.YieldAverageValueString = yieldAverage.ToString("n2", new CultureInfo("pt-br"));

                    dividendsYieldPerYear.Add(dividendYieldPerYear);

                }
            }

            return dividendsYieldPerYear;
        }

        private decimal CalculateDividendYield(long idPortfolio, int month, int year, decimal totalDivPerPeriod, decimal lastTotalGtZero, out decimal outLastTotalGtZero, long? idStock = null, int? idStockType = null)
        {
            outLastTotalGtZero = 0;
            decimal yield = 0;
            bool stockSold = false;
            DateTime lastDayMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            if (lastDayMonth.Date >= DateTime.Now.Date)
            {
                lastDayMonth = DateTime.Now;
            }

            ResultServiceObject<Operation> resultOp = null;

            if (idStock.HasValue)
            {
                resultOp = _operationService.GetByPortfolioAndIdStock(idPortfolio, idStock.Value, 1);

                if (resultOp.Value != null && !resultOp.Value.Active)
                {
                    stockSold = true;

                    decimal total = _performanceStockService.GetLatestTotalByIdStock(idPortfolio, idStock.Value, year);

                    if (total == 0 && lastTotalGtZero > 0)
                    {
                        total = lastTotalGtZero;
                    }

                    if (total > 0)
                    {
                        yield = (totalDivPerPeriod / total) * 100;
                        outLastTotalGtZero = total;
                    }
                }
            }

            if (!stockSold)
            {
                lastDayMonth = CheckLastDayOpenMarket(lastDayMonth);
                ResultServiceObject<PortfolioPerformance> resultPortPerformance = _portfolioPerformanceService.GetByCalculationDate(idPortfolio, lastDayMonth);


                if (resultPortPerformance.Value != null)
                {
                    if (idStockType.HasValue || idStock.HasValue)
                    {
                        decimal total = _performanceStockService.GetTotalByIdStockType(resultPortPerformance.Value.IdPortfolioPerformance, idStock, idStockType);

                        if (total == 0 && lastTotalGtZero > 0)
                        {
                            total = lastTotalGtZero;
                        }

                        if (total > 0)
                        {
                            yield = (totalDivPerPeriod / total) * 100;
                            outLastTotalGtZero = total;
                        }
                    }
                    else
                    {
                        if (resultPortPerformance.Value.Total > 0)
                        {
                            yield = (totalDivPerPeriod / resultPortPerformance.Value.Total) * 100;
                            outLastTotalGtZero = resultPortPerformance.Value.Total;
                        }
                    }
                }
                else
                {
                    decimal? totalPortfolio = _portfolioService.GetTotalPortfolio(idPortfolio, lastDayMonth, idStock, idStockType);

                    if (totalPortfolio.HasValue && totalPortfolio.Value > 0)
                    {
                        yield = (totalDivPerPeriod / totalPortfolio.Value) * 100;
                        outLastTotalGtZero = totalPortfolio.Value;
                    }
                }
            }

            return yield;
        }

        public DateTime CheckLastDayOpenMarket(DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Sunday || date.DayOfWeek == DayOfWeek.Saturday || _holidayService.IsHoliday(date, 1))
            {
                date = date.AddDays(-1);
                date = CheckLastDayOpenMarket(date);
            }

            return date;
        }

        public ResultResponseObject<IEnumerable<NextDividendVM>> GetDividendList(Guid guidPortfolioSub, int year, int month, long? idStock = null, int? idStockType = null)
        {
            ResultResponseObject<IEnumerable<NextDividendVM>> resultServiceObject = new ResultResponseObject<IEnumerable<NextDividendVM>>();
            List<NextDividendVM> nextDividendVMs = new List<NextDividendVM>();
            resultServiceObject.Success = true;
            DividendYieldWrapperVM dividendViewWrapperVM = new DividendYieldWrapperVM();

            DateTime? startDateParam = null;
            DateTime? endDateParam = null;

            if (resultServiceObject.ErrorMessages == null || resultServiceObject.ErrorMessages.Count() == 0)
            {
                using (_uow.Create())
                {
                    ResultServiceObject<Portfolio> resultPortfolio = _portfolioService.GetByGuid(guidPortfolioSub);
                    ResultServiceObject<SubPortfolio> resultSubPortfolio = _subPortfolioService.GetByGuid(guidPortfolioSub);

                    if (resultSubPortfolio.Success && resultPortfolio.Success)
                    {
                        ResultServiceObject<IEnumerable<DividendDetailsView>> result = new ResultServiceObject<IEnumerable<DividendDetailsView>>();
                        Portfolio portfolio = resultPortfolio.Value;
                        SubPortfolio subportfolio = resultSubPortfolio.Value;

                        startDateParam = new DateTime(year, month, 1);
                        endDateParam = new DateTime(year, month, DateTime.DaysInMonth(year, month));

                        if (portfolio != null)
                        {
                            dividendViewWrapperVM.GuidPortfolioSubPortfolio = portfolio.GuidPortfolio;
                            dividendViewWrapperVM.IdPortfolio = portfolio.IdPortfolio;
                            dividendViewWrapperVM.IdCountry = portfolio.IdCountry;
                            result = _dividendService.GetDetailsByPortfolio(portfolio.IdPortfolio, startDateParam, endDateParam, idStock, idStockType);
                        }
                        else if (subportfolio != null)
                        {
                            dividendViewWrapperVM.GuidPortfolioSubPortfolio = subportfolio.GuidSubPortfolio;
                            dividendViewWrapperVM.IdPortfolio = subportfolio.IdPortfolio;
                            resultPortfolio = _portfolioService.GetById(subportfolio.IdPortfolio);
                            portfolio = resultPortfolio.Value;

                            if (resultPortfolio.Value != null)
                            {
                                dividendViewWrapperVM.IdCountry = resultPortfolio.Value.IdCountry;
                            }

                            result = _dividendService.GetDetailsBySubportfolio(subportfolio.IdSubPortfolio, startDateParam, endDateParam, idStock, idStockType);
                        }

                        if (result.Success && result.Value != null && result.Value.Count() > 0)
                        {
                            List<DividendDetailsView> dividendDetailsViews = result.Value.Where(div => div.PaymentDate.HasValue).ToList();

                            if (dividendDetailsViews != null && dividendDetailsViews.Count > 0)
                            {
                                dividendDetailsViews = result.Value.Where(div => div.PaymentDate.HasValue).OrderByDescending(div => div.PaymentDate).ToList();

                                foreach (DividendDetailsView dividendDetailsView in dividendDetailsViews)
                                {
                                    NextDividendVM nextDividendVM = new NextDividendVM();
                                    nextDividendVM.GuidDividend = Guid.NewGuid();
                                    nextDividendVM.NetValue = dividendDetailsView.NetValue.ToString("n2", new CultureInfo("pt-br"));
                                    nextDividendVM.PaymentDate = dividendDetailsView.PaymentDate.HasValue ? dividendDetailsView.PaymentDate.Value.ToString("dd/MM") : "Data Indef";
                                    nextDividendVM.Symbol = dividendDetailsView.Symbol;
                                    nextDividendVM.DividendType = _dividendService.Abreviation(dividendDetailsView.IdDividendType);

                                    nextDividendVMs.Add(nextDividendVM);
                                }
                            }
                        }
                    }
                }
            }

            resultServiceObject.Value = nextDividendVMs;

            return resultServiceObject;
        }

        public ResultResponseObject<DividendYieldWrapperVM> GetRankingDividendYield(Guid guidPortfolioSub, int year, int? month, long? idStock = null, int? idStockType = null)
        {
            ResultResponseObject<DividendYieldWrapperVM> resultServiceObject = new ResultResponseObject<DividendYieldWrapperVM>();
            DividendYieldWrapperVM dividendViewWrapperVM = new DividendYieldWrapperVM();
            dividendViewWrapperVM.DividendsYieldPerYear = new List<DividendYieldPerYear>();

            DateTime? startDateParam = null;
            DateTime? endDateParam = null;

            if (month.HasValue)
            {
                startDateParam = new DateTime(year, month.Value, 1);
                endDateParam = new DateTime(year, month.Value, DateTime.DaysInMonth(year, month.Value));
            }
            else
            {
                startDateParam = new DateTime(year, 1, 1);
                endDateParam = new DateTime(year, 12, 31);
            }

            if (resultServiceObject.ErrorMessages == null || resultServiceObject.ErrorMessages.Count() == 0)
            {
                using (_uow.Create())
                {
                    ResultServiceObject<Portfolio> resultPortfolio = _portfolioService.GetByGuid(guidPortfolioSub);
                    ResultServiceObject<SubPortfolio> resultSubPortfolio = _subPortfolioService.GetByGuid(guidPortfolioSub);

                    if (resultSubPortfolio.Success && resultPortfolio.Success)
                    {
                        ResultServiceObject<IEnumerable<DividendDetailsView>> result = new ResultServiceObject<IEnumerable<DividendDetailsView>>();
                        Portfolio portfolio = resultPortfolio.Value;
                        SubPortfolio subportfolio = resultSubPortfolio.Value;

                        if (portfolio != null)
                        {
                            dividendViewWrapperVM.GuidPortfolioSubPortfolio = portfolio.GuidPortfolio;
                            dividendViewWrapperVM.IdPortfolio = portfolio.IdPortfolio;
                            dividendViewWrapperVM.IdCountry = portfolio.IdCountry;
                            result = _dividendService.GetDetailsByPortfolio(portfolio.IdPortfolio, startDateParam, endDateParam, idStock, idStockType);
                        }
                        else if (subportfolio != null)
                        {
                            dividendViewWrapperVM.GuidPortfolioSubPortfolio = subportfolio.GuidSubPortfolio;
                            dividendViewWrapperVM.IdPortfolio = subportfolio.IdPortfolio;
                            resultPortfolio = _portfolioService.GetById(subportfolio.IdPortfolio);
                            portfolio = resultPortfolio.Value;

                            if (resultPortfolio.Value != null)
                            {
                                dividendViewWrapperVM.IdCountry = resultPortfolio.Value.IdCountry;
                            }

                            result = _dividendService.GetDetailsBySubportfolio(subportfolio.IdSubPortfolio, startDateParam, endDateParam, idStock, idStockType);
                        }

                        if (result.Success && result.Value != null && result.Value.Count() > 0)
                        {
                            List<DividendView> lstStockGroup = result.Value.GroupBy(dividendGp => dividendGp.Symbol).Select(dividend => new DividendView { IdStock = dividend.First().IdStock, Symbol = dividend.First().Symbol, Logo = dividend.First().Logo, Company = dividend.First().Company }).ToList();


                            if (lstStockGroup != null && lstStockGroup.Count > 0)
                            {
                                foreach (DividendView stockGroup in lstStockGroup)
                                {
                                    List<DividendDetailsView> dividendDetailsViews = result.Value.Where(div => div.IdStock == stockGroup.IdStock).ToList();
                                    dividendViewWrapperVM.DividendsYieldPerYear.AddRange(GetDividendYieldList(dividendDetailsViews, portfolio.IdPortfolio, stockGroup.IdStock, idStockType, startDateParam, endDateParam, stockGroup.Symbol, stockGroup.Logo, stockGroup.Company));
                                }

                                if (dividendViewWrapperVM.DividendsYieldPerYear.Count > 0)
                                {
                                    dividendViewWrapperVM.DividendsYieldPerYear = dividendViewWrapperVM.DividendsYieldPerYear.OrderByDescending(div => div.YieldValue).ToList();
                                    decimal totalPeriod = dividendViewWrapperVM.DividendsYieldPerYear.Sum(div => div.TotalValue);
                                    dividendViewWrapperVM.TotalValueString = totalPeriod.ToString("n2", new CultureInfo("pt-br"));

                                    decimal yieldValue = CalculateDividendYield(portfolio.IdPortfolio, 12, year, totalPeriod, totalPeriod, out totalPeriod);
                                    dividendViewWrapperVM.YieldValueString = yieldValue.ToString("n2", new CultureInfo("pt-br"));
                                }
                            }
                        }
                    }
                }
            }

            resultServiceObject.Value = dividendViewWrapperVM;
            resultServiceObject.Success = true;

            return resultServiceObject;
        }

        public ResultResponseBase RestorePastDividends(Guid guidPortfolio, string token)
        {
            string idUser = string.Empty;
            List<string> divs = new List<string>();
            ResultResponseBase resultResponseBase = new ResultResponseBase();
            ResultServiceObject<Portfolio> resultPortfolio = new ResultServiceObject<Portfolio>();

            using (_uow.Create())
            {
                resultPortfolio = _portfolioService.GetByGuid(guidPortfolio);
            }

            if (resultPortfolio.Value != null)
            {
                Trader trader = null;

                using (_uow.Create())
                {
                    trader = _traderService.GetById(resultPortfolio.Value.IdTrader).Value;
                }

                if (trader != null)
                {
                    idUser = trader.IdUser;

                    if (trader.TraderTypeID == (int)TraderTypeEnum.Clear)
                    {
                        RestoreClearDividends(divs, resultResponseBase, resultPortfolio, trader);
                    }
                    else if (trader.TraderTypeID == (int)TraderTypeEnum.Rico)
                    {
                        RestoreRicoDividends(token, divs, resultResponseBase, resultPortfolio, trader);
                    }
                    else if (trader.TraderTypeID == (int)TraderTypeEnum.RendaVariavelAndTesouroDiretoNewCEI)
                    {
                        divs = RestoreNewB3Dividends(divs, resultPortfolio, trader);
                        resultResponseBase.Success = true;
                    }
                    else
                    {
                        divs = RestorePastDividends(resultPortfolio.Value.IdPortfolio, resultPortfolio.Value.IdCountry);
                        resultResponseBase.Success = true;
                    }
                }
            }

            if (divs.Count > 0)
            {
                divs = divs.Distinct().ToList();
                divs = divs.OrderBy(stk => stk).ToList();
                string stocks = string.Join(", ", divs);

                using (_uow.Create())
                {
                    _portfolioService.SendNotificationNewItensOnPortfolio(idUser, true, stocks, _settingsService, _deviceService, _notificationHistoricalService, _cacheService, _notificationService, _logger, true, true);
                }
            }

            return resultResponseBase;
        }

        private void RestoreRicoDividends(string token, List<string> divs, ResultResponseBase resultResponseBase, ResultServiceObject<Portfolio> resultPortfolio, Trader trader)
        {
            ImportRicoResult importRicoResult = _ricoHelper.RestoreDividends(trader.Identifier, trader.Password, token);

            if (importRicoResult != null && importRicoResult.Success && importRicoResult.Dividends != null && importRicoResult.Dividends.Count > 0)
            {
                using (_uow.Create())
                {
                    SaveDividendsRico(trader.IdUser, trader.Identifier, trader.Password, importRicoResult, false, divs, resultPortfolio.Value);
                }

                resultResponseBase.Success = true;
            }
        }

        private List<string> RestoreNewB3Dividends(List<string> divs, ResultServiceObject<Portfolio> resultPortfolio, Trader trader)
        {
            DateTime startReferenceDate = DateTime.Now.AddDays(-909);
            DateTime endReferenceDate = DateTime.Now;
            bool isNightCei = CheckCeiNight();

            using (_uow.Create())
            {
                endReferenceDate = _holidayService.PreviousWorkDay(1, isNightCei);
            }

            string jwtToken = _iImportInvestidorB3Helper.GetAutorizationToken();
            Tuple<List<Dividendos.InvestidorB3.Interface.Model.StockOperation>, List<Dividendos.InvestidorB3.Interface.Model.DividendImport>> tupResult = _iImportInvestidorB3Helper.GetHistoricalEvents(trader.Identifier, startReferenceDate, endReferenceDate, jwtToken);

            if (tupResult.Item2 != null && tupResult.Item2.Count > 0)
            {
                List<DividendImportView> dividendImportViews = new List<DividendImportView>();

                foreach (Dividendos.InvestidorB3.Interface.Model.DividendImport dividendImport in tupResult.Item2)
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

                    dividendImportViews.Add(dividendImportView);
                }

                using (_uow.Create())
                {
                    ResultServiceObject<IEnumerable<Stock>> resultServiceStock = _stockService.GetAllByCountry((int)CountryEnum.Brazil);
                    _portfolioService.SaveDividend(resultServiceStock.Value, dividendImportViews, resultPortfolio.Value, trader.IdUser, _dividendService, _dividendTypeService, _logger, out divs);
                }
            }

            return divs;
        }

        private void RestoreClearDividends(List<string> divs, ResultResponseBase resultResponseBase, ResultServiceObject<Portfolio> resultPortfolio, Trader trader)
        {
            string[] authInfo = trader.Password.Split("€");

            ScrapyScheduler scrapyScheduler = null;

            using (_uow.Create())
            {
                scrapyScheduler = new ScrapyScheduler();
                scrapyScheduler.Agent = "Clear";
                scrapyScheduler.AutomaticImport = true;
                scrapyScheduler.CreatedDate = DateTime.Now;
                scrapyScheduler.ExecutionTime = null;
                scrapyScheduler.FinishDate = null;
                scrapyScheduler.Identifier = trader.Identifier;
                scrapyScheduler.Password = trader.Password;
                scrapyScheduler.IdUser = trader.IdUser;
                scrapyScheduler.Priority = 1;
                scrapyScheduler.Sent = true;
                scrapyScheduler.StartDate = scrapyScheduler.CreatedDate;
                scrapyScheduler.Status = (int)ScrapySchedulerStatusEnum.Running;
                scrapyScheduler.TimedOut = false;
                scrapyScheduler.WaitingTime = scrapyScheduler.CreatedDate;
                scrapyScheduler.IdTraderType = (long)TraderTypeEnum.Clear;
                scrapyScheduler.ActionType = ActionTypeEnum.RestoreDividends.ToString();
                _scrapySchedulerService.AddBrokerLog(scrapyScheduler);
            }


            ImportClearResult importClearResult = _clearHelper.RestoreDividends(trader.Identifier, authInfo[0], authInfo[1], DateTime.Now.AddDays(-20));

            using (_uow.Create())
            {
                if (scrapyScheduler != null)
                {
                    scrapyScheduler.Results = importClearResult.DividendException;
                    scrapyScheduler.FinishDate = DateTime.Now;
                    scrapyScheduler.ExecutionTime = DateTime.Now.Date.Add(scrapyScheduler.FinishDate.Value.Subtract(scrapyScheduler.StartDate.Value));
                    scrapyScheduler.Status = (int)ScrapySchedulerStatusEnum.Completed;
                    scrapyScheduler.ResponseBody = importClearResult.ResponseBody;
                    scrapyScheduler.Results = importClearResult.DividendException;

                    _scrapySchedulerService.Update(scrapyScheduler);
                }
            }

            if (importClearResult != null && importClearResult.Success)
            {
                using (_uow.Create())
                {
                    SaveDividendsClear(trader.IdUser, importClearResult, false, divs, resultPortfolio.Value);
                }

                resultResponseBase.Success = true;
            }
        }

        public List<string> RestorePastDividends(long idPortfolio, int idCountry)
        {
            return RestorePastDividends(idPortfolio, idCountry, null);
        }

        public List<string> RestorePastDividends(long idPortfolio, int idCountry, DateTime? dataCom)
        {
            List<string> divs = new List<string>();

            using (_uow.Create())
            {
                ResultServiceObject<Portfolio> resultServicePortfolio = _portfolioService.GetById(idPortfolio);

                if (resultServicePortfolio.Value != null)
                {
                    ResultServiceObject<Trader> resultServiceTrader = _traderService.GetById(resultServicePortfolio.Value.IdTrader);
                    bool? cei = resultServiceTrader.Value != null && resultServiceTrader.Value.TraderTypeID == (int)TraderTypeEnum.RendaVariavelAndTesouroDiretoCEI;

                    var returnDivs = _dividendService.RestorePastDividends(idPortfolio, idCountry, _operationItemService, _dividendCalendarService, _operationService, _stockService, _performanceStockService, _portfolioService, dataCom, cei);

                    if (resultServiceTrader.Value.ShowOnPatrimony)
                    {
                        divs = returnDivs;
                    }
                }
            }

            return divs;
        }

        private void SaveDividendsClear(string idUser, ImportClearResult importClearResult, bool newPortfolio, List<string> dividendCeiItems, Portfolio portfolio)
        {
            IEnumerable<DividendType> resultServiceDividendType = _dividendTypeService.GetAll().Value;
            ResultServiceObject<IEnumerable<Dividend>> resultServiceDividend = new ResultServiceObject<IEnumerable<Dividend>>();

            if (!newPortfolio)
            {
                resultServiceDividend = _dividendService.GetAutomaticByIdPortfolio(portfolio.IdPortfolio);
            }

            foreach (ClearDividend ricoDividend in importClearResult.Dividends)
            {
                Stock stock = _stockService.GetBySymbolOrLikeOldSymbol(ricoDividend.Symbol, 1).Value;

                if (stock != null)
                {
                    Dividend dividend = new Dividend();
                    dividend.GrossValue = ricoDividend.NetValue;
                    dividend.NetValue = ricoDividend.NetValue;
                    dividend.HomeBroker = "Clear";
                    dividend.PaymentDate = ricoDividend.EventDate;
                    dividend.IdStock = stock.IdStock;
                    dividend.IdPortfolio = portfolio.IdPortfolio;

                    DividendType dividendType = resultServiceDividendType.FirstOrDefault(divType => divType.Name.ToLower().Contains(ricoDividend.Type.ToLower()));

                    if (dividendType == null)
                    {
                        dividend.IdDividendType = (int)DividendTypeEnum.Dividend;
                    }
                    else
                    {
                        dividend.IdDividendType = dividendType.IdDividendType;
                    }

                    if (resultServiceDividend.Value != null && resultServiceDividend.Value.Count() > 0)
                    {
                        List<Dividend> dividendsDb = resultServiceDividend.Value.Where(div => (div.Active && div.PaymentDate.HasValue && div.IdStock == dividend.IdStock && div.IdDividendType == dividend.IdDividendType && div.PaymentDate.Value.Date == dividend.PaymentDate.Value.Date)).ToList();

                        if (dividendsDb != null && dividendsDb.Count > 0)
                        {
                            foreach (Dividend dividendDb in dividendsDb)
                            {
                                if (dividendDb.HomeBroker == "Sistema")
                                {
                                    dividendDb.Active = false;
                                    _dividendService.Update(dividendDb);
                                }
                                else
                                {
                                    dividend = dividendDb;
                                }
                            }
                        }
                    }

                    if (dividend.IdDividend == 0)
                    {
                        dividendCeiItems.Add(stock.Symbol);

                        dividend.AutomaticImport = true;
                        _dividendService.Insert(dividend);
                    }
                }
                else
                {
                    _ = _logger.SendInformationAsync(new { Message = string.Format("Stock not found {0} : {1}", ricoDividend.Symbol, idUser) });
                }
            }
        }

        private ResultServiceObject<Trader> SaveDividendsRico(string idUser, string account, string password, ImportRicoResult importRicoResult, bool newPortfolio, List<string> dividendCeiItems, Portfolio portfolio)
        {
            ResultServiceObject<Trader> resultTraderService;
            DateTime lastSyncOut = DateTime.Now;
            resultTraderService = _traderService.SaveTrader(account, password, idUser, false, false, TraderTypeEnum.Rico, out lastSyncOut);
            IEnumerable<DividendType> resultServiceDividendType = _dividendTypeService.GetAll().Value;
            ResultServiceObject<IEnumerable<Dividend>> resultServiceDividend = new ResultServiceObject<IEnumerable<Dividend>>();

            if (portfolio == null)
            {
                portfolio = _portfolioService.SavePortfolio(resultTraderService.Value, CountryEnum.Brazil, "Rico", out newPortfolio);
            }

            if (!newPortfolio)
            {
                resultServiceDividend = _dividendService.GetAutomaticByIdPortfolio(portfolio.IdPortfolio);
            }

            foreach (RicoDividend ricoDividend in importRicoResult.Dividends)
            {
                Stock stock = _stockService.GetBySymbolOrLikeOldSymbol(ricoDividend.Symbol, 1).Value;

                if (stock != null)
                {
                    Dividend dividend = new Dividend();
                    dividend.NetValue = ricoDividend.NetValue;
                    dividend.GrossValue = ricoDividend.GrossValue;
                    dividend.HomeBroker = "Rico";
                    dividend.PaymentDate = ricoDividend.EventDate;
                    dividend.IdStock = stock.IdStock;
                    dividend.IdPortfolio = portfolio.IdPortfolio;
                    dividend.BaseQuantity = ricoDividend.BaseQuantity;

                    DividendType dividendType = resultServiceDividendType.FirstOrDefault(divType => divType.Name.ToLower().Contains(ricoDividend.Type.ToLower()));

                    if (dividendType == null)
                    {
                        dividend.IdDividendType = (int)DividendTypeEnum.Dividend;
                    }
                    else
                    {
                        dividend.IdDividendType = dividendType.IdDividendType;
                    }

                    if (resultServiceDividend.Value != null && resultServiceDividend.Value.Count() > 0)
                    {
                        Dividend dividendDb = resultServiceDividend.Value.FirstOrDefault(div => (div.PaymentDate.HasValue && div.IdStock == dividend.IdStock && div.IdDividendType == dividend.IdDividendType && div.PaymentDate.Value.Date == dividend.PaymentDate.Value.Date));

                        if (dividendDb != null)
                        {
                            dividend = dividendDb;
                        }
                    }

                    if (dividend.IdDividend == 0)
                    {
                        dividendCeiItems.Add(stock.Symbol);

                        dividend.AutomaticImport = true;
                        _dividendService.Insert(dividend);
                    }
                }
                else
                {
                    _ = _logger.SendInformationAsync(new { Message = string.Format("Stock not found {0} : {1}", ricoDividend.Symbol, idUser) });
                }
            }

            return resultTraderService;
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
    }
}