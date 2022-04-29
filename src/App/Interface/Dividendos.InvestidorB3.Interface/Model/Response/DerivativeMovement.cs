using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Dividendos.InvestidorB3.Interface.Model.Response.DerivativeMovement
{
    public class Derivative
    {
        public string referenceDate { get; set; }
        public string productCategoryName { get; set; }
        public string productTypeName { get; set; }
        public string movementType { get; set; }
        public string operationType { get; set; }
        public string tickerSymbol { get; set; }
        public string underlyingInstrument { get; set; }
        public string quotedCurrency { get; set; }
        public string baseCurrency { get; set; }
        public string derivativeVariable { get; set; }
        public string derivativeCounterpartVariable { get; set; }
        public string expirationDate { get; set; }
        public string exercisePrice { get; set; }
        public string participantName { get; set; }
        public string derivativeQuantity { get; set; }
        public string unitPrice { get; set; }
        public string operationValue { get; set; }
    }

    public class DerivativesPeriods
    {
        public string documentNumber { get; set; }
        public string referenceStartDate { get; set; }
        public string referenceEndDate { get; set; }
        public List<Derivative> derivatives { get; set; }
    }

    public class Data
    {
        public DerivativesPeriods derivativesPeriods { get; set; }
    }

    public class Root
    {
        public Data data { get; set; }
        public Links Links { get; set; }
    }


}
