using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Xp.Interface.Model
{
    public class ImportXpResult
    {
        public string Message { get; set; }
        public string JwtToken { get; set; }
        public List<XpOrder> Orders { get; set; }
        public List<XpBond> Bonds { get; set; }
        public List<XpFund> Funds { get; set; }
        public List<XpDividend> Dividends { get; set; }
        public bool Success { get; set; }
        public string ApiResult { get; set; }
        public string ResponseBody { get; set; }
    }
}
