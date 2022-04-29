using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.TradeMap.Interface.Model
{
    public class TradeMapUSAStocks
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("result")]
        public List<ResultStock> ResultStock { get; set; }
    }

    public class ResultStock
    {
        [JsonProperty("id_stock")]
        public long IdStock { get; set; }

        [JsonProperty("cd_stock")]
        public string CdStock { get; set; }

        [JsonProperty("id_company")]
        public long IdCompany { get; set; }

        [JsonProperty("cd_company")]
        public string CdCompany { get; set; }

        [JsonProperty("price_previous_closing")]
        public double PricePreviousClosing { get; set; }

        [JsonProperty("nm_company_exchange")]
        public string NmCompanyExchange { get; set; }

        [JsonProperty("cd_country")]
        public string CdCountry { get; set; }

        [JsonProperty("sector")]
        public string Sector { get; set; }

        [JsonProperty("industry")]
        public string Industry { get; set; }

        [JsonProperty("logo")]
        public string Logo { get; set; }

        [JsonProperty("ds_position_segment")]
        public string DsPositionSegment { get; set; }

        [JsonProperty("report_date")]
        public long ReportDate { get; set; }

        [JsonProperty("share_holder_equity")]
        public long ShareHolderEquity { get; set; }

        [JsonProperty("marketcap")]
        public double Marketcap { get; set; }

        [JsonProperty("roa")]
        public double Roa { get; set; }

        [JsonProperty("roe")]
        public double Roe { get; set; }

        [JsonProperty("ebit_margin")]
        public double EbitMargin { get; set; }

        [JsonProperty("net_margin")]
        public double NetMargin { get; set; }

        [JsonProperty("dividend_yield")]
        public double DividendYield { get; set; }

        [JsonProperty("payout")]
        public double Payout { get; set; }

        [JsonProperty("mcap_over_pl")]
        public double McapOverPl { get; set; }

        [JsonProperty("evo_profit_three")]
        public double EvoProfitThree { get; set; }

        [JsonProperty("evo_profit_five")]
        public long EvoProfitFive { get; set; }

        [JsonProperty("evo_ebit_three")]
        public double EvoEbitThree { get; set; }

        [JsonProperty("evo_ebit_five")]
        public long EvoEbitFive { get; set; }

        [JsonProperty("evo_income_three")]
        public double EvoIncomeThree { get; set; }

        [JsonProperty("evo_income_five")]
        public long EvoIncomeFive { get; set; }

        [JsonProperty("evo_networth_three")]
        public double EvoNetworthThree { get; set; }

        [JsonProperty("evo_networth_five")]
        public long EvoNetworthFive { get; set; }

        [JsonProperty("evo_asset_three")]
        public double EvoAssetThree { get; set; }

        [JsonProperty("evo_asset_five")]
        public long EvoAssetFive { get; set; }

        [JsonProperty("avg_rank")]
        public double AvgRank { get; set; }

        [JsonProperty("total_users")]
        public long TotalUsers { get; set; }
    }
}
