using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dividendos.Service
{
    public class ContactDetailsService : BaseService, IContactDetailsService
    {
        public ContactDetailsService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public ResultServiceObject<ContactDetails> Update(ContactDetails contactDetails)
        {
            ResultServiceObject<ContactDetails> resultService = new ResultServiceObject<ContactDetails>();
            resultService.Value = _uow.ContactDetailsRepository.Update(contactDetails);

            return resultService;
        }

        public ResultServiceObject<ContactDetails> Insert(ContactDetails contactDetails)
        {
            ResultServiceObject<ContactDetails> resultService = new ResultServiceObject<ContactDetails>();
            contactDetails.IdContactDetails = _uow.ContactDetailsRepository.Insert(contactDetails);
            resultService.Value = contactDetails;

            return resultService;
        }

        public ResultServiceObject<ContactDetails> GetByIdSourceInfoAndIdUser(int idSourceInfo, string idUser)
        {
            ResultServiceObject<ContactDetails> resultService = new ResultServiceObject<ContactDetails>();

            IEnumerable<ContactDetails> contactDetails = _uow.ContactDetailsRepository.Select(p => p.IdSourceInfo == idSourceInfo && p.IdUser == idUser);

            resultService.Value = contactDetails.FirstOrDefault();

            return resultService;
        }
    }
}
