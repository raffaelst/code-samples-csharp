using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Xp.Interface.Model
{
    public class XpDividend
    {
        public string Symbol { get; set; }
        public DateTime EventDate { get; set; }
        public decimal NetValue { get; set; }
        public string Type { get; set; }
    }
}
