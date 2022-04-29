using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.API.Model.Response.Purchase
{
    public class Subscribe
    {
        public DateTime ValidUntil { get; set; }
        public bool Active { get; set; }

        public string ProductIdentifier { get; set; }
        public long SubscriptionID { get; set; }
        public long? PartnerID { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
