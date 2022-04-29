using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Avenue.Interface.Model
{
    public class AvenueTransaction
    {
        [JsonProperty("transactionDate")]
        public string TransactionDate { get; set; }

        //[JsonProperty("creditDate")]
        //public string CreditDate { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("transactionId")]
        public string TransactionId { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        //[JsonProperty("entryDate")]
        //public string EntryDate { get; set; }

        [JsonProperty("Type")]
        public string Type { get; set; }

        [JsonProperty("reffered_balance")]
        public string RefferedBalance { get; set; }

        //public AvenueTransactionItem AvenueTransactionItem { get; set; }
    }
}
