using Dividendos.Entity.Entities;
using Dividendos.Repository.GenericRepository;
using Dividendos.Repository.Interface;
using Dividendos.Repository.Interface.UoW;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Linq;


namespace Dividendos.Repository.Repository
{
    public class DividendRepository : Repository<Dividend>, IDividendRepository
    {
        private IUnitOfWork _unitOfWork;

        public DividendRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Dividend> GetByPortfolio(long idPortfolio)
        {
            string sql = @"SELECT Dividend.IdDividend, Dividend.IdStock, stock.Symbol, Dividend.IdPortfolio, Dividend.PaymentDate, Dividend.BaseQuantity, Dividend.GrossValue, Dividend.NetValue, Dividend.NotificationSent, Dividend.GuidDividend, Dividend.HomeBroker, Dividend.IdDividendType, Dividend.DateAdded 
                           FROM Dividend 
                           inner join stock on stock.IdStock = Dividend.IdStock 
                           where Dividend.IdPortfolio = @IdPortfolio 
                           order by stock.symbol  ";

            IEnumerable<Dividend> dividends = _unitOfWork.Connection.Query<Dividend>(sql, new { IdPortfolio = idPortfolio }, _unitOfWork.Transaction);

            return dividends;
        }

        public IEnumerable<Dividend> GetByNotificationStatusAndPaymentDate(bool notificationSent, DateTime paymentDate)
        {
            string sql = @"SELECT Dividend.IdDividend, DividendType.Name, Dividend.IdStock, stock.Symbol, Dividend.IdPortfolio, Dividend.PaymentDate, Dividend.BaseQuantity, Dividend.GrossValue, Dividend.NetValue, Dividend.NotificationSent, Dividend.GuidDividend, Dividend.HomeBroker, Dividend.IdDividendType, Dividend.DateAdded 
                          FROM Dividend 
                          INNER JOIN Portfolio ON Portfolio.IdPortfolio = Dividend.IdPortfolio 
                          INNER JOIN stock on stock.IdStock = Dividend.IdStock 
                          INNER JOIN DividendType on DividendType.IdDividendType = Dividend.IdDividendType 
                          WHERE AND Dividend.NotificationSent = @NotificationSent AND Dividend.PaymentDate = @PaymentDate";

            IEnumerable<Dividend> dividends = _unitOfWork.Connection.Query<Dividend>(sql, new { NotificationSent = notificationSent, PaymentDate = paymentDate }, _unitOfWork.Transaction);

            return dividends;
        }
    }
}
