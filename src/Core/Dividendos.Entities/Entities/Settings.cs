
using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    [Table("Settings")]
    public class Settings
    {
        [Key]
        public long IdSettings { get; set; }
        public string IdUser { get; set; }
        public bool AutoSyncPortfolio { get; set; }

        public bool SendDailySummaryMail { get; set; }

        public bool PushNewDividend { get; set; }


        public bool PushDividendDeposit { get; set; }


        public bool PushChangeInPortfolio { get; set; }

        public bool AutomaticRefreshPortfolio { get; set; }

        public Guid GuidSettings { get; set; }


        public bool PushMarketOpeningAndClosing { get; set; }

        public bool PushBreakingNews { get; set; }

        public bool PushDataComYourStocks { get; set; }

        public bool PushStocksWithAwesomeVariation { get; set; }

        public bool PushRelevantFacts { get; set; }

    }
}
