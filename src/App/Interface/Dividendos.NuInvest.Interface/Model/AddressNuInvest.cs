using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.NuInvest.Interface.Model
{
    public partial class AddressNuInvest
    {
        [JsonProperty("postalCode")]
        public string PostalCode { get; set; }

        [JsonProperty("streetName")]
        public string StreetName { get; set; }

        [JsonProperty("addressNumber")]
        public string AddressNumber { get; set; }

        [JsonProperty("complement")]
        public string Complement { get; set; }

        [JsonProperty("neighborhood")]
        public string Neighborhood { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("stateCode")]
        public string StateCode { get; set; }

        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("mailingAddress")]
        public string MailingAddress { get; set; }

        [JsonProperty("generic")]
        public string Generic { get; set; }
    }
}
