using Newtonsoft.Json;
using System;

namespace Dividendos.RDStation.Interface.Model.Request
{

    public enum EventType
    {
        AccountCreated,
        TrialSubscriptionStarted,
        TrialSubscriptionEnded,
        GoldSubscriptionStarded,
        GoldSubscriptionEnded,
        PartnerToroSubscription,
        IntegrationEnableB3,
        IntegrationEnableBinance,
        IntegrationEnablePassfolio,
        IntegrationEnableAvenue,
        IntegrationEnableNuInvest,
        IntegrationEnableToro,
        IntegrationEnableMercadoBitcoin,
        IntegrationEnableCoinbase,
        IntegrationEnableBitcoinToYou,
        IntegrationEnableBitcoinTrade,
        SupportRequestHelpPage,
        SupportRequestChat
    }
}
