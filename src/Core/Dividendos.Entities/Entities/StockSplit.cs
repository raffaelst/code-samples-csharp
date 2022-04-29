using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("StockSplit")]
    public class StockSplit
    {
        [Key]
        public long StockSplitID { get; set; }

        public DateTime DateSplit { get; set; }

        public long StockID { get; set; }

        public decimal ProportionFrom { get; set; }

        public decimal ProportionTo { get; set; }

        public bool Unfolded { get; set; }

        public int IdCountry { get; set; }
    }
}
