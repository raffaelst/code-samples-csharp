using Dividendos.InvestingCom.Interface.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.InvestingCom.Interface
{
    public interface IInvestingComHelper
    {
        List<StockSplitImport> GetSplitsEvents(int idCountry, int year, int month, int lastDay);
        List<InvestingIndicator> GetIndicator();
    }
}
