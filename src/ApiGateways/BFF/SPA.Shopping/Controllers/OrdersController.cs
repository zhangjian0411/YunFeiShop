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
    public class OrdersController : ApplicationController
    {
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;
        private readonly ICatalogService _catalogService;

        public OrdersController(IOrderService orderService, ICartService cartService, ICatalogService catalogService)
        {
            _orderService = orderService;
            _cartService = cartService;
            _catalogService = catalogService;
        }

        [HttpPost("PlaceOrder")]
        public async Task<IActionResult> PlaceOrderAsync()
        {
            var userId = User.Id;

            var cart = await _cartService.GetCartAsync(userId);
            var selectedCartLines = cart.Lines.Where(l => l.Selected);

            var catalogItems = await _catalogService.GetCatalogItemsAsync(selectedCartLines.Select(l => l.ProductId).ToArray());

            var orderLines = selectedCartLines
                                    .Join(catalogItems,
                                        cartLine => cartLine.ProductId,
                                        catalogItem => catalogItem.Id,
                                        (cartLine, catalogItem) => new OrderLine(cartLine.ProductId, catalogItem.Name, cartLine.Quantity))
                                    .ToArray();

            await _orderService.PlaceOrderAsync(userId, orderLines);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetOrdersAsync()
        {
            var orders = await _orderService.GetOrdersAsync(User.Id);

            return Ok(orders);
        }

        [HttpGet("DraftOrder")]
        public async Task<IActionResult> GetDraftOrderAsync()
        {
            var userId = User.Id;

            var cart = await _cartService.GetCartAsync(userId);
            var selectedCartLines = cart.Lines.Where(l => l.Selected);

            var catalogItems = await _catalogService.GetCatalogItemsAsync(selectedCartLines.Select(l => l.ProductId).ToArray());

            var draftOrderLines = selectedCartLines
                                    .Join(catalogItems,
                                        cartLine => cartLine.ProductId,
                                        catalogItem => catalogItem.Id,
                                        (cartLine, catalogItem) => new DraftOrderLine(cartLine.ProductId, catalogItem.Name, cartLine.Quantity, catalogItem.UnitPrice))
                                    .ToArray();

            var draftOrder = new DraftOrder(userId, draftOrderLines);

            return Ok(draftOrder);
        }


        #region Request & Response Model

        public record DraftOrder(string BuyerId, DraftOrderLine[] Lines);
        public record DraftOrderLine(Guid ProductId, string Name, int Quantity, decimal UnitPrice);

        # endregion
    }
}