using System;
using CompanyX.Dal.LineItems;
using CompanyX.Domain.LineItems;
using FluentAssertions;
using Xunit;

namespace CompanyX.Dal.Tests
{
    public class WebsiteDetailRepositoryTests
    {
        private readonly IRepository<WebsiteDetail> _websiteDetailRepository;

        public WebsiteDetailRepositoryTests()
        {
            _websiteDetailRepository = new WebsiteDetailRepository();
        }
        [Fact]
        public void TestSaveWebsiteDetail()
        {
            var websiteDetail = new WebsiteDetail
            {
                WebsiteBusinessName = "Test Web site"
            };

            var result = _websiteDetailRepository.Save(websiteDetail);

            result.Should().BeGreaterThan(0, "The result should contain created adword Campaign id");
        }

        [Fact]
        public void TestGetWebsiteDetail()
        {
            var websiteDetail = new WebsiteDetail
            {
                WebsiteBusinessName = "Test Web site"
            };

            var id = _websiteDetailRepository.Save(websiteDetail);

            var result = _websiteDetailRepository.Get(id);

            result.Should().NotBeNull("The result should not be null.");
            result.Id.Should().BePositive("The adword campaign must be positive");
            result.Id.Should().Be(id, "The adword campaign id must be equal input id");
        }

        [Fact]
        public void TestSaveNullWebsiteDetail()
        {
            var result = Assert.Throws<ArgumentNullException>(() => _websiteDetailRepository.Save(null));

            result.Should().NotBeNull("The result should not be null.");
            result.Message.Should().NotBeNullOrEmpty("The exception message should not be null");
        }
    }
}
