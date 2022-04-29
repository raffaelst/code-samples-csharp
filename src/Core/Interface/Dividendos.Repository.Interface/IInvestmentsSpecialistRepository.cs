using Dividendos.Entity.Entities;
using Dividendos.Repository.Interface.GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Repository.Interface
{
    public interface IInvestmentsSpecialistRepository : IRepository<InvestmentsSpecialist>
    {
        void DeleteInvestmentsSpecialist(Guid investmentsSpecialistGuid);

        void DeleteSuggestedPortfolio(Guid suggestedPortfolioGuid);
        void DeleteSuggestedPortfolioItensByPortfolio(Guid suggestedPortfolioGuid);
    }
}
