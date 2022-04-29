using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Service.Interface
{
    public interface ISubPortfolioService : IBaseService
    {
        ResultServiceObject<IEnumerable<SubPortfolio>> GetByPortfolio(long idPortfolio);
        ResultServiceObject<SubPortfolio> GetByGuid(Guid guidSubPortfolio);
        ResultServiceBase Disable(SubPortfolio subPortfolio);
        ResultServiceObject<SubPortfolio> Add(SubPortfolio subPortfolio);

        ResultServiceObject<SubPortfolio> Update(SubPortfolio subPortfolio);
    }
}
