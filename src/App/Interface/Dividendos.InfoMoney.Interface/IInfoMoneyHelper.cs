using Dividendos.InfoMoney.Interface.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.InfoMoney.Interface
{
    public interface IInfoMoneyHelper
    {
        RelevantFactInfoMoney ImportRelevantFacts(string nonce);
    }
}
