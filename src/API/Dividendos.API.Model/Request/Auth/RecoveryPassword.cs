using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Dividendos.API.Model.Request.Auth
{
    /// <summary>
    /// ResetPassword
    /// </summary>
    public class RecoveryPassword
    {
        public string Email { get; set; }
    }
}
