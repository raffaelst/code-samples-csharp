using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.API.Model.Response.Crypto
{
    public class CryptoCurrencyStatementVM
    {
        [JsonProperty("guidCryptoCurrency")]
        public Guid GuidCryptoCurrency { get; set; }

        [JsonProperty("guidCryptoTransaction")]
        public Guid GuidCryptoTransaction { get; set; }

        [JsonProperty("guidCryptoPortfolio")]
        public Guid GuidCryptoPortfolio { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("logo")]
        public string Logo { get; set; }

        [JsonProperty("totalMarket")]
        public string TotalMarket { get; set; }

        [JsonProperty("total")]
        public string Total { get; set; }

        [JsonProperty("netValue")]
        public string NetValue { get; set; }

        [JsonProperty("performancePerc")]
        public string PerformancePerc { get; set; }

        [JsonProperty("quantity")]
        public string Quantity { get; set; }

        [JsonProperty("averagePrice")]
        public string AveragePrice { get; set; }

        [JsonProperty("marketPrice")]
        public string MarketPrice { get; set; }

        [JsonProperty("totalMarketN")]
        public decimal TotalMarketN { get; set; }

        [JsonProperty("totalN")]
        public decimal TotalN { get; set; }

        [JsonProperty("netValueN")]
        public decimal NetValueN { get; set; }

        [JsonProperty("performancePercN")]
        public decimal PerformancePercN { get; set; }

        [JsonProperty("quantityN")]
        public decimal QuantityN { get; set; }

        [JsonProperty("averagePriceN")]
        public decimal AveragePriceN { get; set; }

        [JsonProperty("marketPriceN")]
        public decimal MarketPriceN { get; set; }

        [JsonProperty("idFiatCurrency")]
        public int IdFiatCurrency { get; set; }
    }
}
