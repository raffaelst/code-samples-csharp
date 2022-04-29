using Dividendos.Entity.Entities;
using Dividendos.Entity.Enum;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using K.Logger;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Dividendos.Service
{
    public class FollowStockService : BaseService, IFollowStockService
    {
        public FollowStockService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public ResultServiceObject<FollowStock> ExistForASpecificUser(long stockIdOrCryptoId, string userID)
        {
            ResultServiceObject<FollowStock> resultServiceObject = new ResultServiceObject<FollowStock>();

            FollowStock followStock = _uow.FollowStockRepository.ExistForASpecificUser(stockIdOrCryptoId, userID).FirstOrDefault();
            resultServiceObject.Value = followStock;

            return resultServiceObject;
        }

        public ResultServiceObject<FollowStock> Add(FollowStock followStock)
        {
            ResultServiceObject<FollowStock> resultService = new ResultServiceObject<FollowStock>();
            followStock.CreatedDate = DateTime.Now;
            followStock.FollowStockGuid = Guid.NewGuid();
            followStock.Active = true;
            followStock.FollowStockID = _uow.FollowStockRepository.Insert(followStock);

            resultService.Value = followStock;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<FollowStockView>> GetAllActiveByUser(string userID)
        {
            IEnumerable<FollowStockView> followStocks = _uow.FollowStockViewRepository.GetAlertsNotDeletedByUser(userID);

            ResultServiceObject<IEnumerable<FollowStockView>> resultService = new ResultServiceObject<IEnumerable<FollowStockView>>();

            resultService.Value = followStocks;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<FollowStockView>> GetByIdStockOrIdCrypto(string idStockOrCrypto, string userID)
        {
            IEnumerable<FollowStockView> followStocks = _uow.FollowStockViewRepository.GetByIdStockOrIdCrypto(idStockOrCrypto, userID);

            ResultServiceObject<IEnumerable<FollowStockView>> resultService = new ResultServiceObject<IEnumerable<FollowStockView>>();

            resultService.Value = followStocks;

            return resultService;
        }

        public ResultServiceObject<FollowStock> Update(FollowStock followStock)
        {
            ResultServiceObject<FollowStock> resultService = new ResultServiceObject<FollowStock>();

            followStock = _uow.FollowStockRepository.Update(followStock);

            resultService.Value = followStock;

            return resultService;
        }

        public ResultServiceObject<FollowStock> GetByGuid(Guid followStockGuid)
        {
            ResultServiceObject<FollowStock> resultService = new ResultServiceObject<FollowStock>();

            FollowStock followStock = _uow.FollowStockRepository.Select(item => item.FollowStockGuid == followStockGuid).FirstOrDefault();

            resultService.Value = followStock;

            return resultService;
        }
    }
}
