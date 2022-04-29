using Newtonsoft.Json;
using System;

namespace Dividendos.B3.Interface.Model
{

    public class DividendImport
    {
        [JsonProperty("broker")]
        public string Broker { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("dividendType")]
        public string DividendType { get; set; }

        [JsonProperty("paymentDate")]
        public string PaymentDt { get; set; }

        [JsonProperty("netValue")]
        public string NetVal { get; set; }

        [JsonProperty("baseQuantity")]
        public string BaseQtty { get; set; }

        [JsonProperty("grossValue")]
        public string GrossVal { get; set; }

        public DateTime? PaymentDate { get; set; }
        public decimal NetValue { get; set; }
        public int BaseQuantity { get; set; }
        public decimal GrossValue { get; set; }
    }
}
