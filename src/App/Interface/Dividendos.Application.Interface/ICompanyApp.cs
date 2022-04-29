using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using Dividendos.Application.Interface.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Views;
using Dividendos.API.Model.Response.Company;

namespace Dividendos.Application.Interface
{
    public interface ICompanyApp
    {
        ResultResponseObject<IEnumerable<CompanyView>> GetAll();
        ResultResponseObject<IEnumerable<CompanyVM>> GetByName(string name);
        Task UpdateCompanyLogo();
        Task Generatefiles();        
    }
}