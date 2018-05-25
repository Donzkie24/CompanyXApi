using CompanyX.Base.Helpers;
using Microsoft.Extensions.Logging;

namespace CompanyX.Services
{
    public abstract class BaseService
    {
        protected ILogger Logger;

        protected BaseService(ILogger<BaseService> logger)
        {
            Guard.IsNotNull(logger, () => logger);

            Logger = logger;
        }
    }
}
