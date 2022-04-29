using System.ComponentModel.DataAnnotations;

namespace Dividendos.API.Model.Request.v2.User
{
    /// <summary>
    /// LoginModel
    /// </summary>
    public class UserVM
    {

        [Required]
        public string Name { get; set; }

        public string PhoneNumber { get; set; }
    }
}
