using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.IexAPIsHelper.Interface.Model
{
    public class DividendCalendarItem
    {
        public string DividendExDate { get; set; }

        public string PaymentDate { get; set; }

        public string AnnouncementDate { get; set; }

        public string Value { get; set; }


        public string Ticker { get; set; }
    }
}
