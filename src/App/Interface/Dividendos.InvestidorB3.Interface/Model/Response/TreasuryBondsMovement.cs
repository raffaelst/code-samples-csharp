using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Dividendos.InvestidorB3.Interface.Model.Response.TreasuryBondsMovement
{
    public class TreasuryBondsMovement
    {
        public string referenceDate { get; set; }
        public string productCategoryName { get; set; }
        public string productTypeName { get; set; }
        public string movementType { get; set; }
        public string operationType { get; set; }
        public string tickerSymbol { get; set; }
        public string assetDescription { get; set; }
        public string participantName { get; set; }
        public string treasuryBondsQuantity { get; set; }
        public string unitPrice { get; set; }
        public string operationValue { get; set; }
    }

    public class TreasuryBondsPeriods
    {
        public string documentNumber { get; set; }
        public string referenceStartDate { get; set; }
        public string referenceEndDate { get; set; }
        public List<TreasuryBondsMovement> treasuryBondsMovements { get; set; }
    }

    public class Data
    {
        public TreasuryBondsPeriods treasuryBondsPeriods { get; set; }
    }


    public class Root
    {
        public Data data { get; set; }
        public Links Links { get; set; }
    }
}
