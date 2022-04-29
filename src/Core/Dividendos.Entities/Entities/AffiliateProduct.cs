using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("AffiliateProduct")]
    public class AffiliateProduct
    {
        [Key]
 
        public long AffiliateProductId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Producer { get; set; }
        public Guid AffiliateProductGuid { get; set; }

        public string Link { get; set; }
        public bool Active { get; set; }
    }
}
