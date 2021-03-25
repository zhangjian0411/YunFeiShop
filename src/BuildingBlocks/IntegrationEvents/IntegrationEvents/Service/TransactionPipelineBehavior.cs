using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using ZhangJian.YunFeiShop.BuildingBlocks.IntegrationEvents.Abstractions;

namespace ZhangJian.YunFeiShop.BuildingBlocks.IntegrationEvents.Services
{
    public class TransactionPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<TransactionPipelineBehavior<TRequest, TResponse>> _logger;
        private readonly DbContext _dbContext;
        private readonly IIntegrationEventService _integrationEventService;

        public TransactionPipelineBehavior(
            IIntegrationEventService integrationEventService,
            ILogger<TransactionPipelineBehavior<TRequest, TResponse>> logger)
        {
            _integrationEventService = integrationEventService ?? throw new ArgumentNullException(nameof(integrationEventService));
            _dbContext = (_integrationEventService as IHasDbContext)?.DbContext ?? throw new ArgumentException("The implementation of IIntegrationEventService also need implement interface IHasDbContext.", nameof(integrationEventService));
            _logger = logger ?? throw new ArgumentNullException(nameof(ILogger));
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var response = default(TResponse);
            var typeName = request.GetGenericTypeName();

            try
            {
                if (_dbContext.Database.CurrentTransaction != null)
                {
                    return await next();
                }

                var strategy = _dbContext.Database.CreateExecutionStrategy();

                await strategy.ExecuteAsync(async () =>
                {
                    Guid transactionId;

                    using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                    {
                        _logger.LogInformation("----- Begin transaction {TransactionId} for {CommandName} ({@Command})", transaction.TransactionId, typeName, request);

                        response = await next();

                        _logger.LogInformation("----- Commit transaction {TransactionId} for {CommandName}", transaction.TransactionId, typeName);

                        await _dbContext.SaveChangesAsync();
                        transaction.Commit();

                        transactionId = transaction.TransactionId;
                    }

                    await _integrationEventService.PublishEventsAsync(transactionId);
                });

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR Handling transaction for {CommandName} ({@Command})", typeName, request);

                throw;
            }
        }
    }
}
