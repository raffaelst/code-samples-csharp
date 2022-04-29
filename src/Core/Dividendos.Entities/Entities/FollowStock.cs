using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("FollowStock")]
    public class FollowStock
    {
        [Key]

        public long FollowStockID { get; set; }

        public Guid FollowStockGuid { get; set; }
        public bool Active { get; set; }

        public string UserID { get; set; }
        public long? StockID { get; set; }

        public long? CryptoCurrencyID { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
