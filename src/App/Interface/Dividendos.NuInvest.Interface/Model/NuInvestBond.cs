using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.NuInvest.Interface.Model
{
    public class NuInvestBond
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
        public string Issuer { get; set; }
        public string InvestmenType { get; set; }
    }
}
