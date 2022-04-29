using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Entity.Views
{
    public class InfluencerView
    {
        public string Afiliate { get; set; }
        public string MaskedMail { get; set; }
        public bool HasSubscription { get; set; }
        public string SubscriptionType { get; set; }
        public DateTime? SubscriptionCreatedDate { get; set; }
        public string UserID { get; set; }
        public string PartnerID { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
