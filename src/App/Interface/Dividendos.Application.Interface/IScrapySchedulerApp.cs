using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Application.Interface
{
    public interface IScrapySchedulerApp
    {
        Task RunParallelCei(double defaulTimeout, string agentName, int amountItems);
        void RenewBlockedTasks(string resetTime);
        void DeleteOldCompletedTasks(int days);
        void RunCeiDirect(string identifier, string password, string idUser);
        Task RunParallelNewCei(double defaulTimeout, string agentName, int amountItems);
        void DeleteOldCompletedTasksNewCei(int days);
        void RenewBlockedTasksNewCei(string resetTime);
        void RunNewCeiDirect(string identifier, string idUser);
        void EnqueueNewB3Traders();
    }
}
