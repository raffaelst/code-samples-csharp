using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Views
{
    public class DividendCalendarView
    {

        public long DividendCalendarID { get; set; }

        public string DividendCalendarGuid { get; set; }
        public long IdStock { get; set; }
        public int IdCountry { get; set; }

        public string StockName { get; set; }

        public string Logo { get; set; }

        public int IdDividendType { get; set; }

        public string DividendType { get; set; }

        public DateTime DataCom { get; set; }
   
        public DateTime? PaymentDate { get; set; }
     

        public decimal Value { get; set; }

        public bool PaymentDatepartiallyUndefined { get; set; }

        public bool PaymentUndefined { get; set; }

        public decimal Yield { get; set; }

        public decimal MarketStockValue { get; set; }

    }
}
