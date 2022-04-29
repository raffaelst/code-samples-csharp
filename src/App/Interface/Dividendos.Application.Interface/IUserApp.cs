using Dividendos.API.Model.Request;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Application.Interface
{
    public interface IUserApp
    {
        ResultResponseObject<UserVM> GetAccountDetails();
        ResultResponseStringModel RegisterNewUser(UserRegisterVM userRegister);

        ResultResponseStringModel RecoveryPassword(string userName);
        ResultResponseStringModel ResetPassword(string email, string token, string newPassword);
        ResultResponseStringModel ChangePassword(string currentPassword, string newPassword);

        void SendEmailWithDailyStatistics();

        ResultResponseBase ChangeUserData(API.Model.Request.v2.User.UserVM userVM);

        ResultResponseObject<Dividendos.API.Model.Response.v3.UserVM> GetAccountDetailsWithNotificationAmount();

        ResultResponseStringModel RegisterNewUserFromAffiliate(UserRegisterAffiliateVM userRegister);

        ResultResponseObject<IEnumerable<UserVM>> GetAccountPerEmail(string email);

        public ResultResponseBase ChangeUserName(string name);

        void SendPushWithDailyStatistics();

        ResultResponseObject<IEnumerable<UserDataVM>> GetAccountPerNameEmailOrPhoneNumber(string filter);

        ResultResponseObject<IEnumerable<CrossCutting.Identity.Models.ApplicationUser>> GetUserByMail(string email);

        ResultResponseBase DeleteAccount(string userID);
    }
}