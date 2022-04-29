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
using Dividendos.Entity.Enum;

namespace Dividendos.Service
{
    public class SegmentService : BaseService, ISegmentService
    {
        public SegmentService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public ResultServiceObject<IEnumerable<Segment>> GetAll()
        {
            ResultServiceObject<IEnumerable<Segment>> resultService = new ResultServiceObject<IEnumerable<Segment>>();

            IEnumerable<Segment> segments = _uow.SegmentRepository.GetAll();

            resultService.Value = segments;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<SegmentView>> GetSumIdPortfolioPerformance(long idPortfolioPerformance)
        {
            ResultServiceObject<IEnumerable<SegmentView>> resultService = new ResultServiceObject<IEnumerable<SegmentView>>();

            IEnumerable<SegmentView> segments = _uow.SegmentViewRepository.GetSumIdPortfolioPerformance(idPortfolioPerformance);

            resultService.Value = segments;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<SegmentView>> GetSumSubPortfolioPerformance(long idPortfolioPerformance, long idSubportfolio)
        {
            ResultServiceObject<IEnumerable<SegmentView>> resultService = new ResultServiceObject<IEnumerable<SegmentView>>();

            IEnumerable<SegmentView> segments = _uow.SegmentViewRepository.GetSumSubPortfolioPerformance(idPortfolioPerformance, idSubportfolio);

            resultService.Value = segments;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<SegmentView>> GetSumIdPortfolioPerformance(long idPortfolioPerformance, StockTypeEnum stockTypeEnum)
        {
            ResultServiceObject<IEnumerable<SegmentView>> resultService = new ResultServiceObject<IEnumerable<SegmentView>>();

            IEnumerable<SegmentView> segments = _uow.SegmentViewRepository.GetSumIdPortfolioPerformance(idPortfolioPerformance, stockTypeEnum);

            resultService.Value = segments;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<SegmentView>> GetSumSubPortfolioPerformance(long idPortfolioPerformance, long idSubportfolio, StockTypeEnum stockTypeEnum)
        {
            ResultServiceObject<IEnumerable<SegmentView>> resultService = new ResultServiceObject<IEnumerable<SegmentView>>();

            IEnumerable<SegmentView> segments = _uow.SegmentViewRepository.GetSumSubPortfolioPerformance(idPortfolioPerformance, idSubportfolio, stockTypeEnum);

            resultService.Value = segments;

            return resultService;
        }

        public ResultServiceObject<Segment> Update(Segment segment)
        {
            ResultServiceObject<Segment> resultService = new ResultServiceObject<Segment>();
            resultService.Value = _uow.SegmentRepository.Update(segment);

            return resultService;
        }

        public ResultServiceObject<Segment> Insert(Segment segment)
        {
            ResultServiceObject<Segment> resultService = new ResultServiceObject<Segment>();
            segment.GuidSegment = Guid.NewGuid();
            segment.IdSegment = _uow.SegmentRepository.Insert(segment);
            resultService.Value = segment;

            return resultService;
        }
    }
}
