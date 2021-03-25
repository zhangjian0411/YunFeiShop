using System;
using System.Collections.Generic;
using ZhangJian.YunFeiShop.BuildingBlocks.SeedWork.Domain;
using ZhangJian.YunFeiShop.Services.Carts.Domain.Events;

namespace ZhangJian.YunFeiShop.Services.Carts.Domain.AggregatesModel.CartAggregate
{
    public class Cart : Entity, IAggregateRoot
    {
        public Guid BuyerId { get; init; }
        public IReadOnlyCollection<CartLine> Lines { get; init; }

        public Cart()
        {
            AddCartCreatedDomainEvent();
        }

        private void AddCartCreatedDomainEvent()
        {
            var cartCreatedDomainEvent = new CartCreatedDomainEvent { CartId = "test cart id 1" };

            this.AddDomainEvent(cartCreatedDomainEvent);
        }
    }
}