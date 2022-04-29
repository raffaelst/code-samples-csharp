using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("Partner")]
    public class Partner
    {
        [Key]
        public long PartnerID { get; set; }

        public string Description { get; set; }

        public string HTMLContentButton { get; set; }

        public string HTMLContentAds { get; set; }

        public bool Active { get; set; }


        public Guid PartnerGuid { get; set; }

        public string URL { get; set; }

    }
}
