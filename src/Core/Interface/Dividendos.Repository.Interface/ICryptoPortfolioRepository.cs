using Dividendos.Entity.Entities;
using Dividendos.Repository.Interface.GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Repository.Interface
{
    public interface ICryptoPortfolioRepository : IRepository<CryptoPortfolio>
    {
        void UpdateCalculatePerformanceDate(long idCryptoPortfolio, DateTime calculatePerformanceDate);
        IEnumerable<CryptoPortfolio> GetByUserAndStatus(string idUser, bool status, bool? manual);
        void UpdateName(long idCryptoPortfolio, string name);
    }
}
