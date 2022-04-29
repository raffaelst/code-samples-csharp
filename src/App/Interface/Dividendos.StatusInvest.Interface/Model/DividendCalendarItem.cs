using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.StatusInvest.Interface.Model
{
    public class DividendCalendarItem
    {
        public string DataCom { get; set; }
 
        public string PaymentDate { get; set; }

        public string Value { get; set; }


        public string Ticker { get; set; }

        public string Type { get; set; }
    }
}
