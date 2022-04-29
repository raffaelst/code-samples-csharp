using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Service.Interface
{
    public interface IHolidayService : IBaseService
    {
        ResultServiceObject<IEnumerable<Holiday>> GetAll();
        ResultServiceObject<IEnumerable<Holiday>> GetByCountry(int idCountry);
        ResultServiceObject<Holiday> Insert(Holiday holiday);
        ResultServiceObject<Holiday> Update(Holiday holiday);
        bool IsHoliday(DateTime date, int idCountry);
        DateTime PreviousWorkDay(int idCountry, bool isNightCei);
        DateTime PreviousWorkDay(int idCountry, DateTime date);
    }
}
