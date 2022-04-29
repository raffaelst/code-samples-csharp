using Dapper.Contrib.Extensions;
using Dividendos.Entity.Enum;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("TaggedUser")]
    public class TaggedUser
    {
        [Key]
        public long TaggedUserID { get; set; }

        public string UserID { get; set; }
        public string TagValue { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
