using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MShop.Application.Commands;
using MShop.Application.Dtos;
using MShop.Application.Queries;
using Message = MShop.Core.Message;

namespace MShop.API.Customer.Controllers.v1
{
    [ApiVersion(1.0)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AddressController : MainController
    {
        private readonly IMediator _mediator;
        public AddressController(Message.INotification notification, IMediator mediator) : base(notification)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(bool), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> CreateAddress([FromBody] AddAddressCommand command, CancellationToken cancellation)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _mediator.Send(command, cancellation);
            if (!result) return CustomResponse(400);
            return CustomResponse(result);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> DeleteAddress(Guid id, CancellationToken cancellation)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _mediator.Send(new DeleteAddressCommand(id), cancellation);
            if (!result) return CustomResponse(400);
            return CustomResponse(result);

        }


        [HttpGet("customer/{id:guid}")]
        [ProducesResponseType(typeof(AddressResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> GetAddressByCustomerId(Guid id, CancellationToken cancellation)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _mediator.Send(new GetAddressByCustomerIdQuery(id), cancellation);
            if (result.Data is null) return CustomResponse(404);
            return CustomResponse(result);

        }


        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(AddressResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> GetAddressById(Guid id, CancellationToken cancellation)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _mediator.Send(new GetAddressByIdQuery(id), cancellation);
            if (result.Data is null) return CustomResponse(404);
            return CustomResponse(result);

        }
    }
}
