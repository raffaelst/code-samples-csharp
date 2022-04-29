using Dapper.Contrib.Extensions;
using System;


namespace Dividendos.Entity.Entities
{
    [Table("SystemSettings")]
    public class SystemSettings
    {
        [Key]
        public long IdSystemSettings { get; set; }
        public string SettingKey { get; set; }
        public string SettingValue { get; set; }
        public Guid GuidSystemSettings { get; set; }
    }
}
