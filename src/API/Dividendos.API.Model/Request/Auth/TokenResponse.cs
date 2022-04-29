using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Dividendos.API.Model.Request.Auth
{
    /// <summary>
    /// TokenResponse
    /// </summary>
    public class TokenResponse
    {
        /// <summary>
        /// UserName
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// RefreshToken
        /// </summary>
        public string RefreshToken { get; set; }
        /// <summary>
        /// Roles
        /// </summary>
        public IEnumerable<string> Roles { get; set; }
        /// <summary>
        /// Claims
        /// </summary>
        public IEnumerable<Claim> Claims { get; set; }
        /// <summary>
        /// expires
        /// </summary>
        public DateTime Expires { get; set; }
    }
}
