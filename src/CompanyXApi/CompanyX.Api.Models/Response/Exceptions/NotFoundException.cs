using System.Net;

namespace CompanyX.Api.Models.Response.Exceptions
{
    public class NotFoundException : ApiException
    {
        public NotFoundException()
            : base(HttpStatusCode.NotFound)
        {
        }

        public NotFoundException(string message)
            : base(HttpStatusCode.NotFound, HttpStatusCode.NotFound.ToString(), message)
        {
        }
    }
}
