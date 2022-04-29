using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Entity.Entities
{
    [Table("ProductCategory")]
    public class ProductCategory
    {
        [Key]
        public int ProductCategoryID { get; set; }

        public string Description { get; set; }

        public Guid ProductCategoryGuid { get; set; }
    }
}
