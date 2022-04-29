using Dividendos.Entity.Entities;
using Dividendos.Repository.Interface.GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Repository.Interface
{
    public interface IOperationHistRepository : IRepository<OperationHist>
    {
        IEnumerable<OperationHist> GetByPortfolio(long idPortfolio);

        void DeleteAllByPortfolio(long idPortfolio);
    }
}
