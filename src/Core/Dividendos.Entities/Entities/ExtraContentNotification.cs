using Dapper.Contrib.Extensions;
using Dividendos.Entity.Enum;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("ExtraContentNotification")]
    public class ExtraContentNotification
    {
        [Key]
        public long ExtraContentNotificationID { get; set; }

        public int ExtraContentNotificationTypeID { get; set; }

        public bool Complete { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public DateTime NotStartUntil { get; set; }
        
        public int TotalAmountOfRegisters { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        
        public int AgentIterationSequence { get; set; }


        public string PushRedirectType { get; set; }


        public string AppScreenName { get; set; }

        public PushTargetTypeEnum PushTargetType { get; set; }

        public string ExternalRedirectURL { get; set; }
        
    }
}
