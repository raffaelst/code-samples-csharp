using Dapper.Contrib.Extensions;
using System;


namespace Dividendos.Entity.Entities
{
    [Table("CryptoTransaction")]
    public class CryptoTransaction
    {
        [Key]
        public long IdCryptoTransaction { get; set; }
        public long IdCryptoPortfolio { get; set; }
        public long IdCryptoCurrency { get; set; }
        public decimal Quantity { get; set; }
        public decimal AveragePrice { get; set; }
        public string Exchange { get; set; }
        public int TransactionType { get; set; }
        public bool Active { get; set; }
        public Guid GuidCryptoTransaction { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }
}
