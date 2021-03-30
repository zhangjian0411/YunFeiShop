using System;
using System.Collections.Generic;
using MediatR;

namespace ZhangJian.YunFeiShop.Services.Ordering.API.Application.Commands
{
    public class CreateOrderCommand : IRequest<bool>
    {
        public Guid UserId { get; }
        public OrderLine[] OrderLines { get; }

        public CreateOrderCommand(Guid userId, OrderLine[] orderLines)
        {
            UserId = userId;
            OrderLines = orderLines;
        }

        public class OrderLine
        {
            public Guid ProductId { get; }
            public string Name { get; }
            public int Quantity { get; }

            public OrderLine(Guid productId, string name, int quantity)
            {
                ProductId = productId;
                Name = name;
                Quantity = quantity;
            }

        }
    }
}