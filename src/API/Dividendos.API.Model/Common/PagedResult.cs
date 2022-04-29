using Dividendos.API.Model.Response.Common;
using System.Collections.Generic;

namespace Dividendos.API.Model.Common
{
    public class PagedResult<T> : ResultResponseBase
    {
        /// <summary>
        /// The page number this page represents.
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// The size of this page.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// The total number of pages available.
        /// </summary>
        public long TotalNumberOfPages { get; set; }

        /// <summary>
        /// The total number of records available.
        /// </summary>
        public long TotalNumberOfRecords { get; set; }

        /// <summary>
        /// The records this page represents.
        /// </summary>
        public IList<T> Results { get; set; }
    }
}
