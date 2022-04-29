using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.API.Model.Response.Crypto
{
    public class CryptoPortfolioViewVM
    {
        [JsonProperty("guidCryptoPortfolioSubPortfolio")]
        public Guid GuidCryptoPortfolioSubPortfolio { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("totalMarket")]
        public string TotalMarket { get; set; }

        [JsonProperty("total")]
        public string Total { get; set; }

        [JsonProperty("netValue")]
        public string NetValue { get; set; }

        [JsonProperty("latestNetValue")]
        public string LatestNetValue { get; set; }

        [JsonProperty("previousNetValue")]
        public string PreviousNetValue { get; set; }

        [JsonProperty("performancePerc")]
        public string PerformancePerc { get; set; }

        [JsonProperty("performancePercTWR")]
        public string PerformancePercTWR { get; set; }

        [JsonProperty("profit")]
        public string Profit { get; set; }

        [JsonProperty("calculationDate")]
        public string CalculationDate { get; set; }

        [JsonProperty("lastUpdated")]
        public string LastUpdated { get; set; }

        [JsonProperty("idFiatCurrency")]
        public int IdFiatCurrency { get; set; }

        [JsonProperty("isPortfolio")]
        public bool IsPortfolio { get; set; }
    }
}
