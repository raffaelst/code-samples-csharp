using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dividendos.TradeMap.Interface.Model
{
    public class HolidayTd
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("result")]
        public List<string> Result { get; set; }
    }
}
