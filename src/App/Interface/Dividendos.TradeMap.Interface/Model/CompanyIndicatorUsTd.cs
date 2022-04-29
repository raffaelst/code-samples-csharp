using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.TradeMap.Interface.Model
{
    public class Finance
    {
        [JsonProperty("report_date")]
        public string ReportDate { get; set; }

        [JsonProperty("share_holder_equity")]
        public string ShareHolderEquity { get; set; }

        [JsonProperty("total_assets")]
        public string TotalAssets { get; set; }

        [JsonProperty("net_income")]
        public string NetIncome { get; set; }
    }

    public class MarketData
    {
        [JsonProperty("marketcap")]
        public string Marketcap { get; set; }
    }

    public class Index
    {
        [JsonProperty("divida_liquida")]
        public string DividaLiquida { get; set; }

        [JsonProperty("roe")]
        public string Roe { get; set; }

        [JsonProperty("roa")]
        public string Roa { get; set; }

        [JsonProperty("price_per_worth")]
        public string PricePerWorth { get; set; }

        [JsonProperty("price_per_profit")]
        public string PricePerProfit { get; set; }
    }

    public class ResultIndicatorUs
    {
        [JsonProperty("report_date")]
        public string ReportDate { get; set; }

        [JsonProperty("finance")]
        public Finance Finance { get; set; }

        [JsonProperty("market_data")]
        public MarketData MarketData { get; set; }

        [JsonProperty("index")]
        public Index Index { get; set; }
    }

    public class CompanyIndicatorUsTd
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("result")]
        public List<ResultIndicatorUs> Result { get; set; }
    }
}
