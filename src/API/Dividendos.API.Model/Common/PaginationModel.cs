using Arquitetura.Api.Model.Common.Interface;

namespace Arquitetura.Api.Model.Common
{
    public class PaginationModel : IPagination
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
