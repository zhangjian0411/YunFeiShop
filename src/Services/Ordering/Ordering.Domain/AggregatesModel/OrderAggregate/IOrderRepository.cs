using System;
using System.Threading.Tasks;
using ZhangJian.YunFeiShop.Services.Ordering.Domain.SeedWork;

namespace ZhangJian.YunFeiShop.Services.Ordering.Domain.AggregatesModel.OrderAggregate
{
    public interface IOrderRepository: IRepository<Order>
    {
        Order Add(Order item);
        Task<Order> GetAsync(Guid buyerId);
    }
}