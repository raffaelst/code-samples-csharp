using Dapper;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Views;
using Dividendos.Repository.GenericRepository;
using Dividendos.Repository.Interface;
using Dividendos.Repository.Interface.UoW;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Dividendos.Repository
{
    public class CryptoCurrencyViewRepository : Repository<CryptoBrokerView>, ICryptoCurrencyViewRepository
    {
        private IUnitOfWork _unitOfWork;

        public CryptoCurrencyViewRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public IEnumerable<CryptoBrokerView> GetCryptosBroker(string idUser)
        {
            string sql = @"
				SELECT DISTINCT FinancialInstitution.Name AS Name, Trader.GuidTrader AS GuidTrader FROM ProductUser
				INNER JOIN FinancialInstitution ON FinancialInstitution.FinancialInstitutionID = ProductUser.FinancialInstitutionID
				INNER JOIN Trader ON Trader.IdTrader = ProductUser.TraderID
				WHERE (ProductUser.UserID = @UserID OR Trader.IdUser = @UserID) 
				AND Trader.Active = 1
				AND ProductUser.Active = 1
				AND ProductUser.CurrentValue > 0 ";

            IEnumerable<CryptoBrokerView> cryptoStatementViews = _unitOfWork.Connection.Query<CryptoBrokerView>(sql, new { UserID = idUser }, _unitOfWork.Transaction);

            return cryptoStatementViews;
        }
    }
}
