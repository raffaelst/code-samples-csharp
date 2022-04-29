using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.NovaDax.Interface.Model
{
    public class Datum
    {
        public string available { get; set; }
        public string balance { get; set; }
        public string currency { get; set; }
        public string hold { get; set; }
    }

    public class Root
    {
        public string code { get; set; }
        public List<Datum> data { get; set; }
        public string message { get; set; }
    }

}
