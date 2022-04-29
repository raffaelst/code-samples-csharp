using Dividendos.CrossCutting.Identity.Models;
using Dividendos.Entity.Enum;
using Dividendos.Repository.Interface.GenericRepository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Repository.Interface
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        string GetUsername(long idUser);

        ApplicationUser GetByEmail(string email, bool includeExcluded);

        ApplicationUser GetByPortfolio(long idPortfolio);

        ApplicationUser GetByIdentifier(string idUser);
        void UpdatePassword(string idUser, string newPassword);

        void UpdateRecoveryPasswordToken(string idUser, string token);


        IEnumerable<ApplicationUser> GetAllUserWithSendDailySummaryMailEnable();

        void UpdateUserName(string idUser, string userName);

        int GetNotificationAmount(string idUser);

        int CountAllUserWithThatHasActivePortfolio(PushTargetTypeEnum pushTargetTypeEnum);

        IEnumerable<ApplicationUser> GetAllUserWithRecentActivitiesOnAppPaged(int pageIndex, int pageSize, PushTargetTypeEnum pushTargetTypeEnum);
        void UpdatePhone(string idUser, string phoneNumber);

        IEnumerable<ApplicationUser> GetAccountPerEmail(string email);

        void UpdateUserNameAndPhoneNumber(string idUser, string userName, string phoneNumber);

        IEnumerable<ApplicationUser> GetAllUserWithPortfolioActive(DateTime lastSyncAfter);

        IEnumerable<ApplicationUser> GetAccountPerNameEmailOrPhoneNumber(string email);

        void UpdateLastAccessDate(string idUser);

        void ExcludeAccount(string idUser);
        void ReactivateAccount(string idUser);
        
    }
}
