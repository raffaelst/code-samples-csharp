using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.API.Model.Response.Purchase
{
    public class ProductVM
    {
        public string Name { get; set; }
        public string Price { get; set; }
        public string OldPrice { get; set; }
        public string Type { get; set; }
        public string Sku { get; set; }
        public string Discount { get; set; }

        public string PartnerGuid { get; set; }
    }
}
