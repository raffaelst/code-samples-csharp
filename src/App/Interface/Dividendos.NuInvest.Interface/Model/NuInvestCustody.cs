using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.NuInvest.Interface.Model
{
    public partial class NuInvestCustody
    {
        [JsonProperty("easyBalance")]
        public string EasyBalance { get; set; }

        [JsonProperty("totalInvestments")]
        public string TotalInvestments { get; set; }

        [JsonProperty("totalBalance")]
        public string TotalBalance { get; set; }

        [JsonProperty("hasIpo")]
        public string HasIpo { get; set; }

        [JsonProperty("hasEquity")]
        public string HasEquity { get; set; }

        [JsonProperty("investmentsQuantity")]
        public string InvestmentsQuantity { get; set; }

        [JsonProperty("availableWithdrawMoney")]
        public string AvailableWithdrawMoney { get; set; }

        [JsonProperty("isProjectedBalance")]
        public string IsProjectedBalance { get; set; }

        [JsonProperty("isCached")]
        public string IsCached { get; set; }

        [JsonProperty("cacheUpdatedData")]
        public string CacheUpdatedData { get; set; }

        [JsonProperty("investments")]
        public List<Investment> Investments { get; set; }

        [JsonProperty("investmentPortfolio")]
        public List<InvestmentPortfolio> InvestmentPortfolio { get; set; }

        [JsonProperty("isDelayed")]
        public string IsDelayed { get; set; }

        [JsonProperty("isFirstInvestment")]
        public string IsFirstInvestment { get; set; }

        [JsonProperty("delayedMessages")]
        public string DelayedMessages { get; set; }
    }

    public partial class InvestmentPortfolio
    {
        [JsonProperty("investmentType")]
        public InvestmentType InvestmentType { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("percentagePortfolio")]
        public string PercentagePortfolio { get; set; }

        [JsonProperty("isDelayed")]
        public string IsDelayed { get; set; }
    }

    public partial class InvestmentType
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }
    }

    public partial class Investment
    {
        [JsonProperty("ir")]
        public string Ir { get; set; }

        [JsonProperty("maturity")]
        public string Maturity { get; set; }

        [JsonProperty("yield", NullValueHandling = NullValueHandling.Ignore)]
        public string Yield { get; set; }

        [JsonProperty("hasSchedulings", NullValueHandling = NullValueHandling.Ignore)]
        public string HasSchedulings { get; set; }

        [JsonProperty("isCloseToMaturityDate", NullValueHandling = NullValueHandling.Ignore)]
        public string IsCloseToMaturityDate { get; set; }

        [JsonProperty("currencyProfitAndLoss")]
        public string CurrencyProfitAndLoss { get; set; }

        [JsonProperty("percentageProfitAndLoss")]
        public string PercentageProfitAndLoss { get; set; }

        [JsonProperty("showProfitAndLoss")]
        public string ShowProfitAndLoss { get; set; }

        [JsonProperty("symbolId")]
        public string SymbolId { get; set; }

        [JsonProperty("investedCapital")]
        public string InvestedCapital { get; set; }

        [JsonProperty("custodyId")]
        public string CustodyId { get; set; }

        [JsonProperty("securityNameType")]
        public string SecurityNameType { get; set; }

        [JsonProperty("minimumWithdrawal")]
        public string MinimumWithdrawal { get; set; }

        [JsonProperty("incomeTaxFree")]
        public string IncomeTaxFree { get; set; }

        [JsonProperty("securityType")]
        public string SecurityType { get; set; }

        [JsonProperty("nickName")]
        public string NickName { get; set; }

        [JsonProperty("settlement")]
        public string Settlement { get; set; }

        [JsonProperty("settlementType")]
        public string SettlementType { get; set; }

        [JsonProperty("grossValue")]
        public string GrossValue { get; set; }

        [JsonProperty("netValue")]
        public string NetValue { get; set; }

        [JsonProperty("rentability")]
        public string Rentability { get; set; }

        [JsonProperty("updateDate")]
        public string UpdateDate { get; set; }

        [JsonProperty("cache")]
        public string Cache { get; set; }

        [JsonProperty("unitPrice")]
        public string UnitPrice { get; set; }

        [JsonProperty("quantity")]
        public string Quantity { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("investmentType")]
        public InvestmentType InvestmentType { get; set; }

        [JsonProperty("isSecondaryMarket")]
        public string IsSecondaryMarket { get; set; }

        [JsonProperty("isValid")]
        public string IsValid { get; set; }

        [JsonProperty("details")]
        public List<Detail> Details { get; set; }

        [JsonProperty("repurchaseUnitPrice")]
        public string RepurchaseUnitPrice { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("orderId")]
        public string OrderId { get; set; }
    }

    public partial class Detail
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

    }
}
