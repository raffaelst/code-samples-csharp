using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Dividendos.API.Model.Request.Auth
{
    /// <summary>
    /// ResetPassword
    /// </summary>
    public class ResetPassword
    {
        public string Token { get; set; }

        public string Email { get; set; }

        public string NewPassword { get; set; }
    }
}
