using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Entity.Entities
{
    [Table("PerformanceStock")]
    public class PerformanceStock
    {
        [Key]
        public long IdPerformanceStock { get; set; }
        public long IdPortfolioPerformance { get; set; }
        public long IdStock { get; set; }
        public decimal NumberOfShares { get; set; }
        public decimal AveragePrice { get; set; }
        public DateTime CalculationDate { get; set; }
        public Guid GuidPerformanceStock { get; set; }
        public decimal Total { get; set; }
        public decimal TotalMarket { get; set; }
        public decimal PerformancePerc { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public decimal NetValue { get; set; }
        public decimal PerformancePercTWR { get; set; }
        public decimal? NetValueTWR { get; set; }
    }
}
