using MediatR;
using MShop.Application.Dtos;
using MShop.Core.Base;
using MShop.Core.DomainObject;
using MShop.Infra.Data.Interface;
using Message = MShop.Core.Message;

namespace MShop.Application.Queries.Handlers
{
    public class GetCustomerByNameQueryHandler : BaseQuery, IRequestHandler<GetCustomerByNameQuery, Result<CustomerResultDto>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IAddressRepository _addressRepository;

        public GetCustomerByNameQueryHandler(
            IAddressRepository addressRepository,
            ICustomerRepository customerRepository,
            Message.INotification notification
        ) : base(notification)
        {
            _customerRepository = customerRepository;
            _addressRepository = addressRepository;
        }

        public async Task<Result<CustomerResultDto>> Handle(GetCustomerByNameQuery request, CancellationToken cancellationToken)
        {
            var customers = await _customerRepository.Filter(c => c.Name == request.Name);
            var customer = customers.FirstOrDefault();

            if (customer == null)
            {
                Notificar("Customer não encontrado.");
                return Result<CustomerResultDto>.Error(Notifications);
            }

            //var address = await _addressRepository.GetById(customer.AddressId);
            var addressFilter = await _addressRepository.Filter(a => a.CustomerId == customer.Id);
            var address = addressFilter.FirstOrDefault();
            if (address is not null)
                customer.AddAddress(address);

            if (!customer.IsValid(Notifications) || TheareErrors())
                return Result<CustomerResultDto>.Error(Notifications);

            var dto = new CustomerResultDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                Phone = customer.Phone,
                Address = customer.Address == null ? null : new AddressResultDto
                {
                    Street = customer.Address.Street,
                    Number = customer.Address.Number,
                    Complement = customer.Address.Complement,
                    District = customer.Address.District,
                    City = customer.Address.City,
                    State = customer.Address.State,
                    PostalCode = customer.Address.PostalCode,
                    Country = customer.Address.Country
                }
            };

            return Result<CustomerResultDto>.Success(dto);
        }
    }
}