using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using CompanyX.Api.Models.LineItems;
using CompanyX.Api.Models.Orders;
using CompanyX.Api.Models.Response;
using CompanyX.Api.V1.Controllers;
using CompanyX.Domain.LineItems;
using CompanyX.Domain.Orders;
using CompanyX.Resource;
using CompanyX.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CompanyX.Api.Tests
{
    public class OrdersControllerTests
    {
        private readonly Mock<IOrderService> _orderServiceMock;
        private readonly OrdersController _ordersController;

        public OrdersControllerTests()
        {
            _orderServiceMock = new Mock<IOrderService>();
            var logger = new Mock<ILogger<BaseControllers.Base>>();
            var config = new Mock<IConfiguration>();

            _ordersController = new OrdersController(logger.Object, config.Object, _orderServiceMock.Object);
        }

        [Fact]
        public async Task TestApiAdWordCampaignOrder()
        {
            #region test data and setup

            var inputOrder = new OrderModel
            {
                Partner = "Test partner",
                OrderId = "Test order Id",
                LineItems = new List<LineItemModel> { GetAdWordCampaignLineItemModel() }
            };

            _orderServiceMock.Setup(o => o.SaveOrderAsync(inputOrder)).Returns(
                Task.FromResult<Order>(new Order()));
            #endregion

            var result = await _ordersController.Orders(inputOrder).ConfigureAwait(false) as OkObjectResult;


            result.StatusCode.Should().Be((int)HttpStatusCode.OK, "Orders created status ok");
            result.Value.As<ApiResponse>().Message.Should().Be(Global.OrderProcessed, "Orders created status ok");
        }

        [Fact]
        public async Task TestApiWebSiteDetailOrder()
        {
            #region test data and setup

            var inputOrder = new OrderModel
            {
                Partner = "Test partner",
                LineItems = new List<LineItemModel> { GetWebsiteDetailsLineItemModel() }
            };

            _orderServiceMock.Setup(o => o.SaveOrderAsync(inputOrder)).Returns(
                Task.FromResult<Order>(
                    new Order
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
                                WebsiteDetail = new WebsiteDetail {Id = 1, TemplateId = "Test template Id",}
                            }
                        }
                    }
                ));
            #endregion

            var result = await _ordersController.Orders(inputOrder).ConfigureAwait(false) as OkObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK, "Orders created status ok");
            result.Value.As<ApiResponse>().Message.Should().Be(Global.OrderProcessed, "Orders created status ok");
        }


        [Fact]
        public async Task TestApiMultipleOrder()
        {
            #region test data and setup

            var inputOrder = new OrderModel
            {
                Partner = "Test partner",
                LineItems = new List<LineItemModel> { GetWebsiteDetailsLineItemModel(), GetAdWordCampaignLineItemModel() }
            };

            _orderServiceMock.Setup(o => o.SaveOrderAsync(inputOrder)).Returns(
                Task.FromResult<Order>(new Order()));
            #endregion

            var result = await _ordersController.Orders(inputOrder).ConfigureAwait(false) as OkObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK, "Orders created status ok");
            result.Value.As<ApiResponse>().Message.Should().Be(Global.OrderProcessed, "Orders created status ok");
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
