using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ZhangJian.YunFeiShop.BuildingBlocks.SeedWork.Application.Commands;
using ZhangJian.YunFeiShop.Services.Carts.Application.Commands;

namespace ZhangJian.YunFeiShop.Services.Carts.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CartController> _logger;

        public CartController(IMediator mediator, ILogger<CartController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> UserCheckOutAsync()
        {
            var buyerId = new Guid("FFFAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAB");
            var orderLines = new[] {
                new UserCheckOutCommand.OrderLine(new Guid("99111111-1111-1111-1111-111111111111"), "Prod1", 1),
                new UserCheckOutCommand.OrderLine(new Guid("99222222-1111-1111-1111-111111111111"), "Prod2", 2)
            };
            
            var userCheckOutCommand = new UserCheckOutCommand(buyerId, orderLines);
            var identifiedCommand = new IdentifiedCommand<UserCheckOutCommand, bool>(new UserCheckOutCommand(buyerId, orderLines), new Guid("50211111-1111-1111-1111-111111111111"));
            await _mediator.Send(identifiedCommand);

            return Ok("User's checkout accepted!");
        }

        [HttpPut("/lines/{lineId}/quantity")]
        public async Task<IActionResult> ChangeCartLineQuantityAsync()
        {
            return Ok();
        }

        [HttpPut("/lines/{lineId}/selected")]
        public async Task<IActionResult> ChangeCartLineSelectedStatusAsync()
        {
            return NotFound();
        }

        [HttpDelete("/lines/{lineId}")]
        public async Task<IActionResult> RemoveCartLineAsync()
        {
            return NotFound();
        }
    }
}
