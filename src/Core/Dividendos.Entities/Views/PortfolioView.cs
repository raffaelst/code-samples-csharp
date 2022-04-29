using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Entity.Views
{
    public class PortfolioView
    {
        public long IdPortfolio { get; set; }
        public Guid GuidPortfolioSubPortfolio { get; set; }
        public string Name { get; set; }
        public decimal TotalMarket { get; set; }
        public decimal Total { get; set; }
        public decimal NetValue { get; set; }
        public decimal LatestNetValue { get; set; }
        public decimal PreviousNetValue { get; set; }
        public decimal PerformancePerc { get; set; }
        public decimal PerformancePercTWR { get; set; }
        public decimal TotalDividend { get; set; }
        public decimal Profit { get; set; }
        public DateTime CalculationDate { get; set; }

        public DateTime LastSync { get; set; }

        public bool IsPortfolio { get; set; }

        public Guid ParentPortfolio { get; set; }
        public int IdCountry { get; set; }

        public bool HasSubscription { get; set; }

    }
}
