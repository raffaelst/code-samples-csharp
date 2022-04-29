using Dividendos.Entity.Entities;
using Dividendos.Repository.Interface.GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Repository.Interface
{
    public interface IOperationRepository : IRepository<Operation>
    {
        IEnumerable<Operation> GetByIdOperationItem(long idOperationItem);
        bool IsAdjusted(long idOperation);

        void DeleteAllByPortfolio(long idPortfolio);
        bool HasRecentSubscription(long idCompany, long idPortfolio);
        DateTime? GetLatestEventDate(long idPortfolio, long idStock);
        decimal GetTotalAmount(long idPortfolio, DateTime limitDate, long idStock);
        IEnumerable<Operation> GetOperationSplits(string idUser, DateTime limitDate);
        void Update(long idOperation, bool active);
    }
}
