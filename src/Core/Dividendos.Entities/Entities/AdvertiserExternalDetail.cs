using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("AdvertiserExternalDetail")]
    public class AdvertiserExternalDetail
    {
        [Key]
 
        public long AdvertiserExternalDetailID { get; set; }

        public string ContentHTML { get; set; }
        public long AdvertiserExternalID { get; set; }
        public string URL { get; set; }
        public string CallToActionButtonTitle { get; set; }
    }
}
