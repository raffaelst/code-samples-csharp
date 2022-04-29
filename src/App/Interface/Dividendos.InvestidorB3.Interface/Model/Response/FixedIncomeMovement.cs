using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Dividendos.InvestidorB3.Interface.Model.Response.FixedIncomeMovement
{
    public class FixedIncomeMovement
    {
        public string referenceDate { get; set; }
        public string productCategoryName { get; set; }
        public string productTypeName { get; set; }
        public string movementType { get; set; }
        public string operationType { get; set; }
        public string tickerSymbol { get; set; }
        public string corporationName { get; set; }
        public string expirationDate { get; set; }
        public string participantName { get; set; }
        public string fixedIncomeQuantity { get; set; }
        public string unitPrice { get; set; }
        public string operationValue { get; set; }
        public string assetDescription { get; set; }
        public string updatedUnitPriceMTM { get; set; }
        public string updateValueMTM { get; set; }
    }

    public class FixedIncomes
    {
        public string documentNumber { get; set; }
        public string referenceStartDate { get; set; }
        public string referenceEndDate { get; set; }
        public List<FixedIncomeMovement> fixedIncomeMovements { get; set; }
    }

    public class Data
    {
        public FixedIncomes fixedIncomes { get; set; }
    }

    public class Root
    {
        public Data data { get; set; }
        public Links Links { get; set; }
    }
}
