using Newtonsoft.Json;
using System;

namespace Dividendos.InvestidorB3.Interface.Model.Response
{

    public class Links
    {
        public string self { get; set; }
        public string first { get; set; }
        public string prev { get; set; }
        public string next { get; set; }
        public string last { get; set; }
    }
}
