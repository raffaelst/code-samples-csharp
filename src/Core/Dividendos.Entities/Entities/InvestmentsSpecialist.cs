using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("InvestmentsSpecialist")]
    public class InvestmentsSpecialist
    {
        [Key]
        public long InvestmentsSpecialistID { get; set; }
        
        public string Name { get; set; }
      
        public string Bio { get; set; }

        public Guid InvestmentsSpecialistGuid { get; set; }
    }
}
