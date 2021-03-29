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

namespace ZhangJian.YunFeiShop.Services.Carts.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;
        private readonly ILogger<CartController> _logger;
        public CartController(IMediator mediator, IIdentityService identityService, IMapper mapper, ILogger<CartController> logger)
        {
            _mediator = mediator;
            _identityService = identityService;
            _mapper = mapper;
            _logger = logger;
        }

        // [HttpPost("addtocart")]
        // public async Task<IActionResult> AddToCartAsync()
        // {
        //     var command = new AddItemToCartCommand
        //     {
        //         BuyerId = _identityService.GetUserIdentity(),
        //         ProductId = new Guid("11111112-1111-1111-1111-111111111111")
        //     };
        //     var identityCommand = new IdentifiedCommand<AddItemToCartCommand, bool>(command, new Guid("99999999-1111-1111-1111-111111111111"));
        //     await _mediator.Send(identityCommand);

        //     return Ok();
        // }

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

        [HttpPut("items")]
        public async Task<IActionResult> UpdateCartItemAsync(UpdateCartItemRequest request, [FromHeader(Name = "x-requestid")] string requestId)
        {
            bool commandResult = false;

            if (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty)
            {
                var command = _mapper.Map<UpdateCartItemCommand>(request);
                var identifiedCommand = new IdentifiedCommand<UpdateCartItemCommand, bool>(command, guid);

                commandResult = await _mediator.Send(identifiedCommand);
            }

            if (!commandResult)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpDelete("items")]
        public async Task<IActionResult> RemoveCartItemsAsync(RemoveCartItemsRequest request)
        {
            var command = _mapper.Map<RemoveCartItemsCommand>(request);

            await _mediator.Send(command);

            return Ok();
        }
    }
}
