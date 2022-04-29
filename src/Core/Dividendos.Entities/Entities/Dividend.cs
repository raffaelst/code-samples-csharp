using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("Dividend")]
    public class Dividend
    {
        [Key]
        public long IdDividend { get; set; }
        public long IdStock { get; set; }        
        public long IdPortfolio { get; set; }
        public int IdDividendType { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int BaseQuantity { get; set; }
        public decimal GrossValue { get; set; }
        public decimal NetValue { get; set; }
        public bool NotificationSent { get; set; }
        public Guid GuidDividend { get; set; }
        public string HomeBroker { get; set; }
        public DateTime DateAdded { get; set; }
        public bool Active { get; set; }
        public bool AutomaticImport { get; set; }
        public string TransactionId { get; set; }
    }
}
