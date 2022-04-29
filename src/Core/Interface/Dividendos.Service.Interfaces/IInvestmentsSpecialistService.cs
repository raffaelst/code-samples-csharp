using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Service.Interface
{
    public interface IInvestmentsSpecialistService : IBaseService
    {
        ResultServiceObject<IEnumerable<InvestmentsSpecialist>> GetAllInvestmentsSpecialist();

        ResultServiceObject<IEnumerable<SuggestedPortfolio>> GetSuggestedPortfolio(string specialistGuid);

        ResultServiceObject<IEnumerable<SuggestedPortfolioItemView>> GetSuggestedPortfolioItems(string suggestedPortfolio);

        ResultServiceBase DeleteInvestmentsSpecialist(Guid investmentsSpecialistGuid, ICacheService cacheService);

        ResultServiceBase DeleteSuggestedPortfolioWithItens(Guid suggestedPortfolioGuid, ICacheService cacheService);

        ResultServiceBase DeleteSuggestedPortfolioItems(Guid suggestedPortfolioGuid, ICacheService cacheService);
    }
}
