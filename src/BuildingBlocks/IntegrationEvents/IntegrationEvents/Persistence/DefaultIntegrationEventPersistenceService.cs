using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace ZhangJian.YunFeiShop.BuildingBlocks.IntegrationEvents.Persistence
{
    public class DefaultIntegrationEventPersistenceService : IIntegrationEventPersistenceService
    {
        private readonly IntegrationEventEntryContext _eventEntryContext;
        private readonly List<Type> _eventTypes;

        public DefaultIntegrationEventPersistenceService(IntegrationEventEntryContext eventEntryContext) 
        {
            _eventEntryContext = eventEntryContext;
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

            var result = await _eventEntryContext.IntegrationEventEntries
                .Where(e => e.TransactionId == tid && e.State == EventStateEnum.NotPublished).ToListAsync();

            if (result != null && result.Any())
            {
                return result.OrderBy(o => o.CreationTime)
                    .Select(e => e.DeserializeJsonContent(_eventTypes.Find(t => t.Name == e.EventTypeShortName)));
            }

            return new List<IntegrationEventEntry>();
        }

        public Task SaveEventAsync(IntegrationEvent @event, IDbContextTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            var eventEntry = new IntegrationEventEntry(@event, transaction.TransactionId);

            var noTransactionInUse = _eventEntryContext.Database.CurrentTransaction == null;

            if (noTransactionInUse) _eventEntryContext.Database.UseTransaction(transaction.GetDbTransaction());

            _eventEntryContext.IntegrationEventEntries.Add(eventEntry);

            var result = _eventEntryContext.SaveChangesAsync();

            if (noTransactionInUse) _eventEntryContext.Database.UseTransaction(null);

            return result;
        }

        private Task UpdateEventStatus(Guid eventId, EventStateEnum status)
        {
            var eventEntry = _eventEntryContext.IntegrationEventEntries.Single(ie => ie.EventId == eventId);
            eventEntry.State = status;

            if (status == EventStateEnum.InProgress)
                eventEntry.TimesSent++;

            _eventEntryContext.IntegrationEventEntries.Update(eventEntry);

            return _eventEntryContext.SaveChangesAsync();
        }
    }
}