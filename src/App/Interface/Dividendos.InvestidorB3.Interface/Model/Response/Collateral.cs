using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Dividendos.InvestidorB3.Interface.Model.Response.Collateral
{

    public class Product
    {
        public string productName { get; set; }
        public string collateralType { get; set; }
        public string holder { get; set; }
        public string collateralQuantity { get; set; }
        public string unitPrice { get; set; }
        public string collateralValue { get; set; }
        public string tickerSymbol { get; set; }
    }

    public class Participant
    {
        public string participantName { get; set; }
        public string totalCollateral { get; set; }
        public string riskWithoutCollateral { get; set; }
        public List<Product> products { get; set; }
    }

    public class Collateral
    {
        public string documentNumber { get; set; }
        public string referenceDate { get; set; }
        public List<Participant> participants { get; set; }
    }

    public class Data
    {
        public List<Collateral> collaterals { get; set; }
    }

    public class Root
    {
        public Data data { get; set; }
        public Links Links { get; set; }
    }
}
