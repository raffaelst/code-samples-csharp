using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Entity.Enum
{
    public enum ScrapyClusterEnum
    {
        RunParallelCei = 1,
        ResetTasks = 2,
        DeleteTasks = 3,
        RunDelayedCei = 4,
        RunScrapyAgent = 5,
        ImportCompanies = 6,
        ImportSplit = 7,
        RunParallelNewCei = 8,
        ResetTasksNewCei = 9,
        DeleteTasksNewCei = 10,
        EnqueueNewB3Traders = 11,
    }
}
