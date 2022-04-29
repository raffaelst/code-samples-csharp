using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Views
{
    public class FollowStockAlertView
    {
        public long FollowStockAlertID { get; set; }
        public Guid FollowStockAlertGuid { get; set; }

        public int FollowStockTypeID { get; set; }
        
        public long FollowStockID { get; set; }


        public decimal TargetPrice { get; set; }
        public bool IsStockType { get; set; }
        public string CustomMessage { get; set; }
        public DateTime CreatedDate { get; set; }

        public bool Reached { get; set; }
        public bool Active { get; set; }
        public DateTime? ReachedDate { get; set; }
    }
}
