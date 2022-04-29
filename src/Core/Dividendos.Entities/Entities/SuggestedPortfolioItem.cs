using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("SuggestedPortfolioItem")]
    public class SuggestedPortfolioItem
    {
        [Key]
        public long SuggestedPortfolioItemID { get; set; }
        public long SuggestedPortfolioID { get; set; }
     
        public string Name { get; set; }
    
        public string Ticker { get; set; }
      
        public string Segment { get; set; }
        public decimal Price { get; set; }
        public decimal MaxPrice { get; set; }
        public decimal FairPrice { get; set; }
      
        public string Rating { get; set; }
        public decimal Yield { get; set; }
        public decimal DiscountPrice { get; set; }

        public string SuggestedAction { get; set; }
    }
}
