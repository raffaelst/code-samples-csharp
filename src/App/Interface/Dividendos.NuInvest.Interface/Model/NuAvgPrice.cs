using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.NuInvest.Interface.Model
{
    public class NuAvgPrice
    {
        [JsonProperty("averagePrice")]
        public string AveragePrice { get; set; }
    }
}
