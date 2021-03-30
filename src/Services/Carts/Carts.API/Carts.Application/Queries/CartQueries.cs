using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZhangJian.YunFeiShop.Services.Carts.Domain.AggregatesModel.CartAggregate;
using ZhangJian.YunFeiShop.Services.Carts.Infrastructure;

namespace ZhangJian.YunFeiShop.Services.Carts.Application.Queries
{
    public class CartQueries : ICartQueries
    {
        private readonly CartContext _dbContext;
        public CartQueries(CartContext dbContext) => _dbContext = dbContext;

        public Task<Cart> GetCartAsync(Guid buyerId)
        {
            return _dbContext.Carts.Include(c => c.Items).AsNoTracking().SingleOrDefaultAsync(c => c.BuyerId == buyerId);
        }
    }
}