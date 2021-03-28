using System;
using System.Collections.Generic;
using System.Linq;
using ZhangJian.YunFeiShop.BuildingBlocks.SeedWork.Domain;

namespace ZhangJian.YunFeiShop.Services.Carts.Domain.AggregatesModel.CartAggregate
{
    public class Cart : Entity, IAggregateRoot
    {
        public Guid BuyerId { get; private set; }

        private readonly List<CartItem> _items;
        public IReadOnlyCollection<CartItem> Items => _items;

        public Cart(Guid buyerId)
        {
            _items = new List<CartItem>();

            BuyerId = buyerId;
        }

        public void AddItem(Guid productId)
        {
            var item = _items.SingleOrDefault(i => i.ProductId == productId);

            if (item != null)
            {
                item.Quantity += 1;
            }
            else
            {
                item = new CartItem(productId, 1, true);
                _items.Add(item);
            }
        }

        public void RemoveItems(Guid[] productIds)
        {
            _items.RemoveAll(i => productIds.Contains(i.ProductId));
        }
    }
}