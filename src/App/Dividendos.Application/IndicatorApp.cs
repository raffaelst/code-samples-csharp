using Dividendos.Bacen.Interface;
using Dividendos.Bacen.Interface.Model;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Dividendos.Entity.Model;
using Dividendos.Entity.Entities;
using System;
using System.Globalization;
using K.Logger;
using Dividendos.Application.Interface;
using System.Threading.Tasks;
using Dividendos.API.Model.Response.Common;
using Dividendos.API.Model.Response;
using AutoMapper;
using Dividendos.Entity.Enum;
using Dividendos.Entity.Views;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Dividendos.InvestingCom.Interface;
using Dividendos.InvestingCom.Interface.Model;
using Dividendos.API.Model.Request.Indicator;

namespace Dividendos.Application
{
    public class IndicatorApp : IIndicatorApp
    {
        private readonly IUnitOfWork _uow;
        private readonly IIndicatorSeriesService _indicatorSeriesService;
        private readonly IImportBacenHelper _iImportBacenHelper;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        private readonly IInvestingComHelper _investingComHelper;
        private readonly ICryptoCurrencyService _cryptoCurrencyService;

        public IndicatorApp(IUnitOfWork uow,
                            IIndicatorSeriesService indicatorSeriesService,
                            IImportBacenHelper iImportBacenHelper,
                            ILogger logger,
                            IMapper mapper,
                            ICacheService cacheService,
                            IInvestingComHelper investingComHelper,
                            ICryptoCurrencyService cryptoCurrencyService)
        {
            _uow = uow;
            _indicatorSeriesService = indicatorSeriesService;
            _iImportBacenHelper = iImportBacenHelper;
            _logger = logger;
            _mapper = mapper;
            _cacheService = cacheService;
            _investingComHelper = investingComHelper;
            _cryptoCurrencyService = cryptoCurrencyService;
        }

