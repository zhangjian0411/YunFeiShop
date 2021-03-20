using System;
using ZhangJian.YunFeiShop.BuildingBlocks.EventBus.Events;

namespace ZhangJian.YunFeiShop.Services.Ordering.API.Application.IntegrationEvents.Events
{
    public class OrderStartedIntegrationEvent : IntegrationEvent
    {
        public Guid UserId { get; }

        public OrderStartedIntegrationEvent(Guid userId)
        {
            UserId = userId;
        }
    }
}