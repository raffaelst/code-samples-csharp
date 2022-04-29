using System;
namespace Dividendos.Passfolio.Interface.Model
{
    
    public class Instrument
    {
        public string id { get; set; }
        public string symbol { get; set; }
        public string name { get; set; }
    }

    public class Root
    {
        public int accountAmount { get; set; }
        public int accountBalance { get; set; }
        public string accountType { get; set; }
        public string comment { get; set; }
        public bool dnb { get; set; }
        public string finTranID { get; set; }
        public string finTranTypeID { get; set; }
        public int feeSec { get; set; }
        public int feeTaf { get; set; }
        public int feeBase { get; set; }
        public int feeXtraShares { get; set; }
        public int feeExchange { get; set; }
        public double fillQty { get; set; }
        public double fillPx { get; set; }
        public Instrument instrument { get; set; }
        public string orderID { get; set; }
        public string orderNo { get; set; }
        public bool sendCommissionToInteliclear { get; set; }
        public int systemAmount { get; set; }
        public int tranAmount { get; set; }
        public string tranSource { get; set; }
        public DateTime tranWhen { get; set; }
        public int wlpAmount { get; set; }
        public string wlpFinTranTypeID { get; set; }
    }


}
