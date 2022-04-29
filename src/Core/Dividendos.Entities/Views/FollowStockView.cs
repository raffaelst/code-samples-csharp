using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Views
{
    public class FollowStockView
    {
        public long FollowStockID { get; set; }

        public int FollowStockTypeID { get; set; }

        public Guid FollowStockGuid { get; set; }
        public Guid FollowStockAlertGuid { get; set; }
        public bool Active { get; set; }

        public string UserID { get; set; }
        public long StockID { get; set; }
        public DateTime CreatedDate { get; set; }

        public decimal MarketPrice { get; set; }

        public decimal TargetPrice { get; set; }

        public decimal LastChangePerc { get; set; }

        public string Symbol { get; set; }

        public string CompanyName { get; set; }
        public string CustomMessage { get; set; }
        public string Logo { get; set; }

        public int IdCountry { get; set; }

        public bool IsStockType { get; set; }
        
    }
}
