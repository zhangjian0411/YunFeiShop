using MediatR;

namespace ZhangJian.YunFeiShop.Services.Carts.Domain.Events
{
    public class CartCreatedDomainEvent : INotification
    {
        public string CartId { get; init; }
    }
}