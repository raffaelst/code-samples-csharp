using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Dividendos.InvestidorB3.Interface.Model.Response.SecurityLendingMovement
{
    public class SecurityLendingMovement
    {
        public string referenceDate { get; set; }
        public string productCategoryName { get; set; }
        public string productTypeName { get; set; }
        public string movementType { get; set; }
        public string operationType { get; set; }
        public string tickerSymbol { get; set; }
        public string contractNumber { get; set; }
        public string corporationName { get; set; }
        public string sideLenderBorrowerName { get; set; }
        public string securityLendingModality { get; set; }
        public string participantName { get; set; }
        public string securirtiesLendingQuantity { get; set; }
        public string unitPrice { get; set; }
        public string operationValue { get; set; }
    }

    public class SecuritiesLendingPeriod
    {
        public string documentNumber { get; set; }
        public string referenceStartDate { get; set; }
        public string referenceEndDate { get; set; }
        public List<SecurityLendingMovement> securityLendingMovements { get; set; }
    }

    public class Data
    {
        public SecuritiesLendingPeriod securitiesLendingPeriod { get; set; }
    }

    public class Root
    {
        public Links Links { get; set; }
        public Data data { get; set; }
    }
}
