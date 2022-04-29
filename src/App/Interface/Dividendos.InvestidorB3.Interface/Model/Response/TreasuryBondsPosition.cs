using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Dividendos.InvestidorB3.Interface.Model.Response.TreasuryBondsPosition
{
    public class TreasuryBondsPosition
    {
        public string documentNumber { get; set; }
        public string referenceDate { get; set; }
        public string productCategoryName { get; set; }
        public string productTypeName { get; set; }
        public string markingIndicator { get; set; }
        public string tickerSymbol { get; set; }
        public string assetDescription { get; set; }
        public string participantName { get; set; }
        public string expirationDate { get; set; }
        public string treasuryBondsQuantity { get; set; }
        public string applicationValue { get; set; }
        public string updateValue { get; set; }
        public string isin { get; set; }
        public string interestRate { get; set; }
        public string indexShortName { get; set; }
        public string grossAmount { get; set; }
        public string netValue { get; set; }
    }

    public class Data
    {
        public List<TreasuryBondsPosition> treasuryBondsPositions { get; set; }
    }

    public class Links
    {
        public string self { get; set; }
        public string first { get; set; }
        public string prev { get; set; }
        public string next { get; set; }
        public string last { get; set; }
    }

    public class Root
    {
        public Data data { get; set; }
        public Links Links { get; set; }
    }
}
