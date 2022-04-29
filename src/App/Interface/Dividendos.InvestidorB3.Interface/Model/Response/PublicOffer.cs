using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Dividendos.InvestidorB3.Interface.Model.Response.PublicOffer
{

    public class PublicOffer
    {
        public string referenceDate { get; set; }
        public string tickerSymbol { get; set; }
        public string corporationName { get; set; }
        public string specificationCode { get; set; }
        public string publicOfferName { get; set; }
        public string isin { get; set; }
        public string participantName { get; set; }
        public int allocatedQuantity { get; set; }
        public string publicOfferPrice { get; set; }
        public string maximumPrice { get; set; }
        public string totalPublicOfferPrice { get; set; }
        public string publicOfferModality { get; set; }
        public int reserveQuantity { get; set; }
        public string reservePrice { get; set; }
        public string settlementDate { get; set; }
    }

    public class Periods
    {
        public string documentNumber { get; set; }
        public string referenceStartDate { get; set; }
        public string referenceEndDate { get; set; }
        public List<PublicOffer> publicOffers { get; set; }
    }

    public class Data
    {
        public Periods periods { get; set; }
    }

    public class Root
    {
        public Data data { get; set; }
        public Links Links { get; set; }
    }
}
