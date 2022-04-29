using Dividendos.Entity.Entities;
using Dividendos.Entity.Views;
using Dividendos.Repository.Interface.GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;


namespace Dividendos.Repository.Interface
{
    public interface IDividendViewRepository : IRepository<DividendView>
    {
        IEnumerable<DividendView> GetByPortfolio(long idPortfolio, bool? automatic = null, bool? active = null, DateTime? startDate = null, DateTime? endDate = null);
        IEnumerable<DividendView> GetAllActiveBySubPortfolio(long idSubPortfolio, DateTime? startDate = null, DateTime? endDate = null);
        IEnumerable<DividendView> GetByNotificationStatusAndPaymentDate(bool notificationSent, DateTime paymentDate);
        IEnumerable<DividendDetailsView> GetDetailsByPortfolio(long idPortfolio, DateTime? startDate = null, DateTime? endDate = null, long? idStock = null, int? idStockType = null);
        IEnumerable<DividendDetailsView> GetDetailsBySubportfolio(long idSubportfolio, DateTime? startDate = null, DateTime? endDate = null, long? idStock = null, int? idStockType = null);

        IEnumerable<DividendView> GetAllByDate(DateTime startDate, DateTime endDate);
        IEnumerable<DividendView> GetNextDividendByUser(string user, DateTime dateTime, int amountToReturn);

        bool ExistInDataBase(Dividend dividend);
    }
}
