using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.API.Model.Request.Crypto
{
    public class CryptoPortolioRequest
    {
        public string Name { get; set; }
        public int IdFiatCurrency { get; set; }
    }
}
