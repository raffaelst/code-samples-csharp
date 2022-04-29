using Dividendos.Entity.Entities;
using Dividendos.Repository.Interface.GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Repository.Interface
{
    public interface IOperationItemHistRepository : IRepository<OperationItemHist>
    {
        IEnumerable<OperationItemHist> GetActiveByPortfolio(long idPortfolio);
        IEnumerable<OperationItemHist> GetAllByPortfolio(long idPortfolio);

        void DeleteAllByPortfolio(long idPortfolio);
    }
}
