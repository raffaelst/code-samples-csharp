using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Toro.Interface.Model
{
    public class ImportToroResult
    {
        public string Message { get; set; }
        public string JwtToken { get; set; }
        public List<ToroOrder> Orders { get; set; }
        public List<ToroDividend> Dividends { get; set; }
        public List<ToroBond> Bonds { get; set; }
        public List<ToroFund> Funds { get; set; }
        public bool Success { get; set; }
        public string ApiResult { get; set; }
    }
}
