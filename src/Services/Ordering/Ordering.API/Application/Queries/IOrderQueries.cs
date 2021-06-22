using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZhangJian.YunFeiShop.Services.Ordering.Domain.AggregatesModel.OrderAggregate;

namespace ZhangJian.YunFeiShop.Services.Ordering.API.Application.Queries
{
    public interface IOrderQueries
    {
        Task<IEnumerable<Order>> GetOrdersAsync(Guid buyerId);
    }
}