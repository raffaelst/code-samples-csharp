using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Service.Interface
{
    public interface IDividendCalendarService : IBaseService
    {
        ResultServiceObject<IEnumerable<DividendCalendar>> Save(List<DividendCalendar> dividendCalendars);
        ResultServiceObject<DividendCalendar> Save(DividendCalendar dividendCalendar);

        ResultServiceObject<IEnumerable<DividendCalendarView>> GetByDataComLimit(DateTime startDate);

        ResultServiceObject<IEnumerable<DividendCalendarView>> GetByPaymentDateAndStock(DateTime paymentStartDate, DateTime paymentEndDate, long idStock);

        void Delete(DividendCalendarView dividendCalendarView);

        ResultServiceObject<IEnumerable<DividendCalendarWaitApproval>> SaveWaitForApproval(List<DividendCalendarWaitApproval> dividendCalendarsWaitApproval);

        ResultServiceObject<DividendCalendarWaitApproval> SaveWaitForApproval(DividendCalendarWaitApproval dividendCalendar);

        ResultServiceObject<IEnumerable<DividendCalendarWaitApproval>> GetAllWaitApproval();

        void DeleteWaitApproval(long dividendCalendarWaitApprovalID);
        ResultServiceObject<IEnumerable<DividendCalendar>> GetDividendRangeDate(long idStock, DateTime minDate, DateTime maxDate);
        ResultServiceObject<IEnumerable<DividendCalendar>> GetByPaymentDateAndStock(DateTime paymentDate, long idStock);
        void DeleteByStockAllFutureItens(long stockID);
    }
}
