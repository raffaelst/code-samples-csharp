using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Avenue.Interface.Model
{
    public class ImportAvenueResult
    {
        public string Message { get; set; }
        public string Session { get; set; }
        public string Challenge { get; set; }
        public List<AvenueOrder> Orders { get; set; }
        public List<AvenueDividend> Dividends { get; set; }
        public AvenueContactDetails ContactDetails { get; set; }
        public List<AvenueContactPhone> ContactPhones { get; set; }
        public bool Success { get; set; }
        public string ApiResult { get; set; }
        public string AccessToken { get; set; }
        public string IdToken { get; set; }
    }
}
