using Dapper.Contrib.Extensions;
using System;


namespace Dividendos.Entity.Entities
{
    [Table("CryptoPortfolio")]
    public class CryptoPortfolio
    {
        [Key]
        public long IdCryptoPortfolio { get; set; }
        public long IdTrader { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid GuidCryptoPortfolio { get; set; }
        public bool Manual { get; set; }
        public DateTime CalculatePerformanceDate { get; set; }
        public bool Active { get; set; }
        public int IdFiatCurrency { get; set; }
    }
}
