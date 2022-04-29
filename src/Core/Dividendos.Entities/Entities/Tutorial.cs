using Dapper.Contrib.Extensions;

namespace Dividendos.Entity.Entities
{
    [Table("Tutorial")]
    public class Tutorial
    {
        [Key]
        public long TutorialId { get; set; }

        public string Title { get; set; }
  
        public string HTMLContent { get; set; }
  

        public bool Active { get; set; }
    }
}
