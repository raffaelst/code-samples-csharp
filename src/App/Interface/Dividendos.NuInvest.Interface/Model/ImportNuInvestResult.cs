using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.NuInvest.Interface.Model
{
    public class ImportNuInvestResult
    {
        public string Message { get; set; }
        public string ResponseBody { get; set; }
        public string JwtToken { get; set; }
        public List<NuInvestOrder> Orders { get; set; }
        public List<NuInvestOrder> OrdersAvgPrice { get; set; }
        public List<NuInvestBond> Bonds { get; set; }
        public List<NuInvestFund> Funds { get; set; }
        public NuInvestContactDetails ContactDetails { get; set; }
        public List<NuInvestContactPhone> ContactPhones { get; set; }
        public bool Success { get; set; }
        public string ApiResult { get; set; }
        public List<string> Warnings { get; set; }
    }
}
