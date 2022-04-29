using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.B3.Interface.Model
{
    public class GenericImport
    {
        [JsonProperty("broker")]
        public string Broker { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("period")]
        public string Period { get; set; }

        [JsonProperty("market")]
        public string Market { get; set; }

        [JsonProperty("expire")]
        public string Expire { get; set; }

        [JsonProperty("stockSpec")]
        public string StockSpec { get; set; }

        [JsonProperty("factor")]
        public string Factor { get; set; }

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

        [JsonProperty("numberOfShares")]
        public string NumShares { get; set; }

        [JsonProperty("averagePrice")]
        public string AvgPrice { get; set; }

        [JsonProperty("importType")]
        public int ImportType { get; set; }

        [JsonProperty("operationType")]
        public string OperationType { get; set; }

        [JsonProperty("gridType")]
        public string GridType { get; set; }

        //public DateTime? PaymentDate { get; set; }
        //public decimal NetValue { get; set; }
        //public int BaseQuantity { get; set; }
        //public decimal GrossValue { get; set; }
        //public long NumberOfShares { get; set; }
        //public decimal AveragePrice { get; set; }
    }
}
