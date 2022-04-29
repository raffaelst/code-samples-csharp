using FluentValidation.Results;
using Dividendos.CrossCutting.Identity.Models;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using Dividendos.Service.Validator.Common;
using Dividendos.Service.Validator.User;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Dividendos.Entity.Enum;

namespace Dividendos.Service
{
    public class UserService : BaseService, IUserService
    {
        public UserService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public ResultServiceObject<ApplicationUser> GetAccountDetails(string idUser)
        {
            StringValidador stringValidador = new StringValidador();
            ValidationResult results = stringValidador.Validate(idUser);

            ResultServiceObject<ApplicationUser> resultService = new ResultServiceObject<ApplicationUser>(results, _uow);

            if (results.IsValid)
            {
                ApplicationUser aspNetUsers = _uow.UserRepository.GetByIdentifier(idUser);

                resultService.Value = aspNetUsers;
            }

            return resultService;
        }

        public ResultServiceObject<IEnumerable<ApplicationUser>> GetAccountPerEmail(string email)
        {
            ResultServiceObject<IEnumerable<ApplicationUser>> resultService = new ResultServiceObject<IEnumerable<ApplicationUser>>();

            IEnumerable<ApplicationUser> aspNetUsers = _uow.UserRepository.GetAccountPerEmail(email);

            resultService.Value = aspNetUsers;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<ApplicationUser>> GetAccountPerNameEmailOrPhoneNumber(string filter)
        {
            ResultServiceObject<IEnumerable<ApplicationUser>> resultService = new ResultServiceObject<IEnumerable<ApplicationUser>>();

            IEnumerable<ApplicationUser> aspNetUsers = _uow.UserRepository.GetAccountPerNameEmailOrPhoneNumber(filter);

            resultService.Value = aspNetUsers;

            return resultService;
        }

        public ResultServiceObject<ApplicationUser> GetByEmail(string email, bool includeExcluded = false)
        {
            StringValidador stringValidador = new StringValidador();
            ValidationResult results = stringValidador.Validate(email);

            ResultServiceObject<ApplicationUser> resultService = new ResultServiceObject<ApplicationUser>(results, _uow);

            if (results.IsValid)
            {
                ApplicationUser aspNetUsers = _uow.UserRepository.GetByEmail(email, includeExcluded);

                resultService.Value = aspNetUsers;
            }

            return resultService;
        }

        public ResultServiceObject<ApplicationUser> GetByID(string idUser)
        {
            StringValidador stringValidador = new StringValidador();
            ValidationResult results = stringValidador.Validate(idUser);

            ResultServiceObject<ApplicationUser> resultService = new ResultServiceObject<ApplicationUser>(results, _uow);

            if (results.IsValid)
            {
                ApplicationUser aspNetUsers = _uow.UserRepository.GetByIdentifier(idUser);

                resultService.Value = aspNetUsers;
            }

            return resultService;
        }


        public ResultServiceObject<ApplicationUser> GetByPortfolio(long idPortfolio)
        {
            IdValidator idValidator = new IdValidator();
            ValidationResult results = idValidator.Validate(idPortfolio);

            ResultServiceObject<ApplicationUser> resultService = new ResultServiceObject<ApplicationUser>(results, _uow);

            if (results.IsValid)
            {
                ApplicationUser aspNetUsers = _uow.UserRepository.GetByPortfolio(idPortfolio);

                resultService.Value = aspNetUsers;
            }

            return resultService;
        }

        public ResultServiceObject<ApplicationUser> UpdatePassword(string idUser, string newPassword)
        {
            StringValidador validator = new StringValidador();
            ValidationResult results = validator.Validate(newPassword);
            ResultServiceObject<ApplicationUser> resultService = new ResultServiceObject<ApplicationUser>(results, _uow);

            if (results.IsValid)
            {
                _uow.UserRepository.UpdatePassword(idUser, newPassword);
            }

            return resultService;
        }

        public ResultServiceObject<ApplicationUser> SaveRecoveryPasswordToken(string idUser, string token)
        {
            StringValidador validator = new StringValidador();
            ValidationResult results = validator.Validate(token);
            ResultServiceObject<ApplicationUser> resultService = new ResultServiceObject<ApplicationUser>(results, _uow);

            if (results.IsValid)
            {
                _uow.UserRepository.UpdateRecoveryPasswordToken(idUser, token);
            }

            return resultService;
        }

        public void UpdateUserName(string idUser, string userName)
        {
            if (!string.IsNullOrEmpty(idUser))
            {
                _uow.UserRepository.UpdateUserName(idUser, userName);
            }
        }

        public void UpdateLastAccessDate(string idUser)
        {
            _uow.UserRepository.UpdateLastAccessDate(idUser);
        }

        public void ExcludeAccount(string idUser)
        {
            _uow.UserRepository.ExcludeAccount(idUser);
        }

        public void ReactivateAccount(string idUser)
        {
            _uow.UserRepository.ReactivateAccount(idUser);
        }

        public void UpdateUserNameAndPhoneNumber(string idUser, string userName, string phoneNumber)
        {
            if (!string.IsNullOrEmpty(idUser))
            {
                _uow.UserRepository.UpdateUserNameAndPhoneNumber(idUser, userName, phoneNumber);
            }
        }

        public ResultServiceObject<int> CountAllUserWithRecentActivitiesOnApp(PushTargetTypeEnum pushTargetTypeEnum)
        {
            ResultServiceObject<int> resultService = new ResultServiceObject<int>();

            int count = _uow.UserRepository.CountAllUserWithThatHasActivePortfolio(pushTargetTypeEnum);

            resultService.Value = count;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<ApplicationUser>> GetAllUserWithRecentActivitiesOnAppPaged(int pageIndex, int pageSize, PushTargetTypeEnum pushTargetTypeEnum)
        {
            ResultServiceObject<IEnumerable<ApplicationUser>> resultService = new ResultServiceObject<IEnumerable<ApplicationUser>>();

            IEnumerable<ApplicationUser> aspNetUsers = _uow.UserRepository.GetAllUserWithRecentActivitiesOnAppPaged(pageIndex, pageSize, pushTargetTypeEnum);

            resultService.Value = aspNetUsers;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<ApplicationUser>> GetAllUserWithSendDailySummaryMailEnable()
        {
            ResultServiceObject<IEnumerable<ApplicationUser>> resultService = new ResultServiceObject<IEnumerable<ApplicationUser>>();

            IEnumerable<ApplicationUser> aspNetUsers = _uow.UserRepository.GetAllUserWithSendDailySummaryMailEnable();

            resultService.Value = aspNetUsers;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<ApplicationUser>> GetAllUserWithPortfolioActive()
        {
            ResultServiceObject<IEnumerable<ApplicationUser>> resultService = new ResultServiceObject<IEnumerable<ApplicationUser>>();

            IEnumerable<ApplicationUser> aspNetUsers = _uow.UserRepository.GetAllUserWithPortfolioActive(DateTime.Now.AddDays(-30));

            resultService.Value = aspNetUsers;

            return resultService;
        }

        public ResultServiceObject<int> GetNotificationAmount(string idUser)
        {
            ResultServiceObject<int> resultService = new ResultServiceObject<int>();

            int notificationAmount = _uow.UserRepository.GetNotificationAmount(idUser);

            resultService.Value = notificationAmount;

            return resultService;
        }

        public void UpdatePhone(string idUser, string phoneNumber)
        {
            if (!string.IsNullOrEmpty(idUser))
            {
                _uow.UserRepository.UpdatePhone(idUser, phoneNumber);
            }
        }
    }
}
