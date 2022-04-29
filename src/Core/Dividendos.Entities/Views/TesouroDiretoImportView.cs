using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Dividendos.Entity.Views
{
    public class TesouroDiretoImportView
    {
        [JsonProperty("broker")]
        public string Broker { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("period")]
        public string Period { get; set; }

        [JsonProperty("market")]
        public string Market { get; set; }

        [JsonProperty("grossValue")]
        public string GrossVal { get; set; }

        [JsonProperty("netValue")]
        public string NetVal { get; set; }

        [JsonProperty("baseQuantity")]
        public string BaseQtty { get; set; }

        [JsonProperty("stockSpec")]
        public string StockSpec { get; set; }


        public DateTime PeriodValue { get; set; }
        public decimal NetValue { get; set; }
        public decimal MarketValue { get; set; }
        public int BaseQuantity { get; set; }
        public decimal GrossValue { get; set; }
    }
}
