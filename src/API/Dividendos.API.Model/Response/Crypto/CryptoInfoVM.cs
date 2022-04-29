using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.API.Model.Response.Crypto
{
    public class CryptoInfoVM
    {
        public Guid CryptoCurrencyGuid { get; set; }
        public string Symbol { get; set; }
        public string Description { get; set; }
        public string LogoURL { get; set; }
    }
}
