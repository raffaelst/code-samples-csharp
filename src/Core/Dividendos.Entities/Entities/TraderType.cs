using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("TraderType")]
    public static class TraderType
    {
        [Key]
        public static long TraderTypeID { get; set; }
        public static string Description { get; set; }
    }
}
