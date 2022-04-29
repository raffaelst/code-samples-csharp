using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Dividendos.InvestidorB3.Interface.Model.Response.DerivativesPosition
{
    public class ActiveTip
    {
        public string activeUnderlyngInstrument { get; set; }
        public string activePercent { get; set; }
        public string activeInterestRate { get; set; }
        public string activeValue { get; set; }
    }

    public class PassiveTip
    {
        public string passiveUnderlyngInstrument { get; set; }
        public string passivePercent { get; set; }
        public string passiveInterestRate { get; set; }
        public string passiveValue { get; set; }
    }

    public class Reason
    {
        public string reasonName { get; set; }
        public string collateralQuantity { get; set; }
        public string counterpartName { get; set; }
    }

    public class Call
    {
        public string exercisePrice { get; set; }
        public string referencePrice { get; set; }
        public string optionStyle { get; set; }
        public string expirationCode { get; set; }
    }

    public class Put
    {
        public string exercisePrice { get; set; }
        public string referencePrice { get; set; }
        public string optionStyle { get; set; }
        public string expirationCode { get; set; }
        public string coveredUncoveredCollateral { get; set; }
    }

    public class DerivativesPosition
    {
        public string documentNumber { get; set; }
        public string referenceDate { get; set; }
        public string productCategoryName { get; set; }
        public string productTypeName { get; set; }
        public string markingIndicator { get; set; }
        public string tickerSymbol { get; set; }
        public string participantName { get; set; }
        public string startDate { get; set; }
        public string expirationDate { get; set; }
        public string investorPositionTypeName { get; set; }
        public string derivativeQuantity { get; set; }
        public string updateValue { get; set; }
        public string baseValue { get; set; }
        public string underlyingInstrument { get; set; }
        public string assetDescription { get; set; }
        public string counterpartName { get; set; }
        public string unitPrice { get; set; }
        public string ajustedQuoted { get; set; }
        public string applicationValue { get; set; }
        public string redemptionValue { get; set; }
        public string availableQuantity { get; set; }
        public string unavailableQuantity { get; set; }
        public List<ActiveTip> activeTips { get; set; }
        public List<PassiveTip> passiveTips { get; set; }
        public List<Reason> reasons { get; set; }
        public List<Call> calls { get; set; }
        public List<Put> puts { get; set; }
    }

    public class Data
    {
        public List<DerivativesPosition> derivativesPositions { get; set; }
    }

    public class Root
    {
        public Data data { get; set; }
        public Links Links { get; set; }
    }
}
