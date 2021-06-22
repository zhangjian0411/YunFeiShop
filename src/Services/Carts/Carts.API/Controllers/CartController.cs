using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ZhangJian.YunFeiShop.BuildingBlocks.SeedWork.Application.Commands;
using ZhangJian.YunFeiShop.Services.Carts.Application.Commands;
using ZhangJian.YunFeiShop.Services.Carts.Application.Queries;
using ZhangJian.YunFeiShop.Services.Carts.Domain.AggregatesModel.CartAggregate;

namespace ZhangJian.YunFeiShop.Services.Carts.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICartQueries _cartQueries;
        private readonly ILogger<CartController> _logger;
        public CartController(IMediator mediator, ICartQueries cartQueries, ILogger<CartController> logger)
        {
            _mediator = mediator;
            _cartQueries = cartQueries;
            _logger = logger;
        }

        #region Commands

        [HttpPost("AddItemToCart")]
        public async Task<IActionResult> AddItemToCartAsync(
            AddItemToCartCommand command, [FromHeader(Name = "x-requestid"), Required] Guid requestId)
        {
            var identifiedCommand = new IdentifiedCommand<AddItemToCartCommand, bool>(command, requestId);

            var commandResult = await _mediator.Send(identifiedCommand);

            if (!commandResult)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPut("UpdateOrCreateCartLine")]
        public async Task<IActionResult> UpdateOrCreateCartLineAsync(
            UpdateOrCreateCartLineCommand command, [FromHeader(Name = "x-requestid"), Required] Guid requestId)
        {
            var identifiedCommand = new IdentifiedCommand<UpdateOrCreateCartLineCommand, bool>(command, requestId);

            var commandResult = await _mediator.Send(identifiedCommand);

            if (!commandResult)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPut("RemoveCartLines")]
        public async Task<IActionResult> RemoveCartLinesAsync(
            RemoveCartLinesCommand command, [FromHeader(Name = "x-requestid"), Required] Guid requestId)
        {
            var identifiedCommand = new IdentifiedCommand<RemoveCartLinesCommand, bool>(command, requestId);

            var commandResult = await _mediator.Send(command);

            if (!commandResult)
            {
                return BadRequest();
            }

            return Ok();
        }

        #endregion

        #region Queries

        [HttpGet("{buyerId:guid}")]
        public async Task<ActionResult<Cart>> GetCartAsync(Guid buyerId)
        {
            var cart = await _cartQueries.GetCartAsync(buyerId);

            return Ok(cart ?? new Cart(buyerId));
        }

        #endregion
    }
}