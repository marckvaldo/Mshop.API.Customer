using MediatR;
using MShop.Application.Dtos;
using MShop.Core.DomainObject;
using MShop.Core.Message;
using MShop.Domain.Entities;

namespace MShop.Application.Queries
{
    public class GetAddressByIdQuery : IRequest<Result<AddressResultDto>>
    {
        public Guid Id { get; }

        public GetAddressByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}