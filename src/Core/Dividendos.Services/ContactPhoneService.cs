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
    public class ContactPhoneService : BaseService, IContactPhoneService
    {
        public ContactPhoneService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public ResultServiceObject<ContactPhone> Update(ContactPhone contactPhone)
        {
            ResultServiceObject<ContactPhone> resultService = new ResultServiceObject<ContactPhone>();
            resultService.Value = _uow.ContactPhoneRepository.Update(contactPhone);

            return resultService;
        }

        public ResultServiceObject<ContactPhone> Insert(ContactPhone contactPhone)
        {
            ResultServiceObject<ContactPhone> resultService = new ResultServiceObject<ContactPhone>();
            contactPhone.IdContactPhone = _uow.ContactPhoneRepository.Insert(contactPhone);
            resultService.Value = contactPhone;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<ContactPhone>> GetAllByIdSourceInfoAndIdUser(int idSourceInfo, string idUser)
        {
            ResultServiceObject<IEnumerable<ContactPhone>> resultService = new ResultServiceObject<IEnumerable<ContactPhone>>();

            IEnumerable<ContactPhone> phones = _uow.ContactPhoneRepository.Select(p => p.IdSourceInfo == idSourceInfo && p.IdUser == idUser);

            resultService.Value = phones;

            return resultService;
        }
    }
}
