using System.Net;
using System.Threading.Tasks;
using CompanyX.Api.Infrastructure.Filters;
using CompanyX.Api.Models.Orders;
using CompanyX.Api.Models.Response;
using CompanyX.Base.Helpers;
using CompanyX.Resource;
using CompanyX.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CompanyX.Api.V1.Controllers
{
    /// <summary>
    /// Orders controller
    /// </summary>
    public class OrdersController : BaseControllers.Base
    {
        private readonly IOrderService _orderService;

        #region constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="configuration"></param>
        /// <param name="orderService"></param>
        public OrdersController(ILogger<BaseControllers.Base> logger, IConfiguration configuration,
            IOrderService orderService)
            : base(logger, configuration)
        {
            _orderService = orderService;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Api end point to create/update orders
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [ValidateModelState]
        [HttpPut("orders")]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Orders([FromBody]OrderModel order)
        {
            Guard.IsNotNull(order, ()=> order);

            Logger.Log(LogLevel.Debug, JsonConvert.SerializeObject(order));

            await _orderService.SaveOrderAsync(order).ConfigureAwait(false);

            return Ok(new ApiResponse(HttpStatusCode.OK, Global.OrderProcessed));
        }
        #endregion

    }
}
