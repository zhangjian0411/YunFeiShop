using System;
using ZhangJian.YunFeiShop.Services.Ordering.Domain.SeedWork;

namespace ZhangJian.YunFeiShop.Services.Ordering.Domain.AggregatesModel.OrderAggregate
{
    public class OrderLine : Entity
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}