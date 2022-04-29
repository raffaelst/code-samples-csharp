using Newtonsoft.Json;
using System;

namespace Dividendos.Passfolio.Interface.Model
{

    public class DividendPassfolioImport
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
