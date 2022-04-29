using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Toro.Interface.Model
{
    public class ToroDividend
    {
        public string Symbol { get; set; }
        public DateTime EventDate { get; set; }
        public decimal NetValue { get; set; }
        public string TransactionId { get; set; }
    }
}
