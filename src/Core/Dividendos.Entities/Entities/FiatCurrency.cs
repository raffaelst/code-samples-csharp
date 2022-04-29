using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("FiatCurrency")]
    public class FiatCurrency
    {
        [Key]
        public int IdFiatCurrency { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Symbol { get; set; }
    }
}
