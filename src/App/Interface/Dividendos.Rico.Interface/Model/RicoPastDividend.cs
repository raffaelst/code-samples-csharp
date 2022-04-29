using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Rico.Interface.Model
{
    public partial class RicoPastDividend
    {
        [JsonProperty("items")]
        public List<Item> Items { get; set; }
    }

    public partial class Item
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("balance")]
        public string Balance { get; set; }

        [JsonProperty("settlementDate")]
        public string SettlementDate { get; set; }

        [JsonProperty("transactionDate")]
        public string TransactionDate { get; set; }
    }
}
