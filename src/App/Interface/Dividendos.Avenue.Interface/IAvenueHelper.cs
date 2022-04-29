using Dividendos.Avenue.Interface.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Avenue.Interface
{
    public interface IAvenueHelper
    {
        Task<ImportAvenueResult> ImportAvenue(string user, string password, string token, string challenge, string sessiondId, DateTime? lastEventDate, bool getContactDetails);
        Task<ImportAvenueResult> ValidateUser(string user, string password);
    }
}
