using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.TradeMap.Interface.Model
{
    public class TradeCompanyInfo
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("result")]
        public ResultCompanyInfo Result { get; set; }
    }

    public partial class ResultCompanyInfo
    {
        [JsonProperty("market_position")]
        public MarketPosition MarketPosition { get; set; }

        [JsonProperty("detail")]
        public Detail Detail { get; set; }

        [JsonProperty("ipo")]
        public Ipo Ipo { get; set; }

        [JsonProperty("governanca")]
        public object[] Governanca { get; set; }

        [JsonProperty("mercado")]
        public object[] Mercado { get; set; }

        [JsonProperty("distribuicao")]
        public object[] Distribuicao { get; set; }

        [JsonProperty("calendario")]
        public object[] Calendario { get; set; }
    }

    public partial class Detail
    {
        [JsonProperty("cd_cvm")]
        public string CdCvm { get; set; }

        [JsonProperty("ds_company_activict")]
        public string DsCompanyActivict { get; set; }

        [JsonProperty("company_web_page")]
        public string CompanyWebPage { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("neighborhood")]
        public string Neighborhood { get; set; }

        [JsonProperty("zip")]
        public string Zip { get; set; }

        [JsonProperty("email_address")]
        public string EmailAddress { get; set; }

        [JsonProperty("ceo")]
        public string Ceo { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("nm_country_pt")]
        public string NmCountryPt { get; set; }

        [JsonProperty("nm_country_en")]
        public string NmCountryEn { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("exchange")]
        public string Exchange { get; set; }

        [JsonProperty("security_type_us")]
        public string SecurityTypeUs { get; set; }
    }

    public partial class Ipo
    {
    }

    public partial class MarketPosition
    {
        [JsonProperty("sector")]
        public string Sector { get; set; }

        [JsonProperty("industry")]
        public string Industry { get; set; }
    }
}
