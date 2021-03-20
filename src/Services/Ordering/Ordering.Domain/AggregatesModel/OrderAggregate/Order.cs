using System;
using System.Collections.Generic;
using System.Linq;
using ZhangJian.YunFeiShop.Services.Ordering.Domain.Events;
using ZhangJian.YunFeiShop.Services.Ordering.Domain.SeedWork;

namespace ZhangJian.YunFeiShop.Services.Ordering.Domain.AggregatesModel.OrderAggregate
{
    public class Order : Entity, IAggregateRoot
    {
        public Guid BuyerId { get; private set; }

        private readonly List<OrderLine> _orderLines;
        public IReadOnlyCollection<OrderLine> OrderLines => _orderLines;

        protected Order()
        {
            _orderLines = new List<OrderLine>();
        }

        public Order(Guid buyerId, IEnumerable<OrderLine> orderLines) : this()
        {
            BuyerId = buyerId;
            _orderLines.AddRange(orderLines);

            AddOrderStartedDomainEvent(buyerId);
        }

        private void AddOrderStartedDomainEvent(Guid userId)
        {
            var orderStartedDomainEvent = new OrderStartedDomainEvent(userId, this);

            this.AddDomainEvent(orderStartedDomainEvent);
        }
    }
}