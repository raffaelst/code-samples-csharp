using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("CryptoCurrencyPerformance")]
    public class CryptoCurrencyPerformance
    {
        [Key]
        public long IdCryptoCurrencyPerformance { get; set; }
        public long IdCryptoPortfolioPerformance { get; set; }
        public long IdCryptoPortfolio { get; set; }
        public long IdCryptoCurrency { get; set; }
        public decimal Quantity { get; set; }
        public decimal AveragePrice { get; set; }
        public DateTime CalculationDate { get; set; }
        public decimal Total { get; set; }
        public decimal TotalMarket { get; set; }
        public decimal PerformancePerc { get; set; }
        public decimal NetValue { get; set; }
        public decimal PerformancePercTWR { get; set; }
        public decimal NetValueTWR { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public Guid GuidCryptoCurrencyPerformance { get; set; }
    }
}
