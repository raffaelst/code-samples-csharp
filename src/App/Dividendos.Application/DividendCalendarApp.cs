using AutoMapper;
using Dividendos.API.Model.Request.DividendCalendar;
using Dividendos.API.Model.Request.Settings;
using Dividendos.API.Model.Request.Stock;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;

using Dividendos.Application.Base;
using Dividendos.Application.Interface;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Enum;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using Dividendos.IexAPIsHelper.Interface;
using Dividendos.Nasdaq.Interface;
using Dividendos.Nasdaq.Interface.Model;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using Dividendos.StatusInvest.Interface;
using K.Logger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dividendos.Application
{
    public class DividendCalendarApp : BaseApp, IDividendCalendarApp
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IDividendCalendarViewService _dividendCalendarViewService;
        private readonly ILogger _logger;
        private readonly IStatusInvestHelper _statusInvestHelper;
        private readonly IDeviceService _deviceService;
        private readonly IDividendCalendarService _dividendCalendarService;
        private readonly IDividendTypeService _dividendTypeService;

        private readonly IStockService _stockService;
        private readonly IDividendService _dividendService;
        private readonly INotificationService _notificationService;
        private readonly ICacheService _cacheService;
        private readonly INasdaqHelper _nasdaqHelper;
        private readonly IOperationItemService _operationItemService;
        private readonly INotificationHistoricalService _notificationHistoricalService;
        private readonly ISettingsService _settingsService;
        private readonly IGlobalAuthenticationService _globalAuthenticationService;
        private readonly ISystemSettingsService _systemSettingsService;
        private readonly IIexAPIsHelper _iexAPIsHelper;

        public DividendCalendarApp(IMapper mapper,
            IUnitOfWork uow,
            IDividendCalendarViewService dividendCalendarViewService,
            IDividendCalendarService dividendCalendarService,
            ILogger logger,
            IStatusInvestHelper statusInvestHelper,
            IDeviceService deviceService,
            IStockService stockService,
            IDividendTypeService dividendTypeService,
            IDividendService dividendService,
            INotificationService notificationService,
            ICacheService cacheService,
            INasdaqHelper nasdaqHelper,
            IOperationItemService operationItemService,
            INotificationHistoricalService notificationHistoricalService,
            ISettingsService settingsService,
            IGlobalAuthenticationService globalAuthenticationService,
            ISystemSettingsService systemSettingsService,
            IIexAPIsHelper iexAPIsHelper)
        {
            _mapper = mapper;
            _uow = uow;
            _dividendCalendarViewService = dividendCalendarViewService;
            _logger = logger;
            _statusInvestHelper = statusInvestHelper;
            _deviceService = deviceService;
            _dividendCalendarService = dividendCalendarService;
            _stockService = stockService;
            _dividendTypeService = dividendTypeService;
            _dividendService = dividendService;
            _notificationService = notificationService;
            _cacheService = cacheService;
            _nasdaqHelper = nasdaqHelper;
            _operationItemService = operationItemService;
            _notificationHistoricalService = notificationHistoricalService;
            _settingsService = settingsService;
            _globalAuthenticationService = globalAuthenticationService;
            _systemSettingsService = systemSettingsService;
            _iexAPIsHelper = iexAPIsHelper;
        }

        public void GetAndUpdateFromSIFIIs()
        {
            GetAndUpdateFromSIBase(StockTypeEnum.FII);
        }

        public void GetAndUpdateFromSIStocks()
        {
            GetAndUpdateFromSIBase(StockTypeEnum.Stocks);
        }

        public void GetAndUpdateFromSIBDRs()
        {
            GetAndUpdateFromSIBase(StockTypeEnum.BDR);
        }

        public void GetAllAndUpdateFromSIFIIs()
        {
            GetAllAndUpdateFromSIBase(StockTypeEnum.FII);
        }

        public void GetAllAndUpdateFromSIStocks()
        {
            GetAllAndUpdateFromSIBase(StockTypeEnum.Stocks);
        }

        public void GetAllAndUpdateFromSIBDRs()
        {
            GetAllAndUpdateFromSIBase(StockTypeEnum.BDR);
        }

        private void GetAndUpdateFromSIBase(StockTypeEnum stockType)
        {
            try
            {
                ResultServiceObject<Stock> resultStockServiceObject = null;

                using (_uow.Create())
                {
                    resultStockServiceObject = _stockService.GetByLastDividendUpdateSyncOrderingAscAndStockType((int)CountryEnum.Brazil, stockType);
                    _stockService.UpdateLastDividendUpdateSync(resultStockServiceObject.Value);
                }

                List<DividendCalendar> dividendCalendarsWaitApproval = new List<DividendCalendar>();

                IEnumerable<StatusInvest.Interface.Model.DividendCalendarItem> dividendCalendarItems = null;
                dividendCalendarItems = _statusInvestHelper.GetDividendCalendar(DateTime.Now.AddYears(-1), DateTime.Now.AddYears(2), resultStockServiceObject.Value.Symbol, (StatusInvest.Interface.Model.AssetsTypeEnum)resultStockServiceObject.Value.IdStockType).Result;

                if (dividendCalendarItems != null && dividendCalendarItems.Count() > 0)
                {
                    using (_uow.Create())
                    {
                        foreach (var item in dividendCalendarItems)
                        {
                            DividendType dividendType = GetCorrespondentDividendType(item);

                            if (dividendType != null)
                            {
                                DividendCalendar dividendCalendar = new DividendCalendar();
                                dividendCalendar.IdStock = resultStockServiceObject.Value.IdStock;
                                dividendCalendar.IdDividendType = dividendType.IdDividendType;
                                dividendCalendar.DataCom = DateTime.ParseExact(item.DataCom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                dividendCalendar.IdCountry = (int)CountryEnum.Brazil;
                                dividendCalendar.PaymentUndefined = false;
                                dividendCalendar.PaymentDatepartiallyUndefined = false;

                                if (item.PaymentDate.Equals("-"))
                                {
                                    dividendCalendar.PaymentDate = null;
                                }
                                else
                                {
                                    dividendCalendar.PaymentDate = DateTime.ParseExact(item.PaymentDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                }

                                dividendCalendar.Value = decimal.Round(decimal.Parse(item.Value, CultureInfo.CreateSpecificCulture("pt-BR")), 6);
                                dividendCalendarsWaitApproval.Add(dividendCalendar);
                            }
                        }
                    }

                    using (_uow.Create())
                    {
                        _dividendCalendarService.Save(dividendCalendarsWaitApproval);
                    }
                }
            }
            catch (Exception exception)
            {
                SendAdminNotification(exception.Message);
                _logger.SendErrorAsync(exception);
            }
        }

        private void GetAllAndUpdateFromSIBase(StockTypeEnum stockType)
        {
            ResultServiceObject<IEnumerable<Stock>> resultStockServiceObject = null;

            using (_uow.Create())
            {
                resultStockServiceObject = _stockService.GetAllByStockType((int)CountryEnum.Brazil, stockType);
            }

            foreach (var itemStock in resultStockServiceObject.Value)
            {
                List<DividendCalendar> dividendCalendarsWaitApproval = new List<DividendCalendar>();

                IEnumerable<StatusInvest.Interface.Model.DividendCalendarItem> dividendCalendarItems = null;

                try
                {
                    using (_uow.Create())
                    {
                        _stockService.UpdateLastDividendUpdateSync(itemStock);
                    }

                    dividendCalendarItems = _statusInvestHelper.GetDividendCalendar(DateTime.Now.AddYears(-1), DateTime.Now.AddYears(2), itemStock.Symbol, (StatusInvest.Interface.Model.AssetsTypeEnum)itemStock.IdStockType).Result;


                    if (dividendCalendarItems != null && dividendCalendarItems.Count() > 0)
                    {
                        using (_uow.Create())
                        {
                            foreach (var item in dividendCalendarItems)
                            {
                                DividendType dividendType = GetCorrespondentDividendType(item);

                                if (dividendType != null)
                                {
                                    DividendCalendar dividendCalendar = new DividendCalendar();
                                    dividendCalendar.IdStock = itemStock.IdStock;
                                    dividendCalendar.IdDividendType = dividendType.IdDividendType;
                                    dividendCalendar.DataCom = DateTime.ParseExact(item.DataCom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    dividendCalendar.IdCountry = (int)CountryEnum.Brazil;
                                    dividendCalendar.PaymentUndefined = false;
                                    dividendCalendar.PaymentDatepartiallyUndefined = false;

                                    if (item.PaymentDate.Equals("-"))
                                    {
                                        dividendCalendar.PaymentDate = null;
                                    }
                                    else
                                    {
                                        dividendCalendar.PaymentDate = DateTime.ParseExact(item.PaymentDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    }

                                    dividendCalendar.Value = decimal.Round(decimal.Parse(item.Value, CultureInfo.CreateSpecificCulture("pt-BR")), 6);
                                    dividendCalendarsWaitApproval.Add(dividendCalendar);
                                }
                            }
                        }

                        using (_uow.Create())
                        {
                            //delete all itens data-com + 2 days
                            _dividendCalendarService.DeleteByStockAllFutureItens(itemStock.IdStock);

                            _dividendCalendarService.Save(dividendCalendarsWaitApproval);
                        }
                    }
                }
                catch (Exception exception)
                {
                    //SendAdminNotification(exception.Message);
                    //_logger.SendErrorAsync(exception);
                }
            }
        }

        public void GetDividendsFromIexAPI()
        {
            ResultServiceObject<IEnumerable<Stock>> resultStockServiceObject = null;

            using (_uow.Create())
            {
                resultStockServiceObject = _stockService.GetAllByCountryOrderByLastDividendUpdateSync((int)CountryEnum.EUA);
            }

            foreach (var itemStock in resultStockServiceObject.Value)
            {
                List<DividendCalendar> dividendCalendarsWaitApproval = new List<DividendCalendar>();

                IEnumerable<IexAPIsHelper.Interface.Model.DividendCalendarItem> dividendCalendarItems = null;

                try
                {
                    using (_uow.Create())
                    {
                        _stockService.UpdateLastDividendUpdateSync(itemStock);
                    }

                    dividendCalendarItems = _iexAPIsHelper.GetDividendCalendar(itemStock.Symbol);

                    if (dividendCalendarItems != null && dividendCalendarItems.Count() > 0)
                    {
                        using (_uow.Create())
                        {
                            foreach (var item in dividendCalendarItems)
                            {
                                DividendCalendar dividendCalendar = new DividendCalendar();
                                dividendCalendar.IdStock = itemStock.IdStock;
                                dividendCalendar.IdDividendType = (int)DividendTypeEnum.Dividend;
                                dividendCalendar.DataCom = DateTime.ParseExact(item.DividendExDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).AddDays(-1);
                                dividendCalendar.IdCountry = (int)CountryEnum.Brazil;
                                dividendCalendar.PaymentUndefined = false;
                                dividendCalendar.PaymentDatepartiallyUndefined = false;

                                if (item.PaymentDate.Equals("-"))
                                {
                                    dividendCalendar.PaymentDate = null;
                                }
                                else
                                {
                                    dividendCalendar.PaymentDate = DateTime.ParseExact(item.PaymentDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                                }

                                dividendCalendar.Value = decimal.Round(decimal.Parse(item.Value, CultureInfo.CreateSpecificCulture("en-US")), 6);
                                dividendCalendarsWaitApproval.Add(dividendCalendar);
                            }
                        }

                        using (_uow.Create())
                        {
                            //delete all itens data-com + 2 days
                            _dividendCalendarService.DeleteByStockAllFutureItens(itemStock.IdStock);

                            _dividendCalendarService.Save(dividendCalendarsWaitApproval);
                        }
                    }
                }
                catch (Exception exception)
                {
                    //SendAdminNotification(exception.Message);
                    //_logger.SendErrorAsync(exception);
                }
            }
        }

        public void GetAndUpdateFromNasdaq(DateTime starDate, DateTime endDate)
        {
            try
            {
                IEnumerable<Nasdaq.Interface.Model.DividendCalendarItem> dividendCalendarItems = null;

                do
                {
                    List<DividendCalendar> dividendCalendars = new List<DividendCalendar>();

                    dividendCalendarItems = _nasdaqHelper.GetDividendCalendar(starDate);

                    using (_uow.Create())
                    {
                        foreach (var item in dividendCalendarItems)
                        {
                            ResultServiceObject<Stock> stock = _stockService.GetBySymbolOrLikeOldSymbol(item.Ticker, (int)CountryEnum.EUA);

                            if (stock.Value != null)
                            {
                                DividendCalendar dividendCalendar = new DividendCalendar();
                                dividendCalendar.IdStock = stock.Value.IdStock;
                                dividendCalendar.IdDividendType = (int)DividendTypeEnum.Dividend;
                                dividendCalendar.DataCom = DateTime.ParseExact(item.DividendExDate, "MM/dd/yyyy", CultureInfo.InvariantCulture).AddDays(-1);
                                dividendCalendar.IdCountry = (int)CountryEnum.EUA;
                                if (item.PaymentDate.Equals("N/A"))
                                {
                                    dividendCalendar.PaymentDate = null;
                                }
                                else
                                {
                                    dividendCalendar.PaymentDate = DateTime.ParseExact(item.PaymentDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                                }
                                dividendCalendar.Value = decimal.Round(decimal.Parse(item.Value, CultureInfo.CreateSpecificCulture("en-US")), 6);
                                dividendCalendars.Add(dividendCalendar);
                            }
                        }
                    }

                    using (_uow.Create())
                    {
                        _dividendCalendarService.Save(dividendCalendars);
                    }

                    starDate = starDate.AddDays(1);

                } while (starDate <= endDate);
            }
            catch (Exception exception)
            {
                SendAdminNotification(exception.Message);
                _logger.SendErrorAsync(exception);
            }

        }

        public void ApproveAndUpdateDividendCalendar()
        {
            List<DividendCalendar> dividendCalendars = new List<DividendCalendar>();

            ResultServiceObject<IEnumerable<DividendCalendarWaitApproval>> resultDividendCalendarWaitApprovalServiceObject = null;

            using (_uow.Create())
            {
                resultDividendCalendarWaitApprovalServiceObject = _dividendCalendarService.GetAllWaitApproval();
            }

            foreach (var itemDividendCalendarWaitApproval in resultDividendCalendarWaitApprovalServiceObject.Value)
            {
                using (_uow.Create())
                {
                    var dividendCalendarItens = _dividendCalendarService.GetByPaymentDateAndStock(DateTime.Now.AddMonths(-1), DateTime.Now.AddYears(2), itemDividendCalendarWaitApproval.IdStock);

                    foreach (var dividendCalendar in dividendCalendarItens.Value)
                    {
                        var dividendCalendarToRemove = resultDividendCalendarWaitApprovalServiceObject.Value.FirstOrDefault(item => item.DataCom.Equals(dividendCalendar.DataCom) &&
                        item.Value.Equals(dividendCalendar.Value) &&
                        item.IdStock.Equals(dividendCalendar.IdStock) &&
                        item.PaymentDate.Equals(dividendCalendar.PaymentDate));

                        if (dividendCalendarToRemove == null)
                        {
                            _dividendCalendarService.Delete(dividendCalendar);
                        }
                    }

                    _dividendCalendarService.DeleteWaitApproval(itemDividendCalendarWaitApproval.DividendCalendarWaitApprovalID);
                }

                dividendCalendars.Add(_mapper.Map<DividendCalendar>(itemDividendCalendarWaitApproval));
            }
            

            using (_uow.Create())
            {
                _dividendCalendarService.Save(dividendCalendars);
            }
        }

        private void SendNotificationNewDividends(List<KeyValuePair<string, string>> dicPushNewDividends)
        {
            List<KeyValuePair<string, string>> keyValueFinal = new List<KeyValuePair<string, string>>();

            //Group notifications by user
            foreach (var itemPush in dicPushNewDividends)
            {
                IEnumerable<KeyValuePair<string, string>> stocks = dicPushNewDividends.Where(item => item.Key.Equals(itemPush.Key)).Distinct();

                StringBuilder stringBuilder = new StringBuilder();

                foreach (var item in stocks)
                {
                    bool firstExecutionLoop = true;

                    if (firstExecutionLoop)
                    {
                        if (stringBuilder.Length < 170)
                        {
                            stringBuilder.Append(string.Format(" {0}", item.Value));
                        }

                        firstExecutionLoop = false;
                    }
                    else
                    {
                        if (stringBuilder.Length < 170)
                        {
                            stringBuilder.Append(string.Format(", {0}", item.Value));
                        }
                    }
                }

                keyValueFinal.Add(new KeyValuePair<string, string>(itemPush.Key, stringBuilder.ToString()));
            }

            foreach (var itemPush in keyValueFinal)
            {
                using (_uow.Create())
                {
                    ResultServiceObject<Settings> settings = _settingsService.GetByUser(itemPush.Key);

                    string pushMessage = string.Empty;
                    string pushMessageTitle = string.Empty;

                    if (settings.Value == null || settings.Value.PushNewDividend)
                    {
                        pushMessage = string.Format("🎉 Você tem o(s) seguinte(s) provento(s) agendado(s):{0}. Veja mais detalhes no App Dividendos.me! 💰", itemPush.Value);
                        pushMessageTitle = "🤑 Tem novidade nos seus dividendos 🤑";
                    }

                    if (!string.IsNullOrEmpty(pushMessage) && !string.IsNullOrEmpty(pushMessageTitle))
                    {
                        ResultServiceObject<IEnumerable<Device>> devices = _deviceService.GetByUser(itemPush.Key);

                        _notificationHistoricalService.New(pushMessageTitle, pushMessage, itemPush.Key, AppScreenNameEnum.HomeToday.ToString(), PushRedirectTypeEnum.Internal.ToString(), null, _cacheService);

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
        }

        public void CreateDividendForManualPortfolios()
        {
            List<KeyValuePair<string, string>> dicPushNewDividends = new List<KeyValuePair<string, string>>();

            ResultServiceObject<IEnumerable<DividendCalendarView>> resultDividendCalendarServiceObject;

            //Get all dividend calendar with datacom + 1 day equal today
            using (_uow.Create())
            {
                resultDividendCalendarServiceObject = _dividendCalendarViewService.GetAllByDataEx(DateTime.Now.Date);
            }

            foreach (var dividendCalendarItem in resultDividendCalendarServiceObject.Value)
            {
                ResultServiceObject<IEnumerable<DividendInfoView>> resultPortfoliosServiceObject;

                //Get list of user that have this stock in portfolio
                using (_uow.Create())
                {
                    resultPortfoliosServiceObject = _dividendService.GetManualPortfoliosWithStock(dividendCalendarItem.IdStock);
                }

                ResultServiceObject<IEnumerable<OperationItem>> resultOperationItemServiceObject;

                DateTime dateOffSet = DateTime.Now.Date.AddDays(-1);

                dateOffSet = new DateTime(dateOffSet.Year, dateOffSet.Month, dateOffSet.Day, 23, 59, 59);

                foreach (var itemPortfolio in resultPortfoliosServiceObject.Value)
                {
                    using (_uow.Create())
                    {
                        resultOperationItemServiceObject = _operationItemService.GetActiveByPortfolioAndDate(itemPortfolio.IdPortfolio, dividendCalendarItem.IdStock, dateOffSet);
                    }

                    if (resultOperationItemServiceObject.Value != null && resultOperationItemServiceObject.Value.Count() > 0)
                    {
                        var sumPurchased = resultOperationItemServiceObject.Value.Where(item => item.IdOperationType.Equals((int)OperationTypeEnum.Compra)).Sum(item => item.NumberOfShares);
                        var sumSold = resultOperationItemServiceObject.Value.Where(item => item.IdOperationType.Equals((int)OperationTypeEnum.Venda)).Sum(item => item.NumberOfShares);

                        var total = sumPurchased - sumSold;

                        if (total > 0)
                        {
                            decimal netValue = 0;
                            decimal grossValue = 0;

                            if (dividendCalendarItem.IdCountry.Equals((int)CountryEnum.EUA))
                            {
                                grossValue = dividendCalendarItem.Value * total;
                                netValue = grossValue * ((decimal)0.70);
                            }
                            else
                            {
                                if (dividendCalendarItem.IdDividendType.Equals((int)DividendTypeEnum.JCP))
                                {
                                    grossValue = dividendCalendarItem.Value * total;
                                    netValue = grossValue * ((decimal)0.85);
                                }
                                else
                                {
                                    grossValue = dividendCalendarItem.Value * total;
                                    netValue = grossValue;
                                }
                            }

                            Dividend dividend = new Dividend();
                            dividend.PaymentDate = dividendCalendarItem.PaymentDate;
                            dividend.IdStock = dividendCalendarItem.IdStock;
                            dividend.IdDividendType = dividendCalendarItem.IdDividendType;
                            dividend.AutomaticImport = true;
                            dividend.HomeBroker = "Sistema";
                            dividend.IdPortfolio = itemPortfolio.IdPortfolio;
                            dividend.BaseQuantity = 0;
                            dividend.NetValue = netValue;
                            dividend.GrossValue = grossValue;

                            if (dividend.NetValue >= (decimal)0.01)
                            {
                                using (_uow.Create())
                                {
                                    //already included?
                                    if (!_dividendService.ExistInDataBase(dividend).Value)
                                    {
                                        dividend.Active = true;
                                        dicPushNewDividends.Add(new KeyValuePair<string, string>(itemPortfolio.IdUser, dividendCalendarItem.StockName));
                                        ResultServiceObject<Dividend> resultDividendInsert = _dividendService.Insert(dividend);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            SendNotificationNewDividends(dicPushNewDividends);
        }

        private DividendType GetCorrespondentDividendType(StatusInvest.Interface.Model.DividendCalendarItem dividendCalendarItem)
        {
            ResultServiceObject<IEnumerable<DividendType>> dividendTypes = _dividendTypeService.GetAll();

            DividendType dividendType = null;


            switch (dividendCalendarItem.Type)
            {
                case "Dividendo":
                    {
                        dividendType = dividendTypes.Value.FirstOrDefault(itemdividendType => itemdividendType.IdDividendType.Equals((int)DividendTypeEnum.Dividend));
                    }
                    break;
                case "Rend. Tributado":
                    {
                        dividendType = dividendTypes.Value.FirstOrDefault(itemdividendType => itemdividendType.IdDividendType.Equals((int)DividendTypeEnum.Rendimento));
                    }
                    break;
                case "Rendimento":
                    {
                        dividendType = dividendTypes.Value.FirstOrDefault(itemdividendType => itemdividendType.IdDividendType.Equals((int)DividendTypeEnum.Rendimento));
                    }
                    break;
                case "JCP":
                    {
                        dividendType = dividendTypes.Value.FirstOrDefault(itemdividendType => itemdividendType.IdDividendType.Equals((int)DividendTypeEnum.JCP));
                    }
                    break;
                case "Amortização":
                    {
                        dividendType = dividendTypes.Value.FirstOrDefault(itemdividendType => itemdividendType.IdDividendType.Equals((int)DividendTypeEnum.Amortizacao));
                    }
                    break;
            }

            return dividendType;
        }

        public ResultResponseObject<DividendCalendarChart> GetDividendChart(int? year = null)
        {
            bool validPeriod = true;

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

            DateTime startDate = new DateTime(year.Value, 1, 1);
            DateTime endDate = new DateTime(year.Value, DateTime.Now.Month, DateTime.Now.Day);

            if (year == DateTime.Now.Year)
            {
                endDate = endDate.AddMonths(2);
            }
            else if (year > DateTime.Now.Year)
            {
                validPeriod = false;
            }
            else if (year < DateTime.Now.Year)
            {
                endDate = new DateTime(year.Value, 12, 31);
            }

            string resultFromCache = _cacheService.GetFromCache(string.Concat("DividendChartYear", year));

            if (resultFromCache == null)
            {
                using (_uow.Create())
                {
                    ResultServiceObject<IEnumerable<DividendView>> resultDividend = new ResultServiceObject<IEnumerable<DividendView>>();

                    resultDividend = _dividendService.GetAllByDate(startDate, endDate);


                    if (resultDividend.Success)
                    {
                        IEnumerable<DividendView> dividends = resultDividend.Value;

                        if (dividends != null && dividends.Count() > 0 && validPeriod)
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

                                if (dividend.PaymentDate.HasValue)
                                {
                                    taskClass.Label = string.Format("<b>{0} <br>Data: {1}<br>", dividend.DividendType, dividend.PaymentDate.Value.ToString("dd/MM/yyyy"));
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

                                dividendChart.Task.ListTask.Add(taskClass);
                            }

                            //DividendCalendar
                            ResultServiceObject<IEnumerable<DividendCalendarView>> dividendCalender = _dividendCalendarService.GetByDataComLimit(DateTime.Now.AddMonths(-1));

                            foreach (DividendCalendarView dividend in dividendCalender.Value)
                            {
                                TaskClass taskClass = new TaskClass();
                                taskClass.Id = Guid.NewGuid().ToString();
                                taskClass.Processid = dividend.IdStock.ToString();
                                taskClass.Height = "25%";
                                taskClass.Toppadding = "40%";

                                string dataPagamento = string.Empty;

                                if (dividend.PaymentDate.HasValue)
                                {
                                    dataPagamento = string.Concat(dataPagamento, dividend.PaymentDate.Value.ToString("dd/MM/yyyy"));
                                }
                                else
                                {
                                    dataPagamento = "Indefinido ";
                                }


                                taskClass.Label = string.Format("<b>{0}: R$ {1}<br>Data Com (com-dividendos/data-base): {2}<br>Data Pagamento: {3}</br>", dividend.DividendType, dividend.Value.ToString(new CultureInfo("pt-br")), dividend.DataCom.ToString("dd/MM/yyyy"), dataPagamento);
                                taskClass.Start = new DateTime(dividend.DataCom.Year, dividend.DataCom.Month, 1).ToString("dd/MM/yyyy");
                                taskClass.End = new DateTime(dividend.DataCom.Year, dividend.DataCom.Month, DateTime.DaysInMonth(dividend.DataCom.Year, dividend.DataCom.Month)).ToString("dd/MM/yyyy");
                                taskClass.Color = "#dcda00";

                                dividendChart.Task.ListTask.Add(taskClass);
                            }

                            dividendChart.TotalReceived = netValueReceived.ToString(new CultureInfo("pt-br"));
                            dividendChart.TotalToReceive = netValueToReceive.ToString(new CultureInfo("pt-br"));
                            dividendChart.TotalNoDate = netValueNoDate.ToString(new CultureInfo("pt-br"));
                            dividendChart.LastUpdated = DateTime.Now.ToString("HH:mm:ss");
                            dividendChart.Title = string.Format("Calendário {0} (Todos proventos de empresas listadas na bolsa brasileira)", year.Value);
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

                        _cacheService.SaveOnCache(string.Concat("DividendChartYear", year), TimeSpan.FromHours(1), JsonConvert.SerializeObject(dividendChart));
                    }
                }
            }
            else
            {
                dividendChart = JsonConvert.DeserializeObject<DividendCalendarChart>(resultFromCache);
            }

            resultServiceObject.Value = dividendChart;
            resultServiceObject.Success = true;

            return resultServiceObject;
        }

        private void SendAdminNotification(string message)
        {
            using (_uow.Create())
            {
                ResultServiceObject<IEnumerable<Device>> devices = _deviceService.GetAdminDevices();

                foreach (Device itemDevice in devices.Value)
                {
                    try
                    {
                        _notificationService.SendPush("Dividend Calendar", message, itemDevice, null);
                    }
                    catch (Exception ex)
                    {
                        _ = _logger.SendErrorAsync(ex);
                    }
                }
            }
        }

        public ResultResponseObject<DividendCalendarResultWrapperVM> GetListByYear(int year, int month, CountryType countryType, DividendCalendarType dividendCalendarType, bool onlyMyStocks, List<API.Model.Request.Stock.StockType> stockTypes)
        {
            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = new DateTime(year, month, 1).AddMonths(1).AddDays(-1);
            TimeSpan timeSpan = endDate - startDate;
            int amountOfDays = timeSpan.Days;
            List<StockTypeEnum> stockTypeEnums = new List<StockTypeEnum>();

            string stockType = "AllStockTypes";

            if (stockTypes != null)
            {
                stockType = string.Join(",", stockTypes);

                foreach (var itemStockType in stockTypes)
                {
                    stockTypeEnums.Add((StockTypeEnum)itemStockType);
                }
            }

            ResultResponseObject<DividendCalendarResultWrapperVM> result = new ResultResponseObject<DividendCalendarResultWrapperVM>();
            result.Success = true;

            string resultFromCache = null;

            if (!onlyMyStocks)
            {
                resultFromCache = _cacheService.GetFromCache(string.Format("DividendListByYear:{0}:{1}:{2}:{3}:{4}", countryType.ToString(), dividendCalendarType.ToString(), year, month, stockType));
            }

            if (resultFromCache == null)
            {
                using (_uow.Create())
                {
                    string monthName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(new CultureInfo("pt-BR").DateTimeFormat.GetMonthName(month));

                    result.Value = new DividendCalendarResultWrapperVM() { Month = monthName, DividendCalendarWrappers = new List<DividendCalendarWrapperVM>() };

                    ResultServiceObject<IEnumerable<DividendCalendarView>> resultServiceObject = null;

                    if (dividendCalendarType == DividendCalendarType.ByDataCom)
                    {
                        resultServiceObject = _dividendCalendarViewService.GetByDataComByYear(startDate, endDate, (CountryEnum)countryType, _globalAuthenticationService.IdUser, onlyMyStocks, stockTypeEnums);

                        if (resultServiceObject.Value != null)
                        {
                            int currentDay = 0;

                            for (int i = 0; i <= amountOfDays; i++)
                            {
                                DividendCalendarWrapperVM dividendCalendarWrapperVM = new DividendCalendarWrapperVM();
                                dividendCalendarWrapperVM.Date = startDate.AddDays(currentDay).ToString("dd/MM/yyyy");

                                IEnumerable<DividendCalendarView> dividendCalendarViews = resultServiceObject.Value.Where(item => item.DataCom.Date.Equals(startDate.AddDays(currentDay)));

                                dividendCalendarWrapperVM.Dividends = _mapper.Map<IEnumerable<DividendCalendarVM>>(dividendCalendarViews);

                                result.Value.DividendCalendarWrappers.Add(dividendCalendarWrapperVM);

                                currentDay++;
                            }
                        }
                    }
                    else
                    {
                        resultServiceObject = _dividendCalendarViewService.GetByPaymentDateByYear(startDate, endDate, (CountryEnum)countryType, _globalAuthenticationService.IdUser, onlyMyStocks, stockTypeEnums);

                        if (resultServiceObject.Value != null)
                        {
                            int currentDay = 0;

                            for (int i = 0; i <= amountOfDays; i++)
                            {
                                DividendCalendarWrapperVM dividendCalendarWrapperVM = new DividendCalendarWrapperVM();
                                dividendCalendarWrapperVM.Date = startDate.AddDays(currentDay).ToString("dd/MM/yyyy");

                                IEnumerable<DividendCalendarView> dividendCalendarViews = resultServiceObject.Value.Where(item => item.PaymentDate.HasValue && item.PaymentDate.Value.Equals(startDate.AddDays(currentDay)));


                                dividendCalendarWrapperVM.Dividends = _mapper.Map<IEnumerable<DividendCalendarVM>>(dividendCalendarViews);

                                result.Value.DividendCalendarWrappers.Add(dividendCalendarWrapperVM);

                                currentDay++;
                            }
                        }
                    }

                    if (!onlyMyStocks)
                    {
                        _cacheService.SaveOnCache(string.Format("DividendListByYear:{0}:{1}:{2}:{3}:{4}", countryType.ToString(), dividendCalendarType.ToString(), year, month, stockType), TimeSpan.FromHours(4), JsonConvert.SerializeObject(result));
                    }
                }
            }
            else
            {
                result = JsonConvert.DeserializeObject<ResultResponseObject<DividendCalendarResultWrapperVM>>(resultFromCache);
                result.Success = true;
            }

            return result;
        }

        public ResultResponseObject<IEnumerable<DividendCalendarVM>> GetListBySymbol(DateTime start, DateTime end, string symbol, DividendCalendarType dividendCalendarType)
        {
            DateTime startDate = new DateTime(start.Year, start.Month, 1);
            DateTime endDate = new DateTime(end.Year, end.Month, 1).AddMonths(1).AddDays(-1);
            TimeSpan timeSpan = endDate - startDate;
            int amountOfDays = timeSpan.Days;

            ResultResponseObject<IEnumerable<DividendCalendarVM>> result = new ResultResponseObject<IEnumerable<DividendCalendarVM>>();

            if (!string.IsNullOrWhiteSpace(symbol))
            {
                using (_uow.Create())
                {
                    ResultServiceObject<IEnumerable<DividendCalendarView>> resultServiceObject = null;

                    if (dividendCalendarType == DividendCalendarType.ByDataCom)
                    {
                        resultServiceObject = _dividendCalendarViewService.GetByDataComBySymbol(startDate, endDate, symbol);

                        result = _mapper.Map<ResultResponseObject<IEnumerable<DividendCalendarVM>>>(resultServiceObject);
                    }
                    else
                    {
                        resultServiceObject = _dividendCalendarViewService.GetByPaymentDateBySymbol(startDate, endDate, symbol);

                        result = _mapper.Map<ResultResponseObject<IEnumerable<DividendCalendarVM>>>(resultServiceObject);
                    }
                }
            }

            return result;
        }

        public void SendNotificationDividendDataComRemind()
        {
            List<Tuple<string, string>> keyValueFinal = new List<Tuple<string, string>>();

            ResultServiceObject<IEnumerable<DividendCalendarView>> resultDividendCalendarServiceObject;

            using (_uow.Create())
            {
                resultDividendCalendarServiceObject = _dividendCalendarViewService.GetByDataCom(DateTime.Now.Date.AddDays(2));
            }

            foreach (var itemDividendCalendarView in resultDividendCalendarServiceObject.Value)
            {
                ResultServiceObject<IEnumerable<string>> resultUsersServiceObject;

                //Get list of user that have this stock in portfolio
                using (_uow.Create())
                {
                    resultUsersServiceObject = _dividendService.GetAllUsersWithStock(itemDividendCalendarView.IdStock);
                }

                foreach (var itemUser in resultUsersServiceObject.Value)
                {
                    var itemFound = keyValueFinal.Where(item => item.Item1.Equals(itemUser)).FirstOrDefault();

                    if (itemFound != null)
                    {
                        if (itemFound.Item2.Length < 170)
                        {
                            keyValueFinal.RemoveAll(item => item.Item1.Equals(itemUser));
                            keyValueFinal.Add(new Tuple<string, string>(itemUser, string.Concat(itemFound.Item2, string.Format(", {0}", itemDividendCalendarView.StockName))));
                        }
                    }
                    else
                    {
                        keyValueFinal.Add(new Tuple<string, string>(itemUser, string.Format("{0}", itemDividendCalendarView.StockName)));
                    }
                }
            }


            foreach (var itemPush in keyValueFinal)
            {
                using (_uow.Create())
                {
                    ResultServiceObject<Settings> settings = _settingsService.GetByUser(itemPush.Item1);

                    string pushMessage = string.Empty;
                    string pushMessageTitle = string.Empty;

                    if (settings.Value == null || settings.Value.PushDataComYourStocks)
                    {
                        pushMessage = string.Format("App Dividendos.me alerta: Você possui em sua carteira as seguintes ações que estão se aproximando da Data-Com: ({0}). Se você vai aproveitar, fique atento! A data limite é {1}.", itemPush.Item2, DateTime.Now.Date.AddDays(2).ToString("dd/MM/yyyy"));
                        pushMessageTitle = "Data-Com chegando fique atento!";
                    }

                    if (!string.IsNullOrEmpty(pushMessage) && !string.IsNullOrEmpty(pushMessageTitle))
                    {
                        ResultServiceObject<IEnumerable<Device>> devices = _deviceService.GetByUser(itemPush.Item1);

                        _notificationHistoricalService.New(pushMessageTitle, pushMessage, itemPush.Item1, AppScreenNameEnum.HomeToday.ToString(), PushRedirectTypeEnum.Internal.ToString(), null, _cacheService);

                        foreach (Device itemDevice in devices.Value)
                        {
                            try
                            {
                                _notificationService.SendPush(pushMessageTitle, pushMessage, itemDevice, new PushRedirect() { PushRedirectType = Entity.Enum.PushRedirectTypeEnum.Internal, AppScreenName = AppScreenNameEnum.HomeToday });
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



        public ResultResponseObject<IEnumerable<DividendCalendarVM>> GetListByYear(string token, int year, CountryType countryType, DividendCalendarType dividendCalendarType)
        {
            ResultResponseObject<IEnumerable<DividendCalendarVM>> finalResult = new ResultResponseObject<IEnumerable<DividendCalendarVM>>() { Success = false };

            if (!string.IsNullOrEmpty(token) && token.Equals("0e4475b2-8177-4a57-a449-57a67a2265ca"))
            {
                DateTime startDate = new DateTime(year, 1, 1);
                DateTime endDate = new DateTime(year, 12, 1).AddMonths(1).AddDays(-1);

                using (_uow.Create())
                {
                    ResultServiceObject<IEnumerable<DividendCalendarView>> resultServiceObject = null;

                    if (dividendCalendarType == DividendCalendarType.ByDataCom)
                    {
                        resultServiceObject = _dividendCalendarViewService.GetByDataComByYear(startDate, endDate, (CountryEnum)countryType, _globalAuthenticationService.IdUser, false, null);

                        finalResult = _mapper.Map<ResultResponseObject<IEnumerable<DividendCalendarVM>>>(resultServiceObject);
                    }
                    else
                    {
                        resultServiceObject = _dividendCalendarViewService.GetByPaymentDateByYear(startDate, endDate, (CountryEnum)countryType, _globalAuthenticationService.IdUser, false, null);

                        finalResult = _mapper.Map<ResultResponseObject<IEnumerable<DividendCalendarVM>>>(resultServiceObject);
                    }
                }
            }

            return finalResult;
        }

        public ResultResponseObject<DividendNextDataComWrapperVM> GetNextDataComByUser()
        {
            DividendNextDataComWrapperVM dividendNextDataComWrapperVM = new DividendNextDataComWrapperVM();

            using (_uow.Create())
            {
                ResultServiceObject<IEnumerable<DividendCalendarView>> resultService = _dividendCalendarViewService.GetNextDataComByUser(DateTime.Now, DateTime.Now.AddDays(30), _globalAuthenticationService.IdUser);

                dividendNextDataComWrapperVM.Dividends = _mapper.Map<IEnumerable<DividendNextDataComVM>>(resultService.Value);
            }

            ResultResponseObject<DividendNextDataComWrapperVM> result = new ResultResponseObject<DividendNextDataComWrapperVM>() { Success = true, Value = dividendNextDataComWrapperVM };

            return result;
        }
    }
}