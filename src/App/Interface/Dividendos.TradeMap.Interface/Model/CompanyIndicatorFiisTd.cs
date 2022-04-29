using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.TradeMap.Interface.Model
{
    public class Monthly
    {
        [JsonProperty("dt_reference")]
        public string DtReference { get; set; }

        [JsonProperty("vl_patrimony_quotas")]
        public string VlPatrimonyQuotas { get; set; }

        [JsonProperty("vl_patrimony_liq")]
        public string VlPatrimonyLiq { get; set; }

        [JsonProperty("vl_asset")]
        public string VlAsset { get; set; }

        [JsonProperty("total_quotaholder")]
        public string TotalQuotaholder { get; set; }

        [JsonProperty("price_over_vpa")]
        public string PriceOverVpa { get; set; }
    }

    public class ResultIndicatorFiis
    {
        [JsonProperty("monthly")]
        public List<Monthly> Monthly { get; set; }
    }

    public class CompanyIndicatorFiisTd
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("result")]
        public ResultIndicatorFiis Result { get; set; }
    }


}
