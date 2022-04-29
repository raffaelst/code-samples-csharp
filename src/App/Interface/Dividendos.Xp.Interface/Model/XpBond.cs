using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Xp.Interface.Model
{
    public class XpBond
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
        public string Issuer { get; set; }
        public string InvestmenType { get; set; }
    }
}
