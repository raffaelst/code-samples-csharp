using Dividendos.Entity.Entities;
using Dividendos.Repository.GenericRepository;
using Dividendos.Repository.Interface;
using Dividendos.Repository.Interface.UoW;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Linq;
using Dividendos.Entity.Views;
using System.Dynamic;

namespace Dividendos.Repository.Repository
{
    public class DividendInfoViewRepository : Repository<DividendDetailsView>, IDividendInfoViewRepository
    {
        private IUnitOfWork _unitOfWork;

        public DividendInfoViewRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

		public IEnumerable<DividendInfoView> GetManualPortfoliosWithStock(long idStock)
		{
			string sql = @"SELECT DISTINCT Operation.IdPortfolio, Trader.IdUser
							FROM Operation
							INNER JOIN Portfolio ON Portfolio.IdPortfolio = Operation.IdPortfolio AND Portfolio.Active = 1 AND Portfolio.ManualPortfolio = 1
							INNER JOIN Trader ON Trader.IdTrader = Portfolio.IdTrader
							INNER JOIN Stock ON Stock.IdStock = Operation.IdStock AND Stock.ShowOnPortolio = 1
							WHERE Operation.IdStock = @IdStock";

			var portfolios = _unitOfWork.Connection.Query<DividendInfoView>(sql, new { IdStock = idStock }, _unitOfWork.Transaction);


			return portfolios;
		}

		public IEnumerable<string> GetAllUsersWithStock(long idStock)
		{
			string sql = @"SELECT DISTINCT Trader.IdUser
							FROM Operation
							INNER JOIN Portfolio ON Portfolio.IdPortfolio = Operation.IdPortfolio AND Portfolio.Active = 1 AND Operation.Active = 1 AND Operation.IdOperationType = 1 AND Operation.NumberOfShares > 0
							INNER JOIN Trader ON Trader.IdTrader = Portfolio.IdTrader
							INNER JOIN Stock ON Stock.IdStock = Operation.IdStock AND Stock.ShowOnPortolio = 1
							WHERE Operation.IdStock = @IdStock";

			var portfolios = _unitOfWork.Connection.Query<string>(sql, new { IdStock = idStock }, _unitOfWork.Transaction);


			return portfolios;
		}
	}
}
