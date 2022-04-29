using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Dividendos.InvestidorB3.Interface.Model.Response.ProvisionedEvent
{
    public class Reason
    {
        public string reasonName { get; set; }
        public string collateralQuantity { get; set; }
        public string counterpartName { get; set; }
    }

    public class ProvisionedEvent
    {
        public string documentNumber { get; set; }
        public string referenceDate { get; set; }
        public string productCategoryName { get; set; }
        public string productTypeName { get; set; }
        public string corporateActionTypeDescription { get; set; }
        public string paymentDate { get; set; }
        public string participantName { get; set; }
        public string eventValue { get; set; }
        public string eventQuantity { get; set; }
        public string netValue { get; set; }
        public string tickerSymbol { get; set; }
        public string isin { get; set; }
        public string distributionIdentification { get; set; }
        public string bookkeeperName { get; set; }
        public string corporationName { get; set; }
        public string specificationCode { get; set; }
        public string approvalDate { get; set; }
        public string updateDate { get; set; }
        public string specialExDate { get; set; }
        public string incomeTaxPercent { get; set; }
        public string incomeTaxAmount { get; set; }
        public string grossAmount { get; set; }
        public string availableQuantity { get; set; }
        public string unavailableQuantity { get; set; }
        public List<Reason> reasons { get; set; }
    }

    public class Data
    {
        public List<ProvisionedEvent> provisionedEvents { get; set; }
    }

    public class Root
    {
        public Data data { get; set; }
        public Links Links { get; set; }
    }


}
