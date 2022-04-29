using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("Segment")]
    public class Segment
    {
        [Key]
  
        public long IdSegment { get; set; }
        public long IdSubsector { get; set; }

        public string Name { get; set; }
        public Guid GuidSegment { get; set; }
    }
}
