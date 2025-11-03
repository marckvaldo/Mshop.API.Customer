using MediatR;
using MShop.Application.Queries;
using Notification = MShop.Core.Message;

namespace MShop.API.GraphQL.GraphQL.Customer
{
    [ExtendObjectType(OperationTypeNames.Query)]
    public class CustomerQueries : BaseGraphQL
    {
        public async Task<CustomerPayload> GetCustomerById(
            [Service] IMediator mediator,
            [Service] Notification.INotification notification,
            Guid id,
            CancellationToken cancellationToken)
        {
            var requestCustomer = new GetCustomerByIdQuery(id);
            var outPutCustomer = await mediator.Send(requestCustomer, cancellationToken);

            RequestIsValid(notification);
            
            var requestAddress = new GetAddressByCustomerIdQuery(id);
            var outPutAddress = await mediator.Send(requestAddress, cancellationToken);


            var resultCustemer = outPutCustomer.Data;
            var resultAddress = outPutAddress.Data;

            var result = new CustomerPayload()
            {
                Address = resultAddress.Addresses.First(),
                Email = resultCustemer.Email,
                Name = resultCustemer.Name,
                Phone = resultCustemer.Phone,
                Id = resultCustemer.Id
            };

            return result;
        }


        public async Task<CustomerPayload> GetCustomerByName(
            [Service] IMediator mediator,
            [Service] Notification.INotification notification,
            string name,
            CancellationToken cancellationToken)
        {
            var requestCustomer = new GetCustomerByNameQuery(name);
            var outPutCustomer = await mediator.Send(requestCustomer, cancellationToken);

            RequestIsValid(notification);

            var requestAddress = new GetAddressByCustomerIdQuery(outPutCustomer.Data.Id);
            var outPutAddress = await mediator.Send(requestAddress, cancellationToken);

            var resultCustemer = outPutCustomer.Data;
            var resultAddress = outPutAddress.Data;

            var result = new CustomerPayload()
            {
                Address = resultAddress.Addresses.First(),
                Email = resultCustemer.Email,
                Name = resultCustemer.Name,
                Phone = resultCustemer.Phone,
                Id = resultCustemer.Id
            };

            return result;
        }


        public async Task<CustomerPayload> GetCustomerByEmail(
            [Service] IMediator mediator,
            [Service] Notification.INotification notification,
            string name,
            CancellationToken cancellationToken)
        {
            var requestCustomer = new GetCustomerByEmailQuery(name);
            var outPutCustomer = await mediator.Send(requestCustomer, cancellationToken);

            RequestIsValid(notification);

            var requestAddress = new GetAddressByCustomerIdQuery(outPutCustomer.Data.Id);
            var outPutAddress = await mediator.Send(requestAddress, cancellationToken);

            var resultCustemer = outPutCustomer.Data;
            var resultAddress = outPutAddress.Data;

            var result = new CustomerPayload()
            {
                Address = resultAddress.Addresses.First(),
                Email = resultCustemer.Email,
                Name = resultCustemer.Name,
                Phone = resultCustemer.Phone,
                Id = resultCustemer.Id
            };

            return result;
        }
    }
}
