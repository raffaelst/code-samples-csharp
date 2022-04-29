using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.InvestidorB3.Config
{
    public class ImportInvestidorB3Config
    {
        public string URLBasePortalInvestidorB3 { get; set; }
        public string UrlEndPointLogin { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scope { get; set; }
        public string PasswordCertificate { get; set; }
        public string URLAuthB3 { get; set; }
    }
}
