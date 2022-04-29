using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Dividendos.InvestidorB3.Interface.Model.Response.UpdatedProduct
{
    public class UpdatedProduct
    {
        public string documentNumber { get; set; }
    }

    public class Data
    {
        public string product { get; set; }
        public string referenceStartDate { get; set; }
        public string referenceEndDate { get; set; }
        public List<UpdatedProduct> updatedProducts { get; set; }
    }

    public class Links
    {
        public string next { get; set; }
        public string self { get; set; }
        public string first { get; set; }
        public string last { get; set; }
    }

    public class Root
    {
        public Data data { get; set; }
        public Links links { get; set; }
    }
}
