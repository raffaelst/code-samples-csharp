using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.TradeMap.Interface.Model
{
    public class StockMarketMover
    {
        [JsonProperty("cd_stock")]

        public string Stock { get; set; }

        [JsonProperty("variation")]

        
        public string Variation { get; set; }
        
        [JsonProperty("vl_close")]

        public string CloseValue { get; set; }


        [JsonProperty("dividend_yield")]
        public string DividendYield { get; set; }


        [JsonProperty("provent_price_ajust")]
        public string TotalPaid { get; set; }
        
    }
}
