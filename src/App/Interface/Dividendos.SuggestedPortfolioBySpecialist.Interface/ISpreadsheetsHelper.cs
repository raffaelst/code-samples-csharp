using Dividendos.SuggestedPortfolioBySpecialist.Interface.Model;
using System;

namespace Dividendos.SuggestedPortfolioBySpecialist.Interface
{
    public interface ISpreadsheetsHelper
    {
        ImportFinanceResult ReadEntries();
    }
}
