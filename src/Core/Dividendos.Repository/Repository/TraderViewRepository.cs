using Dapper;
using Dividendos.Entity.Entities;
using Dividendos.Repository.GenericRepository;
using Dividendos.Repository.Interface;
using Dividendos.Repository.Interface.UoW;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Dividendos.Entity.Enum;
using Dividendos.Entity.Views;

namespace Dividendos.Repository.Repository
{
    public class TraderViewRepository : Repository<TraderView>, ITraderViewRepository
    {
        private IUnitOfWork _unitOfWork;

        public TraderViewRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<TraderView> GetItemsComposePatrimony(string userID)
        {
            string sql = @"SELECT Trader.IdTrader, Identifier, GuidTrader, TraderTypeID, ShowOnPatrimony, Portfolio.Name FROM Trader 
                          INNER JOIN Portfolio ON Portfolio.IdTrader = Trader.IdTrader
                          WHERE Trader.IdUser = @IdUser
                          AND 
                          Trader.Active = 1 AND Portfolio.Active = 1
                          UNION ALL
                          SELECT Trader.IdTrader, Identifier, GuidTrader, TraderTypeID, ShowOnPatrimony, CryptoPortfolio.Name FROM Trader 
                          INNER JOIN CryptoPortfolio ON CryptoPortfolio.IdTrader = Trader.IdTrader
                          WHERE Trader.IdUser = @IdUser
                          AND 
                          Trader.Active = 1 AND CryptoPortfolio.Active = 1";

            var traders = _unitOfWork.Connection.Query<TraderView>(sql, new { IdUser = userID}, _unitOfWork.Transaction);

            return traders;
        }

        public void UpdateShowOnPatrimony(Guid guidTrader, bool showOnPatrimony)
        {
            string sql = @"UPDATE Trader SET ShowOnPatrimony = @ShowOnPatrimony WHERE GuidTrader = @GuidTrader";

            _unitOfWork.Connection.Execute(sql, new { GuidTrader = guidTrader, ShowOnPatrimony = showOnPatrimony }, _unitOfWork.Transaction);
        }
    }
}
