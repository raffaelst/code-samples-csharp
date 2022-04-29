using Dividendos.Entity.Entities;
using Dividendos.Entity.Enum;
using Dividendos.Entity.Model;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dividendos.Service
{
    public class FinancialProductService : BaseService, IFinancialProductService
    {
        public FinancialProductService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public ResultServiceObject<FinancialInstitution> GetFinancialInstitutionByExternalCode(string externalCode)
        {
            ResultServiceObject<FinancialInstitution> resultService = new ResultServiceObject<FinancialInstitution>();

            FinancialInstitution financialInstitution = _uow.FinancialInstitutionRepository.Select(item => item.ExternalCode == externalCode).FirstOrDefault();

            resultService.Value = financialInstitution;

            return resultService;
        }

        public ResultServiceObject<FinancialInstitution> GetFinancialInstitutionByGuid(Guid financialInstitutionGuid)
        {
            ResultServiceObject<FinancialInstitution> resultService = new ResultServiceObject<FinancialInstitution>();

            FinancialInstitution financialInstitution = _uow.FinancialInstitutionRepository.Select(item => item.FinancialInstitutionGuid == financialInstitutionGuid).FirstOrDefault();

            resultService.Value = financialInstitution;

            return resultService;
        }

        public ResultServiceObject<FinancialInstitution> GetFinancialInstitutionByID(Guid financialInstitutionGuid)
        {
            ResultServiceObject<FinancialInstitution> resultService = new ResultServiceObject<FinancialInstitution>();

            FinancialInstitution financialInstitution = _uow.FinancialInstitutionRepository.Select(item => item.FinancialInstitutionGuid == financialInstitutionGuid).FirstOrDefault();

            resultService.Value = financialInstitution;

            return resultService;
        }

        public ResultServiceObject<Product> GetFinancialProductByExternalCode(string externalCode)
        {
            ResultServiceObject<Product> resultService = new ResultServiceObject<Product>();

            Product product = _uow.ProductRepository.Select(item => item.ExternalName == externalCode).FirstOrDefault();

            resultService.Value = product;

            return resultService;
        }


        public ResultServiceObject<Product> GetFinancialProductByGuid(Guid productGuid)
        {
            ResultServiceObject<Product> resultService = new ResultServiceObject<Product>();

            Product product = _uow.ProductRepository.Select(item => item.ProductGuid == productGuid).FirstOrDefault();

            resultService.Value = product;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<ProductUserView>> GetProductsByCategoryAndTrader(ProductCategoryEnum productCategory, long idTrader)
        {
            ResultServiceObject<IEnumerable<ProductUserView>> resultService = new ResultServiceObject<IEnumerable<ProductUserView>>();

            IEnumerable<ProductUserView> productUsers = _uow.ProductUserViewRepository.GetProductsByCategoryAndTrader((int)productCategory, idTrader);

            resultService.Value = productUsers;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<ProductUserView>> GetProductsByTrader(long idTrader)
        {
            ResultServiceObject<IEnumerable<ProductUserView>> resultService = new ResultServiceObject<IEnumerable<ProductUserView>>();

            IEnumerable<ProductUserView> productUsers = _uow.ProductUserViewRepository.GetProductsByTrader(idTrader);

            resultService.Value = productUsers;

            return resultService;
        }


        public ResultServiceObject<IEnumerable<ProductUserView>> GetAllProductsByUser(string userID)
        {
            ResultServiceObject<IEnumerable<ProductUserView>> resultService = new ResultServiceObject<IEnumerable<ProductUserView>>();

            IEnumerable<ProductUserView> productUsers = _uow.ProductUserViewRepository.GetProductsByUser(userID);

            resultService.Value = productUsers;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<ProductUserView>> GetAllProductsByUserAndType(string userID, ProductCategoryEnum productCategory)
        {
            ResultServiceObject<IEnumerable<ProductUserView>> resultService = new ResultServiceObject<IEnumerable<ProductUserView>>();

            IEnumerable<ProductUserView> productUsers = _uow.ProductUserViewRepository.GetProductsByUserAndType(userID, (int)productCategory);

            resultService.Value = productUsers;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<ProductUserView>> GetAllProductsByUserAndListOfTypes(string userID, List<ProductCategoryEnum> productCategories)
        {
            ResultServiceObject<IEnumerable<ProductUserView>> resultService = new ResultServiceObject<IEnumerable<ProductUserView>>();

            IEnumerable<ProductUserView> productUsers = _uow.ProductUserViewRepository.GetAllProductsByUserAndListOfTypes(userID, productCategories);

            resultService.Value = productUsers;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<ProductUserView>> GetCryptosWithLogoByUserAndType(string userID, ProductCategoryEnum productCategory)
        {
            ResultServiceObject<IEnumerable<ProductUserView>> resultService = new ResultServiceObject<IEnumerable<ProductUserView>>();

            IEnumerable<ProductUserView> productUsers = _uow.ProductUserViewRepository.GetCryptosWithLogoByUserAndType(userID, (int)productCategory);

            resultService.Value = productUsers;

            return resultService;
        }

        public ResultServiceObject<ProductUser> Update(ProductUser productUser)
        {
            ResultServiceObject<ProductUser> resultService = new ResultServiceObject<ProductUser>();

            ProductUser productUsers = _uow.ProductUserRepository.Update(productUser);

            resultService.Value = productUsers;

            return resultService;
        }

        public ResultServiceObject<ProductUser> UpdateAveragePrice(string productUserGuid, decimal averagePrice)
        {
            ResultServiceObject<ProductUser> resultService = new ResultServiceObject<ProductUser>();

            _uow.ProductUserRepository.UpdateAveragePrice(productUserGuid, averagePrice);

            resultService.Value = new ProductUser() { ProductUserGuid = Guid.Parse(productUserGuid), AveragePrice = averagePrice };

            return resultService;
        }

        public ResultServiceObject<Product> ProductUpdate(Product product)
        {
            ResultServiceObject<Product> resultService = new ResultServiceObject<Product>();

            Product productUsers = _uow.ProductRepository.Update(product);

            resultService.Value = productUsers;

            return resultService;
        }

        public ResultServiceObject<ProductUser> Insert(ProductUser productUser)
        {
            ResultServiceObject<ProductUser> resultService = new ResultServiceObject<ProductUser>();

            productUser.Active = true;
            productUser.CreatedDate = DateTime.Now;
            productUser.ProductUserGuid = Guid.NewGuid();

            productUser.ProductUserID  = _uow.ProductUserRepository.Insert(productUser);

            resultService.Value = productUser;

            return resultService;
        }

        public ResultServiceObject<FinancialInstitution> InsertFinancialInstitution(FinancialInstitution financialInstitution)
        {
            ResultServiceObject<FinancialInstitution> resultService = new ResultServiceObject<FinancialInstitution>();


            financialInstitution.CreatedDate = DateTime.Now;
            financialInstitution.FinancialInstitutionGuid = Guid.NewGuid();

            financialInstitution.FinancialInstitutionID = (int)_uow.FinancialInstitutionRepository.Insert(financialInstitution);

            resultService.Value = financialInstitution;

            return resultService;
        }

        public void InsertOrUpdate(ProductUser productUser)
        {
            if (productUser.ProductUserID != 0)
            {
                this.Update(productUser);
            }
            else
            {
                this.Insert(productUser);
            }
        }

        public ResultServiceObject<IEnumerable<FinancialInstitution>> GetAllFinancialInstitution(string name)
        {
            ResultServiceObject<IEnumerable<FinancialInstitution>> resultService = new ResultServiceObject<IEnumerable<FinancialInstitution>>();

            IEnumerable<FinancialInstitution> financialInstitutions = _uow.FinancialInstitutionRepository.GetAllFinancialInstitution(name);

            resultService.Value = financialInstitutions;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<ProductCategory>> GetAllFinancialProductCategory()
        {
            ResultServiceObject<IEnumerable<ProductCategory>> resultService = new ResultServiceObject<IEnumerable<ProductCategory>>();

            IEnumerable<ProductCategory> productCategories = _uow.ProductCategoryRepository.GetAll();

            resultService.Value = productCategories;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<ProductCategory>> GetFinancialProductCategoryByType(List<ProductCategoryEnum> productCategoryEnums)
        {
            ResultServiceObject<IEnumerable<ProductCategory>> resultService = new ResultServiceObject<IEnumerable<ProductCategory>>();

            IEnumerable<ProductCategory> productCategories = _uow.ProductCategoryRepository.GetAll();

            resultService.Value = productCategories;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<Product>> GetAllFinancialProductByCategory(string financialProductCategoryGuid)
        {
            ResultServiceObject<IEnumerable<Product>> resultService = new ResultServiceObject<IEnumerable<Product>>();

            IEnumerable<Product> products = _uow.ProductRepository.GetAllFinancialProductByCategory(financialProductCategoryGuid);

            resultService.Value = products;

            return resultService;
        }


        public ResultServiceObject<ProductUser> GetFinancialProductUserByGuid(Guid productGuid)
        {
            ResultServiceObject<ProductUser> resultService = new ResultServiceObject<ProductUser>();

            ProductUser productUser = _uow.ProductUserRepository.Select(item => item.ProductUserGuid == productGuid).FirstOrDefault();

            resultService.Value = productUser;

            return resultService;
        }

        public ResultServiceObject<Product> AddProduct(Product product)
        {
            ResultServiceObject<Product> resultService = new ResultServiceObject<Product>();

            product.ProductGuid = Guid.NewGuid();

            product.ProductID = (int)_uow.ProductRepository.Insert(product);

            resultService.Value = product;

            return resultService;
        }

        public ResultServiceObject<Product> AddNewProductIfNotExist(string productExternalName, ProductCategoryEnum productCategoryEnum)
        {
            ResultServiceObject<Product> resultService = new ResultServiceObject<Product>();

            Product product = _uow.ProductRepository.Select(item => item.ExternalName == productExternalName && item.ProductCategoryID == (int)productCategoryEnum).FirstOrDefault();

            if (product == null)
            {
                product = new Product();
                product.ExternalName = productExternalName;
                product.Description = productExternalName;
                product.ProductCategoryID = (int)productCategoryEnum;
                resultService = this.AddProduct(product);
            }
            else
            {
                resultService.Value = product;
            }

            return resultService;
        }


        public ResultServiceObject<IEnumerable<CryptoBrokerView>> GetCryptosBroker(string userID)
        {
            ResultServiceObject<IEnumerable<CryptoBrokerView>> resultService = new ResultServiceObject<IEnumerable<CryptoBrokerView>>();

            IEnumerable<CryptoBrokerView> productUsers = _uow.CryptoCurrencyViewRepository.GetCryptosBroker(userID);

            resultService.Value = productUsers;


            return resultService;
        }
    }
}
