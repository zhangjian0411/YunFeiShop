using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZhangJian.YunFeiShop.BFF.Spa.Shopping.HttpAggregator.Models;
using ZhangJian.YunFeiShop.BFF.Spa.Shopping.HttpAggregator.Services;

namespace ZhangJian.YunFeiShop.BFF.Spa.Shopping.HttpAggregator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ICatalogService _catalogService;
        private readonly IIdentityService _identityService;
        public CartController(ICartService cartService, ICatalogService catalogService, IIdentityService identityService)
        {
            _cartService = cartService;
            _catalogService = catalogService;
            _identityService = identityService;
        }

        [HttpPut("lines")]
        public async Task<ActionResult<UpdateCartLineResponse>> UpdateCartLine(UpdateCartLineRequest request)
        {
            var userId = _identityService.GetUserIdentity();
            var cartLine = new CartLine(request.ProductId, request.Quantity, request.Selected);

            await _cartService.UpdateCartLineAsync(userId,cartLine);

            var totalPrice = await GetTotalPriceOfCart(userId);

            return new UpdateCartLineResponse(totalPrice);
        }

        [HttpDelete("lines/{productId:guid}")]
        public async Task<ActionResult<RemoveCartLineResponse>> RemoveCartLine(Guid productId)
        {
            var userId = _identityService.GetUserIdentity();

            await _cartService.RemoveCartLineAsync(userId, productId);

            var totalPrice = await GetTotalPriceOfCart(userId);

            return new RemoveCartLineResponse(totalPrice);
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> CheckOutAsync()
        {
            var userId = _identityService.GetUserIdentity();

            await _cartService.CheckOutAsync(userId);

            // Get the order info.
            throw new NotImplementedException();
        }

        private async Task<decimal> GetTotalPriceOfCart(Guid buyerId)
        {
            var cart = await _cartService.GetCartAsync(buyerId);

            var selectedLines = cart.Lines.Where(line => line.Selected);

            var catalogItems = await _catalogService.GetCatalogItemsAsync(selectedLines.Select(l => l.ProductId).ToArray());

            var totalPrice = selectedLines
                .Select(line => new 
                    {
                        Quantity = line.Quantity,
                        UnitPrice = catalogItems.Single(c => c.Id == line.ProductId).UnitPrice
                    })
                .Sum(a => a.Quantity * a.UnitPrice);

            return totalPrice;
        }

    }

    
}