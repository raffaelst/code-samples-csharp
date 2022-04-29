using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using K.Logger;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Service.Interface
{
    public interface IFollowStockService : IBaseService
    {
        ResultServiceObject<FollowStock> Add(FollowStock followStock);
        ResultServiceObject<FollowStock> Update(FollowStock followStock);

        ResultServiceObject<FollowStock> GetByGuid(Guid followStockGuid);

        ResultServiceObject<IEnumerable<FollowStockView>> GetAllActiveByUser(string userID);

        ResultServiceObject<FollowStock> ExistForASpecificUser(long stockIdOrCryptoId, string userID);

        ResultServiceObject<IEnumerable<FollowStockView>> GetByIdStockOrIdCrypto(string idStockOrCrypto, string userID);
    }
}
