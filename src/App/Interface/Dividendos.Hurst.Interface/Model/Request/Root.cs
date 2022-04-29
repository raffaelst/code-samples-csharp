using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Dividendos.Hurst.Interface.Model.Request
{
    public class Root
    {
        public Data data { get; set; }
    }

    public class Data
    {
        public string email { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string tag { get; set; }
        public string source { get; set; }
    }
}
