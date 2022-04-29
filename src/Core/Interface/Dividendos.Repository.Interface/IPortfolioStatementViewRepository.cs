using Dividendos.Entity.Views;
using Dividendos.Repository.Interface.GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;


namespace Dividendos.Repository.Interface
{
    public interface IPortfolioStatementViewRepository : IRepository<PortfolioStatementView>
    {
        IEnumerable<PortfolioStatementView> GetByPortfolio(Guid guidPortfolio, int? idStockType);
        IEnumerable<PortfolioStatementView> GetBySubportfolio(Guid guidSubportfolio, int? idStockType);
        IEnumerable<PortfolioStatementView> GetZeroPriceByUser(string idUser);

        StockStatementView GetByIdStock(Guid guidPortfolio, long idStock);
        IEnumerable<PortfolioStatementView> GetByPortfolioSubscription(Guid guidPortfolio);
        IEnumerable<PortfolioStatementView> GetBySubportfolioSubscription(Guid guidSubportfolio);
    }
}
