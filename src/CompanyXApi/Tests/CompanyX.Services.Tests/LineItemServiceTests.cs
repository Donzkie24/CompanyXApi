using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompanyX.Api.Models.LineItems;
using CompanyX.Dal;
using CompanyX.Domain.LineItems;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CompanyX.Services.Tests
{
    public class LineItemServiceTests
    {
        private readonly ILineItemService _lineItemService;

        private readonly Mock<IRepository<LineItem>> _lineItemRepositoryMock;
        private readonly Mock<IRepository<AdWordCampaign>> _adWordCampaignRepositoryMock;
        private readonly Mock<IRepository<WebsiteDetail>> _websiteDetailRepositoryMock;

        public LineItemServiceTests()
        {
            _lineItemRepositoryMock = new Mock<IRepository<LineItem>>();
            _adWordCampaignRepositoryMock = new Mock<IRepository<AdWordCampaign>>();
            _websiteDetailRepositoryMock = new Mock<IRepository<WebsiteDetail>>();

            var logger = new Mock<ILogger<BaseService>>();

            _lineItemService = new LineItemService(logger.Object,
                                        _lineItemRepositoryMock.Object,
                                        _adWordCampaignRepositoryMock.Object,
                                        _websiteDetailRepositoryMock.Object);
        }

        [Fact]
        public async Task TestWebsiteDetailsLineItem()
        {
            #region data and mock
            var websiteDetailsLineItem = new WebsiteDetailsLineItemModel
            {
                Category = "test",
                Notes = "Test Note",
                WebsiteDetails = new WebsiteDetailsProduct
                {
                    TemplateId = "Test Id",
                }
            };

            _websiteDetailRepositoryMock.Setup(c => c.Save(It.IsAny<WebsiteDetail>())).Returns(1);

            var inputLineItems = new List<LineItemModel> { websiteDetailsLineItem };
            #endregion

            var result = await _lineItemService.CreateLineItemsAsync(inputLineItems).ConfigureAwait(false);

            result.Should().NotBeNullOrEmpty("Line items must be created");
            result.Count.Should().Be(1, "Line items must be created");
            result.First().WebsiteDetail.Should().NotBeNull("Web site detail line item should be created");
            result.First().AdWordCampaign.Should().BeNull("Add word campaign should not exist");
        }

        [Fact]
        public async Task TestAdWordCampaignLineItem()
        {
            #region data and mock

            var adWordCampaignLineItem = new AdWordCampaignLineItemModel()
            {
                Category = "test",
                Notes = "Test Note",
                AdWordCampaign = new AddWordCampaignProduct
                {
                    CampaignName = "Test campaign"
                }
            };

            _adWordCampaignRepositoryMock.Setup(c => c.Save(It.IsAny<AdWordCampaign>())).Returns(1);

            var inputLineItems = new List<LineItemModel> { adWordCampaignLineItem };
            #endregion

            var result = await _lineItemService.CreateLineItemsAsync(inputLineItems).ConfigureAwait(false);

            result.Should().NotBeNullOrEmpty("Line items must be created");
            result.Count.Should().Be(1, "Line items must be created");
            result.First().AdWordCampaign.Should().NotBeNull(" Add word campaign line item should be created");
            result.First().WebsiteDetail.Should().BeNull(" Web site detail line item should not exist");
        }

        [Fact]
        public async Task TestMultipleLineItems()
        {
            #region data and mock
            var websiteDetailsLineItem = new WebsiteDetailsLineItemModel
            {
                Category = "test",
                Notes = "Test Note",
                WebsiteDetails = new WebsiteDetailsProduct
                {
                    TemplateId = "Test Id",
                }
            };

            _websiteDetailRepositoryMock.Setup(c => c.Save(It.IsAny<WebsiteDetail>())).Returns(1);

            var adWordCampaignLineItem = new AdWordCampaignLineItemModel()
            {
                Category = "test",
                Notes = "Test Note",
                AdWordCampaign = new AddWordCampaignProduct
                {
                    CampaignName = "Test campaign"
                }
            };

            _adWordCampaignRepositoryMock.Setup(c => c.Save(It.IsAny<AdWordCampaign>())).Returns(1);

            var inputLineItems = new List<LineItemModel> { adWordCampaignLineItem, websiteDetailsLineItem };
            #endregion

            var result = await _lineItemService.CreateLineItemsAsync(inputLineItems).ConfigureAwait(false);

            result.Should().NotBeNullOrEmpty("Line items must be created");
            result.Count.Should().Be(2, "two Line items must be created");
            result.First().AdWordCampaign.Should().NotBeNull(" Add word campaign line item should be created");
            result.Last().WebsiteDetail.Should().NotBeNull(" Web site detail line item should not exist");
        }

        [Fact]
        public async Task TestNoLineItems()
        {
            var result = await Assert.ThrowsAsync<ArgumentNullException>(() => _lineItemService.CreateLineItemsAsync(null));

            result.Should().NotBeNull("The result should not be null.");
            result.Message.Should().NotBeNullOrEmpty("The exception message should not be null");
        }
    }
}
