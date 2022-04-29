using Dividendos.Toro.Interface.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Toro.Interface
{
    public interface IToroHelper
    {
        ImportToroResult ValidateUser(string user, string password, string token = null);
        public ImportToroResult ImportToro(string user, string password, DateTime? lastEventDate, string token = null);
    }
}
