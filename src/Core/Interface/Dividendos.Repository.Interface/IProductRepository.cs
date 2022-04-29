using Dividendos.Entity.Entities;
using Dividendos.Entity.Enum;
using Dividendos.Repository.Interface.GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Repository.Interface
{
    public interface IProductRepository : IRepository<Product>
    {
        IEnumerable<Product> GetAllFinancialProductByCategory(string financialProductCategoryGuid);

        IEnumerable<Product> GetAllFinancialProductByCategories(List<ProductCategoryEnum> productCategoryEnums);
    }
}
