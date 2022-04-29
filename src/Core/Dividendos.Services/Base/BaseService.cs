using FluentValidation.Results;
using Dividendos.CrossCutting.Identity.Models;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using Dividendos.Service.Validator.Common;
using Dividendos.Service.Validator.User;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using K.UnitOfWorkBase;

namespace Dividendos.Service
{
    public class BaseService : IBaseService
    {
        internal IUnitOfWork _uow;
    }
}
