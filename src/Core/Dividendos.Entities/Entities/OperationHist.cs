using Dapper.Contrib.Extensions;
using System;


namespace Dividendos.Entity.Entities
{
    [Table("OperationHist")]
    public class OperationHist
    {
        [Key]
        public long IdOperationHist { get; set; }
        public long IdStock { get; set; }
        public long IdPortfolio { get; set; }
        public decimal NumberOfShares { get; set; }
        public decimal AveragePrice { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public Guid GuidOperationHist { get; set; }
        public int IdOperationType { get; set; }
        public bool Active { get; set; }
    }
}
