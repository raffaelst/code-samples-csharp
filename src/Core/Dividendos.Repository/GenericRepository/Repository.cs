using Dividendos.Repository.Context;
using Dividendos.Repository.Interface.GenericRepository;
using Dividendos.Repository.Interface.UoW;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using K.UnitOfWorkBase;

namespace Dividendos.Repository.GenericRepository
{
    public class Repository<TEntity> : K.Repository.RepositoryBase<TEntity>, IRepository<TEntity> where TEntity : class, new()
    {
        private IUnitOfWorkBase _unitOfWorkBase;

        public Repository(IUnitOfWorkBase unitOfWorkBase) : base(unitOfWorkBase)
        {
            _unitOfWorkBase = unitOfWorkBase;
        }
    }
}