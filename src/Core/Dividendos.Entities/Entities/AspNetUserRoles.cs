using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("AspNetUserRoles")]
    public class AspNetUserRoles
    {
        [Key]

        public string UserId { get; set; }
        [Key]

        public string RoleId { get; set; }
    }
}
