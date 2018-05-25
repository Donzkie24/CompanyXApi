using System.Threading.Tasks;
using CompanyX.Api.Models.Orders;
using CompanyX.Base.Helpers;
using CompanyX.Dal;
using CompanyX.Domain.Orders;
using CompanyX.Services.Helpers;
using Microsoft.Extensions.Logging;

namespace CompanyX.Services
{
    #region Interface

    /// <summary>
    /// Service to interact with orders
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        /// Save order
        /// </summary>
        /// <returns></returns>
        Task SaveOrderAsync(OrderModel orderModel);
    }
    #endregion

    #region Implementation

    /// <summary>
    /// Service to interact with orders
    /// </summary>
    public class OrderService : BaseService, IOrderService
    {
        #region Members
        private readonly ILineItemService _lineItemService;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<AdditionalInfo> _additionalInfoRepository;
        private readonly IRepository<Order> _orderRepository;
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="lineItemService"></param>
        /// <param name="customerRepository"></param>
        /// <param name="additionalInfoRepository"></param>
        /// <param name="orderRepository"></param>
        // ReSharper disable once TooManyDependencies
        public OrderService(ILogger<BaseService> logger,
            ILineItemService lineItemService,
            IRepository<Customer> customerRepository,
            IRepository<AdditionalInfo> additionalInfoRepository, 
            IRepository<Order> orderRepository) 
            : base(logger)
        {
            Guard.IsNotNull(lineItemService, () => lineItemService);
            Guard.IsNotNull(customerRepository, () => customerRepository);
            Guard.IsNotNull(additionalInfoRepository, () => additionalInfoRepository);
            Guard.IsNotNull(orderRepository, () => orderRepository);

            _lineItemService = lineItemService;
            _customerRepository = customerRepository;
            _additionalInfoRepository = additionalInfoRepository;
            _orderRepository = orderRepository;
        }

        /// <summary>
        /// Save order
        /// TODO, currently domain models saved using temp in memory list object, using EFCore with unitOfWork would manage relation between models
        /// </summary>
        /// <returns></returns>
        public async Task SaveOrderAsync(OrderModel inputOrder)
        {
            Guard.IsNotNull(inputOrder, () => inputOrder);

            var order = MapperHelper.MapToOrder(inputOrder);

            var lineItems = await _lineItemService.CreateLineItemsAsync(inputOrder?.LineItems).ConfigureAwait(false);

            order.LineItems = lineItems;

            var customer = MapperHelper.MapToCustomer(inputOrder?.AdditionalFields?.Customer);

            if (customer != null)
            {
                var customerId = _customerRepository.Save(customer);
                order.Customer = _customerRepository.Get(customerId);
            }

            var additionalInfo = MapperHelper.MapToAdditionalInfo(inputOrder?.AdditionalFields?.AdditionalInfo);

            if (additionalInfo != null)
            {
                var additionalInfoId = _additionalInfoRepository.Save(additionalInfo);
                order.AdditionalInfo = _additionalInfoRepository.Get(additionalInfoId);
            }

            _orderRepository.Save(order);
        }

        #endregion
    }
}
