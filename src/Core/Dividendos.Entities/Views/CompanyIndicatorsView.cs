using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Entity.Views
{
    public class CompanyIndicatorsView
    {
        public long IdStock { get; set; }
        public int IdStockType { get; set; }
        public string Company { get; set; }
        public string Symbol { get; set; }
        public string Country { get; set; }
        public string Logo { get; set; }
        public string Segment { get; set; }
        public string Sector { get; set; }
        public string Subsector { get; set; }
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
