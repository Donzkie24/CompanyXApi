using System;
using CompanyX.Dal.Orders;
using CompanyX.Domain.Orders;
using FluentAssertions;
using Xunit;

namespace CompanyX.Dal.Tests
{
    public class CustomerRepositoryTests
    {
        private readonly IRepository<Customer> _customerRepository;

        public CustomerRepositoryTests()
        {
            _customerRepository = new CustomerRepository();
        }
        [Fact]
        public void TestSaveCustomer()
        {
            var customer = new Customer
            {
                ContactEmail = "test@email.com",
                ContactFirstName = "test",
                ContactLastName = "testLast"
            };

            var result =_customerRepository.Save(customer);

            result.Should().BeGreaterThan(0, "The result should contain created customer id");
        }

        [Fact]
        public void TestGetCustomer()
        {
            var customer = new Customer
            {
                ContactEmail = "test@email.com",
                ContactFirstName = "test",
                ContactLastName = "testLast"
            };

            var id = _customerRepository.Save(customer);

            var result = _customerRepository.Get(id);

            result.Should().NotBeNull("The result should not be null.");
            result.Id.Should().BePositive("The customer must be positive");
            result.Id.Should().Be(id, "The customer id must be equal input id");
        }

        [Fact]
        public void TestSaveNullCustomer()
        {
            var result = Assert.Throws<ArgumentNullException>(() => _customerRepository.Save(null));

            result.Should().NotBeNull("The result should not be null.");
            result.Message.Should().NotBeNullOrEmpty("The exception message should not be null");
        }
    }
}
