using Dividendos.API.Model.Request.Stock;
using Dividendos.Application.Interface;
using Dividendos.CrossCutting.Config.Model;
using Dividendos.Entity.Enum;
using Hangfire;
using Hangfire.Storage;
using K.Logger;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dividendos.Job
{
    public class Scheduler
    {
        public Scheduler(
            IRecurringJobManager recurringJobManager,
            ILogger logger, 
            IOptions<JobConfig> jobConfig)
        {
            //Remove all jobs before add again
            using (var connection = JobStorage.Current.GetConnection())
            {
                foreach (var recurringJob in connection.GetRecurringJobs())
                {
                    RecurringJob.RemoveIfExists(recurringJob.Id);
                }
            }

            //Generated using https://crontab-generator.org/

            recurringJobManager.AddOrUpdate("Sync Stock Price",
            Hangfire.Common.Job.FromExpression<IStockApp>(stockApp => stockApp.SyncStockPriceUsingGoogleFinanceAsync(1)),
            jobConfig.Value.IntervalTimeToExecuteSyncStockPrice, TimeZoneInfo.Local, "syncstockprice");

            recurringJobManager.AddOrUpdate("Calculate portfolio performance one by one",
            Hangfire.Common.Job.FromExpression<IPortfolioApp>(portfolioApp => portfolioApp.CalculatePerformanceOneByOne()),
            jobConfig.Value.IntervalTimeToCalculatePerformanceOfPorffolio, TimeZoneInfo.Local, "canculateperformance");

            recurringJobManager.AddOrUpdate("Import infomation from BACEN",
            Hangfire.Common.Job.FromExpression<IIndicatorApp>(indicatorApp => indicatorApp.ImportIndicators()),
            Cron.Daily(jobConfig.Value.TimeInHourToStartImportBacenProcess), TimeZoneInfo.Local, "importindicators");

            recurringJobManager.AddOrUpdate("Send Dividends Push Notification (Nightly)",
            Hangfire.Common.Job.FromExpression<IDividendApp>(dividendApp => dividendApp.SendNotification(false)),
            Cron.Daily(jobConfig.Value.TimeInHourToGetAndSendDividendsNotificationNightly), TimeZoneInfo.Local, "sendpushdividendsnight");

            recurringJobManager.AddOrUpdate("Send Dividends Push Notification (Morning)",
            Hangfire.Common.Job.FromExpression<IDividendApp>(dividendApp => dividendApp.SendNotification(true)),
            jobConfig.Value.IntervalTimeToGetAndSendDividendsNotificationMorning, TimeZoneInfo.Local, "sendpushdividendsmorning");

            recurringJobManager.AddOrUpdate("Send Alert Trader Blocked (CEI)",
            Hangfire.Common.Job.FromExpression<ITraderApp>(traderApp => traderApp.SendAlertToTraderBlocked()),
            Cron.Daily(jobConfig.Value.TimeInHourToSendAlertTraderBlocked), TimeZoneInfo.Local, "sendpushalertblockedcei");

            recurringJobManager.AddOrUpdate("Email User Daily Statistics",
            Hangfire.Common.Job.FromExpression<IUserApp>(userApp => userApp.SendEmailWithDailyStatistics()),
            jobConfig.Value.TimeInHourToSendStatisticsEmailToUsers, TimeZoneInfo.Local, "sendmaildailystatistics");

            recurringJobManager.AddOrUpdate("Dividend Calendar - EUA IexAPI",
            Hangfire.Common.Job.FromExpression<IDividendCalendarApp>(devidendCalendar => devidendCalendar.GetDividendsFromIexAPI()),
            jobConfig.Value.TimeToExecuteGetDividendCalendarEUAFromIEXAPI, TimeZoneInfo.Local, "divcalendareuaiex");

            recurringJobManager.AddOrUpdate("Dividend Calendar (Get All BDRs BR From S)", Hangfire.Common.Job.FromExpression<IDividendCalendarApp>(devidendCalendar => devidendCalendar.GetAllAndUpdateFromSIBDRs()),
 jobConfig.Value.IntervalTimeToExecuteGetAllDividendCalendarBDRs, TimeZoneInfo.Local, "dividendcalendargetall");

            recurringJobManager.AddOrUpdate("Dividend Calendar (Get All FIIs BR From S)",
            Hangfire.Common.Job.FromExpression<IDividendCalendarApp>(devidendCalendar => devidendCalendar.GetAllAndUpdateFromSIFIIs()),
            jobConfig.Value.IntervalTimeToExecuteGetAllDividendCalendarFIIs, TimeZoneInfo.Local, "dividendcalendargetall");

            recurringJobManager.AddOrUpdate("Dividend Calendar (Get All  Stock BR From S)",
            Hangfire.Common.Job.FromExpression<IDividendCalendarApp>(devidendCalendar => devidendCalendar.GetAllAndUpdateFromSIStocks()),
            jobConfig.Value.IntervalTimeToExecuteGetAllDividendCalendarStocks, TimeZoneInfo.Local, "dividendcalendargetall");

            recurringJobManager.AddOrUpdate("Dividend Calendar (EUA From Nasdaq) (Next month)",
            Hangfire.Common.Job.FromExpression<IDividendCalendarApp>(devidendCalendar => devidendCalendar.GetAndUpdateFromNasdaq(DateTime.Now.AddDays(30), DateTime.Now.AddDays(60))),
            jobConfig.Value.IntervalTimeToExecuteGetDividendCalendarEUA, TimeZoneInfo.Local, "dividendcalendareua0");

            recurringJobManager.AddOrUpdate("Dividend Calendar (EUA From Nasdaq) (Next Months)",
            Hangfire.Common.Job.FromExpression<IDividendCalendarApp>(devidendCalendar => devidendCalendar.GetAndUpdateFromNasdaq(DateTime.Now.AddDays(60), DateTime.Now.AddDays(365))),
            jobConfig.Value.IntervalTimeToExecuteGetDividendCalendarEUA, TimeZoneInfo.Local, "dividendcalendareua13");

            recurringJobManager.AddOrUpdate("Crypto Currencies - Get Quotations",
            Hangfire.Common.Job.FromExpression<ICryptoCurrencyApp>(cryptoCurrencyApp => cryptoCurrencyApp.SyncCryptoCurrencyPrice()),
            jobConfig.Value.IntervalTimeToExecuteSyncCryptoCurrencyPrice, TimeZoneInfo.Local, "criptocurrencies");

            recurringJobManager.AddOrUpdate("Sync Stock Price (USA)",
             Hangfire.Common.Job.FromExpression<IStockApp>(stockApp => stockApp.SyncStockPriceUsingGoogleFinanceAsync(2)),
             jobConfig.Value.IntervalTimeToExecuteSyncUSAStockPrice, TimeZoneInfo.Local, "stocksusa");

            recurringJobManager.AddOrUpdate("Biggest Highs Stocks (BR)",
            Hangfire.Common.Job.FromExpression<IStockApp>(stockApp => stockApp.ImportMarketMover(MakertMoversType.BiggestHighsStocksBR)),
            jobConfig.Value.IntervalTimeToGetMarketMoversBR, TimeZoneInfo.Local, "marketmovers");

            recurringJobManager.AddOrUpdate("Biggest Highs FIIs (BR)",
            Hangfire.Common.Job.FromExpression<IStockApp>(stockApp => stockApp.ImportMarketMover(MakertMoversType.BiggestHighsFIIsBR)),
            jobConfig.Value.IntervalTimeToGetMarketMoversBR, TimeZoneInfo.Local, "marketmovers");

            recurringJobManager.AddOrUpdate("Biggest Highs (USA)",
            Hangfire.Common.Job.FromExpression<IStockApp>(stockApp => stockApp.ImportMarketMover(MakertMoversType.BiggestHighsUS)),
            jobConfig.Value.IntervalTimeToGetMarketMoversUS, TimeZoneInfo.Local, "marketmovers"); 

            recurringJobManager.AddOrUpdate("Biggest falls Stocks (BR)",
            Hangfire.Common.Job.FromExpression<IStockApp>(stockApp => stockApp.ImportMarketMover(MakertMoversType.BiggestFallsStocksBR)),
            jobConfig.Value.IntervalTimeToGetMarketMoversBR, TimeZoneInfo.Local, "marketmovers");

            recurringJobManager.AddOrUpdate("Biggest falls FIIs (BR)",
            Hangfire.Common.Job.FromExpression<IStockApp>(stockApp => stockApp.ImportMarketMover(MakertMoversType.BiggestFallsFIIsBR)),
            jobConfig.Value.IntervalTimeToGetMarketMoversBR, TimeZoneInfo.Local, "marketmovers");

            recurringJobManager.AddOrUpdate("Biggest falls (USA)",
            Hangfire.Common.Job.FromExpression<IStockApp>(stockApp => stockApp.ImportMarketMover(MakertMoversType.BiggestFallsUS)),
            jobConfig.Value.IntervalTimeToGetMarketMoversUS, TimeZoneInfo.Local, "marketmovers");

            recurringJobManager.AddOrUpdate("Top Dividend Stocks (BR)",
            Hangfire.Common.Job.FromExpression<IStockApp>(stockApp => stockApp.ImportMarketMover(MakertMoversType.TopDividendPaidStocksBR)),
            jobConfig.Value.IntervalTimeToGetMarketMoversDividend, TimeZoneInfo.Local, "marketmovers"); 

            recurringJobManager.AddOrUpdate("Top Dividend FIIs (BR)",
            Hangfire.Common.Job.FromExpression<IStockApp>(stockApp => stockApp.ImportMarketMover(MakertMoversType.TopDividendPaidFIIsBR)),
            jobConfig.Value.IntervalTimeToGetMarketMoversDividend, TimeZoneInfo.Local, "marketmovers"); 

            recurringJobManager.AddOrUpdate("Top Dividend (US)",
            Hangfire.Common.Job.FromExpression<IStockApp>(stockApp => stockApp.ImportMarketMover(MakertMoversType.TopDividendPaidUS)),
            jobConfig.Value.IntervalTimeToGetMarketMoversDividend, TimeZoneInfo.Local, "marketmovers");

            recurringJobManager.AddOrUpdate("Top Dividend Yield Stocks (BR)",
            Hangfire.Common.Job.FromExpression<IStockApp>(stockApp => stockApp.ImportMarketMover(MakertMoversType.TopDividendYieldStocksBR)),
            jobConfig.Value.IntervalTimeToGetMarketMoversDividend, TimeZoneInfo.Local, "marketmovers"); 

            recurringJobManager.AddOrUpdate("Top Dividend Yield FIIs (BR)",
            Hangfire.Common.Job.FromExpression<IStockApp>(stockApp => stockApp.ImportMarketMover(MakertMoversType.TopDividendYieldFIIsBR)),
            jobConfig.Value.IntervalTimeToGetMarketMoversDividend, TimeZoneInfo.Local, "marketmovers"); 

            recurringJobManager.AddOrUpdate("Top Dividend Yield (US)",
            Hangfire.Common.Job.FromExpression<IStockApp>(stockApp => stockApp.ImportMarketMover(MakertMoversType.TopDividendYieldUS)),
            jobConfig.Value.IntervalTimeToGetMarketMoversDividend, TimeZoneInfo.Local, "marketmovers");

            recurringJobManager.AddOrUpdate("Follow Stocks",
            Hangfire.Common.Job.FromExpression<IFollowStockApp>(followStockApp => followStockApp.CheckFollowStockAlerts()),
            jobConfig.Value.IntervalTimeToCheckFollowStocks, TimeZoneInfo.Local, "followstock");


            recurringJobManager.AddOrUpdate("Send extra content push notification with historical",
            Hangfire.Common.Job.FromExpression<IExtraContentNotificationApp>(extraContentNotificationApp => extraContentNotificationApp.CheckAndSendExtraContentPushNotification(jobConfig.Value.AmountItemsPerAgent)),
            jobConfig.Value.IntervalTimeToSendExtraContentPushNotification, TimeZoneInfo.Local, "notification");


            recurringJobManager.AddOrUpdate("Push Dividend (DataCom Remind)",
            Hangfire.Common.Job.FromExpression<IDividendCalendarApp>(devidendCalendar => devidendCalendar.SendNotificationDividendDataComRemind()),
            jobConfig.Value.TimeIntervalSendNotificationDividendDataComRemind, TimeZoneInfo.Local, "datacomnotification");


            recurringJobManager.AddOrUpdate("Stocks with Awesome daily variation",
            Hangfire.Common.Job.FromExpression<IStockApp>(stockApp => stockApp.SendAlertAwesomeDailyVariations()),
            jobConfig.Value.TimeToExecuteAwesomeVariationsJob, TimeZoneInfo.Local, "awesomevariations");

            recurringJobManager.AddOrUpdate("Push User Daily Statistics",
            Hangfire.Common.Job.FromExpression<IUserApp>(userApp => userApp.SendPushWithDailyStatistics()),
            jobConfig.Value.TimeInHourToSendStatisticsPushToUsers, TimeZoneInfo.Local, "sendpushdailystatistics");

            recurringJobManager.AddOrUpdate("Send content push notification with relevant fact",
            Hangfire.Common.Job.FromExpression<IRelevantFactApp>(relevantFactApp => relevantFactApp.SendNotificationToInteressedUsers()),
            jobConfig.Value.IntervalTimeToSendRelevantFactPushNotification, TimeZoneInfo.Local, "relevantfact");

            recurringJobManager.AddOrUpdate("Import Relevant Facts",
            Hangfire.Common.Job.FromExpression<IRelevantFactApp>(relevantFactApp => relevantFactApp.ImportRelevantFacts()),
            jobConfig.Value.IntervalTimeToImportRelevantFacts, TimeZoneInfo.Local, "importrelevantfacts");

            recurringJobManager.AddOrUpdate("Import InvestingCom Indicators",
            Hangfire.Common.Job.FromExpression<IIndicatorApp>(indicatorApp => indicatorApp.ImportInvestingIndicators()),
            jobConfig.Value.IntervalTimeToImportInvestingIndicators, TimeZoneInfo.Local, "importinvestingindicators");

            recurringJobManager.AddOrUpdate("Import Company Indicators",
            Hangfire.Common.Job.FromExpression<ICompanyIndicatorsApp>(companyIndicatorsApp => companyIndicatorsApp.ImportCompanyIndicators()),
            jobConfig.Value.IntervalTimeToImportCompanyIndicators, TimeZoneInfo.Local, "importompanyindicators");

            recurringJobManager.AddOrUpdate("Import Company Br ST",
            Hangfire.Common.Job.FromExpression<IStockApp>(stockApp => stockApp.ImportStatusInvestCompanies(1)),
            jobConfig.Value.IntervalTimeToImportCompanyStatusInvestBrSt, TimeZoneInfo.Local, "importompanybrst");

            recurringJobManager.AddOrUpdate("Import Company Fii ST",
            Hangfire.Common.Job.FromExpression<IStockApp>(stockApp => stockApp.ImportStatusInvestCompanies(2)),
            jobConfig.Value.IntervalTimeToImportCompanyStatusInvestFiiSt, TimeZoneInfo.Local, "importompanyfiist");

            recurringJobManager.AddOrUpdate("Import Company Etf ST",
            Hangfire.Common.Job.FromExpression<IStockApp>(stockApp => stockApp.ImportStatusInvestCompanies(6)),
            jobConfig.Value.IntervalTimeToImportCompanyStatusInvestEtfSt, TimeZoneInfo.Local, "importompanyetfst");

            recurringJobManager.AddOrUpdate("Import Company Stock ST",
            Hangfire.Common.Job.FromExpression<IStockApp>(stockApp => stockApp.ImportStatusInvestCompanies(12)),
            jobConfig.Value.IntervalTimeToImportCompanyStatusInvestStockSt, TimeZoneInfo.Local, "importompanystockst");

            recurringJobManager.AddOrUpdate("Import Company Reits ST",
            Hangfire.Common.Job.FromExpression<IStockApp>(stockApp => stockApp.ImportStatusInvestCompanies(13)),
            jobConfig.Value.IntervalTimeToImportCompanyStatusInvestReitsSt, TimeZoneInfo.Local, "importompanyreitsst");

            recurringJobManager.AddOrUpdate("Import Us Stocks",
            Hangfire.Common.Job.FromExpression<IStockApp>(stockApp => stockApp.ImportUsStocks(1)),
            jobConfig.Value.IntervalTimeToImportCompanyUsStocks, TimeZoneInfo.Local, "importusstocks");

            recurringJobManager.AddOrUpdate("Import Us Reits",
            Hangfire.Common.Job.FromExpression<IStockApp>(stockApp => stockApp.ImportUsStocks(2)),
            jobConfig.Value.IntervalTimeToImportCompanyUsReits, TimeZoneInfo.Local, "importusreits");

            recurringJobManager.AddOrUpdate("Import Us Etfs",
            Hangfire.Common.Job.FromExpression<IStockApp>(stockApp => stockApp.ImportUsStocks(1)),
            jobConfig.Value.IntervalTimeToImportCompanyUsEtf, TimeZoneInfo.Local, "importusetfs");

            recurringJobManager.AddOrUpdate("Import Stock Split Br",
            Hangfire.Common.Job.FromExpression<IStockSplitApp>(stockSplitApp => stockSplitApp.ImportStockSplit(1)),
            jobConfig.Value.IntervalTimeToImportStockSplitBr, TimeZoneInfo.Local, "importstocksplitbr");

            recurringJobManager.AddOrUpdate("Import Stock Split Us",
            Hangfire.Common.Job.FromExpression<IStockSplitApp>(stockSplitApp => stockSplitApp.ImportStockSplit(2)),
            jobConfig.Value.IntervalTimeToImportStockSplitUs, TimeZoneInfo.Local, "importstocksplitus");

            recurringJobManager.AddOrUpdate("Stocks with Awesome variation (checking all day long)",
            Hangfire.Common.Job.FromExpression<IStockApp>(stockApp => stockApp.SendAlertAwesomeDailyVariationsCheckingAllDayLong()),
            jobConfig.Value.IntervalTimeToExecuteAwesomeVariationsJob, TimeZoneInfo.Local, "awesomevariationsalldaylong");
        }
    }
}
