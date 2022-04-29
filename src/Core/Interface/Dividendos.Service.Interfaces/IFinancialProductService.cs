using Dividendos.Entity.Entities;
using Dividendos.Entity.Enum;
using Dividendos.Entity.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Service.Interface
{
    public interface IFinancialProductService : IBaseService
    {
        ResultServiceObject<IEnumerable<ProductUserView>> GetProductsByCategoryAndTrader(ProductCategoryEnum productCategory, long idTrader);

        ResultServiceObject<IEnumerable<ProductUserView>> GetProductsByTrader(long idTrader);

        ResultServiceObject<IEnumerable<ProductUserView>> GetAllProductsByUser(string userID);

        ResultServiceObject<ProductUser> Update(ProductUser productUser);

        ResultServiceObject<ProductUser> Insert(ProductUser productUser);

        ResultServiceObject<FinancialInstitution> GetFinancialInstitutionByExternalCode(string externalCode);

        ResultServiceObject<Product> GetFinancialProductByExternalCode(string externalCode);

        void InsertOrUpdate(ProductUser productUser);

        ResultServiceObject<FinancialInstitution> InsertFinancialInstitution(FinancialInstitution financialInstitution);

        ResultServiceObject<IEnumerable<ProductUserView>> GetAllProductsByUserAndType(string userID, ProductCategoryEnum productCategory);

        ResultServiceObject<IEnumerable<FinancialInstitution>> GetAllFinancialInstitution(string name);

        ResultServiceObject<IEnumerable<Product>> GetAllFinancialProductByCategory(string financialProductCategoryGuid);

        ResultServiceObject<IEnumerable<ProductCategory>> GetAllFinancialProductCategory();

        ResultServiceObject<FinancialInstitution> GetFinancialInstitutionByID(Guid financialInstitutionGuid);

        ResultServiceObject<Product> GetFinancialProductByGuid(Guid productGuid);

        ResultServiceObject<ProductUser> GetFinancialProductUserByGuid(Guid productGuid);

        ResultServiceObject<Product> ProductUpdate(Product product);

        ResultServiceObject<IEnumerable<ProductUserView>> GetCryptosWithLogoByUserAndType(string userID, ProductCategoryEnum productCategory);

        ResultServiceObject<Product> AddNewProductIfNotExist(string productExternalName, ProductCategoryEnum productCategoryEnum);

        ResultServiceObject<Product> AddProduct(Product product);

        ResultServiceObject<IEnumerable<CryptoBrokerView>> GetCryptosBroker(string userID);

        ResultServiceObject<ProductUser> UpdateAveragePrice(string productUserGuid, decimal averagePrice);

        public ResultServiceObject<IEnumerable<ProductUserView>> GetAllProductsByUserAndListOfTypes(string userID, List<ProductCategoryEnum> productCategories);

        ResultServiceObject<IEnumerable<ProductCategory>> GetFinancialProductCategoryByType(List<ProductCategoryEnum> productCategoryEnums);
    }
}
