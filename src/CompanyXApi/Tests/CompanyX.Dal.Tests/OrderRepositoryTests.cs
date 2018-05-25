using System;
using System.Linq;
using CompanyX.Dal.Orders;
using CompanyX.Domain.Orders;
using FluentAssertions;
using Xunit;

namespace CompanyX.Dal.Tests
{
    public class OrderRepositoryTests
    {
        private readonly IRepository<Order> _orderRepository;

        public OrderRepositoryTests()
        {
            _orderRepository = new OrderRepository();
        }
        [Fact]
        public void TestSaveOrder()
        {
            var order = new Order
            {
               OrderId = "24",
            };

            var result = _orderRepository.Save(order);

            result.Should().BeGreaterThan(0, "The result should contain created order id");
        }

        [Fact]
        public void TestGetOrder()
        {
            var order = new Order
            {
                OrderId = "24",
            };

            var id = _orderRepository.Save(order);

            var result = _orderRepository.Get(id);

            result.Should().NotBeNull("The result should not be null.");
            result.Id.Should().BePositive("The order must be positive");
            result.Id.Should().Be(id, "The order id must be equal input id");
        }


        [Fact]
        public void TestMultipleOrders()
        {
            var order1 = new Order
            {
                OrderId = "24",
            };

            var order2 = new Order
            {
                OrderId = "25",
            };
            var id = _orderRepository.Save(order1);
            _orderRepository.Save(order2);

            var result = _orderRepository.GetAll();

            result.Should().NotBeNull("The result should not be null.");
            result.Count().Should().Be(2,"The orders count must match");
        }

        [Fact]
        public void TestSaveSameOrders()
        {
            var order1 = new Order
            {
                OrderId = "24",
            };

            
            var id = _orderRepository.Save(order1);

            var order2 = new Order
            {
                Id = id,
                OrderId = "25",
            };

            _orderRepository.Save(order2);

            var result = _orderRepository.GetAll();

            result.Should().NotBeNull("The result should not be null.");
            result.Count().Should().Be(1, "The orders count must match");
            result.First().OrderId.Should().Be("25", "The orderId must be updated with new value");
        }

        [Fact]
        public void TestSaveNullOrder()
        {
            var result = Assert.Throws<ArgumentNullException>(() => _orderRepository.Save(null));

            result.Should().NotBeNull("The result should not be null.");
            result.Message.Should().NotBeNullOrEmpty("The exception message should not be null");
        }
    }
}
