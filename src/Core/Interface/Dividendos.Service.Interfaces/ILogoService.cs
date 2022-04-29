using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Service.Interface
{
    public interface ILogoService : IBaseService
    {
        ResultServiceObject<Logo> Insert(Logo logo);
        ResultServiceObject<IEnumerable<Logo>> GetAll();
        ResultServiceObject<IEnumerable<Logo>> GetGreater(long idLogo);
        ResultServiceObject<Logo> GetByCompanyCode(string companyCode);
        ResultServiceObject<Logo> Updte(Logo logo);
        ResultServiceObject<IEnumerable<Logo>> GetAllWithPage(int page);
        ResultServiceObject<Logo> GetById(long idLogo);
    }
}
