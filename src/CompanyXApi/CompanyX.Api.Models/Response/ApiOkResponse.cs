using System.Net;
using CompanyX.Resource;

namespace CompanyX.Api.Models.Response
{
    public class ApiOkResponse<T> : ApiResponse
    {
        public T Data { get; }

        public ApiOkResponse(T data)
            : base(HttpStatusCode.OK, Global.SuccessMessage)
        {
            Data = data;
        }
    }
}
