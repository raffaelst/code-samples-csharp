namespace Dividendos.CrossCutting.Identity.Models
{
    public class TokenConfigurations
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public string Key { get; set; }
        public int HoursToExpireToken { get; set; }
        public int HoursToExpireRefreshToken { get; set; }
    }

    public class RefreshTokenData
    {
        public string RefreshToken { get; set; }
        public string UserID { get; set; }
    }
}
