using Dividendos.Bacen.Interface.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Bacen.Interface
{
    public interface IImportBacenHelper
    {
        IEnumerable<Indicator> ImportIndicatorsAsync();
    }
}
