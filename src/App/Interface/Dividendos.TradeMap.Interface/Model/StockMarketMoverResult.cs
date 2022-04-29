using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.TradeMap.Interface.Model
{
    public class StockMarketMoverResult
    {

        [JsonProperty("result")]
        public List<StockMarketMover> StockMarketMover { get; set; }
    }
}
