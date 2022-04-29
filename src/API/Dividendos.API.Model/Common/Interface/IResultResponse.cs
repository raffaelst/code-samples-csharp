using System.Collections.Generic;

namespace Dividendos.API.Model.Common.Interface
{
    /// <summary>
    /// IResultResponse
    /// </summary>
    public interface IResultResponse
    {
        /// <summary>
        /// Success
        /// </summary>
        bool Success { get; set; }
        /// <summary>
        /// ErrorMessages
        /// </summary>
        ICollection<string> ErrorMessages { get; set; }
    }
}
