using System;
using System.Collections.Generic;

namespace Dividendos.Passfolio.Interface.Model
{
    public class SessionMFARequest
    {
        public string code { get; set; }
        public string authenticatorId { get; set; }
    }
}
