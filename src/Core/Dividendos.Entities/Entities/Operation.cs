using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("Operation")]
    public class Operation
    {
        [Key]
        public long IdOperation { get; set; }
        public long IdStock { get; set; }
        public long IdPortfolio { get; set; }
        public decimal NumberOfShares { get; set; }
        public decimal AveragePrice { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public Guid GuidOperation { get; set; }
        public string HomeBroker { get; set; }
        public int IdOperationType { get; set; }
        public bool Active { get; set; }
    }
}
