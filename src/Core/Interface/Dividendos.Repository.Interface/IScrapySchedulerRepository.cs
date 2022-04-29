using Dividendos.Entity.Entities;
using Dividendos.Entity.Enum;
using Dividendos.Repository.Interface.GenericRepository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Repository.Interface
{
    public interface IScrapySchedulerRepository : IRepository<ScrapyScheduler>
    {
        IEnumerable<ScrapyScheduler> GetNextScrapyItems(int quantity, bool? newB3 = null);
        IEnumerable<ScrapyScheduler> GetCompletedTasks(int days, bool? newB3 = null);
        IEnumerable<ScrapyScheduler> GetJobsRunning(bool? newB3 = null);
        bool CheckJobIsRunningOrAwaiting(string idUser, string identifier, string password, TraderTypeEnum traderTypeEnum);
        int CountJobsRunningOrAwaiting(string agent, bool? newB3 = null);
        int CountJobsRunningOrAwaiting(bool? newB3 = null);
        void UpdateRunning(long idScrapyScheduler, int idStatus, string agent, DateTime startDate, DateTime waitingTime);
        void UpdateFinishTask(long idScrapyScheduler, int idStatus, DateTime finishDate, DateTime executionTime, string json, bool sent, bool timedOut, string responseBody);
        void UpdateRenewTask(long idScrapyScheduler, int idStatus, string agent);
        IEnumerable<ScrapyScheduler> IsIntegrationRunning(string idUser, string identifier, TraderTypeEnum traderTypeEnum);
    }
}
