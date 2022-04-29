using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Finance.Interface.Model
{
    public class Stock
    {
        public string IdStock { get; set; }
        public string MarketPrice { get; set; }
        public string TradeTime { get; set; }
        public string LastChangePerc { get; set; }
    }
}
