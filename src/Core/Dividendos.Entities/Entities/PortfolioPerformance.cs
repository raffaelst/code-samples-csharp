using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Entity.Entities
{
    [Table("PortfolioPerformance")]
    public class PortfolioPerformance
    {
        [Key]
        public long IdPortfolioPerformance { get; set; }
        public long IdPortfolio { get; set; }
        public decimal Total { get; set; }
        public decimal TotalMarket { get; set; }
        public decimal PerformancePerc { get; set; }
        public DateTime CalculationDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public Guid GuidPortfolioPerformance { get; set; }
        public decimal NetValue { get; set; }
        public decimal PerformancePercTWR { get; set; }
        public decimal? NetValueTWR { get; set; }
    }
}
