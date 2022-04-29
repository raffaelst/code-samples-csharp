using System.ComponentModel.DataAnnotations;

namespace Dividendos.API.Model.Request
{
    /// <summary>
    /// LoginModel
    /// </summary>
    public class UserRegisterVM
    {

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }


        [Required]
        public string Password { get; set; }

        public string PhoneNumber { get; set; }
    }
}
