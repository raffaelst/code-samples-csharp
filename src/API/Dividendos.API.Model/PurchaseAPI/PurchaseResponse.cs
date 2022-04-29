using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.API.Model.PurchaseAPI
{
    public class ProductsForSale
    {
        public string id { get; set; }
        public string type { get; set; }
        public string sku { get; set; }
        public string subscriptionPeriodType { get; set; }
        public string group { get; set; }
        public string groupName { get; set; }
    }

    public class ActiveProduct
    {
        public string id { get; set; }
        public string type { get; set; }
        public string sku { get; set; }
        public string purchase { get; set; }
        public DateTime purchaseDate { get; set; }
        public DateTime expirationDate { get; set; }
        public bool isSubscriptionRenewable { get; set; }
        public bool isSubscriptionRetryPeriod { get; set; }
        public string subscriptionPeriodType { get; set; }
        public string groupName { get; set; }
    }

    public class PurchaseResponse
    {
        public IList<ProductsForSale> productsForSale { get; set; }
        public IList<ActiveProduct> activeProducts { get; set; }
    }

}
