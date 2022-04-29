using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.InvestingCom.Interface.Model
{
    public class StockSplitImport
    {
        public DateTime SplitDate { get; set; }
        public string Symbol { get; set; }
        public decimal ProportionFrom { get; set; }
        public decimal ProportionTo { get; set; }
    }
}
