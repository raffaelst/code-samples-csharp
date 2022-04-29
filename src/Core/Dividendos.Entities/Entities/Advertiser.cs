using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("Advertiser")]
    public class Advertiser
    {
        [Key]
 
        public long AdvertiserID { get; set; }

        public string Text { get; set; }
        public string BackGroundColor { get; set; }
        public string FontColor { get; set; }
        public Guid AdvertiserGuid { get; set; }

        public bool HasLinkToDetails { get; set; }

        public bool Active { get; set; }
    }
}
