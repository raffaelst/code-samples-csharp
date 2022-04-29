using Dividendos.Entity.Entities;
using Dividendos.Entity.Enum;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Service.Interface
{
    public interface ISegmentService : IBaseService
    {
        ResultServiceObject<IEnumerable<SegmentView>> GetSumIdPortfolioPerformance(long idPortfolioPerformance);
        ResultServiceObject<IEnumerable<SegmentView>> GetSumSubPortfolioPerformance(long idPortfolioPerformance, long idSubportfolio);
        ResultServiceObject<Segment> Update(Segment segment);
        ResultServiceObject<Segment> Insert(Segment segment);
        ResultServiceObject<IEnumerable<Segment>> GetAll();
        ResultServiceObject<IEnumerable<SegmentView>> GetSumSubPortfolioPerformance(long idPortfolioPerformance, long idSubportfolio, StockTypeEnum stockTypeEnum);
        ResultServiceObject<IEnumerable<SegmentView>> GetSumIdPortfolioPerformance(long idPortfolioPerformance, StockTypeEnum stockTypeEnum);
    }
}
