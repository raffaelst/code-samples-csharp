using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Xp.Interface.Model
{
    public partial class XpPortfolio
    {
        [JsonProperty("totalInvested")]
        public string TotalInvested { get; set; }

        [JsonProperty("totalEarnings")]
        public string TotalEarnings { get; set; }

        [JsonProperty("totalEarningsPercentage")]
        public string TotalEarningsPercentage { get; set; }

        [JsonProperty("groupType")]
        public string GroupType { get; set; }

        [JsonProperty("referenceDate")]
        public string ReferenceDate { get; set; }

        [JsonProperty("patrimony")]
        public string Patrimony { get; set; }

        [JsonProperty("netWorth")]
        public string NetWorth { get; set; }

        [JsonProperty("absolutePatrimony")]
        public string AbsolutePatrimony { get; set; }

        [JsonProperty("declaredAssets")]
        public string DeclaredAssets { get; set; }

        [JsonProperty("totalPositive")]
        public string TotalPositive { get; set; }

        [JsonProperty("totalNegative")]
        public string TotalNegative { get; set; }

        [JsonProperty("balanceAvailable")]
        public string BalanceAvailable { get; set; }

        [JsonProperty("balanceProjected")]
        public string BalanceProjected { get; set; }

        [JsonProperty("insurancePolicies")]
        public string InsurancePolicies { get; set; }

        [JsonProperty("investedPercentage")]
        public string InvestedPercentage { get; set; }

        [JsonProperty("portfolioRisk")]
        public string PortfolioRisk { get; set; }

        [JsonProperty("riskGraph")]
        public string RiskGraph { get; set; }

        [JsonProperty("investments")]
        public List<Investment> Investments { get; set; }

        [JsonProperty("earnings")]
        public List<Earning> Earnings { get; set; }

        [JsonProperty("paidCustody")]
        public List<object> PaidCustody { get; set; }

        [JsonProperty("servicesExpressDebtBalance")]
        public string ServicesExpressDebtBalance { get; set; }
    }

    public partial class Investment
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("children")]
        public List<InvestmentChild> Children { get; set; }

        [JsonProperty("patrimony")]
        public string Patrimony { get; set; }

        [JsonProperty("percentage")]
        public string Percentage { get; set; }

        [JsonProperty("negative")]
        public string Negative { get; set; }
    }

    public partial class InvestmentChild
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("children")]
        public List<ChildChild> Children { get; set; }

        [JsonProperty("patrimony")]
        public string Patrimony { get; set; }

        [JsonProperty("percentage")]
        public string Percentage { get; set; }

        [JsonProperty("negative")]
        public string Negative { get; set; }
    }

    public partial class ChildChild
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("surrogateId")]
        public string SurrogateId { get; set; }

        [JsonProperty("purchaseDate")]
        public string PurchaseDate { get; set; }

        [JsonProperty("positionDate")]
        public string PositionDate { get; set; }

        [JsonProperty("categoryType")]
        public string CategoryType { get; set; }

        [JsonProperty("strategyType")]
        public string StrategyType { get; set; }

        [JsonProperty("liquidityType")]
        public string LiquidityType { get; set; }

        [JsonProperty("groupedType")]
        public string GroupedType { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("redemptionAvailable")]
        public string RedemptionAvailable { get; set; }

        [JsonProperty("investmentAvailable")]
        public string InvestmentAvailable { get; set; }

        [JsonProperty("risk")]
        public string Risk { get; set; }

        [JsonProperty("patrimony")]
        public string Patrimony { get; set; }

        [JsonProperty("percentage")]
        public string Percentage { get; set; }

        [JsonProperty("negative")]
        public string Negative { get; set; }
    }

    public partial class Earning
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("children")]
        public List<EarningChild> Children { get; set; }

        [JsonProperty("patrimony")]
        public string Patrimony { get; set; }

        [JsonProperty("percentage")]
        public string Percentage { get; set; }

        [JsonProperty("negative")]
        public string Negative { get; set; }
    }

    public partial class EarningChild
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("categoryType")]
        public string CategoryType { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("children")]
        public List<PurpleChild> Children { get; set; }

        [JsonProperty("patrimony")]
        public string Patrimony { get; set; }

        [JsonProperty("percentage")]
        public string Percentage { get; set; }

        [JsonProperty("negative")]
        public string Negative { get; set; }
    }

    public partial class PurpleChild
    {
        [JsonProperty("provisionDate")]
        public string ProvisionDate { get; set; }

        [JsonProperty("event")]
        public string Event { get; set; }

        [JsonProperty("provisionQuantity")]
        public string ProvisionQuantity { get; set; }

        [JsonProperty("provisionValue")]
        public string ProvisionValue { get; set; }
    }
}
