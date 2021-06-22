using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ZhangJian.YunFeiShop.BFF.SPA.Shopping.Config;
using ZhangJian.YunFeiShop.BFF.SPA.Shopping.Models;

namespace ZhangJian.YunFeiShop.BFF.SPA.Shopping.Services.Implements
{
    public class CartHttpClient : ICartService
    {
        private readonly HttpClient _httpClient;

        public CartHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task AddItemToCartAsync(string buyerId, Guid productId)
        {
            var url = ApiUrls.Cart.AddItemToCart();

            var data = new {
                BuyerId = buyerId,
                ProductId = productId
            };

            await _httpClient.PostWithIdAsync(url, data);
        }

        public async Task UpdateOrCreateCartLineAsync(string buyerId, CartLine line)
        {
            var url = ApiUrls.Cart.UpdateOrCreateCartLine();

            var data = new {
                BuyerId = buyerId,
                ProductId = line.ProductId,
                Quantity = line.Quantity,
                Selected = line.Selected
            };

            await _httpClient.PutWithIdAsync(url, data);
        }

        public async Task RemoveCartLinesAsync(string buyerId, Guid[] productIds)
        {
            var url = ApiUrls.Cart.RemoveCartLines();

            var data = new {
                BuyerId = buyerId,
                ProductIds = productIds
            };

            await _httpClient.PutWithIdAsync(url, data);
        }


        public async Task<Cart> GetCartAsync(string buyerId)
        {
            var url = ApiUrls.Cart.GetCart(buyerId);
            
            return await _httpClient.GetFromJsonAsync<Cart>(url);
        }
    }
}