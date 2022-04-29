using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Dividendos.InvestidorB3.Interface.Model.Response.EquitiesPosition
{
    public class Reason
    {
        public string reasonName { get; set; }
        public string collateralQuantity { get; set; }
        public string counterpartName { get; set; }
    }

    public class EquitiesPosition
    {
        public string documentNumber { get; set; }
        public string referenceDate { get; set; }
        public string productCategoryName { get; set; }
        public string productTypeName { get; set; }
        public string markingIndicator { get; set; }
        public string tickerSymbol { get; set; }
        public string corporationName { get; set; }
        public string specificationCode { get; set; }
        public string participantName { get; set; }
        public string equitiesQuantity { get; set; }
        public string closingPrice { get; set; }
        public string updateValue { get; set; }
        public string isin { get; set; }
        public string distributionIdentification { get; set; }
        public string bookkeeperName { get; set; }
        public string availableQuantity { get; set; }
        public string unavailableQuantity { get; set; }
        public string administratorName { get; set; }
        public List<Reason> reasons { get; set; }        
    }

    public class Data
    {
        public List<EquitiesPosition> equitiesPositions { get; set; }
    }

    public class Root
    {
        public Data data { get; set; }
        public Links Links { get; set; }

        /// <summary>
        /// Helper Field
        /// </summary>
        public string ResponseJson { get; set; }
    }
}
