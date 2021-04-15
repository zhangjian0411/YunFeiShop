using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ZhangJian.YunFeiShop.BFF.Spa.Shopping.HttpAggregator.Services;

namespace ZhangJian.YunFeiShop.BFF.Spa.Shopping.HttpAggregator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogService _catalogService;
        private readonly ICartService _cartService;
        private readonly IIdentityService _identityService;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(ICatalogService catalogService, ICartService cartService,IIdentityService identityService, ILogger<CatalogController> logger)
        {
            _catalogService = catalogService;
            _cartService = cartService;
            _identityService = identityService;
            _logger = logger;
        }

        [HttpPost("{productId:guid}/AddToCart")]
        public async Task<IActionResult> AddToCartAsync(Guid productId)
        {
            var userId = _identityService.GetUserIdentity();

            await _cartService.AddItemToCartAsync(userId, productId);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var items = await _catalogService.GetCatalogItemsAsync(null);

            return Ok(items);
        }
    }
}
