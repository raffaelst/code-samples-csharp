
using Newtonsoft.Json;
using System;

namespace Dividendos.B3.Interface.Model
{
    public class StockOperation
    {
        public string Broker { get; set; }
        public string Symbol { get; set; }
        public decimal NumberOfShares { get; set; }
        public decimal NumberOfBuyShares { get; set; }
        public decimal NumberOfSellShares { get; set; }
        public decimal AveragePrice { get; set; }
        public int OperationType { get; set; }
        public DateTime? EventDate { get; set; }
        public string Market { get; set; }
        public string Expire { get; set; }
        public string StockSpec { get; set; }
        public string Factor { get; set; }
        public decimal AcquisitionPrice { get; set; }
        public long IdOperationItem { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public bool EditedByUser { get; set; }

        /// <summary>
        /// Non-Db column
        /// </summary>
        /// 
        public bool PriceAdjust { get; set; }
        public bool PriceAdjustNew { get; set; }        
        public bool CopyData { get; set; }
        public bool HasNewItem { get; set; }
        public bool IsCeiOk { get; set; }
        public double DaysLastItem { get; set; }

        public bool PriceLastEditedByUser { get; set; }
    }
}
