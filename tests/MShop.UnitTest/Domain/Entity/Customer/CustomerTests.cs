using Xunit;
using DomainEntity = MShop.Domain.Entities;
using MShop.Core.Test.Domain.Entity.Customer;
using MShop.Core.Test.Domain.Entity.Address;
using MShop.Core.Message;
using Moq;

namespace MShop.UnitTest.Domain.Entity.Customer
{
    public class CustomerTests
    {
        [Fact]
        public void Customer_ValidData_ShouldBeValid()
        {
            var customer = new CustomerFaker().Generate();
            var notification = new Mock<INotification>();
            notification.Setup(n => n.HasErrors()).Returns(false);

            var result = customer.IsValid(notification.Object);

            Assert.True(result);
        }

        [Theory]
        [InlineData("", "email@email.com", "11999999999")]
        [InlineData("Nome", "", "11999999999")]
        [InlineData("Nome", "email@email.com", "")]
        public void Customer_InvalidData_ShouldBeInvalid(string name, string email, string phone)
        {
            var customer = new DomainEntity.Customer(name, email, phone);
            var notification = new Notifications();

            var result = customer.IsValid(notification);

            Assert.False(result);
        }

        [Fact]
        public void Customer_AddAddress_ShouldSetAddress()
        {
            var customer = new CustomerFaker().Generate();
            var address = new AddressFaker().Generate();

            var result = customer.AddAddress(address);

            Assert.True(result);
            Assert.Equal(address, customer.Address);
        }

        [Fact]
        public void Customer_RemoveAddress_ShouldUnsetAddress()
        {
            var customer = new CustomerFaker().Generate();
            var address = new AddressFaker().Generate();
            customer.AddAddress(address);

            var result = customer.RemoveAddress();

            Assert.True(result);
            Assert.Null(customer.Address);
        }

        [Fact]
        public void Customer_UpdateCustomer_ShouldUpdateFields()
        {
            var customer = new CustomerFaker().Generate();
            customer.UpdateCustomer("Novo Nome", "novo@email.com", "11988888888");

            Assert.Equal("Novo Nome", customer.Name);
            Assert.Equal("novo@email.com", customer.Email);
            Assert.Equal("11988888888", customer.Phone);
        }
    }
}