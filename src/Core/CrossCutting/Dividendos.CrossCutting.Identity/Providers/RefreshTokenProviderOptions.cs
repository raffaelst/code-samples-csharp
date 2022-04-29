using System;

namespace Dividendos.CrossCutting.Identity.Providers
{
    public class RefreshTokenProviderOptions
    {
        public string Name { get; set; } = "RefreshTokenProvider";
        public TimeSpan TokenLifespan { get; set; } = TimeSpan.FromDays(1);
    }
}
