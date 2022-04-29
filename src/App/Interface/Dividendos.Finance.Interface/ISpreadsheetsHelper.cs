using Dividendos.Finance.Interface.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Finance.Interface
{
    public interface ISpreadsheetsHelper
    {
        ImportFinanceResult ReadEntries();
    }
}
