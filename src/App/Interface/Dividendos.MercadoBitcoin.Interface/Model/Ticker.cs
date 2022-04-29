using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.MercadoBitcoin.Interface.Model
{
    public class Ticker
    {
        public string high { get; set; }
        public string low { get; set; }
        public string vol { get; set; }
        public decimal last { get; set; }
        public string buy { get; set; }
        public string sell { get; set; }
        public decimal open { get; set; }
        public int date { get; set; }

        public string name { get; set; }

        public string from { get; set; }

        public Ticker()
        {
            from = "MBitcoin";
        }

    }
}
