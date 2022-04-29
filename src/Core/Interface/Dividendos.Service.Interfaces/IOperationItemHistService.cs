using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Service.Interface
{
    public interface IOperationItemHistService : IBaseService
    {
        ResultServiceObject<OperationItemHist> Update(OperationItemHist operationItemHist);
        ResultServiceObject<OperationItemHist> Insert(OperationItemHist operationItemHist);
        ResultServiceObject<IEnumerable<OperationItemHist>> GetActiveByPortfolio(long idPortfolio);
        void InactiveByOperationHist(long idOperationHist);
        ResultServiceObject<OperationItemHist> GetByGuidOperationItem(Guid guidOperationItemHist);
        ResultServiceObject<IEnumerable<OperationItemHist>> GetAllByPortfolio(long idPortfolio);

        ResultServiceObject<bool> DeleteAllByPortfolio(long idPortfolio);
    }
}
