using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Rico.Interface.Model
{
    public partial class RicoContactDetailApi
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("items")]
        public List<ContactDetailItem> Items { get; set; }
    }

    public partial class ContactDetailItem
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("value")]
        public string? Value { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
