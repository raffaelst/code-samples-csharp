using Dividendos.API.Model.Request.FollowStock;
using Dividendos.API.Model.Request.Stock;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.API.Model.Response
{
    public class FollowStockAlertResponseVM
    {
        public Guid FollowStockAlertGuid { get; set; }
        public bool Active { get; set; }

        public bool Reached { get; set; }
        

        public DateTime CreatedDate { get; set; }
        public DateTime? ReachedDate { get; set; }

        public string TargetPrice { get; set; }

        public string CustomMessage { get; set; }

        public FollowStockType FollowStockType { get; set; }

    }
}
