using K.Logger;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Dividendos.Hurst.Interface
{
    public interface IHurstHelper
    {
        void SendEvent(string name, string email, string phoneNumber);
    }
}
