using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZhangJian.YunFeiShop.Services.Ordering.Domain.AggregatesModel.OrderAggregate;
using ZhangJian.YunFeiShop.Services.Ordering.Infrastructure;

namespace ZhangJian.YunFeiShop.Services.Ordering.API.Application.Queries
{
    public class OrderQueries : IOrderQueries
    {
        private readonly OrderingContext _dbContext;
        public OrderQueries(OrderingContext dbContext) => _dbContext = dbContext;

        public async Task<IEnumerable<Order>> GetOrdersAsync(Guid buyerId)
        {
            return await _dbContext.Orders.Include(o => o.OrderLines).AsNoTracking()
                            .Where(o => o.BuyerId == buyerId)
                            .ToArrayAsync();
        }
    }
}