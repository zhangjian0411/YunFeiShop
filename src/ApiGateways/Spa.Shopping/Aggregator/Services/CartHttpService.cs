using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using ZhangJian.YunFeiShop.BFF.Spa.Shopping.HttpAggregator.Config;
using ZhangJian.YunFeiShop.BFF.Spa.Shopping.HttpAggregator.Models;

namespace ZhangJian.YunFeiShop.BFF.Spa.Shopping.HttpAggregator.Services
{
    public class CartHttpService : ICartService
    {
        private readonly HttpClient _httpClient;
        private readonly UrlsConfig _urls;

        public CartHttpService(HttpClient httpClient, IOptions<UrlsConfig> config)
        {
            _httpClient = httpClient;
            _urls = config.Value;
        }

        public async Task AddItemToCartAsync(Guid buyerId, Guid productId)
        {
            var url = _urls.CartOperations.AddItemToCart();

            var data = new {
                BuyerId = buyerId,
                ProductId = productId
            };

            await _httpClient.PostWithIdAsync(url, data);
        }

        public async Task UpdateCartLineAsync(Guid buyerId, CartLine line)
        {
            var url = _urls.CartOperations.UpdateOrCreateCartLine();

            var data = new {
                BuyerId = buyerId,
                ProductId = line.ProductId,
                Quantity = line.Quantity,
                Selected = line.Selected
            };

            await _httpClient.PutWithIdAsync(url, data);
        }

        public async Task RemoveCartLineAsync(Guid buyerId, Guid productId)
        {
            var url = _urls.CartOperations.RemoveCartLines();

            var data = new {
                BuyerId = buyerId,
                ProductIds = new [] { productId }
            };

            await _httpClient.PutWithIdAsync(url, data);
        }

        public async Task CheckOutAsync(Guid buyerId)
        {
            var url = _urls.CartOperations.CheckOut();

            var data = new {
                BuyerId = buyerId
            };

            await _httpClient.PostWithIdAsync(url, data);
        }

        public async Task<Cart> GetCartAsync(Guid buyerId)
        {
            var url = _urls.CartOperations.GetCart(buyerId);
            
            return await _httpClient.GetFromJsonAsync<Cart>(url);
        }
    }
}