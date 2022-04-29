using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.API.Model.Response.BrokerIntegration
{
    public class AvenueAddResponse
    {
        public string Email { get; set; }
        public string Auth { get; set; }
        public string SessionId { get; set; }
        public string Challenge { get; set; }
    }
}
