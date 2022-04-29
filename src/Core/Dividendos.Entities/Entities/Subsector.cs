
using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("Subsector")]
    public class Subsector
    {
        [Key]
        public long IdSubsector { get; set; }
        public long IdSector { get; set; }
        public string Name { get; set; }
        public Guid GuidSubsector { get; set; }
    }
}
