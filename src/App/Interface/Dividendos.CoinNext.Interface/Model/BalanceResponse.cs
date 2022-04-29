using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.CoinNext.Interface.Model
{
    public class BalanceResponse
    {
        public string asset { get; set; }
        public decimal free { get; set; }
        public decimal locked { get; set; }
    }
}
