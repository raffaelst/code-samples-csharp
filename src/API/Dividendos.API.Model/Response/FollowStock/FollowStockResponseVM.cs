using Dividendos.API.Model.Request.Stock;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.API.Model.Response
{
    public class FollowStockResponseVM
    {
        public Guid FollowStockGuid { get; set; }
        public bool Active { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Symbol { get; set; }

        public string CompanyName { get; set; }

        public string MarketPrice { get; set; }

        public string PerformancePerc { get; set; }

        public string Logo { get; set; }

        public CountryType CountryType { get; set; }

        public bool IsStockType { get; set; }

    }
}
