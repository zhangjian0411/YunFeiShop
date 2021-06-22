using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ZhangJian.YunFeiShop.BFF.SPA.Shopping.Models;

namespace ZhangJian.YunFeiShop.BFF.SPA.Shopping.Services
{
    public interface ICartService
    {
        Task AddItemToCartAsync(string buyerId, Guid productId);
        Task UpdateOrCreateCartLineAsync(string buyerId, CartLine line);
        Task RemoveCartLinesAsync(string buyerId, Guid[] productIds);

        Task<Cart> GetCartAsync(string buyerId);
    }
}