using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Service.Interface
{
    public interface IHealthCheckService : IBaseService
    {
        ResultServiceObject<bool> GetStatus();

    }
}
