using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ZhangJian.YunFeiShop.BuildingBlocks.IntegrationEvents.Persistence
{
    public class DefaultIntegrationEventPersistenceService<TDbContext> : IIntegrationEventPersistenceService where TDbContext : DbContext
    {
        private readonly TDbContext _context;
        private readonly List<Type> _eventTypes;

        public DefaultIntegrationEventPersistenceService(TDbContext context) 
        {
            _context = context;
            _eventTypes = Assembly.Load(Assembly.GetEntryAssembly().FullName)
                .GetTypes()
                .Where(t => t.Name.EndsWith(nameof(IntegrationEvent)))
                .ToList();
        }

        public Task MarkEventAsFailedAsync(Guid eventId)
        {
            return UpdateEventStatus(eventId, EventStateEnum.PublishedFailed);
        }

        public Task MarkEventAsInProgressAsync(Guid eventId)
        {
            return UpdateEventStatus(eventId, EventStateEnum.InProgress);
        }

        public Task MarkEventAsPublishedAsync(Guid eventId)
        {
            return UpdateEventStatus(eventId, EventStateEnum.Published);
        }

        public async Task<IEnumerable<IntegrationEventEntry>> RetrieveEventEntriesPendingToPublishAsync(Guid transactionId)
        {
            var tid = transactionId.ToString();

            var result = await _context.Set<IntegrationEventEntry>()
                .Where(e => e.TransactionId == tid && e.State == EventStateEnum.NotPublished).ToListAsync();

            if (result != null && result.Any())
            {
                return result.OrderBy(o => o.CreationTime)
                    .Select(e => e.DeserializeJsonContent(_eventTypes.Find(t => t.Name == e.EventTypeShortName)));
            }

            return new List<IntegrationEventEntry>();
        }

        public Task SaveEventAsync(IntegrationEvent @event)
        {
            var transaction = _context.Database.CurrentTransaction;

            if (transaction == null) throw new ApplicationException("DbContext's current transaction shouldn't be null. SaveEventAsync need run in a transaction. ");

            var eventEntry = new IntegrationEventEntry(@event, transaction.TransactionId);

            _context.Set<IntegrationEventEntry>().Add(eventEntry);

            var result = _context.SaveChangesAsync();

            return result;
        }

        private Task UpdateEventStatus(Guid eventId, EventStateEnum status)
        {
            var eventEntry = _context.Set<IntegrationEventEntry>().Single(ie => ie.EventId == eventId);
            eventEntry.State = status;

            if (status == EventStateEnum.InProgress)
                eventEntry.TimesSent++;

            _context.Set<IntegrationEventEntry>().Update(eventEntry);

            return _context.SaveChangesAsync();
        }
    }
}