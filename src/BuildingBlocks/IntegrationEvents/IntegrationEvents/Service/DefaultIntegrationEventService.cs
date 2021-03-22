using System;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ZhangJian.YunFeiShop.BuildingBlocks.IntegrationEvents.Abstractions;
using ZhangJian.YunFeiShop.BuildingBlocks.IntegrationEvents.EventBus;
using ZhangJian.YunFeiShop.BuildingBlocks.IntegrationEvents.Persistence;

namespace ZhangJian.YunFeiShop.BuildingBlocks.IntegrationEvents.Services
{
    public class DefaultIntegrationEventService<TDbContext> : IIntegrationEventService, IHasDataContext where TDbContext : DbContext
    {
        private readonly IEventBus _eventBus;
        private readonly TDbContext _dbContext;
        private readonly Func<DbConnection, IIntegrationEventPersistenceService> _eventPersistenceServiceFactory;
        private readonly IIntegrationEventPersistenceService _eventPersistenceService;
        private readonly ILogger<DefaultIntegrationEventService<TDbContext>> _logger;

        public DefaultIntegrationEventService(IEventBus eventBus, TDbContext dbContext, Func<DbConnection, IIntegrationEventPersistenceService> eventPersistenceServiceFactory, ILogger<DefaultIntegrationEventService<TDbContext>> logger)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _eventPersistenceServiceFactory = eventPersistenceServiceFactory ?? throw new ArgumentNullException(nameof(eventPersistenceServiceFactory));
            _eventPersistenceService = _eventPersistenceServiceFactory(_dbContext.Database.GetDbConnection());
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        DbContext IHasDataContext.DataContext => _dbContext;

        public async Task AddAndSaveEventAsync(IntegrationEvent evt)
        {
            _logger.LogInformation("----- Enqueuing integration event {IntegrationEventId} to repository ({@IntegrationEvent})", evt.Id, evt);

            await _eventPersistenceService.SaveEventAsync(evt, _dbContext.Database.CurrentTransaction);
        }

        public async Task PublishEventsAsync(Guid transactionId)
        {
            var pendingLogEvents = await _eventPersistenceService.RetrieveEventEntriesPendingToPublishAsync(transactionId);

            foreach (var logEvt in pendingLogEvents)
            {
                _logger.LogInformation("----- Publishing integration event: {IntegrationEventId} - ({@IntegrationEvent})", logEvt.EventId, logEvt.IntegrationEvent);

                try
                {
                    await _eventPersistenceService.MarkEventAsInProgressAsync(logEvt.EventId);
                    _eventBus.Publish(logEvt.IntegrationEvent);
                    await _eventPersistenceService.MarkEventAsPublishedAsync(logEvt.EventId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "ERROR publishing integration event: {IntegrationEventId}", logEvt.EventId);

                    await _eventPersistenceService.MarkEventAsFailedAsync(logEvt.EventId);
                }
            }
        }
    }
}