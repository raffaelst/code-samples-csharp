using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("Logo")]
    public class Logo
    {
        [Key]
        public long IdLogo { get; set; }

        public string LogoImage { get; set; }

        public Guid GuidLogo { get; set; }

        public string CompanyCode { get; set; }

        public string URL { get; set; }
    }
}
