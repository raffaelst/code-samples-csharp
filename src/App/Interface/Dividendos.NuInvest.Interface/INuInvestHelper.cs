using Dividendos.NuInvest.Interface.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.NuInvest.Interface
{
    public interface INuInvestHelper
    {
        ImportNuInvestResult ImportFromNuInvest(string identifier, string password, string token, DateTime? lastEventDate, bool getContactDetails);
    }
}
