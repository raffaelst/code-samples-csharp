using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Dividendos.InvestidorB3.Interface.Model.Response.EquitiesMovement
{
    public class EquitiesMovement
    {
        public string referenceDate { get; set; }
        public string productCategoryName { get; set; }
        public string productTypeName { get; set; }
        public string movementType { get; set; }
        public string operationType { get; set; }
        public string tickerSymbol { get; set; }
        public string corporationName { get; set; }
        public string participantName { get; set; }
        public string equitiesQuantity { get; set; }
        public string unitPrice { get; set; }
        public string operationValue { get; set; }
    }

    public class EquitiesPeriods
    {
        public string documentNumber { get; set; }
        public string referenceStartDate { get; set; }
        public string referenceEndDate { get; set; }
        public List<EquitiesMovement> equitiesMovements { get; set; }
    }

    public class Data
    {
        public EquitiesPeriods equitiesPeriods { get; set; }
    }

    public class Root
    {
        public Data data { get; set; }
        public Links Links { get; set; }
    }
}
