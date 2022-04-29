using Dapper.Contrib.Extensions;

namespace Dividendos.Entity.Entities
{
    [Table("SupportChannel")]
    public class SupportChannel
    {
        [Key]
        public long SupportChannelID { get; set; }

        public string Description { get; set; }
  
        public string Icon { get; set; }
   
        public string Link { get; set; }

        public bool OnlyForSubscribers { get; set; }
    }
}
