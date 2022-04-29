using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Rico.Interface.Model
{
    public partial class RicoSummary
    {
        [JsonProperty("metadata")]
        public Metadata Metadata { get; set; }

        [JsonProperty("totalValue")]
        public string TotalValue { get; set; }

        [JsonProperty("allocation")]
        public string Allocation { get; set; }

        [JsonProperty("grossValue")]
        public string GrossValue { get; set; }

        [JsonProperty("totalInvestedValue")]
        public string TotalInvestedValue { get; set; }

        [JsonProperty("balance")]
        public Balance Balance { get; set; }

        [JsonProperty("positions")]
        public List<Position> Positions { get; set; }
    }

    public partial class Balance
    {
        [JsonProperty("availableToWithdrawal")]
        public string AvailableToWithdrawal { get; set; }

        [JsonProperty("totalValue")]
        public string TotalValue { get; set; }

        [JsonProperty("grossValue")]
        public string GrossValue { get; set; }

        [JsonProperty("availableValue")]
        public string AvailableValue { get; set; }

        [JsonProperty("projections")]
        public Projections Projections { get; set; }
    }

    public partial class Projections
    {
        [JsonProperty("d1")]
        public string D1 { get; set; }

        [JsonProperty("d2")]
        public string D2 { get; set; }

        [JsonProperty("d3")]
        public string D3 { get; set; }
    }

    public partial class Metadata
    {
        [JsonProperty("dateHour")]
        public string DateHour { get; set; }

        [JsonProperty("accountNumber")]
        public string AccountNumber { get; set; }
    }

    public partial class Position
    {
        [JsonProperty("product")]
        public string Product { get; set; }

        [JsonProperty("productName")]
        public string ProductName { get; set; }

        [JsonProperty("totalValue")]
        public string TotalValue { get; set; }

        [JsonProperty("allocation")]
        public string Allocation { get; set; }

        [JsonProperty("grossValue")]
        public string GrossValue { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("investedValue")]
        public string InvestedValue { get; set; }

        [JsonProperty("itens", NullValueHandling = NullValueHandling.Ignore)]
        public List<Iten> Itens { get; set; }
    }

    public partial class Iten
    {
        [JsonProperty("stockCode")]
        public string StockCode { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("quantity")]
        public string Quantity { get; set; }

        [JsonProperty("totalValue")]
        public string TotalValue { get; set; }

        [JsonProperty("averagePrice")]
        public string AveragePrice { get; set; }

        [JsonProperty("profitability")]
        public string Profitability { get; set; }

        [JsonProperty("averagePriceStatus")]
        public string AveragePriceStatus { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("yield")]
        public string Yield { get; set; }

        [JsonProperty("maturityDate")]
        public string MaturityDate { get; set; }

        [JsonProperty("currentGrossValue")]
        public string CurrentGrossValue { get; set; }

        [JsonProperty("rescueBlocked")]
        public string RescueBlocked { get; set; }

        [JsonProperty("orderDate")]
        public string OrderDate { get; set; }

        [JsonProperty("positionId")]
        public string PositionId { get; set; }

        [JsonProperty("investedValue")]
        public string InvestedValue { get; set; }

        [JsonProperty("grossValue")]
        public string GrossValue { get; set; }

        [JsonProperty("paymentDate")]
        public string PaymentDate { get; set; }

        [JsonProperty("paymentDateString")]
        public string PaymentDateString { get; set; }

        [JsonProperty("netValue")]
        public string NetValue { get; set; }

        [JsonProperty("incomeTaxValue")]
        public string IncomeTaxValue { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("currentTotalValue")]
        public string CurrentTotalValue { get; set; }

        [JsonProperty("currentTotalValueGross")]
        public string CurrentTotalValueGross { get; set; }

        [JsonProperty("currentTotalValueQuota")]
        public string CurrentTotalValueQuota { get; set; }
    }
}
