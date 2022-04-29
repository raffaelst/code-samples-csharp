using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.B3.Interface.Model
{
    public class GenericImportWrapper
    {
        [JsonProperty("identifier")]
        public string Identifier { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("userID")]
        public string UserID { get; set; }
        [JsonProperty("automaticProcess")]
        public bool AutomaticProcess { get; set; }
        [JsonProperty("retryLocal")]
        public bool RetryLocal { get; set; }
        [JsonProperty("webhookUrl")]
        public string WebhookUrl { get; set; }

        [JsonProperty("genericImport")]
        public List<GenericImport> GenericImport;
    }
}
