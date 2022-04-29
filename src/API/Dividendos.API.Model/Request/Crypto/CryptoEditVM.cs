using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.API.Model.Request.Crypto
{
    public class CryptoEditVM
    {
        [Required]
        public Guid GuidCryptoTransactionItem { get; set; }

        [Required]
        public Guid GuidCryptoCurrency { get; set; }

        [Required]
        public string EventDate { get; set; }

        [Required]
        public string Price { get; set; }

        [Required]
        public string Quantity { get; set; }

        public string Exchange { get; set; }

        public string AcquisitionPrice { get; set; }
    }
}
