using Dapper.Contrib.Extensions;

namespace Dividendos.Entity.Entities
{
    [Table("SubscriptionType")]
    public class SubscriptionType
    {
        [Key]
        public int SubscriptionTypeID { get; set; }

        public string Description { get; set; }
    }
}
