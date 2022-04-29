using FluentValidation.Results;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Repository.Interface;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using Dividendos.Service.Validator.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Dividendos.Service
{
    public class EmailTemplateService : BaseService, IEmailTemplateService
    {
        public EmailTemplateService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public ResultServiceObject<IEnumerable<EmailTemplate>> GetAll()
        {
            ResultServiceObject<IEnumerable<EmailTemplate>> resultService = new ResultServiceObject<IEnumerable<EmailTemplate>>();

            IEnumerable<EmailTemplate> stocks = _uow.EmailTemplateRepository.GetAll();

            resultService.Value = stocks;

            return resultService;
        }


        public ResultServiceObject<EmailTemplate> GetById(int idEmailTemplate)
        {
            ResultServiceObject<EmailTemplate> resultService = new ResultServiceObject<EmailTemplate>();

            IEnumerable<EmailTemplate> stocks = _uow.EmailTemplateRepository.Select(p => p.EmailTemplateId == idEmailTemplate);

            resultService.Value = stocks.FirstOrDefault();

            return resultService;
        }

        
    }
}
