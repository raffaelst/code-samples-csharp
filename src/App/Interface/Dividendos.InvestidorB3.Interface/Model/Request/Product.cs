using Newtonsoft.Json;
using System;

namespace Dividendos.InvestidorB3.Interface.Model.Request
{

    public enum Product
    {
        ProvisionedEvents,
        Collaterals, AssetsTrading,
        PublicOffers, DerivativesMovement,
        SecuritiesLendingMovement,
        FixedIncommeMovement,
        EquitiesMovement,
        TreasuryBondsMovement,
        DerivativesPosition,
        SecuritiesLendingPosition,
        FixedIncommePosition, EquitiesPosition,
        TreasuryBondsPosition
    }
}
