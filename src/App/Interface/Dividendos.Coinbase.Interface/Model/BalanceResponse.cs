using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Coinbase.Interface.Model
{
    public class BalanceResponse
    {
        public string asset { get; set; }
        public decimal free { get; set; }
        public decimal locked { get; set; }
    }
}
