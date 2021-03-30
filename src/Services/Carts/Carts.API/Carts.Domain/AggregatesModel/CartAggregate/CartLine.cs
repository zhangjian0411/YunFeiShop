using System;
using ZhangJian.YunFeiShop.BuildingBlocks.SeedWork.Domain;

namespace ZhangJian.YunFeiShop.Services.Carts.Domain.AggregatesModel.CartAggregate
{
    public class CartLine : Entity
    {
        public Guid ProductId { get; private set; }
        public int Quantity { get; set; }
        public bool Selected { get; set; }

        public CartLine(Guid productId, int quantity, bool selected)
        {
            ProductId = productId;
            Quantity = quantity;
            Selected = selected;
        }
    }
}