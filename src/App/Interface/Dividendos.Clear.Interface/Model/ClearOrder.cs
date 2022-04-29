using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Clear.Interface.Model
{
    public class ClearOrder
    {
        public decimal AveragePrice { get; set; }
        public decimal NumberOfShares { get; set; }
        public string Symbol { get; set; }
        public DateTime EventDate { get; set; }
        public int IdOperationType { get; set; }
    }
}
