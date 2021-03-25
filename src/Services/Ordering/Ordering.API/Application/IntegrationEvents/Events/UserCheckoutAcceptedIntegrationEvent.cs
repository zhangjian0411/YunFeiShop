using System;
using ZhangJian.YunFeiShop.BuildingBlocks.IntegrationEvents;

namespace ZhangJian.YunFeiShop.Services.Ordering.API.Application.IntegrationEvents.Events
{
    public class UserCheckoutAcceptedIntegrationEvent : IntegrationEvent
    {
        public Guid UserId { get; init; }
        public CheckoutLine[] CheckoutLines { get; init; }

        public class CheckoutLine
        {
            public Guid ProductId { get; init; }
            public string ProductName { get; init; }
            public int Quantity { get; init; }
        }
    }
}