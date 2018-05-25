using System;
using System.Net;
using System.Threading.Tasks;
using CompanyX.Api.Infrastructure.Helpers;
using CompanyX.Api.Models.Response;
using CompanyX.Api.Models.Response.Exceptions;
using CompanyX.Base.Helpers;
using CompanyX.Resource;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CompanyX.Api.Infrastructure.Middleware
{
    /// <summary>
    /// Error handling middle-ware
    /// Credits: https://github.com/sulhome/log-request-response-middleware
    /// </summary>
    public class ErrorWrappingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorWrappingMiddleware> _logger;
        private readonly IHostingEnvironment _env;

        /// <summary>
        /// ErrorWrappingMiddleware constructor
        /// </summary>
        /// <param name="next"></param>
        /// <param name="logger"></param>
        /// <param name="env"></param>
        public ErrorWrappingMiddleware(RequestDelegate next, ILogger<ErrorWrappingMiddleware> logger, IHostingEnvironment env)
        {
            Guard.IsNotNull(logger, () => logger);
            Guard.IsNotNull(env, () => env);

            _next = next;
            _logger = logger;
            _env = env;
        }

        /// <summary>
        /// Log error and pass http context to next pipeline middle ware
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            Exception exception = null;
            string jsonResponse = string.Empty;
            try
            {
                await _next.Invoke(context).ConfigureAwait(false);
            }
            catch (BadRequestException badReqEx)
            {
                HandleException(Const.BadRequestException, badReqEx, badReqEx.StatusCode, badReqEx.Message);
            }
            catch (NotFoundException notFoundEx)
            {
                HandleException(Const.NotFoundException, notFoundEx, notFoundEx.StatusCode, notFoundEx.Message);
            }
            catch (UnauthorizedException authEx)
            {
                HandleException(Const.NotFoundException, authEx, authEx.StatusCode, authEx.Message);
            }
            catch (ApiException apiEx)
            {
                HandleException(Const.UnhandledException, apiEx, apiEx.StatusCode, apiEx.Message);
            }
            catch (Exception ex)
            {
                HandleException(Const.UnhandledException, ex, HttpStatusCode.InternalServerError, Global.InternalServerError);
            }

            void HandleException(EventId eventId, Exception ex, HttpStatusCode httpStatusCode, string message)
            {
                exception = ex;
                _logger.LogError(eventId, ex, ex.Message);
                context.Response.StatusCode = (int)httpStatusCode;
                context.Response.ContentType = "application/json";
                ApiResponse response;
                //Log stack trace
                if (!_env.IsProduction())
                {
                    response = new JsonErrorResponse(httpStatusCode, exception.Message, ex);
                }
                else
                {
                    response = new JsonErrorResponse(httpStatusCode, exception.Message);
                }

                jsonResponse = JsonConvert.SerializeObject(response, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
            });
            }

            if (!context.Response.HasStarted && exception != null)
            {
                await context.Response.WriteAsync(jsonResponse);
            }
        }

        private class JsonErrorResponse : ApiResponse
        {
            public JsonErrorResponse(HttpStatusCode statusCode, string message = null, object developerMessage = null)
                : base(statusCode, message)
            {
                DeveloperMessage = developerMessage;
            }
            public object DeveloperMessage { get; set; }
        }
    }

}
