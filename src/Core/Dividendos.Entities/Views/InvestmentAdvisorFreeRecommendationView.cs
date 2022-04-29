using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    public class InvestmentAdvisorFreeRecommendationView
    {
        public long InvestmentAdvisorFreeRecommendationID { get; set; }
        public string Stock { get; set; }
        public string StockLogo { get; set; }
        public decimal MarketPrice { get; set; }
        public string InvestmentAdvisorFreeRecommendationType { get; set; }
        public string Entry { get; set; }
        public string Objective1 { get; set; }
        public string Objective2 { get; set; }
        public string Stop { get; set; }
    }
}
