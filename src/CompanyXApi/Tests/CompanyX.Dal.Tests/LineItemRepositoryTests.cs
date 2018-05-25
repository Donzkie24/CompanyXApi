using System;
using CompanyX.Dal.LineItems;
using CompanyX.Domain.LineItems;
using FluentAssertions;
using Xunit;

namespace CompanyX.Dal.Tests
{
    public class LineItemRepositoryTests
    {
        private readonly IRepository<LineItem> _lineItemRepository;

        public LineItemRepositoryTests()
        {
            _lineItemRepository = new LineItemRepository();
        }
        [Fact]
        public void TestSaveLineItem()
        {
            var lineItem = new LineItem
            {
                ProductId = "Test Product Id",
                Category = "Test Category",
                Notes = "Test notes"
            };

            var result = _lineItemRepository.Save(lineItem);

            result.Should().BeGreaterThan(0, "The result should contain created LineItem id");
        }

        [Fact]
        public void TestGetLineItem()
        {
            var lineItem = new LineItem
            {
                ProductId = "Test Product Id",
                Category = "Test Category",
                Notes = "Test notes"
            };

            var id = _lineItemRepository.Save(lineItem);

            var result = _lineItemRepository.Get(id);

            result.Should().NotBeNull("The result should not be null.");
            result.Id.Should().BePositive("The LineItem must be positive");
            result.Id.Should().Be(id, "The LineItem id must be equal input id");
        }

        [Fact]
        public void TestSaveNullLineItem()
        {
            var result = Assert.Throws<ArgumentNullException>(() => _lineItemRepository.Save(null));

            result.Should().NotBeNull("The result should not be null.");
            result.Message.Should().NotBeNullOrEmpty("The exception message should not be null");
        }
    }
}
