using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Entity.Enum
{
    public enum ScrapySchedulerStatusEnum
    {
        Awaiting = 1,
        Running = 2,
        Completed = 3,
        Canceled = 4,
        Retry = 5,
    }
}
