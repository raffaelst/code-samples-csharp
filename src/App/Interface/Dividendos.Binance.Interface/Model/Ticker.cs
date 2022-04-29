using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Binance.Interface.Model
{
    public class Ticker
    {
        public decimal last { get; set; }
        public decimal open { get; set; }
        public string name { get; set; }

        public string from { get; set; }
        
        public Ticker()
        {
            from = "Binance";
        }
    }
}
