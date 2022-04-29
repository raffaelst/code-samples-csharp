using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("ScrapyScheduler")]
    public class ScrapyScheduler
    {
        [Key]
        public long IdScrapyScheduler { get; set; }
        public Guid ScrapySchedulerGuid { get; set; }
        public string IdUser { get; set; }
        public string Identifier { get; set; }
        public string Password { get; set; }
        public bool AutomaticImport { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }
        public bool Sent { get; set; }
        public bool TimedOut { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public DateTime? ExecutionTime { get; set; }
        public string Results { get; set; }
        public string Agent { get; set; }
        public DateTime? WaitingTime { get; set; }
        public bool? ScrapyAgentRun { get; set; }
        public bool? NewB3 { get; set; }
        public long IdTraderType { get; set; }
        public string ResponseBody { get; set; }
        public string ActionType { get; set; }
    }
}
