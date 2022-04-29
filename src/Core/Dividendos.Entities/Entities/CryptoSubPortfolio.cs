using Dapper.Contrib.Extensions;
using System;


namespace Dividendos.Entity.Entities
{
    [Table("CryptoSubPortfolio")]
    public class CryptoSubPortfolio
    {
        [Key]
        public long IdCryptoSubPortfolio { get; set; }
        public long IdCryptoPortfolio { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid GuidCryptoSubPortfolio { get; set; }
        public bool Active { get; set; }
    }
}
