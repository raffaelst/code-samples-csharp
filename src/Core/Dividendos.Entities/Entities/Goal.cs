using Dapper.Contrib.Extensions;
using Dividendos.Entity.Enum;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("Goal")]
    public class Goal
    {
        [Key]
        public long GoalID { get; set; }

        public Guid GoalGuid { get; set; }

        public string Name { get; set; }

        public DateTime Limit { get; set; }

        public DateTime CreatedDate { get; set; }

        public decimal Value { get; set; }

        public long PortfolioID { get; set; }

        public bool Active { get; set; }
    }
}
