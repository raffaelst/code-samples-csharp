using System;
using System.Collections.Generic;
using System.Text;

namespace Arquitetura.Api.Model.Common.Interface
{
    public interface IPagination
    {
        int PageIndex { get; set; }
        int PageSize { get; set; }
    }
}
