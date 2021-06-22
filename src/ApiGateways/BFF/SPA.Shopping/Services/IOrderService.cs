using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZhangJian.YunFeiShop.BFF.SPA.Shopping.Models;

namespace ZhangJian.YunFeiShop.BFF.SPA.Shopping.Services
{
    public interface IOrderService
    {
        Task PlaceOrderAsync(string buyerId, OrderLine[] lines);

        Task<IEnumerable<Order>> GetOrdersAsync(string buyerId);
    }
}