using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Dividendos.API.Model.Request.Dividend
{
    public class DividendAddVM
    {
        [Required]
        public long IdStock { get; set; }

        [Required]
        public long IdPortfolio { get; set; }

        [Required]
        public string DividendValue { get; set; }

        [Required]
        public string PaymentDate { get; set; }

        [Required]
        public int IdDividendType { get; set; }
    }
}
