using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Entity.Entities
{
    [Table("Product")]
    public class Product
    {
        [Key]
 
        public int ProductID { get; set; }
        public int ProductCategoryID { get; set; }
     
        public string Description { get; set; }
        public Guid? ProductGuid { get; set; }

        public string ExternalName { get; set; }

    }
}
