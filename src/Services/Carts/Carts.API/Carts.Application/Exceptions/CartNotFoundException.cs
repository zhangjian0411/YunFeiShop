using System;

namespace ZhangJian.YunFeiShop.Services.Carts.Application.Exceptions
{
    public class CartNotFoundException : ApplicationException
    {
        public CartNotFoundException(Guid buyerId)
            : base($"Cart - {{BuyerId: {buyerId}}} is not found.") { }
    }
}