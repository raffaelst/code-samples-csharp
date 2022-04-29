using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Entity.Views
{
    public class CryptoStatementView
    {
        public string Logo { get; set; }
        public string LogoUrl { get; set; }
        public long IdProductUser { get; set; }
        public string Company { get; set; }
        public string Symbol { get; set; }

        public string Description { get; set; }
        public Guid CryptoCurrencyGuid { get; set; }
        public string Segment { get; set; }
        public decimal TotalMarket { get; set; }
        public decimal Total { get; set; }
        public decimal NetValue { get; set; }
        public decimal PerformancePerc { get; set; }
        public decimal NumberOfShares { get; set; }
        public decimal MarketPrice { get; set; }

        public decimal? AveragePrice { get; set; }

        public string FinancialInstitutionName { get; set; }

        public decimal PercentChange1h { get; set; }
        public decimal PercentChange24h { get; set; }
        public decimal PercentChange7d { get; set; }
        public decimal PercentChange30d { get; set; }
        public decimal PercentChange60d { get; set; }
        public decimal PercentChange90d { get; set; }

    }
}
