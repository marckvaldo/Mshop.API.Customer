using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MShop.Application.Dtos;
using MShop.Core.Data;
using MShop.Domain.Entities;
using MShop.E2ETest.Base;
using MShop.E2ETest.Common;
using MShop.Infra.Data.Context;
using MShop.Infra.Data.Interface;
using MShop.Infra.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Domain =  MShop.Domain.Entities;

namespace MShop.E2ETest.API.Address
{
    [Collection("API Address Collection")]
    [CollectionDefinition("API Address Collection", DisableParallelization = true)]
    public class AddAddresAPITest : AddressAPITestFixture
    {
        private IAddressRepository _addressRepository;
        private ICustomerRepository _customerRepository;
        private IUnitOfWork _unitOfWork;
       
        public AddAddresAPITest() : base()
        {
            _addressRepository = _serviceProvider.GetRequiredService<IAddressRepository>();
            _unitOfWork = _serviceProvider.GetRequiredService<IUnitOfWork>();
            _customerRepository = _serviceProvider.GetRequiredService<ICustomerRepository>();
            

            DeleteDataBase(_dbContext).Wait();
            AddMigration(_dbContext).Wait();
        }

        [Fact(DisplayName = "Register Address With Success")]
        [Trait("EndToEnd/API", "Address - Endpoints")]
        public async Task RegisterAddressShouldReturnSuccess()
        {
            //Guid id = Guid.NewGuid();
            var customer = await CriarCustomer();
            var request = RequestAddressValidCommand(customer.Id);
            var (response, outPut) = await _apiClient.Post<CustomResponse<bool>>($"{Configuration.URL_API_ADDRESS}", request);

            var addressDb = (await _addressRepository.Filter(a => a.CustomerId == customer.Id)).FirstOrDefault();

            Assert.NotNull(addressDb);
            Assert.True(outPut.Success);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(addressDb.State, request.Address.State);
            Assert.Equal(addressDb.Street, request.Address.Street);
            Assert.Equal(addressDb.Country, request.Address.Country);
            Assert.Equal(addressDb.City, request.Address.City);
            Assert.Equal(addressDb.Number, request.Address.Number);
            Assert.Equal(addressDb.District, request.Address.District);
            Assert.Equal(addressDb.PostalCode, request.Address.PostalCode);
        }



        [Fact(DisplayName = "Delete Address With Success")]
        [Trait("EndToEnd/API", "Address - Endpoints")]
        public async Task DeleteAddressShouldReturnSuccess()
        {
            //Guid id = Guid.NewGuid();
            var customer = await CriarCustomer();
            var address = await CreateAddress(customer);
            var request = RequestAddressValidCommand(customer.Id);

            var (response, outPut) = await _apiClient.Delete<CustomResponse<bool>>($"{Configuration.URL_API_ADDRESS}{address.Id}");

            var addressDb = (await _addressRepository.Filter(a => a.CustomerId == customer.Id)).FirstOrDefault();

            Assert.Null(addressDb);
            Assert.True(outPut.Success);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }


        [Fact(DisplayName = "Get Address By Id With Success")]
        [Trait("EndToEnd/API", "Address - Endpoints")]
        public async Task GetAddressByIdShouldReturnSuccess()
        {            
            var customer = await CriarCustomer();
            var address = await CreateAddress(customer);

            var (response, outPut) = await _apiClient.Get<CustomResponse<AddressResultDto>>($"{Configuration.URL_API_ADDRESS}{address.Id}");

            var addressDb = (await _addressRepository.Filter(a => a.Id == address.Id)).FirstOrDefault();

            Assert.NotNull(addressDb);
            Assert.True(outPut.Success);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(outPut.Data.Number, address.Number);
            Assert.Equal(outPut.Data.State, address.State);
            Assert.Equal(outPut.Data.Country, address.Country);
            Assert.Equal(outPut.Data.Complement, address.Complement);
            Assert.Equal(outPut.Data.City, address.City);
            Assert.Equal(outPut.Data.District, address.District);
            Assert.Equal(outPut.Data.PostalCode, address.PostalCode);
        }


        [Fact(DisplayName = "Get Address By Customer Id With Success")]
        [Trait("EndToEnd/API", "Address - Endpoints")]
        public async Task GetAddressByCustomerIdShouldReturnSuccess()
        {
            var customer = await CriarCustomer();
            var address = await CreateAddress(customer);

            var (response, outPut) = await _apiClient.Get<CustomResponse<ListAddressResultDto>>($"{Configuration.URL_API_ADDRESS}customer/{customer.Id}");

            var addressDb = (await _addressRepository.Filter(a => a.CustomerId == customer.Id)).ToList();

            Assert.NotNull(addressDb);
            Assert.True(outPut.Success);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            foreach(var item in outPut.Data.Addresses) 
            {
                var addrees = addressDb.Where(r => r.Id == item.Id).First();
                Assert.NotNull(addrees);
                Assert.Equal(item.Number, address.Number);
                Assert.Equal(item.State, address.State);
                Assert.Equal(item.Country, address.Country);
                Assert.Equal(item.Complement, address.Complement);
                Assert.Equal(item.City, address.City);
                Assert.Equal(item.District, address.District);
                Assert.Equal(item.PostalCode, address.PostalCode);

            }
        }






        private async Task<Domain.Entities.Customer> CriarCustomer()
        {
            var customer = _customerFaker.Generate();
            customer.SetPassword("password");
            await _customerRepository.Create(customer, CancellationToken.None);
            await _unitOfWork.CommitAsync();
            //DetachedEntity(customer);
            return customer;
        }

        private async Task<Domain.ValueObjects.Address> CreateAddress(Domain.Entities.Customer customer)
        {
            var address = _addressFaker.Generate();
            address.AddCustomer(customer);
            await _addressRepository.Create(address, CancellationToken.None);
            await _unitOfWork.CommitAsync(CancellationToken.None);
            return address;
            
        }

    }
}
