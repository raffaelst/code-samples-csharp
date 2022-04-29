using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Dividendos.InvestidorB3.Interface.Model.Response.SecurityLendingPosition
{
    public class SecurityLendingPosition
    {
        public string documentNumber { get; set; }
        public string referenceDate { get; set; }
        public string productCategoryName { get; set; }
        public string productTypeName { get; set; }
        public string markingIndicator { get; set; }
        public string tickerSymbol { get; set; }
        public string corporationName { get; set; }
        public string sideLenderBorrowerName { get; set; }
        public string participantName { get; set; }
        public string lenderBorrowerRate { get; set; }
        public string lenderBorrowerComissionRate { get; set; }
        public string securitiesLendingQuantity { get; set; }
        public string closingPrice { get; set; }
        public string updateValue { get; set; }
        public string issueDate { get; set; }
        public string expirationDate { get; set; }
        public string contractNumber { get; set; }
        public string tenderEarlySettlementIndicator { get; set; }
        public string earlySettlementIndicator { get; set; }
    }

    public class Data
    {
        public List<SecurityLendingPosition> securityLendingPositions { get; set; }
    }

    public class Root
    {
        public Data data { get; set; }
        public Links Links { get; set; }
    }
}
