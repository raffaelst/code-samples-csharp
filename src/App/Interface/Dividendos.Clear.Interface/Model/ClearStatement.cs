using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Dividendos.Clear.Interface.Model
{
    public partial class ClearStatement
    {
        [JsonProperty("HasMoreItems")]
        public string HasMoreItems { get; set; }

        [JsonProperty("Orders")]
        public List<Order> Orders { get; set; }

        [JsonProperty("Staff")]
        public string Staff { get; set; }
    }

    public partial class Order
    {
        [JsonProperty("PublicationKey")]
        public string PublicationKey { get; set; }

        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Account")]
        public string Account { get; set; }

        [JsonProperty("CreatedAt")]
        public string CreatedAt { get; set; }

        [JsonProperty("Symbol")]
        public string Symbol { get; set; }

        [JsonProperty("Price")]
        public string Price { get; set; }

        [JsonProperty("Quantity")]
        public string Quantity { get; set; }

        [JsonProperty("Side")]
        public string Side { get; set; }

        [JsonProperty("Module")]
        public string Module { get; set; }

        [JsonProperty("Status")]
        public string Status { get; set; }

        [JsonProperty("OpenQuantity")]
        public string OpenQuantity { get; set; }

        [JsonProperty("ExecutedQuantity")]
        public string ExecutedQuantity { get; set; }

        [JsonProperty("IsCancelable")]
        public string IsCancelable { get; set; }

        [JsonProperty("IsCanceled")]
        public string IsCanceled { get; set; }

        [JsonProperty("UpdatedAt")]
        public string UpdatedAt { get; set; }

        [JsonProperty("Progress")]
        public string Progress { get; set; }

        [JsonProperty("ExchangeOrderType")]
        public string ExchangeOrderType { get; set; }

        [JsonProperty("Action")]
        public string Action { get; set; }

        [JsonProperty("AveragePrice")]
        public string AveragePrice { get; set; }

        [JsonProperty("ExpiresAt")]
        public string ExpiresAt { get; set; }

        [JsonProperty("ActiveExchangeOrder")]
        public ActiveExchangeOrder ActiveExchangeOrder { get; set; }

        [JsonProperty("SkipProtectionOverflowCheck")]
        public string SkipProtectionOverflowCheck { get; set; }

        [JsonProperty("IsRollback")]
        public string IsRollback { get; set; }

        [JsonProperty("NeedsCanceling")]
        public string NeedsCanceling { get; set; }

        [JsonProperty("StrategyType")]
        public string StrategyType { get; set; }

        [JsonProperty("WorkflowStep")]
        public string WorkflowStep { get; set; }

        [JsonProperty("RolloverDebitOrCredit")]
        public string RolloverDebitOrCredit { get; set; }

        [JsonProperty("SwingTradeOrderInfo")]
        public string SwingTradeOrderInfo { get; set; }

        [JsonProperty("FromExternalPlatform")]
        public string FromExternalPlatform { get; set; }

        [JsonProperty("FromNationalTreasure")]
        public string FromNationalTreasure { get; set; }

        [JsonProperty("Platform")]
        public string Platform { get; set; }

        [JsonProperty("TradingCode")]
        public string TradingCode { get; set; }

        [JsonProperty("SenderStaffName")]
        public string SenderStaffName { get; set; }

        [JsonProperty("CancelerStaffName")]
        public string CancelerStaffName { get; set; }
    }

    public partial class ActiveExchangeOrder
    {
        [JsonProperty("AveragePrice")]
        public string AveragePrice { get; set; }

        [JsonProperty("ClOrdID")]
        public string ClOrdId { get; set; }

        [JsonProperty("Code")]
        public string Code { get; set; }

        [JsonProperty("CreatedAt")]
        public string CreatedAt { get; set; }

        [JsonProperty("CumQty")]
        public string CumQty { get; set; }

        [JsonProperty("ExchangeAssignedOrderID")]
        public string ExchangeAssignedOrderId { get; set; }

        [JsonProperty("ExecutedAmount")]
        public string ExecutedAmount { get; set; }

        [JsonProperty("HasLeftovers")]
        public string HasLeftovers { get; set; }

        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("IsBuy")]
        public string IsBuy { get; set; }

        [JsonProperty("IsCanceled")]
        public string IsCanceled { get; set; }

        [JsonProperty("IsFilled")]
        public string IsFilled { get; set; }

        [JsonProperty("IsModificable")]
        public string IsModificable { get; set; }

        [JsonProperty("IsOpen")]
        public string IsOpen { get; set; }

        [JsonProperty("IsPendingCancel")]
        public string IsPendingCancel { get; set; }

        [JsonProperty("IsPendingNew")]
        public string IsPendingNew { get; set; }

        [JsonProperty("IsPendingReplace")]
        public string IsPendingReplace { get; set; }

        [JsonProperty("IsRejected")]
        public string IsRejected { get; set; }

        [JsonProperty("IsReplaced")]
        public string IsReplaced { get; set; }

        [JsonProperty("IsSell")]
        public string IsSell { get; set; }

        [JsonProperty("IsWaiting")]
        public string IsWaiting { get; set; }

        [JsonProperty("LeavesQty")]
        public string LeavesQty { get; set; }

        [JsonProperty("Leftovers")]
        public string Leftovers { get; set; }

        [JsonProperty("MarketSegment")]
        public string MarketSegment { get; set; }

        [JsonProperty("MustNotPublishEvents")]
        public string MustNotPublishEvents { get; set; }

        [JsonProperty("OwnerAccountId")]
        public string OwnerAccountId { get; set; }

        [JsonProperty("PreviousStatus")]
        public string PreviousStatus { get; set; }

        [JsonProperty("Price")]
        public string Price { get; set; }

        [JsonProperty("PriceDivisor")]
        public string PriceDivisor { get; set; }

        [JsonProperty("Quantity")]
        public string Quantity { get; set; }

        [JsonProperty("ReplaceWasRejected")]
        public string ReplaceWasRejected { get; set; }

        [JsonProperty("SecondaryOrderID")]
        public string SecondaryOrderId { get; set; }

        [JsonProperty("Side")]
        public string Side { get; set; }

        [JsonProperty("Status")]
        public string Status { get; set; }

        [JsonProperty("Symbol")]
        public string Symbol { get; set; }

        [JsonProperty("TimeInForce")]
        public string TimeInForce { get; set; }

        [JsonProperty("TransactTime")]
        public string TransactTime { get; set; }

        [JsonProperty("Type")]
        public string Type { get; set; }
    }

}
