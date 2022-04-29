using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("InvestmentAdvisorFreeRecommendation")]
    public class InvestmentAdvisorFreeRecommendation
    {
        [Key]
 
        public long InvestmentAdvisorFreeRecommendationID { get; set; }

        public long StockID { get; set; }
        public int InvestmentAdvisorFreeRecommendationTypeID { get; set; }
        public string Entry { get; set; }
        public string Objective1 { get; set; }
        public string Objective2 { get; set; }
        public string Stop { get; set; }
        public bool Active { get; set; }
    }
}
