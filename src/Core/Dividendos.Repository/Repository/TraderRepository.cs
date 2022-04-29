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

namespace Dividendos.Repository.Repository
{
    public class TraderRepository : Repository<Trader>, ITraderRepository
    {
        private IUnitOfWork _unitOfWork;

        public TraderRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Trader GetByOlderSyncDate()
        {
            string sql = @"SELECT TOP 1 Trader.IdTrader, Trader.IdUser, Trader.Identifier, Trader.Password,
                        Trader.CreatedDate, Trader.Active, Trader.GuidTrader, Trader.LastSync, Trader.TraderTypeID, Trader.ShowOnPatrimony 
                        FROM Trader
                        inner join AspNetUsers on AspNetUsers.Id = Trader.IdUser
                        WHERE Trader.Active = @Active and Trader.ManualPortfolio = 0 and Trader.BlockedCei = 0
                        ORDER BY Trader.LastSync ASC";

            var traders = _unitOfWork.Connection.Query<Trader>(sql, new { Active = true }, _unitOfWork.Transaction).FirstOrDefault();

            return traders;
        }

        public IEnumerable<Trader> GetTradersBlocked(string userID, TraderTypeEnum traderTypeEnum)
        {
            string sql = @"SELECT TOP 1 Trader.IdTrader, Trader.IdUser, Trader.Identifier, Trader.Password,
                        Trader.CreatedDate, Trader.Active, Trader.GuidTrader, Trader.LastSync, Trader.TraderTypeID, Trader.ShowOnPatrimony  
                        FROM Trader
                        INNER JOIN Portfolio on Portfolio.IdTrader = Trader.IdTrader
                        WHERE Trader.Active = 1 
                        AND Portfolio.Active = 1 
                        AND Trader.BlockedCei = 1
                        AND Trader.TraderTypeID = @TraderTypeID
                        AND Trader.IdUser = @IdUser";

            var traders = _unitOfWork.Connection.Query<Trader>(sql, new { IdUser = userID, TraderTypeID = (int)traderTypeEnum }, _unitOfWork.Transaction);

            return traders;
        }

        public IEnumerable<Trader> GetAllBlockedAutomatic()
        {
            string sql = @"SELECT Trader.IdTrader, Trader.IdUser, Trader.Identifier, Trader.Password,
                        Trader.CreatedDate, Trader.Active, Trader.GuidTrader, Trader.LastSync, Trader.TraderTypeID, Trader.ShowOnPatrimony  
                        FROM Trader
                        INNER JOIN AspNetUsers on AspNetUsers.Id = Trader.IdUser
                        INNER JOIN Portfolio on Portfolio.IdTrader = Trader.IdTrader
                        WHERE Trader.Active = 1 
                        AND Portfolio.Active = 1 
                        AND Trader.BlockedCei = 1
                        AND Trader.ManualPortfolio = 0
                        AND AspNetUsers.LastAccess > @OffSetLasAccessDate
                        AND (Excluded IS NULL OR Excluded = 0)";

            var traders = _unitOfWork.Connection.Query<Trader>(sql, new { OffSetLasAccessDate = DateTime.Now.AddDays(-30) }, _unitOfWork.Transaction);

            return traders;
        }


        public void UpdateSyncData(long idTrader, DateTime syncDate)
        {

            string sql = $"UPDATE Trader SET LastSync = @LastSync WHERE IdTrader = @IdTrader";

            _unitOfWork.Connection.Execute(sql, new { IdTrader = idTrader, LastSync = syncDate }, _unitOfWork.Transaction);
        }

        public void Disable(long idTrader)
        {

            string sql = $"UPDATE Trader SET Active = @Active WHERE IdTrader = @IdTrader";

            _unitOfWork.Connection.Execute(sql, new { IdTrader = idTrader, Active = false }, _unitOfWork.Transaction);
        }

        public Trader GetByLatestSyncDate()
        {
            string sql = @"SELECT TOP 1 Trader.IdTrader, Trader.IdUser, Trader.Identifier, Trader.Password,
                        Trader.CreatedDate, Trader.Active, Trader.GuidTrader, Trader.LastSync, Trader.TraderTypeID, Trader.ShowOnPatrimony 
                        FROM Trader 
                        inner join AspNetUsers on AspNetUsers.Id = Trader.IdUser
                        WHERE Trader.Active = @Active and Trader.ManualPortfolio = 0 and Trader.BlockedCei = 0
                        ORDER BY Trader.LastSync DESC";

            var traders = _unitOfWork.Connection.Query<Trader>(sql, new { Active = true }, _unitOfWork.Transaction).FirstOrDefault();

            return traders;
        }

        public IEnumerable<Trader> GetTodayDelayedSync()
        {
            string sql = @"select distinct Trader.IdTrader, Trader.IdUser, Trader.Identifier, Trader.Password,
                        Trader.CreatedDate, Trader.Active, Trader.GuidTrader, Trader.LastSync, Trader.TraderTypeID, Trader.ShowOnPatrimony 
                            from Trader 
                            inner join Portfolio on Portfolio.IdTrader = Trader.IdTrader
                            where Trader.TraderTypeID = 1 and Trader.BlockedCei = 0
                            and  CONVERT(date, Portfolio.CalculatePerformanceDate) = CONVERT(date, DATEADD(HOUR, -3,getutcdate()))
                            and  CONVERT(date, Trader.LastSync) <> CONVERT(date, DATEADD(HOUR, -3,getutcdate())) and Trader.Active = 1 
                            order by Trader.LastSync desc";

            var traders = _unitOfWork.Connection.Query<Trader>(sql, null, _unitOfWork.Transaction);

            return traders;

        }

