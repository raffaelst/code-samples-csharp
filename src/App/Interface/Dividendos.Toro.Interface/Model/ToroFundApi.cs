using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Toro.Interface.Model
{
    public partial class ToroFundApi
    {
        [JsonProperty("isOrder")]
        public string IsOrder { get; set; }

        [JsonProperty("isActive")]
        public string IsActive { get; set; }

        [JsonProperty("updatedAt")]
        public string UpdatedAt { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("fundId")]
        public string FundId { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("createdAt")]
        public string CreatedAt { get; set; }

        [JsonProperty("cblcIdClient")]
        public string CblcIdClient { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("fund")]
        public Fund Fund { get; set; }
    }

    public partial class Fund
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("offerName")]
        public string OfferName { get; set; }

        [JsonProperty("shortName")]
        public string ShortName { get; set; }

        [JsonProperty("daysToPayRedeem")]
        public string DaysToPayRedeem { get; set; }

        [JsonProperty("daysToQuoteRedeem")]
        public string DaysToQuoteRedeem { get; set; }

        [JsonProperty("classification")]
        public string Classification { get; set; }

        [JsonProperty("minimumTransaction")]
        public long MinimumTransaction { get; set; }

        [JsonProperty("acceptanceTermId")]
        public string AcceptanceTermId { get; set; }

        [JsonProperty("isAvailable")]
        public string IsAvailable { get; set; }
    }
}
