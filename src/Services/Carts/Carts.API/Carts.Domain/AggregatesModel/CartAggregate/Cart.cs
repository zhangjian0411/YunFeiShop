using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using ZhangJian.YunFeiShop.BuildingBlocks.SeedWork.Domain;

namespace ZhangJian.YunFeiShop.Services.Carts.Domain.AggregatesModel.CartAggregate
{
    public class Cart : Entity, IAggregateRoot
    {
        [JsonIgnore]
        public override Guid Id { get => base.Id; protected set => base.Id = value; }

        public Guid BuyerId { get; private set; }

        private List<CartLine> _lines;
        public IReadOnlyCollection<CartLine> Lines => _lines;

        public Cart(Guid buyerId)
        {
            BuyerId = buyerId;
            _lines = new List<CartLine>();
        }

        public CartLine AddItem(Guid productId)
        {
            var line = _lines.SingleOrDefault(i => i.ProductId == productId);

            if (line != null)
            {
                line.Quantity += 1;
                line.Selected = true;
            }
            else
            {
                line = new CartLine(productId, 1, true);
                _lines.Add(line);
            }

            return line;
        }

        public void RemoveLines(Guid[] productIds)
        {
            _lines.RemoveAll(i => productIds.Contains(i.ProductId));
        }
    }
}