using System;
using System.ComponentModel.DataAnnotations;

namespace Dividendos.API.Model.Request
{
    /// <summary>
    /// LoginModel
    /// </summary>
    public class UserRegisterAffiliateVM
    {

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }


        [Required]
        public string Password { get; set; }

        [Required]
        public string AffiliateGuid { get; set; }
    }
}
