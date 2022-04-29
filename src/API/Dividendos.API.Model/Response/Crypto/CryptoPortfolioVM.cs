using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.API.Model.Response.Crypto
{
    public class CryptoPortfolioVM
    {
        public Guid CryptoPortfolioGuid { get; set; }
        public string Name { get; set; }
        public bool ManualPortfolio { get; set; }
    }
}
