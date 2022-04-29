using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Entity.Views
{
    public class IndicatorSeriesView
    {
        public long IdIndicatorSeries { get; set; }
        public int IdIndicatorType { get; set; }
        public int IdPeriodType { get; set; }
        public DateTime CalculationDate { get; set; }
        public decimal Price { get; set; }
        public decimal Points { get; set; }
        public decimal Perc { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public Guid GuidIndicatorSeries { get; set; }
        public string Name { get; set; }
    }
}
