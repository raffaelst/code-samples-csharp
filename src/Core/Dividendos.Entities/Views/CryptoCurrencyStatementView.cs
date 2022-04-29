using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Entity.Views
{
    public class CryptoCurrencyStatementView
    {
        public Guid GuidCryptoCurrency { get; set; }
        public Guid GuidCryptoTransaction { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public decimal TotalMarket { get; set; }
        public decimal Total { get; set; }
        public decimal NetValue { get; set; }
        public decimal PerformancePerc { get; set; }
        public decimal Quantity { get; set; }
        public decimal MarketPrice { get; set; }
        public decimal AveragePrice { get; set; }
    }
}
