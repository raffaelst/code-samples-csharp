using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("SubPortfolioOperation")]
    public class SubPortfolioOperation
    {
        [Key]
        public long IdSubPortfolioOperation { get; set; }
        public long IdSubPortfolio { get; set; }
        public long IdOperation { get; set; }
    }
}
