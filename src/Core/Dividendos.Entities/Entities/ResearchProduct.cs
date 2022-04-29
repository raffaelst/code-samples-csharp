using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("ResearchProduct")]
    public class ResearchProduct
    {
        [Key]
 
        public long ResearchProductID { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string URL { get; set; }
        public string ContentResponsible { get; set; }
        public bool Active { get; set; }
        public string Titile { get; set; }
    }
}
