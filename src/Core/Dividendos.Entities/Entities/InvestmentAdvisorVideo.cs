using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("InvestmentAdvisorVideo")]
    public class InvestmentAdvisorVideo
    {
        [Key]
 
        public long InvestmentAdvisorVideoID { get; set; }

        public string URL { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid InvestmentAdvisorVideoGuid { get; set; }

        public DateTime Date { get; set; }

        public bool Active { get; set; }
    }
}
