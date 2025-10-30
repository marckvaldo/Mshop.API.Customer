using Xunit;
using DomainEntity = MShop.Domain.ValueObjects;
using MShop.Core.Test.Domain.Entity.Address;
using MShop.Core.Message;
using Moq;

namespace MShop.UnitTest.Domain.Entity.Address
{
    public class AddressTests
    {
        [Fact]
        public void Address_ValidData_ShouldBeValid()
        {
            var address = new AddressFaker().Generate();
            var notification = new Mock<INotification>();
            notification.Setup(n => n.HasErrors()).Returns(false);

            var result = address.IsValid(notification.Object);

            Assert.True(result);
        }

        [Theory]
        [InlineData("", "123", "Apt 1", "Centro", "São Paulo", "SP", "01000-000", "Brasil")]
        [InlineData("Rua A", "", "Apt 1", "Centro", "São Paulo", "SP", "01000-000", "Brasil")]
        //[InlineData("Rua A", "123", "", "Centro", "São Paulo", "SP", "01000-000", "Brasil")]
        [InlineData("Rua A", "123", "Apt 1", "", "São Paulo", "SP", "01000-000", "Brasil")]
        [InlineData("Rua A", "123", "Apt 1", "Centro", "", "SP", "01000-000", "Brasil")]
        [InlineData("Rua A", "123", "Apt 1", "Centro", "São Paulo", "", "01000-000", "Brasil")]
        [InlineData("Rua A", "123", "Apt 1", "Centro", "São Paulo", "SP", "", "Brasil")]
        [InlineData("Rua A", "123", "Apt 1", "Centro", "São Paulo", "SP", "01000-000", "")]
        public void Address_InvalidData_ShouldBeInvalid(
            string street, string number, string complement, string district,
            string city, string state, string postalCode, string country)
        {
            var address = new DomainEntity.Address(street, number, complement, district, city, state, postalCode, country);
            var notification = new Mock<INotification>();
            notification.Setup(n => n.AddNotifications(It.IsAny<string>()));

            var result = address.IsValid(notification.Object);

            Assert.False(result);
        }
    }
}