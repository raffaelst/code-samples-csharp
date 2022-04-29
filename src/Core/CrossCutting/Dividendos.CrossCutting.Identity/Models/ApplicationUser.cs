using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using Dapper.Contrib.Extensions;

namespace Dividendos.CrossCutting.Identity.Models
{
    [Table("AspNetUsers")]
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [PersonalData]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [DataType(DataType.Text)]
        public string RecoveryPasswordToken { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? LastAccess { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? Created { get; set; }

        [PersonalData]
        [DataType(DataType.Text)]
        public string InfluencerAffiliatorGuid { get; set; }

        [PersonalData]
        public bool? Excluded { get; set; }

    }
}
