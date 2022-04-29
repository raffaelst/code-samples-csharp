using Dapper.Contrib.Extensions;
using System;


namespace Dividendos.Entity.Entities
{
    [Table("ContactPhone")]
    public class ContactPhone
    {
        [Key]
        public long IdContactPhone { get; set; }
        public string IdUser { get; set; }
        public int IdSourceInfo { get; set; }
        public string AreaCode { get; set; }
        public string CountryCode { get; set; }
        public string PhoneNumber { get; set; }
    }
}
