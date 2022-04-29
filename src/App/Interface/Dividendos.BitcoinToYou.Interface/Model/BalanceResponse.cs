using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.BitcoinToYou.Interface.Model
{
    public class Available
    {
        public string asset { get; set; }
        public decimal amount { get; set; }
    }

    public class Pending
    {
        public string asset { get; set; }
        public string amount { get; set; }
    }

    public class Root
    {
        public List<Available> available { get; set; }
        public List<Pending> pending { get; set; }

        public bool InvalidCredencial { get; set; }
    }
}
