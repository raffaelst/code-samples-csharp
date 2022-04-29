using Arquitetura.Api.Model.Common.Interface;

namespace Arquitetura.Api.Model.Common
{
    public class DomainModel : IDomain
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }
}