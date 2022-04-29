using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Rico.Interface.Model
{
    public class RicoDividend
    {
        public string Symbol { get; set; }
        public DateTime? EventDate { get; set; }
        public decimal NetValue { get; set; }
        public string Type { get; set; }
        public decimal GrossValue { get; set; }
        public int BaseQuantity { get; set; }
    }
}
