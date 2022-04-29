using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Service.Interface
{
    public interface IEmailTemplateService : IBaseService
    {
        ResultServiceObject<IEnumerable<EmailTemplate>> GetAll();

        ResultServiceObject<EmailTemplate> GetById(int idEmailTemplate);
    }
}
