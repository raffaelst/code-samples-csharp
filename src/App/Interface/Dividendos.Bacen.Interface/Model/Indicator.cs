using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Bacen.Interface.Model
{
    public class Indicator
    {
        public int IndicatorType { get; set; }
        public int PeriodType { get; set; }
        public decimal Percentage { get; set; }
        public string Points { get; set; }
        public string TradeTime { get; set; }
    }
}
