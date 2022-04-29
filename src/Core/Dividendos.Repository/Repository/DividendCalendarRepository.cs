using Dapper;
using Dividendos.Entity.Entities;
using Dividendos.Repository.GenericRepository;
using Dividendos.Repository.Interface;
using Dividendos.Repository.Interface.UoW;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Repository.Repository
{
    public class DividendCalendarRepository : Repository<DividendCalendar>, IDividendCalendarRepository
    {
        private IUnitOfWork _unitOfWork;

        public DividendCalendarRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Delete(long idDividendCalendar)
        {
            string sql = $"DELETE FROM DividendCalendar WHERE DividendCalendarID = @DividendCalendarID";

            _unitOfWork.Connection.Execute(sql, new { DividendCalendarID = idDividendCalendar }, _unitOfWork.Transaction);
        }

        public void DeleteByStockAllFutureItens(long stockID)
        {
            string sql = $"DELETE FROM DividendCalendar WHERE IdStock = @StockID AND DataCom >= @DateLimit";

            _unitOfWork.Connection.Execute(sql, new { StockID = stockID, DateLimit = DateTime.Now.AddDays(2) }, _unitOfWork.Transaction);
        }

        public IEnumerable<DividendCalendar> GetDividendRangeDate(long idStock, DateTime minDate, DateTime maxDate)
        {
            string sql = @"select DividendCalendarID,IdStock,IdDividendType,DataCom,PaymentDate,Value,PaymentDatepartiallyUndefined,PaymentUndefined,IdCountry from DividendCalendar where IdStock = @IdStock and DataCom between @MinDate and @MaxDate ";

            IEnumerable<DividendCalendar> dividendCalendars = _unitOfWork.Connection.Query<DividendCalendar>(sql, new { IdStock = idStock, MinDate = minDate, MaxDate = maxDate }, _unitOfWork.Transaction);

            return dividendCalendars;
        }
    }
}
