using System;

namespace Dividendos.Entity.Views
{
    public class StockView
    {
        public long IdStock { get; set; }
        public long IdCompany { get; set; }
        public Guid GuidStock { get; set; }
        public string Symbol { get; set; }
        public string CompanyName { get; set; }

        public string Logo { get; set; }
        public string LogoURL { get; set; }

        public int IdStockType { get; set; }
        public int IdCountry { get; set; }
    }
}
