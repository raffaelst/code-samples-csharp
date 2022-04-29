using Dividendos.API.Model.Request.Settings;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using Dividendos.Entity.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Application.Interface
{
    public interface IInvestmentsSpecialistApp
    {
        ResultResponseObject<IEnumerable<InvestmentsSpecialistVM>> GetAllInvestmentsSpecialist();
        ResultResponseObject<IEnumerable<SuggestedPortfolioVM>> GetSuggestedPortfolioBySpecialist(string specialistGuid);

        ResultResponseObject<IEnumerable<SuggestedPortfolioItemVM>> GetSuggestedPortfolioItems(string suggestedPortfolioGuid);

        ResultResponseBase DeleteInvestmentsSpecialist(Guid investmentsSpecialistGuid);

        ResultResponseBase DeleteSuggestedPortfolio(Guid suggestedPortfolioGuid);
    }
}
