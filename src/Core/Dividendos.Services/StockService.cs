using FluentValidation.Results;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Repository.Interface;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using Dividendos.Service.Validator.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Dividendos.Entity.Views;
using System;
using Dividendos.Entity.Enum;

namespace Dividendos.Service
{
    public class StockService : BaseService, IStockService
    {
        public StockService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public ResultServiceObject<IEnumerable<Stock>> GetAll()
        {
            ResultServiceObject<IEnumerable<Stock>> resultService = new ResultServiceObject<IEnumerable<Stock>>();

            IEnumerable<Stock> stocks = _uow.StockRepository.Select(p => p.ShowOnPortolio == true);

            resultService.Value = stocks;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<Stock>> GetAllByCountry(int idCountry)
        {
            ResultServiceObject<IEnumerable<Stock>> resultService = new ResultServiceObject<IEnumerable<Stock>>();

            IEnumerable<Stock> stocks = _uow.StockRepository.GetAllByCountry(idCountry);

            resultService.Value = stocks;

            return resultService;
        }

        public ResultServiceObject<Stock> GetBySymbolOrLikeOldSymbol(string symbol, int idCountry)
        {
            ResultServiceObject<Stock> resultService = new ResultServiceObject<Stock>();

            IEnumerable<Stock> stocks = _uow.StockRepository.GetBySymbolOrLikeOldSymbol(symbol, idCountry);

            resultService.Value = stocks.FirstOrDefault();

            return resultService;
        }

        public ResultServiceObject<Stock> GetById(long idStock)
        {
            ResultServiceObject<Stock> resultService = new ResultServiceObject<Stock>();

            IEnumerable<Stock> stocks = _uow.StockRepository.Select(p => p.IdStock == idStock);

            resultService.Value = stocks.FirstOrDefault();

            return resultService;
        }

        public ResultServiceObject<Stock> GetByGuid(Guid stockGui)
        {
            ResultServiceObject<Stock> resultService = new ResultServiceObject<Stock>();

            IEnumerable<Stock> stocks = _uow.StockRepository.Select(p => p.GuidStock == stockGui);

            resultService.Value = stocks.FirstOrDefault();

            return resultService;

        }
        public ResultServiceObject<IEnumerable<Stock>> GetByUser(string idUser)
        {
            ResultServiceObject<IEnumerable<Stock>> resultService = new ResultServiceObject<IEnumerable<Stock>>();

            IEnumerable<Stock> stocks = _uow.StockRepository.GetByUser(idUser);

            resultService.Value = stocks;


            return resultService;
        }

        public ResultServiceObject<Stock> Update(Stock stock)
        {
            ResultServiceObject<Stock> resultService = new ResultServiceObject<Stock>();

            resultService.Value = _uow.StockRepository.Update(stock);


            return resultService;
        }

        public ResultServiceObject<Stock> UpdateLastDividendUpdateSync(Stock stock)
        {
            ResultServiceObject<Stock> resultService = new ResultServiceObject<Stock>();

            resultService.Value = _uow.StockRepository.UpdateLastDividendUpdateSync(stock, DateTime.Now);


            return resultService;
        }

        public ResultServiceObject<Stock> UpdateLastDailyVariationNotification(long idStock)
        {
            ResultServiceObject<Stock> resultService = new ResultServiceObject<Stock>();

            _uow.StockRepository.UpdateLastDailyVariationNotification(idStock, DateTime.Now);


            return resultService;
        }

        public ResultServiceObject<IEnumerable<StockView>> GetLikeSymbol(string symbol, int idCountry)
        {
            ResultServiceObject<IEnumerable<StockView>> resultService = new ResultServiceObject<IEnumerable<StockView>>();

            IEnumerable<StockView> stocks = _uow.StockViewRepository.GetLikeSymbol(symbol, idCountry);

            resultService.Value = stocks;


            return resultService;
        }

        public ResultServiceObject<IEnumerable<StockView>> GetLikeCompanyName(string symbol, int idCountry)
        {
            ResultServiceObject<IEnumerable<StockView>> resultService = new ResultServiceObject<IEnumerable<StockView>>();

            IEnumerable<StockView> stocks = _uow.StockViewRepository.GetLikeCompanyName(symbol, idCountry);

            resultService.Value = stocks;


            return resultService;
        }

        public ResultServiceObject<IEnumerable<StockView>> GetByNameOrSymbol(string name)
        {
            ResultServiceObject<IEnumerable<StockView>> resultService = new ResultServiceObject<IEnumerable<StockView>>();

            IEnumerable<StockView> stocks = _uow.StockViewRepository.GetByNameOrSymbol(name);

            resultService.Value = stocks;


            return resultService;
        }

        public ResultServiceObject<StockStatementView> GetByIdStock(long idStock)
        {
            ResultServiceObject<StockStatementView> resultService = new ResultServiceObject<StockStatementView>();

            StockStatementView stockStatementView = _uow.StockStatementViewRepository.GetByIdStock(idStock);

            resultService.Value = stockStatementView;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<Stock>> GetByCompanyID(long companyID)
        {
            ResultServiceObject<IEnumerable<Stock>> resultService = new ResultServiceObject<IEnumerable<Stock>>();

            IEnumerable<Stock> stocks = _uow.StockRepository.GetByCompanyID(companyID);

            resultService.Value = stocks;

            return resultService;
        }


        public ResultServiceObject<IEnumerable<Stock>> GetAllShowOnPortfolio(int idCountry)
        {
            ResultServiceObject<IEnumerable<Stock>> resultService = new ResultServiceObject<IEnumerable<Stock>>();

            IEnumerable<Stock> stocks = _uow.StockRepository.GetAllShowOnPortfolio(idCountry);

            resultService.Value = stocks;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<string>> GetAllUsersWithStock(long idStock)
        {
            ResultServiceObject<IEnumerable<string>> resultService = new ResultServiceObject<IEnumerable<string>>();

            IEnumerable<string> idUsers = _uow.StockRepository.GetAllUsersWithStock(idStock);

            resultService.Value = idUsers;

            return resultService;
        }

        public ResultServiceObject<Stock> Insert(Stock stock)
        {
            ResultServiceObject<Stock> resultService = new ResultServiceObject<Stock>();
            stock.GuidStock = Guid.NewGuid();
            stock.IdStock = _uow.StockRepository.Insert(stock);
            resultService.Value = stock;

            return resultService;
        }

        public ResultServiceObject<Stock> GetByLastDividendUpdateSyncOrderingAsc(int idCountry)
        {
            ResultServiceObject<Stock> resultService = new ResultServiceObject<Stock>();

            Stock stocks = _uow.StockRepository.GetByLastDividendUpdateSyncOrderingAsc(idCountry);

            resultService.Value = stocks;

            return resultService;
        }

        public ResultServiceObject<Stock> GetByLastDividendUpdateSyncOrderingAscAndStockType(int idCountry, StockTypeEnum stockType)
        {
            ResultServiceObject<Stock> resultService = new ResultServiceObject<Stock>();

            Stock stocks = _uow.StockRepository.GetByLastDividendUpdateSyncOrderingAsc(idCountry, stockType);

            resultService.Value = stocks;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<Stock>> GetAllByStockType(int idCountry, StockTypeEnum stockType)
        {
            ResultServiceObject<IEnumerable<Stock>> resultService = new ResultServiceObject<IEnumerable<Stock>>();

            IEnumerable<Stock> stocks = _uow.StockRepository.GetAllByStockType(idCountry, stockType);

            resultService.Value = stocks;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<Stock>> GetAllByCountryOrderByLastDividendUpdateSync(int idCountry)
        {
            ResultServiceObject<IEnumerable<Stock>> resultService = new ResultServiceObject<IEnumerable<Stock>>();

            IEnumerable<Stock> stocks = _uow.StockRepository.GetAllByCountryOrderByLastDividendUpdateSync(idCountry);

            resultService.Value = stocks;

            return resultService;
        }
    }
}
