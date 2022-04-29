using Dividendos.API.Model.Request;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using Dividendos.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Application.Interface
{
    public interface ISubPortfolioApp
    {
        ResultResponseBase Disable(Guid idPortfolio, Guid idSubPortfolio);

        ResultResponseObject<IEnumerable<SubPortfolioVM>> GetByPortfolio(Guid idPortifolio);

        ResultResponseObject<SubPortfolioVM> Add(Guid idPortfolio, SubPortfolioVM subPortfolioVM);

        ResultResponseObject<SubPortfolioVM> Update(Guid idSubPortfolio, SubPortfolioVM subPortfolioVM);
    }
}
