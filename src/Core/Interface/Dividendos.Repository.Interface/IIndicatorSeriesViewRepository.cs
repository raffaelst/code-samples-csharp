using Dividendos.Entity.Entities;
using Dividendos.Entity.Views;
using Dividendos.Repository.Interface.GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Repository.Interface
{
    public interface IIndicatorSeriesViewRepository : IRepository<IndicatorSeriesView>
    {
        IEnumerable<IndicatorSeriesView> GetByRangeDate(long idIndicatorType);
        IEnumerable<IndicatorSeriesView> GetMonthRange(long idIndicatorType);
        IEnumerable<IndicatorSeriesView> GetLatest(long idIndicatorType);
        IEnumerable<IndicatorSeriesView> GetLatestByMonth(long idIndicatorType, int month);
        IEnumerable<IndicatorSeriesView> GetByCalculationDateView(long idIndicatorType, DateTime calculationDate, int idPeriodType);
        IEnumerable<IndicatorSeriesView> GetByMonth(long idIndicatorType, DateTime currentDate, bool first);
        IEnumerable<IndicatorSeriesView> GetByYear(long idIndicatorType, DateTime currentDate, bool first);
        IEnumerable<IndicatorSeriesView> GetSumByYear(long idIndicatorType, int year);
        IEnumerable<IndicatorSeriesView> GetSumByMonth(long idIndicatorType, int month);
        IEnumerable<IndicatorSeriesView> GetAllLatest();
        IEnumerable<IndicatorSeriesView> GetAllLatestInvestingCom();
        IEnumerable<IndicatorSeriesView> GetAllLatestCommodities();
        IEnumerable<IndicatorSeriesView> GetAllLatestForex();
    }
}
