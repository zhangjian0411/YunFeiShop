using System;
using System.Collections.Generic;
using MediatR;

namespace ZhangJian.YunFeiShop.Services.Ordering.API.Application.Commands
{
    public class PlaceOrderCommand : IRequest<bool>
    {
        public Guid UserId { get; init; }
        public PlaceOrderCommandOrderLine[] OrderLines { get; init; }

        public PlaceOrderCommand(Guid userId, PlaceOrderCommandOrderLine[] orderLines)
        {
            UserId = userId;
            OrderLines = orderLines;
        }

        public class PlaceOrderCommandOrderLine
        {
            public Guid ProductId { get; init; }
            public string Name { get; init; }
            public int Quantity { get; init; }

            public PlaceOrderCommandOrderLine(Guid productId, string name, int quantity)
            {
                ProductId = productId;
                Name = name;
                Quantity = quantity;
            }

        }
    }
}