using System.Security.Claims;
using System.Threading.Tasks;

namespace Dividendos.Service.Interface
{
    /// <summary>
    /// IGlobalAuthentication
    /// </summary>
    public interface IGlobalAuthenticationService
    {
        /// <summary>
        /// User
        /// </summary>
        ClaimsPrincipal User { get; }

        /// <summary>
        /// IdUser
        /// </summary>
        string IdUser { get; }

        string Email { get; }

        /// <summary>
        /// IsAdministrator
        /// </summary>
        /// <returns></returns>
        bool IsAdministrator();

        /// <summary>
        /// GetDefaultUserRole
        /// </summary>
        /// <returns></returns>
        string GetDefaultUserRole();

        string GetCurrentAccessToken();
    }
}
