using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Service.Interface
{
    public interface IContactDetailsService : IBaseService
    {
        ResultServiceObject<ContactDetails> Update(ContactDetails contactDetails);
        ResultServiceObject<ContactDetails> Insert(ContactDetails contactDetails);
        ResultServiceObject<ContactDetails> GetByIdSourceInfoAndIdUser(int idSourceInfo, string idUser);
    }
}
