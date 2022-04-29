using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Dividendos.InvestidorB3.Interface.Model.Response.FixedIncomePosition
{
    public class Reason
    {
        public string reasonName { get; set; }
        public string collateralQuantity { get; set; }
        public string counterpartName { get; set; }
    }

    public class FixedIncomePosition
    {
        public string documentNumber { get; set; }
        public string referenceDate { get; set; }
        public string productCategoryName { get; set; }
        public string productTypeName { get; set; }
        public string markingIndicator { get; set; }
        public string tickerSymbol { get; set; }
        public string corporationName { get; set; }
        public string participantName { get; set; }
        public string issueDate { get; set; }
        public string expirationDate { get; set; }
        public string fixedIncomeQuantity { get; set; }
        public string updatedUnitPrice { get; set; }
        public string applicationValue { get; set; }
        public string assetDescription { get; set; }
        public string updateValue { get; set; }
        public string updatedUnitPriceMTM { get; set; }
        public string updateValueMTM { get; set; }
        public string indexShortName { get; set; }
        public string fixedIncomeModalityType { get; set; }
        public string availableQuantity { get; set; }
        public string unavailableQuantity { get; set; }
        public List<Reason> reasons { get; set; }
    }

    public class Data
    {
        public List<FixedIncomePosition> fixedIncomePositions { get; set; }
    }

    public class Root
    {
        public Data data { get; set; }
        public Links Links { get; set; }
    }
}
