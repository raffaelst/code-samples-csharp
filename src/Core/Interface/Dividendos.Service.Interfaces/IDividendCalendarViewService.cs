using Dividendos.Entity.Entities;
using Dividendos.Entity.Enum;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Service.Interface
{
    public interface IDividendCalendarViewService : IBaseService
    {
        ResultServiceObject<IEnumerable<DividendCalendarView>> GetAll();

        ResultServiceObject<IEnumerable<DividendCalendarView>> GetByDataComByYear(DateTime startDate, DateTime endDate, CountryEnum countryEnum, string idUser, bool onlyMyStocks, List<StockTypeEnum> stockTypes);

        ResultServiceObject<IEnumerable<DividendCalendarView>> GetByPaymentDateByYear(DateTime startDate, DateTime endDate, CountryEnum countryEnum, string idUser, bool onlyMyStocks, List<StockTypeEnum> stockTypes);

        ResultServiceObject<IEnumerable<DividendCalendarView>> GetByDataComBySymbol(DateTime startDate, DateTime endDate, string symbol);

        ResultServiceObject<IEnumerable<DividendCalendarView>> GetByPaymentDateBySymbol(DateTime startDate, DateTime endDate, string symbol);

        ResultServiceObject<IEnumerable<DividendCalendarView>> GetAllByDataEx(DateTime paymentDate);

        ResultServiceObject<IEnumerable<DividendCalendarView>> GetByDataCom(DateTime paymentDate);

        ResultServiceObject<IEnumerable<DividendCalendarView>> GetNextDataComByUser(DateTime dataComStartDate, DateTime dataComEndDate, string idUser);
    }
}
