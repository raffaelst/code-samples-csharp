using Dividendos.Entity.Entities;
using Dividendos.Repository.Interface.GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Repository.Interface
{
    public interface IPartnerRepository : IRepository<Partner>
    {
        Partner GetButtonAvailable(string idUser);
        IEnumerable<Partner> GetAllButtonsAvailable(string idUser);
    }
}
