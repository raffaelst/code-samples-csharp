using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Dividendos.Repository.Interface.GenericRepository
{
    public interface IRepository<TEntity> : K.IRepository.IRepositoryBase<TEntity> where TEntity : class, new()
    {
    }
}
