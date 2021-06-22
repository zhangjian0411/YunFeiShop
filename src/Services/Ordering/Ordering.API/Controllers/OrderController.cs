using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZhangJian.YunFeiShop.BuildingBlocks.SeedWork.Application.Commands;
using ZhangJian.YunFeiShop.Services.Ordering.API.Application.Commands;
using ZhangJian.YunFeiShop.Services.Ordering.API.Application.Queries;
using ZhangJian.YunFeiShop.Services.Ordering.Domain.AggregatesModel.OrderAggregate;

namespace ZhangJian.YunFeiShop.Services.Ordering.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IOrderQueries _orderQueries;

        public OrderController(IMediator mediator, IOrderQueries orderQueries)
        {
            _mediator = mediator;
            _orderQueries = orderQueries;
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

        #region Queries

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersAsync([FromQuery, Required] Guid buyerId) {
            var orders = await _orderQueries.GetOrdersAsync(buyerId);

            return Ok (orders);
        }

        #endregion
    }
}
