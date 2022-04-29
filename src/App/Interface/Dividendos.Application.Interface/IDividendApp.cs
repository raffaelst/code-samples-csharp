using Dividendos.API.Model.Request.Dividend;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using Dividendos.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Application.Interface
{
    public interface IDividendApp
    {
        void SendNotification(bool morningNotification);
        ResultResponseObject<DividendEditVM> EditDividend(long idDividend, DividendEditVM dividendEditVM);
        ResultResponseObject<DividendAddVM> AddDividend(long idStock, DividendAddVM dividendAddVM);
        ResultResponseBase InactiveDividend(long idDividend);

        ResultResponseObject<DividendCalendarChart> GetDividendChart(Guid guidPortfolioSub, int? year = null);
        ResultResponseObject<IEnumerable<NextDividendVM>> GetNextDividendsByLoggedUser();
        ResultResponseObject<IEnumerable<API.Model.Response.v2.NextDividendVM>> GetNextDividendsByLoggedUserV2();
        ResultResponseObject<IEnumerable<API.Model.Response.v2.NextDividendVM>> GetNextDividendsByLoggedUserV3();

        ResultResponseObject<API.Model.Response.v4.NextDividendWrapperVM> GetNextDividendsByLoggedUserV4();
        ResultResponseObject<DividendViewWrapperVM> GetDividendView(Guid guidPortfolioSub, string startDate, string endDate);
        ResultResponseObject<DividendYieldWrapperVM> GetDividendYieldList(Guid guidPortfolioSub, string startDate, string endDate, long? idStock = null, int? idStockType = null);
        ResultResponseObject<IEnumerable<NextDividendVM>> GetDividendList(Guid guidPortfolioSub, int year, int month, long? idStock = null, int? idStockType = null);
        ResultResponseObject<DividendYieldWrapperVM> GetRankingDividendYield(Guid guidPortfolioSub, int year, int? month, long? idStock = null, int? idStockType = null);
        ResultResponseBase RestorePastDividends(Guid guidPortfolio, string token);
        List<string> RestorePastDividends(long idPortfolio, int idCountry);
        List<string> RestorePastDividends(long idPortfolio, int idCountry, DateTime? dataCom);
    }
}
