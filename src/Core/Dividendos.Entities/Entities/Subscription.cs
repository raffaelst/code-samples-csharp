using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("Subscription")]
    public class Subscription
    {
        [Key]

        public long SubscriptionID { get; set; }
        public bool Active { get; set; }
        public DateTime ValidUntil { get; set; }

        public string IdUser { get; set; }
        public int SubscriptionTypeID { get; set; }

        public long? PartnerID { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
