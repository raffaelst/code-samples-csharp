using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Service.Interface
{
    public interface IOperationHistService : IBaseService
    {
        ResultServiceObject<OperationHist> Update(OperationHist operationHist);
        ResultServiceObject<OperationHist> Insert(OperationHist operationHist);
        ResultServiceObject<IEnumerable<OperationHist>> GetByPortfolio(long idPortfolio);
        ResultServiceObject<OperationHist> Inactive(OperationHist operationHist);
        ResultServiceObject<OperationHist> GetByPortfolioAndIdStock(long idPortfolio, long idStock, int idOperationType);
        ResultServiceObject<OperationHist> GetById(long idOperationHist);
        ResultServiceObject<bool> DeleteAllByPortfolio(long idPortfolio);
    }
}
