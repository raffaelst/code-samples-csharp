using System;
using System.Collections.Generic;

namespace Dividendos.Passfolio.Interface.Model
{
    public class AuthenticatorResponse
    {
        public string id { get; set; }
        public bool verified { get; set; }
        public string type { get; set; }
        public string description { get; set; }
    }
}
