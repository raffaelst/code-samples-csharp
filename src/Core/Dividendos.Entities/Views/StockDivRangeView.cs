using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Entity.Views
{
    public class StockDivRangeView
    {
        public long IdStock { get; set; }
        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }
    }
}
