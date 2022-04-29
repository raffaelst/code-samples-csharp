using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Service.Interface
{
    public interface IContactPhoneService : IBaseService
    {
        ResultServiceObject<ContactPhone> Update(ContactPhone contactPhone);
        ResultServiceObject<ContactPhone> Insert(ContactPhone contactPhone);
        ResultServiceObject<IEnumerable<ContactPhone>> GetAllByIdSourceInfoAndIdUser(int idSourceInfo, string idUser);
    }
}
