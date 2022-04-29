using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("AdvertiserUser")]
    public class AdvertiserUser
    {
        [Key]
 
        public long AdvertiserUserID { get; set; }

        public long AdvertiserID { get; set; }
        public string UserID { get; set; }
        public DateTime ShowUntil { get; set; }
    }
}
