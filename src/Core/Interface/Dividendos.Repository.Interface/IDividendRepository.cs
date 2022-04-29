using Dividendos.Entity.Entities;
using Dividendos.Repository.Interface.GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Repository.Interface
{
    public interface IDividendRepository : IRepository<Dividend>
    {
        IEnumerable<Dividend> GetByPortfolio(long idPortfolio);
        IEnumerable<Dividend> GetByNotificationStatusAndPaymentDate(bool notificationSent, DateTime paymentDate);
    }
}
