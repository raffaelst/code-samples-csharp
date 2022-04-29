using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("RelevantFact")]
    public class RelevantFact
    {
        [Key]
        public long RelevantFactID { get; set; }

        public DateTime CreatedDate { get; set; }

        public long CompanyID { get; set; }

        public Guid RelevantFactGuid { get; set; }

        public bool Active { get; set; }

        public string URL { get; set; }

        public bool NotificationSent { get; set; }

        public DateTime ReferenceDate { get; set; }
    }
}
