using Dividendos.Entity.Views;
using Dividendos.Repository.Interface.GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Repository.Interface
{
    public interface IPortfolioViewRepository : IRepository<PortfolioView>
    {
        IEnumerable<PortfolioView> GetByUser(string idUser);
        IEnumerable<PortfolioView> GetSubportfolioPerformance(Guid guidPortfolioSub, DateTime? startDate, DateTime? endDate);
    }
}
