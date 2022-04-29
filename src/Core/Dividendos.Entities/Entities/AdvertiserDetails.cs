using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("AdvertiserDetails")]
    public class AdvertiserDetails
    {
        [Key]
 
        public long AdvertiserDetailsID { get; set; }

        public string ContentHtml { get; set; }
        public long AdvertiserID { get; set; }
    }
}
