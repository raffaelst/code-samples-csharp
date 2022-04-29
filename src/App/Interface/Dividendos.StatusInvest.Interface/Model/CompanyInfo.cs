using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dividendos.StatusInvest.Interface.Model
{
    public partial class CompanyInfo
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("data")]
        public List<Datum> Data { get; set; }
    }

    public partial class Datum
    {
        [JsonProperty("companyId")]
        public long CompanyId { get; set; }

        [JsonProperty("companyName")]
        public string CompanyName { get; set; }

        [JsonProperty("companyNameClean")]
        public string CompanyNameClean { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("descriptionLink")]
        public string DescriptionLink { get; set; }

        [JsonProperty("categoryId")]
        public long CategoryId { get; set; }

        [JsonProperty("tickers")]
        public List<Ticker> Tickers { get; set; }

        [JsonProperty("urlClear")]
        public string UrlClear { get; set; }

        public string Logo { get; set; }

        public string Segment { get; set; }
    }

    public partial class Ticker
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
