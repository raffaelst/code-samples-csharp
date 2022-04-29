using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.NuInvest.Interface.Model
{
    public partial class PersonalInfoNuInvest
    {
        [JsonProperty("personalInfo")]
        public PersonalInfo PersonalInfo { get; set; }
    }

    public partial class PersonalInfo
    {
        [JsonProperty("documentNumber")]
        public string DocumentNumber { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("nickname")]
        public string Nickname { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("motherName")]
        public string MotherName { get; set; }

        [JsonProperty("birthCity")]
        public string BirthCity { get; set; }

        [JsonProperty("birthState")]
        public string BirthState { get; set; }

        [JsonProperty("birthDate")]
        public string BirthDate { get; set; }

        [JsonProperty("countryFrom")]
        public string CountryFrom { get; set; }

        [JsonProperty("maritalStatus")]
        public long MaritalStatus { get; set; }

        [JsonProperty("spouseName")]
        public string SpouseName { get; set; }

        [JsonProperty("spouseDocumentNumber")]
        public string SpouseDocumentNumber { get; set; }

        [JsonProperty("nationality")]
        public string Nationality { get; set; }

        [JsonProperty("phones")]
        public Phone[] Phones { get; set; }
    }

    public partial class Phone
    {
        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }

        [JsonProperty("areaCode")]
        public string AreaCode { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("mainPhone")]
        public string MainPhone { get; set; }
    }
}
