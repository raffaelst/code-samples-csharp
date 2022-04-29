using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Service.Interface
{
    public interface ISettingsService : IBaseService
    {
        ResultServiceObject<Settings> ChangeSettings(Settings settings);
        ResultServiceObject<Settings> GetByUser(string idUser);
        ResultServiceObject<Settings> Insert(Settings settings);

        ResultServiceObject<Settings> InitAndCreate(string idUser);
    }
}
