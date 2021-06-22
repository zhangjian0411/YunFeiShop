using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ZhangJian.YunFeiShop.BuildingBlocks.SeedWork.Application.Commands;
using ZhangJian.YunFeiShop.BuildingBlocks.SeedWork.Infrastructure.Idempotency;
using ZhangJian.YunFeiShop.Services.Ordering.Domain.AggregatesModel.OrderAggregate;

namespace ZhangJian.YunFeiShop.Services.Ordering.API.Application.Commands
{
    public class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;

        public PlaceOrderCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<bool> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
        {
            var orderLines = request.OrderLines.Select(o => new Domain.AggregatesModel.OrderAggregate.OrderLine(o.ProductId, o.Name, o.Quantity));
            var order = Order.Create(request.UserId, orderLines);
            _orderRepository.Add(order);

            var result = await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return result;
        }
    }

    // Use for Idempotency in Command process
    public class PlaceOrderIdentifiedCommandHandler : IdentifiedCommandHandler<PlaceOrderCommand, bool>
    {
        public PlaceOrderIdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager) : base(mediator, requestManager) { }

        protected override bool CreateResultForDuplicateRequest()
        {
            return true;                // Ignore duplicate requests for creating order.
        }
    }
}