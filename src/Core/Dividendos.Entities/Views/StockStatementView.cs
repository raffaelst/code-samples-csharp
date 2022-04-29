using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Entity.Views
{
    public class StockStatementView
    {
        public long IdStock { get; set; }
        public Guid GuidOperation { get; set; }
        public string Company { get; set; }
        public string Symbol { get; set; }
        public string Logo { get; set; }
        public string Segment { get; set; }
        public decimal TotalMarket { get; set; }
        public decimal Total { get; set; }
        public decimal NetValue { get; set; }
        public decimal PerformancePerc { get; set; }
        public decimal NumberOfShares { get; set; }
        public decimal MarketPrice { get; set; }
        public decimal AveragePrice { get; set; }
        public decimal TotalDividends { get; set; }
        public bool ShowOnPortolio { get; set; }
    }
}
