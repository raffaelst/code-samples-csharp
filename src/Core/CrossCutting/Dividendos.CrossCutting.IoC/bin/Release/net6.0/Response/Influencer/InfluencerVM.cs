using System;
namespace Dividendos.API.Model.Response.Influencer
{
    public class InfluencerVM
    {
        public string Afiliate { get; set; }
        public string MaskedMail { get; set; }
        public bool HasSubscription { get; set; }
        public string SubscriptionType { get; set; }
        public string UserID { get; set; }
        public string PartnerID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? SubscriptionCreatedDate { get; set; }
    }
}

