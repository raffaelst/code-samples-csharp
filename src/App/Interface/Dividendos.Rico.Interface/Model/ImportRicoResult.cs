using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Rico.Interface.Model
{
    public class ImportRicoResult
    {
        public string Message { get; set; }
        public string JwtToken { get; set; }
        public List<RicoOrder> Orders { get; set; }
        public List<RicoDividend> Dividends { get; set; }
        public RicoContactDetails ContactDetails { get; set; }
        public List<RicoContactPhone> ContactPhones { get; set; }
        public bool Success { get; set; }
        public string ApiResult { get; set; }
        public List<string> RicoWarnings { get; set; }
        public List<RicoBond> Bonds { get; set; }
        public List<RicoFund> Funds { get; set; }
    }
}
