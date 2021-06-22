using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ZhangJian.YunFeiShop.BFF.SPA.Shopping.Config;
using ZhangJian.YunFeiShop.BFF.SPA.Shopping.Models;

namespace ZhangJian.YunFeiShop.BFF.SPA.Shopping.Services.Implements
{
    public class OrderHttpClient : IOrderService
    {
        private readonly HttpClient _httpClient;

        public OrderHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task PlaceOrderAsync(string buyerId, OrderLine[] lines)
        {
            var url = ApiUrls.Order.PlaceOrder();

            var data = new {
                UserId = buyerId,
                OrderLines = lines
            };

            await _httpClient.PostWithIdAsync(url, data);
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync(string buyerId)
        {
            var url = ApiUrls.Order.GetOrders(buyerId);

            return await _httpClient.GetFromJsonAsync<IEnumerable<Order>>(url);
        }
    }
}