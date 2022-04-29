using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Entity.Entities
{
    public class ProductUserView
    {
  
        public long ProductUserID { get; set; }
        public Guid ProductUserGuid { get; set; }
        public int FinancialInstitutionID { get; set; }
        public int ProductCategoryID { get; set; }
        
        public int ProductID { get; set; }
 
        public long TraderID { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal CurrentValue { get; set; }
        public bool Active { get; set; }

        public string ExternalName { get; set; }

        public string FinancialInstitution { get; set; }

        public string ProductName { get; set; }
        public string ProductCategory { get; set; }

        public string UserID { get; set; }

        public string Logo { get; set; }

        public decimal AveragePrice { get; set; }
    }
}
