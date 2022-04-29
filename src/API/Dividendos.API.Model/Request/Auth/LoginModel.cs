using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Dividendos.API.Model.Request.Auth
{
    /// <summary>
    /// LoginModel
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// GrantType
        /// </summary>
        [Required]
        [JsonProperty("grant_type")]
        public string GrantType { get; set; }
        /// <summary>
        /// Login
        /// </summary>
        [JsonProperty("username")]
        public string UserName { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        [JsonProperty("password")]
        public string Password { get; set; }
        /// <summary>
        /// RefreshToken
        /// </summary>
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
