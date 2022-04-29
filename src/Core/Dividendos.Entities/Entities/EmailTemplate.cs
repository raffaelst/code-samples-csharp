using Dapper.Contrib.Extensions;

namespace Dividendos.Entity.Entities
{
    [Table("EmailTemplate")]
    public class EmailTemplate
    {
        [Key]
        public int EmailTemplateId { get; set; }

        public string Template { get; set; }
    }
}
