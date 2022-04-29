using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Entity.Views
{
    public class DividendView
    {
        public long IdDividend { get; set; }
        public long IdStock { get; set; }
        public int IdStockType { get; set; }
        public string Symbol { get; set; }
        public string DividendType { get; set; }
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
        public decimal NetValueGroup { get; set; }
        public bool Active { get; set; }
        public bool AutomaticImport { get; set; }
        public long IdCountry { get; set; }
        public string IdUser { get; set; }


        public string GroupedNotificationTitle { get; set; }
        public string GroupedNotificationMessage { get; set; }

        public bool AlreadyIncludedToSend { get; set; }
        public string Company { get; set; }
        public string Logo { get; set; }
        public string TransactionId { get; set; }
    }
}
