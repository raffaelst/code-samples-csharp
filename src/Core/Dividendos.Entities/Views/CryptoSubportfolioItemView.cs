using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Entity.Views
{
    public class CryptoSubportfolioItemView
    {
        public Guid GuidCryptoTransaction { get; set; }
        public bool Selected { get; set; }
        public string CryptoName { get; set; }
        public string Logo { get; set; }
    }
}
