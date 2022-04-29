using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("MarketMover")]
    public class MarketMover
    {
        [Key]
        public long MarketMoverID { get; set; }

        public int MarketMoverTypeID { get; set; }

        public long StockID { get; set; }



        public decimal MarketPrice { get; set; }

        public decimal Value { get; set; }
    }
}
