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

        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            var addProductToCartCommand = new AddItemToCartCommand
            {
                BuyerId = new Guid("DDDDDDDD-AAAA-AAAA-AAAA-AAAAAAAAAAAB"),
                ProductId = new Guid("FFFAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAB")
            };
            
            await _mediator.Send(addProductToCartCommand);

            return Ok("Yes");
        }

        [HttpPut("items")]
        public async Task<IActionResult> UpdateCartItemAsync(UpdateCartItemRequest request)
        {
            _logger.LogInformation("Get UpdateCartItemRequest: {0}", Newtonsoft.Json.JsonConvert.SerializeObject(request));
            var command = _mapper.Map<UpdateCartItemCommand>(request);
            _logger.LogInformation("Get UpdateCartItemCommand: {0}", Newtonsoft.Json.JsonConvert.SerializeObject(command));

            return Ok(command);
        }

        [HttpDelete("/item/{itemId}")]
        public async Task<IActionResult> RemoveCartLineAsync()
        {
            return NotFound();
        }
    }
}
