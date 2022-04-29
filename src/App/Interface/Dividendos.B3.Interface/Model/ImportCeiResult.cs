using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.B3.Interface.Model
{
    public class ImportCeiResult
    {
        public List<StockOperation> ListStockPortfolio { get; set; }
        public List<StockOperation> ListStockOperation { get; set; }
        public List<DividendImport> ListDividend { get; set; }        
        public List<TesouroDiretoImport> ListTesouroDireto { get; set; }
        public List<StockOperation> ListStockAveragePrice { get; set; }
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
        public string Json { get; set; }
        public int ErrorCode { get; set; }
    }
}
