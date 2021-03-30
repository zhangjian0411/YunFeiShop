using System;

namespace ZhangJian.YunFeiShop.Services.Carts.Application.Exceptions
{
    public class CartNotFoundException : ApplicationException
    {
        public Guid BuyerId { get; private set; }

        public CartNotFoundException(Guid buyerId)
            : base($"Cart is not found.") 
            {
                BuyerId = buyerId;
            }
    }
}