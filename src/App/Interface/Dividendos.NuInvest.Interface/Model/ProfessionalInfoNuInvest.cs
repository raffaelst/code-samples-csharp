using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.NuInvest.Interface.Model
{
    public partial class ProfessionalInfoNuInvest
    {
        [JsonProperty("occupationId")]
        public string OccupationId { get; set; }

        [JsonProperty("occupationDescription")]
        public string OccupationDescription { get; set; }

        [JsonProperty("activityArea")]
        public string ActivityArea { get; set; }

        [JsonProperty("institutionName")]
        public string InstitutionName { get; set; }

        [JsonProperty("cnpjNumber")]
        public string CnpjNumber { get; set; }

        [JsonProperty("working")]
        public string Working { get; set; }

        [JsonProperty("isJob")]
        public string IsJob { get; set; }
    }
}
