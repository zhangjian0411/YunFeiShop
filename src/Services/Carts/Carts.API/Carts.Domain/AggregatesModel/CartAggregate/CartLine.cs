using System;
using ZhangJian.YunFeiShop.BuildingBlocks.SeedWork.Domain;

namespace ZhangJian.YunFeiShop.Services.Carts.Domain.AggregatesModel.CartAggregate
{
    public class CartLine : Entity
    {
        public Guid ProductId { get; init; }
        public int Quantity { get; init; }
        public bool Selected { get; init; }
    }
}