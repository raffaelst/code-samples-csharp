using Dapper;
using Dividendos.Entity.Entities;
using Dividendos.Repository.GenericRepository;
using Dividendos.Repository.Interface;
using Dividendos.Repository.Interface.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dividendos.Repository.Repository
{
    public class DividendCalendarWaitApprovalRepository : Repository<DividendCalendarWaitApproval>, IDividendCalendarWaitApprovalRepository
    {
        private IUnitOfWork _unitOfWork;

        public DividendCalendarWaitApprovalRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public void Delete(long idDividendCalendar)
        {
            string sql = $"DELETE FROM DividendCalendarWaitApproval WHERE DividendCalendarWaitApprovalID = @DividendCalendarID";

            _unitOfWork.Connection.Execute(sql, new { DividendCalendarID = idDividendCalendar }, _unitOfWork.Transaction);
        }


        public DividendCalendarWaitApproval ExistByDateAndStock(long idStock, DateTime dataCom, DateTime? paymentDate, decimal value)
        {
            StringBuilder sql = new StringBuilder(@"SELECT DividendCalendarWaitApprovalID
                      FROM DividendCalendarWaitApproval
					  WHERE IdStock = @IdStock AND Value = @Value AND DataCom = @DataCom AND ");

            if (paymentDate.HasValue)
            {
                sql.Append("PaymentDate = @PaymentDate ");
            }
            else
            {
                sql.Append("PaymentDate IS NULL ");
            }

            DividendCalendarWaitApproval dividend = _unitOfWork.Connection.Query<DividendCalendarWaitApproval>(sql.ToString(), new { IdStock = idStock, Value = value, DataCom = dataCom, PaymentDate = paymentDate }, _unitOfWork.Transaction).FirstOrDefault();

            return dividend;
        }
    }
}
