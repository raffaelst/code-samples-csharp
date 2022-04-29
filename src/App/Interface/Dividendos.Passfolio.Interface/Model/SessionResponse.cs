using System;
using System.Collections.Generic;

namespace Dividendos.Passfolio.Interface.Model
{
    public class SessionResponse
    {
        public bool mfaRequired { get; set; }
        public string token { get; set; }
    }
}
