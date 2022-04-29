using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("StockType")]
    public class StockType
    {
        [Key]
        public int IdStockType { get; set; }
   
        public string Name { get; set; }
        public Guid GuidStockType { get; set; }
    }
}
