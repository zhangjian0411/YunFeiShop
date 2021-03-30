using System;
using System.Threading.Tasks;
using ZhangJian.YunFeiShop.Services.Carts.Domain.AggregatesModel.CartAggregate;

namespace ZhangJian.YunFeiShop.Services.Carts.Application.Queries
{
    public interface ICartQueries
    {
        Task<Cart> GetCartAsync(Guid buyerId);
    }
}