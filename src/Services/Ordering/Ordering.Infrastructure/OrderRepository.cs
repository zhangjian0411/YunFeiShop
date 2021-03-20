using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZhangJian.YunFeiShop.Services.Ordering.Domain.AggregatesModel.OrderAggregate;
using ZhangJian.YunFeiShop.Services.Ordering.Domain.SeedWork;

namespace ZhangJian.YunFeiShop.Services.Ordering.Infrastructure
{
     public class OrderRepository
        : IOrderRepository
    {
        private readonly OrderingContext _context;

        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _context;
            }
        }

        public OrderRepository(OrderingContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Order Add(Order order)
        {
            return _context.Orders.Add(order).Entity;

        }

        public async Task<Order> GetAsync(Guid buyerId)
        {
            return await _context.Orders.FirstOrDefaultAsync(o => o.BuyerId == buyerId);
        }
    }
}