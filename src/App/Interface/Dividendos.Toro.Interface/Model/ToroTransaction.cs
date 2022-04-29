using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Toro.Interface.Model
{
    public class ToroTransaction
    {
        [JsonProperty("value")]
        public List<ToroTransactionItem> ToroTransactionItems { get; set; }
    }

    public partial class ToroTransactionItem
    {
        [JsonProperty("avaragePriceExecuted")]
        public string AvaragePriceExecuted { get; set; }

        [JsonProperty("clOrderID")]
        public string ClOrderId { get; set; }

        [JsonProperty("creationDate")]
        public string CreationDate { get; set; }

        [JsonProperty("cumQty")]
        public string CumQty { get; set; }

        [JsonProperty("enteringTrade")]
        public string EnteringTrade { get; set; }

        [JsonProperty("executionDate")]
        public string ExecutionDate { get; set; }

        [JsonProperty("expirationDate")]
        public string ExpirationDate { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("investorID")]
        public string InvestorId { get; set; }

        [JsonProperty("isDayTrade")]
        public string IsDayTrade { get; set; }

        [JsonProperty("operation")]
        public string Operation { get; set; }

        [JsonProperty("operationDescription")]
        public string OperationDescription { get; set; }

        [JsonProperty("operationOpenValue")]
        public string OperationOpenValue { get; set; }

        [JsonProperty("originType")]
        public string OriginType { get; set; }

        [JsonProperty("originDescription")]
        public string OriginDescription { get; set; }

        [JsonProperty("paramGain")]
        public string ParamGain { get; set; }

        [JsonProperty("paramLoss")]
        public string ParamLoss { get; set; }

        [JsonProperty("putOrCall")]
        public string PutOrCall { get; set; }

        [JsonProperty("putOrCallDescription")]
        public string PutOrCallDescription { get; set; }

        [JsonProperty("rejectionType")]
        public string RejectionType { get; set; }

        [JsonProperty("rejectionTypeDescription")]
        public string RejectionTypeDescription { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("statusDescription")]
        public string StatusDescription { get; set; }

        [JsonProperty("strategyID")]
        public string StrategyId { get; set; }

        [JsonProperty("totalExecutedValue")]
        public string TotalExecutedValue { get; set; }

        [JsonProperty("trades")]
        public string Trades { get; set; }

        [JsonProperty("transactionTime")]
        public string TransactionTime { get; set; }

        [JsonProperty("leavesQty")]
        public string LeavesQty { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("priceTrigger")]
        public string PriceTrigger { get; set; }

        [JsonProperty("orderQty")]
        public string OrderQty { get; set; }

        [JsonProperty("internalOrderType")]
        public string InternalOrderType { get; set; }

        [JsonProperty("internalOrderTypeDescription")]
        public string InternalOrderTypeDescription { get; set; }

        [JsonProperty("priceType")]
        public string PriceType { get; set; }

        [JsonProperty("priceTypeDescription")]
        public string PriceTypeDescription { get; set; }

        [JsonProperty("side")]
        public string Side { get; set; }

        [JsonProperty("sideDescription")]
        public string SideDescription { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("isExtern")]
        public string IsExtern { get; set; }

        [JsonProperty("isStrategy")]
        public string IsStrategy { get; set; }

        [JsonProperty("validityType")]
        public string ValidityType { get; set; }

        [JsonProperty("validityTypeDescription")]
        public string ValidityTypeDescription { get; set; }

        [JsonProperty("orderMarketType")]
        public long OrderMarketType { get; set; }

        [JsonProperty("orderMarketDescription")]
        public string OrderMarketDescription { get; set; }

        [JsonProperty("market")]
        public string Market { get; set; }

        [JsonProperty("marketDescription")]
        public string MarketDescription { get; set; }

        [JsonProperty("updatedAt")]
        public string UpdatedAt { get; set; }

        public DateTime EventDate { get; set; }
    }
}
