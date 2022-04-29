using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Entity.Entities
{
    [Table("DividendType")]
    public class DividendType
    {
        [Key]
        public int IdDividendType { get; set; }
        public string Name { get; set; }
        public string NameB3 { get; set; }
        public string NameNewB3 { get; set; }
        public string NameNewB3Copy { get; set; }
    }
}
