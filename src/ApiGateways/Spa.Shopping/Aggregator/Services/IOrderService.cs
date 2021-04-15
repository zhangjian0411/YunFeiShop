using System;
using System.Threading.Tasks;
using ZhangJian.YunFeiShop.BFF.Spa.Shopping.HttpAggregator.Models;

namespace ZhangJian.YunFeiShop.BFF.Spa.Shopping.HttpAggregator.Services
{
    public interface IOrderService
    {
        Task PlaceOrderAsync(Guid buyerId, OrderLine[] lines);
    }
}