using Microsoft.Extensions.DependencyInjection;
using MShop.Core.Data;
using MShop.E2ETest.Base;
using MShop.E2ETest.Common;
using MShop.Infra.Data.Interface;

namespace MShop.E2ETest.GraphQL.Address
{
    public class AddressGraphQLTestFixture : BaseFixture
    {
        protected ICustomerRepository _customerRepository;
        protected IAddressRepository _addressRepository;
        protected IUnitOfWork _unitOfWork;
        public AddressGraphQLTestFixture(TypeProject typeProject) : base(typeProject)
        {
            _customerRepository = _serviceProvider.GetRequiredService<ICustomerRepository>();
            _addressRepository = _serviceProvider.GetRequiredService<IAddressRepository>();
            _unitOfWork = _serviceProvider.GetRequiredService<IUnitOfWork>();
        }

        protected List<Domain.Entities.Customer> CustomerFaker(int quantity = 3 , bool address = true)
        {
            var customerFaker = _customerFaker.Generate(quantity);
            var listCustomer = new List<Domain.Entities.Customer>();

            foreach (var item in customerFaker)
            {
                var customerModel = new Domain.Entities.Customer(item.Name, item.Email, item.Phone);
                customerModel.SetPassword("p@ssw0rd");
                customerModel.SetProviderIdentityId(Guid.NewGuid().ToString());
                var addressFake = _addressFaker.Generate();
                var addressModel = new Domain.ValueObjects.Address(
                    addressFake.Street, 
                    addressFake.Number, 
                    addressFake.Complement, 
                    addressFake.District, 
                    addressFake.City, 
                    addressFake.State, 
                    addressFake.PostalCode, 
                    addressFake.Country);

                customerModel.AddAddress(addressModel);
                listCustomer.Add(customerModel);
            }

            return listCustomer;
        }

        protected async Task<bool> CreateCustmerDataBase(List<Domain.Entities.Customer> listCustomers)
        {
            foreach (var customer in listCustomers)
            {
               await _customerRepository.Create(customer, CancellationToken.None);
                if(customer.Address != null)
                    await _addressRepository.Create(customer.Address, CancellationToken.None);
            }

            await _unitOfWork.CommitAsync(CancellationToken.None);

            return true;
        }

    }
}
