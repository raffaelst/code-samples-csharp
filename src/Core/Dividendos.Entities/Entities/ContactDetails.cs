using Dapper.Contrib.Extensions;
using System;


namespace Dividendos.Entity.Entities
{
    [Table("ContactDetails")]
    public class ContactDetails
    {
        [Key]
        public long IdContactDetails { get; set; }
        public string IdUser { get; set; }
        public int IdSourceInfo { get; set; }
        public string Name { get; set; }
        public string BirthDate { get; set; }
        public string DocumentNumber { get; set; }
        public string Email { get; set; }
        public string BirthCity { get; set; }
        public string Gender { get; set; }
        public string MotherName { get; set; }
        public string SpouseName { get; set; }
        public string SpouseDocumentNumber { get; set; }
        public string StreetName { get; set; }
        public string AddressNumber { get; set; }
        public string Complement { get; set; }
        public string Neighborhood { get; set; }
        public string PostalCode { get; set; }
        public string StateCode { get; set; }
        public string AdressType { get; set; }
        public string CompanyDocumentNumber { get; set; }
        public string CompanyName { get; set; }
        public string OcupationDesc { get; set; }
        public string MonthlyIncome { get; set; }
        public string PatrimonialTotalAmount { get; set; }
        public string BankDepositAmount { get; set; }
        public string City { get; set; }
    }
}
