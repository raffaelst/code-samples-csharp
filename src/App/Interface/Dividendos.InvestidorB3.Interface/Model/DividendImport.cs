using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.InvestidorB3.Interface.Model
{
    public class DividendImport
    {
        public string Broker { get; set; }
        public string Symbol { get; set; }
        public string DividendType { get; set; }
        public string PaymentDt { get; set; }
        public string NetVal { get; set; }
        public string BaseQtty { get; set; }
        public string GrossVal { get; set; }

        public DateTime? PaymentDate { get; set; }
        public decimal NetValue { get; set; }
        public int BaseQuantity { get; set; }
        public decimal GrossValue { get; set; }
    }
}
