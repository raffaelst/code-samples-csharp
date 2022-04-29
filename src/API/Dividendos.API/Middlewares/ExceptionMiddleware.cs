using K.Logger;
using Dividendos.API.Model.Response.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Dividendos.Service.Interface;

namespace Dividendos.API.Middlewares
{

    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, ILogger logger, IGlobalAuthenticationService globalAuthenticationService)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine("");
                Debug.WriteLine("-----------------------------");
                Debug.WriteLine("ERROR_INVOKE_DEBUG: ");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("-----------------------------");
                Debug.WriteLine("ERROR_SOURCE: ");
                Debug.WriteLine(ex.Source);
                Debug.WriteLine("-----------------------------");
                Debug.WriteLine("ERROR_STACKTRACE: ");
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine("");
#endif

                if (!string.IsNullOrEmpty(globalAuthenticationService.IdUser))
                {
                    ex.Source = string.Concat(ex.Source, " userid: ", globalAuthenticationService.IdUser);
                }

                logger.SendErrorAsync(ex);

                await  HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = "Internal Server Error"
            }.ToString());
        }
    }
}
