using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Entity.Views
{
    public class StockAllocation
    {
        public string Company { get; set; }
        public string Symbol { get; set; }
        public decimal TotalMarket { get; set; }
        public decimal PerformancePerc { get; set; }
    }
}
