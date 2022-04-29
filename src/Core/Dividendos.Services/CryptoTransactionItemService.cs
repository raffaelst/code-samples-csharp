using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Service
{
    public class CryptoTransactionItemService : BaseService, ICryptoTransactionItemService
    {
        public CryptoTransactionItemService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public ResultServiceObject<CryptoTransactionItem> Insert(CryptoTransactionItem cryptoTransactionItem, DateTime? lastUpdatedDate = null)
        {
            ResultServiceObject<CryptoTransactionItem> resultService = new ResultServiceObject<CryptoTransactionItem>();

            if (cryptoTransactionItem.AveragePrice >= 9999999999999M || cryptoTransactionItem.AveragePrice <= -9999999999999M)
            {
                cryptoTransactionItem.AveragePrice = 0;
            }

            if (cryptoTransactionItem.AcquisitionPrice >= 9999999999999M || cryptoTransactionItem.AcquisitionPrice <= -9999999999999M)
            {
                cryptoTransactionItem.AcquisitionPrice = 0;
            }

            cryptoTransactionItem.GuidCryptoTransactionItem = Guid.NewGuid();
            cryptoTransactionItem.LastUpdatedDate = lastUpdatedDate.HasValue ? lastUpdatedDate.Value : DateTime.Now;
            cryptoTransactionItem.Active = true;
            cryptoTransactionItem.IdCryptoTransactionItem = _uow.CryptoTransactionItemRepository.Insert(cryptoTransactionItem);
            resultService.Value = cryptoTransactionItem;

            return resultService;
        }

        public ResultServiceObject<CryptoTransactionItem> Update(CryptoTransactionItem cryptoTransactionItem)
        {
            if (cryptoTransactionItem.AveragePrice >= 9999999999999M || cryptoTransactionItem.AveragePrice <= -9999999999999M)
            {
                cryptoTransactionItem.AveragePrice = 0;
            }

            if (cryptoTransactionItem.AcquisitionPrice >= 9999999999999M || cryptoTransactionItem.AcquisitionPrice <= -9999999999999M)
            {
                cryptoTransactionItem.AcquisitionPrice = 0;
            }

            ResultServiceObject<CryptoTransactionItem> resultService = new ResultServiceObject<CryptoTransactionItem>();
            cryptoTransactionItem.LastUpdatedDate = DateTime.Now;
            resultService.Value = _uow.CryptoTransactionItemRepository.Update(cryptoTransactionItem);

            return resultService;
        }

        public ResultServiceObject<IEnumerable<CryptoTransactionItem>> GetByIdCryptoTransaction(long idCryptoTransaction, int transactionType)
        {
            ResultServiceObject<IEnumerable<CryptoTransactionItem>> resultService = new ResultServiceObject<IEnumerable<CryptoTransactionItem>>();

            IEnumerable<CryptoTransactionItem> operations = _uow.CryptoTransactionItemRepository.Select(p => p.IdCryptoTransaction == idCryptoTransaction && p.TransactionType == transactionType && p.Active == true);

            resultService.Value = operations;

            return resultService;
        }

        public ResultServiceObject<CryptoTransactionItem> GetByGuid(Guid guidCryptoTransactionItem)
        {
            ResultServiceObject<CryptoTransactionItem> resultService = new ResultServiceObject<CryptoTransactionItem>();

            IEnumerable<CryptoTransactionItem> crTransactionItems = _uow.CryptoTransactionItemRepository.Select(p => p.GuidCryptoTransactionItem == guidCryptoTransactionItem);

            resultService.Value = crTransactionItems.FirstOrDefault();

            return resultService;
        }

        public ResultServiceObject<bool> Delete(CryptoTransactionItem cryptoTransactionItem)
        {
            ResultServiceObject<bool> resultService = new ResultServiceObject<bool>();
            resultService.Value = _uow.CryptoTransactionItemRepository.Delete(cryptoTransactionItem);

            return resultService;
        }

    }
}