        public IEnumerable<Trader> GetYesterdayDelayedSync()
        {
            string sql = @"select distinct Trader.IdTrader, Trader.IdUser, Trader.Identifier, Trader.Password,
                        Trader.CreatedDate, Trader.Active, Trader.GuidTrader, Trader.LastSync, Trader.TraderTypeID, Trader.ShowOnPatrimony 
                            from Trader  
                            inner join Portfolio on Portfolio.IdTrader = Trader.IdTrader 
                            where Trader.TraderTypeID = 1 and Trader.BlockedCei = 0
                            and  CONVERT(date, Portfolio.CalculatePerformanceDate) = CONVERT(date, getutcdate() - 1)
                            and  Trader.LastSync <  Portfolio.CalculatePerformanceDate - 1 and Trader.Active = 1 
                            order by Trader.LastSync desc
                            ";

            var traders = _unitOfWork.Connection.Query<Trader>(sql, null, _unitOfWork.Transaction);

            return traders;

        }

        public IEnumerable<Trader> GetLikeIdentifier(string identifier)
        {
            string sql = @"select Trader.IdTrader, Trader.IdUser, Trader.Identifier, Trader.Password,
                        Trader.CreatedDate, Trader.Active, Trader.GuidTrader, Trader.LastSync, Trader.TraderTypeID, Trader.ShowOnPatrimony 
                        from Trader where Identifier LIKE '%' + @Identifier + '%' ";

            var traders = _unitOfWork.Connection.Query<Trader>(sql, new { Identifier = identifier }, _unitOfWork.Transaction);

            return traders;

        }

        public IEnumerable<Trader> GetWeekDelayedSync()
        {
            string sql = @"select distinct Trader.IdTrader, Trader.IdUser, Trader.Identifier, Trader.Password,
                        Trader.CreatedDate, Trader.Active, Trader.GuidTrader, Trader.TraderTypeID, Trader.LastSync, Trader.ShowOnPatrimony 
                            from Trader 
                            inner join Portfolio on Portfolio.IdTrader = Trader.IdTrader
                            where Trader.TraderTypeID = 1 and Trader.BlockedCei = 0
                            and  CONVERT(date, Portfolio.CalculatePerformanceDate) between CONVERT(date, getutcdate() - 8) and CONVERT(date, getutcdate() - 1)
                            and   CONVERT(date,Trader.LastSync) <   CONVERT(date,Portfolio.CalculatePerformanceDate) and Trader.Active = 1 
                            order by Trader.LastSync desc
                            ";

            var traders = _unitOfWork.Connection.Query<Trader>(sql, null, _unitOfWork.Transaction);

            return traders;

        }

        public void UpdateTraderCei(long idTrader, DateTime syncDate, bool blockedCei, string password, bool active)
        {

            string sql = $"UPDATE Trader SET LastSync = @LastSync, BlockedCei = @BlockedCei, Password = @Password, Active = @Active WHERE IdTrader = @IdTrader";

            _unitOfWork.Connection.Execute(sql, new { IdTrader = idTrader, LastSync = syncDate, BlockedCei = blockedCei, Password = password, Active = active }, _unitOfWork.Transaction);
        }

        public IEnumerable<Trader> GetTradersInactiveB3()
        {
            string sql = @"select distinct Trader.IdTrader, Trader.IdUser, Trader.Identifier, Trader.Password,
                        Trader.CreatedDate, Trader.Active, Trader.GuidTrader, Trader.TraderTypeID, Trader.LastSync, Trader.ShowOnPatrimony 
                        from trader where TraderTypeID = 18 and Active = 0 and createddate >
                            (
                            select top 1 createddate from trader where TraderTypeID = 18 and Active = 1 order by createddate desc
                            )
                            ";

            var traders = _unitOfWork.Connection.Query<Trader>(sql, null, _unitOfWork.Transaction);

            return traders;

        }

        public IEnumerable<Trader> GetLatestInactiveCei(string idUser)
        {
            string sql = @"select top 1 Trader.IdTrader, Trader.IdUser, Trader.Identifier, Trader.Password,
                            Trader.CreatedDate, Trader.Active, Trader.GuidTrader, Trader.TraderTypeID, Trader.LastSync, Trader.ShowOnPatrimony   
                            from trader where active = 0 and IdUser = @IdUser 
                            and  CONVERT(date, Trader.LastSync) >= CONVERT(date, DATEADD(DAY, -4,getutcdate()))
                            and TraderTypeID = 1
                            order by lastsync desc
                            ";

            var traders = _unitOfWork.Connection.Query<Trader>(sql, new { idUser = idUser }, _unitOfWork.Transaction);

            return traders;
        }
    }
}
