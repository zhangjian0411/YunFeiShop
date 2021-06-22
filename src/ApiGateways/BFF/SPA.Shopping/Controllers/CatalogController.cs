using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ZhangJian.YunFeiShop.BFF.SPA.Shopping.Common;
using ZhangJian.YunFeiShop.BFF.SPA.Shopping.Services;

namespace ZhangJian.YunFeiShop.BFF.SPA.Shopping.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CatalogController : ApplicationController
    {
        private readonly ICatalogService _catalogService;
        private readonly ICartService _cartService;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(ICatalogService catalogService, ICartService cartService, ILogger<CatalogController> logger)
        {
            _catalogService = catalogService;
            _cartService = cartService;
            _logger = logger;
        }

        [HttpPost("{productId:guid}/AddToCart")]
        public async Task<IActionResult> AddToCartAsync(Guid productId)
        {
            await _cartService.AddItemToCartAsync(User.Id, productId);

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
