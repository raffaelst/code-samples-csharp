using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Avenue.Interface.Model
{
    public class AvenueTransactionItem
    {
        [JsonProperty("items")]
        public Items Items { get; set; }
    }

    public partial class Items
    {
        [JsonProperty("Side")]
        public string Side { get; set; }

        [JsonProperty("Price")]
        public string Price { get; set; }

        [JsonProperty("Symbol")]
        public string Symbol { get; set; }

        [JsonProperty("ClOrdId")]
        public string ClOrdId { get; set; }

        [JsonProperty("Quantity")]
        public string Quantity { get; set; }

        [JsonProperty("Operation")]
        public string Operation { get; set; }

        [JsonProperty("Commission")]        
        public string Commission { get; set; }

        [JsonProperty("HistoryCode")]
        public string HistoryCode { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }
    }
}
