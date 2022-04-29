using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("Stock")]
    public class Stock
    {
        [Key]
        public long IdStock { get; set; }
        public long IdCompany { get; set; }

        public string Symbol { get; set; }

        [Computed]
        public string Logo { get; set; }

        public int IdStockType { get; set; }
        public Guid GuidStock { get; set; }

        public decimal MarketPrice { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime TradeTime { get; set; }
        public bool ShowOnPortolio { get; set; }
        public int IdCountry { get; set; }
        public decimal LastChangePerc { get; set; }
        public string OldSymbols { get; set; }
        public DateTime? LastDividendUpdateSync { get; set; }
        public DateTime? LastDailyVariationNotification { get; set; }
    }
}
