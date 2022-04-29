using Dapper.Contrib.Extensions;
using System;


namespace Dividendos.Entity.Entities
{
    [Table("OperationItem")]
    public class OperationItem
    {
        [Key]
        public long IdOperationItem { get; set; }
        public long IdOperation { get; set; }
        public long IdStock { get; set; }
        public int IdOperationType { get; set; }
        public DateTime? EventDate { get; set; }
        public decimal NumberOfShares { get; set; }
        public decimal AveragePrice { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public Guid GuidOperationItem { get; set; }
        public string HomeBroker { get; set; }
        public string Market { get; set; }
        public string Expire { get; set; }
        public string StockSpec { get; set; }
        public string Factor { get; set; }
        public bool PriceAdjust { get; set; }
        public bool Active { get; set; }
        public decimal AcquisitionPrice { get; set; }
        public bool PriceAdjustNew { get; set; }
        public bool EditedByUser { get; set; }
        public string TransactionId { get; set; }
        public bool SplitApplied { get; set; }
        public DateTime LastSplitDate { get; set; }
    }
}
