using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Dividendos.API.Model.Request.Crypto
{
    public enum CryptoMakertMoversType
    {
        [EnumMember(Value = "Gainers")]
        Gainers = 1,
        [EnumMember(Value = "Losers")]
        Losers = 2
    }
}