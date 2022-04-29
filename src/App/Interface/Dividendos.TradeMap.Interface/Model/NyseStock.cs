using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.TradeMap.Interface.Model
{
    public class NyseStock
    {
        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("exchangeId")]
        public long ExchangeId { get; set; }

        [JsonProperty("instrumentType")]
        public string InstrumentType { get; set; }

        [JsonProperty("symbolTicker")]
        public string SymbolTicker { get; set; }

        [JsonProperty("symbolExchangeTicker")]
        public string SymbolExchangeTicker { get; set; }

        [JsonProperty("normalizedTicker")]
        public string NormalizedTicker { get; set; }

        [JsonProperty("symbolEsignalTicker")]
        public string SymbolEsignalTicker { get; set; }

        [JsonProperty("instrumentName")]
        public string InstrumentName { get; set; }

        [JsonProperty("micCode")]
        public string MicCode { get; set; }
    }
}
