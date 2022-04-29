using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Xp.Interface.Model
{
    public partial class XpPosition
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("yieldData")]
        public object YieldData { get; set; }

        [JsonProperty("position")]
        public Position Position { get; set; }

        [JsonProperty("description")]
        public object Description { get; set; }

        [JsonProperty("plan")]
        public object Plan { get; set; }

        [JsonProperty("compose")]
        public object Compose { get; set; }
    }

    public partial class Position
    {
        [JsonProperty("positionValue")]
        public PositionValue PositionValue { get; set; }

        [JsonProperty("enabledInApplication")]
        public string EnabledInApplication { get; set; }

        [JsonProperty("graphTypes")]
        public List<object> GraphTypes { get; set; }

        [JsonProperty("graph")]
        public List<object> Graph { get; set; }
    }

    public partial class PositionValue
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("openningUnitPrice")]
        public string OpenningUnitPrice { get; set; }

        [JsonProperty("unitPrice")]
        public string UnitPrice { get; set; }

        [JsonProperty("structuredQuantity")]
        public string StructuredQuantity { get; set; }

        [JsonProperty("intradayQuantity")]
        public string IntradayQuantity { get; set; }

        [JsonProperty("projectedQuantity")]
        public string ProjectedQuantity { get; set; }

        [JsonProperty("paidCustodyQuantity")]
        public string PaidCustodyQuantity { get; set; }

        [JsonProperty("termSettlementQuantity")]
        public string TermSettlementQuantity { get; set; }

        [JsonProperty("blocked")]
        public string Blocked { get; set; }

        [JsonProperty("totalBlocked")]
        public string TotalBlocked { get; set; }

        [JsonProperty("averageCost")]
        public string AverageCost { get; set; }

        [JsonProperty("averageCostStatus")]
        public string AverageCostStatus { get; set; }

        [JsonProperty("averageCostClient")]
        public string AverageCostClient { get; set; }

        [JsonProperty("rentability")]
        public string Rentability { get; set; }

        [JsonProperty("warrantyQuantity")]
        public string WarrantyQuantity { get; set; }

        [JsonProperty("totalQuantity")]
        public string TotalQuantity { get; set; }

        [JsonProperty("availableQuantity")]
        public string AvailableQuantity { get; set; }
    }
}
