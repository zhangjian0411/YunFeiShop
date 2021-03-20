using System;
using MediatR;
using ZhangJian.YunFeiShop.Services.Ordering.Domain.AggregatesModel.OrderAggregate;

namespace ZhangJian.YunFeiShop.Services.Ordering.Domain.Events
{
    public class OrderStartedDomainEvent : INotification
    {
        public Guid BuyerId { get; }
        public Order Order { get;}

        public OrderStartedDomainEvent(Guid buyerId, Order order)
        {
            BuyerId = buyerId;
            Order = order;
        }
    }
}