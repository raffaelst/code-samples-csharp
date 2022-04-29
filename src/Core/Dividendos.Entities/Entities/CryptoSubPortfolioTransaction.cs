using Dapper.Contrib.Extensions;
using System;


namespace Dividendos.Entity.Entities
{
    [Table("CryptoSubPortfolioTransaction")]
    public class CryptoSubPortfolioTransaction
    {
        [Key]
        public long IdCryptoSubPortfolioTransaction { get; set; }
        public long IdCryptoPortfolio { get; set; }
        public long IdCryptoSubPortfolio { get; set; }
        public long IdCryptoTransaction { get; set; }
        public long IdCryptoCurrency { get; set; }

    }
}
