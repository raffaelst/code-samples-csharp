using Dividendos.Clear.Interface.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Clear.Interface
{
    public interface IClearHelper
    {
        ImportClearResult ImportFromClear(string identifier, string birthDate, string password, DateTime? lastEventDate, bool getContactDetails, bool getDividends);
        ImportClearResult RestoreDividends(string identifier, string birthDate, string password, DateTime? lastEventDate);
    }
}
