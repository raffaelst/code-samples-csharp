using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("SubPortfolio")]
    public class SubPortfolio
    {
        [Key]
        public long IdSubPortfolio { get; set; }

        public long IdPortfolio { get; set; }

        public string Name { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid GuidSubPortfolio { get; set; }
    }
}
