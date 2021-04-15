using System;
using ZhangJian.YunFeiShop.BuildingBlocks.SeedWork.Domain;

namespace ZhangJian.YunFeiShop.Services.Ordering.Domain.AggregatesModel.OrderAggregate
{
    public class OrderLine : Entity
    {
        public Guid ProductId { get; init; }
        public string Name { get; init; }
        public int Quantity { get; init; }

        public OrderLine(Guid productId, string name, int quantity)
        {
            ProductId = productId;
            Name = name;
            Quantity = quantity;
        }
    }
}