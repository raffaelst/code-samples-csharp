using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.API.Model.Response.Crypto
{
    public class CryptoSubportfolioItemAddVM
    {
        public Guid GuidCryptoTransaction { get; set; }
        public bool Selected { get; set; }
        public string CryptoName { get; set; }
        public string Logo { get; set; }
    }
}
