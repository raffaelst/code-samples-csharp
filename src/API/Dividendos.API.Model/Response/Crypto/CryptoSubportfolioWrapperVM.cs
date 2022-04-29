using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.API.Model.Response.Crypto
{
    public class CryptoSubportfolioWrapperVM
    {
        public Guid GuidCryptoPortfolio { get; set; }
        public Guid GuidCryptoSubportfolio { get; set; }
        public string Name { get; set; }
        public List<CryptoSubportfolioItemAddVM> CryptoSubportfolioItems { get; set; }
    }
}
