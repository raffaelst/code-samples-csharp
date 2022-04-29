using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Entity.Views
{
    public class CryptoTransactionDetailsView
    {
        public Guid GuidCryptoTransactionItem { get; set; }
        public long IdTransactionItem { get; set; }
        public DateTime EventDate { get; set; }
        public decimal Quantity { get; set; }
        public decimal AveragePrice { get; set; }
        public decimal AcquisitionPrice { get; set; }
        public long IdCryptoPortfolio { get; set; }
        public Guid GuidCryptoCurrency { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string Exchange { get; set; }
    }
}
