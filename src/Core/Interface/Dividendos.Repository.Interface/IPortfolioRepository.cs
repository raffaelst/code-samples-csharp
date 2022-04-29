using Dividendos.Entity.Entities;
using Dividendos.Repository.Interface.GenericRepository;
using System;
using System.Collections.Generic;

namespace Dividendos.Repository.Interface
{
    public interface IPortfolioRepository : IRepository<Portfolio>
    {
        IEnumerable<Portfolio> GetByUserAndStatus(string idUser, bool status, bool? manual, bool onlySelectedToShowOnPatrimony);
        IEnumerable<Portfolio> GetLastPortfoliosWithoutCalculation(DateTime currentDate);
        IEnumerable<Portfolio> GetByGuidOperationItem(long idOperationItem);
        IEnumerable<Portfolio> GetByIdOperationItemHist(long idOperationItemHist);

        bool HasSubscription(Guid guidPortfolioOrSubportfolio);

        void UpdateCalculatePerformanceDate(long idPortfolio, DateTime calculatePerformanceDate);

        void UpdateName(long idPortfolio, string name);
        decimal? GetTotalPortfolio(long idPortfolio, DateTime limitDate, long? idStock = null, int? idStockType = null);
        bool HasZeroPrice(string idUser, string identifier, string password, DateTime deployDate);
        void Disable(long idPortfolio);
    }
}
