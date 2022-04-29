using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("SyncQueue")]
    public class SyncQueue
    {
        [Key]
        public long IdSyncQueue { get; set; }

        public long IdTrader { get; set; }

        public DateTime DateIn { get; set; }

        public bool InUse { get; set; }

        public bool Done { get; set; }
        public int Attempts { get; set; }
        public bool AutomaticImport { get; set; }
        public string ServerName { get; set; }
        public DateTime? ExecutionTime { get; set; }
        public string Message { get; set; }
    }
}
