using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("CompanyIndicators")]
    public class CompanyIndicators
    {
        [Key]
        public long IdCompanyIndicators { get; set; }

        public string CompanyCode { get; set; }
        public DateTime ReferenceDate { get; set; }
        public decimal NetWorth { get; set; }
        public decimal TotalAssets { get; set; }
        public decimal NetDebt { get; set; }
        public decimal NetProfitAnnual { get; set; }
        public decimal RoeAnnual { get; set; }
        public decimal RoaAnnual { get; set; }
        public decimal PricePerVpa { get; set; }
        public decimal PricePerProfit { get; set; }
        public decimal QttyStock { get; set; }
        public decimal MarketCap { get; set; }
        public decimal RoicAnnual { get; set; }
        public decimal PayoutAnnual { get; set; }
        public decimal Dividend12Months { get; set; }
        public decimal Dividend12MonthsYield { get; set; }
        public decimal Dividend24Months { get; set; }
        public decimal Dividend24MonthsYield { get; set; }
        public decimal Dividend36Months { get; set; }
        public decimal Dividend36MonthsYield { get; set; }
        public decimal VlPatrimonyQuotas { get; set; }
        public decimal TotalQuotaHolder { get; set; }
    }
}
