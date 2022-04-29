using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{

    [Table("AspNetRoleClaims")]
    public class AspNetRoleClaims
    {
        [Key]
        public int Id { get; set; }

        public string RoleId { get; set; }

        public string ClaimType { get; set; }

        public string ClaimValue { get; set; }
    }
}