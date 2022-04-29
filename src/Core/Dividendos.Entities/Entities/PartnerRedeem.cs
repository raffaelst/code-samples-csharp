using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("PartnerRedeem")]
    public class PartnerRedeem
    {
        [Key]
        public long PartnerRedeemID { get; set; }

        public long PartnerID { get; set; }

        public string UserID { get; set; }

        public DateTime Created { get; set; }
   
        public DateTime? PartnerConfirmation { get; set; }

        public DateTime? UserConfirmation { get; set; }

        public Guid PartnerRedeemGuid { get; set; }
    }
}
