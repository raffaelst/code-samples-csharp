using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Dividendos.API.Model.Request.Crypto
{
    public class CryptoPortfolioEditVM
    {
        [Required]
        public Guid GuidCryptoPortfolio { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
