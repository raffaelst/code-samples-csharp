using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("ScrapyAgent")]
    public class ScrapyAgent
    {
        [Key]
        public long IdScrapyAgent { get; set; }
        public long IdTrader { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public DateTime? ExecutionTime { get; set; }
    }
}
