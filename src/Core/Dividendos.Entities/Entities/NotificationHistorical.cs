using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("NotificationHistorical")]
    public class NotificationHistorical
    {
        [Key]
 
        public long NotificationHistoricalId { get; set; }

        public string Text { get; set; }
        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Active { get; set; }
        public Guid NotificationHistoricalGuid { get; set; }
        public string UserID { get; set; }
        public string PushRedirectType { get; set; }
        public string AppScreenName { get; set; }
        public string ExternalRedirectURL { get; set; }
    }
}
