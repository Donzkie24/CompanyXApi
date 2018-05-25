using System;
using CompanyX.Dal.Orders;
using CompanyX.Domain.Orders;
using FluentAssertions;
using Xunit;

namespace CompanyX.Dal.Tests
{
    public class AdditionalInfoRepositoryTests
    {
        private readonly IRepository<AdditionalInfo> _additionalInfoRepository;

        public AdditionalInfoRepositoryTests()
        {
            _additionalInfoRepository = new AdditionalInfoRepository();
        }
        [Fact]
        public void TestSaveAdditionalInfo()
        {
            var additionalInfo = new AdditionalInfo
            {
                ExposureId = "Test ExposureId",
                UDAC = "Test UDAC",
                RelatedOrder = "1"
            };

            var result = _additionalInfoRepository.Save(additionalInfo);

            result.Should().BeGreaterThan(0, "The result should contain created AdditionalInfo id");
        }

        [Fact]
        public void TestGetAdditionalInfo()
        {
            var additionalInfo = new AdditionalInfo
            {
                ExposureId = "Test ExposureId",
                UDAC = "Test UDAC",
                RelatedOrder = "1"
            };

            var id = _additionalInfoRepository.Save(additionalInfo);

            var result = _additionalInfoRepository.Get(id);

            result.Should().NotBeNull("The result should not be null.");
            result.Id.Should().BePositive("The AdditionalInfo must be positive");
            result.Id.Should().Be(id, "The AdditionalInfo id must be equal input id");
        }

        [Fact]
        public void TestSaveNullAdditionalInfo()
        {
            var result = Assert.Throws<ArgumentNullException>(() => _additionalInfoRepository.Save(null));

            result.Should().NotBeNull("The result should not be null.");
            result.Message.Should().NotBeNullOrEmpty("The exception message should not be null");
        }
    }
}
