using Dividendos.Entity.Entities;
using Dividendos.Repository.Interface.GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Repository.Interface
{
    public interface ISuggestedPortfolioRepository : IRepository<SuggestedPortfolio>
    {
        IEnumerable<SuggestedPortfolio> GetSuggestedPortfolio(string specialistGuid);
    }
}
