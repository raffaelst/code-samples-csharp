using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Xp.Interface.Model
{
    public partial class XpInvestment
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("yieldData")]
        public YieldData YieldData { get; set; }

        [JsonProperty("position")]
        public Position2 Position { get; set; }

        [JsonProperty("description")]
        public Description Description { get; set; }

        [JsonProperty("plan")]
        public string Plan { get; set; }

        [JsonProperty("compose")]
        public string Compose { get; set; }

        public string Name { get; set; }
    }

    public partial class Description
    {
        [JsonProperty("assetType")]
        public string AssetType { get; set; }

        [JsonProperty("descriptionValue")]
        public DescriptionValue DescriptionValue { get; set; }

        //[JsonProperty("graph")]
        //public List<string> Graph { get; set; }

        [JsonProperty("indexer")]
        public string Indexer { get; set; }
    }

    public partial class DescriptionValue
    {
        [JsonProperty("issuerName")]
        public string IssuerName { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("issueDate")]
        public string IssueDate { get; set; }

        [JsonProperty("unitPrice")]
        public string UnitPrice { get; set; }

        [JsonProperty("unitPriceDate")]
        public string UnitPriceDate { get; set; }

        [JsonProperty("expirationDate")]
        public string ExpirationDate { get; set; }

        [JsonProperty("issueUnitPrice")]
        public string IssueUnitPrice { get; set; }

        //[JsonProperty("rating")]
        //public List<string> Rating { get; set; }

        [JsonProperty("remuneration")]
        public string Remuneration { get; set; }

        [JsonProperty("liquidityDate")]
        public string LiquidityDate { get; set; }

        [JsonProperty("interest")]
        public string Interest { get; set; }

        [JsonProperty("amortization")]
        public string Amortization { get; set; }

        [JsonProperty("taxMethodDescription")]
        public string TaxMethodDescription { get; set; }
    }

    public partial class YieldData
    {
        [JsonProperty("originalValue")]
        public string OriginalValue { get; set; }

        [JsonProperty("pendingValue")]
        public string PendingValue { get; set; }

        [JsonProperty("grossClosingValue")]
        public string GrossClosingValue { get; set; }

        [JsonProperty("netClosingValue")]
        public string NetClosingValue { get; set; }

        [JsonProperty("closingQuantity")]
        public string ClosingQuantity { get; set; }

        [JsonProperty("warrantyQuantity")]
        public string WarrantyQuantity { get; set; }

        [JsonProperty("netProfitAndLoss")]
        public string NetProfitAndLoss { get; set; }

        [JsonProperty("grossProfitAndLoss")]
        public string GrossProfitAndLoss { get; set; }

        [JsonProperty("yield")]
        public string Yield { get; set; }

        [JsonProperty("incomeTax")]
        public string IncomeTax { get; set; }

        [JsonProperty("iof")]
        public string Iof { get; set; }

        [JsonProperty("benchmark")]
        public string Benchmark { get; set; }

        [JsonProperty("daysBehind")]
        public string DaysBehind { get; set; }

        //[JsonProperty("graphTypes")]
        //public List<string> GraphTypes { get; set; }
    }

    public partial class Position2
    {
        [JsonProperty("positionValue")]
        public PositionValue2 PositionValue2 { get; set; }

        [JsonProperty("enabledInApplication")]
        public string EnabledInApplication2 { get; set; }

        //[JsonProperty("graphTypes")]
        //public List<object> GraphTypes { get; set; }

        //[JsonProperty("graph")]
        //public List<object> Graph { get; set; }
    }

    public partial class PositionValue2
    {
        //[JsonProperty("productType")]
        //public string ProductType { get; set; }

        //[JsonProperty("expirationDate")]
        //public DateTimeOffset ExpirationDate { get; set; }

        //[JsonProperty("totalQuantity")]
        //public long TotalQuantity { get; set; }

        //[JsonProperty("availableQuantity")]
        //public long AvailableQuantity { get; set; }

        //[JsonProperty("warrantyQuantity")]
        //public long WarrantyQuantity { get; set; }

        [JsonProperty("position")]
        public string Position { get; set; }

        //[JsonProperty("price")]
        //public double Price { get; set; }
    }
}
