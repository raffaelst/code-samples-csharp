using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.API.Model.Response.Crypto
{
    public class CryptoPortfolioStatementWrapperVM
    {
        [JsonProperty("guidPortfolioSubPortfolio")]
        public Guid GuidPortfolioSubPortfolio { get; set; }

        [JsonProperty("manualPortfolio")]
        public bool ManualPortfolio { get; set; }

        [JsonProperty("lastUpdated")]
        public string LastUpdated { get; set; }

        [JsonProperty("lastSyncDate")]
        public DateTime LastSyncDate { get; set; }

        [JsonProperty("idCountry")]
        public int IdCountry { get; set; }

        [JsonProperty("stocksStatement")]
        public IEnumerable<CryptoStatementVM> StocksStatement { get; set; }


        public string TotalInReal { get; set; }

        public string TotalInDolar { get; set; }

        public string TotalInBTC { get; set; }
    }
}
