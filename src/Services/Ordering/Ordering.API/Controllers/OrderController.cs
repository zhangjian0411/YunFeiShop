using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZhangJian.YunFeiShop.BuildingBlocks.SeedWork.Application.Commands;
using ZhangJian.YunFeiShop.Services.Ordering.API.Application.Commands;

namespace ZhangJian.YunFeiShop.Services.Ordering.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost("PlaceOrder")]
        public async Task<IActionResult> PlaceOrderAsync(
            PlaceOrderCommand command,
            [FromHeader(Name = "x-requestid"), Required] Guid requestId)
        {
            var identifiedCommand = new IdentifiedCommand<PlaceOrderCommand, bool>(command, requestId);

            var commandResult = await _mediator.Send(identifiedCommand);

            if (!commandResult)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
