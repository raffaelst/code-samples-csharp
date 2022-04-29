using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Service.Interface
{
    public interface IDividendService : IBaseService
    {
        ResultServiceObject<IEnumerable<DividendView>> GetByPortfolio(long idPortfolio);
        ResultServiceObject<IEnumerable<DividendView>> GetAllAutomaticByPortfolio(long idPortfolio);
        ResultServiceObject<IEnumerable<DividendView>> GetAllActiveByPortfolio(long idPortfolio);
        ResultServiceObject<IEnumerable<DividendView>> GetAllActiveByPortfolioRangeDate(long idPortfolio, DateTime? startDate, DateTime? endDate);
        ResultServiceObject<IEnumerable<DividendView>> GetAllActiveBySubPortfolioRangeDate(long idSubPortfolio, DateTime? startDate, DateTime? endDate);
        ResultServiceObject<Dividend> Update(Dividend dividend);
        ResultServiceObject<Dividend> Insert(Dividend dividend);
        ResultServiceObject<Dividend> GetById(long idDividend);
        ResultServiceObject<IEnumerable<DividendView>> GetByNotificationStatusAndPaymentDate(bool morningNotification);
        ResultServiceObject<IEnumerable<DividendDetailsView>> GetDetailsByPortfolio(long idPortfolio, DateTime? startDate = null, DateTime? endDate = null, long? idStock = null, int? idStockType = null);
        ResultServiceObject<IEnumerable<DividendDetailsView>> GetDetailsBySubportfolio(long idSubPortfolio, DateTime? startDate = null, DateTime? endDate = null, long? idStock = null, int? idStockType = null);
        ResultServiceObject<IEnumerable<DividendView>> GetAllByDate(DateTime startDate, DateTime endDate);
        ResultServiceObject<IEnumerable<DividendView>> GetNextDividendByUser(string user, int amountToReturn);
        ResultServiceObject<IEnumerable<DividendInfoView>> GetManualPortfoliosWithStock(long idStock);
        ResultServiceObject<IEnumerable<string>> GetAllUsersWithStock(long idStock);
        ResultServiceObject<bool> ExistInDataBase(Dividend dividend);
        ResultServiceObject<IEnumerable<Dividend>> GetByFutureDate(long idPortfolio);
        ResultServiceObject<bool> Delete(Dividend dividend);
        string Abreviation(int idDividendType);
        List<string> RestorePastDividends(long idPortfolio, int idCountry, IOperationItemService _operationItemService, IDividendCalendarService _dividendCalendarService, IOperationService _operationService, IStockService _stockService, IPerformanceStockService _performanceStockService, IPortfolioService _portfolioService, DateTime? minDataCom = null, bool? cei = null, long? idOldPortfolio = null, bool? newB3 = null);
        ResultServiceObject<IEnumerable<Dividend>> GetAutomaticByIdPortfolio(long idPortfolio);
        void RemoveDuplicated(long idPortfolio, IDividendCalendarService _dividendCalendarService);
        void RemoveDuplicatedEasyInvest(long idPortfolio, IDividendCalendarService _dividendCalendarService);
        ResultServiceObject<IEnumerable<Dividend>> GetByIdPortfolio(long idPortfolio);
        ResultServiceObject<IEnumerable<Dividend>> GetByIdPortfolioActive(long idPortfolio);
        ResultServiceObject<IEnumerable<Dividend>> GetActiveByPortfolio(long idPortfolio);
    }
}
