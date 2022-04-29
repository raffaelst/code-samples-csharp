using System;
using System.Collections.Generic;

namespace Dividendos.Passfolio.Interface.Model
{
    public class ImportPassfolioResult
    {
        public List<StockPassfolioOperation> ListStockPortfolio { get; set; }
        public List<StockPassfolioOperation> ListStockOperation { get; set; }
        public List<DividendPassfolioImport> ListDividend { get; set; }
        public List<CryptoPassfolioImport> ListCrypto { get; set; }
        
        public bool Imported { get; set; }
        public string Message { get; set; }
        public long? IdTrader { get; set; }

        public bool PasswordWrong { get; set; }
    }
}
