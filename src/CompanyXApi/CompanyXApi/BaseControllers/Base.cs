using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CompanyX.Api.BaseControllers
{
    /// <summary>
    /// Base api controller
    /// </summary>
    [Controller]
    [Produces("application/json")]
    public abstract class Base : Controller
    {
        /// <summary>
        /// Application configuration
        /// </summary>
        protected IConfiguration Configuration { get; }

        /// <summary>
        /// Logger
        /// </summary>
        protected ILogger Logger;

        /// <summary>
        /// Base controller constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="configuration"></param>
        protected Base(ILogger<Base> logger, IConfiguration configuration)
        {
            Configuration = configuration;
            Logger = logger;
        }

    }
}
