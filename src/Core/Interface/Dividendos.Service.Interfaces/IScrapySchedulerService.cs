using Dividendos.Entity.Entities;
using Dividendos.Entity.Enum;
using Dividendos.Entity.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Dividendos.Service.Interface
{
    public interface IScrapySchedulerService : IBaseService
    {
        ResultServiceObject<ScrapyScheduler> Add(ScrapyScheduler scrapyScheduler);
        ResultServiceObject<ScrapyScheduler> Update(ScrapyScheduler scrapyScheduler);
        ResultServiceObject<bool> Delete(ScrapyScheduler scrapyScheduler);
        ResultServiceObject<IEnumerable<ScrapyScheduler>> GetNextScrapyItems(int quantity, bool? newB3 = null);
        ResultServiceObject<bool> CheckJobIsRunningOrAwaiting(string idUser, string identifier, string password, TraderTypeEnum traderTypeEnum);
        ResultServiceObject<IEnumerable<ScrapyScheduler>> GetJobsRunning(bool? newB3 = null);
        ResultServiceObject<IEnumerable<ScrapyScheduler>> GetCompletedTasks(int days, bool? newB3 = null);
        ResultServiceObject<int> CountJobsRunningOrAwaiting(string agent, bool? newB3 = null);
        ResultServiceObject<int> CountJobsRunningOrAwaiting(bool? newB3 = null);
        ResultServiceObject<ScrapyScheduler> CreateFromAgent(ScrapyScheduler scrapyScheduler);
        bool CreateTask(string identifier, string password, string idUser, bool automaticProcess, ITraderService _traderService, IScrapySchedulerService _scrapySchedulerService, ISubscriptionService _subscriptionService, bool hangfire = false, TraderTypeEnum traderTypeEnum = TraderTypeEnum.RendaVariavelAndTesouroDiretoCEI);
        void UpdateRunning(long idScrapyScheduler, int idStatus, string agent, DateTime startDate, DateTime waitingTime);
        void UpdateFinishTask(long idScrapyScheduler, int idStatus, DateTime finishDate, DateTime executionTime, string json, bool sent, bool timedOut, string responseBody);
        void UpdateRenewTask(long idScrapyScheduler, int idStatus, string agent);
        ResultServiceObject<ScrapyScheduler> AddBrokerLog(ScrapyScheduler scrapyScheduler);
        ResultServiceObject<bool> IsIntegrationRunning(string idUser, string identifier, TraderTypeEnum traderTypeEnum);
    }
}
