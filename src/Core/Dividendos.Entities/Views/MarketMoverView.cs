using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
  
    public class MarketMoverView
    {

        public long MarketMoverID { get; set; }

        public int MarketMoverTypeID { get; set; }

        public long StockID { get; set; }


        public decimal MarketPrice { get; set; }

        public decimal Value { get; set; }

        public string Logo { get; set; }


        public string Stock { get; set; }

        public DateTime? LastDailyVariationNotification { get; set; }

    }
}
