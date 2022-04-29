namespace Dividendos.CrossCutting.Config.Model
{
    public class JobConfig
    {
        public string URLSite { get; set; }
        public string IntervalTimeToExecuteSyncStockPrice { get; set; }
        public string WhiteListIpsToAccess { get; set; }

        public string TimeAutoSyncOfTradersPortfolios { get; set; }


        public int TimeInHourToStartImportBacenProcess { get; set; }
        public int TimeInHourToGetAndSendDividendsNotificationNightly { get; set; }

        public string IntervalTimeToGetAndSendDividendsNotificationMorning { get; set; }

        public int TimeInHourToSendAlertTraderBlocked { get; set; }

        public string TimeInHourToSendStatisticsEmailToUsers { get; set; }

        public string IntervalTimeToCalculatePerformanceOfPorffolio { get; set; }

        public string IntervalTimeToExecuteGetDividendCalendar{ get; set; }

        public string IntervalTimeToExecuteGetAllDividendCalendarFIIs { get; set; }
        public string IntervalTimeToExecuteGetAllDividendCalendarStocks { get; set; }
        public string IntervalTimeToExecuteGetAllDividendCalendarBDRs { get; set; }
        public string TimeToExecuteGetDividendCalendarEUAFromIEXAPI { get; set; }
        
        public string IntervalTimeToExecuteGetDividendCalendarEUA { get; set; }

        public string TimeIntervalCreateDividendForManualPortfolio { get; set; }

        public string IntervalTimeToImportSpecialistPortfolios { get; set; }

        public string TimeInHourToSendPushMarketOpening { get; set; }

        public string TimeInHourToSendPushMarketClosing { get; set; }

        public string IntervalTimeToExecuteSyncUSAStockPrice { get; set; }
		
		public string IntervalTimeToExecuteSyncCryptoCurrencyPrice { get; set; }

        public string IntervalTimeToGetMarketMoversBR { get; set; }

        public string IntervalTimeToGetMarketMoversUS { get; set; }

        public string IntervalTimeToGetMarketMoversDividend { get; set; }


        public string IntervalTimeToSyncDataFromCEI { get; set; }


        public string TimeToExecuteManutOnQueue { get; set; }

        public int AmountOfSyncAgents { get; set; }

        public string ServerName { get; set; }

        public string IntervalTimeToCheckFollowStocks { get; set; }

        public int NumberOfAgentExtraContentPushNotification { get; set; }

        public string IntervalTimeToSendExtraContentPushNotification { get; set; }

        public int AmountItemsPerAgent { get; set; }

        public string TimeIntervalSendNotificationDividendDataComRemind { get; set; }

        public string TimeToExecuteAwesomeVariationsJob { get; set; }

        public string TimeToExecuteStockSplitAlert { get; set; }

        public string TimeInHourToSendStatisticsPushToUsers { get; set; }

        public string IntervalTimeToSendRelevantFactPushNotification { get; set; }
        public string IntervalTimeToImportRelevantFacts { get; set; }
        public string IntervalTimeToImportInvestingIndicators { get; set; }
        public string IntervalTimeToImportCompanyIndicators { get; set; }
        public string IntervalTimeToImportCompanyStatusInvestBrSt { get; set; }
        public string IntervalTimeToImportCompanyStatusInvestFiiSt { get; set; }
        public string IntervalTimeToImportCompanyStatusInvestEtfSt { get; set; }
        public string IntervalTimeToImportCompanyStatusInvestStockSt { get; set; }
        public string IntervalTimeToImportCompanyStatusInvestReitsSt { get; set; }
        public string IntervalTimeToImportCompanyUsStocks { get; set; }
        public string IntervalTimeToImportCompanyUsReits { get; set; }
        public string IntervalTimeToImportCompanyUsEtf { get; set; }
        public string IntervalTimeToImportStockSplitBr { get; set; }
        public string IntervalTimeToImportStockSplitUs { get; set; }
        public string AgentName { get; set; }
        public string TimeoutSeconds { get; set; }
        public string ResetTime { get; set; }
        public string AmountItems { get; set; }
        public string DeleteDays { get; set; }
        public string IntervalTimeToRunParallelNewCei { get; set; }
        public string IntervalTimeToRenewBlockedTasksNewCei { get; set; }
        public string IntervalTimeToDeleteOldCompletedTasksNewCei { get; set; }
        public string IntervalTimeToExecuteAwesomeVariationsJob { get; set; }
    }
}