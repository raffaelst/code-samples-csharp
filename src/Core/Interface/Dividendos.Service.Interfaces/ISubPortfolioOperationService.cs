using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Service.Interface
{
    public interface ISubPortfolioOperationService : IBaseService
    {
        ResultServiceObject<IEnumerable<SubPortfolioOperation>> GetBySubPortfolio(long idSubPortfolio);
        ResultServiceObject<SubPortfolioOperation> Update(SubPortfolioOperation subPortfolioOperation);
        ResultServiceObject<SubPortfolioOperation> Insert(SubPortfolioOperation subPortfolioOperation);
        ResultServiceBase Delete(SubPortfolioOperation subPortfolioOperation);
        ResultServiceObject<SubPortfolioOperation> GetBySubPortfolioAndIdOperation(long idSubPortfolio, long idOperation);
    }
}
