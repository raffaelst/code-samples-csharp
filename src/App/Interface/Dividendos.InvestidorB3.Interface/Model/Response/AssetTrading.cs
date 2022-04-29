using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Dividendos.InvestidorB3.Interface.Model.Response.AssetTrading
{
    public class AssetTrading
    {
        public string tickerSymbol { get; set; }
        public string side { get; set; }
        public string marketName { get; set; }
        public string expirationDate { get; set; }
        public string participantName { get; set; }
        public string tradeQuantity { get; set; }
        public string priceValue { get; set; }
        public string grossAmount { get; set; }
    }

    public class PeriodList
    {
        public string referenceDate { get; set; }
        public string buyTotal { get; set; }
        public string sellTotal { get; set; }
        public List<AssetTrading> assetTradingList { get; set; }
    }

    public class Periods
    {
        public string documentNumber { get; set; }
        public string referenceStartDate { get; set; }
        public string referenceEndDate { get; set; }
        public List<PeriodList> periodLists { get; set; }
    }

    public class Data
    {
        public Periods periods { get; set; }
    }

    public class Root
    {
        public Data data { get; set; }
        public Links Links { get; set; }
    }
}
