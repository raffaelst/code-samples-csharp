using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Toro.Interface.Model
{
    public class ToroBondApi
    {
        [JsonProperty("value")]
        public List<Bond> Bonds { get; set; }

        [JsonProperty("hasValue")]
        public string HasValue { get; set; }

        [JsonProperty("metaData")]
        public MetaDataPlan MetaData { get; set; }

        [JsonProperty("messages")]
        public object[] Messages { get; set; }

        [JsonProperty("hasWarning")]
        public string HasWarning { get; set; }

        [JsonProperty("hasAlerta")]
        public string HasAlerta { get; set; }

        [JsonProperty("hasError")]
        public string HasError { get; set; }

        [JsonProperty("hasInfo")]
        public string HasInfo { get; set; }

        [JsonProperty("ok")]
        public string Ok { get; set; }

    }
    public partial class MetaDataPlan
    {
        [JsonProperty("nextPageLink")]
        public string NextPageLink { get; set; }

        [JsonProperty("totalCount")]
        public string TotalCount { get; set; }
    }

    public partial class Bond
    {
        [JsonProperty("bondAssetID")]
        public string BondAssetId { get; set; }

        [JsonProperty("bondDescription")]
        public string BondDescription { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("issuerID")]
        public string IssuerId { get; set; }

        [JsonProperty("issuerName")]
        public string IssuerName { get; set; }

        [JsonProperty("userID")]
        public long UserId { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("cblcId")]
        public string CblcId { get; set; }

        [JsonProperty("initialValue")]
        public string InitialValue { get; set; }

        [JsonProperty("openingValue")]
        public string OpeningValue { get; set; }

        [JsonProperty("openingQuantity")]
        public string OpeningQuantity { get; set; }

        [JsonProperty("blockedValue")]
        public string BlockedValue { get; set; }

        [JsonProperty("blockedQuantity")]
        public string BlockedQuantity { get; set; }

        [JsonProperty("currentValue")]
        public string CurrentValue { get; set; }

        [JsonProperty("currentQuantity")]
        public string CurrentQuantity { get; set; }

        [JsonProperty("pendingAcquireValue")]
        public string PendingAcquireValue { get; set; }

        [JsonProperty("pendingAcquireQuantity")]
        public string PendingAcquireQuantity { get; set; }

        [JsonProperty("pendingRedeemValue")]
        public string PendingRedeemValue { get; set; }

        [JsonProperty("pendingRedeemQuantity")]
        public string PendingRedeemQuantity { get; set; }

        [JsonProperty("scheduleAcquireValue")]
        public string ScheduleAcquireValue { get; set; }

        [JsonProperty("scheduleRedeemQuantity")]
        public string ScheduleRedeemQuantity { get; set; }

        [JsonProperty("scheduleRedeemValue")]
        public string ScheduleRedeemValue { get; set; }

        [JsonProperty("scheduleAcquireQuantity")]
        public string ScheduleAcquireQuantity { get; set; }

        [JsonProperty("unitPrice")]
        public string UnitPrice { get; set; }

        [JsonProperty("incomeTaxValue")]
        public string IncomeTaxValue { get; set; }

        [JsonProperty("iofValue")]
        public string IofValue { get; set; }

        [JsonProperty("virtualOrderId")]
        public string VirtualOrderId { get; set; }

        [JsonProperty("virtualPositionId")]
        public string VirtualPositionId { get; set; }

        [JsonProperty("maturityDate")]
        public string MaturityDate { get; set; }

        [JsonProperty("operationDate")]
        public string OperationDate { get; set; }

        [JsonProperty("anticipate")]
        public string Anticipate { get; set; }

        [JsonProperty("anticipateUnitPrice")]
        public string AnticipateUnitPrice { get; set; }

        [JsonProperty("liquidityDate")]
        public string LiquidityDate { get; set; }

        [JsonProperty("rateReturn")]
        public string RateReturn { get; set; }

        [JsonProperty("profitability")]
        public string Profitability { get; set; }

        [JsonProperty("indexer")]
        public string Indexer { get; set; }

        [JsonProperty("bondOrderID")]
        public string BondOrderId { get; set; }

        [JsonProperty("unitPriceAcquire")]
        public string UnitPriceAcquire { get; set; }
    }
}
