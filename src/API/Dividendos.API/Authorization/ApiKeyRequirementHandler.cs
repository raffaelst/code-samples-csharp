using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace K.ApiKeyPolicy
{
    /// <summary>
    /// ApiKeyRequirementHandler
    /// </summary>
    public class ApiKeyRequirementHandler : AuthorizationHandler<ApiKeyRequirement>
    {
        const string API_KEY_HEADER_NAME = "X-API-KEY";

        //private readonly IIntegrationKeyApp _integrationKeyApp;

        /// <summary>
        /// ApiKeyRequirementHandler
        /// </summary>
        /// <param name="integrationKeyApp"></param>
        //public ApiKeyRequirementHandler(IIntegrationKeyApp integrationKeyApp)
        public ApiKeyRequirementHandler()
        {

        }

        /// <summary>
        /// HandleRequirementAsync
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ApiKeyRequirement requirement)
        {
            SucceedRequirementIfApiKeyPresentAndValid(context, requirement);
            return Task.CompletedTask;
        }

        private void SucceedRequirementIfApiKeyPresentAndValid(AuthorizationHandlerContext context, ApiKeyRequirement requirement)
        {
            if (context.Resource is AuthorizationFilterContext authorizationFilterContext)
            {
                string apiKey = authorizationFilterContext.HttpContext.Request.Headers[API_KEY_HEADER_NAME].FirstOrDefault();

                //Guid companyGuid = _integrationKeyApp.ValidateKey(apiKey);

                if (apiKey != null && apiKey.Equals("123456"))
                {
                    ClaimsIdentity identity = new ClaimsIdentity(
                    new GenericIdentity("1321", "Login"),
                    new[] {
                        new Claim("Culture", "pt-BR") // To do: Colocar cultura no usuário
                    });

                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(identity);

                    ClaimsPrincipal principal = new ClaimsPrincipal(claimsIdentity);

                    authorizationFilterContext.HttpContext.User = principal;

                    context.Succeed(requirement);
                }
            }
        }
    }
}
