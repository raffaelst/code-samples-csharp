using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("CryptoTransactionItem")]
    public class CryptoTransactionItem
    {
        [Key]
        public long IdCryptoTransactionItem { get; set; }
        public long IdCryptoTransaction { get; set; }
        public long IdCryptoPortfolio { get; set; }
        public long IdCryptoCurrency { get; set; }
        public DateTime EventDate { get; set; }
        public decimal Quantity { get; set; }
        public decimal AveragePrice { get; set; }
        public string Exchange { get; set; }
        public int TransactionType { get; set; }
        public decimal AcquisitionPrice { get; set; }
        public bool EditedByUser { get; set; }
        public bool Active { get; set; }
        public Guid GuidCryptoTransactionItem { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }
}
