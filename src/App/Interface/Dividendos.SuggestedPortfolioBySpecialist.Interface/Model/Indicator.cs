using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.SuggestedPortfolioBySpecialist.Interface.Model
{
    public class Indicator
    {
        public int IndicatorType { get; set; }
        public string Percentage { get; set; }
        public string Points { get; set; }
        public string TradeTime { get; set; }
    }
}
