using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompanyX.Api.Models.LineItems;
using CompanyX.Api.Models.Orders;
using CompanyX.Dal;
using CompanyX.Domain.LineItems;
using CompanyX.Domain.Orders;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CompanyX.Services.Tests
{
    public class OrderServiceTests
    {
        private readonly IOrderService _orderService;

        private readonly Mock<ILineItemService> _lineItemServiceMock;
        private readonly Mock<IRepository<Customer>> _customerRepositoryMock;
        private readonly Mock<IRepository<AdditionalInfo>> _additionalInfoRepositoryMock;
        private readonly Mock<IRepository<Order>> _orderRepositoryMock;

        public OrderServiceTests()
        {
            var logger = new Mock<ILogger<BaseService>>();
            _lineItemServiceMock = new Mock<ILineItemService>();
            _customerRepositoryMock = new Mock<IRepository<Customer>>();
            _additionalInfoRepositoryMock = new Mock<IRepository<AdditionalInfo>>();
            _orderRepositoryMock = new Mock<IRepository<Order>>();

            _orderService = new OrderService(logger.Object,
                _lineItemServiceMock.Object,
                _customerRepositoryMock.Object,
                _additionalInfoRepositoryMock.Object,
                _orderRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateOrderWebSiteDetails()
        {
            #region Setup data
            var inputOrder = new OrderModel
            {
                Partner = "Test partner",
                OrderId = "Test order Id",
                LineItems = new List<LineItemModel> { GetWebsiteDetailsLineItemModel() }
            };

            _lineItemServiceMock.Setup(l => l.CreateLineItemsAsync(inputOrder.LineItems)).Returns(
                Task.FromResult<IList<LineItem>>(
                    new List<LineItem>
                    {
                        new LineItem
                        {
                            Id = 1,
                            WebsiteDetail = new WebsiteDetail{Id = 1,TemplateId = "Test template Id", }
                        }
                    }
                ));

            _orderRepositoryMock.Setup(o => o.Save(It.IsAny<Order>())).Returns(1);
            _orderRepositoryMock.Setup(o => o.Get(1)).Returns(new Order
            {
                Id = 1,
                LineItems = new List<LineItem>
                {
                    new LineItem
                    {
                        Category = "test",
                        Notes = "Test Note",
                        WebsiteDetail = new WebsiteDetail {TemplateId = "Test template"},

                    }
                }
            });
            #endregion

            var result = await _orderService.SaveOrderAsync(inputOrder).ConfigureAwait(false);

            result.Should().NotBeNull("Order should be created");
            result.LineItems.First().WebsiteDetail.Should().NotBeNull("Order website details should be created");
        }

        [Fact]
        public async Task CreateOrderAddWordCampaign()
        {
            #region Setup data

            var inputOrder = new OrderModel
            {
                Partner = "Test partner",
                OrderId = "Test order Id",
                LineItems = new List<LineItemModel> { GetAdWordCampaignLineItemModel() }
            };

            _lineItemServiceMock.Setup(l => l.CreateLineItemsAsync(inputOrder.LineItems)).Returns(
                Task.FromResult<IList<LineItem>>(
                    new List<LineItem>
                    {
                        new LineItem
                        {
                            Id = 1,
                            AdWordCampaign = new AdWordCampaign {Id = 1, CampaignName = "Test Campaign"}
                        }
                    }
                ));

            _orderRepositoryMock.Setup(o => o.Save(It.IsAny<Order>())).Returns(1);
            _orderRepositoryMock.Setup(o => o.Get(1)).Returns(new Order
            {
                Id = 1,
                LineItems = new List<LineItem>
                {
                    new LineItem
                    {
                        Category = "test",
                        Notes = "Test Note",
                        AdWordCampaign = new AdWordCampaign {Id = 1, CampaignName = "Test Campaign"}
                    }
                }
            });

            #endregion

            var result = await _orderService.SaveOrderAsync(inputOrder).ConfigureAwait(false);

            result.Should().NotBeNull("Order should be created");
            result.LineItems.First().AdWordCampaign.Should().NotBeNull("Order AddWord campaign details should be created");
            result.LineItems.First().WebsiteDetail.Should().BeNull("Order website details should not be created");
        }

        [Fact]
        public async Task CreateOrderMultipleProduct()
        {
            #region Setup data

            var inputOrder = new OrderModel
            {
                Partner = "Test partner",
                OrderId = "Test order Id",
                LineItems = new List<LineItemModel>
                {
                    GetAdWordCampaignLineItemModel(),
                    GetWebsiteDetailsLineItemModel(),
                }
            };

            _lineItemServiceMock.Setup(l => l.CreateLineItemsAsync(inputOrder.LineItems)).Returns(
                Task.FromResult<IList<LineItem>>(
                    new List<LineItem>
                    {
                        new LineItem
                        {
                            Id = 1,
                            AdWordCampaign = new AdWordCampaign {Id = 1, CampaignName = "Test Campaign"},
                        },
                        new LineItem
                        {
                            Id = 2,
                            WebsiteDetail = new WebsiteDetail{Id = 1,TemplateId = "Test template Id", }
                        }
                    }
                ));

            _orderRepositoryMock.Setup(o => o.Save(It.IsAny<Order>())).Returns(1);
            _orderRepositoryMock.Setup(o => o.Get(1)).Returns(new Order
            {
                Id = 1,
                LineItems = new List<LineItem>
                {
                    new LineItem
                    {
                        Id = 1,
                        Category = "test AdWordCampaign",
                        Notes = "Test Note",
                        AdWordCampaign = new AdWordCampaign {Id = 1, CampaignName = "Test Campaign"},
                    },
                    new LineItem
                    {
                        Id = 2,
                        Category = "test WebsiteDetail",
                        Notes = "Test Note",
                        WebsiteDetail = new WebsiteDetail{Id = 1,TemplateId = "Test template Id", }
                    }
                }
            });

            #endregion

            var result = await _orderService.SaveOrderAsync(inputOrder).ConfigureAwait(false);

            result.Should().NotBeNull("Order should be created");
            result.LineItems.First().AdWordCampaign.Should().NotBeNull("Order AddWord campaign details should be created");
            result.LineItems.Last().WebsiteDetail.Should().NotBeNull("Order website details should be created");
        }

        [Fact]
        public async Task TestNullOrder()
        {
            var result = await Assert.ThrowsAsync<ArgumentNullException>(() => _orderService.SaveOrderAsync(null));

            result.Should().NotBeNull("The result should not be null.");
            result.Message.Should().NotBeNullOrEmpty("The exception message should not be null");

        }

        private WebsiteDetailsLineItemModel GetWebsiteDetailsLineItemModel()
        {
            return new WebsiteDetailsLineItemModel
            {
                Category = "test",
                Notes = "Test Note",
                WebsiteDetails = new WebsiteDetailsProduct
                {
                    TemplateId = "Test template Id",
                }
            };
        }

        private AdWordCampaignLineItemModel GetAdWordCampaignLineItemModel()
        {
            return new AdWordCampaignLineItemModel
            {
                Category = "test",
                Notes = "Test Note",
                AdWordCampaign = new AddWordCampaignProduct
                {
                    CampaignName = "Test Campaign"
                }
            };
        }
    }
}
