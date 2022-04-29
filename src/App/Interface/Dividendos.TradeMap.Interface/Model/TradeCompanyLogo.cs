using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.TradeMap.Interface.Model
{
    public class TradeCompanyLogo
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("result")]
        public string ResultCompanyLogo { get; set; }
    }
}
