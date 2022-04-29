using Newtonsoft.Json;
using System;

namespace Dividendos.Passfolio.Interface.Model
{

    public class CryptoPassfolioImport
    {
        public string Type { get; set; }

        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public decimal AvailableForWithdrawal { get; set; }
        public decimal Available { get; set; }
    }
}
