using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Entity.Views
{
    public class ImportCeiResultView
    {
        public List<StockOperationView> ListStockPortfolio { get; set; }
        public List<StockOperationView> ListStockOperation { get; set; }
        public List<DividendImportView> ListDividend { get; set; }
        public List<StockOperationView> ListStockAveragePrice { get; set; }
        public List<TesouroDiretoImportView> ListTesouroDireto { get; set; }
        public bool Imported { get; set; }
        public bool UserBlocked { get; set; }
        public string Message { get; set; }
        public bool ErrorCEI { get; set; }
        public long? IdTrader { get; set; }
        public bool HasRent { get; set; }
        public string Identifier { get; set; }
        public string Password { get; set; }
        public string IdUser { get; set; }
        public bool AutomaticProcess { get; set; }
    }
}
