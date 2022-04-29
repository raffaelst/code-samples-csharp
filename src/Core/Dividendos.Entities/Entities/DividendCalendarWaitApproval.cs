using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("DividendCalendarWaitApproval")]
    public class DividendCalendarWaitApproval
    {
        [Key]

        public long DividendCalendarWaitApprovalID { get; set; }

        public long IdStock { get; set; }

        public int IdDividendType { get; set; }

        public DateTime DataCom { get; set; }

        public DateTime? PaymentDate { get; set; }


        public decimal Value { get; set; }


        public bool PaymentDatepartiallyUndefined { get; set; }

        public bool PaymentUndefined { get; set; }

        public int IdCountry { get; set; }
    }
}
