using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZhangJian.YunFeiShop.BFF.SPA.Shopping.Common;
using ZhangJian.YunFeiShop.BFF.SPA.Shopping.Models;
using ZhangJian.YunFeiShop.BFF.SPA.Shopping.Services;

namespace ZhangJian.YunFeiShop.BFF.SPA.Shopping.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : ApplicationController
    {
        private readonly ICartService _cartService;
        private readonly ICatalogService _catalogService;
        public CartController(ICartService cartService, ICatalogService catalogService)
        {
            _cartService = cartService;
            _catalogService = catalogService;
        }

        [HttpPut("lines")]
        public async Task<ActionResult<UpdateOrCreateCartLineResponse>> UpdateOrCreateCartLine(UpdateOrCreateCartLineRequest request)
        {
            var userId = User.Id;
            var cartLine = new CartLine(request.ProductId, request.Quantity, request.Selected);

            await _cartService.UpdateOrCreateCartLineAsync(userId,cartLine);

            var totalPrice = await GetTotalPriceOfCart(userId);

            return new UpdateOrCreateCartLineResponse(totalPrice);
        }

        [HttpDelete("lines/{productId:guid}")]
        public async Task<ActionResult<RemoveCartLineResponse>> RemoveCartLine(Guid productId)
        {
            var userId = User.Id;

            await _cartService.RemoveCartLinesAsync(userId, new Guid[]{ productId });

            var totalPrice = await GetTotalPriceOfCart(userId);

            return new RemoveCartLineResponse(totalPrice);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserCartAsync()
        {
            var userId = User.Id;

            var cart = await _cartService.GetCartAsync(userId);
            var catalogItems = await _catalogService.GetCatalogItemsAsync(cart.Lines.Select(l => l.ProductId).ToArray());

            var lines = cart.Lines
                            .Join(catalogItems,
                                line => line.ProductId,
                                catalogItem => catalogItem.Id,
                                (line, catalogItem) => new { ProductId = line.ProductId, Quantity = line.Quantity, UnitPrice = catalogItem.UnitPrice, Selected = line.Selected })
                            .ToArray();

            return Ok(new { BuyerId = cart.BuyerId, Lines = lines });
        }

        private async Task<decimal> GetTotalPriceOfCart(string buyerId)
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

        # region Request & Response Model

        public record UpdateOrCreateCartLineRequest(Guid ProductId, int Quantity, bool Selected);
        public record UpdateOrCreateCartLineResponse(decimal TotalPrice);

        public record RemoveCartLineResponse(decimal TotalPrice);

        # endregion
    }

    
}