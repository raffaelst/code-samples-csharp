using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.API.Model.Request.Crypto
{
    public class CryptoSubportfolioVM
    {
        public Guid GuidCryptoSubportfolio { get; set; }
        public Guid GuidCryptoPortfolio { get; set; }

        [Required]
        public string Name { get; set; }

        public List<Guid> TransactionsGuid { get; set; }
    }
}
