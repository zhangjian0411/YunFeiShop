using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace ZhangJian.YunFeiShop.BuildingBlocks.IntegrationEvents.Persistence
{
    public interface IIntegrationEventPersistenceService
    {
        Task MarkEventAsFailedAsync(Guid eventId);

        Task MarkEventAsInProgressAsync(Guid eventId);

        Task MarkEventAsPublishedAsync(Guid eventId);

        Task<IEnumerable<IntegrationEventEntry>> RetrieveEventEntriesPendingToPublishAsync(Guid transactionId);

        Task SaveEventAsync(IntegrationEvent @event);
    }
}