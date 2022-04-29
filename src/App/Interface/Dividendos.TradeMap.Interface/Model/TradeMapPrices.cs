using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.TradeMap.Interface.Model
{
    public class TradeMapPrices
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("result")]
        public Dictionary<string, string[]> Result { get; set; }
    }

    public class Result
    {
        [JsonProperty("close_limit")]
        public CloseLimit CloseLimit { get; set; }

        [JsonProperty("close_variation")]
        public Dictionary<string, string[]> CloseVariation { get; set; }
    }

    public class CloseLimit
    {
    }
}
