using Dapper.Contrib.Extensions;
using System;
namespace Dividendos.Entity.Entities
{
    [Table("SuggestedPortfolio")]
    public class SuggestedPortfolio
    {
        [Key]
     
        public long SuggestedPortfolioID { get; set; }
        public long InvestmentsSpecialistID { get; set; }
   
        public string Name { get; set; }
        public DateTime ChangeDate { get; set; }
  
        public string LandingPageURL { get; set; }
        public Guid SuggestedPortfolioGuid { get; set; }

    }
}
