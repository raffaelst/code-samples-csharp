using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Finance.Interface.Model
{
    public class ImportFinanceResult
    {
        public List<Stock> ListStock { get; set; }
        public List<Indicator> ListIndicator { get; set; }
    }
}
