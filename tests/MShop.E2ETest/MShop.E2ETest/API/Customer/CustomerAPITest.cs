using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MShop.Application.Dtos;
using MShop.Core.Data;
using MShop.E2ETest.Base;
using MShop.E2ETest.Common;
using MShop.Infra.Data.Context;
using MShop.Infra.Data.Interface;

namespace MShop.E2ETest.API.Customer
{
    [Collection("API Customer Collection")]
    [CollectionDefinition("API Customer Collection", DisableParallelization = true)]
    public class CustomerAPITest : CustomerAPITestFixture
    {
        protected ICustomerRepository _customerRepository;
        protected IUnitOfWork _unitOfWork;
        protected MshopDbContext _dbContext;
        public CustomerAPITest():base() 
        {
            _customerRepository = _serviceProvider.GetRequiredService<ICustomerRepository>();
            _unitOfWork = _serviceProvider.GetRequiredService<IUnitOfWork>();
            _dbContext = _serviceProvider.GetRequiredService<MshopDbContext>();

            DeleteDataBase(_dbContext).Wait();
            AddMigration(_dbContext).Wait();
        }

        [Fact(DisplayName = nameof(CreateCustomer))]
        [Trait("EndToEnd/API", "Customer - Endpoints")]
        public async Task CreateCustomer()
        {
            var request = RequestCreateCustomerValid();
            var (response, outPut) = await _apiClient.Post<CustomResponse<CustomerResultDto>>(Configuration.URL_API_CUSTOMER, request);

            Assert.NotNull(request);
            Assert.Equal(System.Net.HttpStatusCode.OK, response?.StatusCode);
            Assert.NotNull(outPut);
            Assert.True(outPut.Success);
            Assert.Equal(request.Customer.Name, outPut.Data.Name);
            Assert.Equal(request.Customer.Email, outPut.Data.Email);
            Assert.Equal(request.Customer.Phone, outPut.Data.Phone);

            var dbCategory = (await _customerRepository.Filter(c => c.Id == outPut.Data.Id)).First();

            Assert.NotNull(dbCategory);
            Assert.Equal(dbCategory.Name, request.Customer.Name);
            Assert.Equal(dbCategory.Phone, request.Customer.Phone);
            Assert.Equal(dbCategory.Email, request.Customer.Email);

        }

        [Fact(DisplayName = nameof(UpdateCustomer))]
        [Trait("EndToEnd/API", "Customer - Endpoints")]
        public async Task UpdateCustomer()
        {
            var customer = _customerFaker.Generate();
            customer.SetPassword("password@Php");
            await _customerRepository.Create(customer, CancellationToken.None);
            await _unitOfWork.CommitAsync(CancellationToken.None);

            var request = RequestUpdateCustomerValid(customer.Id);            
            var (response, outPut) = await _apiClient.Put<CustomResponse<bool>>($"{Configuration.URL_API_CUSTOMER}{customer.Id}", request);

            Assert.NotNull(response);
            Assert.Equal(System.Net.HttpStatusCode.OK, response?.StatusCode);
            Assert.NotNull(outPut);
            Assert.True(outPut.Success);

            var dbCustomer = await _customerRepository.GetById(customer.Id);

            Assert.NotNull(dbCustomer);
            Assert.Equal(dbCustomer.Name, request.Customer.Name);
            Assert.Equal(dbCustomer.Phone, request.Customer.Phone);
            Assert.Equal(dbCustomer.Email, request.Customer.Email);

        }

        
        [Fact(DisplayName = nameof(GetCustomerById))]
        [Trait("EndToEnd/API", "Customer - Endpoints")]
        public async Task GetCustomerById()
        {
            var customer = _customerFaker.Generate();
            customer.SetPassword("password@Php");
            await _customerRepository.Create(customer, CancellationToken.None);
            await _unitOfWork.CommitAsync(CancellationToken.None);

            var (response, outPut) = await _apiClient.Get<CustomResponse<CustomerResultDto>>($"{Configuration.URL_API_CUSTOMER}{customer.Id}");

            Assert.NotNull(response);
            Assert.Equal(System.Net.HttpStatusCode.OK, response?.StatusCode);
            Assert.NotNull(outPut);
            Assert.True(outPut.Success);

            Assert.Equal(customer.Name, outPut.Data.Name);
            Assert.Equal(customer.Phone, outPut.Data.Phone);
            Assert.Equal(customer.Email, outPut.Data.Email);
        }


        [Fact(DisplayName = nameof(GetCustomerByName))]
        [Trait("EndToEnd/API", "Customer - Endpoints")]
        public async Task GetCustomerByName()
        {
            var customer = _customerFaker.Generate();
            customer.SetPassword("password@Php");
            await _customerRepository.Create(customer, CancellationToken.None);
            await _unitOfWork.CommitAsync(CancellationToken.None);

            var (response, outPut) = await _apiClient.Get<CustomResponse<CustomerResultDto>>($"{Configuration.URL_API_CUSTOMER}name/{customer.Name}");

            Assert.NotNull(response);
            Assert.Equal(System.Net.HttpStatusCode.OK, response?.StatusCode);
            Assert.NotNull(outPut);
            Assert.True(outPut.Success);

            Assert.Equal(customer.Name, outPut.Data.Name);
            Assert.Equal(customer.Phone, outPut.Data.Phone);
            Assert.Equal(customer.Email, outPut.Data.Email);
        }


        [Fact(DisplayName = nameof(GetCustomerByEmail))]
        [Trait("EndToEnd/API", "Customer - Endpoints")]
        public async Task GetCustomerByEmail()
        {
            var customer = _customerFaker.Generate();
            customer.SetPassword("password@Php");
            await _customerRepository.Create(customer, CancellationToken.None);
            await _unitOfWork.CommitAsync(CancellationToken.None);

            var (response, outPut) = await _apiClient.Get<CustomResponse<CustomerResultDto>>($"{Configuration.URL_API_CUSTOMER}email/{customer.Email}");

            Assert.NotNull(response);
            Assert.Equal(System.Net.HttpStatusCode.OK, response?.StatusCode);
            Assert.NotNull(outPut);
            Assert.True(outPut.Success);

            Assert.Equal(customer.Name, outPut.Data.Name);
            Assert.Equal(customer.Phone, outPut.Data.Phone);
            Assert.Equal(customer.Email, outPut.Data.Email);
        }
    }
}
