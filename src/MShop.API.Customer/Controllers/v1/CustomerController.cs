using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mshop.Application.Commands;
using Mshop.Application.Dtos;
using Mshop.Application.Queries;
using Mshop.Core.DomainObject;
using System.Runtime.InteropServices;
using Message = Mshop.Core.Message;

namespace MShop.API.Customer.Controllers.v1
{
    [ApiVersion(1.0)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CustomerController : MainController
    {
        private readonly IMediator _mediator;
        public CustomerController(Message.INotification notification, IMediator mediator) : base(notification)
        {
            _mediator = mediator;
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(CustomerResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<CustomerResultDto>> Customer(Guid id, CancellationToken cancellation)
        {
            var result = await _mediator.Send(new GetCustomerByIdQuery(id));
            if (result.Data is null) return CustomResponse(404);
            return CustomResponse(result.Data);
        }

        [HttpGet("email/{email}")]
        [ProducesResponseType(typeof(CustomerResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CustomerResultDto>> CustomerByEmail(string email, CancellationToken cancellation)
        {
            var result = await _mediator.Send(new GetCustomerByEmailQuery(email));
            if (result.Data is null) return CustomResponse(404);
            return CustomResponse(result.Data);
        }

        [HttpGet("name/{name}")]
        [ProducesResponseType(typeof(CustomerResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<CustomerResultDto>> CustomerByName(string name, CancellationToken cancellation)
        {
            var result = await _mediator.Send(new GetCustomerByNameQuery(name));
            if (result.Data is null) return CustomResponse(404);
            return CustomResponse(result.Data);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CustomerResultDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<bool>> CreateCustomer([FromBody] CreateCustomerCommand command, CancellationToken cancellation)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _mediator.Send(command, cancellation);
            if (!result) return CustomResponse(400);
            return CustomResponse(result);
        }

        [HttpPut]
        [ProducesResponseType(typeof(CustomerResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> UpdateCustomer(Guid id, [FromBody] UpdateCustomerCommand command, CancellationToken cancellation)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            if(command.Customer.Id != id)
            {
                Notify("O id do cliente não confere com o id da rota.");
                return CustomResponse(400);
            }

            var result = await _mediator.Send(command, cancellation);
            return CustomResponse(result);
        }


    }
}
