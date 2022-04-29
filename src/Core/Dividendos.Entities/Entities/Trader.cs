using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("Trader")]
    public class Trader
    {
        [Key]
        public long IdTrader { get; set; }

        public string IdUser { get; set; }

        public string Identifier { get; set; }
 
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Active { get; set; }
        public Guid GuidTrader { get; set; }

        public DateTime LastSync { get; set; }
        public bool BlockedCei { get; set; }
        public bool ManualPortfolio { get; set; }

        public long TraderTypeID { get; set; }

        public bool ShowOnPatrimony { get; set; }
    }
}
