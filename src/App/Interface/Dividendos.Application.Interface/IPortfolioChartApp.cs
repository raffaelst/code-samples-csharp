using Dividendos.API.Model.Request.Charts;
using Dividendos.API.Model.Request.Portfolio;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Charts;
using Dividendos.API.Model.Response.Common;
using Dividendos.Application.Interface.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Application.Interface
{
    public interface IPortfolioChartApp
    {
        ResultResponseObject<DividendChartScrollVM> GetDividendStackedChart(Guid guidPortfolioSub, int year);
        ResultResponseObject<TreeMapWrapperVM> GetTreeMapChart(Guid guidPortfolioSub);
        ResultResponseObject<SunburstVM> GetSunburstChart(Guid guidPortfolioSub, SunburstType sunburstType);
        ResultResponseObject<ComparativeContainerChart> GetAssetsProgress(Guid guidPortfolioSub, DateRangeType dateRangeType, string startDate, string endDate);
        ResultResponseObject<DividendChartScrollVM> GetDividendPerMonthChart(Guid guidPortfolioSub, string startDate, string endDate);
    }
}
