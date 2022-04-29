using Dapper.Contrib.Extensions;
using Dividendos.Entity.Enum;
using System;

namespace Dividendos.Entity.Views
{
    public class GoalView
    {
        public long GoalID { get; set; }

        public Guid GoalGuid { get; set; }

        public string Name { get; set; }

        public DateTime Limit { get; set; }

        public DateTime CreatedDate { get; set; }

        public decimal Value { get; set; }

        public decimal ValueReached { get; set; }

        public long PortfolioID { get; set; }

        public bool Active { get; set; }

        public decimal PercentageReached { get; set; }

        public int IdCountry { get; set; }
    }
}
