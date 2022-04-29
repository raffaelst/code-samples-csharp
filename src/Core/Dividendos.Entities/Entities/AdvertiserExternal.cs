using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("AdvertiserExternal")]
    public class AdvertiserExternal
    {
        [Key]
        public long AdvertiserExternalID { get; set; }

        public string ContentHTML { get; set; }

        public Guid AdvertiserExternalGuid { get; set; }

        public bool Active { get; set; }
    }
}
