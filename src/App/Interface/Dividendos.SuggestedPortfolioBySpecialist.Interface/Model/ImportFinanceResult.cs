using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.SuggestedPortfolioBySpecialist.Interface.Model
{
    public class ImportFinanceResult
    {
        public List<Stock> ListStock { get; set; }
        public List<Indicator> ListIndicator { get; set; }
    }
}
