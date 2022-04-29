using Dividendos.API.Model.Response.v2;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.API.Model.Response.Crypto
{
    public class CryptoCurrencyStatementWrapperVM
    {
        [JsonProperty("guidCryptoPortfolioSubPortfolio")]
        public Guid GuidCryptoPortfolioSubPortfolio { get; set; }

        [JsonProperty("manualPortfolio")]
        public bool ManualPortfolio { get; set; }

        [JsonProperty("lastUpdated")]
        public string LastUpdated { get; set; }

        [JsonProperty("lastSyncDate")]
        public DateTime LastSyncDate { get; set; }

        [JsonProperty("idFiatCurrency")]
        public int IdFiatCurrency { get; set; }

        [JsonProperty("cryptoCurrencyStatement")]
        public IEnumerable<CryptoCurrencyStatementVM> CryptoCurrencyStatement { get; set; }

        [JsonProperty("traderTypeId")]
        public long TraderTypeId { get; set; }

        [JsonProperty("canEdit")]
        public bool CanEdit { get; set; }
    }
}
