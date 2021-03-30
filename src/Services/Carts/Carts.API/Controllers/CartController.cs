using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ZhangJian.YunFeiShop.BuildingBlocks.SeedWork.Application.Commands;
using ZhangJian.YunFeiShop.Services.Carts.API.Infrastructure.RequestModels;
using ZhangJian.YunFeiShop.Services.Carts.API.Infrastructure.Services;
using ZhangJian.YunFeiShop.Services.Carts.Application.Commands;
using ZhangJian.YunFeiShop.Services.Carts.Application.Queries;

namespace ZhangJian.YunFeiShop.Services.Carts.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICartQueries _cartQueries;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;
        private readonly ILogger<CartController> _logger;
        public CartController(IMediator mediator, ICartQueries cartQueries, IIdentityService identityService, IMapper mapper, ILogger<CartController> logger)
        {
            _mediator = mediator;
            _cartQueries = cartQueries;
            _identityService = identityService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetCartAsync()
        {
            var cart = await _cartQueries.GetCartAsync(_identityService.GetUserIdentity());

            return Ok(cart);
        }

        [HttpPut("checkout")]
        public async Task<IActionResult> CheckOutAsync([FromHeader(Name = "x-requestid")] string requestId)
        {
            bool commandResult = false;

            if (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty)
            {
                var identifiedCommand = new IdentifiedCommand<CheckOutCommand, bool>(
                        new CheckOutCommand { BuyerId = _identityService.GetUserIdentity() },
                        guid);

                commandResult = await _mediator.Send(identifiedCommand);
            }

            if (!commandResult)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPut("lines")]
        public async Task<IActionResult> UpdateOrCreateCartLineAsync(UpdateOrCreateCartLineRequest request, [FromHeader(Name = "x-requestid")] string requestId)
        {
            bool commandResult = false;

            if (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty)
            {
                var command = _mapper.Map<UpdateOrCreateCartLineCommand>(request);
                var identifiedCommand = new IdentifiedCommand<UpdateOrCreateCartLineCommand, bool>(command, guid);

                commandResult = await _mediator.Send(identifiedCommand);
            }

            if (!commandResult)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpDelete("lines")]
        public async Task<IActionResult> RemoveCartLinesAsync(RemoveCartLinesRequest request)
        {
            var command = _mapper.Map<RemoveCartLinesCommand>(request);

            await _mediator.Send(command);

            return Ok();
        }
    }
}
