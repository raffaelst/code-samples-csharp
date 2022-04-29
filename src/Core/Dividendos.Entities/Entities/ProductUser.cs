using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Entity.Entities
{
    [Table("ProductUser")]
    public class ProductUser
    {
        [Key]

        public long ProductUserID { get; set; }
        public Guid ProductUserGuid { get; set; }
        public int FinancialInstitutionID { get; set; }
        public int ProductID { get; set; }

        public long? TraderID { get; set; }

        public string UserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal CurrentValue { get; set; }
        public bool Active { get; set; }

        public decimal AveragePrice { get; set; }
        
        public ProductUser()
        { }

        public ProductUser(ProductUserView productUserView)
        {
            this.ProductUserID = productUserView.ProductUserID;
            this.Active = productUserView.Active;
            this.TraderID = productUserView.TraderID;
            this.ProductUserGuid = productUserView.ProductUserGuid;
            this.FinancialInstitutionID = productUserView.FinancialInstitutionID;
            this.ProductID = productUserView.ProductID;
            this.CreatedDate = productUserView.CreatedDate;
            this.CurrentValue = productUserView.CurrentValue;
            this.AveragePrice = productUserView.AveragePrice;
        }
    }
}
