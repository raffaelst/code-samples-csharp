using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("CeiLog")]
    public class CeiLog
    {
        [Key]
        public long IdCeiLog { get; set; }

        public long? IdTrader { get; set; }
        public DateTime? ExecutionTime { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Message { get; set; }
        public string Identifier { get; set; }
        public string Password { get; set; }
    }
}
