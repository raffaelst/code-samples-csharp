using Dapper.Contrib.Extensions;
using Dividendos.Entity.Entities;
using Dividendos.Repository.GenericRepository;
using Dividendos.Repository.Interface;
using Dividendos.Repository.Interface.GenericRepository;
using Dividendos.Repository.Interface.UoW;
using Dapper;
using System.Linq;
using System.Collections.Generic;
using System;
using Dividendos.Entity.Enum;
using System.Text;
using System.Dynamic;

namespace Dividendos.Repository.Repository
{
    public class ScrapySchedulerRepository : Repository<ScrapyScheduler>, IScrapySchedulerRepository
    {
        private IUnitOfWork _unitOfWork;

        public ScrapySchedulerRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<ScrapyScheduler> GetNextScrapyItems(int quantity, bool? newB3 = null)
        {
            dynamic queryParams = new ExpandoObject();
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
                            SELECT *
                            FROM(SELECT    ROW_NUMBER() OVER(ORDER BY ScrapyScheduler.priority, ScrapyScheduler.createddate) AS RowNum, *
                                      FROM      ScrapyScheduler
                                      WHERE     ScrapyScheduler.Status = 1 and isnull(NewB3,0) = @NewB3 ");

            if (newB3.HasValue && newB3.Value)
            {
                queryParams.NewB3 = true;
            }
            else 
            {
                queryParams.NewB3 = false;
            }

            sb.Append(@") AS RowConstrainedResult
                            WHERE   RowNum >= 1
                                AND RowNum <= @Quantity
                            ORDER BY RowNum");

            queryParams.Quantity = quantity;

            IEnumerable<ScrapyScheduler> scrapySchedulers = _unitOfWork.Connection.Query<ScrapyScheduler>(sb.ToString(), (object)queryParams, _unitOfWork.Transaction);

            return scrapySchedulers;
        }

        public IEnumerable<ScrapyScheduler> GetCompletedTasks(int days, bool? newB3 = null)
        {
            dynamic queryParams = new ExpandoObject();
            StringBuilder sb = new StringBuilder(@"select top 100 * from ScrapyScheduler where ScrapyScheduler.Status in (3,4) and  DATEDIFF(DAY, ScrapyScheduler.StartDate, getdate()) >= @Days and isnull(NewB3,0) = @NewB3 ");


            if (newB3.HasValue && newB3.Value)
            {
                queryParams.NewB3 = true;
            }
            else
            {
                queryParams.NewB3 = false;
            }

            queryParams.Days = days;

            IEnumerable<ScrapyScheduler> scrapySchedulers = _unitOfWork.Connection.Query<ScrapyScheduler>(sb.ToString(), (object)queryParams, _unitOfWork.Transaction);

            return scrapySchedulers;
        }

        public IEnumerable<ScrapyScheduler> GetJobsRunning(bool? newB3 = null)
        {
            dynamic queryParams = new ExpandoObject();
            StringBuilder sb = new StringBuilder(@"select * from ScrapyScheduler where (ScrapyScheduler.Status = 1 or ScrapyScheduler.Status = 2) and StartDate is not null  and isnull(NewB3,0) = @NewB3 ");

            if (newB3.HasValue && newB3.Value)
            {
                queryParams.NewB3 = true;
            }
            else
            {
                queryParams.NewB3 = false;
            }

            IEnumerable<ScrapyScheduler> scrapySchedulers = _unitOfWork.Connection.Query<ScrapyScheduler>(sb.ToString(), (object)queryParams, _unitOfWork.Transaction);

            return scrapySchedulers;
        }

        public bool CheckJobIsRunningOrAwaiting(string idUser, string identifier, string password, TraderTypeEnum traderTypeEnum)
        {
            StringBuilder sql = new StringBuilder(@"select case when count(*) = 0 then 0 else 1 end from ScrapyScheduler where IdUser = @IdUser and Identifier = @Identifier and Password = @Password and  (Status = 1 or Status = 2) and isnull(NewB3,0) = @NewB3 ");

            dynamic queryParams = new ExpandoObject();
            queryParams.IdUser = idUser;
            queryParams.Identifier = identifier;
            queryParams.Password = password;

            if (traderTypeEnum == TraderTypeEnum.RendaVariavelAndTesouroDiretoNewCEI)
            {
                queryParams.NewB3 = true;
            }
            else
            {
                queryParams.NewB3 = false;
            }

            bool scrapySchedulers = Convert.ToBoolean(_unitOfWork.Connection.ExecuteScalar(sql.ToString(), (object)queryParams, _unitOfWork.Transaction));

            return scrapySchedulers;
        }

        public int CountJobsRunningOrAwaiting(string agent, bool? newB3 = null)
        {
            dynamic queryParams = new ExpandoObject();
            StringBuilder sb = new StringBuilder(@"select count(*) from ScrapyScheduler where Agent = @Agent and  (Status = 1 or Status = 2) and isnull(NewB3,0) = @NewB3 ");

            if (newB3.HasValue && newB3.Value)
            {
                queryParams.NewB3 = true;
            }
            else
            {
                queryParams.NewB3 = false;
            }

            queryParams.Agent = agent;

            int scrapySchedulers = Convert.ToInt32(_unitOfWork.Connection.ExecuteScalar(sb.ToString(), (object)queryParams, _unitOfWork.Transaction));

            return scrapySchedulers;
        }

        public int CountJobsRunningOrAwaiting(bool? newB3 = null)
        {
            dynamic queryParams = new ExpandoObject();
            StringBuilder sb = new StringBuilder(@"select count(*) from ScrapyScheduler where (Status = 1 or Status = 2) and isnull(NewB3,0) = @NewB3 ");

            if (newB3.HasValue && newB3.Value)
            {
                queryParams.NewB3 = true;
            }
            else
            {
                queryParams.NewB3 = false;
            }

            int scrapySchedulers = Convert.ToInt32(_unitOfWork.Connection.ExecuteScalar(sb.ToString(), (object)queryParams, _unitOfWork.Transaction));

            return scrapySchedulers;
        }

        public void UpdateRunning(long idScrapyScheduler, int idStatus, string agent, DateTime startDate, DateTime waitingTime)
        {

            string sql = $"UPDATE ScrapyScheduler SET Status = @IdStatus, Agent = @Agent, StartDate = @StartDate, WaitingTime = @WaitingTime WHERE IdScrapyScheduler = @IdScrapyScheduler";

            _unitOfWork.Connection.Execute(sql, new { IdScrapyScheduler = idScrapyScheduler, IdStatus = idStatus, Agent = agent, StartDate = startDate, WaitingTime = waitingTime }, _unitOfWork.Transaction);
        }

        public void UpdateFinishTask(long idScrapyScheduler, int idStatus, DateTime finishDate, DateTime executionTime, string results, bool sent, bool timedOut, string responseBody)
        {

            string sql = $"UPDATE ScrapyScheduler SET Status = @IdStatus, FinishDate = @FinishDate, ExecutionTime = @ExecutionTime, Results = @Json, Sent = @Sent, TimedOut = @TimedOut, ResponseBody = @ResponseBody WHERE IdScrapyScheduler = @IdScrapyScheduler";

            _unitOfWork.Connection.Execute(sql, new { IdScrapyScheduler = idScrapyScheduler, IdStatus = idStatus, FinishDate = finishDate, ExecutionTime = executionTime, Json = results, Sent = sent, TimedOut = timedOut, ResponseBody = responseBody }, _unitOfWork.Transaction);
        }

        public void UpdateRenewTask(long idScrapyScheduler, int idStatus, string agent)
        {

            string sql = $"UPDATE ScrapyScheduler SET Status = @IdStatus, Agent = @Agent WHERE IdScrapyScheduler = @IdScrapyScheduler";

            _unitOfWork.Connection.Execute(sql, new { IdScrapyScheduler = idScrapyScheduler, IdStatus = idStatus, Agent = agent }, _unitOfWork.Transaction);
        }

        public IEnumerable<ScrapyScheduler> IsIntegrationRunning(string idUser, string identifier, TraderTypeEnum traderTypeEnum)
        {
            StringBuilder sql = new StringBuilder(@"select top 1 * from ScrapyScheduler where IdUser = @IdUser and Identifier = @Identifier and (Status = 1 or Status = 2) and IdTraderType = @IdTraderType order by CreatedDate desc ");

            dynamic queryParams = new ExpandoObject();
            queryParams.IdUser = idUser;
            queryParams.Identifier = identifier;
            queryParams.IdTraderType = (long)traderTypeEnum;

            IEnumerable<ScrapyScheduler> scrapySchedulers = _unitOfWork.Connection.Query<ScrapyScheduler>(sql.ToString(), (object)queryParams, _unitOfWork.Transaction);

            return scrapySchedulers;
        }

    }
}
