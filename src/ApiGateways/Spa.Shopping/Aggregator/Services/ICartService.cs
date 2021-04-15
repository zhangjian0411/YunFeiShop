using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ZhangJian.YunFeiShop.BFF.Spa.Shopping.HttpAggregator.Models;

namespace ZhangJian.YunFeiShop.BFF.Spa.Shopping.HttpAggregator.Services
{
    public interface ICartService
    {
        Task AddItemToCartAsync(Guid buyerId, Guid productId);
        Task UpdateCartLineAsync(Guid buyerId, CartLine line);
        Task RemoveCartLineAsync(Guid buyerId, Guid productId);
        Task CheckOutAsync(Guid buyerId);
        Task<Cart> GetCartAsync(Guid buyerId);

        // Task UpdateOrCreateCartLineAsync(UpdateOrCreateCartLineSvcRequest request);
    }

    // public class UpdateOrCreateCartLineSvcRequest
    // {
    //     public Guid BuyerId { get; init; }
    //     public Guid ProductId { get; init; }
    //     public int Quantity { get; init; }
    //     public bool Selected { get; init; }
    // }
}