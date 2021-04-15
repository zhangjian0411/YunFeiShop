using System;
using System.Collections.Generic;
using System.Linq;
using ZhangJian.YunFeiShop.BuildingBlocks.SeedWork.Domain;
using ZhangJian.YunFeiShop.Services.Ordering.Domain.Events;

namespace ZhangJian.YunFeiShop.Services.Ordering.Domain.AggregatesModel.OrderAggregate
{
    public class Order : Entity, IAggregateRoot
    {
        public Guid BuyerId { get; private set; }

        private readonly List<OrderLine> _orderLines;
        public IReadOnlyCollection<OrderLine> OrderLines => _orderLines;

        public OrderStatus OrderStatus { get; private set; }

        private Order()
        {
            _orderLines = new List<OrderLine>();
        }

        public static Order Create(Guid buyerId, IEnumerable<OrderLine> orderLines)
        {
            var order = new Order();
            order.BuyerId = buyerId;
            order._orderLines.AddRange(orderLines);
            order.OrderStatus = OrderStatus.Submitted;

            order.AddOrderStartedDomainEvent(buyerId, order);

            return order;
        }

        private void AddOrderStartedDomainEvent(Guid buyerId, Order order)
        {
            var orderStartedDomainEvent = new OrderStartedDomainEvent(buyerId, order);

            this.AddDomainEvent(orderStartedDomainEvent);
        }
    }
}