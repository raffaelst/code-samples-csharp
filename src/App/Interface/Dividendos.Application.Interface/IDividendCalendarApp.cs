using Dividendos.API.Model.Request.DividendCalendar;
using Dividendos.API.Model.Request.Settings;
using Dividendos.API.Model.Request.Stock;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using Dividendos.Entity.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Application.Interface
{
    public interface IDividendCalendarApp
    {
        void GetAndUpdateFromSIBDRs();
        void GetAndUpdateFromSIFIIs();
        void GetAndUpdateFromSIStocks();

        void GetAndUpdateFromNasdaq(DateTime starDate, DateTime endDate);

        void CreateDividendForManualPortfolios();

        ResultResponseObject<DividendCalendarChart> GetDividendChart(int? year = null);

        ResultResponseObject<DividendCalendarResultWrapperVM> GetListByYear(int year, int month, CountryType countryType, DividendCalendarType dividendCalendarType, bool onlyMyStocks, List<StockType> stockTypes);

        ResultResponseObject<IEnumerable<DividendCalendarVM>> GetListBySymbol(DateTime startDate, DateTime endDate, string symbol, DividendCalendarType dividendCalendarType);

        void SendNotificationDividendDataComRemind();

        ResultResponseObject<IEnumerable<DividendCalendarVM>> GetListByYear(string token, int year, CountryType countryType, DividendCalendarType dividendCalendarType);

        void ApproveAndUpdateDividendCalendar();

        ResultResponseObject<DividendNextDataComWrapperVM> GetNextDataComByUser();

        void GetAllAndUpdateFromSIFIIs();

        void GetAllAndUpdateFromSIStocks();

        void GetAllAndUpdateFromSIBDRs();

        void GetDividendsFromIexAPI();
    }
}
