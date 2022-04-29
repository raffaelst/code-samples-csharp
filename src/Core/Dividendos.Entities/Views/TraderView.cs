using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Views
{
    public class TraderView
    {
        public long IdTrader { get; set; }
        public string Identifier { get; set; }

        public string Name { get; set; }
        public Guid GuidTrader { get; set; }
        public bool? ShowOnPatrimony { get; set; }
        public long TraderTypeID { get; set; }
    }
}
