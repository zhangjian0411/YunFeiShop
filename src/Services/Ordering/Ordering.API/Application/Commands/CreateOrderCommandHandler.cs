using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ZhangJian.YunFeiShop.Services.Ordering.Domain.AggregatesModel.OrderAggregate;

namespace ZhangJian.YunFeiShop.Services.Ordering.API.Application.Commands
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;

        public CreateOrderCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<bool> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderLines = request.OrderLines.Select(o => new OrderLine { ProductId = o.ProductId, Name = o.Name, Quantity = o.Quantity });
            var order = new Order(request.UserId, orderLines);
            _orderRepository.Add(order);

            return await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}