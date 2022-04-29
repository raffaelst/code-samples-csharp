using Dapper;
using Dividendos.CrossCutting.Identity.Models;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Enum;
using Dividendos.Repository.GenericRepository;
using Dividendos.Repository.Interface;
using Dividendos.Repository.Interface.UoW;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Repository
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        private IUnitOfWork _unitOfWork;

        private const string USER_TABLE = "[AspNetUsers]";

        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public string GetUsername(long idUser)
        {
            string username = string.Empty;

            string sql = $"SELECT Username FROM {USER_TABLE} WHERE IdUser = @IdUser";

            username =  _unitOfWork.Connection.QueryFirst<string>(sql, new { IdUser = idUser }, _unitOfWork.Transaction);

            return username;
        }


        public ApplicationUser GetByEmail(string email, bool includeExcluded)
        {
           
            StringBuilder sql = new StringBuilder($"SELECT Id, Name, Email, PhoneNumber, Excluded FROM {USER_TABLE} WHERE Email = @Email");

            if (!includeExcluded)
            {
                sql.Append(" AND(Excluded IS NULL OR Excluded = 0)");
            }

            ApplicationUser aspNetUsers =  _unitOfWork.Connection.QueryFirstOrDefault<ApplicationUser>(sql.ToString(), new { Email = email }, _unitOfWork.Transaction);

            return aspNetUsers;
        }

        public ApplicationUser GetByIdentifier(string idUser)
        {

            string sql = $"SELECT Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount, Name , RecoveryPasswordToken, LastAccess, InfluencerAffiliatorGuid" +
                $" FROM {USER_TABLE} WHERE Id = @Id";

            ApplicationUser aspNetUsers = _unitOfWork.Connection.QueryFirstOrDefault<ApplicationUser>(sql, new { Id = idUser }, _unitOfWork.Transaction);

            return aspNetUsers;
        }

        public void UpdatePassword(string idUser, string newPassword)
        {

            string sql = $"UPDATE {USER_TABLE} SET PasswordHash = @PasswordHash, LockoutEnd = NULL  WHERE Id = @Id";

            _unitOfWork.Connection.Execute(sql, new { Id = idUser, PasswordHash = newPassword}, _unitOfWork.Transaction);
        }


        public void UpdateUserName(string idUser, string userName)
        {

            string sql = $"UPDATE {USER_TABLE} SET Name = @Name WHERE Id = @Id";

            _unitOfWork.Connection.Execute(sql, new { Id = idUser, Name = userName }, _unitOfWork.Transaction);
        }

        public void UpdateUserNameAndPhoneNumber(string idUser, string userName, string phoneNumber)
        {

            string sql = $"UPDATE {USER_TABLE} SET Name = @Name, PhoneNumber = @PhoneNumber WHERE Id = @Id";

            _unitOfWork.Connection.Execute(sql, new { Id = idUser, Name = userName, PhoneNumber = phoneNumber }, _unitOfWork.Transaction);
        }

        public void UpdateRecoveryPasswordToken(string idUser, string token)
        {

            string sql = $"UPDATE {USER_TABLE} SET RecoveryPasswordToken = @RecoveryPasswordToken WHERE Id = @Id";
            
            _unitOfWork.Connection.Execute(sql, new { Id = idUser, RecoveryPasswordToken = token }, _unitOfWork.Transaction);
        }


        public ApplicationUser GetByPortfolio(long idPortfolio)
        {
            string sql = @"SELECT AspNetUsers.Id FROM Portfolio
                            INNER JOIN Trader ON Trader.IdTrader = Portfolio.IdTrader
                            INNER JOIN AspNetUsers ON AspNetUsers.Id = Trader.IdUser WHERE Portfolio.IdPortfolio = @IdPortfolio";

            ApplicationUser aspNetUsers = _unitOfWork.Connection.QueryFirstOrDefault<ApplicationUser>(sql, new { IdPortfolio = idPortfolio }, _unitOfWork.Transaction);

            return aspNetUsers;
        }

        public int CountAllUserWithThatHasActivePortfolio(PushTargetTypeEnum pushTargetTypeEnum)
        {
            StringBuilder sql = new StringBuilder(@"SELECT COUNT(Id) FROM AspNetUsers ");

            switch(pushTargetTypeEnum)
            {
                case PushTargetTypeEnum.NotSubscriber:
                {
                    sql.Append(@"LEFT JOIN Subscription ON Subscription.IdUser = AspNetUsers.Id
                                WHERE Subscription.IdUser IS NULL");
                    break;
                }
                case PushTargetTypeEnum.Subscriber:
                {
                    sql.Append(@"INNER JOIN Subscription ON Subscription.IdUser = AspNetUsers.Id
                                WHERE Subscription.Active = 1 AND Subscription.ValidUntil >= GETDATE()");
                    break;
                }
                case PushTargetTypeEnum.SubscriptionInactive:
                {
                    sql.Append(@"INNER JOIN Subscription ON Subscription.IdUser = AspNetUsers.Id
                                WHERE Subscription.Active = 0 OR Subscription.ValidUntil < GETDATE()");
                    break;
                }
            }

            var count = _unitOfWork.Connection.Query<int>(sql.ToString(), null, _unitOfWork.Transaction).FirstOrDefault();

            return count;
        }

        public IEnumerable<ApplicationUser> GetAllUserWithSendDailySummaryMailEnable()
        {
            string sql = @"SELECT * FROM AspNetUsers LEFT JOIN
            Settings on Settings.IdUser = AspNetUsers.Id WHERE Settings.SendDailySummaryMail = 1 AND AspNetUsers.LastAccess > @OffSetLasAccessDate AND (Excluded IS NULL OR Excluded = 0)";

            var users = _unitOfWork.Connection.Query<ApplicationUser>(sql, new { OffSetLasAccessDate = DateTime.Now.AddDays(-30) }, _unitOfWork.Transaction);

            return users;
        }


        public IEnumerable<ApplicationUser> GetAllUserWithPortfolioActive(DateTime lastSyncAfter)
        {
            string sql = @"SELECT * FROM AspNetUsers 
                        INNER JOIN Trader on Trader.IdUser = AspNetUsers.Id
                        INNER JOIN Portfolio on Portfolio.IdTrader = Trader.IdTrader
                        WHERE Portfolio.Active = 1 AND Trader.Active = 1
                        AND Trader.LastSync >= @LastSyncAfter";

            var users = _unitOfWork.Connection.Query<ApplicationUser>(sql, new { LastSyncAfter = lastSyncAfter } , _unitOfWork.Transaction);

            return users;
        }

        public IEnumerable<ApplicationUser> GetAccountPerEmail(string email)
        {
            string sql = @"SELECT * FROM AspNetUsers WHERE email LIKE '%' + @Name + '%'";

            var users = _unitOfWork.Connection.Query<ApplicationUser>(sql, new { Name = email }, _unitOfWork.Transaction);

            return users;
        }

        public IEnumerable<ApplicationUser> GetAccountPerNameEmailOrPhoneNumber(string email)
        {
            string sql = @"SELECT * FROM AspNetUsers WHERE email LIKE '%' + @Filter + '%' OR [name] LIKE '%' + @Filter + '%' OR PhoneNumber LIKE '%' + @Filter + '%'";

            var users = _unitOfWork.Connection.Query<ApplicationUser>(sql, new { Filter = email }, _unitOfWork.Transaction);

            return users;
        }

        public int GetNotificationAmount(string idUser)
        {
            string sql = @"SELECT COUNT(NotificationHistoricalId) FROM NotificationHistorical WHERE UserID = @UserID AND Active = @Active";

            var notificationAmount = _unitOfWork.Connection.Query<int>(sql, new { UserID = idUser, Active = true }, _unitOfWork.Transaction).FirstOrDefault();

            return notificationAmount;
        }

        public IEnumerable<ApplicationUser> GetAllUserWithRecentActivitiesOnAppPaged(int pageIndex, int pageSize, PushTargetTypeEnum pushTargetTypeEnum)
        {
            StringBuilder sql = new StringBuilder("SELECT * FROM AspNetUsers ");

            switch(pushTargetTypeEnum)
            {
                case PushTargetTypeEnum.NotSubscriber:
                {
                    sql.Append(@"LEFT JOIN Subscription ON Subscription.IdUser = AspNetUsers.Id
                                WHERE Subscription.IdUser IS NULL");
                    break;
                }
                case PushTargetTypeEnum.Subscriber:
                {
                    sql.Append(@"INNER JOIN Subscription ON Subscription.IdUser = AspNetUsers.Id
                                WHERE Subscription.Active = 1 AND Subscription.ValidUntil >= GETDATE()");
                    break;
                }
                case PushTargetTypeEnum.SubscriptionInactive:
                {
                    sql.Append(@"INNER JOIN Subscription ON Subscription.IdUser = AspNetUsers.Id
                                WHERE Subscription.Active = 0 OR Subscription.ValidUntil < GETDATE()");
                    break;
                }
            }

            string pagedResult = BuildPaging(sql.ToString(), "ORDER BY AspNetUsers.Id ASC", pageIndex, pageSize);

            var users = _unitOfWork.Connection.Query<ApplicationUser>(pagedResult, new { pageIndex = pageIndex, pageSize = pageSize }, _unitOfWork.Transaction);

            return users;
        }

        private string BuildPaging(string sql, string orderBy, int? pageNumber, int? pageSize)
        {
            if (pageNumber.HasValue && pageNumber.Value > 0 && pageSize.HasValue && pageSize.Value > 0)
            {
                return $"{sql} {orderBy} OFFSET @pageSize * (@pageIndex - 1) ROWS FETCH NEXT @pageSize ROWS ONLY; ";
            }

            return $"{sql} {orderBy}";
        }

        public void UpdatePhone(string idUser, string phoneNumber)
        {

            string sql = $"UPDATE {USER_TABLE} SET PhoneNumber = @PhoneNumber, PhoneNumberConfirmed = 1 WHERE Id = @Id";

            _unitOfWork.Connection.Execute(sql, new { Id = idUser, PhoneNumber = phoneNumber }, _unitOfWork.Transaction);
        }

        public void UpdateLastAccessDate(string idUser)
        {

            string sql = $"UPDATE {USER_TABLE} SET LastAccess = @LastAccess WHERE Id = @Id";

            _unitOfWork.Connection.Execute(sql, new { Id = idUser, LastAccess = DateTime.Now }, _unitOfWork.Transaction);
        }

        public void ExcludeAccount(string idUser)
        {
            string sql = $"UPDATE {USER_TABLE} SET Excluded = 1 WHERE Id = @Id";

            _unitOfWork.Connection.Execute(sql, new { Id = idUser}, _unitOfWork.Transaction);
        }

        public void ReactivateAccount(string idUser)
        {
            string sql = $"UPDATE {USER_TABLE} SET Excluded = 0 WHERE Id = @Id";

            _unitOfWork.Connection.Execute(sql, new { Id = idUser }, _unitOfWork.Transaction);
        }
    }
}
