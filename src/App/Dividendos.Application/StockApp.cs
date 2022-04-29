using AutoMapper;
using K.Logger;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;

using Dividendos.Application.Base;
using Dividendos.Application.Interface;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Finance.Interface;
using Dividendos.Finance.Interface.Model;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Dividendos.Application.Interface.Model;
using Dividendos.Entity.Views;
using Dividendos.TradeMap.Interface.Model;
using Dividendos.TradeMap.Interface;
using Dividendos.CrossCutting.Identity.Models;
using Newtonsoft.Json;
using Dividendos.Entity.Enum;
using Dividendos.API.Model.Request.Stock;
using System.Resources;
using Dividendos.StatusInvest.Interface;
using Dividendos.API.Model.Response.MilkingCows;
using Dividendos.AWS;
using System.Text;
using System.Net.Http;

namespace Dividendos.Application
{
    public class StockApp : BaseApp, IStockApp
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly ISpreadsheetsHelper _iSpreadsheetsHelper;
        private readonly IStockService _stockService;
        private readonly ISystemSettingsService _systemSettingsService;
        private readonly ILogger _logger;
        private readonly IIndicatorSeriesService _indicatorSeriesService;
        private readonly IGlobalAuthenticationService _globalAuthenticationService;
        private readonly ITradeMapHelper _iTradeMapHelper;
        private readonly IUserService _userService;
        private readonly IDeviceService _deviceService;
        private readonly ICompanyService _companyService;
        private readonly ISectorService _sectorService;
        private readonly ISegmentService _segmentService;
        private readonly ISubsectorService _subsectorService;
        private readonly ILogoService _logoService;
        private readonly IMarketMoverService _marketMoverService;
        private readonly INotificationService _notificationService;
        private readonly IStatusInvestHelper _iStatusInvestHelper;
        private readonly ISpreadsheetsUSAHelper _iSpreadsheetsUSAHelper;
        private readonly ICacheService _cacheService;
        private readonly ISettingsService _settingsService;
        private readonly INotificationHistoricalService _notificationHistoricalService;
        private readonly IHolidayService _holidayService;
        private readonly IS3Service _iS3Service;

        public StockApp(IMapper mapper,
            IUnitOfWork uow,
            ISpreadsheetsHelper iSpreadsheetsHelper,
            IIndicatorSeriesService indicatorSeriesService,
            ISystemSettingsService systemSettingsService,
            IStockService stockService,
            IGlobalAuthenticationService globalAuthenticationService,
            ITradeMapHelper iTradeMapHelper,
            ILogger logger,
            IUserService userService,
            ICompanyService companyService,
            ISectorService sectorService,
            ISegmentService segmentService,
            ISubsectorService subsectorService,
            ILogoService logoService,
            IDeviceService deviceService,
            IMarketMoverService marketMoverService,
            INotificationService notificationService,
            IStatusInvestHelper iStatusInvestHelper,
            ISpreadsheetsUSAHelper iSpreadsheetsUSAHelper,
            ICacheService cacheService,
            ISettingsService settingsService,
            INotificationHistoricalService notificationHistoricalService,
            IHolidayService holidayService,
            IS3Service s3Service)
        {
            _mapper = mapper;
            _uow = uow;
            _iSpreadsheetsHelper = iSpreadsheetsHelper;
            _stockService = stockService;
            _indicatorSeriesService = indicatorSeriesService;
            _systemSettingsService = systemSettingsService;
            _iTradeMapHelper = iTradeMapHelper;
            _globalAuthenticationService = globalAuthenticationService;
            _logger = logger;
            _userService = userService;
            _sectorService = sectorService;
            _subsectorService = subsectorService;
            _segmentService = segmentService;
            _logoService = logoService;
            _deviceService = deviceService;
            _companyService = companyService;
            _marketMoverService = marketMoverService;
            _notificationService = notificationService;
            _iStatusInvestHelper = iStatusInvestHelper;
            _iSpreadsheetsUSAHelper = iSpreadsheetsUSAHelper;
            _cacheService = cacheService;
            _settingsService = settingsService;
            _notificationHistoricalService = notificationHistoricalService;
            _holidayService = holidayService;
            _iS3Service = s3Service;
        }


