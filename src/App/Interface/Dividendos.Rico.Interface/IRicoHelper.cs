using Dividendos.Rico.Interface.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Rico.Interface
{
    public interface IRicoHelper
    {
        ImportRicoResult ImportFromRico(string identifier, string password, string token, DateTime? lastEventDate, bool getContactDetails);
        ImportRicoResult RestoreDividends(string identifier, string password, string token);
    }
}
