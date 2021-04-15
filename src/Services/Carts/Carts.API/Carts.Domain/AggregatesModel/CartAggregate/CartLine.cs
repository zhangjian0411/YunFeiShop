using System;
using System.Text.Json.Serialization;
using ZhangJian.YunFeiShop.BuildingBlocks.SeedWork.Domain;

namespace ZhangJian.YunFeiShop.Services.Carts.Domain.AggregatesModel.CartAggregate
{
    public class CartLine : Entity
    {
        [JsonIgnore]
        public override Guid Id { get => base.Id; protected set => base.Id = value; } 
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