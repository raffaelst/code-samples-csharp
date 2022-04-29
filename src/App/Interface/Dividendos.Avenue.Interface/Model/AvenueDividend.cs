using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Avenue.Interface.Model
{
    public class AvenueDividend
    {
        public string Symbol { get; set; }
        public DateTime EventDate { get; set; }
        public decimal NetValue { get; set; }
        public string TransactionId { get; set; }
    }
}
