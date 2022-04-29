using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("AspNetUserTokens")]
    public class AspNetUserTokens
    {
        [Key]

        public string UserId { get; set; }
        [Key]

        public string LoginProvider { get; set; }
        [Key]

        public string Name { get; set; }
 
        public string Value { get; set; }
    }
}
