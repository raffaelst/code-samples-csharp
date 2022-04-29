using Dapper.Contrib.Extensions;
using Dividendos.Entity.Enum;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("InitialOffer")]
    public class InitialOffer
    {
        [Key]
        public long InitialOfferID { get; set; }

        public string CompanyName { get; set; }

        public string CNPJ { get; set; }

        public decimal? InitialValue { get; set; }

        public decimal? OfferValue { get; set; }

        public string Symbol { get; set; }

        public string Image { get; set; }

        public DateTime DateRegister { get; set; }

        public DateTime? StarDate { get; set; }

        public InitialOfferEnum InitialOfferEnum { get; set; }

        public bool IsStock { get; set; }
    }
}
