using Dapper.Contrib.Extensions;

namespace Dividendos.Entity.Entities
{
    [Table("VideoTutorial")]
    public class VideoTutorial
    {
        [Key]
        public long VideoTutorialId { get; set; }

        public string URL { get; set; }
  
        public string VideoTutorialType { get; set; }
  

        public bool Active { get; set; }
    }
}
