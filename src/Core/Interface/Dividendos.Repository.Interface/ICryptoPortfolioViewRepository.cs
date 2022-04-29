using Dividendos.Entity.Views;
using Dividendos.Repository.Interface.GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;


namespace Dividendos.Repository.Interface
{
    public interface ICryptoPortfolioViewRepository : IRepository<CryptoPortfolioView>
    {
        IEnumerable<CryptoPortfolioView> GetByUser(string idUser);
        IEnumerable<CryptoSubportfolioItemView> GetByCryptoPortfolio(Guid guidCryptoPortfolio);
        IEnumerable<CryptoSubportfolioItemView> GetBySubCryptoPortfolio(long idCryptoPortfolio, long idCryptoSubPortfolio);
    }
}
