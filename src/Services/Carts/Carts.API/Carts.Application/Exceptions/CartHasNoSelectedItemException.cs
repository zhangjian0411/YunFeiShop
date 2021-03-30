using System;

namespace ZhangJian.YunFeiShop.Services.Carts.Application.Exceptions
{
    public class CartHasNoSelectedItemException : ApplicationException
    {
        public Guid BuyerId { get; private set; }

        public CartHasNoSelectedItemException(Guid buyerId)
            : base($"No item is selected in cart.") 
            {
                BuyerId = buyerId;
            }
    }
}