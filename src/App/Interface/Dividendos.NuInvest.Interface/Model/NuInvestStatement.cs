using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.NuInvest.Interface.Model
{
    public class NuInvestStatement
    {
        [JsonProperty("value")]
        public Value Value { get; set; }

        [JsonProperty("messages")]
        public List<object> Messages { get; set; }
    }

    public partial class Value
    {
        [JsonProperty("statements")]
        public List<Statement> Statements { get; set; }
    }

    public partial class Statement
    {
        [JsonProperty("settlementDate")]
        public string SettlementDate { get; set; }

        [JsonProperty("ticker")]
        public string Ticker { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("buyQuantity")]
        public string BuyQuantity { get; set; }

        [JsonProperty("sellQuantity")]
        public string SellQuantity { get; set; }

        [JsonProperty("buyValue")]
        public string BuyValue { get; set; }

        [JsonProperty("sellValue")]
        public string SellValue { get; set; }

        [JsonProperty("negotiationNumber")]
        public string NegotiationNumber { get; set; }
    }
}
