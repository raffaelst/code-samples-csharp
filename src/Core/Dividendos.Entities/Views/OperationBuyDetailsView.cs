using System;
namespace Dividendos.Entity.Views
{
    public class OperationBuyDetailsView
    {
        public Guid GuidOperationItem { get; set; }
        public long IdOperationItem { get; set; }
        public DateTime? EventDate { get; set; }
        public decimal NumberOfShares { get; set; }
        public decimal AveragePrice { get; set; }
        public long IdCompany { get; set; }
        public long IdPortfolio { get; set; }
        public string Company { get; set; }
        public long IdStock { get; set; }
        public string Symbol { get; set; }
        public string Logo { get; set; }
        public string Segment { get; set; }
        public string HomeBroker { get; set; }
    }
}
