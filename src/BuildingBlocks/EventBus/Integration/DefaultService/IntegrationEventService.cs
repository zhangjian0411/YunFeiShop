using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ZhangJian.YunFeiShop.BuildingBlocks.EventBus.Abstractions;
using ZhangJian.YunFeiShop.BuildingBlocks.EventBus.Events;
using ZhangJian.YunFeiShop.BuildingBlocks.Integration.IntegrationEventLog;

namespace ZhangJian.YunFeiShop.BuildingBlocks.Integration.DefaultService
{
    public class IntegrationEventService<TDbContext> : IIntegrationEventService, IHasDataContext where TDbContext : DbContext
    {
        private readonly IEventBus _eventBus;
        private readonly TDbContext _dbContext;
        private readonly IntegrationEventLogService _eventLogService;
        private readonly ILogger<IntegrationEventService<TDbContext>> _logger;
        private readonly string _appName;

        public IntegrationEventService(IEventBus eventBus, TDbContext dbContext, IntegrationEventLogService eventLogService, ILogger<IntegrationEventService<TDbContext>> logger)
        {
            _eventBus = eventBus;
            _dbContext = dbContext;
            _eventLogService = eventLogService;
            _logger = logger;
            _appName = Assembly.GetEntryAssembly().FullName;
        }

        DbContext IHasDataContext.DataContext => _dbContext;

        public async Task PublishEventsThroughEventBusAsync(Guid transactionId)
        {
            var pendingLogEvents = await _eventLogService.RetrieveEventLogsPendingToPublishAsync(transactionId);

            foreach (var logEvt in pendingLogEvents)
            {
                _logger.LogInformation("----- Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", logEvt.EventId, _appName, logEvt.IntegrationEvent);

                try
                {
                    await _eventLogService.MarkEventAsInProgressAsync(logEvt.EventId);
                    _eventBus.Publish(logEvt.IntegrationEvent);
                    await _eventLogService.MarkEventAsPublishedAsync(logEvt.EventId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "ERROR publishing integration event: {IntegrationEventId} from {AppName}", logEvt.EventId, _appName);

                    await _eventLogService.MarkEventAsFailedAsync(logEvt.EventId);
                }
            }
        }

        public async Task AddAndSaveEventAsync(IntegrationEvent evt)
        {
            _logger.LogInformation("----- Enqueuing integration event {IntegrationEventId} to repository ({@IntegrationEvent})", evt.Id, evt);

            await _eventLogService.SaveEventAsync(evt, _dbContext.Database.CurrentTransaction);
        }
    }
}
