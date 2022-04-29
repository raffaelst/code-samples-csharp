using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.TradeMap.Interface.Model
{
    public class CompanyHistoricTd
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("result")]
        public CompanyDividendTd CompanyDividendTd { get; set; }
    }

    public class CompanyDividendTd
    {
        [JsonProperty("divident_tres_meses")]
        public string DividentTresMeses { get; set; }

        [JsonProperty("divident_seis_meses")]
        public string DividentSeisMeses { get; set; }

        [JsonProperty("divident_um")]
        public string DividentUm { get; set; }

        [JsonProperty("divident_dois")]
        public string DividentDois { get; set; }

        [JsonProperty("divident_tres")]
        public string DividentTres { get; set; }

        [JsonProperty("divident_ultimo")]
        public string DividentUltimo { get; set; }

        [JsonProperty("divident_last_value")]
        public string DividentLastValue { get; set; }

        [JsonProperty("divident_ipo")]
        public string DividentIpo { get; set; }

        [JsonProperty("divident_yield_tres_meses")]
        public string DividentYieldTresMeses { get; set; }

        [JsonProperty("divident_yield_seis_meses")]
        public string DividentYieldSeisMeses { get; set; }

        [JsonProperty("divident_yield_um")]
        public string DividentYieldUm { get; set; }

        [JsonProperty("divident_yield_dois")]
        public string DividentYieldDois { get; set; }

        [JsonProperty("divident_yield_tres")]
        public string DividentYieldTres { get; set; }

        [JsonProperty("divident_yield_ultimo")]
        public string DividentYieldUltimo { get; set; }

        [JsonProperty("divident_yield_last_value")]
        public string DividentYieldLastValue { get; set; }

        [JsonProperty("divident_yield_ipo")]
        public string DividentYieldIpo { get; set; }

        [JsonProperty("divident_yield_avg")]
        public string DividentYieldAvg { get; set; }
    }

}
