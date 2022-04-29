using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.TradeMap.Interface.Model
{
    public partial class CompanyIndicatorTd
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("result")]
        public CompanyIndicator[] Result { get; set; }
    }

    public partial class CompanyIndicator
    {
        [JsonProperty("indices")]
        public Indices Indices { get; set; }

        [JsonProperty("indices_last")]
        public IndicesLast IndicesLast { get; set; }
    }

    public partial class Indices
    {
        [JsonProperty("roe_annual")]
        public string RoeAnnual { get; set; }

        [JsonProperty("roa_annual")]
        public string RoaAnnual { get; set; }

        [JsonProperty("qtty_stock")]
        public string QttyStock { get; set; }

        [JsonProperty("roic_annual")]
        public string RoicAnnual { get; set; }

        [JsonProperty("payout_annual")]
        public string PayoutAnnual { get; set; }

        [JsonProperty("dt_entry")]
        public string DtEntry { get; set; }

        [JsonProperty("divida_liquida")]
        public string DividaLiquida { get; set; }

        [JsonProperty("vl_profit_annual")]
        public string VlProfitAnnual { get; set; }
        
        [JsonProperty("total_assets")]
        public string TotalAssets { get; set; }

        [JsonProperty("price_profit_annual")]
        public string PriceProfitAnnual { get; set; }
    }

    public partial class IndicesLast
    {
        [JsonProperty("net_worth")]
        public string NetWorth { get; set; }

        [JsonProperty("price_per_vpa")]
        public string PricePerVpa { get; set; }

        [JsonProperty("mcap")]
        public string MarketCap { get; set; }
    }
}
