using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("Portfolio")]
    public class Portfolio
    {
        [Key]
        public long IdPortfolio { get; set; }

        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Active { get; set; }
        public Guid GuidPortfolio { get; set; }
        public long IdTrader { get; set; }
        public bool ManualPortfolio { get; set; }
        public int IdCountry { get; set; }

        public DateTime CalculatePerformanceDate { get; set; }
        public bool? RestoredDividends { get; set; }

    }
}
