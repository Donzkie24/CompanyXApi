using System;
using System.IO;
using System.Threading.Tasks;
using CompanyX.Api.Infrastructure.Helpers;
using CompanyX.Base.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CompanyX.Api.Infrastructure.Middleware
{
    /// <summary>
    /// Log Response
    /// Credits: https://github.com/sulhome/log-request-response-middleware
    /// </summary>
    public class LogResponseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LogResponseMiddleware> _logger;
        private readonly Func<string, Exception, string> _defaultFormatter = (state, exception) => state;

        /// <summary>
        /// LogResponseMiddleware constructor
        /// </summary>
        /// <param name="next"></param>
        /// <param name="logger"></param>
        public LogResponseMiddleware(RequestDelegate next, ILogger<LogResponseMiddleware> logger)
        {
            Guard.IsNotNull(logger, () => logger);

            _logger = logger;
            _next = next;
        }

        /// <summary>
        /// Log response object and pass http context to next pipeline middle ware
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            var bodyStream = context.Response.Body;

            if (!bodyStream.CanRead || !bodyStream.CanWrite)
            {
                await _next(context);
                return;
            }

            var responseBodyStream = new MemoryStream();
            context.Response.Body = responseBodyStream;

            await _next(context);

            responseBodyStream.Seek(0, SeekOrigin.Begin);
            var responseBody = new StreamReader(responseBodyStream).ReadToEnd();
            _logger.Log(LogLevel.Information, Const.LogResponseEventId, $"RESPONSE LOG: {responseBody}", null, _defaultFormatter);
            responseBodyStream.Seek(0, SeekOrigin.Begin);
            await responseBodyStream.CopyToAsync(bodyStream);
        }
    }

}
