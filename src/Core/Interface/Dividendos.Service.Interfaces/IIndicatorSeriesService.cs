using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Service.Interface
{
    public interface IIndicatorSeriesService : IBaseService
    {
        ResultServiceObject<IEnumerable<IndicatorSeries>> GetAll();
        ResultServiceObject<IndicatorSeries> GetByCalculationDate(long idIndicatorType, DateTime calculationDate, int idPeriodType);
        ResultServiceObject<IndicatorSeries> Update(IndicatorSeries indicatorSeries);
        ResultServiceObject<IndicatorSeries> Insert(IndicatorSeries indicatorSeries);
        ResultServiceObject<IEnumerable<IndicatorSeriesView>> GetByRangeDate(long idIndicatorType);
        ResultServiceObject<IEnumerable<IndicatorSeriesView>> GetMonthRange(long idIndicatorType);
        ResultServiceObject<IEnumerable<IndicatorSeriesView>> GetLatest(long idIndicatorType);
        ResultServiceObject<IndicatorSeriesView> GetLatestByMonth(long idIndicatorType, int month);
        ResultServiceObject<IndicatorSeriesView> GetByCalculationDateView(long idIndicatorType, DateTime calculationDate, int idPeriodType);
        ResultServiceObject<IndicatorSeriesView> GetByMonth(long idIndicatorType, DateTime currentDate, bool first);
        ResultServiceObject<IndicatorSeriesView> GetByYear(long idIndicatorType, DateTime currentDate, bool first);
        ResultServiceObject<IndicatorSeriesView> GetSumByMonth(long idIndicatorType, int month);
        ResultServiceObject<IndicatorSeriesView> GetSumByYear(long idIndicatorType, int year);
        ResultServiceObject<IEnumerable<IndicatorSeriesView>> GetAllLatest();
        ResultServiceObject<IEnumerable<IndicatorSeriesView>> GetAllLatestInvestingCom();
        ResultServiceObject<IndicatorSeries> GetById(long idIndicatorSeries);
        ResultServiceObject<IEnumerable<IndicatorSeriesView>> GetAllLatestCommodities();
        ResultServiceObject<IEnumerable<IndicatorSeriesView>> GetAllLatestForex();
    }
}
