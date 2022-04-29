using AutoMapper;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Bounds;
using Dividendos.API.Model.Response.Common;
using Dividendos.API.Model.Response.FinancialProducts;
using Dividendos.API.Model.Response.v2;
using Dividendos.Application.Base;
using Dividendos.Application.Interface;
using Dividendos.Application.Interface.Model;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Enum;
using Dividendos.Entity.Model;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Dividendos.Application
{
    public class FinancialProductsApp : BaseApp, IFinancialProductsApp
    {
        private readonly IFinancialProductService _financialProductService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly IGlobalAuthenticationService _globalAuthenticationService;
        private readonly ICryptoCurrencyService _cryptoCurrencyService;
        private readonly ICacheService _cacheService;

        public FinancialProductsApp(IMapper mapper,
            IUnitOfWork uow,
            IFinancialProductService financialProductService,
            IGlobalAuthenticationService globalAuthenticationService,
            ICryptoCurrencyService cryptoCurrencyService,
            ICacheService cacheService)
        {
            _financialProductService = financialProductService;
            _mapper = mapper;
            _uow = uow;
            _globalAuthenticationService = globalAuthenticationService;
            _cryptoCurrencyService = cryptoCurrencyService;
            _cacheService = cacheService;
        }


        public void UpdateListOfBounds()
        {
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://api.apprendafixa.com.br/dividendosme/fundos/"))
                {
                    request.Headers.TryAddWithoutValidation("x-api-key", "Olaoi0TuFp9sTwvKvA9H44lv1sEKnMcu3noJLwWA");

                    var response = httpClient.SendAsync(request).Result;

                    dynamic resultAPI = JsonConvert.DeserializeObject<ExpandoObject>(response.Content.ReadAsStringAsync().Result);
                    IDictionary<string, object> propertyValues = resultAPI;

                    foreach (var property in propertyValues)
                    {
                        dynamic value = property.Value;

                        foreach (var item in value)
                        {
                            using (_uow.Create())
                            {
                                _financialProductService.AddNewProductIfNotExist(item.nome, ProductCategoryEnum.Funds);
                            }
                        }
                    }
                }
            }
        }

        public ResultResponseObject<IEnumerable<FinancialProductDetailVM>> GetAllByLoggedUser()
        {
            ResultServiceObject<IEnumerable<ProductUserView>> resultService = null;

            using (_uow.Create())
            {
                resultService = _financialProductService.GetAllProductsByUser(_globalAuthenticationService.IdUser);
                ResultServiceObject<IEnumerable<CryptoCurrency>> cryptoCurrencies = _cryptoCurrencyService.GetAll();

                foreach (var item in resultService.Value)
                {
                    if (item.ProductCategoryID.Equals((int)ProductCategoryEnum.CryptoCurrencies))
                    {
                        item.CurrentValue = cryptoCurrencies.Value.Where(currency => currency.Name.Equals(item.ExternalName)).Sum(totalSum => totalSum.MarketPrice) * item.CurrentValue;
                    }
                }
            }

            ResultResponseObject<IEnumerable<FinancialProductDetailVM>> result = _mapper.Map<ResultResponseObject<IEnumerable<FinancialProductDetailVM>>>(resultService);

            return result;
        }

        public ResultResponseObject<FinancialProductCryptoVM> GetCryptosByLoggedUser()
        {
            List<FinancialProductDetailVM> financialProductDetailVMs = new List<FinancialProductDetailVM>();

            ResultServiceObject<IEnumerable<ProductUserView>> resultService = null;

            using (_uow.Create())
            {
                resultService = _financialProductService.GetCryptosWithLogoByUserAndType(_globalAuthenticationService.IdUser, ProductCategoryEnum.CryptoCurrencies);

                ResultServiceObject<IEnumerable<CryptoCurrency>> cryptoCurrencies = _cryptoCurrencyService.GetAll();

                foreach (var item in resultService.Value)
                {
                    var valueWithMarketPrice = cryptoCurrencies.Value.Where(currency => currency.Name.Equals(item.ExternalName)).Sum(totalSum => totalSum.MarketPrice) * item.CurrentValue;

                    var finalcialProduct = financialProductDetailVMs.FirstOrDefault(value => value.FinancialInstitution.Equals(item.FinancialInstitution));

                    if (finalcialProduct == null)
                    {
                        financialProductDetailVMs.Add(new FinancialProductDetailVM() { FinancialInstitution = item.FinancialInstitution, InternalCurrentValue = valueWithMarketPrice });
                    }
                    else
                    {
                        finalcialProduct.InternalCurrentValue = finalcialProduct.InternalCurrentValue + valueWithMarketPrice;
                    }
                }
            }

            decimal total = 0;

            foreach (var item in financialProductDetailVMs)
            {
                total = total + item.InternalCurrentValue;
                item.CurrentValue = item.InternalCurrentValue.ToString("n2", new CultureInfo("pt-br"));
            }

            ResultResponseObject<FinancialProductCryptoVM> resultResponseObject = null;

            if (total != 0)
            {
                resultResponseObject = new ResultResponseObject<FinancialProductCryptoVM>() { Value = new FinancialProductCryptoVM() { Cryptos = financialProductDetailVMs, Total = total.ToString("n2", new CultureInfo("pt-br")) }, Success = true };
            }
            else
            {
                resultResponseObject = new ResultResponseObject<FinancialProductCryptoVM>() { Value = null, Success = true };
            }

            return resultResponseObject;
        }

        public ResultResponseObject<IEnumerable<FinancialProductDetailVM>> GetTDByLoggedUser()
        {
            ResultResponseObject<IEnumerable<FinancialProductDetailVM>> result = null;

            using (_uow.Create())
            {
                ResultServiceObject<IEnumerable<ProductUserView>> resultService = _financialProductService.GetAllProductsByUserAndType(_globalAuthenticationService.IdUser, ProductCategoryEnum.TesouroDireto);

                result = _mapper.Map<ResultResponseObject<IEnumerable<FinancialProductDetailVM>>>(resultService);
            }

            return result;
        }

        public ResultResponseObject<BoundsWrapperVM> GetBoundsExtractByLoggedUser()
        {
            decimal total = 0;

            BoundsWrapperVM boundsWrapperVM = new BoundsWrapperVM() { Bounds = new List<FinancialProductDetailVM>() };

            using (_uow.Create())
            {
                List<ProductCategoryEnum> productCategoryEnums = new List<ProductCategoryEnum>();
                productCategoryEnums.Add(ProductCategoryEnum.Funds);
                ResultServiceObject<IEnumerable<ProductUserView>> resultService = _financialProductService.GetAllProductsByUserAndListOfTypes(_globalAuthenticationService.IdUser, productCategoryEnums);

                foreach (var itemProductUserView in resultService.Value)
                {
                    total = total + itemProductUserView.CurrentValue;

                    boundsWrapperVM.Bounds.Add(new FinancialProductDetailVM() { CurrentValue = itemProductUserView.CurrentValue.ToString("n2", new CultureInfo("pt-br")), FinancialInstitution = itemProductUserView.FinancialInstitution, ProductName = itemProductUserView.ProductName, ProductCategory = itemProductUserView.ProductCategory, ProductUserGuid = itemProductUserView.ProductUserGuid });
                }
            }

            boundsWrapperVM.Total = total.ToString("n2", new CultureInfo("pt-br"));

            ResultResponseObject<BoundsWrapperVM> result = new ResultResponseObject<BoundsWrapperVM>() { Success = true, Value = boundsWrapperVM };

            return result;
        }

        public ResultResponseObject<BoundsWrapperVM> GetFixedIncomeExtracByLoggedUser()
        {
            decimal total = 0;

            BoundsWrapperVM boundsWrapperVM = new BoundsWrapperVM() { Bounds = new List<FinancialProductDetailVM>() };

            using (_uow.Create())
            {
                List<ProductCategoryEnum> productCategoryEnums = new List<ProductCategoryEnum>();
                productCategoryEnums.Add(ProductCategoryEnum.TesouroDireto);
                productCategoryEnums.Add(ProductCategoryEnum.CDB);
                productCategoryEnums.Add(ProductCategoryEnum.LCA);
                productCategoryEnums.Add(ProductCategoryEnum.LCI);
                productCategoryEnums.Add(ProductCategoryEnum.Savings);
                ResultServiceObject<IEnumerable<ProductUserView>> resultService = _financialProductService.GetAllProductsByUserAndListOfTypes(_globalAuthenticationService.IdUser, productCategoryEnums);

                foreach (var itemProductUserView in resultService.Value)
                {
                    total = total + itemProductUserView.CurrentValue;

                    boundsWrapperVM.Bounds.Add(new FinancialProductDetailVM() { CurrentValue = itemProductUserView.CurrentValue.ToString("n2", new CultureInfo("pt-br")), FinancialInstitution = itemProductUserView.FinancialInstitution, ProductName = itemProductUserView.ProductName, ProductCategory = itemProductUserView.ProductCategory, ProductUserGuid = itemProductUserView.ProductUserGuid });
                }
            }

            boundsWrapperVM.Total = total.ToString("n2", new CultureInfo("pt-br"));

            ResultResponseObject<BoundsWrapperVM> result = new ResultResponseObject<BoundsWrapperVM>() { Success = true, Value = boundsWrapperVM };

            return result;
        }

        public ResultResponseObject<StockTypeChart> GetCryptosChartsByLoggedUser()
        {
            ResultResponseObject<StockTypeChart> financialProductsChartReturn = new ResultResponseObject<StockTypeChart>() { Success = true };

            StockTypeChart financialProductsChart = new StockTypeChart();

            financialProductsChart.ListChartLabelValue = new List<ChartLabelValue>();
            decimal total = 0;

            using (_uow.Create())
            {
                var resultService = _financialProductService.GetAllProductsByUserAndType(_globalAuthenticationService.IdUser, ProductCategoryEnum.CryptoCurrencies);
                ResultServiceObject<IEnumerable<CryptoCurrency>> cryptoCurrencies = _cryptoCurrencyService.GetAll();

                if (resultService.Value != null)
                {
                    foreach (var itemProductUser in resultService.Value)
                    {
                        var itemFounded = financialProductsChart.ListChartLabelValue.FirstOrDefault(item => item.Label.Equals(itemProductUser.ProductName));
                        
                        if (itemFounded == null)
                        {
                            ChartLabelValue dataItem = new ChartLabelValue() { Label = itemProductUser.ProductName, InternalLabel = itemProductUser.ExternalName, InternalValue = itemProductUser.CurrentValue };
                            financialProductsChart.ListChartLabelValue.Add(dataItem);
                        }
                        else
                        {
                            itemFounded.InternalValue = itemProductUser.CurrentValue + itemFounded.InternalValue;
                        }
                    }

                    foreach (var itemChart in financialProductsChart.ListChartLabelValue)
                    {
                        var crypto = cryptoCurrencies.Value.Where(productUser => productUser.Name.Equals(itemChart.InternalLabel)).FirstOrDefault();

                        var currentValue = itemChart.InternalValue * crypto.MarketPrice;

                        total += currentValue;

                        itemChart.Value = currentValue.ToString("n2", new CultureInfo("en-us")).Replace(",", string.Empty);
                    }
                }
            }

            financialProductsChart.TotalMarket = total.ToString("n2", new CultureInfo("pt-br"));
            financialProductsChart.LastUpdated = DateTime.Now.ToString("HH:mm:ss");

            financialProductsChartReturn.Value = financialProductsChart;

            return financialProductsChartReturn;
        }

        public ResultResponseObject<StockTypeChart> GetBoundsChartsByLoggedUser()
        {
            ResultResponseObject<StockTypeChart> financialProductsChartReturn = new ResultResponseObject<StockTypeChart>() { Success = true };

            StockTypeChart financialProductsChart = new StockTypeChart();

            financialProductsChart.ListChartLabelValue = new List<ChartLabelValue>();
            decimal total = 0;

            using (_uow.Create())
            {
                List<ProductCategoryEnum> productCategoryEnums = new List<ProductCategoryEnum>();
                productCategoryEnums.Add(ProductCategoryEnum.Funds);

                var resultService = _financialProductService.GetAllProductsByUserAndListOfTypes(_globalAuthenticationService.IdUser, productCategoryEnums);


                foreach (var item in resultService.Value)
                {
                    ChartLabelValue dataItem = new ChartLabelValue() { Label = item.ProductName, Value = item.CurrentValue.ToString("n2", new CultureInfo("en-us")).Replace(",", string.Empty) };
                    total += item.CurrentValue;
                    financialProductsChart.ListChartLabelValue.Add(dataItem);
                }
            }

            financialProductsChart.TotalMarket = total.ToString("n2", new CultureInfo("pt-br"));
            financialProductsChart.LastUpdated = DateTime.Now.ToString("HH:mm:ss");

            financialProductsChartReturn.Value = financialProductsChart;

            return financialProductsChartReturn;
        }

        public ResultResponseObject<StockTypeChart> GetFixedIncomeChartsByLoggedUser()
        {
            ResultResponseObject<StockTypeChart> financialProductsChartReturn = new ResultResponseObject<StockTypeChart>() { Success = true };

            StockTypeChart financialProductsChart = new StockTypeChart();

            financialProductsChart.ListChartLabelValue = new List<ChartLabelValue>();
            decimal total = 0;

            using (_uow.Create())
            {
                List<ProductCategoryEnum> productCategoryEnums = new List<ProductCategoryEnum>();
                productCategoryEnums.Add(ProductCategoryEnum.TesouroDireto);
                productCategoryEnums.Add(ProductCategoryEnum.Savings);
                productCategoryEnums.Add(ProductCategoryEnum.CDB);
                productCategoryEnums.Add(ProductCategoryEnum.LCA);
                productCategoryEnums.Add(ProductCategoryEnum.LCI);

                var resultService = _financialProductService.GetAllProductsByUserAndListOfTypes(_globalAuthenticationService.IdUser, productCategoryEnums);


                foreach (var item in resultService.Value)
                {
                    ChartLabelValue dataItem = new ChartLabelValue() { Label = item.ProductName, Value = item.CurrentValue.ToString("n2", new CultureInfo("en-us")).Replace(",", string.Empty) };
                    total += item.CurrentValue;
                    financialProductsChart.ListChartLabelValue.Add(dataItem);
                }
            }

            financialProductsChart.TotalMarket = total.ToString("n2", new CultureInfo("pt-br"));
            financialProductsChart.LastUpdated = DateTime.Now.ToString("HH:mm:ss");

            financialProductsChartReturn.Value = financialProductsChart;

            return financialProductsChartReturn;
        }

        public ResultResponseObject<IEnumerable<FinancialInstitutionVM>> GetAllFinancialInstitution(string name)
        {
            ResultServiceObject<IEnumerable<FinancialInstitution>> resultService = null;

            using (_uow.Create())
            {
                resultService = _financialProductService.GetAllFinancialInstitution(name);
            }

            ResultResponseObject<IEnumerable<FinancialInstitutionVM>> result = _mapper.Map<ResultResponseObject<IEnumerable<FinancialInstitutionVM>>>(resultService);

            return result;
        }

        public ResultResponseObject<IEnumerable<FinancialProductCategoryVM>> GetAllFinancialProductCategory()
        {
            ResultServiceObject<IEnumerable<ProductCategory>> resultService = null;

            using (_uow.Create())
            {
                List<ProductCategoryEnum> productCategoryEnums = new List<ProductCategoryEnum>();
                productCategoryEnums.Add(ProductCategoryEnum.TesouroDireto);
                productCategoryEnums.Add(ProductCategoryEnum.CDB);
                productCategoryEnums.Add(ProductCategoryEnum.LCA);
                productCategoryEnums.Add(ProductCategoryEnum.LCI);
                productCategoryEnums.Add(ProductCategoryEnum.Funds);
                productCategoryEnums.Add(ProductCategoryEnum.Savings);

                resultService = _financialProductService.GetFinancialProductCategoryByType(productCategoryEnums);
            }

            ResultResponseObject<IEnumerable<FinancialProductCategoryVM>> result = _mapper.Map<ResultResponseObject<IEnumerable<FinancialProductCategoryVM>>>(resultService);

            return result;
        }

        public ResultResponseObject<IEnumerable<FinancialProductVM>> GetAllFinancialProductByCategory(string financialProductCategoryGuid)
        {
            ResultServiceObject<IEnumerable<Product>> resultService = null;

            using (_uow.Create())
            {
                resultService = _financialProductService.GetAllFinancialProductByCategory(financialProductCategoryGuid);
            }

            ResultResponseObject<IEnumerable<FinancialProductVM>> result = _mapper.Map<ResultResponseObject<IEnumerable<FinancialProductVM>>>(resultService);

            return result;
        }


        public ResultResponseObject<FinancialProductResponse> AddNewProductUser(FinancialProductAddVM financialProductAddVM)
        {
            ResultResponseObject<FinancialProductResponse> result;

            using (_uow.Create())
            {
                ProductUser productUser = new ProductUser();
                productUser.UserID = _globalAuthenticationService.IdUser;
                
                decimal currentValue = 0;
                decimal.TryParse(financialProductAddVM.CurrentValue.Replace(".", string.Empty), NumberStyles.Currency, new CultureInfo("pt-br"), out currentValue);

                productUser.CurrentValue = currentValue;
                productUser.ProductID = _financialProductService.GetFinancialProductByGuid(Guid.Parse(financialProductAddVM.ProductGuid)).Value.ProductID;
                productUser.FinancialInstitutionID = _financialProductService.GetFinancialInstitutionByID(Guid.Parse(financialProductAddVM.FinancialInstitutionGuid)).Value.FinancialInstitutionID;

                ResultServiceObject<ProductUser> resultService = _financialProductService.Insert(productUser);

                result = _mapper.Map<ResultResponseObject<FinancialProductResponse>>(resultService);
            }

            return result;
        }

        public ResultResponseBase DeleteProductUser(string productUserGuid)
        {
            ResultResponseBase resultService = new ResultResponseBase() { Success = false };

            using (_uow.Create())
            {
                ResultServiceObject<ProductUser> resultServiceProduct = _financialProductService.GetFinancialProductUserByGuid(Guid.Parse(productUserGuid));

                if (resultServiceProduct.Value != null)
                {
                    resultServiceProduct.Value.Active = false;

                    _financialProductService.Update(resultServiceProduct.Value);

                    resultService = new ResultResponseBase() { Success = true };
                }
            }

            return resultService;
        }

        public ResultResponseObject<FinancialProductResponse> UpdateProductUser(FinancialProductEditVM financialProductEditVM)
        {
            ResultResponseObject<FinancialProductResponse> result;

            using (_uow.Create())
            {
                ResultServiceObject<ProductUser> productUserResult = _financialProductService.GetFinancialProductUserByGuid(Guid.Parse(financialProductEditVM.ProductGuid));

                decimal currentValue = 0;
                decimal.TryParse(financialProductEditVM.CurrentValue.Replace(".", string.Empty), NumberStyles.Currency, new CultureInfo("pt-br"), out currentValue);

                productUserResult.Value.CurrentValue = currentValue;

                ResultServiceObject<ProductUser> resultService = _financialProductService.Update(productUserResult.Value);

                result = _mapper.Map<ResultResponseObject<FinancialProductResponse>>(resultService);
            }

            return result;
        }

        public ResultResponseObject<BoundsSummaryVM> GetFixedIncomeSummaryByLoggedUser()
        {
            List<BoundsDetailVM> boundsDetailVM = new List<BoundsDetailVM>();

            ResultServiceObject<IEnumerable<ProductUserView>> resultService = null;

            using (_uow.Create())
            {
                List<ProductCategoryEnum> productCategoryEnums = new List<ProductCategoryEnum>();
                productCategoryEnums.Add(ProductCategoryEnum.TesouroDireto);
                productCategoryEnums.Add(ProductCategoryEnum.CDB);
                productCategoryEnums.Add(ProductCategoryEnum.LCA);
                productCategoryEnums.Add(ProductCategoryEnum.LCI);
                productCategoryEnums.Add(ProductCategoryEnum.Savings);

                resultService = _financialProductService.GetAllProductsByUserAndListOfTypes(_globalAuthenticationService.IdUser, productCategoryEnums);


                foreach (var item in resultService.Value)
                {
                    var finalcialProduct = boundsDetailVM.FirstOrDefault(value => value.ProductName.Equals(item.ProductName));

                    if (finalcialProduct == null)
                    {
                        boundsDetailVM.Add(new BoundsDetailVM() { FinancialInstitution = item.FinancialInstitution, ProductName = item.ProductName, InternalCurrentValue = item.CurrentValue });
                    }
                    else
                    {
                        finalcialProduct.InternalCurrentValue = finalcialProduct.InternalCurrentValue + item.CurrentValue;
                    }
                }
            }

            decimal total = 0;

            foreach (var item in boundsDetailVM)
            {
                total = total + item.InternalCurrentValue;
                item.CurrentValue = item.InternalCurrentValue.ToString("n2", new CultureInfo("pt-br"));
            }

            ResultResponseObject<BoundsSummaryVM> resultResponseObject = null;

            if (total != 0)
            {
                resultResponseObject = new ResultResponseObject<BoundsSummaryVM>() { Value = new BoundsSummaryVM() { Bounds = boundsDetailVM, Total = total.ToString("n2", new CultureInfo("pt-br")) }, Success = true };
            }
            else
            {
                resultResponseObject = new ResultResponseObject<BoundsSummaryVM>() { Value = null, Success = true };
            }

            return resultResponseObject;
        }


        public ResultResponseObject<BoundsSummaryVM> GetBoundsSummaryByLoggedUser()
        {
            List<BoundsDetailVM> boundsDetailVM = new List<BoundsDetailVM>();

            ResultServiceObject<IEnumerable<ProductUserView>> resultService = null;

            using (_uow.Create())
            {
                List<ProductCategoryEnum> productCategoryEnums = new List<ProductCategoryEnum>();
                productCategoryEnums.Add(ProductCategoryEnum.Funds);

                resultService = _financialProductService.GetAllProductsByUserAndListOfTypes(_globalAuthenticationService.IdUser, productCategoryEnums);


                foreach (var item in resultService.Value)
                {
                    var finalcialProduct = boundsDetailVM.FirstOrDefault(value => value.ProductName.Equals(item.ProductName));

                    if (finalcialProduct == null)
                    {
                        boundsDetailVM.Add(new BoundsDetailVM() { FinancialInstitution = item.FinancialInstitution, ProductName = item.ProductName, InternalCurrentValue = item.CurrentValue });
                    }
                    else
                    {
                        finalcialProduct.InternalCurrentValue = finalcialProduct.InternalCurrentValue + item.CurrentValue;
                    }
                }
            }

            decimal total = 0;

            foreach (var item in boundsDetailVM)
            {
                total = total + item.InternalCurrentValue;
                item.CurrentValue = item.InternalCurrentValue.ToString("n2", new CultureInfo("pt-br"));
            }

            ResultResponseObject<BoundsSummaryVM> resultResponseObject = null;

            if (total != 0)
            {
                resultResponseObject = new ResultResponseObject<BoundsSummaryVM>() { Value = new BoundsSummaryVM() {Bounds = boundsDetailVM, Total = total.ToString("n2", new CultureInfo("pt-br")) }, Success = true };
            }
            else
            {
                resultResponseObject = new ResultResponseObject<BoundsSummaryVM>() { Value = null, Success = true };
            }

            return resultResponseObject;
        }
    }
}