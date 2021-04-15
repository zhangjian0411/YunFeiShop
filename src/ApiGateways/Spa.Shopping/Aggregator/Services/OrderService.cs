using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using ZhangJian.YunFeiShop.BFF.Spa.Shopping.HttpAggregator.Config;
using ZhangJian.YunFeiShop.BFF.Spa.Shopping.HttpAggregator.Models;

namespace ZhangJian.YunFeiShop.BFF.Spa.Shopping.HttpAggregator.Services
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _httpClient;
        private readonly UrlsConfig _urls;

        public OrderService(HttpClient httpClient, IOptions<UrlsConfig> config)
        {
            _httpClient = httpClient;
            _urls = config.Value;
        }

        public async Task PlaceOrderAsync(Guid buyerId, OrderLine[] lines)
        {
            var url = _urls.CartOperations.AddItemToCart();

            var data = new {
                UserId = buyerId,
                OrderLines = lines
            };

            await _httpClient.PostWithIdAsync(url, data);
        }
    }
}