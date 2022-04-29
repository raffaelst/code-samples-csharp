using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Entity.Views
{
    public class OperationSummaryView
    {
        public long IdOperation { get; set; }
        public Guid GuidOperation { get; set; }
        public string Symbol { get; set; }
        public decimal MarketPrice { get; set; }
        public decimal AveragePrice { get; set; }
        public decimal NumberOfShares { get; set; }
        public decimal PerformancePerc { get; set; }
        public decimal PerformancePercTWR { get; set; }
        public DateTime CalculationDate { get; set; }
        public int IdCountry { get; set; }
        public decimal TotalDividends { get; set; }
        public decimal LastChangePerc { get; set; }

        public decimal Dividend12Months { get; set; }
        public decimal Dividend12MonthsYield { get; set; }
        public decimal Dividend24Months { get; set; }
        public decimal Dividend24MonthsYield { get; set; }
        public decimal Dividend36Months { get; set; }
        public decimal Dividend36MonthsYield { get; set; }
        public decimal PricePerVpa { get; set; }
        
        public long IdStock { get; set; }
        public string Logo { get; set; }
    }
}