        public async Task SyncStockPriceUsingGoogleFinanceAsync(int idCountry)
        {
            try
            {
                ResultServiceObject<Entity.Entities.SystemSettings> resultStockPriceService = new ResultServiceObject<SystemSettings>();
                List<Entity.Entities.Stock> stocks = new List<Entity.Entities.Stock>();
                string tradeMapCookie = string.Empty;
                string stockPriceService = "1";

                using (_uow.Create())
                {
                    if (idCountry == 1)
                    {
                        resultStockPriceService = _systemSettingsService.GetByKey(Constants.SYSTEM_SETTINGS_STOCK_PRICE_SERVICE);
                    }
                    else
                    {
                        resultStockPriceService = _systemSettingsService.GetByKey(Constants.SYSTEM_SETTINGS_STOCK_PRICE_EUA_SERVICE);
                    }
                }

                if (resultStockPriceService.Success && resultStockPriceService.Value != null)
                {
                    stockPriceService = resultStockPriceService.Value.SettingValue;
                }

                //TradeMap
                if (stockPriceService == "2")
                {
                    await SyncStockPriceUsingTradeMapAsync(idCountry);
                }

                var style = NumberStyles.Number;
                var culture = CultureInfo.CreateSpecificCulture("pt-BR");

                ImportFinanceResult financeResult = null;

                if (idCountry == 1)
                {
                    //Google Finance
                    financeResult = _iSpreadsheetsHelper.ReadEntries();
                }
                else
                {
                    //Google Finance
                    financeResult = _iSpreadsheetsUSAHelper.ReadEntries();
                }

                if (financeResult != null)
                {
                    if (financeResult.ListStock != null && financeResult.ListStock.Count > 0)
                    {
                        using (_uow.Create())
                        {
                            ResultServiceObject<IEnumerable<Entity.Entities.Stock>> result = _stockService.GetAllShowOnPortfolio(idCountry);

                            if (result.Success && result.Value != null && result.Value.Count() > 0)
                            {
                                stocks = result.Value.ToList();
                            }
                        }

                        foreach (var item in financeResult.ListStock)
                        {
                            using (_uow.Create())
                            {
                                //get by stock symbol
                                Entity.Entities.Stock stock = stocks.FirstOrDefault(stockTmp => stockTmp.Symbol == item.IdStock);

                                if (stock != null)
                                {
                                    //ResultServiceObject<Entity.Entities.Stock> resultService = _stockService.GetBySymbol(item.IdStock);
                                    decimal decimalMarketPrice = 0;
                                    decimal lastChangePerc = 0;
                                    DateTime date;

                                    ////update
                                    if (decimal.TryParse(item.MarketPrice, style, culture, out decimalMarketPrice) &&
                                        DateTime.TryParseExact(item.TradeTime, "dd/MM/yyyy HH:mm:ss", culture, DateTimeStyles.None, out date))
                                    {
                                        if (stock.TradeTime < date || stockPriceService == "1")
                                        {
                                            decimal.TryParse(item.LastChangePerc, style, culture, out lastChangePerc);
                                            stock.MarketPrice = decimalMarketPrice;
                                            stock.LastChangePerc = lastChangePerc / 100;
                                            stock.UpdatedDate = DateTime.Now;
                                            stock.TradeTime = date;
                                            _stockService.Update(stock);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (financeResult.ListIndicator != null && financeResult.ListIndicator.Count > 0)
                    {
                        foreach (var item in financeResult.ListIndicator)
                        {
                            decimal perc = 0;
                            decimal points = 0;
                            DateTime calcDate;

                            if (decimal.TryParse(item.Percentage, style, culture, out perc) &&
                                decimal.TryParse(item.Points, style, culture, out points) &&
                                DateTime.TryParseExact(item.TradeTime, "dd/MM/yyyy HH:mm:ss", culture, DateTimeStyles.None, out calcDate))
                            {
                                calcDate = calcDate.Date;

                                using (_uow.Create())
                                {
                                    ResultServiceObject<IndicatorSeries> resultService = _indicatorSeriesService.GetByCalculationDate(item.IndicatorType, calcDate, 1);

                                    if (resultService.Success)
                                    {
                                        IndicatorSeries indicatorSeries = resultService.Value;

                                        if (indicatorSeries == null)
                                        {
                                            indicatorSeries = new IndicatorSeries();
                                        }

                                        indicatorSeries.CalculationDate = calcDate;
                                        indicatorSeries.IdIndicatorType = item.IndicatorType;
                                        indicatorSeries.IdPeriodType = 1;
                                        indicatorSeries.Perc = perc / 100;
                                        indicatorSeries.Points = points;

                                        if (indicatorSeries.IdIndicatorSeries == 0)
                                        {
                                            resultService = _indicatorSeriesService.Insert(indicatorSeries);
                                        }
                                        else
                                        {
                                            resultService = _indicatorSeriesService.Update(indicatorSeries);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.SendErrorAsync(exception);
                throw;
            }

        }


        public ResultResponseObject<IEnumerable<StockVM>> GetAllByLoggedUser()
        {
            ResultResponseObject<IEnumerable<StockVM>> resultResponseObject = new ResultResponseObject<IEnumerable<StockVM>>();

            using (_uow.Create())
            {
                ResultServiceObject<IEnumerable<Entity.Entities.Stock>> resultServiceObject = _stockService.GetByUser(_globalAuthenticationService.IdUser);

                resultResponseObject = _mapper.Map<ResultResponseObject<IEnumerable<StockVM>>>(resultServiceObject);
            }

            return resultResponseObject;
        }

        public ResultResponseObject<IEnumerable<StockSymbolVM>> GetLikeSymbol(string symbol, int? idCountry)
        {
            ResultResponseObject<IEnumerable<StockSymbolVM>> resultResponseObject = new ResultResponseObject<IEnumerable<StockSymbolVM>>();

            using (_uow.Create())
            {
                ResultServiceObject<IEnumerable<Entity.Views.StockView>> resultServiceObject = this.GetLikeSymbolBase(symbol, idCountry);

                resultResponseObject = _mapper.Map<ResultResponseObject<IEnumerable<StockSymbolVM>>>(resultServiceObject);
            }

            return resultResponseObject;
        }

        public ResultResponseObject<IEnumerable<Dividendos.API.Model.Response.v3.StockSymbolVM>> GetLikeSymbolV3(string symbol, int? idCountry)
        {
            ResultResponseObject<IEnumerable<Dividendos.API.Model.Response.v3.StockSymbolVM>> resultResponseObject = new ResultResponseObject<IEnumerable<Dividendos.API.Model.Response.v3.StockSymbolVM>>();

            using (_uow.Create())
            {
                ResultServiceObject<IEnumerable<Entity.Views.StockView>> resultServiceObject = this.GetLikeSymbolBase(symbol, idCountry);

                if (resultServiceObject.Value == null)
                {
                    resultServiceObject.Value = this.GetLikeCompanyNameBase(symbol, idCountry).Value;
                }
                else if (resultServiceObject.Value.Count() < 3)
                {
                    resultServiceObject.Value = resultServiceObject.Value.Concat(this.GetLikeCompanyNameBase(symbol, idCountry).Value);
                }

                resultResponseObject = _mapper.Map<ResultResponseObject<IEnumerable<Dividendos.API.Model.Response.v3.StockSymbolVM>>>(resultServiceObject);
            }

            return resultResponseObject;
        }

        private ResultServiceObject<IEnumerable<Entity.Views.StockView>> GetLikeSymbolBase(string symbol, int? idCountry)
        {
            ResultServiceObject<IEnumerable<Entity.Views.StockView>> resultServiceObject = null;

            if (!idCountry.HasValue)
            {
                idCountry = 1;
            }

            resultServiceObject = _stockService.GetLikeSymbol(symbol, idCountry.Value);

            return resultServiceObject;
        }

        private ResultServiceObject<IEnumerable<Entity.Views.StockView>> GetLikeCompanyNameBase(string symbol, int? idCountry)
        {
            ResultServiceObject<IEnumerable<Entity.Views.StockView>> resultServiceObject = null;

            if (!idCountry.HasValue)
            {
                idCountry = 1;
            }

            resultServiceObject = _stockService.GetLikeCompanyName(symbol, idCountry.Value);

            return resultServiceObject;
        }

        public ResultResponseObject<StockStatementViewModel> GetPortfolioStatementView(long idStock)
        {
            ResultResponseObject<StockStatementViewModel> resultServiceObject = new ResultResponseObject<StockStatementViewModel>();

            using (_uow.Create())
            {
                ResultServiceObject<StockStatementView> resultStock = _stockService.GetByIdStock(idStock);

                if (resultStock.Success)
                {
                    StockStatementView stockStatementView = resultStock.Value;
                    ResultServiceObject<IEnumerable<PortfolioStatementView>> result = new ResultServiceObject<IEnumerable<PortfolioStatementView>>();


                    decimal perc = stockStatementView.PerformancePerc * 100;
                    StockStatementViewModel stockStatementViewModel = new StockStatementViewModel();
                    stockStatementViewModel.IdStock = stockStatementView.IdStock;
                    stockStatementViewModel.GuidOperation = stockStatementView.GuidOperation;
                    stockStatementViewModel.Company = stockStatementView.Company;
                    stockStatementViewModel.Segment = stockStatementView.Segment;
                    stockStatementViewModel.Symbol = stockStatementView.Symbol;
                    stockStatementViewModel.Logo = stockStatementView.Logo;
                    stockStatementViewModel.PerformancePerc = GetSignal(perc) + perc.ToString("n2", new CultureInfo("pt-br"));
                    stockStatementViewModel.AveragePrice = stockStatementView.AveragePrice.ToString("n2", new CultureInfo("pt-br"));
                    stockStatementViewModel.MarketPrice = stockStatementView.MarketPrice.ToString("n2", new CultureInfo("pt-br"));
                    stockStatementViewModel.NetValue = GetSignal(stockStatementView.NetValue) + stockStatementView.NetValue.ToString("n2", new CultureInfo("pt-br"));
                    stockStatementViewModel.TotalDividends = stockStatementView.TotalDividends.ToString("n2", new CultureInfo("pt-br"));
                    stockStatementViewModel.NumberOfShares = stockStatementView.NumberOfShares.ToString("0.#############################", new CultureInfo("pt-br"));
                    stockStatementViewModel.TotalMarket = stockStatementView.TotalMarket.ToString("n2", new CultureInfo("pt-br"));
                    stockStatementViewModel.Total = stockStatementView.Total.ToString("n2", new CultureInfo("pt-br"));

                    stockStatementViewModel.PerformancePercN = perc;
                    stockStatementViewModel.AveragePriceN = stockStatementView.AveragePrice;
                    stockStatementViewModel.MarketPriceN = stockStatementView.MarketPrice;
                    stockStatementViewModel.NetValueN = stockStatementView.NetValue;
                    stockStatementViewModel.TotalDividendsN = stockStatementView.TotalDividends;
                    stockStatementViewModel.NumberOfSharesN = stockStatementView.NumberOfShares;
                    stockStatementViewModel.TotalMarketN = stockStatementView.TotalMarket;
                    stockStatementViewModel.TotalN = stockStatementView.Total;
                    stockStatementViewModel.LastUpdated = DateTime.Now.ToString("HH:mm:ss");

                    resultServiceObject.Value = stockStatementViewModel;
                    resultServiceObject.Success = true;
                }
            }

            return resultServiceObject;
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

        public async Task SyncStockPriceUsingTradeMapAsync(int idCountry)
        {
            try
            {
                var style = NumberStyles.Number;
                var culture = CultureInfo.CreateSpecificCulture("en-US");
                List<Entity.Entities.Stock> stocks = new List<Entity.Entities.Stock>();
                string tradeMapCookie = string.Empty;
                string excludeTrademap = string.Empty;

                using (_uow.Create())
                {
                    ResultServiceObject<Entity.Entities.SystemSettings> resultExcludeStock = _systemSettingsService.GetByKey(Constants.SYSTEM_SETTINGS_EXCLUDE_STOCK_TRADEMAP);
                    ResultServiceObject<Entity.Entities.SystemSettings> resultSettingsCookie = _systemSettingsService.GetByKey(Constants.SYSTEM_SETTINGS_TRADE_MAP_COOKIE);
                    ResultServiceObject<IEnumerable<Entity.Entities.Stock>> result = _stockService.GetAllShowOnPortfolio(idCountry);

                    if (result.Success && result.Value != null && result.Value.Count() > 0)
                    {
                        stocks = result.Value.ToList();
                    }

                    if (resultSettingsCookie.Success && resultSettingsCookie.Value != null)
                    {
                        tradeMapCookie = resultSettingsCookie.Value.SettingValue;
                    }

                    if (resultExcludeStock.Success && resultExcludeStock.Value != null)
                    {
                        excludeTrademap = resultExcludeStock.Value.SettingValue;
                    }
                }

                if (stocks != null && stocks.Count > 0 && !string.IsNullOrWhiteSpace(tradeMapCookie))
                {

                    if (!string.IsNullOrWhiteSpace(excludeTrademap))
                    {
                        string[] stocksExclude = excludeTrademap.Split(';');

                        if (stocksExclude != null && stocksExclude.Count() > 0)
                        {
                            foreach (string stockExclude in stocksExclude)
                            {
                                stocks.RemoveAll(item => item.Symbol == stockExclude);
                            }
                        }
                    }

                    TradeMapPrices tradeMap = await _iTradeMapHelper.ImportStockPricesAsync(stocks, tradeMapCookie, idCountry);

                    if (tradeMap != null && tradeMap.Success && tradeMap.Result != null && tradeMap.Result != null && tradeMap.Result.Count > 0)
                    {
                        foreach (KeyValuePair<string, string[]> keyValuePair in tradeMap.Result)
                        {
                            using (_uow.Create())
                            {
                                if (keyValuePair.Value.Length > 0)
                                {
                                    string symbol = keyValuePair.Value[1];

                                    Entity.Entities.Stock stock = stocks.FirstOrDefault(stockTmp => stockTmp.Symbol == symbol);

                                    if (stock != null)
                                    {
                                        decimal decimalMarketPrice = 0;
                                        string price = keyValuePair.Value[7];

                                        if (!string.IsNullOrWhiteSpace(price) && price.Contains("E+"))
                                        {
                                            string[] priceParts = price.Split("E+");

                                            if (priceParts != null && priceParts.Length >= 1)
                                            {
                                                decimal firstPart = 0;
                                                long secondPart = 0;

                                                decimal.TryParse(priceParts[0], style, culture, out firstPart);

                                                if (priceParts.Length > 1)
                                                {
                                                    long.TryParse(priceParts[1].Replace(".", string.Empty), out secondPart);
                                                }

                                                if (secondPart > 0)
                                                {
                                                    price = (firstPart * ((decimal)Math.Pow(10, secondPart))).ToString(culture);
                                                }
                                                else
                                                {
                                                    price = firstPart.ToString();
                                                }
                                            }
                                        }


                                        decimal lastChangePerc = 0;
                                        string chgPerc = keyValuePair.Value[2];

                                        if (!string.IsNullOrWhiteSpace(chgPerc) && chgPerc.Contains("E+"))
                                        {
                                            string[] chgParts = price.Split("E+");

                                            if (chgParts != null && chgParts.Length >= 1)
                                            {
                                                decimal firstPart = 0;
                                                long secondPart = 0;

                                                decimal.TryParse(chgParts[0], style, culture, out firstPart);

                                                if (chgParts.Length > 1)
                                                {
                                                    long.TryParse(chgParts[1].Replace(".", string.Empty), out secondPart);
                                                }

                                                if (secondPart > 0)
                                                {
                                                    chgPerc = (firstPart * ((decimal)Math.Pow(10, secondPart))).ToString(culture);
                                                }
                                                else
                                                {
                                                    chgPerc = firstPart.ToString();
                                                }
                                            }
                                        }


                                        ////update
                                        if (decimal.TryParse(price, style, culture, out decimalMarketPrice))
                                        {
                                            decimal.TryParse(chgPerc, style, culture, out lastChangePerc);
                                            stock.MarketPrice = decimalMarketPrice;
                                            stock.UpdatedDate = DateTime.Now;
                                            stock.TradeTime = DateTime.Now;
                                            stock.LastChangePerc = lastChangePerc / 100;
                                            _stockService.Update(stock);
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception exception)
            {
                SendAdminNotification(exception.Message);
                _logger.SendErrorAsync(exception);
            }
        }

        public void SendAdminNotification(string message)
        {
            using (_uow.Create())
            {
                ResultServiceObject<IEnumerable<Device>> devices = _deviceService.GetAdminDevices();

                foreach (Device itemDevice in devices.Value)
                {
                    try
                    {
                        _notificationService.SendPush("Falha na integração", message, itemDevice, null);
                    }
                    catch (Exception ex)
                    {
                        _ = _logger.SendErrorAsync(ex);
                    }
                }
            }
        }

        public void ImportUsStocks(int stockType)
        {
            try
            {
                List<Entity.Entities.Stock> lstStock = new List<Entity.Entities.Stock>();
                ResultServiceObject<Entity.Entities.SystemSettings> resultSettingsCookie = null;
                ResultServiceObject<IEnumerable<Entity.Entities.Stock>> resultStockDb = null;

                using (_uow.Create())
                {
                    resultSettingsCookie = _systemSettingsService.GetByKey(Constants.SYSTEM_SETTINGS_TRADE_MAP_COOKIE);

                    resultStockDb = _stockService.GetAllByCountry(2);

                    if (resultStockDb.Success && resultStockDb.Value != null && resultStockDb.Value.Count() > 0)
                    {
                        lstStock = resultStockDb.Value.ToList();
                    }

                }

                TradeMapUSAStocks tradeMapUSAStocks = null;

                if (resultSettingsCookie.Success && resultSettingsCookie.Value != null)
                {
                    tradeMapUSAStocks = _iTradeMapHelper.ImportReistsAndEtfs(resultSettingsCookie.Value.SettingValue, 2, stockType, lstStock);

                    //switch (stockType)
                    //{
                    //    case 1:
                    //        tradeMapUSAStocks = await _iTradeMapHelper.ImportUsStocks(resultSettingsCookie.Value.SettingValue);
                    //        break;
                    //    case 2:
                    //    case 3:
                    //        tradeMapUSAStocks = await _iTradeMapHelper.ImportReistsAndEtfs(resultSettingsCookie.Value.SettingValue, 2, stockType, lstStock);
                    //        break;
                    //    default:
                    //        break;
                    //}


                }

                List<Entity.Entities.Company> lstCompany = new List<Company>();
                List<Entity.Entities.Sector> lstSector = new List<Sector>();
                List<Entity.Entities.Subsector> lstSubsector = new List<Subsector>();
                List<Entity.Entities.Segment> lstSegement = new List<Segment>();
                List<Entity.Entities.Logo> lstLogo = new List<Logo>();

                ResultServiceObject<IEnumerable<Entity.Entities.Sector>> resultSector = null;
                ResultServiceObject<IEnumerable<Entity.Entities.Company>> resultCompany = null;
                ResultServiceObject<IEnumerable<Entity.Entities.Subsector>> resultSubsector = null;
                ResultServiceObject<IEnumerable<Entity.Entities.Segment>> resultSegment = null;
                ResultServiceObject<IEnumerable<Entity.Entities.Logo>> resultLogo = null;

                using (_uow.Create())
                {
                    resultCompany = _companyService.GetAllByCountry(2);

                    if (resultCompany.Success && resultCompany.Value != null && resultCompany.Value.Count() > 0)
                    {
                        lstCompany = resultCompany.Value.ToList();
                    }

                    resultSector = _sectorService.GetAll();

                    if (resultSector.Success && resultSector.Value != null && resultSector.Value.Count() > 0)
                    {
                        lstSector = resultSector.Value.ToList();
                    }

                    resultSubsector = _subsectorService.GetAll();

                    if (resultSubsector.Success && resultSubsector.Value != null && resultSubsector.Value.Count() > 0)
                    {
                        lstSubsector = resultSubsector.Value.ToList();
                    }

                    resultSegment = _segmentService.GetAll();

                    if (resultSegment.Success && resultSegment.Value != null && resultSegment.Value.Count() > 0)
                    {
                        lstSegement = resultSegment.Value.ToList();
                    }


                    resultLogo = _logoService.GetAll();

                    if (resultLogo.Success && resultLogo.Value != null && resultLogo.Value.Count() > 0)
                    {
                        lstLogo = resultLogo.Value.ToList();
                    }
                }

                if (tradeMapUSAStocks != null && tradeMapUSAStocks.ResultStock != null && tradeMapUSAStocks.ResultStock.Count() > 0)
                {
                    //if (resultCompany != null)
                    //{
                    foreach (ResultStock resultStock in tradeMapUSAStocks.ResultStock)
                    {

                        using (_uow.Create())
                        {
                            if (!lstCompany.ToList().Exists(cp => cp.Code == resultStock.CdCompany))
                            {
                                Logo logo = new Logo();

                                if (string.IsNullOrWhiteSpace(resultStock.Logo))
                                {
                                    logo = lstLogo.FirstOrDefault(sec => sec.CompanyCode.Trim() == "DEFAULT");
                                }
                                else
                                {
                                    if (!lstLogo.Exists(sec => sec.CompanyCode.Trim() == resultStock.CdCompany.Trim()))
                                    {
                                        try
                                        {

                                            logo.CompanyCode = resultStock.CdCompany;
                                            logo.LogoImage = resultStock.Logo;
                                            logo = _logoService.Insert(logo).Value;
                                            //
                                            lstLogo.Add(logo);
                                        }
                                        catch (Exception ex)
                                        {
                                            throw ex;
                                        }
                                    }
                                    else
                                    {
                                        logo = lstLogo.FirstOrDefault(sec => sec.CompanyCode.Trim() == resultStock.CdCompany.Trim());
                                    }
                                }


                                Sector sector = null;

                                if (!lstSector.Exists(sec => sec.Name.Trim() == resultStock.Sector.Trim()))
                                {
                                    try
                                    {
                                        sector = new Sector();
                                        sector.IdCountry = 2;
                                        sector.Name = resultStock.Sector.Trim();
                                        sector = _sectorService.Insert(sector).Value;
                                        lstSector.Add(sector);
                                    }
                                    catch (Exception ex)
                                    {

                                        throw ex;
                                    }

                                }
                                else
                                {
                                    sector = lstSector.FirstOrDefault(sec => sec.Name.Trim() == resultStock.Sector.Trim());
                                }

                                Subsector subsector = null;

                                if (!lstSubsector.Exists(sec => sec.Name.Trim() == resultStock.Industry.Trim()))
                                {
                                    try
                                    {
                                        subsector = new Subsector();
                                        subsector.IdSector = sector.IdSector;
                                        subsector.Name = resultStock.Industry.Trim();
                                        subsector = _subsectorService.Insert(subsector).Value;
                                        lstSubsector.Add(subsector);
                                    }
                                    catch (Exception ex)
                                    {

                                        throw ex;
                                    }

                                }
                                else
                                {
                                    subsector = lstSubsector.ToList().FirstOrDefault(sec => sec.Name.Trim() == resultStock.Industry.Trim());
                                }

                                Segment segment = null;

                                if (!lstSegement.Exists(sec => sec.Name.Trim() == resultStock.DsPositionSegment.Trim()))
                                {
                                    try
                                    {
                                        segment = new Segment();
                                        segment.IdSubsector = subsector.IdSubsector;
                                        segment.Name = resultStock.DsPositionSegment.Trim();
                                        segment = _segmentService.Insert(segment).Value;
                                        lstSegement.Add(segment);
                                    }
                                    catch (Exception ex)
                                    {

                                        throw ex;
                                    }

                                }
                                else
                                {
                                    segment = lstSegement.FirstOrDefault(sec => sec.Name.Trim() == resultStock.DsPositionSegment.Trim());
                                }

                                Company company = null;
                                string cdCompany = resultStock.CdCompany.Trim();

                                if (!lstCompany.Exists(sec => sec.Code.Trim() == cdCompany))
                                {
                                    company = new Company();
                                    company.Code = resultStock.CdCompany;
                                    company.FullName = resultStock.NmCompanyExchange;
                                    company.IdCountry = 2;
                                    company.Name = resultStock.NmCompanyExchange;
                                    company.IdSegment = segment.IdSegment;
                                    company.IdLogo = logo.IdLogo;

                                    company = _companyService.Insert(company).Value;
                                    lstCompany.Add(company);
                                }
                                else
                                {
                                    company = lstCompany.FirstOrDefault(sec => sec.Code.Trim() == cdCompany);
                                }

                                Entity.Entities.Stock stock = null;

                                if (!lstStock.Exists(sec => sec.Symbol.Trim() == cdCompany))
                                {
                                    stock = new Entity.Entities.Stock();
                                    stock.IdCompany = company.IdCompany;
                                    stock.IdCountry = 2;
                                    stock.IdStockType = stockType;
                                    stock.MarketPrice = 0;
                                    stock.ShowOnPortolio = true;
                                    stock.Symbol = company.Code;
                                    stock.TradeTime = DateTime.Now;
                                    stock.UpdatedDate = DateTime.Now;
                                    stock.LastDividendUpdateSync = DateTime.Now.AddDays(-1);
                                    stock = _stockService.Insert(stock).Value;
                                    lstStock.Add(stock);

                                    //company = _companyService.Insert(company).Value;
                                }



                            }
                            else
                            {
                                Entity.Entities.Stock stock = lstStock.FirstOrDefault(sec => sec.Symbol.Trim() == resultStock.CdCompany.Trim());

                                if (stock != null)
                                {
                                    stock.IdStockType = stockType;
                                    _stockService.Update(stock);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ImportMarketMover(MakertMoversType makertMoversType)
        {
            var style = NumberStyles.Number;
            var culture = CultureInfo.CreateSpecificCulture("en-US");

            string tradeMapCookie = string.Empty;
            using (_uow.Create())
            {
                ResultServiceObject<SystemSettings> resultSettingsCookie = _systemSettingsService.GetByKey(Constants.SYSTEM_SETTINGS_TRADE_MAP_COOKIE);

                if (resultSettingsCookie.Success && resultSettingsCookie.Value != null)
                {
                    tradeMapCookie = resultSettingsCookie.Value.SettingValue;
                }
            }

            if (!string.IsNullOrWhiteSpace(tradeMapCookie))
            {
                string country = null, index = null, exchange = null, order = null, marketType = null;

                CountryEnum countryEnum = CountryEnum.Brazil;

                List<StockMarketMover> stockMarketMover = null;

                switch (makertMoversType)
                {
                    case MakertMoversType.BiggestHighsStocksBRAll:
                        {
                            countryEnum = CountryEnum.Brazil;
                            country = "brasil";
                            index = "Geral";
                            exchange = "1";
                            order = "DESC";

                            stockMarketMover = _iTradeMapHelper.ImportMarketMoversAsync(tradeMapCookie, country, order, index, exchange).Result;
                        }
                        break;
                    case MakertMoversType.BiggestFallsStocksBRAll:
                        {
                            countryEnum = CountryEnum.Brazil;
                            country = "brasil";
                            index = "Geral";
                            exchange = "1";
                            order = "ASC";

                            stockMarketMover = _iTradeMapHelper.ImportMarketMoversAsync(tradeMapCookie, country, order, index, exchange).Result;
                        }
                        break;
                    case MakertMoversType.BiggestHighsStocksBR:
                        {
                            countryEnum = CountryEnum.Brazil;
                            country = "brasil";
                            index = "IBOV";
                            exchange = "1";
                            order = "DESC";

                            stockMarketMover = _iTradeMapHelper.ImportMarketMoversAsync(tradeMapCookie, country, order, index, exchange).Result;
                        }
                        break;
                    case MakertMoversType.BiggestHighsFIIsBR:
                        {
                            countryEnum = CountryEnum.Brazil;
                            country = "brasil";
                            index = "IFIX";
                            exchange = "1";
                            order = "DESC";
                            stockMarketMover = _iTradeMapHelper.ImportMarketMoversAsync(tradeMapCookie, country, order, index, exchange).Result;
                        }
                        break;
                    case MakertMoversType.BiggestHighsUS:
                        {
                            countryEnum = CountryEnum.EUA;
                            country = "eua";
                            index = "DJI";
                            exchange = "7777";
                            order = "DESC";
                            stockMarketMover = _iTradeMapHelper.ImportMarketMoversAsync(tradeMapCookie, country, order, index, exchange).Result;
                        }
                        break;
                    case MakertMoversType.BiggestFallsStocksBR:
                        {
                            countryEnum = CountryEnum.Brazil;
                            country = "brasil";
                            index = "IBOV";
                            exchange = "1";
                            order = "ASC";
                            stockMarketMover = _iTradeMapHelper.ImportMarketMoversAsync(tradeMapCookie, country, order, index, exchange).Result;
                        }
                        break;
                    case MakertMoversType.BiggestFallsFIIsBR:
                        {
                            countryEnum = CountryEnum.Brazil;
                            country = "brasil";
                            index = "IFIX";
                            exchange = "1";
                            order = "ASC";
                            stockMarketMover = _iTradeMapHelper.ImportMarketMoversAsync(tradeMapCookie, country, order, index, exchange).Result;
                        }
                        break;
                    case MakertMoversType.BiggestFallsUS:
                        {
                            countryEnum = CountryEnum.EUA;
                            country = "eua";
                            index = "DJI";
                            exchange = "7777";
                            order = "ASC";
                            stockMarketMover = _iTradeMapHelper.ImportMarketMoversAsync(tradeMapCookie, country, order, index, exchange).Result;
                        }
                        break;
                    case MakertMoversType.TopDividendPaidStocksBR:
                        {
                            countryEnum = CountryEnum.Brazil;
                            index = "IBOV";
                            exchange = "1";
                            marketType = "acoes";
                            stockMarketMover = _iTradeMapHelper.ImportMarketMoversDividendPaidAsync(tradeMapCookie, marketType, index, exchange).Result;
                        }
                        break;
                    case MakertMoversType.TopDividendPaidFIIsBR:
                        {
                            countryEnum = CountryEnum.Brazil;
                            index = "IFIX";
                            exchange = "1";
                            marketType = "fii";
                            stockMarketMover = _iTradeMapHelper.ImportMarketMoversDividendPaidAsync(tradeMapCookie, marketType, index, exchange).Result;
                        }
                        break;
                    case MakertMoversType.TopDividendPaidUS:
                        {
                            countryEnum = CountryEnum.EUA;
                            index = "DJI";
                            exchange = "7777";
                            marketType = "acoes";
                            stockMarketMover = _iTradeMapHelper.ImportMarketMoversDividendPaidAsync(tradeMapCookie, marketType, index, exchange).Result;
                        }
                        break;
                    case MakertMoversType.TopDividendYieldStocksBR:
                        {
                            countryEnum = CountryEnum.Brazil;
                            index = "IBOV";
                            exchange = "1";
                            marketType = "acoes";
                            stockMarketMover = _iTradeMapHelper.ImportMarketMoversDividendYieldAsync(tradeMapCookie, marketType, index, exchange).Result;
                        }
                        break;
                    case MakertMoversType.TopDividendYieldFIIsBR:
                        {
                            countryEnum = CountryEnum.Brazil;
                            country = "brasil";
                            index = "IFIX";
                            exchange = "1";
                            marketType = "fii";
                            stockMarketMover = _iTradeMapHelper.ImportMarketMoversDividendYieldAsync(tradeMapCookie, marketType, index, exchange).Result;
                        }
                        break;
                    case MakertMoversType.TopDividendYieldUS:
                        {
                            countryEnum = CountryEnum.EUA;
                            country = "eua";
                            index = "DJI";
                            exchange = "7777";
                            marketType = "acoes";
                            stockMarketMover = _iTradeMapHelper.ImportMarketMoversDividendYieldAsync(tradeMapCookie, marketType, index, exchange).Result;
                        }
                        break;
                    case MakertMoversType.TopDividendPaidREITsEUA:
                        {
                            countryEnum = CountryEnum.EUA;
                            marketType = "{\"cd_stocklist_dynamic\":\"MOVERS\",\"type_filter\":{\"movers_id_exchange\":7777,\"movers_cd_index\":\"GERAL\",\"movers_order_by\":\"desc\",\"movers_market_type\":\"acoes\",\"movers_call\":\"TOP_DIVIDENDOS\",\"movers_period\":\"oneday\",\"movers_limit\":20}}";
                            stockMarketMover = _iTradeMapHelper.ImportMarketMoversDividendREITsAsync(tradeMapCookie, marketType).Result;
                        }
                        break;
                    case MakertMoversType.TopDividendYieldREITsEUA:
                        {
                            countryEnum = CountryEnum.EUA;
                            marketType = "{\"cd_stocklist_dynamic\":\"MOVERS\",\"type_filter\":{\"movers_id_exchange\":7777,\"movers_cd_index\":\"GERAL\",\"movers_order_by\":\"desc\",\"movers_market_type\":\"acoes\",\"movers_call\":\"TOP_DIVIDEND_YIELD\",\"movers_period\":\"oneday\",\"movers_limit\":20}}";
                            stockMarketMover = _iTradeMapHelper.ImportMarketMoversDividendREITsAsync(tradeMapCookie, marketType).Result;
                        }
                        break;

                }


                if (stockMarketMover != null)
                {
                    List<MarketMover> marketMovers = new List<MarketMover>();

                    foreach (var itemStockMarketMover in stockMarketMover)
                    {
                        Entity.Views.StockView stock = null;

                        using (_uow.Create())
                        {
                            ResultServiceObject<IEnumerable<Entity.Views.StockView>> stocks = _stockService.GetLikeSymbol(itemStockMarketMover.Stock, (int)countryEnum);

                            stock = stocks.Value.FirstOrDefault();
                        }


                        decimal marketPrice = 0, variaton = 0;

                        decimal.TryParse(itemStockMarketMover.CloseValue, style, culture, out marketPrice);


                        if (!string.IsNullOrEmpty(itemStockMarketMover.Variation))
                        {
                            decimal.TryParse(itemStockMarketMover.Variation, style, culture, out variaton);
                        }
                        else
                        {
                            if (makertMoversType == MakertMoversType.TopDividendYieldFIIsBR ||
                                makertMoversType == MakertMoversType.TopDividendYieldStocksBR ||
                                makertMoversType == MakertMoversType.TopDividendYieldREITsEUA ||
                                makertMoversType == MakertMoversType.TopDividendYieldUS)
                            {
                                decimal.TryParse(itemStockMarketMover.DividendYield, style, culture, out variaton);
                            }
                            else if (makertMoversType == MakertMoversType.TopDividendPaidFIIsBR ||
                                makertMoversType == MakertMoversType.TopDividendPaidStocksBR ||
                                makertMoversType == MakertMoversType.TopDividendPaidREITsEUA ||
                                makertMoversType == MakertMoversType.TopDividendPaidUS)
                            {
                                decimal.TryParse(itemStockMarketMover.TotalPaid, style, culture, out variaton);
                            }
                        }




                        if (stock != null)
                        {
                            marketMovers.Add(new MarketMover() { MarketPrice = marketPrice, Value = variaton, StockID = stock.IdStock });
                        }
                    }

                    using (_uow.Create())
                    {
                        if (marketMovers.Count > 1)
                        {
                            _marketMoverService.InsertAll(marketMovers, (MaketMoversTypeEnum)makertMoversType);
                        }
                    }
                }
            }
        }

        public ResultResponseObject<IEnumerable<MarketMoverVM>> GetMarketMoverByType(MakertMoversType makertMoversType)
        {
            ResultResponseObject<IEnumerable<MarketMoverVM>> resultResponseObject = new ResultResponseObject<IEnumerable<MarketMoverVM>>();

            string resultFromCache = _cacheService.GetFromCache(string.Concat("MarketMover:", makertMoversType.ToString()));

            if (resultFromCache == null)
            {
                using (_uow.Create())
                {
                    ResultServiceObject<IEnumerable<MarketMoverView>> resultServiceObject = _marketMoverService.GetByType((MaketMoversTypeEnum)(int)makertMoversType);

                    resultResponseObject = _mapper.Map<ResultResponseObject<IEnumerable<MarketMoverVM>>>(resultServiceObject);

                    _cacheService.SaveOnCache(string.Concat("MarketMover:", makertMoversType.ToString()), TimeSpan.FromMinutes(3), JsonConvert.SerializeObject(resultResponseObject));
                }
            }
            else
            {
                resultResponseObject = JsonConvert.DeserializeObject<ResultResponseObject<IEnumerable<MarketMoverVM>>>(resultFromCache);
                resultResponseObject.Success = true;
            }


            return resultResponseObject;
        }

        /// <summary>
        /// 1 - ACOES (bdrs) | 2 - FIIs (bdrs) | 6 - Bdr ETF | 12 - Stocks | 13 - REITs | 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public void ImportStatusInvestCompanies(int type)
        {
            List<string> lstSegmentNotFound = new List<string>();

            try
            {
                List<StatusInvest.Interface.Model.Datum> companies = _iStatusInvestHelper.GetCompanies(type).Result;

                if (companies != null && companies.Count > 0)
                {
                    List<Entity.Entities.Company> lstCompany = new List<Company>();
                    List<Entity.Entities.Segment> lstSegement = new List<Segment>();
                    List<Entity.Entities.Stock> lstStock = new List<Entity.Entities.Stock>();
                    List<Entity.Entities.Logo> lstLogo = new List<Logo>();

                    ResultServiceObject<IEnumerable<Entity.Entities.Company>> resultCompany = null;
                    ResultServiceObject<IEnumerable<Entity.Entities.Segment>> resultSegment = null;
                    ResultServiceObject<IEnumerable<Entity.Entities.Stock>> resultStockDb = null;
                    ResultServiceObject<IEnumerable<Entity.Entities.Logo>> resultLogo = null;

                    int idCountry = 1;

                    if (type == 12 || type == 13)
                    {
                        idCountry = 2;
                    }

                    using (_uow.Create())
                    {
                        resultCompany = _companyService.GetAllByCountry(idCountry);

                        if (resultCompany.Success && resultCompany.Value != null && resultCompany.Value.Count() > 0)
                        {
                            lstCompany = resultCompany.Value.ToList();
                        }

                        resultSegment = _segmentService.GetAll();

                        if (resultSegment.Success && resultSegment.Value != null && resultSegment.Value.Count() > 0)
                        {
                            lstSegement = resultSegment.Value.ToList();
                        }

                        resultStockDb = _stockService.GetAllByCountry(idCountry);

                        if (resultStockDb.Success && resultStockDb.Value != null && resultStockDb.Value.Count() > 0)
                        {
                            lstStock = resultStockDb.Value.ToList();
                        }

                        resultLogo = _logoService.GetAll();

                        if (resultLogo.Success && resultLogo.Value != null && resultLogo.Value.Count() > 0)
                        {
                            lstLogo = resultLogo.Value.ToList();
                        }
                    }

                    foreach (StatusInvest.Interface.Model.Datum resultStock in companies)
                    {
                        try
                        {
                            int stockType = 4;
                            string ticker = resultStock.Tickers.First().Code;
                            string companyCode = ticker;

                            if (idCountry == 1)
                            {
                                companyCode = resultStock.Tickers.First().Code.Remove(4).Trim();

                                if (type == 1 && !ticker.Contains("34"))
                                {
                                    stockType = 1;
                                }
                                else if (type == 2 && !ticker.Contains("34"))
                                {
                                    stockType = 2;
                                }
                            }
                            else
                            {
                                if (type == 12)
                                {
                                    stockType = 1;
                                }
                                else if (type == 13)
                                {
                                    stockType = 5;
                                }
                            }


                            using (_uow.Create())
                            {
                                if (!lstCompany.ToList().Exists(cp => cp.Code == companyCode))
                                {
                                    Logo logo = new Logo();

                                    if (string.IsNullOrWhiteSpace(resultStock.Logo))
                                    {
                                        logo = lstLogo.FirstOrDefault(sec => sec.CompanyCode.Trim() == "DEFAULT");
                                    }
                                    else
                                    {
                                        if (!lstLogo.Exists(sec => sec.CompanyCode.Trim() == companyCode))
                                        {
                                            try
                                            {

                                                logo.CompanyCode = companyCode;
                                                logo.LogoImage = resultStock.Logo;
                                                logo = _logoService.Insert(logo).Value;
                                                lstLogo.Add(logo);
                                            }
                                            catch (Exception ex)
                                            {
                                                throw ex;
                                            }
                                        }
                                        else
                                        {
                                            logo = lstLogo.FirstOrDefault(sec => sec.CompanyCode.Trim() == companyCode);
                                        }
                                    }

                                    Segment segment = lstSegement.FirstOrDefault(sec => sec.Name.Trim() == resultStock.Segment.Trim());

                                    if (segment != null)
                                    {
                                        Company company = null;

                                        if (!lstCompany.Exists(sec => sec.Code.Trim() == companyCode))
                                        {
                                            company = new Company();
                                            company.Code = companyCode;

                                            string companyName = resultStock.CompanyName;

                                            if (companyName.Length > 98)
                                            {
                                                companyName = companyName.Substring(0, 98);
                                            }

                                            company.FullName = companyName;
                                            company.IdCountry = idCountry;
                                            company.Name = companyName;
                                            company.IdSegment = segment.IdSegment;
                                            company.IdLogo = logo.IdLogo;

                                            company = _companyService.Insert(company).Value;
                                            lstCompany.Add(company);
                                        }
                                        else
                                        {
                                            company = lstCompany.FirstOrDefault(sec => sec.Code.Trim() == companyCode);
                                        }

                                        Entity.Entities.Stock stock = null;

                                        foreach (Dividendos.StatusInvest.Interface.Model.Ticker stockTicker in resultStock.Tickers)
                                        {
                                            if (!lstStock.Exists(sec => sec.Symbol.Trim() == stockTicker.Code.Trim()))
                                            {
                                                stock = new Entity.Entities.Stock();
                                                stock.IdCompany = company.IdCompany;
                                                stock.IdCountry = idCountry;
                                                stock.IdStockType = stockType;
                                                stock.MarketPrice = 0;
                                                stock.ShowOnPortolio = true;
                                                stock.Symbol = stockTicker.Code;
                                                stock.TradeTime = DateTime.Now;
                                                stock.UpdatedDate = DateTime.Now;
                                                stock.LastDividendUpdateSync = DateTime.Now.AddDays(-1);
                                                stock = _stockService.Insert(stock).Value;
                                                lstStock.Add(stock);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        lstSegmentNotFound.Add(resultStock.Segment);
                                    }
                                }
                                else
                                {
                                    Segment segment = lstSegement.FirstOrDefault(sec => sec.Name.Trim() == resultStock.Segment.Trim());
                                    Company company = lstCompany.FirstOrDefault(sec => sec.Code.Trim() == companyCode);

                                    if (segment != null)
                                    {
                                        if (company != null)
                                        {
                                            company.IdSegment = segment.IdSegment;
                                            _companyService.Update(company);
                                        }
                                    }

                                    Logo logo = lstLogo.FirstOrDefault(sec => sec.CompanyCode.Trim() == companyCode);

                                    if (logo != null && logo.IdLogo != 552)
                                    {
                                        if (company != null)
                                        {
                                            company.IdLogo = logo.IdLogo;
                                            _companyService.Update(company);
                                            _iS3Service.PutImage(Encoding.UTF8.GetBytes(logo.LogoImage), string.Concat(company.Name, ".jpg"));
                                        }
                                    }
                                    else if (!string.IsNullOrWhiteSpace(resultStock.Logo))
                                    {
                                        logo = new Logo();
                                        logo.CompanyCode = companyCode;
                                        logo.LogoImage = resultStock.Logo;
                                        logo = _logoService.Insert(logo).Value;
                                        lstLogo.Add(logo);

                                        if (company != null)
                                        {
                                            company.IdLogo = logo.IdLogo;
                                            _companyService.Update(company);
                                            _iS3Service.PutImage(Encoding.UTF8.GetBytes(logo.LogoImage), string.Concat(company.Name, ".jpg"));
                                        }
                                    }

                                    Entity.Entities.Stock stock = lstStock.FirstOrDefault(sec => sec.Symbol.Trim() == ticker);

                                    if (stock != null)
                                    {
                                        stock.IdStockType = stockType;
                                        _stockService.Update(stock);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            int cnt = lstSegmentNotFound.Count;
        }


        public ResultResponseObject<IEnumerable<MilkingCowsVM>> GetMilkingCows(CountryType countryType)
        {
            ResultResponseObject<IEnumerable<MilkingCowsVM>> resultResponseObject = new ResultResponseObject<IEnumerable<MilkingCowsVM>>();

            string resultFromCache = _cacheService.GetFromCache(string.Concat("MilkingCows", countryType.ToString()));

            if (resultFromCache == null)
            {
                using (_uow.Create())
                {
                    ResultServiceObject<IEnumerable<MilkingCowsView>> resultServiceObject = _marketMoverService.GetMilkingCows((CountryEnum)(int)countryType);

                    resultResponseObject = _mapper.Map<ResultResponseObject<IEnumerable<MilkingCowsVM>>>(resultServiceObject);

                    _cacheService.SaveOnCache(string.Concat("MilkingCows", countryType.ToString()), TimeSpan.FromHours(6), JsonConvert.SerializeObject(resultResponseObject));
                }
            }
            else
            {
                resultResponseObject = JsonConvert.DeserializeObject<ResultResponseObject<IEnumerable<MilkingCowsVM>>>(resultFromCache);
                resultResponseObject.Success = true;
            }


            return resultResponseObject;
        }


        public void SendAlertAwesomeDailyVariations()
        {
            List<MaketMoversTypeEnum> maketMoversTypeEnums = new List<MaketMoversTypeEnum>();

            bool isHollidayEUA = false;
            bool isHollidayBR = false;

            using (_uow.Create())
            {
                isHollidayEUA = _holidayService.IsHoliday(DateTime.Now.Date, (int)CountryEnum.EUA);
                isHollidayBR = _holidayService.IsHoliday(DateTime.Now.Date, (int)CountryEnum.Brazil);
            }

            if (!isHollidayEUA)
            {
                maketMoversTypeEnums.Add(MaketMoversTypeEnum.BiggestHighsUS);
                maketMoversTypeEnums.Add(MaketMoversTypeEnum.BiggestFallsUS);
            }

            if (!isHollidayBR)
            {
                maketMoversTypeEnums.Add(MaketMoversTypeEnum.BiggestHighsStocksBRAll);
                maketMoversTypeEnums.Add(MaketMoversTypeEnum.BiggestFallsStocksBRAll);
            }

            List<Tuple<string, string>> keyValueFinal = new List<Tuple<string, string>>();

            foreach (var itemMaketMoversTypeEnum in maketMoversTypeEnums)
            {
                ResultServiceObject<IEnumerable<MarketMoverView>> resultServiceObject;

                using (_uow.Create())
                {
                    resultServiceObject = _marketMoverService.GetByType(itemMaketMoversTypeEnum);
                }

                foreach (var itemStock in resultServiceObject.Value)
                {
                    if ((itemMaketMoversTypeEnum == MaketMoversTypeEnum.BiggestHighsStocksBRAll && itemStock.Value > 5) ||
                        (itemMaketMoversTypeEnum == MaketMoversTypeEnum.BiggestHighsUS && itemStock.Value > 5) ||
                        (itemMaketMoversTypeEnum == MaketMoversTypeEnum.BiggestFallsStocksBRAll && itemStock.Value < -5) ||
                        (itemMaketMoversTypeEnum == MaketMoversTypeEnum.BiggestFallsUS && itemStock.Value < -5))
                    {
                        ResultServiceObject<IEnumerable<string>> resultUsersServiceObject;

                        //Get list of user that have this stock in portfolio
                        using (_uow.Create())
                        {
                            resultUsersServiceObject = _stockService.GetAllUsersWithStock(itemStock.StockID);
                        }

                        foreach (var itemUser in resultUsersServiceObject.Value)
                        {
                            var itemFound = keyValueFinal.Where(item => item.Item1.Equals(itemUser)).FirstOrDefault();

                            if (itemFound != null)
                            {
                                if (itemFound.Item2.Length < 170)
                                {
                                    keyValueFinal.RemoveAll(item => item.Item1.Equals(itemUser));
                                    keyValueFinal.Add(new Tuple<string, string>(itemUser, string.Concat(itemFound.Item2, string.Format(", {0} {1}%", itemStock.Stock, itemStock.Value.ToString("n2", new CultureInfo("pt-br"))))));
                                }
                            }
                            else
                            {
                                keyValueFinal.Add(new Tuple<string, string>(itemUser, string.Format("{0} {1}%", itemStock.Stock, itemStock.Value.ToString("n2", new CultureInfo("pt-br")))));
                            }
                        }
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

                    if (settings.Value == null || settings.Value.PushStocksWithAwesomeVariation)
                    {
                        pushMessage = string.Format("Atenção! Detectamos que as seguintes ações ({0}) de sua carteira, considerando apenas o dia de hoje {1}, variaram fortemente. Entre no App Dividendos.me e confira mais detalhes desta e de outras ações.", itemPush.Item2, DateTime.Now.Date.ToString("dd/MM/yyyy"));

                        pushMessageTitle = "Aconteceu algo importante com a sua carteira!";
                    }

                    if (!string.IsNullOrEmpty(pushMessage) && !string.IsNullOrEmpty(pushMessageTitle))
                    {
                        ResultServiceObject<IEnumerable<Device>> devices = _deviceService.GetByUser(itemPush.Item1);

                        _notificationHistoricalService.New(pushMessageTitle, pushMessage, itemPush.Item1, AppScreenNameEnum.HomeStocks.ToString(), PushRedirectTypeEnum.Internal.ToString(), null, _cacheService);

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

        public void SendAlertAwesomeDailyVariationsCheckingAllDayLong()
        {
            List<MaketMoversTypeEnum> maketMoversTypeEnums = new List<MaketMoversTypeEnum>();

            bool isHollidayEUA = false;
            bool isHollidayBR = false;

            using (_uow.Create())
            {
                isHollidayEUA = _holidayService.IsHoliday(DateTime.Now.Date, (int)CountryEnum.EUA);
                isHollidayBR = _holidayService.IsHoliday(DateTime.Now.Date, (int)CountryEnum.Brazil);
            }

            if (!isHollidayEUA)
            {
                maketMoversTypeEnums.Add(MaketMoversTypeEnum.BiggestHighsUS);
                maketMoversTypeEnums.Add(MaketMoversTypeEnum.BiggestFallsUS);
            }

            if (!isHollidayBR)
            {
                maketMoversTypeEnums.Add(MaketMoversTypeEnum.BiggestHighsStocksBRAll);
                maketMoversTypeEnums.Add(MaketMoversTypeEnum.BiggestFallsStocksBRAll);
            }

            List<Tuple<string, string>> keyValueFinal = new List<Tuple<string, string>>();

            foreach (var itemMaketMoversTypeEnum in maketMoversTypeEnums)
            {
                ResultServiceObject<IEnumerable<MarketMoverView>> resultServiceObject;

                using (_uow.Create())
                {
                    resultServiceObject = _marketMoverService.GetByType(itemMaketMoversTypeEnum);
                }

                foreach (var itemStock in resultServiceObject.Value)
                {
                    if (((itemMaketMoversTypeEnum == MaketMoversTypeEnum.BiggestHighsStocksBRAll && itemStock.Value > 5) ||
                        (itemMaketMoversTypeEnum == MaketMoversTypeEnum.BiggestHighsUS && itemStock.Value > 5) ||
                        (itemMaketMoversTypeEnum == MaketMoversTypeEnum.BiggestFallsStocksBRAll && itemStock.Value < -5) ||
                        (itemMaketMoversTypeEnum == MaketMoversTypeEnum.BiggestFallsUS && itemStock.Value < -5)) && (!itemStock.LastDailyVariationNotification.HasValue ||  itemStock.LastDailyVariationNotification.Value.Date != DateTime.Now.Date))
                    {
                        ResultServiceObject<IEnumerable<string>> resultUsersServiceObject;

                        //Get list of user that have this stock in portfolio
                        using (_uow.Create())
                        {
                            resultUsersServiceObject = _stockService.GetAllUsersWithStock(itemStock.StockID);

                            //update last Notification on stock
                            _stockService.UpdateLastDailyVariationNotification(itemStock.StockID);
                        }

                        foreach (var itemUser in resultUsersServiceObject.Value)
                        {
                            var itemFound = keyValueFinal.Where(item => item.Item1.Equals(itemUser)).FirstOrDefault();

                            if (itemFound != null)
                            {
                                if (itemFound.Item2.Length < 170)
                                {
                                    keyValueFinal.RemoveAll(item => item.Item1.Equals(itemUser));
                                    keyValueFinal.Add(new Tuple<string, string>(itemUser, string.Concat(itemFound.Item2, string.Format(", {0} {1}%", itemStock.Stock, itemStock.Value.ToString("n2", new CultureInfo("pt-br"))))));
                                }
                            }
                            else
                            {
                                keyValueFinal.Add(new Tuple<string, string>(itemUser, string.Format("{0} {1}%", itemStock.Stock, itemStock.Value.ToString("n2", new CultureInfo("pt-br")))));
                            }
                        }
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

                    if (settings.Value == null || settings.Value.PushStocksWithAwesomeVariation)
                    {
                        pushMessage = string.Format("Detectamos que as seguintes ações ({0}) variaram muito deste a abertura do pregão. Entre no App Dividendos.me e confira mais detalhes desta e de outras ações.", itemPush.Item2, DateTime.Now.Date.ToString("dd/MM/yyyy"));

                        pushMessageTitle = "Breaking News!";
                    }

                    if (!string.IsNullOrEmpty(pushMessage) && !string.IsNullOrEmpty(pushMessageTitle))
                    {
                        ResultServiceObject<IEnumerable<Device>> devices = _deviceService.GetByUser(itemPush.Item1);

                        _notificationHistoricalService.New(pushMessageTitle, pushMessage, itemPush.Item1, AppScreenNameEnum.HomeStocks.ToString(), PushRedirectTypeEnum.Internal.ToString(), null, _cacheService);

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

        public ResultServiceObject<Entity.Entities.Stock> GetBySymbolOrLikeOldSymbol(string symbol, int idCountry)
        {
            ResultServiceObject<Entity.Entities.Stock> resultServiceStock = null;

            using (_uow.Create())
            {
                resultServiceStock = _stockService.GetBySymbolOrLikeOldSymbol(symbol, idCountry);
            }

            return resultServiceStock;
        }

    }
}