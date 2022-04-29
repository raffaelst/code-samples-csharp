using Dividendos.API.Model.Request.Charts;
using Dividendos.API.Model.Request.Portfolio;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Charts;
using Dividendos.API.Model.Response.Common;
using Dividendos.Application.Base;
using Dividendos.Application.Interface;
using Dividendos.Application.Interface.Model;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Dividendos.Application
{
    public class PortfolioChartApp : BaseApp, IPortfolioChartApp
    {
        private readonly IGlobalAuthenticationService _globalAuthenticationService;
        private readonly IUnitOfWork _uow;
        private readonly IPortfolioService _portfolioService;
        private readonly ISubPortfolioService _subPortfolioService;
        private readonly IDividendService _dividendService;
        private readonly IOperationService _operationService;
        private readonly IPortfolioPerformanceService _portfolioPerformanceService;
        private readonly ISegmentService _segmentService;
        private readonly IPerformanceStockService _performanceStockService;
        private readonly ISubsectorService _subsectorService;
        private readonly ISectorService _sectorService;

        public PortfolioChartApp(IUnitOfWork uow,
            IPortfolioService portfolioService,
            ISubPortfolioService subPortfolioService,
            IDividendService dividendService,
            IGlobalAuthenticationService globalAuthenticationService,
            IPortfolioPerformanceService portfolioPerformanceService,
            ISegmentService segmentService,
            IPerformanceStockService performanceStockService,
            ISubsectorService subsectorService,
            ISectorService sectorService,
            IOperationService operationService)
        {
            _uow = uow;
            _portfolioService = portfolioService;
            _subPortfolioService = subPortfolioService;
            _dividendService = dividendService;
            _globalAuthenticationService = globalAuthenticationService;
            _operationService = operationService;
            _portfolioPerformanceService = portfolioPerformanceService;
            _segmentService = segmentService;
            _performanceStockService = performanceStockService;
            _subsectorService = subsectorService;
            _sectorService = sectorService;
        }

        public ResultResponseObject<DividendChartScrollVM> GetDividendStackedChart(Guid guidPortfolioSub, int year)
        {
            ResultResponseObject<DividendChartScrollVM> resultServiceObject = new ResultResponseObject<DividendChartScrollVM>();
            DividendChartScrollVM dividendChart = new DividendChartScrollVM();
            dividendChart.Categories = new List<DividendChartScrollCategory>();
            dividendChart.Dataset = new List<Dividendos.API.Model.Response.Dataset>();


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
                    DateTime startDate = new DateTime(year, 1, 1);
                    DateTime endDate = new DateTime(year, 12, 31);

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

                            DividendChartScrollCategory dividendChartScrollCategory = new DividendChartScrollCategory();
                            dividendChart.Categories.Add(dividendChartScrollCategory);
                            dividendChartScrollCategory.Category = new List<CategoryCategory>();

                            List<DividendView> lstStocks = dividends.Where(divStock => divStock.IdStockType != 2).ToList();
                            List<DividendView> lstReits = dividends.Where(divStock => divStock.IdStockType == 2).ToList();

                            List<string> lstDividendSymbolGrouped = dividends.GroupBy(dividendTmp => dividendTmp.Symbol).Select(dividendTmp =>dividendTmp.First().Symbol).ToList();

                            if (lstDividendSymbolGrouped.Count > 0)
                            {
                                foreach (string symbol in lstDividendSymbolGrouped)
                                {
                                    Dividendos.API.Model.Response.Dataset dtStocks = new API.Model.Response.Dataset();
                                    dtStocks.SeriesName = symbol;
                                    dtStocks.Data = new List<API.Model.Response.Datum>();
                                    dividendChart.Dataset.Add(dtStocks);
                                }
                            }

                            for (int i = 1; i <= 12; i++)
                            {
                                string monthName = new DateTime(year, i, 1).ToString("MMM", new CultureInfo("pt-br"));
                                CategoryCategory categoryCategory = new CategoryCategory();
                                categoryCategory.Label = monthName.First().ToString().ToUpper() + monthName.Substring(1);
                                dividendChartScrollCategory.Category.Add(categoryCategory);


                                foreach (string symbol in lstDividendSymbolGrouped)
                                {
                                    decimal sumStocks = dividends.Where(stkTmp => stkTmp.PaymentDate.HasValue && stkTmp.PaymentDate.Value.Month == i && stkTmp.Symbol == symbol)
                                                                 .Sum(stkTmp => stkTmp.NetValue);

                                    Dividendos.API.Model.Response.Dataset dtSymbol = dividendChart.Dataset.FirstOrDefault(divChart => divChart.SeriesName == symbol);

                                    if (dtSymbol != null)
                                    {
                                        API.Model.Response.Datum datumStock = new API.Model.Response.Datum();
                                        datumStock.Value = sumStocks.ToString("n2", new CultureInfo("en-us")).Replace(",", string.Empty);
                                        dtSymbol.Data.Add(datumStock);
                                    }
                                }
                            }

                            dividendChart.TotalStocks = lstStocks.Where(stkTmp => stkTmp.PaymentDate.HasValue).Sum(stkTmp => stkTmp.NetValue).ToString("n2", new CultureInfo("pt-br"));
                            dividendChart.TotalReits = lstReits.Where(stkTmp => stkTmp.PaymentDate.HasValue).Sum(stkTmp => stkTmp.NetValue).ToString("n2", new CultureInfo("pt-br"));
                            dividendChart.LastUpdated = DateTime.Now.ToString("HH:mm:ss");
                            dividendChart.Title = string.Format("Calendário {0}", year);
                            dividendChart.Year = year.ToString();
                        }
                    }
                }
            }

            resultServiceObject.Value = dividendChart;
            resultServiceObject.Success = true;

            return resultServiceObject;
        }

        public ResultResponseObject<TreeMapWrapperVM> GetTreeMapChart(Guid guidPortfolioSub)
        {
            ResultResponseObject<TreeMapWrapperVM> resultServiceObject = new ResultResponseObject<TreeMapWrapperVM>();
            TreeMapWrapperVM treeMapChart = new TreeMapWrapperVM();
            //treeMapChart.Data = new List<TreeMapVmDatum>();


            using (_uow.Create())
            {
                ResultServiceObject<IEnumerable<OperationSummaryView>> result = new ResultServiceObject<IEnumerable<OperationSummaryView>>();

                result = _operationService.GetOperationSummaryByPortfolioOrSubPortfolio(_globalAuthenticationService.IdUser, guidPortfolioSub.ToString());


                if (result.Success && result.Value != null && result.Value.Count() > 0)
                {
                    ResultServiceObject<Portfolio> resultPortfolio = _portfolioService.GetByGuid(guidPortfolioSub);
                    ResultServiceObject<SubPortfolio> resultSubPortfolio = _subPortfolioService.GetByGuid(guidPortfolioSub);
                    if (resultSubPortfolio.Success && resultPortfolio.Success)
                    {
                        Portfolio portfolio = resultPortfolio.Value;
                        SubPortfolio subportfolio = resultSubPortfolio.Value;
                        string portfolioName = string.Empty;

                        if (portfolio != null)
                        {
                            portfolioName = portfolio.Name;
                        }
                        else if (subportfolio != null)
                        {
                            portfolioName = subportfolio.Name;
                        }

                        decimal sum = result.Value.Sum(opSum => opSum.MarketPrice * opSum.NumberOfShares);
                        int idCountry = result.Value.First().IdCountry;

                        //TreeMapVmDatum dtCountry = new TreeMapVmDatum();
                        //dtCountry.Value = sum.ToString("n2", new CultureInfo("en-us")).Replace(",", string.Empty);
                        //dtCountry.Fillcolor = "8c8c8c";

                        //if (idCountry == 1)
                        //{
                        //    dtCountry.Label = "Brasil";
                        //}
                        //else
                        //{
                        //    dtCountry.Label = "EUA";
                        //}

                        treeMapChart.Data = new List<PurpleDatum>();
                        PurpleDatum dtPortfolio = new PurpleDatum();
                        dtPortfolio.Label = portfolioName;
                        dtPortfolio.Value = sum.ToString("n2", new CultureInfo("en-us")).Replace(",", string.Empty);
                        dtPortfolio.Data = new List<FluffyDatum>();

                        foreach (OperationSummaryView operationSummaryView in result.Value)
                        {
                            decimal percTwr = operationSummaryView.LastChangePerc * 100;
                            FluffyDatum dtStock = new FluffyDatum();
                            dtStock.Label = operationSummaryView.Symbol;
                            dtStock.Value = (operationSummaryView.MarketPrice * operationSummaryView.NumberOfShares).ToString("n2", new CultureInfo("en-us")).Replace(",", string.Empty);
                            dtStock.SValue = GetSignal(percTwr) + percTwr.ToString("n2", new CultureInfo("en-us")).Replace(",", string.Empty);

                            dtPortfolio.Data.Add(dtStock);
                        }

                        string max = (result.Value.Max(op => op.PerformancePercTWR) * 100).ToString("n2", new CultureInfo("en-us")).Replace(",", string.Empty);
                        string min = (result.Value.Min(op => op.PerformancePercTWR) * 100).ToString("n2", new CultureInfo("en-us")).Replace(",", string.Empty);

                        treeMapChart.Data.Add(dtPortfolio);

                        treeMapChart.IdCountry = idCountry;
                        //treeMapChart.Data.Add(dtCountry);
                        treeMapChart.LastUpdated = DateTime.Now.ToString("HH:mm:ss");
                        treeMapChart.Colorrange = new Colorrange();
                        treeMapChart.Colorrange.Mapbypercent = "1";
                        treeMapChart.Colorrange.Gradient = "1";
                        treeMapChart.Colorrange.Minvalue = min;
                        treeMapChart.Colorrange.Code = "f74005";
                        treeMapChart.Colorrange.Startlabel = "";
                        treeMapChart.Colorrange.Endlabel = "";
                        treeMapChart.Colorrange.Color = new List<Color>();
                        treeMapChart.Colorrange.Color.Add(new Color { Code = "ffffff", Label = "Static", Maxvalue = "0" });
                        treeMapChart.Colorrange.Color.Add(new Color { Code = "63ab03", Label = "AVERAGE", Maxvalue = max });
                    }
                }
            }

            resultServiceObject.Value = treeMapChart;
            resultServiceObject.Success = true;

            return resultServiceObject;
        }

        public ResultResponseObject<SunburstVM> GetSunburstChart(Guid guidPortfolioSub, SunburstType sunburstType)
        {
            ResultResponseObject<SunburstVM> resultServiceObject = new ResultResponseObject<SunburstVM>();

            switch (sunburstType)
            {
                case SunburstType.Segment:
                default:
                    resultServiceObject = GetSegmentAllocationChart(guidPortfolioSub);
                    break;
                case SunburstType.Subsector:
                    resultServiceObject = GetSubsectorAllocationChart(guidPortfolioSub);
                    break;
                case SunburstType.Sector:
                    resultServiceObject = GetSectorAllocationChart(guidPortfolioSub);
                    break;
            }

            return resultServiceObject;
        }

        public ResultResponseObject<SunburstVM> GetSegmentAllocationChart(Guid guidPortfolioSub)
        {
            SunburstVM sunburstVM = new SunburstVM();
            ResultResponseObject<SunburstVM> resultServiceObject = new ResultResponseObject<SunburstVM>();
            List<SunburstData> sunburstDatas = new List<SunburstData>();
            sunburstVM.ListData = sunburstDatas;

            using (_uow.Create())
            {
                ResultServiceObject<Portfolio> resultPortfolio = _portfolioService.GetByGuid(guidPortfolioSub);
                ResultServiceObject<SubPortfolio> resultSubPortfolio = _subPortfolioService.GetByGuid(guidPortfolioSub);

                if (resultSubPortfolio.Success && resultPortfolio.Success)
                {
                    Portfolio portfolio = resultPortfolio.Value;
                    SubPortfolio subportfolio = resultSubPortfolio.Value;

                    long idPortfolio = 0;
                    bool isSub = false;

                    if (portfolio != null)
                    {
                        idPortfolio = portfolio.IdPortfolio;
                        sunburstVM.IdCountry = portfolio.IdCountry;
                    }
                    else if (subportfolio != null)
                    {
                        idPortfolio = subportfolio.IdPortfolio;
                        isSub = true;

                        resultPortfolio = _portfolioService.GetById(subportfolio.IdPortfolio);

                        if (resultPortfolio.Value != null)
                        {
                            sunburstVM.IdCountry = resultPortfolio.Value.IdCountry;
                        }
                    }


                    ResultServiceObject<PortfolioPerformance> resultPortfolioPerformance = _portfolioPerformanceService.GetByCalculationDate(idPortfolio);
                    PortfolioPerformance portfolioPerformance = null;

                    if (resultPortfolioPerformance.Success)
                    {
                        portfolioPerformance = resultPortfolioPerformance.Value;

                        if (portfolioPerformance != null)
                        {
                            ResultServiceObject<IEnumerable<SegmentView>> resultSegments = new ResultServiceObject<IEnumerable<SegmentView>>();

                            if (isSub)
                            {
                                resultSegments = _segmentService.GetSumSubPortfolioPerformance(portfolioPerformance.IdPortfolioPerformance, subportfolio.IdSubPortfolio);
                            }
                            else
                            {
                                resultSegments = _segmentService.GetSumIdPortfolioPerformance(portfolioPerformance.IdPortfolioPerformance);
                            }

                            if (resultSegments.Success)
                            {

                                if (resultSegments.Value != null && resultSegments.Value.Count() > 0)
                                {
                                    resultSegments.Value = resultSegments.Value.OrderBy(sector => sector.TotalMarket);

                                    SunburstData sunburstData = new SunburstData();
                                    sunburstData.Id = "Dividendos";
                                    sunburstData.Value = resultSegments.Value.Sum(sectorTmp => sectorTmp.TotalMarket).ToString("n2", new CultureInfo("en-us")).Replace(",", string.Empty);
                                    sunburstDatas.Add(sunburstData);

                                    foreach (SegmentView segment in resultSegments.Value)
                                    {
                                        sunburstData = new SunburstData();
                                        sunburstData.Id = segment.IdSegment.ToString();
                                        sunburstData.Parent = "Dividendos";
                                        sunburstData.Label = segment.Name;
                                        sunburstData.Value = segment.TotalMarket.ToString("n2", new CultureInfo("en-us")).Replace(",", string.Empty);
                                        sunburstDatas.Add(sunburstData);

                                        ResultServiceObject<IEnumerable<StockAllocation>> resultSegmentStocks = _performanceStockService.GetSumSegmentStock(portfolioPerformance.IdPortfolioPerformance, segment.IdSegment);

                                        if (resultSegmentStocks.Success && resultSegmentStocks.Value != null)
                                        {
                                            foreach (StockAllocation segmentStock in resultSegmentStocks.Value)
                                            {
                                                sunburstData = new SunburstData();
                                                sunburstData.Id = segmentStock.Symbol;
                                                sunburstData.Parent = segment.IdSegment.ToString();
                                                sunburstData.Label = segmentStock.Symbol;
                                                sunburstData.Value = segmentStock.TotalMarket.ToString("n2", new CultureInfo("en-us")).Replace(",", string.Empty);
                                                sunburstDatas.Add(sunburstData);
                                            }
                                        }
                                    }
                                }

                                sunburstVM.LastUpdated = DateTime.Now.ToString("HH:mm:ss");
                                resultServiceObject.Value = sunburstVM;
                            }
                        }
                    }
                }
            }

            resultServiceObject.Success = true;

            return resultServiceObject;
        }

        public ResultResponseObject<SunburstVM> GetSubsectorAllocationChart(Guid guidPortfolioSub)
        {
            SunburstVM sunburstVM = new SunburstVM();
            ResultResponseObject<SunburstVM> resultServiceObject = new ResultResponseObject<SunburstVM>();
            List<SunburstData> sunburstDatas = new List<SunburstData>();
            sunburstVM.ListData = sunburstDatas;

            using (_uow.Create())
            {
                ResultServiceObject<Portfolio> resultPortfolio = _portfolioService.GetByGuid(guidPortfolioSub);
                ResultServiceObject<SubPortfolio> resultSubPortfolio = _subPortfolioService.GetByGuid(guidPortfolioSub);

                if (resultSubPortfolio.Success && resultPortfolio.Success)
                {
                    Portfolio portfolio = resultPortfolio.Value;
                    SubPortfolio subportfolio = resultSubPortfolio.Value;

                    long idPortfolio = 0;
                    bool isSub = false;

                    if (portfolio != null)
                    {
                        idPortfolio = portfolio.IdPortfolio;
                        sunburstVM.IdCountry = portfolio.IdCountry;
                    }
                    else if (subportfolio != null)
                    {
                        idPortfolio = subportfolio.IdPortfolio;
                        isSub = true;

                        resultPortfolio = _portfolioService.GetById(subportfolio.IdPortfolio);

                        if (resultPortfolio.Value != null)
                        {
                            sunburstVM.IdCountry = resultPortfolio.Value.IdCountry;
                        }
                    }


                    ResultServiceObject<PortfolioPerformance> resultPortfolioPerformance = _portfolioPerformanceService.GetByCalculationDate(idPortfolio);
                    PortfolioPerformance portfolioPerformance = null;

                    if (resultPortfolioPerformance.Success)
                    {
                        portfolioPerformance = resultPortfolioPerformance.Value;

                        ResultServiceObject<IEnumerable<SubsectorView>> resultSubsectors = new ResultServiceObject<IEnumerable<SubsectorView>>();

                        if (isSub)
                        {
                            resultSubsectors = _subsectorService.GetSumSubPortfolioPerformance(portfolioPerformance.IdPortfolioPerformance, subportfolio.IdSubPortfolio);
                        }
                        else
                        {
                            resultSubsectors = _subsectorService.GetSumIdPortfolioPerformance(portfolioPerformance.IdPortfolioPerformance);
                        }

                        if (resultSubsectors.Success)
                        {

                            if (resultSubsectors.Value != null && resultSubsectors.Value.Count() > 0)
                            {
                                resultSubsectors.Value = resultSubsectors.Value.OrderBy(sector => sector.TotalMarket);

                                SunburstData sunburstData = new SunburstData();
                                sunburstData.Id = "Dividendos";
                                sunburstData.Value = resultSubsectors.Value.Sum(sectorTmp => sectorTmp.TotalMarket).ToString("n2", new CultureInfo("en-us")).Replace(",", string.Empty);
                                sunburstDatas.Add(sunburstData);

                                foreach (SubsectorView subsector in resultSubsectors.Value)
                                {
                                    sunburstData = new SunburstData();
                                    sunburstData.Id = subsector.IdSubsector.ToString();
                                    sunburstData.Parent = "Dividendos";
                                    sunburstData.Label = subsector.Name;
                                    sunburstData.Value = subsector.TotalMarket.ToString("n2", new CultureInfo("en-us")).Replace(",", string.Empty);
                                    sunburstDatas.Add(sunburstData);

                                    ResultServiceObject<IEnumerable<StockAllocation>> resultSegmentStocks = _performanceStockService.GetSumSubsectorStock(portfolioPerformance.IdPortfolioPerformance, subsector.IdSubsector);

                                    if (resultSegmentStocks.Success && resultSegmentStocks.Value != null)
                                    {
                                        foreach (StockAllocation subsectorStock in resultSegmentStocks.Value)
                                        {
                                            sunburstData = new SunburstData();
                                            sunburstData.Id = subsectorStock.Symbol;
                                            sunburstData.Parent = subsector.IdSubsector.ToString();
                                            sunburstData.Label = subsectorStock.Symbol;
                                            sunburstData.Value = subsectorStock.TotalMarket.ToString("n2", new CultureInfo("en-us")).Replace(",", string.Empty);
                                            sunburstDatas.Add(sunburstData);
                                        }
                                    }
                                }
                            }

                            sunburstVM.LastUpdated = DateTime.Now.ToString("HH:mm:ss");
                            resultServiceObject.Value = sunburstVM;
                        }
                    }
                }
            }

            resultServiceObject.Success = true;

            return resultServiceObject;
        }

        public ResultResponseObject<SunburstVM> GetSectorAllocationChart(Guid guidPortfolioSub)
        {
            SunburstVM sunburstVM = new SunburstVM();
            ResultResponseObject<SunburstVM> resultServiceObject = new ResultResponseObject<SunburstVM>();
            List<SunburstData> sunburstDatas = new List<SunburstData>();
            sunburstVM.ListData = sunburstDatas;

            using (_uow.Create())
            {
                ResultServiceObject<Portfolio> resultPortfolio = _portfolioService.GetByGuid(guidPortfolioSub);
                ResultServiceObject<SubPortfolio> resultSubPortfolio = _subPortfolioService.GetByGuid(guidPortfolioSub);

                if (resultSubPortfolio.Success && resultPortfolio.Success)
                {
                    Portfolio portfolio = resultPortfolio.Value;
                    SubPortfolio subportfolio = resultSubPortfolio.Value;

                    long idPortfolio = 0;
                    bool isSub = false;

                    if (portfolio != null)
                    {
                        idPortfolio = portfolio.IdPortfolio;
                        sunburstVM.IdCountry = portfolio.IdCountry;
                    }
                    else if (subportfolio != null)
                    {
                        idPortfolio = subportfolio.IdPortfolio;
                        isSub = true;

                        resultPortfolio = _portfolioService.GetById(subportfolio.IdPortfolio);

                        if (resultPortfolio.Value != null)
                        {
                            sunburstVM.IdCountry = resultPortfolio.Value.IdCountry;
                        }
                    }


                    ResultServiceObject<PortfolioPerformance> resultPortfolioPerformance = _portfolioPerformanceService.GetByCalculationDate(idPortfolio);
                    PortfolioPerformance portfolioPerformance = null;

                    if (resultPortfolioPerformance.Success)
                    {
                        portfolioPerformance = resultPortfolioPerformance.Value;

                        ResultServiceObject<IEnumerable<SectorView>> resultSectorstest = new ResultServiceObject<IEnumerable<SectorView>>();

                        if (isSub)
                        {
                            resultSectorstest = _sectorService.GetSumSubPortfolioPerformance(portfolioPerformance.IdPortfolioPerformance, subportfolio.IdSubPortfolio);
                        }
                        else
                        {
                            resultSectorstest = _sectorService.GetSumIdPortfolioPerformance(portfolioPerformance.IdPortfolioPerformance);
                        }

                        if (resultSectorstest.Success)
                        {

                            if (resultSectorstest.Value != null && resultSectorstest.Value.Count() > 0)
                            {
                                resultSectorstest.Value = resultSectorstest.Value.OrderBy(sector => sector.TotalMarket);

                                SunburstData sunburstData = new SunburstData();
                                sunburstData.Id = "Dividendos";
                                sunburstData.Value = resultSectorstest.Value.Sum(sectorTmp => sectorTmp.TotalMarket).ToString("n2", new CultureInfo("en-us")).Replace(",", string.Empty);
                                sunburstDatas.Add(sunburstData);

                                foreach (SectorView sector in resultSectorstest.Value)
                                {
                                    sunburstData = new SunburstData();
                                    sunburstData.Id = sector.IdSector.ToString();
                                    sunburstData.Parent = "Dividendos";
                                    sunburstData.Label = sector.Name;
                                    sunburstData.Value = sector.TotalMarket.ToString("n2", new CultureInfo("en-us")).Replace(",", string.Empty);
                                    sunburstDatas.Add(sunburstData);

                                    ResultServiceObject<IEnumerable<StockAllocation>> resultSectorStocks = _performanceStockService.GetSumSectorStock(portfolioPerformance.IdPortfolioPerformance, sector.IdSector);

                                    if (resultSectorStocks.Success && resultSectorStocks.Value != null)
                                    {
                                        foreach (StockAllocation sectorStock in resultSectorStocks.Value)
                                        {
                                            sunburstData = new SunburstData();
                                            sunburstData.Id = sectorStock.Symbol;
                                            sunburstData.Parent = sector.IdSector.ToString();
                                            sunburstData.Label = sectorStock.Symbol;
                                            sunburstData.Value = sectorStock.TotalMarket.ToString("n2", new CultureInfo("en-us")).Replace(",", string.Empty);
                                            sunburstDatas.Add(sunburstData);
                                        }
                                    }
                                }
                            }

                            sunburstVM.LastUpdated = DateTime.Now.ToString("HH:mm:ss");
                            resultServiceObject.Value = sunburstVM;
                        }
                    }
                }
            }

            resultServiceObject.Success = true;

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

        public ResultResponseObject<ComparativeContainerChart> GetAssetsProgress(Guid guidPortfolioSub, DateRangeType dateRangeType, string startDate, string endDate)
        {
            ComparativeContainerChart comparativeChart = new ComparativeContainerChart();
            ResultResponseObject<ComparativeContainerChart> resultServiceObject = new ResultResponseObject<ComparativeContainerChart>();
            List<ChartLabelValue> lstDataset = new List<ChartLabelValue>();
            comparativeChart.ListChartLabelValue = lstDataset;
            resultServiceObject.Value = comparativeChart;

            switch (dateRangeType)
            {
                case DateRangeType.All:
                    break;
                case DateRangeType.Month:
                    startDate = DateTime.Now.AddMonths(-1).ToString("dd/MM/yyyy");
                    endDate = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
                    break;
                case DateRangeType.Quarter:
                    startDate = DateTime.Now.AddMonths(-4).ToString("dd/MM/yyyy");
                    endDate = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
                    break;
                case DateRangeType.Semester:
                    startDate = DateTime.Now.AddMonths(-6).ToString("dd/MM/yyyy");
                    endDate = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
                    break;
                case DateRangeType.Custom:
                    break;
                default:
                    break;
            }

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

            using (_uow.Create())
            {
                ResultServiceObject<Portfolio> resultPortfolio = _portfolioService.GetByGuid(guidPortfolioSub);
                ResultServiceObject<SubPortfolio> resultSubPortfolio = _subPortfolioService.GetByGuid(guidPortfolioSub);

                if (resultSubPortfolio.Success && resultPortfolio.Success)
                {
                    Portfolio portfolio = resultPortfolio.Value;
                    SubPortfolio subportfolio = resultSubPortfolio.Value;

                    string portfolioName = string.Empty;

                    long idPortfolio = 0;
                    bool isSub = false;

                    if (portfolio != null)
                    {
                        idPortfolio = portfolio.IdPortfolio;
                        portfolioName = portfolio.Name;
                        comparativeChart.IdCountry = portfolio.IdCountry;
                    }
                    else if (subportfolio != null)
                    {
                        idPortfolio = subportfolio.IdPortfolio;
                        portfolioName = subportfolio.Name;
                        isSub = true;

                        resultPortfolio = _portfolioService.GetById(subportfolio.IdPortfolio);

                        if (resultPortfolio.Success)
                        {
                            portfolio = resultPortfolio.Value;
                            comparativeChart.IdCountry = portfolio.IdCountry;
                        }
                    }

                    List<PortfolioPerformance> portfolioPerformances = new List<PortfolioPerformance>();

                    if (isSub)
                    {
                        ResultServiceObject<IEnumerable<PortfolioView>> resultSubPortfolioPerf = _portfolioService.GetSubportfolioPerformance(subportfolio.GuidSubPortfolio, startDateParam, endDateParam);

                        if (resultSubPortfolioPerf.Value != null && resultSubPortfolioPerf.Value.Count() > 0)
                        {
                            foreach (PortfolioView portfolioView in resultSubPortfolioPerf.Value)
                            {
                                portfolioPerformances.Add(new PortfolioPerformance { CalculationDate = portfolioView.CalculationDate, TotalMarket = portfolioView.TotalMarket });
                            }
                        }
                    }
                    else
                    {
                        ResultServiceObject<IEnumerable<PortfolioPerformance>> resultPortfolioPerf = _portfolioPerformanceService.GetByPortfolioSkipHoliday(idPortfolio, startDateParam, endDateParam);
                        portfolioPerformances = resultPortfolioPerf.Value.ToList();
                    }

                    if (portfolioPerformances != null && portfolioPerformances.Count > 0)
                    {
                        for (int i = 0; i < portfolioPerformances.Count; i++)
                        {
                            string showLabel = "0";
                            string perfDate = portfolioPerformances[i].CalculationDate.ToString("dd/MM/yy");

                            if (i == 0 || i == portfolioPerformances.Count - 1)
                            {
                                string monthName = portfolioPerformances[i].CalculationDate.ToString("MMM", new CultureInfo("pt-br"));
                                showLabel = "1";
                                perfDate = string.Format("{0}/{1}", monthName.First().ToString().ToUpper() + monthName.Substring(1), portfolioPerformances[i].CalculationDate.ToString("yy")).Replace(".", string.Empty);
                            }

                            ChartLabelValue chartLabelValue = new ChartLabelValue();
                            chartLabelValue.Label = perfDate;
                            chartLabelValue.Value = portfolioPerformances[i].TotalMarket.ToString("n2", new CultureInfo("en-us")).Replace(",", string.Empty);
                            chartLabelValue.ShowLabel = showLabel;

                            comparativeChart.ListChartLabelValue.Add(chartLabelValue);
                        }
                    }

                    comparativeChart.PortfolioName = portfolioName;
                    comparativeChart.LastUpdated = DateTime.Now.ToString("HH:mm:ss");
                }
            }

            if (resultServiceObject.ErrorMessages != null && resultServiceObject.ErrorMessages.Count() > 0)
            {
                resultServiceObject.Success = false;
            }

            resultServiceObject.Success = true;

            return resultServiceObject;
        }

        public ResultResponseObject<DividendChartScrollVM> GetDividendPerMonthChart(Guid guidPortfolioSub, string startDate, string endDate)
        {
            ResultResponseObject<DividendChartScrollVM> resultServiceObject = new ResultResponseObject<DividendChartScrollVM>();
            DividendChartScrollVM dividendChart = new DividendChartScrollVM();
            dividendChart.Categories = new List<DividendChartScrollCategory>();
            dividendChart.Dataset = new List<Dividendos.API.Model.Response.Dataset>();

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

                    if (isSub)
                    {
                        resultDividend = _dividendService.GetAllActiveBySubPortfolioRangeDate(subportfolio.IdSubPortfolio, startDateParam, endDateParam);
                    }
                    else
                    {
                        resultDividend = _dividendService.GetAllActiveByPortfolioRangeDate(portfolio.IdPortfolio, startDateParam, endDateParam);
                    }


                    if (resultDividend.Success)
                    {
                        List<DividendView> dividends = resultDividend.Value.ToList();

                        if (dividends != null && dividends.Count() > 0)
                        {
                            int startYear = 0;
                            int endYear = 0;

                            if (startDateParam.HasValue)
                            {
                                startYear = startDateParam.Value.Year;
                            }
                            else
                            {
                                List<DividendView> dividendDetailsDate = dividends.FindAll(div => div.PaymentDate.HasValue);

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
                                List<DividendView> dividendDetailsDate = dividends.FindAll(div => div.PaymentDate.HasValue);

                                if (dividendDetailsDate != null && dividendDetailsDate.Count > 0)
                                {
                                    endYear = dividendDetailsDate.Max(div => div.PaymentDate).Value.Year;
                                }
                            }


                            DividendChartScrollCategory dividendChartScrollCategory = new DividendChartScrollCategory();
                            dividendChart.Categories.Add(dividendChartScrollCategory);
                            dividendChartScrollCategory.Category = new List<CategoryCategory>();

                            if (startYear != 0 && endYear != 0)
                            {
                                for (int year = startYear; year <= endYear; year++)
                                {
                                    Dividendos.API.Model.Response.Dataset dtYear = new API.Model.Response.Dataset();
                                    dtYear.SeriesName = year.ToString();
                                    dtYear.Data = new List<API.Model.Response.Datum>();

                                    dividendChart.Dataset.Add(dtYear);
                                }


                                for (int month = 1; month <= 12; month++)
                                {
                                    string monthName = new DateTime(DateTime.Now.Year, month, 1).ToString("MMM", new CultureInfo("pt-br"));
                                    CategoryCategory categoryCategory = new CategoryCategory();
                                    categoryCategory.Label = monthName.First().ToString().ToUpper() + monthName.Substring(1);
                                    dividendChartScrollCategory.Category.Add(categoryCategory);

                                    for (int year = startYear; year <= endYear; year++)
                                    {
                                        decimal sumMonth = 0;
                                        List<DividendView> dividendDetailsMonth = dividends.FindAll(divTmp => divTmp.PaymentDate.HasValue && divTmp.PaymentDate.Value.Month == month && divTmp.PaymentDate.Value.Year == year);

                                        if (dividendDetailsMonth != null && dividendDetailsMonth.Count > 0)
                                        {
                                            sumMonth = dividendDetailsMonth.Sum(divM => divM.NetValue);
                                        }

                                        Dividendos.API.Model.Response.Dataset dtYear = dividendChart.Dataset.First(divChart => divChart.SeriesName == year.ToString());

                                        API.Model.Response.Datum datumMonth = new API.Model.Response.Datum();
                                        datumMonth.Value = sumMonth.ToString("n2", new CultureInfo("en-us")).Replace(",", string.Empty);

                                        if (sumMonth.Equals(0))
                                        {
                                            datumMonth.ShowValues = "0";
                                        }
                                        else
                                        {
                                            datumMonth.ShowValues = "1";
                                        }

                                        dtYear.Data.Add(datumMonth);
                                    }
                                }

                            }
                            dividendChart.LastUpdated = DateTime.Now.ToString("HH:mm:ss");
                            //dividendChart.Title = string.Format("Calendário {0}", year);
                            //dividendChart.Year = year.ToString();
                        }
                    }
                }
            }

            resultServiceObject.Value = dividendChart;
            resultServiceObject.Success = true;

            return resultServiceObject;
        }
    }
}
