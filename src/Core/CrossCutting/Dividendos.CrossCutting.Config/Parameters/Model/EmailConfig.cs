using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.CrossCutting.Config.Model
{
    public class EmailConfig
    {
        public string Server { get; set; }
        public string User { get; set; }
        public string Name { get; set; }
        public string Pass { get; set; }
        public string Port { get; set; }
    }
}