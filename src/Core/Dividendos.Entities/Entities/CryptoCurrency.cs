using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("CryptoCurrency")]
    public class CryptoCurrency
    {
        [Key]
        public long CryptoCurrencyID { get; set; }
        public string Name { get; set; }
        public decimal MarketPrice { get; set; }
        public decimal Variation { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Guid CryptoCurrencyGuid { get; set; }

        public long LogoID { get; set; }

        public decimal Volume24h { get; set; }
        public decimal PercentChange1h { get; set; }
        public decimal PercentChange24h { get; set; }
        public decimal PercentChange7d { get; set; }
        public decimal PercentChange30d { get; set; }
        public decimal PercentChange60d { get; set; }
        public decimal PercentChange90d { get; set; }
        public decimal MarketPriceUSD { get; set; }
        public decimal MarketPriceEuro { get; set; }
    }
}
