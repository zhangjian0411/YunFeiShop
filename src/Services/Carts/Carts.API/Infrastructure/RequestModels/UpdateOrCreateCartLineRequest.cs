using System;

namespace ZhangJian.YunFeiShop.Services.Carts.API.Infrastructure.RequestModels
{
    public class UpdateOrCreateCartLineRequest
    {
        public Guid ProductId { get; init; }
        public int Quantity { get; init; }
        public bool Selected { get; init; }
    }
}