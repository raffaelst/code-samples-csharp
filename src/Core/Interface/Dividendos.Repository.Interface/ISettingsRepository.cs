using Dividendos.Entity.Entities;
using Dividendos.Repository.Interface.GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Repository.Interface
{
    public interface ISettingsRepository : IRepository<Settings>
    {
        Settings UpdateAutoSync(Settings settings);
        Settings GetByIdUser(string idUser);
    }
}
