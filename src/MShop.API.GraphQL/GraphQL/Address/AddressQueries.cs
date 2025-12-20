using HotChocolate.Authorization;
using MediatR;
using MShop.API.Customer.Extension;
using MShop.Application.Queries;
using Notification = MShop.Core.Message;

namespace MShop.API.GraphQL.GraphQL.Address
{
    [Authorize]
    [ExtendObjectType(OperationTypeNames.Query)]
    public class AddressQueries : BaseGraphQL
    {
        [Authorize(Policy = Policies.AddressRead)]
        public async Task<AddressPayload> GetAddressById(
            [Service] IMediator mediator,
            [Service] Notification.INotification notification,
            Guid id,
            CancellationToken cancellationToken)
        {
            var request = new GetAddressByIdQuery(id);
            var outPut = await mediator.Send(request);

            RequestIsValid(notification);

            var result = new AddressPayload()
            {
                City = outPut.Data.City,
                Country = outPut.Data.Country,
                Complement = outPut.Data.Complement,
                District = outPut.Data.District,
                Id = id,
                Number = outPut.Data.Number,
                PostalCode = outPut.Data.PostalCode,
                State = outPut.Data.State,
                Street = outPut.Data.Street,
            };

            return result;
                
        }
    }
}
