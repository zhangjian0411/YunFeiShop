using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZhangJian.YunFeiShop.BFF.Spa.Shopping.HttpAggregator.Models;
using ZhangJian.YunFeiShop.BFF.Spa.Shopping.HttpAggregator.Services;
using static ZhangJian.YunFeiShop.BFF.Spa.Shopping.HttpAggregator.Controllers.DraftOrder;

namespace ZhangJian.YunFeiShop.BFF.Spa.Shopping.HttpAggregator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;
        private readonly ICatalogService _catalogService;
        private readonly IIdentityService _identityService;

        public OrdersController(IOrderService orderService, ICartService cartService, ICatalogService catalogService, IIdentityService identityService)
        {
            _orderService = orderService;
            _cartService = cartService;
            _catalogService = catalogService;
            _identityService = identityService;
        }

        [HttpPost("PlaceOrder")]
        public async Task<IActionResult> PlaceOrderAsync()
        {
            var userId = _identityService.GetUserIdentity();

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

        [HttpGet("DraftOrder")]
        public async Task<IActionResult> GetDraftOrderAsync()
        {
            var userId = _identityService.GetUserIdentity();

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
    }
}