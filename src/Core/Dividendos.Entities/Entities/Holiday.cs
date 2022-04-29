using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("Holiday")]
    public class Holiday
    {
        [Key]

        public long IdHoliday { get; set; }
        public DateTime EventDate { get; set; }
        public int IdCountry { get; set; }
    }
}
