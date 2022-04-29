using Dapper.Contrib.Extensions;
using Dividendos.Entity.Entities;
using Dividendos.Repository.GenericRepository;
using Dividendos.Repository.Interface;
using Dividendos.Repository.Interface.GenericRepository;
using Dividendos.Repository.Interface.UoW;
using Dapper;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System;

namespace Dividendos.Repository.Repository
{
    public class CryptoTransactionRepository : Repository<CryptoTransaction>, ICryptoTransactionRepository
    {
        private IUnitOfWork _unitOfWork;

        public CryptoTransactionRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int CountCryptoCurrencyByUser(string idUser)
        {
            StringBuilder sb = new StringBuilder(@"select count(distinct CryptoTransaction.IdCryptoCurrency) from CryptoTransaction
                    inner join CryptoPortfolio on CryptoPortfolio.IdCryptoPortfolio = CryptoTransaction.IdCryptoPortfolio
                    inner join Trader on Trader.IdTrader = CryptoPortfolio.IdTrader
                    where Trader.IdUser = @IdUser and Trader.Active = 1 and CryptoPortfolio.Active = 1 and CryptoTransaction.Active = 1 and CryptoTransaction.TransactionType = 1");


            int count = Convert.ToInt32(_unitOfWork.Connection.ExecuteScalar(sb.ToString(), new { IdUser = idUser }, _unitOfWork.Transaction));

            return count;
        }
    }
}
