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
using System.Text;
using System.Dynamic;

namespace Dividendos.Repository.Repository
{
    public class CryptoPortfolioRepository : Repository<CryptoPortfolio>, ICryptoPortfolioRepository
    {
        private IUnitOfWork _unitOfWork;

        public CryptoPortfolioRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void UpdateCalculatePerformanceDate(long idCryptoPortfolio, DateTime calculatePerformanceDate)
        {
            string sql = $"UPDATE CryptoPortfolio SET CalculatePerformanceDate = @CalculatePerformanceDate WHERE IdCryptoPortfolio = @IdCryptoPortfolio";

            _unitOfWork.Connection.Execute(sql, new { IdCryptoPortfolio = idCryptoPortfolio, CalculatePerformanceDate = calculatePerformanceDate }, _unitOfWork.Transaction);
        }

        public IEnumerable<CryptoPortfolio> GetByUserAndStatus(string idUser, bool status, bool? manual)
        {
            StringBuilder sql = new StringBuilder(@"SELECT CryptoPortfolio.IdCryptoPortfolio ,CryptoPortfolio.IdTrader,CryptoPortfolio.Name,CryptoPortfolio.CreatedDate,CryptoPortfolio.GuidCryptoPortfolio,CryptoPortfolio.Manual,CryptoPortfolio.CalculatePerformanceDate,CryptoPortfolio.Active,CryptoPortfolio.IdFiatCurrency FROM CryptoPortfolio   
                            inner join trader on trader.idtrader = CryptoPortfolio.idtrader
                            where trader.iduser = @Iduser AND CryptoPortfolio.Active = @Active AND Trader.ShowOnPatrimony = 1");

            dynamic queryParams = new ExpandoObject();
            queryParams.Iduser = idUser;
            queryParams.Active = status;

            if (manual.HasValue)
            {
                queryParams.Manual = manual.Value;
                sql.Append("and CryptoPortfolio.Manual = @Manual ");
            }

            var portfolios = _unitOfWork.Connection.Query<CryptoPortfolio>(sql.ToString(), (object)queryParams, _unitOfWork.Transaction);

            return portfolios;
        }

        public void UpdateName(long idCryptoPortfolio, string name)
        {
            string sql = $"UPDATE CryptoPortfolio SET Name = @Name WHERE IdCryptoPortfolio = @IdCryptoPortfolio";

            _unitOfWork.Connection.Execute(sql, new { IdCryptoPortfolio = idCryptoPortfolio, Name = name }, _unitOfWork.Transaction);
        }
    }
}
