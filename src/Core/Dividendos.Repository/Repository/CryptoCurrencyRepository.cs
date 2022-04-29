using Dapper;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Views;
using Dividendos.Repository.GenericRepository;
using Dividendos.Repository.Interface;
using Dividendos.Repository.Interface.UoW;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Repository
{
    public class CryptoCurrencyRepository : Repository<CryptoCurrency>, ICryptoCurrencyRepository
    {
        private IUnitOfWork _unitOfWork;

        public CryptoCurrencyRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public IEnumerable<CryptoStatementView> GetCryptosWithLogoByTrader(Guid? traderGuid, string idUser)
        {
            StringBuilder sql = new StringBuilder(@"SELECT ProductUser.ProductID AS IdProductUser, 
                            Product.Description AS Company, 
                            ProductCategory.Description AS Segment, 
                            Product.ExternalName AS Symbol,
                            ProductUser.AveragePrice AS AveragePrice,
                            ProductUser.CurrentValue AS NumberOfShares,
                            ProductUser.ProductUserGuid AS CryptoCurrencyGuid,
                            CryptoCurrency.MarketPrice AS MarketPrice,
                            Product.Description AS Name,
                            CryptoCurrency.Variation AS PerformancePerc,
                            Logo.LogoImage AS Logo,
                            Logo.URL AS LogoUrl,
							FinancialInstitution.Name AS FinancialInstitutionName
                            FROM ProductUser
                            INNER JOIN Product on Product.ProductID = ProductUser.ProductID
                            INNER JOIN ProductCategory on ProductCategory.ProductCategoryID = Product.ProductCategoryID
                            INNER JOIN CryptoCurrency ON CryptoCurrency.Name = Product.ExternalName
							INNER JOIN FinancialInstitution ON FinancialInstitution.FinancialInstitutionID = ProductUser.FinancialInstitutionID
                            LEFT JOIN Logo ON Logo.IdLogo = CryptoCurrency.LogoID
                            INNER JOIN Trader ON Trader.IdTrader = ProductUser.TraderID
                            WHERE ProductUser.Active = 1 AND Trader.Active = 1 AND ProductUser.CurrentValue > 0
                            ");
            
            if (traderGuid == null)
            {
                sql.Append("AND Trader.IdUser = @IdUser ");
            }
            else
            {
                sql.Append("AND Trader.GuidTrader = @GuidTrader ");
            }

            IEnumerable<CryptoStatementView> cryptoStatementViews = _unitOfWork.Connection.Query<CryptoStatementView>(sql.ToString(), new { GuidTrader = traderGuid, IdUser = idUser }, _unitOfWork.Transaction);

            return cryptoStatementViews;
        }


        public IEnumerable<CryptoStatementView> GetCryptosByNameOrSymbol(string nameOrSymbol)
        {
            StringBuilder sql = new StringBuilder(@"SELECT CryptoCurrency.*, Product.Description AS Description, Product.ExternalName AS Symbol, Logo.URL AS LogoUrl FROM CryptoCurrency
                                                    INNER JOIN Product ON Product.ExternalName = CryptoCurrency.Name
                                                    LEFT JOIN Logo ON Logo.IdLogo = CryptoCurrency.LogoID
                                                    WHERE Product.Description LIKE '%' + @NameOrSymbol + '%' OR Product.ExternalName LIKE '%' + @NameOrSymbol + '%' ");

            IEnumerable<CryptoStatementView> cryptoStatementViews = _unitOfWork.Connection.Query<CryptoStatementView>(sql.ToString(), new { NameOrSymbol = nameOrSymbol }, _unitOfWork.Transaction);

            return cryptoStatementViews;
        }


        public IEnumerable<CryptoStatementView> GetMarketMoverByType(bool gainers)
        {
            StringBuilder sql = new StringBuilder(@"SELECT TOP 100 Product.ExternalName AS Symbol,
                            CryptoCurrency.MarketPrice AS MarketPrice,
                            Product.Description AS Name,
                            CryptoCurrency.PercentChange1h,
							CryptoCurrency.PercentChange24h,
							CryptoCurrency.PercentChange7d,
							CryptoCurrency.PercentChange30d,
							CryptoCurrency.PercentChange60d,
							CryptoCurrency.PercentChange90d,
                            Logo.LogoImage AS Logo,
                            Logo.URL AS LogoUrl
                            FROM CryptoCurrency
                            INNER JOIN Product on Product.ExternalName = CryptoCurrency.Name
                            INNER JOIN ProductCategory on ProductCategory.ProductCategoryID = Product.ProductCategoryID
                            LEFT JOIN Logo ON Logo.IdLogo = CryptoCurrency.LogoID WHERE CryptoCurrency.MarketPrice > 0 AND CryptoCurrency.PercentChange24h IS NOT NULL
                            ");

            if (gainers)
            {
                sql.Append(" ORDER BY PercentChange24h DESC");
            }
            else
            {
                sql.Append(" ORDER BY PercentChange24h ASC ");
            }

            IEnumerable<CryptoStatementView> cryptoStatementViews = _unitOfWork.Connection.Query<CryptoStatementView>(sql.ToString(), null, _unitOfWork.Transaction);

            return cryptoStatementViews;
        }

        public IEnumerable<CryptoStatementView> GetTopCryptos()
        {
            StringBuilder sql = new StringBuilder(@"SELECT CryptoCurrency.Name AS Symbol,
                            CryptoCurrency.MarketPriceUSD AS MarketPrice,
                            Product.Description AS Description,
                            CryptoCurrency.PercentChange1h,
							CryptoCurrency.PercentChange24h,
							CryptoCurrency.PercentChange7d,
							CryptoCurrency.PercentChange30d,
							CryptoCurrency.PercentChange60d,
							CryptoCurrency.PercentChange90d,
                            Logo.LogoImage AS Logo,
                            Logo.URL AS LogoUrl
                            FROM CryptoCurrency
                            INNER JOIN Product on Product.ExternalName = CryptoCurrency.Name
							LEFT JOIN Logo ON Logo.IdLogo = CryptoCurrency.LogoID 
                            where CryptoCurrency.CryptoCurrencyID in (3,6,36,39,159,203,204) 
                            ");

            IEnumerable<CryptoStatementView> cryptoStatementViews = _unitOfWork.Connection.Query<CryptoStatementView>(sql.ToString(), null, _unitOfWork.Transaction);

            return cryptoStatementViews;
        }
    }
}
