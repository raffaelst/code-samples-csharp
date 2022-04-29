using Dapper.Contrib.Extensions;
using System;


namespace Dividendos.Entity.Entities
{
    [Table("CryptoPortfolioPerformance")]
    public class CryptoPortfolioPerformance
    {
        [Key]
        public long IdCryptoPortfolioPerformance { get; set; }
        public long IdCryptoPortfolio { get; set; }
        public decimal Total { get; set; }
        public decimal TotalMarket { get; set; }
        public decimal PerformancePerc { get; set; }
        public DateTime CalculationDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public decimal NetValue { get; set; }
        public decimal PerformancePercTWR { get; set; }
        public decimal NetValueTWR { get; set; }
        public Guid GuidCryptoPortfolioPerformance { get; set; }
    }
}
