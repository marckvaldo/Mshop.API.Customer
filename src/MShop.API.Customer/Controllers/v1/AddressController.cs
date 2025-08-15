using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mshop.Application.Commands;
using Message = Mshop.Core.Message;

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
    }
}
