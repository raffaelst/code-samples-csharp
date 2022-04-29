using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Dividendos.API.Model.Request.Dividend
{
    public class DividendEditVM
    {
        [Required]
        public long IdDividend { get; set; }

        [Required]
        public string DividendValue { get; set; }

        [Required]
        public string PaymentDate { get; set; }
    }
}
