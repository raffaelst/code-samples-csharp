
using Newtonsoft.Json;
using System;

namespace Dividendos.Passfolio.Interface.Model
{
    public class StockPassfolioOperation
    {
        public string Broker { get; set; }
        public string Symbol { get; set; }
        public decimal NumberOfShares { get; set; }
        public decimal NumberOfBuyShares { get; set; }
        public decimal AveragePrice { get; set; }
        public int OperationType { get; set; }
        public DateTime? EventDate { get; set; }
        public string Market { get; set; }
        public string Expire { get; set; }
        public string StockSpec { get; set; }
        public string Factor { get; set; }
        public decimal AcquisitionPrice { get; set; }

        /// <summary>
        /// Non-Db column
        /// </summary>
        public bool Adjusted { get; set; }
    }
}
