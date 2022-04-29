using FluentValidation.Results;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Repository.Interface;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using Dividendos.Service.Validator.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dividendos.Service
{
    public class ExtraContentNotificationService : BaseService, IExtraContentNotificationService
    {
        public ExtraContentNotificationService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public ResultServiceObject<ExtraContentNotification> Add(ExtraContentNotification extraContentNotification)
        {
            ResultServiceObject<ExtraContentNotification> resultService = new ResultServiceObject<ExtraContentNotification>();
            extraContentNotification.CreatedDate = DateTime.Now;
            extraContentNotification.Complete = false;
            extraContentNotification.AgentIterationSequence = 0;
            extraContentNotification.ExtraContentNotificationID = _uow.ExtraContentNotificationRepository.Insert(extraContentNotification);
            resultService.Value = extraContentNotification;

            return resultService;
        }

        public ResultServiceObject<ExtraContentNotification> SubmissionComplete(ExtraContentNotification extraContentNotification)
        {
            ResultServiceObject<ExtraContentNotification> resultService = new ResultServiceObject<ExtraContentNotification>();
            extraContentNotification.CompletedDate = DateTime.Now;
            extraContentNotification.Complete = true;
            _uow.ExtraContentNotificationRepository.Update(extraContentNotification);
            resultService.Value = extraContentNotification;

            return resultService;
        }

        public ResultServiceObject<ExtraContentNotification> UpdateIterationSequence(ExtraContentNotification extraContentNotification)
        {
            ResultServiceObject<ExtraContentNotification> resultService = new ResultServiceObject<ExtraContentNotification>();
            extraContentNotification.AgentIterationSequence = extraContentNotification.AgentIterationSequence + 1;
            _uow.ExtraContentNotificationRepository.Update(extraContentNotification);
            resultService.Value = extraContentNotification;

            return resultService;

        }
        public ResultServiceObject<ExtraContentNotification> GetLastAvailable()
        {
            ResultServiceObject<ExtraContentNotification> resultService = new ResultServiceObject<ExtraContentNotification>();

            ExtraContentNotification extraContentNotification = _uow.ExtraContentNotificationRepository.GetLastAvailable(DateTime.Now);

            resultService.Value = extraContentNotification;

            return resultService;
        }
    }
}
