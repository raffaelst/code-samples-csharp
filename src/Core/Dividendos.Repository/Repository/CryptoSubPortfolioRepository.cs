using Dapper.Contrib.Extensions;
using Dividendos.Entity.Entities;
using Dividendos.Repository.GenericRepository;
using Dividendos.Repository.Interface;
using Dividendos.Repository.Interface.GenericRepository;
using Dividendos.Repository.Interface.UoW;
using Dapper;
using System.Linq;
using System.Collections.Generic;
using System;


namespace Dividendos.Repository.Repository
{
    public class CryptoSubPortfolioRepository : Repository<CryptoSubPortfolio>, ICryptoSubPortfolioRepository
    {
        private IUnitOfWork _unitOfWork;

        public CryptoSubPortfolioRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
