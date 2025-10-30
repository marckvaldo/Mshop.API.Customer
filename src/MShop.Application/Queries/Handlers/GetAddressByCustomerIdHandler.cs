using MediatR;
using MShop.Application.Dtos;
using MShop.Core.Base;
using MShop.Core.DomainObject;
using MShop.Infra.Data.Interface;
using Message = MShop.Core.Message;

namespace MShop.Application.Queries.Handlers
{
    public class GetAddressByCustomerIdHandler : BaseQuery, IRequestHandler<GetAddressByCustomerIdQuery, Result<ListAddressResultDto>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IAddressRepository _addressRepository;

        public GetAddressByCustomerIdHandler(
            IAddressRepository addressRepository,
            ICustomerRepository customerRepository,
            Message.INotification notification
        ) : base(notification)
        {
            _customerRepository = customerRepository;
            _addressRepository = addressRepository;
        }

        public async Task<Result<ListAddressResultDto>> Handle(GetAddressByCustomerIdQuery request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetById(request.CustomerId);
            if (customer == null)
            {
                Notificar("Customer não encontrado.");
                return Result<ListAddressResultDto>.Error(Notifications);
            }

            //var address = await _addressRepository.GetById(customer.AddressId);
            var addressFilter = await _addressRepository.Filter(a => a.CustomerId == customer.Id);
            var addresses = addressFilter.ToList();
            if (addresses is null)
            {
                Notificar("Address não encontrado.");
                return Result<ListAddressResultDto>.Error(Notifications);
            }

            var listAddress = new ListAddressResultDto();
            foreach(var address in addresses)
            {
                var dto = new AddressResultDto
                {
                    Street = address.Street,
                    Number = address.Number,
                    Complement = address.Complement,
                    District = address.District,
                    City = address.City,
                    State = address.State,
                    PostalCode = address.PostalCode,
                    Country = address.Country,
                    Id = address.Id,
                };

                listAddress.Addresses.Add(dto);  
            }

            return Result<ListAddressResultDto>.Success(listAddress);
        }
    }
}