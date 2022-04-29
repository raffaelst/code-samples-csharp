using Dividendos.Entity.Entities;
using Dividendos.Repository.Interface.GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Repository.Interface
{
    public interface IProductUserRepository : IRepository<ProductUser>
    {
        void UpdateAveragePrice(string productUserGuid, decimal averagePrice);
    }
}
