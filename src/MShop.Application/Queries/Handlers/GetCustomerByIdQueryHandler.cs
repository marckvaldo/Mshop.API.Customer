using MediatR;
using Mshop.Application.Dtos;
using Mshop.Core;
using Mshop.Core.Base;
using Mshop.Core.Data;
using Mshop.Core.DomainObject;
using Mshop.Infra.Data.Interface;
using Message = Mshop.Core.Message;

namespace Mshop.Application.Queries.Handlers
{
    public class GetCustomerByIdQueryHandler : BaseQuery, IRequestHandler<GetCustomerByIdQuery, Result<CustomerResultDto>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IAddressRepository _addressRepository;

        public GetCustomerByIdQueryHandler(
            IAddressRepository addressRepository,
            ICustomerRepository customerRepository,            
            Message.INotification notification
        ) : base(notification)
        {
            _customerRepository = customerRepository;
            _addressRepository = addressRepository;

        }

        public async Task<Result<CustomerResultDto>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetById(request.Id);
            if (customer == null)
            {
                Notificar("Customer não encontrado.");
                return Result<CustomerResultDto>.Error(Notifications);
            }

            var address = await _addressRepository.GetById(customer.AddressId);
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