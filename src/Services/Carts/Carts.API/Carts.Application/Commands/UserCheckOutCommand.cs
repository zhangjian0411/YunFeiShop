using System;
using System.Collections.Generic;
using MediatR;

namespace ZhangJian.YunFeiShop.Services.Carts.Application.Commands
{
    public class UserCheckOutCommand : IRequest<bool>
    {
        public Guid UserId { get; }
        public IEnumerable<OrderLine> OrderLines { get; }

        public UserCheckOutCommand(Guid userId, IEnumerable<OrderLine> orderLines)
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