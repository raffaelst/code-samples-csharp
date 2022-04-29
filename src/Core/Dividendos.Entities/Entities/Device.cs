using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("Device")]
    public class Device
    {
        [Key]
        public long IdDevice { get; set; }

        public string IdUser { get; set; }

        public string Name { get; set; }

        public string PushToken { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public Guid GuidDevice { get; set; }

        public bool Active { get; set; }

        public string PushTokenFirebase { get; set; }
        public string AppVersion { get; set; }
        public string UniqueId { get; set; }
        
    }
}
