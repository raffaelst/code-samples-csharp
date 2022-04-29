using Dividendos.CrossCutting.Identity.Models;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Enum;
using Dividendos.Entity.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Service.Interface
{
    public interface IUserService : IBaseService
    {
        ResultServiceObject<ApplicationUser> GetAccountDetails(string idUser);

        ResultServiceObject<ApplicationUser> GetByEmail(string email, bool includeExcluded = false);

        ResultServiceObject<ApplicationUser> GetByPortfolio(long idPortfolio);

        ResultServiceObject<ApplicationUser> GetByID(string idUser);

        ResultServiceObject<ApplicationUser> UpdatePassword(string idUser, string newPassword);

        ResultServiceObject<ApplicationUser> SaveRecoveryPasswordToken(string idUser, string token);

        ResultServiceObject<IEnumerable<ApplicationUser>> GetAllUserWithSendDailySummaryMailEnable();

        void UpdateUserName(string idUser, string userName);

        ResultServiceObject<int> GetNotificationAmount(string idUser);

        ResultServiceObject<int> CountAllUserWithRecentActivitiesOnApp(PushTargetTypeEnum pushTargetTypeEnum);

        ResultServiceObject<IEnumerable<ApplicationUser>> GetAllUserWithRecentActivitiesOnAppPaged(int pageIndex, int pageSize, PushTargetTypeEnum pushTargetTypeEnum);
        void UpdatePhone(string idUser, string phoneNumber);
        ResultServiceObject<IEnumerable<ApplicationUser>> GetAccountPerEmail(string email);
        void UpdateUserNameAndPhoneNumber(string idUser, string userName, string phoneNumber);

        ResultServiceObject<IEnumerable<ApplicationUser>> GetAllUserWithPortfolioActive();

        ResultServiceObject<IEnumerable<ApplicationUser>> GetAccountPerNameEmailOrPhoneNumber(string filter);

        void UpdateLastAccessDate(string idUser);

        void ExcludeAccount(string idUser);
        void ReactivateAccount(string idUser);
    }
}
