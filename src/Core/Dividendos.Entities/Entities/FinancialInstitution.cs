using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Entity.Entities
{
    [Table("FinancialInstitution")]
    public class FinancialInstitution
    {

        [Key]
   
        public int FinancialInstitutionID { get; set; }

        public string ExternalCode { get; set; }
 
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid FinancialInstitutionGuid { get; set; }

    }
}
