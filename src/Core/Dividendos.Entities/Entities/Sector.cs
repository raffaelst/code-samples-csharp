using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("Sector")]
    public class Sector
    {
        [Key]
 
        public long IdSector { get; set; }

        public string Name { get; set; }
        public Guid GuidSector { get; set; }
        public int IdCountry { get; set; }
    }
}
