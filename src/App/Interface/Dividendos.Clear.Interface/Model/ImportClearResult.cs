using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Clear.Interface.Model
{
    public class ImportClearResult
    {
        public string Message { get; set; }
        public string JwtToken { get; set; }
        public List<ClearOrder> Orders { get; set; }
        public List<ClearOrderItem> OrderItems { get; set; }
        public List<ClearDividend> Dividends { get; set; }
        public bool Success { get; set; }
        public string ApiResult { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string DividendException { get; set; }
        public string ResponseBody { get; set; }
    }
}
