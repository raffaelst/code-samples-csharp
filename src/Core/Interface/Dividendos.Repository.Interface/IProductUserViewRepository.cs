using Dividendos.Entity.Entities;
using Dividendos.Entity.Enum;
using Dividendos.Repository.Interface.GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Repository.Interface
{
    public interface IProductUserViewRepository : IRepository<ProductUserView>
    {
        IEnumerable<ProductUserView> GetProductsByCategoryAndTrader(int productCategory, long idTrader);
        IEnumerable<ProductUserView> GetProductsByUser(string idUser);

        IEnumerable<ProductUserView> GetProductsByTrader(long idTrader);

        IEnumerable<ProductUserView> GetProductsByUserAndType(string idUser, int productCategory);

        IEnumerable<ProductUserView> GetCryptosWithLogoByUserAndType(string idUser, int productCategory);

        IEnumerable<ProductUserView> GetAllProductsByUserAndListOfTypes(string idUser, List<ProductCategoryEnum> productCategoryEnums);
    }
}
