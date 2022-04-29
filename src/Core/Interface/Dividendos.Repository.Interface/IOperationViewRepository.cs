using Dividendos.Entity.Entities;
using Dividendos.Entity.Views;
using Dividendos.Repository.Interface.GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Repository.Interface
{
    public interface IOperationViewRepository : IRepository<OperationView>
    {
        IEnumerable<OperationView> GetByIdPortfolio(long idPortfolio);

        IEnumerable<OperationView> GetByIdSubPortfolio(long idSubPortfolio);

        IEnumerable<OperationSellDetailsView> GetSellDetailsByIdPortfolio(long idPortfolio, DateTime? startDate, DateTime? endDate);

        IEnumerable<OperationSellDetailsView> GetSellDetailsByIdSubportfolio(long idPortfolio, long idSubPortfolio, DateTime? startDate, DateTime? endDate);

        IEnumerable<OperationSummaryView> GetOperationSummary(string idUser);

        IEnumerable<OperationItemView> GetOperationItemsByIdPortfolio(long idPortfolio);

        IEnumerable<OperationSummaryView> GetOperationSummaryByPortfolioOrSubPortfolio(string idUser, string idPortfolio);

        IEnumerable<OperationBuyDetailsView> GetBuyDetailsByIdPortfolio(long idPortfolio, DateTime? startDate, DateTime? endDate);

        IEnumerable<OperationBuyDetailsView> GetBuyDetailsByIdSubportfolio(long idPortfolio, long idSubPortfolio, DateTime? startDate, DateTime? endDate);
    }
}
