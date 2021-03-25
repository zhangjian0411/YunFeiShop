using System;
using ZhangJian.YunFeiShop.BuildingBlocks.IntegrationEvents;

namespace ZhangJian.YunFeiShop.Services.Carts.Application.IntegrationEvents.Events
{
    public class OrderStartedIntegrationEvent : IntegrationEvent
    {
        public Guid UserId { get; init; }
    }
}