using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dividendos.StatusInvest.Interface.Model;

namespace Dividendos.StatusInvest.Interface
{
    public interface IStatusInvestHelper
    {
        Task<List<Datum>> GetCompanies(int type);

        Task<List<DividendCalendarItem>> GetDividendCalendar(DateTime startDate, DateTime finalDate, string symbol, AssetsTypeEnum assetsTypeEnum);
    }
}
