using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Dividendos.API.Model.Request.Auth
{
    /// <summary>
    /// ChangePassword
    /// </summary>
    public class ChangePassword
    {
        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
