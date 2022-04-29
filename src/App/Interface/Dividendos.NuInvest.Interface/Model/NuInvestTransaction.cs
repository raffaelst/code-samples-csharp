using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.NuInvest.Interface.Model
{
    public class NuInvestTransaction
    {
        [JsonProperty("accountNumber")]
        public string AccountNumber { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("lastTime")]
        public string LastTime { get; set; }

        [JsonProperty("session")]
        public string Session { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("typeOnTrigger")]
        public string TypeOnTrigger { get; set; }

        [JsonProperty("side")]
        public string Side { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("symbolId")]
        public string SymbolId { get; set; }

        [JsonProperty("clOrderId")]
        public string ClOrderId { get; set; }

        [JsonProperty("origClOrderId")]
        public string OrigClOrderId { get; set; }

        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        [JsonProperty("executionType")]
        public string ExecutionType { get; set; }

        [JsonProperty("validity")]
        public string Validity { get; set; }

        [JsonProperty("expire")]
        public string Expire { get; set; }

        [JsonProperty("quantity")]
        public string Quantity { get; set; }

        [JsonProperty("negotiatedQuantity")]
        public string NegotiatedQuantity { get; set; }

        [JsonProperty("cancelledQuantity")]
        public string CancelledQuantity { get; set; }

        [JsonProperty("accumulatedQuantity")]
        public string AccumulatedQuantity { get; set; }

        [JsonProperty("minimumQuantity")]
        public string MinimumQuantity { get; set; }

        [JsonProperty("floorQuantity")]
        public string FloorQuantity { get; set; }

        [JsonProperty("openQuantity")]
        public string OpenQuantity { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("negotiatedPrice")]
        public string NegotiatedPrice { get; set; }

        [JsonProperty("stopLossPrice")]
        public string StopLossPrice { get; set; }

        [JsonProperty("stopLossFirePrice")]
        public string StopLossFirePrice { get; set; }

        [JsonProperty("stopGainPrice")]
        public string StopGainPrice { get; set; }

        [JsonProperty("stopGainFirePrice")]
        public string StopGainFirePrice { get; set; }

        [JsonProperty("startPrice")]
        public string StartPrice { get; set; }

        [JsonProperty("startFirePrice")]
        public string StartFirePrice { get; set; }

        [JsonProperty("stopMovelStartPrice")]
        public string StopMovelStartPrice { get; set; }

        [JsonProperty("stopMovelAdjustPrice")]
        public string StopMovelAdjustPrice { get; set; }

        [JsonProperty("stopMovelStarted")]
        public string StopMovelStarted { get; set; }

        [JsonProperty("stopActived")]
        public string StopActived { get; set; }

        [JsonProperty("rejectedText")]
        public string RejectedText { get; set; }

        [JsonProperty("negotiatedFlow")]
        public string NegotiatedFlow { get; set; }

        [JsonProperty("canCustomerCancel")]
        public string CanCustomerCancel { get; set; }

        [JsonProperty("stopOrderTrigged")]
        public string StopOrderTrigged { get; set; }

        [JsonProperty("originIp")]
        public string OriginIp { get; set; }

        [JsonProperty("tradeDate")]
        public string TradeDate { get; set; }

        [JsonProperty("dayTradedQuantity")]
        public string DayTradedQuantity { get; set; }

        [JsonProperty("isOpenToTrade")]
        public string IsOpenToTrade { get; set; }
        public DateTime EventDate { get; set; }
    }
}
