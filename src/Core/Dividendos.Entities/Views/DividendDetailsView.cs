using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Entity.Views
{
    public class DividendDetailsView
    {
        public long IdDividend { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string DividendType { get; set; }
        public int IdDividendType { get; set; }
        public decimal NetValue { get; set; }
        public long IdCompany { get; set; }
        public long IdPortfolio { get; set; }
        public string Company { get; set; }
        public long IdStock { get; set; }
        public string Symbol { get; set; }
        public string Logo { get; set; }
        public string Segment { get; set; }
    }
}
