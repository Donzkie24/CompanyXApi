namespace CompanyX.Api.Infrastructure.Helpers
{
    /// <summary>
    /// Constants
    /// </summary>
    public static class Const
    {
        /// <summary>
        /// Un-handled exception id
        /// </summary>
        public const int UnhandledException = 3000;
        /// <summary>
        /// Bad-request exception id
        /// </summary>
        public const int BadRequestException = 4000;
        /// <summary>
        /// Not found exception
        /// </summary>
        public const int NotFoundException = 4001;
        /// <summary>
        /// Log request event id
        /// </summary>
        public const int LogRequestEventId = 4010;
        /// <summary>
        /// Log response event id
        /// </summary>
        public const int LogResponseEventId = 4011;

        /// <summary>
        /// Ok status text
        /// </summary>
        public const string StatusOk = "Ok";
    }
}
