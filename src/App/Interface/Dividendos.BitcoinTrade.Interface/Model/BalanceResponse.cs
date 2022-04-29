using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.BitcoinTrade.Interface.Model
{
    public class Datum
    {
        public decimal available_amount { get; set; }
        public string currency_code { get; set; }
        public decimal locked_amount { get; set; }
    }

    public class Root
    {
        public object code { get; set; }
        public object message { get; set; }
        public List<Datum> data { get; set; }
    }


}