        public void ImportIndicators()
        {
            try
            {
                IEnumerable<Indicator> indicators = _iImportBacenHelper.ImportIndicatorsAsync();

                if (indicators != null && indicators.Count() > 0)
                {
                    var culture = CultureInfo.CreateSpecificCulture("pt-BR");

                    using (_uow.Create())
                    {
                        ResultServiceObject<IEnumerable<IndicatorSeries>> resultService = _indicatorSeriesService.GetAll();

                        if (resultService.Success)
                        {
                            IEnumerable<IndicatorSeries> indicatorsSeries = resultService.Value;

                            foreach (Indicator indicator in indicators)
                            {
                                DateTime calcDate;

                                if (DateTime.TryParseExact(indicator.TradeTime, "dd/MM/yyyy", culture, DateTimeStyles.None, out calcDate))
                                {
                                    calcDate = calcDate.Date;
                                    IndicatorSeries indicatorSeries = null;


                                    if (indicatorsSeries != null && indicatorsSeries.Count() > 0)
                                    {
                                        indicatorSeries = indicatorsSeries.FirstOrDefault(indicatorSerieTmp => indicatorSerieTmp.IdIndicatorType == indicator.IndicatorType &&
                                                                                                               indicatorSerieTmp.IdPeriodType == indicator.PeriodType &&
                                                                                                               indicatorSerieTmp.CalculationDate == calcDate);


                                        if (indicatorSeries == null)
                                        {
                                            indicatorSeries = new IndicatorSeries();
                                        }

                                        indicatorSeries.CalculationDate = calcDate;
                                        indicatorSeries.IdIndicatorType = indicator.IndicatorType;
                                        indicatorSeries.IdPeriodType = indicator.PeriodType;
                                        indicatorSeries.Perc = indicator.Percentage / 100;
                                        indicatorSeries.Points = 0;

                                        if (indicatorSeries.IdIndicatorSeries == 0)
                                        {
                                            ResultServiceObject<IndicatorSeries> resultInser = _indicatorSeriesService.Insert(indicatorSeries);
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

        public ResultResponseObject<IEnumerable<IndicatorVM>> GetAll()
        {
            ResultResponseObject<IEnumerable<IndicatorVM>> result = new ResultResponseObject<IEnumerable<IndicatorVM>>();
            List<IndicatorVM> indicators = new List<IndicatorVM>();

            ResultServiceObject<IEnumerable<IndicatorSeriesView>> resultService;

            string resultFromCache = _cacheService.GetFromCache("Indicator");

            if (resultFromCache == null)
            {
                using (_uow.Create())
                {
                    resultService = _indicatorSeriesService.GetAllLatest();
                }

                foreach (var item in resultService.Value)
                {
                    IndicatorVM indicatorVM = new IndicatorVM();

                    indicatorVM.Name = item.Name;


                    if (item.IdIndicatorType == (int)IndicatorTypeEnum.USDBRL)
                    {
                        indicatorVM.Name = "Dólar";
                        indicatorVM.Value = item.Points.ToString("C", new CultureInfo("pt-br"));
                    }
                    else if (item.IdIndicatorType == (int)IndicatorTypeEnum.EURBRL)
                    {
                        indicatorVM.Name = "Euro";
                        indicatorVM.Value = item.Points.ToString("C", new CultureInfo("pt-br"));
                    }
                    else
                    {
                        item.Perc = (item.Perc * 100);
                        indicatorVM.Value = GetSignal(item.Perc) + item.Perc.ToString("n2", new CultureInfo("pt-br"));
                    }

                    indicators.Add(indicatorVM);
                }

                result.Value = indicators;
                result.Success = true;
                _cacheService.SaveOnCache("Indicator", TimeSpan.FromMinutes(5), JsonConvert.SerializeObject(result));
            }
            else
            {
                result = JsonConvert.DeserializeObject<ResultResponseObject<IEnumerable<IndicatorVM>>>(resultFromCache);
                result.Success = true;
            }

            return result;
        }

        public ResultResponseObject<IEnumerable<IndicatorVM>> GetAllV2()
        {
            ResultResponseObject<IEnumerable<IndicatorVM>> result = null;
            List<IndicatorVM> indicators = new List<IndicatorVM>();

            ResultServiceObject<IEnumerable<IndicatorSeriesView>> resultService;

            string resultFromCache = _cacheService.GetFromCache("IndicatorV2");

            if (resultFromCache == null)
            {
                using (_uow.Create())
                {
                    resultService = _indicatorSeriesService.GetAllLatest();
                }

                foreach (var item in resultService.Value)
                {
                    IndicatorVM indicatorVM = new IndicatorVM();

                    indicatorVM.Name = item.Name;


                    if (item.IdIndicatorType == (int)IndicatorTypeEnum.USDBRL)
                    {
                        indicatorVM.Name = "Dólar";
                        indicatorVM.Value = item.Points.ToString("C", new CultureInfo("pt-br"));
                    }
                    else if (item.IdIndicatorType == (int)IndicatorTypeEnum.EURBRL)
                    {
                        indicatorVM.Name = "Euro";
                        indicatorVM.Value = item.Points.ToString("C", new CultureInfo("pt-br"));
                    }
                    else
                    {
                        item.Perc = (item.Perc * 100);
                        indicatorVM.Value = GetSignal(item.Perc) + item.Perc.ToString("n2", new CultureInfo("pt-br"));
                    }

                    indicators.Add(indicatorVM);
                }
                
                result.Value = indicators;
                result.Success = true;
                _cacheService.SaveOnCache("IndicatorV2", TimeSpan.FromMinutes(10), JsonConvert.SerializeObject(result));
            }
            else
            {
                result = JsonConvert.DeserializeObject<ResultResponseObject<IEnumerable<IndicatorVM>>>(resultFromCache);
                result.Success = true;
            }

            return result;
        }

        private string GetSignal(decimal value)
        {
            string signal = string.Empty;

            if (value > 0)
            {
                signal = "+";
            }

            return signal;
        }

        public void ImportInvestingIndicators()
        {
            List<InvestingIndicator> investingIndicators = _investingComHelper.GetIndicator();

            if (investingIndicators != null && investingIndicators.Count > 0)
            {
                using (_uow.Create())
                {
                    IEnumerable<IndicatorSeriesView> indicatorsSeriesDb = _indicatorSeriesService.GetAllLatestInvestingCom().Value;

                    foreach (InvestingIndicator investingIndicator in investingIndicators)
                    {
                        IndicatorSeries indicatorSeries = new IndicatorSeries();
                        indicatorSeries.IdIndicatorType = investingIndicator.IdIndicatorType;
                        indicatorSeries.IdPeriodType = 1;
                        indicatorSeries.CalculationDate = DateTime.Now.Date;
                        indicatorSeries.Points = investingIndicator.Price;

                        if (indicatorsSeriesDb != null && indicatorsSeriesDb.Count() > 0 && indicatorsSeriesDb.Any(ind => ind.IdIndicatorType == investingIndicator.IdIndicatorType))
                        {
                            IndicatorSeriesView indicatorSeriesDb = indicatorsSeriesDb.FirstOrDefault(ind => ind.IdIndicatorType == investingIndicator.IdIndicatorType);

                            if (indicatorSeriesDb != null)
                            {
                                indicatorSeries.IdIndicatorSeries = indicatorSeriesDb.IdIndicatorSeries;
                                indicatorSeries.GuidIndicatorSeries = indicatorSeriesDb.GuidIndicatorSeries;
                            }
                        }

                        if (indicatorSeries.IdIndicatorSeries == 0)
                        {
                            _indicatorSeriesService.Insert(indicatorSeries);
                        }
                        else
                        {
                            _indicatorSeriesService.Update(indicatorSeries);
                        }
                    }
                }
            }
        }
    
        public ResultResponseObject<IEnumerable<IndicatorVM>> GetIndicatorByType(IndicatorType indicatorType)
        {
            ResultResponseObject<IEnumerable<IndicatorVM>> resultResponse = new ResultResponseObject<IEnumerable<IndicatorVM>>();
            List<IndicatorVM> indicators = new List<IndicatorVM>();

            switch (indicatorType)
            {
                case IndicatorType.Commodity:
                    string resultFromCacheCommodity = _cacheService.GetFromCache("Commodity");

                    if (resultFromCacheCommodity == null)
                    {
                        IEnumerable<IndicatorSeriesView> indicatorSeriesCommodities = null;

                        using (_uow.Create())
                        {
                            indicatorSeriesCommodities = _indicatorSeriesService.GetAllLatestCommodities().Value;
                        }

                        if (indicatorSeriesCommodities != null && indicatorSeriesCommodities.Count() > 0)
                        {
                            foreach (IndicatorSeriesView indicatorSeries in indicatorSeriesCommodities)
                            {
                                IndicatorVM indicatorVM = new IndicatorVM();
                                indicatorVM.Name = indicatorSeries.Name;
                                indicatorVM.Value = "$ " + indicatorSeries.Points.ToString("n2", new CultureInfo("pt-br"));
                                indicators.Add(indicatorVM);
                            }
                        }

                        _cacheService.SaveOnCache("Commodity", TimeSpan.FromMinutes(5), JsonConvert.SerializeObject(indicators));
                    }
                    else
                    {
                        indicators = JsonConvert.DeserializeObject<List<IndicatorVM>>(resultFromCacheCommodity);
                    }

                    break;
                case IndicatorType.Forex:
                    string resultFromCacheForex = _cacheService.GetFromCache("Forex");

                    if (resultFromCacheForex == null)
                    {
                        IEnumerable<IndicatorSeriesView> indicatorSeriesForex = null;

                        using (_uow.Create())
                        {
                            indicatorSeriesForex = _indicatorSeriesService.GetAllLatestForex().Value;
                        }

                        if (indicatorSeriesForex != null && indicatorSeriesForex.Count() > 0)
                        {
                            foreach (IndicatorSeriesView indicatorSeries in indicatorSeriesForex)
                            {
                                IndicatorVM indicatorVM = new IndicatorVM();
                                indicatorVM.Name = indicatorSeries.Name;
                                indicatorVM.Value = indicatorSeries.Points.ToString("n2", new CultureInfo("pt-br"));
                                indicators.Add(indicatorVM);
                            }
                        }

                        _cacheService.SaveOnCache("Forex", TimeSpan.FromMinutes(5), JsonConvert.SerializeObject(indicators));
                    }
                    else
                    {
                        indicators = JsonConvert.DeserializeObject<List<IndicatorVM>>(resultFromCacheForex);
                    }

                    break;
                case IndicatorType.Crypto:
                    string resultFromCacheCrypto = _cacheService.GetFromCache("Crypto");

                    if (resultFromCacheCrypto == null)
                    {
                        IEnumerable<CryptoStatementView> cryptoStatementViews = null;

                        using (_uow.Create())
                        {
                            cryptoStatementViews = _cryptoCurrencyService.GetTopCryptos().Value;
                        }

                        if (cryptoStatementViews != null && cryptoStatementViews.Count() > 0)
                        {
                            foreach (CryptoStatementView cryptoStatementView in cryptoStatementViews)
                            {
                                IndicatorVM indicatorVM = new IndicatorVM();
                                indicatorVM.Name = cryptoStatementView.Description;
                                indicatorVM.Value = "$ " + cryptoStatementView.MarketPrice.ToString("n2", new CultureInfo("pt-br"));
                                indicatorVM.Logo = cryptoStatementView.LogoUrl;
                                indicators.Add(indicatorVM);
                            }
                        }

                        _cacheService.SaveOnCache("Crypto", TimeSpan.FromMinutes(5), JsonConvert.SerializeObject(indicators));
                    }
                    else
                    {
                        indicators = JsonConvert.DeserializeObject<List<IndicatorVM>>(resultFromCacheCrypto);
                    }

                    break;
                case IndicatorType.Indexes:
                    indicators.AddRange(GetAll().Value);
                    break;
                default:
                    break;
            }

            resultResponse.Value = indicators;
            resultResponse.Success = true;

            return resultResponse;
        }
    }
}
