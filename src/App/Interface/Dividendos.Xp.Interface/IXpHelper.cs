using Dividendos.Xp.Interface.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Xp.Interface
{
    public interface IXpHelper
    {
        Task<ImportXpResult> ImportFromXptAsync(string account, string password, string xpToken);
    }
}
