using MediatR;
using MShop.Application.Dtos;
using MShop.Core.Base;
using MShop.Core.DomainObject;
using MShop.Infra.Data.Interface;
using Message = MShop.Core.Message;

namespace MShop.Application.Queries.Handlers
{
    public class GetAddressByIdHandler : BaseQuery, IRequestHandler<GetAddressByIdQuery, Result<AddressResultDto>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IAddressRepository _addressRepository;

        public GetAddressByIdHandler(
            IAddressRepository addressRepository,
            ICustomerRepository customerRepository,
            Message.INotification notification
        ) : base(notification)
        {
            _customerRepository = customerRepository;
            _addressRepository = addressRepository;
        }

        public async Task<Result<AddressResultDto>> Handle(GetAddressByIdQuery request, CancellationToken cancellationToken)
        {
            var addressFilter = await _addressRepository.Filter(a => a.Id == request.Id);
            var address = addressFilter.FirstOrDefault();
            if (address is null)
            {
                Notificar("Address não encontrado.");
                return Result<AddressResultDto>.Error(Notifications);
            }

            if (!address.IsValid(Notifications) || TheareErrors())
                return Result<AddressResultDto>.Error(Notifications);

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
            };

            return Result<AddressResultDto>.Success(dto);
        }
    }
}