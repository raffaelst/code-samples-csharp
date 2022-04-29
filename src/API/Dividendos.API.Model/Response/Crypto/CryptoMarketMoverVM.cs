using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.API.Model.Response
{
    public class CryptoMarketMoverVM
    {
        public string Logo { get; set; }

        public string Symbol { get; set; }
        public string Description { get; set; }

        public string MarketPrice { get; set; }

        public string PercentChange1h { get; set; }

        public string PercentChange24h { get; set; }
        public string PercentChange7d { get; set; }
        public string PercentChange30d { get; set; }
        public string PercentChange60d { get; set; }
        public string PercentChange90d { get; set; }
    }
}
