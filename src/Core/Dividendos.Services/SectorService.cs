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
using System;
using Dividendos.Entity.Views;

namespace Dividendos.Service
{
    public class SectorService : BaseService, ISectorService
    {
        public SectorService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public ResultServiceObject<IEnumerable<Sector>> GetAll()
        {
            ResultServiceObject<IEnumerable<Sector>> resultService = new ResultServiceObject<IEnumerable<Sector>>();

            IEnumerable<Sector> sectors = _uow.SectorRepository.GetAll();

            resultService.Value = sectors;

            return resultService;
        }

        public ResultServiceObject<Sector> GetByIdStock(long idStock)
        {
            ResultServiceObject<Sector> resultService = new ResultServiceObject<Sector>();

            IEnumerable<Sector> sectors = _uow.SectorRepository.GetByIdStock(idStock);

            resultService.Value = sectors.FirstOrDefault();

            return resultService;
        }

        public ResultServiceObject<IEnumerable<SectorView>> GetSumIdPortfolioPerformance(long idPortfolioPerformance)
        {
            ResultServiceObject<IEnumerable<SectorView>> resultService = new ResultServiceObject<IEnumerable<SectorView>>();

            IEnumerable<SectorView> sectors = _uow.SectorViewRepository.GetSumIdPortfolioPerformance(idPortfolioPerformance);

            resultService.Value = sectors;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<SectorView>> GetSumSubPortfolioPerformance(long idPortfolioPerformance, long idSubportfolio)
        {
            ResultServiceObject<IEnumerable<SectorView>> resultService = new ResultServiceObject<IEnumerable<SectorView>>();

            IEnumerable<SectorView> sectors = _uow.SectorViewRepository.GetSumSubPortfolioPerformance(idPortfolioPerformance, idSubportfolio);

            resultService.Value = sectors;

            return resultService;
        }

        public ResultServiceObject<Sector> Update(Sector sector)
        {
            ResultServiceObject<Sector> resultService = new ResultServiceObject<Sector>();
            resultService.Value = _uow.SectorRepository.Update(sector);

            return resultService;
        }

        public ResultServiceObject<Sector> Insert(Sector sector)
        {
            ResultServiceObject<Sector> resultService = new ResultServiceObject<Sector>();
            sector.GuidSector = Guid.NewGuid();
            sector.IdSector = _uow.SectorRepository.Insert(sector);
            resultService.Value = sector;

            return resultService;
        }
    }
}
