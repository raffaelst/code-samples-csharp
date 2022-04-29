using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.CoinMarketCap.Interface.Model
{
    public class TickerCrypto
    {
        public decimal Price { get; set; }

        public decimal Volume24h { get; set; }

        public decimal PercentChange1h { get; set; }
        public decimal PercentChange24h { get; set; }
        public decimal PercentChange7d { get; set; }
        public decimal PercentChange30d { get; set; }
        public decimal PercentChange60d { get; set; }

        public decimal PercentChange90d { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }

        public string From { get; set; }
        
        public TickerCrypto()
        {
            From = "CoinMarketCap";
        }
    }
}
