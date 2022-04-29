using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.API.Model.Response.Crypto
{
    public class CryptoStatementVM
    {
        [JsonProperty("idStock")]
        public long IdStock { get; set; }

        [JsonProperty("guidOperation")]
        public Guid GuidOperation { get; set; }

        [JsonProperty("company")]
        public string Company { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("logo")]
        public string Logo { get; set; }

        [JsonProperty("segment")]
        public string Segment { get; set; }

        [JsonProperty("total")]
        public string Total { get; set; }

        [JsonProperty("netValue")]
        public string NetValue { get; set; }

        [JsonProperty("performancePerc")]
        public string PerformancePerc { get; set; }

        [JsonProperty("numberOfShares")]
        public string NumberOfShares { get; set; }

        [JsonProperty("averagePrice")]
        public string AveragePrice { get; set; }



        [JsonProperty("totalDividends")]
        public string TotalDividends { get; set; }

        [JsonProperty("totalN")]
        public decimal TotalN { get; set; }

        [JsonProperty("netValueN")]
        public decimal NetValueN { get; set; }

        [JsonProperty("performancePercN")]
        public decimal PerformancePercN { get; set; }

        [JsonProperty("numberOfSharesN")]
        public decimal NumberOfSharesN { get; set; }

        [JsonProperty("averagePriceN")]
        public decimal AveragePriceN { get; set; }


        [JsonProperty("totalDividendsN")]
        public decimal TotalDividendsN { get; set; }

        [JsonProperty("idCountry")]
        public int IdCountry { get; set; }


        [JsonProperty("broker")]
        public string Broker { get; set; }

        [JsonProperty("totalMarket")]
        public string TotalMarket { get; set; }

        [JsonProperty("totalMarketN")]
        public decimal TotalMarketN { get; set; }

        [JsonProperty("totalUSD")]
        public string TotalUSD { get; set; }

        [JsonProperty("totalUSDN")]
        public decimal TotalUSDN { get; set; }

        [JsonProperty("marketPriceUSD")]
        public string MarketPriceUSD { get; set; }


        [JsonProperty("marketPriceUSDN")]
        public decimal MarketPriceUSDN { get; set; }


        [JsonProperty("marketPrice")]
        public string MarketPrice { get; set; }


        [JsonProperty("marketPriceN")]
        public decimal MarketPriceN { get; set; }

        [JsonProperty("totalBTC")]
        public string TotalBTC { get; set; }

        [JsonProperty("totalBTCN")]
        public decimal TotalBTCN { get; set; }

        [JsonProperty("marketPriceBTC")]
        public string MarketPriceBTC { get; set; }


        [JsonProperty("marketPriceBTCN")]
        public decimal MarketPriceBTCN { get; set; }


        [JsonProperty("variationInReal")]
        public string VariationInReal { get; set; }

        [JsonProperty("variationInRealN")]
        public decimal VariationInRealN { get; set; }

        [JsonProperty("variationInDolar")]
        public string VariationInDolar { get; set; }

        [JsonProperty("variationInDolarN")]
        public decimal VariationInDolarN { get; set; }


        [JsonProperty("yourPerformancePerc")]
        public string YourPerformancePerc { get; set; }

        [JsonProperty("yourPerformancePercN")]
        public decimal YourPerformancePercN { get; set; }
    }
}
