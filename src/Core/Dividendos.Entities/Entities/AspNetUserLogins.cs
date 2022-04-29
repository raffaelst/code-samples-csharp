using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("AspNetUserLogins")]
    public class AspNetUserLogins
    {
        [Key]

        public string LoginProvider { get; set; }
        [Key]

        public string ProviderKey { get; set; }
        public string ProviderDisplayName { get; set; }
        public string UserId { get; set; }
    }
}
