using System;
using CompanyX.Dal.LineItems;
using CompanyX.Domain.LineItems;
using FluentAssertions;
using Xunit;

namespace CompanyX.Dal.Tests
{
    public class AdWordCampaignRepositoryTests
    {
        private readonly IRepository<AdWordCampaign> _adwordCampaignRepository;

        public AdWordCampaignRepositoryTests()
        {
            _adwordCampaignRepository = new AdWordCampaignRepository();
        }
        [Fact]
        public void TestSaveAdwordCampaign()
        {
            var adwordCampaign = new AdWordCampaign
            {
                CampaignName = "Test Campaign"
            };

            var result = _adwordCampaignRepository.Save(adwordCampaign);

            result.Should().BeGreaterThan(0, "The result should contain created adword Campaign id");
        }

        [Fact]
        public void TestGetAdwordCampaign()
        {
            var adwordCampaign = new AdWordCampaign
            {
                CampaignName = "Test Campaign"
            };

            var id = _adwordCampaignRepository.Save(adwordCampaign);

            var result = _adwordCampaignRepository.Get(id);

            result.Should().NotBeNull("The result should not be null.");
            result.Id.Should().BePositive("The adword campaign must be positive");
            result.Id.Should().Be(id, "The adword campaign id must be equal input id");
        }

        [Fact]
        public void TestSaveNullAdwordCampaign()
        {
            var result = Assert.Throws<ArgumentNullException>(() => _adwordCampaignRepository.Save(null));

            result.Should().NotBeNull("The result should not be null.");
            result.Message.Should().NotBeNullOrEmpty("The exception message should not be null");
        }
    }
}
