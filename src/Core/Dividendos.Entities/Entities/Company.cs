using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("Company")]
    public class Company
    {
        [Key]
        public long IdCompany { get; set; }
   
        public string Name { get; set; }
        public long? IdLogo { get; set; }
        public long IdSegment { get; set; }

        public string FullName { get; set; }
        public Guid GuidCompany { get; set; }

        public string Code { get; set; }

        public int IdCountry { get; set; }
        public long? IdCompanyIndicators { get; set; }
    }
}
