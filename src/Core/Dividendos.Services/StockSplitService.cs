using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dividendos.Service
{
    public class StockSplitService : BaseService, IStockSplitService
    {
        public StockSplitService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public ResultServiceObject<IEnumerable<StockSplit>> Get(bool onlyMyStocks, string userID, DateTime starDate, DateTime endDate)
        {
            ResultServiceObject<IEnumerable<StockSplit>> resultService = new ResultServiceObject<IEnumerable<StockSplit>>();

            IEnumerable<StockSplit> relevantFacts = _uow.StockSplitRepository.Get(onlyMyStocks, userID, starDate, endDate);

            resultService.Value = relevantFacts;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<StockSplit>> GetByGuidAndDate(Guid stockGuid, DateTime starDate, DateTime endDate)
        {
            ResultServiceObject<IEnumerable<StockSplit>> resultService = new ResultServiceObject<IEnumerable<StockSplit>>();

            IEnumerable<StockSplit> relevantFacts = _uow.StockSplitRepository.GetByGuidAndDate(stockGuid, starDate, endDate);

            resultService.Value = relevantFacts;

            return resultService;
        }

        public ResultServiceObject<StockSplit> Add(StockSplit stockSplit)
        {
            ResultServiceObject<StockSplit> resultService = new ResultServiceObject<StockSplit>();
            stockSplit.StockSplitID = _uow.StockSplitRepository.Insert(stockSplit);

            resultService.Value = stockSplit;

            return resultService;
        }


        public ResultServiceObject<StockSplit> GetBy(long idStock, DateTime splitDate, int idCountry)
        {
            ResultServiceObject<StockSplit> resultService = new ResultServiceObject<StockSplit>();

            IEnumerable<StockSplit> stSplits = _uow.StockSplitRepository.Select(scp => scp.StockID == idStock && scp.DateSplit == splitDate && scp.IdCountry == idCountry);

            resultService.Value = stSplits.FirstOrDefault();

            return resultService;
        }

        public ResultServiceObject<IEnumerable<StockSplit>> GetAllByDate(long idStock, DateTime splitDate, int idCountry)
        {
            ResultServiceObject<IEnumerable<StockSplit>> resultService = new ResultServiceObject<IEnumerable<StockSplit>>();

            IEnumerable<StockSplit> stSplits = _uow.StockSplitRepository.Select(scp => scp.StockID == idStock && scp.DateSplit <= splitDate && scp.IdCountry == idCountry);

            resultService.Value = stSplits;

            return resultService;
        }

        public List<OperationItem> ApplyStockSplit(ref List<OperationItem> operationItemStock, int idCountry)
        {
            List<OperationItem> operationItemsSplit = new List<OperationItem>();
            List<long> stockIds = operationItemStock.GroupBy(objStockOperationTmp => objStockOperationTmp.IdStock).Select(objStockOperationGp => objStockOperationGp.First().IdStock).ToList();

            if (stockIds != null && stockIds.Count() > 0)
            {
                foreach (long stockId in stockIds)
                {
                    ResultServiceObject<IEnumerable<StockSplit>> resultStockSplit = GetAllByDate(stockId, DateTime.Now, idCountry);

                    if (resultStockSplit.Value != null && resultStockSplit.Value.Count() > 0)
                    {
                        List<OperationItem> operationItemFiltered = operationItemStock.FindAll(opItem => opItem.IdStock == stockId);

                        if (operationItemFiltered != null && operationItemFiltered.Count() > 0)
                        {
                            foreach (OperationItem opItem in operationItemFiltered)
                            {
                                //if (!opItem.SplitApplied)
                                //{
                                List<StockSplit> stockSplits = resultStockSplit.Value.Where(split => opItem.LastSplitDate < split.DateSplit).ToList();

                                if (stockSplits != null && stockSplits.Count() > 0)
                                {
                                    stockSplits = stockSplits.OrderBy(opItemTemp => opItemTemp.DateSplit).ToList();

                                    foreach (StockSplit stockSplit in stockSplits)
                                    {
                                        if (stockSplit.ProportionFrom > stockSplit.ProportionTo)
                                        {
                                            opItem.NumberOfShares = opItem.NumberOfShares * stockSplit.ProportionFrom;
                                            opItem.AveragePrice = opItem.AveragePrice / stockSplit.ProportionFrom;
                                            opItem.AcquisitionPrice = opItem.AcquisitionPrice / stockSplit.ProportionFrom;
                                        }
                                        else
                                        {
                                            opItem.NumberOfShares = opItem.NumberOfShares / stockSplit.ProportionTo;
                                            opItem.AveragePrice = opItem.AveragePrice * stockSplit.ProportionTo;
                                            opItem.AcquisitionPrice = opItem.AcquisitionPrice * stockSplit.ProportionTo;
                                        }

                                        if (stockSplit.IdCountry == 1)
                                        {
                                            opItem.NumberOfShares = Math.Floor(opItem.NumberOfShares);
                                        }

                                        if (opItem.NumberOfShares == 0)
                                        {
                                            opItem.NumberOfShares = 0.1M;
                                        }

                                        if (opItem.AveragePrice == 0)
                                        {
                                            opItem.AveragePrice = 0.1M;
                                        }

                                        //opItem.SplitApplied = true;
                                        opItem.LastSplitDate = stockSplit.DateSplit.AddDays(1);

                                        operationItemsSplit.Add(opItem);
                                    }
                                }
                                //}
                            }
                        }
                    }
                }
            }

            return operationItemsSplit;
        }

        public ResultServiceObject<StockSplit> GetByIdStock(long idStock, DateTime eventDate)
        {
            ResultServiceObject<StockSplit> resultService = new ResultServiceObject<StockSplit>();

            IEnumerable<StockSplit> stSplits = _uow.StockSplitRepository.GetByIdStock(idStock, eventDate);

            resultService.Value = stSplits.FirstOrDefault();

            return resultService;
        }

        public ResultServiceObject<StockSplit> GetLatestByIdStock(long idStock)
        {
            ResultServiceObject<StockSplit> resultService = new ResultServiceObject<StockSplit>();

            IEnumerable<StockSplit> stSplits = _uow.StockSplitRepository.GetLatestByIdStock(idStock);

            resultService.Value = stSplits.FirstOrDefault();

            return resultService;
        }

        public bool HasStockSplit(string idUser, DateTime limitDate)
        {
            return _uow.StockSplitRepository.HasStockSplit(idUser, limitDate);
        }
    }
}
